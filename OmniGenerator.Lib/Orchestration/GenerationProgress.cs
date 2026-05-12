namespace OmniGenerator.Lib.Orchestration
{
    public enum GenerationStep
    {
        Hierarchy,
        Images,
        Package
    }

    public enum StepStatus
    {
        Pending,
        Processing,
        Succeeded,
        Failed,
        Skipped
    }

    public class GenerationProgress
    {
        public GenerationStep Step { get; init; }
        public StepStatus Status { get; init; }
        public Exception? Error { get; init; }
    }
}
