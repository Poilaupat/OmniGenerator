namespace OmniGenerator.Cli.Commands
{
    /// <summary>
    /// Represents a schedule plan file containing one or more scheduled generation jobs.
    /// </summary>
    internal sealed class SchedulePlan
    {
        /// <summary>
        /// The list of jobs to schedule and run in parallel.
        /// </summary>
        public required IReadOnlyList<ScheduledJobEntry> Jobs { get; init; }
    }
}
