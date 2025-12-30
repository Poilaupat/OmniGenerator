using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Cli.Commands
{
    /// <summary>
    /// Command settings for listing fields used by a specific plugin.
    /// </summary>
    internal sealed class PluginFieldCommandSettings : PluginCommandSettingsBase
    {
        /// <summary>
        /// Gets or sets the name of the plugin to display fields for.
        /// </summary>
        [CommandArgument(0, "<PLUGIN_NAME>")]
        public string PluginName { get; set; } = string.Empty;
    }
}
