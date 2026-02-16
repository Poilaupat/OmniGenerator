using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Cli.Commands
{
    internal class GenerateManyCommand : AsyncCommand<GenerateManyCommandSettings>
    {
        public override Task<int> ExecuteAsync(CommandContext context, GenerateManyCommandSettings settings, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
