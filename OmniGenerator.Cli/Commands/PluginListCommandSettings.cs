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
    /// Command settings for listing available plugins.
    /// Allows filtering by type: packagers and/or drawers.
    /// </summary>
    internal sealed class PluginListCommandSettings : PluginCommandSettingsBase
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
}
