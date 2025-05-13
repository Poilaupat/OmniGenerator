using OmniGenerator.Lib.Interfaces;
using Svg;
using System.Drawing;
using System.ComponentModel.Composition;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Lib.Drawers.Drawers
{
    /// <summary>
    /// A <see cref="IDocumentDrawer"/> for talon optique image generation
    /// </summary>
    [Export(typeof(IDocumentDrawer))]
    [PluginMetadata("drawer.omni.talon", "Draws TIP SEPA images")]
    public class TalonSepaDrawer : DocumentDrawerBase
    {
        /// <summary>
        /// Creates a new <see cref="TalonSepaDrawer"/>
        /// </summary>
        public TalonSepaDrawer()
        {
            Width = 175;
            Height = 80;
        }

        public override SvgDocument DrawRecto(Document document)
        {
            var svg = SvgExtensions.NewBlankSvg(Width, Height);

            DrawRectoBackground(svg);

            svg.DrawText($"{document.Fields["amount"].Value}", "amount", 80f, 30f, "Arial", 4f, Color.Black);

            string lowline = string.Concat(
                document.Fields["group3"].StringValue,
                document.Fields["group2"].StringValue,
                document.Fields["group1"].StringValue
                );

            svg.DrawText(lowline, "lowline", 10f, 68f, "OCRB", 3.5f, Color.Black);


            return svg;
        }

        private void DrawRectoBackground(SvgDocument svg)
        {
            //Box
            svg.Children.Add(new SvgRectangle { ID = "date-sign_rect", X = 1.5f, Y = 22f, Height = 32f, Width = 70f, Stroke = new SvgColourServer(Color.Black), StrokeWidth = 0.1f });

            //Disclaimer
            var discOffset = 2f;
            svg.DrawText("Mandat de prélèvement SEPA ponctuel : en signant ce formulaire de mandat, vous autorisez", "disclaimer1", 1.5f, discOffset + 0f, "Arial", 1.70f, Color.Black);
            svg.DrawText(" le créancier à envoyer des instructions à votre banque pour débiter votre compte, et votre", "disclaimer2", 1.5f, discOffset + 2f, "Arial", 1.70f, Color.Black);
            svg.DrawText("banque à débiter votre compte conformément aux instructions du créancier. Vous", "disclaimer3", 1.5f, discOffset + 4f, "Arial", 1.70f, Color.Black);
            svg.DrawText("bénéficiez du droit d'être remboursé par votre banque selon les conditions décrites dans", "disclaimer4", 1.5f, discOffset + 6f, "Arial", 1.70f, Color.Black);
            svg.DrawText("la convention que vous avez passée avec elle. Une demande de remboursement doit être ", "disclaimer5", 1.5f, discOffset + 8f, "Arial", 1.70f, Color.Black);
            svg.DrawText("présentée dans les 8 semaines suivant la date de débit de votre compte pour un prélèvement", "disclaimer6", 1.5f, discOffset + 10f, "Arial", 1.70f, Color.Black);
            svg.DrawText("autorisé. Vos droits concernant le présent mandat sont expliqués dans un document que vous", "disclaimer7", 1.5f, discOffset + 12f, "Arial", 1.70f, Color.Black);
            svg.DrawText("pouvez obtenir auprès de votre banque.", "disclaimer8", 1.5f, discOffset + 14f, "Arial", 1.70f, Color.Black);
            svg.DrawText("Le présent document a valeur de mandat de prélèvement SEPA ponctuel. Votre signature", "disclaimer9", 1.5f, discOffset + 16f, "Arial", 1.70f, Color.Black);
            svg.DrawText("vaut autorisation pour débiter, à réception, votre compte pour le montant indiqué.", "disclaimer10", 1.5f, discOffset + 18f, "Arial", 1.70f, Color.Black);

            //Date and place
            svg.DrawText("DATE et LIEU", "date-and-place", 3f, 25f, "Arial", 2.1f, Color.Black);

            //Signature
            svg.DrawText("SIGNATURE", "signature", 50.6f, 25f, "Arial", 2.1f, Color.Black);


            //OCRB line
            svg.Children.Add(new SvgLine { ID = "ocrb-line", StartX = 0f, StartY = 55f, EndX = 175.3f, EndY = 55f, Stroke = new SvgColourServer(Color.Black), StrokeWidth = 0.1f });

            //TIP SEPA
            svg.DrawText("TIP SEPA", "tip-sepa", 75.5f, 16.4f, "Arial", 4.2f, Color.Black);

            //Amount
            svg.DrawText("Montant :", "amount-label", 130f, 25.9f, "Arial", 2.5f, Color.Black);


        }
    }
}
