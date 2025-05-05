using JasperFx.Core.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console.Cli;

namespace OmniGenerator.Cli.Commands
{
    internal sealed class InfiniteCommand : CancellableAsyncCommand<InfiniteCommandSettings>
    {
        private ILogger<InfiniteCommand> _logger;

        public InfiniteCommand(
            ILogger<InfiniteCommand> logger, 
            ILogger<CancellableAsyncCommand> baselogger)
            :base(baselogger)
        {
            _logger = logger;
        }

        public override Task<int> ExecuteAsync(CommandContext context, InfiniteCommandSettings settings, CancellationToken cancellation)
        {
            while (true)
            {
                if (cancellation.IsCancellationRequested)
                {
                    _logger.LogInformation("Cancellation requested");
                    if (settings.IsCancellable)
                    {
                        return Task.FromResult(-1);
                    }
                }

                _logger.LogInformation("I'm alive !");
                Thread.Sleep(2000);
            }
        }
    }
}
