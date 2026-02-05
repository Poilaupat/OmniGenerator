using OmniGenerator.Lib.Interfaces;
using Svg;
using System.Drawing;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Renderers;
using QRCoder;

namespace OmniGenerator.Plugins.Renderers.TalonRenderer
{
    /// <summary>
    /// A <see cref="IDocumentRenderer"/> for TIP SEPA (Titre Interbancaire de Paiement SEPA) optical slip image generation.
    /// Renders standardized payment authorization forms used in SEPA direct debit transactions.
    /// </summary>
    [OmniGeneratorPluginMetadata("renderer.omni.talon", "Renders TIP SEPA images")]
    public class TalonSepaRenderer : DocumentRendererBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TalonSepaRenderer"/> class.
        /// Creates a renderer with standard TIP SEPA dimensions (175mm x 80mm).
        /// </summary>
        public TalonSepaRenderer() : base(width: 175, height: 80)
        {
        }

        /// <summary>
        /// Renders the front side (recto) of a TIP SEPA document as an SVG image.
        /// Includes payer information, creditor details, amount, SEPA mandate disclaimer, and OCR lines.
        /// </summary>
        /// <param name="document">The document containing all field data required for rendering.</param>
        /// <returns>An <see cref="SvgDocument"/> representing the rendered TIP SEPA slip.</returns>
        public override SvgDocument RenderRecto(Document document)
        {
            var fields = new TalonSepaRendererFields(document.Fields);
            var svg = SvgExtensions.NewBlankSvg(Width, Height);

            RenderRectoBackground(svg);

            // Upper Left - SEPA identifiers - Optional, if provided in fields; otherwise, will be blank or show "JOIGNEZ UN RIB" for missing IBAN
            svg.DrawText($"IBAN : {fields.Iban ?? "JOIGNEZ UN RIB"}", "rib", 1.5f, 2.5f, "Calibri", 2.8f, Color.Black);
            svg.DrawText($"ICS : {fields.Ics}", "ics", 1.5f, 5.5f, "Calibri", 2.8f, Color.Black);
            svg.DrawText($"RUM : {fields.Rum}", "rum", 1.5f, 8.5f, "Calibri", 2.8f, Color.Black);

            // Upper Middle - Payer information - Optional, if provided in fields; otherwise, will be blank
            svg.DrawText($"M OU MME {fields.PayorName}", "payor-name", 80f, 2.5f, "Calibri", 2.5f, Color.Black);
            svg.DrawText($"{fields.PayorAddress}", "payor-address", 80f, 5.5f, "Calibri", 2.5f, Color.Black);
            svg.DrawText($"{fields.PayorZipCity}", "payor-zip-city", 80f, 8.5f, "Calibri", 2.5f, Color.Black);

            // Upper Right - Amount
            svg.DrawText($"Montant : {Convert.ToSingle(fields.Amount.Value) / 100f:C}", "amount", Width - 2f, 3.5f, "Calibri", 4.2f, Color.Black, SvgFontWeight.Bold, SvgTextAnchor.End);

            // Middle right - Payee/Creditor information - Optional, if provided in fields; otherwise, will be blank
            svg.DrawText($"{fields.PayeeName}", "payee-name", 110f, 36f, "Calibri", 3.8f, Color.Black);
            svg.DrawText($"{fields.PayeeAddress}", "payee-address", 110f, 40f, "Calibri", 3.8f, Color.Black);
            svg.DrawText($"{fields.PayeeZipCity}", "payee-zip-city", 110f, 44f, "Calibri", 3.8f, Color.Black);

            // High OCR Line - Machine-readable upper line - Optional, if provided in fields; otherwise, will be blank
            svg.DrawText(fields.HighLine ?? string.Empty, "highline", 6f, 66f, "OCRB", 3.5f, Color.Black);

            // Low OCR line - Machine-readable lower line
            svg.DrawText(fields.LowLine, "lowline", 150f, 74f, "OCRB", 3.5f, Color.Black, anchor: SvgTextAnchor.End);

            // Datamatrix - Optional, if datamatrix content is provided
            if (fields.Datamatrix is not null)
            {
                svg.DrawBarCode(fields.Datamatrix, EBarCodeType.DataMatrixECC200, 1.5f, 9f, 0.65f);
            }

            return svg;
        }

