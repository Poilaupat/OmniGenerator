namespace OmniGenerator.Cli.Commands
{
    /// <summary>
    /// Represents a single scheduled generation job entry in a <see cref="SchedulePlan"/>.
    /// </summary>
    internal sealed class ScheduledJobEntry
    {
        /// <summary>
        /// Unique name for this job. Used as the Quartz job key and progress hub identifier.
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Path to the OmniGenerator configuration file for this job.
        /// </summary>
        public required string SettingsFilePath { get; init; }

        /// <summary>
        /// Path to the output folder where generated content will be saved.
        /// </summary>
        public required string OutputFolderPath { get; init; }

        /// <summary>
        /// Quartz cron expression that controls when this job executes.
        /// </summary>
        public required string CronSchedule { get; init; }
    }
}
