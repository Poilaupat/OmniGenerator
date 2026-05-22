using Quartz;
using System.Collections.Concurrent;

namespace OmniGenerator.Cli.Quartz
{
    /// <summary>
    /// A Quartz <see cref="IJobListener"/> that tracks running state and execution errors per job.
    /// Designed to be registered as a singleton and shared with the UI layer.
    /// </summary>
    internal sealed class JobStateListener : IJobListener
    {
        private readonly ConcurrentDictionary<string, bool> _runningJobs = new();
        private readonly ConcurrentDictionary<string, string> _errors = new();

        public string Name => nameof(JobStateListener);

        public IReadOnlyDictionary<string, string> Errors => _errors;

        public bool IsRunning(string jobId) => _runningJobs.ContainsKey(jobId);

        public event Action? StateChanged;

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            _runningJobs[context.JobDetail.Key.Name] = true;
            StateChanged?.Invoke();
            return Task.CompletedTask;
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException, CancellationToken cancellationToken = default)
        {
            var jobId = context.JobDetail.Key.Name;
            _runningJobs.TryRemove(jobId, out _);
            if (jobException is not null)
                _errors[jobId] = jobException.InnerException?.Message ?? jobException.Message;
            StateChanged?.Invoke();
            return Task.CompletedTask;
        }

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            _runningJobs.TryRemove(context.JobDetail.Key.Name, out _);
            StateChanged?.Invoke();
            return Task.CompletedTask;
        }
    }
}
