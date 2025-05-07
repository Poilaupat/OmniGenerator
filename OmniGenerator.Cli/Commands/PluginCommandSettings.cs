using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Cli.Commands
{
    /// <summary>
    /// Base class for plugin-related command settings.
    /// Can be extended to define settings for specific plugin commands.
    /// </summary>
    internal class PluginCommandSettings : CommandSettings
    {

    }

    /// <summary>
    /// Command settings for listing available plugins.
    /// Allows filtering by type: packagers and/or drawers.
    /// </summary>
    internal sealed class PluginListCommandSettings : PluginCommandSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to display packager plugins.
        /// </summary>
        [CommandOption("-p|--packagers")]
        public bool? PackagersOption { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display document drawer plugins.
        /// </summary>
        [CommandOption("-d|--drawers")]
        public bool? DrawersOptions { get; set; }

        /// <summary>
        /// Determines whether to show packager plugins based on provided options.
        /// Defaults to true if neither option is explicitly specified.
        /// </summary>
        public bool ShowPackagers => (PackagersOption ?? false) || (PackagersOption is null && DrawersOptions is null);

        /// <summary>
        /// Determines whether to show drawer plugins based on provided options.
        /// Defaults to true if neither option is explicitly specified.
        /// </summary>
        public bool ShowDrawers => (DrawersOptions ?? false) || (PackagersOption is null && DrawersOptions is null);
    }

    /// <summary>
    /// Command settings for displaying detailed information about a specific plugin.
    /// </summary>
    internal sealed class PluginDetailCommandSettings : PluginCommandSettings
    {
        /// <summary>
        /// Gets or sets the name of the plugin to display details for.
        /// </summary>
        [CommandArgument(0, "<PLUGIN_NAME>")]
        public string PluginName { get; set; } = string.Empty;
    }
}
