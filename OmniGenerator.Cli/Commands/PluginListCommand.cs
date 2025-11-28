using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using OmniGenerator.Lib.Interfaces.Infrastructure;
using Spectre.Console;
using Spectre.Console.Cli;

namespace OmniGenerator.Cli.Commands
{
    /// <summary>
    /// Represents a CLI command that lists available plugins (drawers and/or packagers)
    /// using a formatted table output in the console.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="PluginListCommand"/> class.
    /// </remarks>
    /// <param name="pluginservice">Service used to retrieve available plugins.</param>
    internal sealed class PluginListCommand(IPluginService pluginservice) : AsyncCommand<PluginListCommandSettings>
    {

        /// <summary>
        /// Lists available plugins according to the user-defined options and displays them in a formatted table.
        /// </summary>
        /// <param name="context">The current command context.</param>
        /// <param name="settings">The settings parsed from the command-line arguments.</param>
        /// <returns>A task representing the asynchronous execution, returning 0 on success.</returns>
        public override Task<int> ExecuteAsync(CommandContext context, PluginListCommandSettings settings, CancellationToken ct)
        {
            var plugins = new List<PluginInfo>();

            if (settings.ShowDrawers)
                plugins.AddRange(pluginservice.GetPluginsInfo<IDocumentDrawer>());

            if (settings.ShowPackagers)
                plugins.AddRange(pluginservice.GetPluginsInfo<IPackager>());

            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.LeftAligned();
            table.ShowRowSeparators();
            table.AddColumn(new TableColumn("[blue]Type[/]"));
            table.AddColumn(new TableColumn("[blue]Name[/]"));
            table.AddColumn(new TableColumn("[blue]Description[/]"));

            foreach (var plugin in plugins)
            {
                table.AddRow(
                    new Text(plugin.ParentType.ToString(), new Style(Color.White, Color.Black)),
                    new Text(plugin.PluginName, new Style(Color.White, Color.Black)),
                    new Text(plugin.PluginDescription, new Style(Color.White, Color.Black))
                );
            }

            AnsiConsole.Write(table);

            return Task.FromResult(0);
        }
    }


}
