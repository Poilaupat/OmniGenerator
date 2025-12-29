using OmniGenerator.Lib.Interfaces;
using Svg;
using System.Drawing;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Renderers;
using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Plugins.Renderers
{
    /// <summary>
    /// A <see cref="IDocumentRenderer"/> for cheque image generation
    /// </summary>
    [OmniGeneratorPluginMetadata("renderer.omni.cheque", "Renders french cheque images")]
    public class ChequeRenderer : DocumentRendererBase
    {
        public ChequeRenderer() : base(width: 175, height: 80)
        {
        }

        public override SvgDocument RenderRecto(Document document)
        {
            var svg = SvgExtensions.NewBlankSvg(Width, Height);

            RenderRectoBackground(svg);

            //CMC7 with separator chars
            string[] dataread = document.Fields.GetStringValue("dataread").Split(" ");
            svg.DrawText($"{{{dataread[0]} {{{dataread[1]}}} {dataread[2]}[", "dataread", 6f, 74f, "CMC7", 4f, Color.Black);
            //RLMC Key
            svg.DrawText($"({document.Fields.GetStringValue("rlmc")})", "rlmc", 163f, 62f, "Arial", 3f, Color.Black);

            //NumCheque
            svg.DrawText($"N° {dataread[0]}", "numcheque", 8f, 63f, "TimesNewRoman", 3f, Color.Black);

            //Ocrb1
            svg.DrawText($"{dataread[0]}{dataread[1]}", "ocrb1", 138f, 5f, "OCRB", 2.5f, Color.Black);
            //Ocrb2
            svg.DrawText(dataread[2], "ocrb2", 150.5f, 9f, "OCRB", 2.5f, Color.Black);

            //Bank Name
            svg.DrawText(document.Fields.GetStringValueOrDefault("bank-name", "Default Bank Name"), "bank-name", 8f, 42.5f, "TimesNewRoman", 2f, Color.Black);
            //Bank Address
            svg.DrawText(document.Fields.GetStringValueOrDefault("bank-address", string.Empty), "bank-address", 8f, 45f, "TimesNewRoman", 2f, Color.Black);
            //Bank ZipCode and City
            svg.DrawText(document.Fields.GetStringValueOrDefault("bank-zip-city", string.Empty), "bank-zipcity", 8f, 47.5f, "TimesNewRoman", 2f, Color.Black);
            //Bank Phone
            svg.DrawText($"TEL {document.Fields.GetStringValueOrDefault("bank-phone", string.Empty)}", "bank-phone", 8f, 50f, "TimesNewRoman", 2f, Color.Black);

            //Payor Name
            svg.DrawText($"{document.Fields.GetStringValue("payor-name").ToUpper()}", "payor-name", 61f, 42.5f, "TimesNewRoman", 2f, Color.Black);
            //Payor Address
            svg.DrawText(document.Fields.GetStringValue("payor-address"), "payor-address", 61f, 45f, "TimesNewRoman", 2f, Color.Black);
            //Payor ZipCode and City
            svg.DrawText(document.Fields.GetStringValue("payor-zip-city"), "payor-zipcity", 61f, 47.5f, "TimesNewRoman", 2f, Color.Black);

            //Lar
            var amountparts = document.Fields.GetStringValue("amount").Split(",");
            string lar = $"{NumberToWords.Convert(int.Parse(amountparts[0]))} euros";
            if (amountparts.Length > 1)
                lar += $" et {NumberToWords.Convert(int.Parse(amountparts[1]))} centimes";
            svg.DrawText(lar, "lar", 40f, 19.5f, "Arial", 3f, Color.Black);

            //Car
            svg.DrawText($"{document.Fields.GetValue("amount").Value:F2} €", "car", 132f, 31f, "Arial", 3f, Color.Black);

            //Payee
            svg.DrawText($"{document.Fields.GetStringValue("payee-name")}", "payee", 10f, 32f, "Arial", 3f, Color.Black);

            //Place
            svg.DrawText($"{string.Join(' ', document.Fields.GetStringValue("place").Split(" ").Skip(1))}", "payee", 132f, 39.5f, "Arial", 2f, Color.Black);

            //Date
            svg.DrawText($"{document.Fields.GetStringValue("date")}", "date", 132f, 44f, "Arial", 2f, Color.Black);

            return svg;
        }

        private void RenderRectoBackground(SvgDocument svg)
        {
            svg.Children.Add(new SvgRectangle { ID = "background", X = 0, Y = 0, Height = this.Height - 15, Width = this.Width, Fill = new SvgColourServer(Color.LightGray) });
            svg.Children.Add(new SvgRectangle { ID = "amount-cell", X = 131, Y = 25, Height = 9, Width = 41, Fill = new SvgColourServer(Color.White), Stroke = new SvgColourServer(Color.LightSkyBlue), StrokeWidth = 1 });
            svg.Children.Add(new SvgLine { ID = "place-line", StartX = 131, StartY = 40, EndX = 173, EndY = 40, Stroke = new SvgColourServer(Color.DarkBlue), StrokeWidth = 0.1f });
            svg.Children.Add(new SvgLine { ID = "date-line", StartX = 131, StartY = 44.5f, EndX = 173, EndY = 44.5f, Stroke = new SvgColourServer(Color.DarkBlue), StrokeWidth = 0.2f });

            var euroglyph = new SvgText() { ID = "euroglyph", FontFamily = "Arial", FontSize = new SvgUnit(3), Fill = new SvgColourServer(Color.DarkBlue) };
            euroglyph.X.Add(new SvgUnit(128));
            euroglyph.Y.Add(new SvgUnit(30.5f));
            euroglyph.Nodes.Add(new SvgContentNode { Content = $"€" });
            svg.Children.Add(euroglyph);

            var place = new SvgText() { ID = "place", FontFamily = "TimesNewRoman", FontSize = new SvgUnit(2), Fill = new SvgColourServer(Color.DarkBlue) };
            place.X.Add(new SvgUnit(125.5f));
            place.Y.Add(new SvgUnit(40));
            place.Nodes.Add(new SvgContentNode { Content = $"A" });
            svg.Children.Add(place);

            var date = new SvgText() { ID = "date", FontFamily = "TimesNewRoman", FontSize = new SvgUnit(2), Fill = new SvgColourServer(Color.DarkBlue) };
            date.X.Add(new SvgUnit(125.5f));
            date.Y.Add(new SvgUnit(44.5f));
            date.Nodes.Add(new SvgContentNode { Content = $"Le" });
            svg.Children.Add(date);

            var signature = new SvgText() { ID = "signature", FontFamily = "TimesNewRoman", FontSize = new SvgUnit(2), Fill = new SvgColourServer(Color.DarkBlue) };
            signature.X.Add(new SvgUnit(125.5f));
            signature.Y.Add(new SvgUnit(50));
            signature.Nodes.Add(new SvgContentNode { Content = $"Signature" });
            svg.Children.Add(signature);

            var paymention1 = new SvgText() { ID = "paymention1", FontFamily = "TimesNewRoman", FontSize = new SvgUnit(2), Fill = new SvgColourServer(Color.DarkBlue) };
            paymention1.X.Add(new SvgUnit(6.5f));
            paymention1.Y.Add(new SvgUnit(20));
            paymention1.Nodes.Add(new SvgContentNode { Content = $"Payez contre ce chèque en euros €" });
            svg.Children.Add(paymention1);

            svg.Children.Add(new SvgLine { ID = "lar-line1", StartX = 38.5f, StartY = 20f, EndX = 113, EndY = 20f, Stroke = new SvgColourServer(Color.DarkBlue), StrokeWidth = 0.2f });

            var paymention2 = new SvgText() { ID = "paymention2", FontFamily = "TimesNewRoman", FontSize = new SvgUnit(1.8f), Fill = new SvgColourServer(Color.DarkBlue) };
            paymention2.X.Add(new SvgUnit(6.5f));
            paymention2.Y.Add(new SvgUnit(22.5f));
            paymention2.Nodes.Add(new SvgContentNode { Content = $"non endossable sauf au profit d'un établissement bancaire ou assimilé" });
            svg.Children.Add(paymention2);

            svg.Children.Add(new SvgLine { ID = "lar-line2", StartX = 6.5f, StartY = 26.5f, EndX = 113, EndY = 26.5f, Stroke = new SvgColourServer(Color.DarkBlue), StrokeWidth = 0.2f });

            var payee = new SvgText() { ID = "payee", FontFamily = "TimesNewRoman", FontSize = new SvgUnit(1.8f), Fill = new SvgColourServer(Color.DarkBlue) };
            payee.X.Add(new SvgUnit(6.5f));
            payee.Y.Add(new SvgUnit(32.5f));
            payee.Nodes.Add(new SvgContentNode { Content = $"à" });
            svg.Children.Add(payee);

            var banklogo = new SvgText() { ID = "bank-logo", FontFamily = "TimesNewRoman", FontSize = new SvgUnit(10f), Fill = new SvgColourServer(Color.DarkBlue) };
            banklogo.X.Add(new SvgUnit(6.5f));
            banklogo.Y.Add(new SvgUnit(16f));
            banklogo.Nodes.Add(new SvgContentNode { Content = $"SPECIMEN" });
            svg.Children.Add(banklogo);

            svg.DrawText("Payable en France", "paymention3", 8f, 39f, "TimesNewRoman", 2f, Color.Black);

            svg.Children.Add(new SvgLine { ID = "payee-line", StartX = 8f, StartY = 32.5f, EndX = 113, EndY = 32.5f, Stroke = new SvgColourServer(Color.DarkBlue), StrokeWidth = 0.2f });

            svg.Children.Add(new SvgLine { ID = "bar1", StartX = 110f, StartY = 15f, EndX = 100, EndY = 35f, Stroke = new SvgColourServer(Color.DarkBlue), StrokeWidth = 0.2f });
            svg.Children.Add(new SvgLine { ID = "bar2", StartX = 105f, StartY = 15f, EndX = 95, EndY = 35f, Stroke = new SvgColourServer(Color.DarkBlue), StrokeWidth = 0.2f });
        }

        public override SvgDocument RenderVerso(Document document)
        {
            var svg = SvgExtensions.NewBlankSvg(Width, Height);

            svg.DrawText("N° compte : ", "deposit-account-title", 8f, 39f, "Arial", 5f, Color.Black);
            svg.DrawText(document.Fields.GetStringValue("deposit-account"), "deposit-account", 40f, 39f, "Arial", 5f, Color.Black);

            return svg;
        }
    }
}
