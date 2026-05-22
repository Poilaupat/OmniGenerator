---
title: OmniGenerator
---

# OmniGenerator

**OmniGenerator** is a cross-platform command-line tool that generates files in many different formats (text, CSV, SQL, images, banking files, archives, …) filled with smart, plausibly-random data.

It is designed to help test systems that import or process external files — both in **functional testing** and **load testing** scenarios — when realistic but synthetic data is needed at scale.

## How it works

A generation run is driven by a single JSON **configuration file** that describes:

1. A tree of **elements**: a **root** that contains **groups**, which can contain other groups or **documents**.
2. A set of **fields** attached to each element. Each field is produced by a **generator** (constant value, random number, regular expression, date, list, computed value, …).
3. A **renderer** that draws the image of each document (front side / back side).
4. A **packager** that writes everything to disk in a chosen output format (CSV files, ZIP archive, SQL script, banking files, JSON payload, …).

For every run, OmniGenerator first builds the tree in memory, then asks the renderer to draw the documents, and finally hands the result over to the packager that produces the output files.

```text
┌──────────────┐   ┌──────────────────┐   ┌──────────┐   ┌──────────┐
│ config.json  │ → │  Data generation │ → │  Images  │ → │  Output  │
└──────────────┘   │   (tree + data)  │   │ (SVG)    │   │ (files)  │
                   └──────────────────┘   └──────────┘   └──────────┘
```

Renderers and packagers are loaded as **plugins** from the `plugins/` folder placed next to the executable, so new output formats and new document types can be added without modifying OmniGenerator itself.

## Documentation

| Page | Topic |
|------|-------|
| [CLI]({{ '/CLI' | relative_url }}) | Command-line interface reference |
| [Configuration]({{ '/Configuration' | relative_url }}) | Configuration files: root, groups, documents, fields, renderer, packager |
| [Generators]({{ '/Generators' | relative_url }}) | All the available field generators |
| [Liquid Filters]({{ '/Liquid-Filters' | relative_url }}) | Filters available inside composite fields |
| [Plugin Development]({{ '/Plugin-Development' | relative_url }}) | Developer guide for writing your own renderers and packagers |

## Repository layout

| Folder | Content |
|--------|---------|
| `OmniGenerator.Cli/` | The command-line executable. |
| `OmniGenerator.Lib/` | Core engine (configuration parsing, data generation, orchestration, plugin loader). |
| `OmniGenerator.Plugins/` | Built-in renderers and packagers (cheque, deposit slip, TIP SEPA, CSV, ZIP, SQL, image-only, …). |
| `OmniGenerator.Plugins.Tessi/` | Vendor-specific plugins (LOT/PAK/JPK, Compliance eligibility). |
| `Assets/SampleParamFiles/` | Ready-to-run sample configuration files. |

## License

See [LICENSE](https://github.com/Poilaupat/OmniGenerator/blob/main/LICENSE) in the repository root.
