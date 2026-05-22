# OmniGenerator

**OmniGenerator** is a cross-platform .NET CLI tool that generates files in arbitrary formats (text, CSV, SQL, images, banking files, archives, …) filled with smart, plausibly-random data.

It was created to help test systems or sub-systems that import external files — in both **functional testing** and **load testing** scenarios — when realistic but synthetic data is needed at scale.

## How it works

A generation run is driven by a single JSON **configuration file** that describes:

1. A hierarchy of **elements** (`Root` → `Group`s → `Document`s).
2. A set of **fields** attached to each element, produced by a **field generator** (`constant`, `regex`, `numeric`, `date`, `increment`, `list`, `composite`, `key`, `aggregate`).
3. A **renderer** plugin, used to draw an image of each `Document` (recto / verso SVG, later rasterised to JPEG / TIFF).
4. A **packager** plugin, used to write everything to disk in a specific output format (CSV, ZIP, SQL, banking lots, JSON, …).

```
┌──────────────┐   ┌──────────────────┐   ┌───────────┐   ┌──────────┐
│ config.json  │ → │ Hierarchy build  │ → │ Renderers │ → │ Packager │ → output
└──────────────┘   │ (fields + tree)  │   │ (SVG)     │   │ (files)  │
                   └──────────────────┘   └───────────┘   └──────────┘
```

Both renderers and packagers are loaded as **plugins** from the `plugins/` folder next to the executable, which makes the tool fully extensible without recompiling the core.

## Quick start

```powershell
# List installed plugins
OmniGenerator.Cli plugin list

# Run a single generation
OmniGenerator.Cli generate one .\Assets\SampleParamFiles\param-remettant.json .\Output\remettants

# Schedule recurring generation jobs (Quartz cron expressions)
OmniGenerator.Cli generate many .\Assets\SampleParamFiles\schedule-plan-sample.json
```

Sample configuration files are available under [`Assets/SampleParamFiles/`](Assets/SampleParamFiles).

## Documentation

The full documentation is published as a website via [GitHub Pages](https://pages.github.com/):

➡️ **<https://poilaupat.github.io/OmniGenerator/>**

The same sources are also available in the [`docs/`](docs) folder of the repository.

| Page | Topic |
|------|-------|
| [Home](https://poilaupat.github.io/OmniGenerator/) | Overview |
| [CLI](https://poilaupat.github.io/OmniGenerator/CLI) | Command-line interface reference |
| [Configuration](https://poilaupat.github.io/OmniGenerator/Configuration) | Configuration files: root, groups, documents, fields, renderer, packager |
| [Generators](https://poilaupat.github.io/OmniGenerator/Generators) | Built-in field generator types |
| [Liquid Filters](https://poilaupat.github.io/OmniGenerator/Liquid-Filters) | Filters available inside composite fields |
| [Plugin Development](https://poilaupat.github.io/OmniGenerator/Plugin-Development) | Writing your own renderers and packagers |

## Repository layout

| Project | Purpose |
|---------|---------|
| `OmniGenerator.Cli` | CLI entry point (Spectre.Console.Cli + Autofac + Serilog + Quartz). |
| `OmniGenerator.Lib` | Core library: configuration, hierarchy, generators, orchestration, plugin loader. |
| `OmniGenerator.Plugins` | Built-in renderers and packagers (cheque, deposit slip, TIP SEPA, CSV, ZIP, SQL, image-only, …). |
| `OmniGenerator.Plugins.Tessi` | Vendor-specific plugins (LOT/PAK/JPK packager, Compliance eligibility packager). |
| `Assets/SampleParamFiles` | Ready-to-run sample configurations and field files. |

## License

See [LICENSE](LICENSE).
