using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;

namespace OmniGenerator.Cli.Commands
{
    /// <summary>
    /// A command used for testing purposes that simulates a long-running, cancellable operation.
    /// Logs periodic messages and optionally stops if cancellation is requested.
    /// </summary>
    internal sealed class InfiniteCommand : CancellableAsyncCommand<InfiniteCommandSettings>
    {
        private readonly ILogger<InfiniteCommand> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="InfiniteCommand"/> class.
        /// </summary>
        /// <param name="logger">Logger instance for this command.</param>
        /// <param name="baselogger">Logger passed to the base command class.</param>
        public InfiniteCommand(
            ILogger<InfiniteCommand> logger,
            ILogger<CancellableAsyncCommand> baselogger)
            : base(baselogger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Executes the infinite command asynchronously.
        /// This command logs "I'm alive!" every 2 seconds until cancelled.
        /// If <see cref="InfiniteCommandSettings.IsCancellable"/> is true, the command exits with code -1 upon cancellation.
        /// </summary>
        /// <param name="context">The command context.</param>
        /// <param name="settings">The command settings provided via CLI.</param>
        /// <param name="cancellation">A token that indicates if the operation should be cancelled.</param>
        /// <returns>A task representing the result of the execution: -1 if cancelled and cancellable, otherwise it never returns.</returns>
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

                _logger.LogInformation("I'm alive!");
                Thread.Sleep(2000);
            }
        }
    }
}
