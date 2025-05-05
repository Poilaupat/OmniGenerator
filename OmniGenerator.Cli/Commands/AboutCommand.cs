using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using OmniGenerator.Lib.Interfaces;
using Spectre.Console;
using Spectre.Console.Cli;

namespace OmniGenerator.Cli.Commands
{
    internal class AboutCommand : Command
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<AboutCommand> _logger;

        public AboutCommand(
            IOptions<AppSettings> options, 
            ILogger<AboutCommand> logger) 
        {
            _appSettings = options.Value;
            _logger = logger;
        } 

        public override int Execute(CommandContext context)
        {
            var assembly = Assembly
                .GetExecutingAssembly()
                .GetName();

            AnsiConsole.MarkupLineInterpolated($"[bold blue]{assembly.Name}[/] Version {assembly.Version}");
            AnsiConsole.MarkupLineInterpolated($":copyright:2025 Ruben Delapille");

            return 0;
        }
    }
}
