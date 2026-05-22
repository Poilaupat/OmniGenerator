using Microsoft.ProgramSynthesis.Split.Text.Build.NodeTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Cli.Tools
{
    internal static class TimeSpanExtensions
    {
        public static string ToFluidUnitString(this TimeSpan span) => span switch
        {
            TimeSpan when ((TimeSpan)span).TotalMilliseconds < 1000 => $"{((TimeSpan)span).TotalMilliseconds} ms",
            TimeSpan when ((TimeSpan)span).TotalSeconds < 60 => $"{((TimeSpan)span).TotalSeconds} s",
            TimeSpan when ((TimeSpan)span).TotalSeconds >= 60 => $"{((TimeSpan)span).Minutes} m {((TimeSpan)span).Seconds} s",
            TimeSpan when ((TimeSpan)span).TotalMinutes >= 60 => $"{((TimeSpan)span).Hours} h {((TimeSpan)span).Minutes} m {((TimeSpan)span).Seconds} s",
            _ => "0 ms"
        };
    }
}
