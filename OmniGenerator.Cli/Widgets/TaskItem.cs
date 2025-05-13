using JasperFx.Core.Reflection;
using Microsoft.Diagnostics.Tracing.Parsers.AspNet;
using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    internal sealed class TaskItem
    {
        private Stopwatch _watch = new Stopwatch();
        private ETaskItemState _state = ETaskItemState.Pending;
        private Dictionary<ETaskItemState, Markup> _states = new Dictionary<ETaskItemState, Markup>
        {
            { ETaskItemState.Pending, new Markup("Pending...") },
            { ETaskItemState.Processing, new Markup("[yellow]Processing...[/]") },
            { ETaskItemState.Skipped, new Markup("[yellow]Skipped[/]") },
            { ETaskItemState.Failure, new Markup("[red]Failure[/]") },
            { ETaskItemState.Success, new Markup("[green]Success[/]") }
        };

        public string Name { get; private set; }
        public Markup Label { get; private set; }
        public Markup State => _states[_state];
        public long Elapsed => _watch.ElapsedMilliseconds;

        public TaskItem(string name, Markup label)
        {
            Name = name;
            Label = label;
        }

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
