using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Interfaces.Infrastructure;
using OmniGenerator.Lib.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace OmniGenerator.Cli.Commands
{
    /// <summary>
    /// Represents a CLI command that lists fields used by plugins.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="PluginFieldCommand"/> class.
    /// </remarks>
    /// <param name="pluginService">Service used to retrieve available plugins.</param>
    internal sealed class PluginFieldCommand(IPluginService pluginService) : AsyncCommand<PluginFieldCommandSettings>
    {
        /// <summary>
        /// Lists fields used by a specific plugin.
        /// </summary>
        /// <param name="context">The current command context.</param>
        /// <param name="settings">The settings parsed from the command-line arguments.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous execution, returning 0 on success.</returns>
        public override Task<int> ExecuteAsync(CommandContext context, PluginFieldCommandSettings settings, CancellationToken ct)
        {
            var analyzer = new PluginFieldAnalyzer(pluginService);

            var plugin = pluginService.GetAllPluginsInfo()
                .FirstOrDefault(p => p.PluginName.Equals(settings.PluginName, StringComparison.InvariantCultureIgnoreCase));

            if (plugin == null)
            {
                AnsiConsole.MarkupInterpolated($"[red]The plugin <[italic]{settings.PluginName}[/]> is not installed.[/]");
                return Task.FromResult(1);
            }

            var fields = analyzer.GetFieldsForPlugin(settings.PluginName);

            if (fields == null || !fields.Any())
            {
                AnsiConsole.MarkupInterpolated($"[yellow]No field information available for plugin <[italic]{settings.PluginName}[/]>.[/]");
                return Task.FromResult(0);
            }

            DisplayPluginFields(plugin, fields);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Displays field information for a plugin in a formatted table.
        /// </summary>
        private void DisplayPluginFields(PluginInfo plugin, IEnumerable<PluginFieldInfo> fields)
        {
            AnsiConsole.MarkupLineInterpolated($"[bold blue]{plugin.PluginName}[/] [dim]({plugin.ParentType})[/]");

            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.LeftAligned();
            table.ShowRowSeparators();

            // Add columns based on plugin type
            table.AddColumn(new TableColumn("[blue]Field Name[/]"));
            table.AddColumn(new TableColumn("[blue]Description[/]"));
            table.AddColumn(new TableColumn("[blue]Required[/]"));
            table.AddColumn(new TableColumn("[blue]Default Value[/]"));

            if (plugin.ParentType == EPluginParentType.Packager)
            {
                table.AddColumn(new TableColumn("[blue]Entity Name[/]"));
                table.AddColumn(new TableColumn("[blue]Entity Type[/]"));
            }

            // Add rows
            foreach (var field in fields.OrderBy(f => f.FieldName))
            {
                if (plugin.ParentType == EPluginParentType.Packager)
                {
                    table.AddRow(
                        new Text(field.FieldName, new Style(Color.White, Color.Black)),
                        new Text(field.Description, new Style(Color.Grey, Color.Black)),
                        new Markup(field.IsRequired ? "[green]Yes[/]" : "[grey]No[/]"),
                        new Text(field.DefaultValue ?? "-", new Style(Color.Grey, Color.Black)),
                        new Text(field.EntityName ?? "-", new Style(Color.Grey, Color.Black)),
                        new Text(field.EntityType?.ToString() ?? "-", new Style(Color.Grey, Color.Black))
                    );
                }
                else
                {
                    table.AddRow(
                        new Text(field.FieldName, new Style(Color.White, Color.Black)),
                        new Text(field.Description, new Style(Color.Grey, Color.Black)),
                        new Markup(field.IsRequired ? "[green]Yes[/]" : "[grey]No[/]"),
                        new Text(field.DefaultValue ?? "-", new Style(Color.Grey, Color.Black))
                    );
                }
            }

            AnsiConsole.Write(table);
        }
    }
}
