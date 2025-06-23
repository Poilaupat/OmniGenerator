using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Interfaces.Infrastructure;
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
    /// including name, type, description, version, and path.
    /// </summary>
    internal sealed class PluginDetailCommand : AsyncCommand<PluginDetailCommandSettings>
    {
        private readonly IPluginService _pluginservice;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginDetailCommand"/> class.
        /// </summary>
        /// <param name="pluginservice">Service used to query plugin metadata.</param>
        public PluginDetailCommand(IPluginService pluginservice)
        {
            _pluginservice = pluginservice;
        }

        /// <summary>
        /// Displays detailed information about the specified plugin in a key-value table format.
        /// </summary>
        /// <param name="context">The current command context.</param>
        /// <param name="settings">The settings containing the name of the plugin to inspect.</param>
        /// <returns>A task representing the asynchronous execution, returning 0 on success.</returns>
        public override Task<int> ExecuteAsync(CommandContext context, PluginDetailCommandSettings settings)
        {
            var plugin = _pluginservice.GetPluginsInfo<IDocumentDrawer>()
                .Union(_pluginservice.GetPluginsInfo<IPackager>())
                .SingleOrDefault(p => p.PluginName.Equals(settings.PluginName, StringComparison.InvariantCultureIgnoreCase));

            if (plugin is not null)
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
            else
            {
                AnsiConsole.MarkupInterpolated($"[red]The plugin <[italic]{settings.PluginName}[/]> is not installed.[/]");
            }

            return Task.FromResult(0);
        }
    }
}
