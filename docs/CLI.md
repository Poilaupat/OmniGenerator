---
title: Command-Line Interface
---

# Command-Line Interface

OmniGenerator is run through the `OmniGenerator.Cli` executable. Every command supports `-h` / `--help` to display its usage.

```text
OmniGenerator.Cli <command> [arguments] [options]
```

## Global behaviour

- **Cancellation** — pressing `Ctrl+C` once asks the running command to stop gracefully. Pressing it a second time terminates the process immediately (exit code `2`).
- **Settings** — the file `appsettings.json` placed next to the executable controls logging and a few display settings (for instance `AppSettings:progress-resolution`, the refresh interval of the live progress display, in milliseconds).
- **Logs** — by default, log messages are written to the console and to `../Logs/OmniGeneratorCli.log`.

---

## `about`

Displays the application name, version and copyright.

```powershell
OmniGenerator.Cli about
```

---

## `generate one`

Executes a single generation run from a configuration file. The console shows a live three-step progress display (Data generation → Vector images → Package).

```text
generate one <SettingsFilePath> <OutputFolder>
```

| Argument | Description |
|----------|-------------|
| `SettingsFilePath` | Path to the OmniGenerator configuration JSON file (see [Configuration]({{ '/Configuration' | relative_url }})). |
| `OutputFolder` | Directory where the packager will write its output. Created if it does not exist. |

**Examples**

```powershell
OmniGenerator.Cli generate one config.json C:\Output
OmniGenerator.Cli generate one .\Assets\SampleParamFiles\param-remettant.json .\Output\remettants
```

**Exit codes**

| Code | Meaning |
|------|---------|
| `0`  | Success |
| `-1` | An error occurred during generation (also logged) |
| `2`  | Hard cancellation requested by the user (double `Ctrl+C`) |

---

## `generate many`

Schedules one or more generation jobs and runs them repeatedly according to a cron expression. Each job is described by a **schedule plan** JSON file. The console shows a live table with the state of each job (running / idle, next fire time, document throughput, errors).

```text
generate many <SchedulePlanFile>
```

The schedule plan file has the following shape:

```json
{
  "jobs": [
    {
      "name": "remettants",
      "settingsFilePath": "Assets/SampleParamFiles/param-remettant.json",
      "outputFolderPath": "Output/remettants",
      "cronSchedule": "0/30 * * ? * *"
    },
    {
      "name": "lotpakjpk",
      "settingsFilePath": "Assets/SampleParamFiles/param-lotpakjpk.json",
      "outputFolderPath": "Output/lotpakjpk",
      "cronSchedule": "0/15 * * ? * *"
    }
  ]
}
```

| Field | Description |
|-------|-------------|
| `name` | Unique job identifier. |
| `settingsFilePath` | Path to the OmniGenerator configuration JSON for this job. |
| `outputFolderPath` | Output directory (created if missing). |
| `cronSchedule` | Cron expression describing when the job must run. See the [Quartz cron format reference](https://www.quartz-scheduler.net/documentation/quartz-3.x/tutorial/crontrigger.html). |

A sample is available at `Assets/SampleParamFiles/schedule-plan-sample.json`.

Run with:

```powershell
OmniGenerator.Cli generate many schedule-plan.json
```

The command keeps running until cancelled.

---

## `plugin list`

Lists installed plugins discovered in the `plugins/` directory.

```text
plugin list [-r|--renderers] [-p|--packagers]
```

If neither flag is provided, **both** renderers and packagers are listed.

**Examples**

```powershell
OmniGenerator.Cli plugin list
OmniGenerator.Cli plugin list --packagers
OmniGenerator.Cli plugin list --renderers
```

---

## `plugin details`

Displays the description and the list of fields used by a specific plugin (identified by its name, e.g. `renderer.omni.cheque`).

```text
plugin details <PLUGIN_NAME>
```

**Example**

```powershell
OmniGenerator.Cli plugin details renderer.omni.cheque
OmniGenerator.Cli plugin details packager.omni.csv
```

The output lists every field the plugin expects, with its description, whether it is required, and any default value. This is the easiest way to discover which fields you must declare in your configuration for a given renderer or packager.
