using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OmniGenerator.Lib.Interfaces;
using Spectre.Console.Cli;

namespace OmniGenerator.Cli.Commands
{
    internal class VersionCommand : Command
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<VersionCommand> _logger;

        public VersionCommand(IConfiguration configuration, ILogger<VersionCommand> logger) 
        {
            _configuration = configuration;
            _logger = logger;
        } 

        public override int Execute(CommandContext context)
        {
            _logger.LogInformation($"Version : Nichon");
            return 0;
        }
    }
}
