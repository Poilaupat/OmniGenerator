using BenchmarkDotNet.Loggers;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OmniGenerator.Cli.Commands
{
    /// <summary>
    /// Represents an abstract base class for an asynchronous CLI command
    /// that supports cancellation via a shared <see cref="ConsoleAppCancellationTokenSource"/>.
    /// </summary>
    internal abstract class CancellableAsyncCommand : AsyncCommand
    {
        private readonly ConsoleAppCancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Initialize a new instance of the <see cref="CancellableAsyncCommand"/> class
        /// </summary>
        public CancellableAsyncCommand(ILogger<CancellableAsyncCommand> logger)
        {
            _cancellationTokenSource = new ConsoleAppCancellationTokenSource(logger);
        }

        /// <summary>
        /// Executes the command asynchronously with cancellation support.
        /// </summary>
        /// <param name="context">The command context.</param>
        /// <param name="cancellation">A cancellation token used to signal command cancellation.</param>
        /// <returns>An integer result code representing the command execution result.</returns>
        public abstract Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellation);

        /// <inheritdoc />
        public sealed override async Task<int> ExecuteAsync(CommandContext context)
            => await ExecuteAsync(context, _cancellationTokenSource.Token);
    }

    /// <summary>
    /// Represents an abstract base class for a typed asynchronous CLI command
    /// that supports cancellation via a shared <see cref="ConsoleAppCancellationTokenSource"/>.
    /// </summary>
    /// <typeparam name="TSettings">The type of settings used by the command.</typeparam>
    internal abstract class CancellableAsyncCommand<TSettings> : AsyncCommand<TSettings>
        where TSettings : CommandSettings
    {
        private readonly ConsoleAppCancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Initialize a new instance of the <see cref="CancellableAsyncCommand"/> class
        /// </summary>
        public CancellableAsyncCommand(ILogger<CancellableAsyncCommand> logger)
        {
            _cancellationTokenSource = new ConsoleAppCancellationTokenSource(logger);
        }

        /// <summary>
        /// Executes the typed command asynchronously with cancellation support.
        /// </summary>
        /// <param name="context">The command context.</param>
        /// <param name="settings">The command settings.</param>
        /// <param name="cancellation">A cancellation token used to signal command cancellation.</param>
        /// <returns>An integer result code representing the command execution result.</returns>
        public abstract Task<int> ExecuteAsync(CommandContext context, TSettings settings, CancellationToken cancellation);

        /// <inheritdoc />
        public sealed override async Task<int> ExecuteAsync(CommandContext context, TSettings settings)
            => await ExecuteAsync(context, settings, _cancellationTokenSource.Token);
    }
}
