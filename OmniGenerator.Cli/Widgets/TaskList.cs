using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsTCPIP;
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
    internal sealed class TaskList : Renderable, IRenderable
    {
        private Dictionary<string, TaskItem> _tasks = new Dictionary<string, TaskItem>();

        public TaskItem this[string name] => _tasks[name];

        public TaskList AddTask(TaskItem task)
        {
            _tasks.Add(task.Name, task);
            return this;
        }

        protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
        {
            var grid = new Grid()
                .AddColumns(2)
                .LeftAligned();

            foreach(var task in _tasks)
            {
                grid.AddRow(task.Value.Label, task.Value.State);
            }

            return ((IRenderable)grid).Render(options, maxWidth);
        }
    }
}
