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
    internal sealed class PluginListCommand : AsyncCommand<PluginListCommandSettings>
    {
        private readonly IPluginService _pluginservice;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginListCommand"/> class.
        /// </summary>
        /// <param name="pluginservice">Service used to retrieve available plugins.</param>
        public PluginListCommand(IPluginService pluginservice)
        {
            _pluginservice = pluginservice;
        }

        /// <summary>
        /// Lists available plugins according to the user-defined options and displays them in a formatted table.
        /// </summary>
        /// <param name="context">The current command context.</param>
        /// <param name="settings">The settings parsed from the command-line arguments.</param>
        /// <returns>A task representing the asynchronous execution, returning 0 on success.</returns>
        public override Task<int> ExecuteAsync(CommandContext context, PluginListCommandSettings settings)
        {
            var plugins = new List<PluginInfo>();

            if (settings.ShowDrawers)
                plugins.AddRange(_pluginservice.GetPlugins<IDocumentDrawer>());

            if (settings.ShowPackagers)
                plugins.AddRange(_pluginservice.GetPlugins<IPackager>());

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
                    new Text(plugin.PluginType, new Style(Color.White, Color.Black)),
                    new Text(plugin.Name, new Style(Color.White, Color.Black)),
                    new Text(plugin.Description, new Style(Color.White, Color.Black))
                );
            }

            AnsiConsole.Write(table);

            return Task.FromResult(0);
        }
    }

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
            var plugin = _pluginservice.GetPlugins<IDocumentDrawer>()
                .Union(_pluginservice.GetPlugins<IPackager>())
                .SingleOrDefault(p => p.Name.Equals(settings.PluginName, StringComparison.InvariantCultureIgnoreCase));

            if (plugin is not null)
            {
                var table = new Table();
                table.Border(TableBorder.Rounded);
                table.LeftAligned();
                table.ShowRowSeparators();
                table.HideHeaders();
                table.AddColumn(new TableColumn("Key"));
                table.AddColumn(new TableColumn("Value"));

                table.AddRow(new Markup("[blue]Name[/]"), new Text(plugin.Name));
                table.AddRow(new Markup("[blue]Type[/]"), new Text(plugin.PluginType));
                table.AddRow(new Markup("[blue]Description[/]"), new Text(plugin.Description));
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
