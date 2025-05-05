using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;

namespace OmniGenerator.Cli
{
    public class CliAppSettings
    {
        [ConfigurationKeyName("progress-resolution")]
        public int ProgressResolution { get; set; }
    }
}
