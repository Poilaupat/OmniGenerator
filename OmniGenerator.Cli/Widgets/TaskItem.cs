using Spectre.Console;
using System.Diagnostics;

namespace OmniGenerator.Cli.Widgets
{
    internal enum ETaskItemState
    {
        Pending,
        Skipped,
        Processing,
        Failure,
        Success,
    }

    internal sealed class TaskItem(string name, Markup label)
    {
        private readonly Stopwatch _watch = new();
        private ETaskItemState _state = ETaskItemState.Pending;
        private readonly Dictionary<ETaskItemState, Markup> _states = new()
        {
            { ETaskItemState.Pending, new Markup("Pending...") },
            { ETaskItemState.Processing, new Markup("[yellow]Processing...[/]") },
            { ETaskItemState.Skipped, new Markup("[yellow]Skipped[/]") },
            { ETaskItemState.Failure, new Markup("[red]Failure[/]") },
            { ETaskItemState.Success, new Markup("[green]Success[/]") }
        };

        public string Name { get; } = name;
        public Markup Label { get; } = label;
        public Markup State => _states[_state];
        public long Elapsed => _watch.ElapsedMilliseconds;

        public void SetPending()
        {
            _state = ETaskItemState.Pending;
            _watch.Stop();
        }

        public void SetProcessing()
        {
            _state = ETaskItemState.Processing;
            _watch.Start();
        }

        public void SetFailed()
        {
            _state = ETaskItemState.Failure;
            _watch.Stop();
        }

        public void SetSucceeded()
        {
            _state = ETaskItemState.Success;
            _watch.Stop();
        }

        public void SetSkipped()
        {
            _state = ETaskItemState.Skipped;
        }
    }
}
