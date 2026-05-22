using Spectre.Console.Cli;
using System.ComponentModel;

namespace OmniGenerator.Cli.Commands
{
    /// <summary>
    /// Represents the command-line settings for the <see cref="InfiniteCommand"/>.
    /// </summary>
    internal sealed class InfiniteCommandSettings : CommandSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="InfiniteCommand"/> should be cancellable.
        /// If true and cancellation is requested, the command will terminate early with a non-zero exit code.
        /// </summary>
        [Description("Indicates if the InfiniteCommand is cancellable")]
        [CommandOption("-c|--cancellable")]
        public bool IsCancellable { get; set; }
    }
}
