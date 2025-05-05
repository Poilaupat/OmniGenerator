using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Cli.Commands
{
    internal sealed class InfiniteCommandSettings : CommandSettings
    {
        [Description("Indicates if the InfiniteCommand is cancellable")]
        [CommandOption("-c|--cancellable")]
        public bool IsCancellable { get; set; }
    }
}
