using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Cli.Commands
{
    /// <summary>
    /// Base class for plugin-related command settings.
    /// Can be extended to define settings for specific plugin commands.
    /// </summary>
    internal abstract class PluginCommandSettingsBase : CommandSettings;
}
