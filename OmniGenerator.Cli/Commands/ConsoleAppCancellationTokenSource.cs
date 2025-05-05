using BenchmarkDotNet.Loggers;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace OmniGenerator.Cli.Commands
{
    /// <summary>
    /// Provides a <see cref="CancellationTokenSource"/> that integrates with console lifecycle events,
    /// such as <c>Ctrl+C</c> and process termination. This allows graceful cancellation of long-running
    /// or asynchronous console operations.
    /// Inspired by: https://github.com/spectreconsole/spectre.console/issues/701
    /// </summary>
    internal sealed class ConsoleAppCancellationTokenSource
    {
        private ILogger<CancellableAsyncCommand> _logger;
        private readonly CancellationTokenSource _cts = new();

        /// <summary>
        /// Gets the cancellation token that is triggered when the user presses <c>Ctrl+C</c>
        /// or when the process is exiting.
        /// </summary>
        public CancellationToken Token => _cts.Token;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleAppCancellationTokenSource"/> class,
        /// and subscribes to <c>Console.CancelKeyPress</c> and <c>AppDomain.ProcessExit</c> events.
        /// </summary>
        public ConsoleAppCancellationTokenSource(ILogger<CancellableAsyncCommand> logger)
        {
            _logger = logger;

            System.Console.CancelKeyPress += OnCancelKeyPress;
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;

            // Unsubscribes automatically when the token is cancelled
            using var _ = _cts.Token.Register(() =>
            {
                AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
                System.Console.CancelKeyPress -= OnCancelKeyPress;
            });
        }

        /// <summary>
        /// Handles the <c>Ctrl+C</c> event by cancelling the token and preventing immediate termination.
        /// </summary>
        private void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        {
            _logger.LogDebug("OnCancelKeyPress event received");

            // Prevent the process from terminating immediately (so we can let a chance to running processes to terminate gracefully)
            e.Cancel = true;
            _cts.Cancel();
        }

        /// <summary>
        /// Handles the process exit event by cancelling the token if not already cancelled.
        /// </summary>
        private void OnProcessExit(object? sender, EventArgs e)
        {
            _logger.LogDebug("OnProcessExit event received");

            if (_cts.IsCancellationRequested)
            {
                return;
            }

            _cts.Cancel();
        }
    }
}
