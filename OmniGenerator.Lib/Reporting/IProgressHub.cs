namespace OmniGenerator.Lib.Reporting
{
    public interface IProgressHub<TProgressData>
        where TProgressData : class
    {
        // jobId should uniquely identify the job
        void Report(string jobId, TProgressData value);

        // Get current progress snapshot for specified jobId. Returns false if no progress is available for the job.
        bool TryGetLatest(string jobId, out TProgressData value);

        event Action<string, TProgressData>? DataChanged;
    }
}
