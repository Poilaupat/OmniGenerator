using System.Collections.Concurrent;

namespace OmniGenerator.Lib.Reporting
{
    public sealed class ProgressHub<TProgressData> : IProgressHub<TProgressData>
        where TProgressData : class
    {
        private readonly ConcurrentDictionary<string, TProgressData> _store = new();

        public event Action<string, TProgressData>? DataChanged;

        public void Report(string jobId, TProgressData value)
        {
            _store[jobId] = value;
            DataChanged?.Invoke(jobId, value);
        }

        public bool TryGetLatest(string jobId, out TProgressData value)
        {
            if (_store.TryGetValue(jobId, out var foundValue))
            {
                value = foundValue;
                return true;
            }
            value = default!;
            return false;
        }
    }
}
