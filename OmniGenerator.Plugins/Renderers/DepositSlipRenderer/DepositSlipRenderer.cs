using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Renderers;
using OmniGenerator.Plugins.Renderers.DepositSlipRenderer;
using Svg;
using System.Drawing;

namespace OmniGenerator.Plugins.Renderers.DepositSlipRenderer
{
    /// <summary>
    /// Renders generic cheque deposit slips (bordereaux de remise) for the French banking system.
    /// Generates a standardized document that accompanies cheque deposits, displaying summary information
    /// and a list of individual cheques with a CMC7 machine-readable line for automated processing.
    /// </summary>
    [OmniGeneratorPluginMetadata("renderer.omni.deposit-slip", "Renders a neutral cheque deposit slip for french banking system")]
    public sealed class DepositSlipRenderer : DocumentRendererBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DepositSlipRenderer"/> class.
        /// Creates a renderer with standard deposit slip dimensions (175mm x 90mm).
        /// </summary>
        public DepositSlipRenderer() : base(width: 175, height: 90)
        {
        }

        /// <summary>
        /// Renders the front side (recto) of the deposit slip as an SVG image.
        /// Includes the title, remittance date, total number of cheques, total amount,
        /// a table listing up to 11 individual cheques, and a CMC7 line for automated processing.
        /// </summary>
        /// <param name="document">The deposit slip document containing all field data required for rendering.</param>
        /// <returns>An <see cref="SvgDocument"/> representing the rendered deposit slip recto.</returns>
        public override SvgDocument RenderRecto(Document document)
        {

            var depositSlipfields = new DepositSlipFields(document.Fields);
            var remittanceFields = new RemittanceFields(document.Parent.Fields);
            var svg = SvgExtensions.NewBlankSvg(Width, Height);
            RenderRectoBackground(svg);

            //Title
            svg.DrawText(depositSlipfields.Title, "title", Width / 2f, 10f, "Arial", 8f, Color.Black, anchor: SvgTextAnchor.Middle);

            //Remittance date
            svg.DrawText($"Date de remise : {depositSlipfields.DateRemise:dd/MM/yyyy}", "date-remise", 2f, 20f, "Arial", 4f, Color.Black);

            //Number of cheques
            svg.DrawText($"Nombre de chèques : {remittanceFields.TotalCheques}", "total-cheques", Width / 2f, 20f, "Arial", 4f, Color.Black, anchor: SvgTextAnchor.Middle);

            //Amount
            svg.DrawText($"Montant : {remittanceFields.Amount / 100m:0.00} €", "amount", Width - 2f, 20f, "Arial", 4f, Color.Black, anchor: SvgTextAnchor.End);

            //Cheques
            var cheques = document.Parent.GetElements("cheque").Take(11).ToList();
            foreach (var cheque in cheques)
            {
                var chequeFields = new ChequeFields(cheque.Fields);
                int index = cheques.IndexOf(cheque);
                svg.DrawText($"Chèque {index + 1}", $"cheque-index-{index + 1}", 2f, 26f + index * 5f, "Arial", 3.5f, Color.Black);
                svg.DrawText($"{chequeFields.PayorName?.ToUpper() ?? chequeFields.Dataread}", $"cheque-payor-{index + 1}", 22f, 26f + index * 5f, "Arial", 3.5f, Color.Black);
                svg.DrawText($"{chequeFields.Amount / 100m:0.00} €", $"cheque-amount-{index + 1}", 102f, 26f + index * 5f, "Arial", 3.5f, Color.Black);
            }

            // CMC7 line
            svg.DrawText($"{depositSlipfields.Dataread}", "dataread", 6f, 85f, "CMC7", 4f, Color.Black);

            return svg;
        }

        /// <summary>
        /// Renders the back side (verso) of the deposit slip as an SVG image.
        /// Currently returns a blank document as deposit slips typically don't require verso content.
        /// </summary>
        /// <param name="document">The deposit slip document.</param>
        /// <returns>An <see cref="SvgDocument"/> representing a blank verso page.</returns>
        public override SvgDocument RenderVerso(Document document)
        {
            var svg = SvgExtensions.NewBlankSvg(Width, Height);
            return svg;
        }

        /// <summary>
        /// Renders the background structure and layout elements of the deposit slip recto.
        /// Draws the borders, separator lines, and table structure including:
        /// - Document border
        /// - Header separator line
        /// - Gray background area for the cheque table
        /// - Vertical columns for cheque number, payer name, and amount
        /// - Horizontal rows for up to 11 cheques
        /// - CMC7 line separator at the bottom
        /// </summary>
        /// <param name="svg">The SVG document to which background elements will be added.</param>
        private void RenderRectoBackground(SvgDocument svg)
        {
            // Cheque zone (gray area where cheque table is drawn)
            var grayzone = new SvgRectangle()
            {
                X = 0,
                Y = 22,
                Width = Width,
                Height = 90 - 22 - 13,
                Stroke = new SvgColourServer(Color.LightGray),
                Fill = new SvgColourServer(Color.LightGray),
                StrokeWidth = 0.5f
            };
            svg.Children.Add(grayzone);

            // Border
            var border = new SvgRectangle()
            {
                X = 0,
                Y = 0,
                Width = Width,
                Height = Height,
                Stroke = new SvgColourServer(Color.Black),
                Fill = new SvgColourServer(Color.Transparent),
                StrokeWidth = 0.5f
            };
            svg.Children.Add(border);

            // Horizontal line separating header from details
            var line1 = new SvgLine()
            {
                StartX = 0,
                StartY = 15,
                EndX = Width,
                EndY = 15,
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 0.5f
            };
            svg.Children.Add(line1);

            // Horizontal line separating details from cheques
            var line2 = new SvgLine()
            {
                StartX = 0,
                StartY = 22,
                EndX = Width,
                EndY = 22,
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 0.5f
            };
            svg.Children.Add(line2);

            // Horizontal line separating details from CMC7 line
            var line3 = new SvgLine()
            {
                StartX = 0,
                StartY = 77,
                EndX = Width,
                EndY = 77,
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 0.5f
            };
            svg.Children.Add(line3);

            // Vertical line separating payer name from amount column
            var line4 = new SvgLine()
            {
                StartX = 100,
                StartY = 22,
                EndX = 100,
                EndY = 77,
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 0.1f
            };
            svg.Children.Add(line4);

            // Vertical line separating cheque number from payer name column
            var line5 = new SvgLine()
            {
                StartX = 20,
                StartY = 22,
                EndX = 20,
                EndY = 77,
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 0.1f
            };
            svg.Children.Add(line5);

            // Horizontal lines creating rows for each cheque entry
            for (int i = 0; i < 10; i++)
             {
                var line = new SvgLine()
                {
                    StartX = 0,
                    StartY = 27 + i * 5,
                    EndX = Width,
                    EndY = 27 + i * 5,
                    Stroke = new SvgColourServer(Color.Black),
                    StrokeWidth = 0.1f
                };
                svg.Children.Add(line);
            }
        }
    }
}
