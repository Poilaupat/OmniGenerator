---
title: Plugin development
---

# Plugin development

OmniGenerator exposes two extension points implemented as plugins:

- **Renderers** — `IDocumentRenderer`, produce the SVG image (recto / verso) of a `Document`.
- **Packagers** — `IPackager`, write the generated hierarchy to disk in any output format.

Both are discovered at startup by `PluginService` from the `plugins/` directory that sits next to the executable.

---

## Plugin discovery

`PluginService` scans `AppContext.BaseDirectory/plugins/` and, for every subdirectory `plugins/MyPlugin/`, tries to load `plugins/MyPlugin/MyPlugin.dll`. The folder name and the DLL name **must match**. Loading is delegated to [`McMaster.NETCore.Plugins`](https://github.com/natemcmaster/DotNetCorePlugins) so each plugin runs with its own dependencies, sharing only the types `IPackager` and `IDocumentRenderer` with the host.

Inside the assembly, every public, non-abstract type deriving from `OmniGeneratorPluginBase` and carrying the `[OmniGeneratorPluginMetadata]` attribute is registered.

```text
<output>/
└── plugins/
    └── MyCompany.Renderers/
        ├── MyCompany.Renderers.dll
        └── MyCompany.Renderers.deps.json
```

---

## Common plugin contract

Every plugin must:

1. Derive (directly or indirectly) from `OmniGeneratorPluginBase`.
2. Implement either `IDocumentRenderer` or `IPackager`.
3. Be decorated with `[OmniGeneratorPluginMetadata("plugin.name", "human readable description")]`.
4. Document the fields it consumes via a **fields class** — a `FieldExtractorBase`-derived class located in the **same namespace** as the plugin. Its name does not matter; any number of fields classes can coexist.

The plugin name is the public identifier used in configuration files (e.g. `"packager": "packager.omni.csv"`). Conventional naming is `<kind>.<vendor>.<plugin>`.

### Fields documentation

Each field exposed to configuration must be declared as a property of the fields class with `[FieldInfo]`. The optional `[FieldEntity]` on the class indicates to which hierarchy entity the field belongs (used by `plugin details` to group fields by `Root` / `Group` / `Document`).

```csharp
[FieldEntity(EPluginFieldEntityType.Document, "cheque")]
public sealed class ChequeFields : FieldExtractorBase
{
    public ChequeFields(FieldCollection fields) : base(fields) { }

    [FieldInfo("dataread", "CMC7 line", isRequired: true)]
    public string Dataread => GetRequiredString("dataread");

    [FieldInfo("bank-name", "Bank name",
               isRequired: false, DefaultValue = "OmniBank")]
    public string BankName => GetOptionalStringOrDefault("bank-name", "OmniBank");

    [FieldInfo("amount", "Cheque amount", isRequired: true)]
    public Field Amount => GetRequiredField("amount");
}
```

`FieldExtractorBase` provides the helpers used above:

| Helper | Purpose |
|--------|---------|
| `GetRequiredString(name)` | String value — throws `FieldNotFoundException` if missing. |
| `GetOptionalString(name)` | String value or `null`. |
| `GetOptionalStringOrDefault(name, default)` | String value or a default. |
| `GetRequiredField(name)` | Raw `Field` — throws if missing. Use it when you need the typed `Value` (e.g. `DateTime`, `int`). |
| `GetOptionalField(name)` | Raw `Field` or `null`. |

The CLI command `plugin details <name>` reflects on these `[FieldInfo]` attributes; documenting fields properly is therefore not only good practice but also part of the user-facing experience.

---

## Field Channels and Error Simulation

Every `Field` carries three independent values to support error simulation:

```text
Generation         Error Simulation              Plugin Consumption
──────────         ────────────────              ──────────────────
┌─────────┐        ┌──────────────┐             ┌──────────────┐
│  Value  │───────>│  DataValue   │────────────>│  Packager    │
│         │        │              │  (Data)     │  reads Data  │
│ (truth) │        └──────────────┘             └──────────────┘
│         │              ▲
└─────────┘              │ Misread / Substitution
                         │
                    ┌──────────────┐             ┌──────────────┐
                    │  ImageValue  │────────────>│  Renderer    │
                    │              │  (Image)    │  reads Image │
                    └──────────────┘             └──────────────┘
                         ▲
                         │ Inconsistency
```

- **`Value`** — the original generated value (source of truth, never mutated).
- **`DataValue`** — read by packagers via `FieldChannel.Data`, mutated by `Misread` / `Substitution` errors.
- **`ImageValue`** — read by renderers via `FieldChannel.Image`, mutated by `Inconsistency` errors.

At generation time, all three are equal. The `ErrorSimulator` runs **after hierarchy generation** and **before rendering/packaging**, mutating only the relevant channel(s) according to configured rules.

### Choosing the right channel

When constructing a `FieldExtractorBase`, pass the appropriate `FieldChannel`:

```csharp
protected FieldExtractorBase(FieldCollection fields, FieldChannel channel = FieldChannel.Data)
```

**Decision rules**:

| Use case | Channel | Rationale |
|----------|---------|-----------|
| **Packager** reading fields | `FieldChannel.Data` (default) | Packagers export metadata; they should see data-channel mutations. |
| **Renderer** displaying its **own** fields | `FieldChannel.Image` | The document renders what is visually on the page (potentially mutated by `Inconsistency`). |
| **Renderer** displaying **other documents' fields** | `FieldChannel.Data` | Reading the unmutated business value creates intentional inconsistency when the other document's image was mutated. |

### Example: Simulating cross-document inconsistency

Consider a deposit slip (bordereau) that lists cheque amounts. We want the cheque image to show "100.00 €" but the deposit slip to show "99.00 €" (a transcription error).

**Configuration** (enable `Inconsistency` on `cheque/amount`):

```json
{
  "hierarchy": {
    "error-simulations": {
      "enabled": true,
      "rules": [
        {
          "target-document": "cheque",
          "target-field": "amount",
          "type": "Inconsistency",
          "probability": 0.05
        }
      ]
    }
  }
}
```

**Implementation**:

```csharp
// ChequeFields.cs — used by the deposit slip renderer to display cheque amounts
[FieldEntity(EPluginFieldEntityType.Document, "cheque")]
internal class ChequeFields : FieldExtractorBase
{
    // Reading from the Data channel (even in a rendering context) allows
    // inconsistency simulation: the cheque image shows ImageValue (mutated),
    // but the deposit slip shows DataValue (unmutated business truth).
    public ChequeFields(FieldCollection fields) : base(fields, FieldChannel.Data)
    {
    }

    [FieldInfo("amount", "Cheque amount in cents", isRequired: true)]
    public int Amount => GetRequiredValue("amount").Convert<int>();
}

// RemittanceFields.cs — used by the deposit slip to show aggregate totals
[FieldEntity(EPluginFieldEntityType.Group, "remittance")]
internal class RemittanceFields : FieldExtractorBase
{
    // Same rationale: the deposit slip reads business data (Data channel),
    // not the mutated image values, to create cross-document inconsistency.
    public RemittanceFields(FieldCollection fields) : base(fields, FieldChannel.Data)
    {
    }

    public int? Amount => GetOptionalValue("total-amount")?.Convert<int>();
}

// DepositSlipFields.cs — the deposit slip's own fields
[FieldEntity(EPluginFieldEntityType.Document, "deposit-slip")]
public sealed class DepositSlipFields : FieldExtractorBase
{
    // The deposit slip displays its OWN fields: use Image channel
    // so mutations on the slip itself are rendered.
    public DepositSlipFields(FieldCollection fields) : base(fields, FieldChannel.Image)
    {
    }

    [FieldInfo("title", "Slip title", isRequired: false)]
    public string Title => GetOptionalStringOrDefault("title", "BORDEREAU DE REMISE");
}
```

**Result**: The cheque image shows the mutated `ImageValue` (e.g. "99.00 €"), while the deposit slip image shows the unmutated `DataValue` (e.g. "100.00 €"). The inconsistency is visible and traceable, since the canonical `Value` remains "100.00".

### Best practices

- **Always use `FieldChannel.Image` for a document's own fields** — this is the expected behaviour for visual documents.
- **Use `FieldChannel.Data` when reading from `document.Parent` or child elements** — this creates realistic cross-document inconsistencies.
- **Document your choice** with a comment if non-obvious (see examples above).
- **Test with error simulation enabled** to verify the intended inconsistency appears correctly.

---

## Writing a Renderer

`IDocumentRenderer` produces an SVG document per side. Most implementations should derive from `DocumentRendererBase`, which:

- Holds the document size in millimetres (`Width`, `Height`).
- Pre-registers the embedded fonts (`CMC7`, `OCRB`, `Caveat`) so they can be used in SVG.
- Provides a default blank implementation of `RenderRecto` and `RenderVerso`.

```csharp
[OmniGeneratorPluginMetadata("renderer.acme.invoice", "Renders an Acme invoice")]
public sealed class InvoiceRenderer : DocumentRendererBase
{
    public InvoiceRenderer() : base(width: 210, height: 297) { } // A4

    public override SvgDocument RenderRecto(Document document)
    {
        var fields = new InvoiceFields(document.Fields);
        var svg = SvgExtensions.NewBlankSvg(Width, Height);

        svg.DrawText(fields.Title, "title", 10f, 15f, "Arial", 8f, Color.Black);
        svg.DrawText($"{fields.Amount:C}", "amount",
                     Width - 10f, 30f, "Arial", 6f, Color.Black,
                     anchor: SvgTextAnchor.End);
        return svg;
    }

    // RenderVerso is optional — base returns a blank SVG.
}
```

A few useful helpers:

- `SvgExtensions.NewBlankSvg(width, height)` — creates an SVG with a millimetre-based viewbox.
- `SvgExtensions.DrawText(...)` / `DrawHandwrittenText(...)` — text helpers.
- `BarCodeRenderHelper`, `NumberToWordsRenderHelper` — for barcodes and amount-in-letters (LAR).

The host converts the returned SVG to raster (`SvgRenderer`) using the configured `render-resolution`, producing JPEG and Group 4 TIFF that the packagers consume.

### Selecting a renderer in a configuration

```json
{
  "$type": "document",
  "name": "invoice",
  "renderer": "renderer.acme.invoice",
  "min-occurs": 1,
  "max-occurs": 10,
  "fields": [ /* … */ ]
}
```

---

## Writing a Packager

`IPackager` has a single method:

```csharp
Task ProcessAsync(Root root, string basepath, int imageRenderingResolution);
```

| Parameter | Description |
|-----------|-------------|
| `root` | The fully-built hierarchy. Use `root.GetAllDocuments()` / `root.GetAllGroups()` (recursive) or `root.Groups` for top-level groups. |
| `basepath` | The output directory provided by the user (guaranteed to exist). Create a sub-folder if you produce multiple files. |
| `imageRenderingResolution` | DPI to use when rasterising each `Document.RectoVectorImage` / `VersoVectorImage` via `SvgRenderer`. |

Skeleton:

```csharp
[OmniGeneratorPluginMetadata("packager.acme.json", "Exports as JSON")]
public sealed class JsonPackager : OmniGeneratorPluginBase, IPackager
{
    public async Task ProcessAsync(Root root, string basepath, int imageRenderingResolution)
    {
        var rootFields = new RootFields(root.Fields);
        var outputFile = Path.Combine(basepath, $"export_{DateTime.Now:yyyyMMddHHmmss}.json");

        var payload = root
            .GetAllDocuments()
            .Select(d => d.Fields.ToDynamic())
            .ToArray();

        await File.WriteAllTextAsync(outputFile, JsonSerializer.Serialize(payload));

        // Rasterise images if you need them:
        foreach (var doc in root.GetAllDocuments())
        {
            if (doc.RectoVectorImage is null) continue;
            var bytes = new SvgRenderer(doc.RectoVectorImage, imageRenderingResolution).ToJpeg();
            await File.WriteAllBytesAsync(Path.Combine(basepath, $"{doc.Name}_R.jpg"), bytes);
        }
    }
}
```

### Declaring expected fields on different entities

If your packager needs fields on `Root`, on a specific `Group` or on a specific `Document`, declare one fields class per entity in the **same namespace** as the packager:

```csharp
[FieldEntity(EPluginFieldEntityType.Root)]
public sealed class RootFields : FieldExtractorBase
{
    public RootFields(FieldCollection fields) : base(fields) { }

    [FieldInfo("numlot", "Batch number", isRequired: true)]
    public Field Numlot => GetRequiredField("numlot");
}

[FieldEntity(EPluginFieldEntityType.Document, "cheque")]
public sealed class ChequeFields : FieldExtractorBase { /* … */ }
```

All these classes are picked up by `OmniGeneratorPluginBase.GetFieldsDocumentation()` and displayed by `plugin details`.

### Selecting a packager in a configuration

```json
{
  "packager": "packager.acme.json",
  "render-resolution": 240,
  "hierarchy": { /* … */ }
}
```

---

## Testing a plugin locally

1. Build your plugin project: `dotnet build -c Release`.
2. Copy the output (the DLL and its `*.deps.json` / dependent assemblies) into a folder named after the DLL, e.g. `plugins/MyCompany.Renderers/MyCompany.Renderers.dll`.
3. Drop that folder next to `OmniGenerator.Cli.exe` in `plugins/`.
4. Verify it is loaded: `OmniGenerator.Cli plugin list`.
5. Inspect the declared fields: `OmniGenerator.Cli plugin details mycompany.renderer.thing`.
6. Reference the plugin name from your configuration JSON and run `OmniGenerator.Cli generate one …`.

---

## Adding a custom Liquid filter

Composite fields use Liquid templates (see [Liquid Filters]({{ '/Liquid-Filters' | relative_url }})). Adding a filter requires modifying the core library rather than a plugin:

1. Add a `public static` method to `OmniGenerator.Lib/Liquid/LiquidCustomFilters.cs`.
2. The first parameter is the input value; the remaining parameters become Liquid filter arguments.
3. C# method names are automatically exposed as `snake_case` filter names (`PadLeft` → `pad_left`).

```csharp
public static string Truncate(string input, int length, string suffix = "...")
{
    if (input is null || input.Length <= length) return input;
    return input.Substring(0, length - suffix.Length) + suffix;
}
```

The filter is then available in any composite field:

{% raw %}
```liquid
{{description | truncate:50}}
{{description | truncate:50,'…'}}
```
{% endraw %}

---

## Guidelines

- Keep plugin names **stable** — they are referenced from user configuration files.
- Always document every consumed field with `[FieldInfo]`. Required-vs-optional and default values are shown to users by `plugin details`.
- Use `async`/`await` end-to-end (`Task ProcessAsync`); avoid sync-over-async patterns.
- Make plugins **side-effect free** beyond writing to the supplied `basepath`.
- Log via the standard logging abstractions; avoid `Console.WriteLine` since the host owns the live UI.
- For renderers, prefer SVG primitives (`SvgRectangle`, `SvgLine`, `SvgText`, …) over raster operations — the host handles rasterisation.
