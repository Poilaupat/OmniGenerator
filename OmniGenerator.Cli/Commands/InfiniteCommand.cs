using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace OmniGenerator.Cli.Commands
{
    /// <summary>
    /// A command used for testing purposes that simulates a long-running, cancellable operation.
    /// Logs periodic messages and optionally stops if cancellation is requested.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="InfiniteCommand"/> class.
    /// </remarks>
    /// <param name="logger">Logger instance for this command.</param>
    internal sealed class InfiniteCommand(ILogger<InfiniteCommand> logger) : AsyncCommand<InfiniteCommandSettings>
    {

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
                    if (settings.IsCancellable)
                    {
                        AnsiConsole.Console.MarkupLine("[green]Cancelling infinite command gracefully ![/]");
                        return Task.FromResult(-1);
                    }
                    else
                    {
                        AnsiConsole.Console.MarkupLine("[yellow]Taking an infinite time to close[/]");
                    }
                }

                AnsiConsole.Console.MarkupLine("[green]I'm alive![/]");
                Thread.Sleep(2000);
            }
        }
    }
}
