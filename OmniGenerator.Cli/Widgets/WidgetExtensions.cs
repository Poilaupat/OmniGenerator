using Spectre.Console;
using Spectre.Console.Rendering;
using OmniGenerator.Lib.Reporting;

namespace OmniGenerator.Cli.Widgets
{
    public static class WidgetExtensions
    {
        public static IRenderable ToWidget(this HierarchyBuildingProgress progress)
        {
            var grid = new Grid();
            grid.AddColumn();
            grid.AddColumn();

            grid.AddRow(new Markup("[blue]Groups builded[/]"), new Markup($"{progress.ProcessedGroupCount} / {progress.GroupCount}"));
            grid.AddRow(new Markup("[blue]Documents builded[/]"), new Markup($"{progress.ProcessedDocumentCount} / {progress.DocumentCount}"));
            grid.AddRow(new Markup("[blue]Fields generated[/]"), new Markup($"{progress.FieldCount}"));

            var panel = new Panel(grid)
                .ConfigurePanel("Data generation");

            return panel;
        }

        public static IRenderable ToWidget(this RenderingProgress progress)
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

        public static IRenderable ToWidget(this PackagingProgress progress)
        {
            var grid = new Grid();
            grid.AddColumn();
            grid.AddColumn();

            grid.AddRow(new Markup("[blue]Bytes written[/]"), new Markup($"{progress.FormattedBytesWritten}"));
            grid.AddRow(new Markup("[blue]Files written[/]"), new Markup($"{progress.FilesWritten}"));

            if (!string.IsNullOrEmpty(progress.CurrentFileName))
            {
                grid.AddRow(new Markup("[blue]Current file[/]"), new Markup($"[dim]{progress.CurrentFileName.EscapeMarkup()}[/]"));
            }

            var panel = new Panel(grid)
                .ConfigurePanel("Package generation");

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
