---
title: Configuration
---

# Configuration

A generation run is fully described by a single JSON configuration file passed to `generate one` (or referenced from a schedule plan in `generate many`). A JSON Schema is provided at `Assets/SampleParamFiles/omnigenerator-schema.json` and can be referenced from your own files via `"$schema"`.

```json
{
  "$schema": "../omnigenerator-schema.json",
  "packager": "packager.omni.csv",
  "render-resolution": 300,
  "hierarchy": {
    "fields": [ /* root-level fields */ ],
    "root": { /* root group */ }
  }
}
```

## Top-level configuration

The top-level object has three properties:

| Property | Required | Description |
|----------|----------|-------------|
| `packager` | yes | Name of the packager plugin to use, e.g. `packager.omni.csv`. |
| `render-resolution` | no | Image resolution in DPI (dots per inch). Defaults to `240`. |
| `hierarchy` | yes | The tree of elements and fields to generate. |

### `hierarchy`

| Property | Required | Description |
|----------|----------|-------------|
| `fields` | no | Root-level fields (generated once per run). |
| `root` | yes | A `group` element that acts as the top of the tree. |
| `field-configuration-file` | no | Path to an external file containing additional root-level fields. Its fields are appended to `fields`. |

---

## Elements

The hierarchy is a tree of **elements**. Two element types exist, distinguished by the JSON `$type` property:

- `group` — a container for other elements (groups or documents).
- `document` — a leaf that may be rendered as an image.

All elements share the following properties:

| Property | Required | Description |
|----------|----------|-------------|
| `$type` | yes | `"group"` or `"document"`. |
| `name` | yes | Logical name of the element. It also acts as its "type": every element named `cheque` is treated as the same kind of element. |
| `min-occurs` | yes | Minimum number of instances generated under each parent. |
| `max-occurs` | yes | Maximum number of instances generated under each parent. The actual count is randomly chosen between `min-occurs` and `max-occurs` (inclusive). |
| `fields` | no | Fields attached to each instance of this element. |
| `field-configuration-file` | no | Path to an external file containing additional fields. Useful to share field definitions across several configurations. |

### Group

A `group` adds:

| Property | Description |
|----------|-------------|
| `elements` | The child elements (groups or documents) contained in this group. |

Groups have no image of their own. They are typically used to model a logical container (for instance, a remittance containing payments, themselves containing cheques).

### Document

A `document` adds:

| Property | Description |
|----------|-------------|
| `renderer` | Optional. The name of the renderer used to draw the document's front and back images. If omitted, no image is produced for this document. |

A document is a leaf: it cannot contain other elements.

### Example: nested hierarchy

```json
"root": {
  "$type": "group",
  "name": "remittance",
  "min-occurs": 1,
  "max-occurs": 1,
  "elements": [
    {
      "$type": "group",
      "name": "payment",
      "min-occurs": 1,
      "max-occurs": 1,
      "elements": [
        {
          "$type": "document",
          "renderer": "renderer.omni.payment-slip",
          "name": "talon-optique",
          "min-occurs": 1,
          "max-occurs": 1
        },
        {
          "$type": "document",
          "renderer": "renderer.omni.cheque",
          "name": "cheque",
          "min-occurs": 1,
          "max-occurs": 5
        }
      ]
    }
  ]
}
```

---

## Fields

Every field shares two common properties:

| Property | Required | Description |
|----------|----------|-------------|
| `$type` | yes | Type of generator: one of `constant`, `regex`, `numeric`, `date`, `increment`, `list`, `composite`, `key`, `aggregate`. |
| `name` | yes | Field name. Must be unique inside its parent element. |

The other properties depend on the field type. See [Generators]({{ '/Generators' | relative_url }}) for the complete list and behaviour.

### External field files

Both the hierarchy and any element can reference an external file via `field-configuration-file`. Such a file has the following structure:

```json
{
  "$schema": "../../omnigenerator-schema.json",
  "fields": [
    { "$type": "constant", "name": "encline", "value": "03" },
    { "$type": "numeric",  "name": "amount",  "min": 100, "max": 99999 }
  ]
}
```

When the configuration is loaded, the fields from the external file are appended to the parent element's `fields` list.

---

## Renderer

A renderer is selected per document via the `renderer` property of a `document` element. The value must match the name of an installed renderer plugin. Use `plugin list --renderers` to display the available renderers.

Built-in renderers include:

| Plugin name | Description |
|-------------|-------------|
| `renderer.omni.cheque` | French cheque (front with CMC7 / RLMC / amount, back with deposit account). |
| `renderer.omni.payment-slip` | TIP SEPA optical slip. |
| `renderer.omni.deposit-slip` | Generic cheque deposit slip ("bordereau de remise"). |
| `renderer.omni.batch-ticket` | Batch ticket. |

Each renderer expects a specific set of fields on the document it draws. Use `plugin details <renderer-name>` to list them.

---

## Packager

A packager is selected at the **top level** via the `packager` property of the configuration. Its name must match an installed packager plugin (`plugin list --packagers`).

Built-in packagers include:

| Plugin name | Description |
|-------------|-------------|
| `packager.omni.csv` | One CSV per document type and per group type, plus a JPEG and a TIFF image per document. |
| `packager.omni.zip` | Same content as `packager.omni.csv`, packaged inside a single ZIP archive. |
| `packager.omni.imageonly` | Only the documents' SVG, JPEG and TIFF images (no CSV / metadata). |
| `packager.omni.sql` | A `.sql` script where groups act as tables and documents as rows (`INSERT` / `UNION SELECT` statements). |
| `packager.tessi.lotpakjpk` | Tessi LOT + PAK + JPK triplet (metadata text file, black & white TIFF stream, grayscale JPEG stream). |
| `packager.tessi.eligibility` | JSON eligibility request for Wecheck Compliance. |

A packager may also expect specific fields on the root, on some groups or on some documents. Use `plugin details <packager-name>` to display the complete list, grouped by element.