        /// <summary>
        /// Renders the background elements of the TIP SEPA recto, including:
        /// - SEPA mandate legal disclaimer text
        /// - "TIP SEPA" title
        /// - Date, place, and signature box
        /// - Separator line for OCR zone
        /// </summary>
        /// <param name="svg">The SVG document to which background elements will be added.</param>
        private void RenderRectoBackground(SvgDocument svg)
        {
            // Disclaimer - SEPA mandate legal text
            var discOffset = 35f;
            svg.DrawText("Mandat de prélèvement SEPA ponctuel : en signant ce formulaire de mandat, vous autorisez", "disclaimer1", 1.5f, discOffset + 0f, "Arial", 2f, Color.Black);
            svg.DrawText(" le créancier à envoyer des instructions à votre banque pour débiter votre compte, et votre", "disclaimer2", 1.5f, discOffset + 2f, "Arial", 2f, Color.Black);
            svg.DrawText("banque à débiter votre compte conformément aux instructions du créancier. Vous", "disclaimer3", 1.5f, discOffset + 4f, "Arial", 2f, Color.Black);
            svg.DrawText("bénéficiez du droit d'être remboursé par votre banque selon les conditions décrites dans", "disclaimer4", 1.5f, discOffset + 6f, "Arial", 2f, Color.Black);
            svg.DrawText("la convention que vous avez passée avec elle. Une demande de remboursement doit être ", "disclaimer5", 1.5f, discOffset + 8f, "Arial", 2f, Color.Black);
            svg.DrawText("présentée dans les 8 semaines suivant la date de débit de votre compte pour un prélèvement", "disclaimer6", 1.5f, discOffset + 10f, "Arial", 2f, Color.Black);
            svg.DrawText("autorisé. Vos droits concernant le présent mandat sont expliqués dans un document que vous", "disclaimer7", 1.5f, discOffset + 12f, "Arial", 2f, Color.Black);
            svg.DrawText("pouvez obtenir auprès de votre banque.", "disclaimer8", 1.5f, discOffset + 14f, "Arial", 2f, Color.Black);
            svg.DrawText("Le présent document a valeur de mandat de prélèvement SEPA ponctuel. Votre signature", "disclaimer9", 1.5f, discOffset + 16f, "Arial", 2f, Color.Black);
            svg.DrawText("vaut autorisation pour débiter, à réception, votre compte pour le montant indiqué.", "disclaimer10", 1.5f, discOffset + 18f, "Arial", 2f, Color.Black);

            // TIP SEPA title
            svg.DrawText("TIP S€PA", "tip-sepa", 120f, 18f, "Calibri", 6.35f, Color.Black, SvgFontWeight.Bold);

            // Box for date, place, and signature
            svg.Children.Add(new SvgRectangle { ID = "date-sign_rect", X = 30f, Y = 12f, Height = 18f, Width = 60f, Stroke = new SvgColourServer(Color.Black), StrokeWidth = 0.1f });
            svg.DrawText("Date et Lieu", "date-and-place", 32f, 14.1f, "Calibri", 2.8f, Color.Black);
            svg.DrawText("Signature", "signature", 76f, 14.1f, "Calibri", 2.8f, Color.Black);

            // Separator line for OCR zone
            svg.Children.Add(new SvgLine { ID = "ocrb-line", StartX = 0f, StartY = 55f, EndX = Width, EndY = 55f, Stroke = new SvgColourServer(Color.Black), StrokeWidth = 0.1f });
        }
    }
}
