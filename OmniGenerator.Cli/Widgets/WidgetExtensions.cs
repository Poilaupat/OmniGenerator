using Microsoft.ProgramSynthesis.Utils.JetBrains.Annotations;
using OmniGenerator.Lib.Renderers;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Interfaces.Infrastructure;
using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniGenerator.Cli.Widgets
{
    public static class WidgetExtensions
    {
        public static IRenderable ToWidget(this HierarchyBuilderProgress progress)
        {
            var grid = new Grid();
            grid.AddColumn();
            grid.AddColumn();

            grid.AddRow(new Markup("[blue]Groups builded[/]"), new Markup($"{progress.CountProcessedGroup} / {progress.CountGroup}"));
            grid.AddRow(new Markup("[blue]Documents builded[/]"), new Markup($"{progress.CountProcessedDocument} / {progress.CountDocument}"));
            grid.AddRow(new Markup("[blue]Fields generated[/]"), new Markup($"{progress.CountField}"));

            var panel = new Panel(grid)
                .ConfigurePanel("Data generation");

            return panel;
        }

        public static IRenderable ToWidget(this DocumentRendererManagerProgress progress)
        {
            var grid = new Grid();
            grid.AddColumn();
            grid.AddColumn();

            grid.AddRow(new Markup("[blue]Documents processed[/]"), new Markup($"{progress.ProcessedDocuments} / {progress.TotalDocuments}"));
            grid.AddRow(new Markup("[blue]Progress[/]"), new Markup($"{progress.Percentage}%"));

            var panel = new Panel(grid)
                .ConfigurePanel("Vector image generation");

            return panel;
        }

        public static Panel ConfigurePanel(this Panel panel, string title)
        {
            panel
                .Header(new PanelHeader(title, Justify.Left))
                .RoundedBorder()
                .Padding(new Padding(1))
                .Expand();

            return panel;
        }
    }
}
