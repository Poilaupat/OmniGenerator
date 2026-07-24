using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Reflection;

namespace OmniGenerator.Cli.Commands
{
    /// <summary>
    /// CLI command that displays information about the application,
    /// such as name, version, and author.
    /// </summary>
    internal sealed class AboutCommand : AsyncCommand
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<AboutCommand> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutCommand"/> class.
        /// </summary>
        /// <param name="options">The application settings injected via IOptions.</param>
        /// <param name="logger">Logger for capturing diagnostics and runtime information.</param>
        public AboutCommand(
            IOptions<AppSettings> options,
            ILogger<AboutCommand> logger)
        {
            _appSettings = options.Value;
            _logger = logger;
        }

        /// <summary>
        /// Executes the command synchronously.
        /// Displays the application's name, version, and copyright.
        /// </summary>
        /// <param name="context">The current command context.</param>
        /// <returns>Returns 0 if the command executed successfully.</returns>
        public override Task<int> ExecuteAsync(CommandContext context, CancellationToken ct)
        {
            var assembly = Assembly
                .GetExecutingAssembly()
                .GetName();

            AnsiConsole.MarkupLineInterpolated($"[bold blue]{assembly.Name}[/] Version {assembly.Version}");
            AnsiConsole.MarkupLine($":copyright:2025 Ruben Delapille");

            return Task.FromResult(0);
        }
    }
}
