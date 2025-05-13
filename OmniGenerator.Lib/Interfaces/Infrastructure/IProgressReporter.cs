using OmniGenerator.Lib.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Lib.Interfaces.Infrastructure
{
    /// <summary>
    /// Defines a contract for long-running task that regularly reports their progress 
    /// </summary>
    public interface IProgressReporter<TReport> 
    {
        /// <summary>
        /// Gets of sets an optionnal progress reporter used to receive updates about the process.
        /// </summary>
        IProgress<TReport>? Progress { get; set; }

        /// <summary>
        /// Gets or sets the minimum time interval, in milliseconds, between two progress updates.
        /// This value controls how frequently progress notifications should be emitted. 
        /// </summary>
        int ProgressResolution { get; set; }
    }
}
