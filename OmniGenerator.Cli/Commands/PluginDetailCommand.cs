using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Cli.Commands
{
    /// <summary>
    /// Represents a CLI command that shows detailed information about a specific plugin,
    /// including name, type, description, version, path, and fields documentation.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="PluginDetailCommand"/> class.
    /// </remarks>
    /// <param name="pluginservice">Service used to query plugin metadata.</param>
    internal sealed class PluginDetailCommand(IPluginService pluginservice) : AsyncCommand<PluginDetailCommandSettings>
    {

        /// <summary>
        /// Displays detailed information about the specified plugin in a key-value table format,
        /// followed by a table of fields used by the plugin.
        /// </summary>
        /// <param name="context">The current command context.</param>
        /// <param name="settings">The settings containing the name of the plugin to inspect.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task representing the asynchronous execution, returning 0 on success.</returns>
        public override Task<int> ExecuteAsync(CommandContext context, PluginDetailCommandSettings settings, CancellationToken ct)
        {
            var plugin = pluginservice.GetAllPluginsInfo()
                .SingleOrDefault(p => p.PluginName.Equals(settings.PluginName, StringComparison.InvariantCultureIgnoreCase));

            if (plugin is null)
            {
                AnsiConsole.MarkupInterpolated($"[red]The plugin <[italic]{settings.PluginName}[/]> is not installed.[/]");
                return Task.FromResult(1);
            }

            // Display plugin details
            DisplayPluginDetails(plugin);

            // Display plugin fields
            try
            {
                var instance = Activator.CreateInstance(plugin.PluginType);
                if (instance is not IOmniGeneratorPlugin omniplugin)
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupInterpolated($"[yellow]Plugin does not provide field documentation.[/]");
                    return Task.FromResult(0);
                }

                var fields = omniplugin.GetFieldsDocumentation()
                    .OrderBy(p => p.EntityType)
                    .ThenBy(p => p.EntityName)
                    .ThenBy(p => p.FieldName)
                    .ToList();

                if (!fields.Any())
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupInterpolated($"[yellow]No field information available for this plugin.[/]");
                    return Task.FromResult(0);
                }

                AnsiConsole.WriteLine();
                DisplayPluginFields(plugin, fields);
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupInterpolated($"[red]Error loading field documentation: {ex.Message}[/]");
                return Task.FromResult(1);
            }
        }

        /// <summary>
        /// Displays plugin details in a key-value table format.
        /// </summary>
        /// <param name="plugin">The plugin information to display.</param>
        private void DisplayPluginDetails(PluginInfo plugin)
        {
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.LeftAligned();
            table.ShowRowSeparators();
            table.HideHeaders();
            table.AddColumn(new TableColumn("Key"));
            table.AddColumn(new TableColumn("Value"));

            table.AddRow(new Markup("[blue]Name[/]"), new Text(plugin.PluginName));
            table.AddRow(new Markup("[blue]Type[/]"), new Text(plugin.ParentType.ToString()));
            table.AddRow(new Markup("[blue]Description[/]"), new Text(plugin.PluginDescription));
            table.AddRow(new Markup("[blue]Version[/]"), new Text(plugin.AssemblyVersion));
            table.AddRow(new Markup("[blue]Path[/]"), new TextPath(plugin.Location));

            AnsiConsole.Write(table);
        }

        /// <summary>
        /// Displays field information for a plugin in a formatted table.
        /// </summary>
        /// <param name="plugin">The plugin information.</param>
        /// <param name="fields">The fields to display.</param>
        private void DisplayPluginFields(PluginInfo plugin, IEnumerable<FieldInfo> fields)
        {
            AnsiConsole.MarkupLineInterpolated($"[bold blue]Fields[/]");

            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.LeftAligned();
            table.ShowRowSeparators();

            if (plugin.ParentType == EPluginParentType.Packager)
            {
                table.AddColumn(new TableColumn("[blue]Entity Type[/]"));
                table.AddColumn(new TableColumn("[blue]Entity Name[/]"));
            }

            table.AddColumn(new TableColumn("[blue]Field Name[/]"));
            table.AddColumn(new TableColumn("[blue]Description[/]"));
            table.AddColumn(new TableColumn("[blue]Required[/]"));
            table.AddColumn(new TableColumn("[blue]Default Value[/]"));

            foreach (var field in fields)
            {
                if (plugin.ParentType == EPluginParentType.Packager)
                {
                    table.AddRow(
                        new Text(field.EntityType?.ToString() ?? "-", new Style(Color.Grey, Color.Black)),
                        new Text(field.EntityName ?? "-", new Style(Color.Grey, Color.Black)),
                        new Text(field.FieldName, new Style(Color.White, Color.Black)),
                        new Text(field.Description, new Style(Color.Grey, Color.Black)),
                        new Markup(field.IsRequired ? "[green]Yes[/]" : "[grey]No[/]"),
                        new Text(field.DefaultValue ?? "-", new Style(Color.Grey, Color.Black))
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
