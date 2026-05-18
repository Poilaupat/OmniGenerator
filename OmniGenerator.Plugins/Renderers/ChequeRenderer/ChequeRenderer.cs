using OmniGenerator.Lib.Interfaces;
using Svg;
using System.Drawing;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Renderers;
using OmniGenerator.Lib.Hierarchy;

namespace OmniGenerator.Plugins.Renderers.ChequeRenderer
{
    /// <summary>
    /// A <see cref="IDocumentRenderer"/> for cheque image generation
    /// </summary>
    [OmniGeneratorPluginMetadata("renderer.omni.cheque", "Renders french cheque images")]
    public sealed class ChequeRenderer : DocumentRendererBase
    {
        public ChequeRenderer() : base(width: 175, height: 80)
        {
        }

        public override SvgDocument RenderRecto(Document document)
        {
            var fields = new ChequeFields(document.Fields);
            var svg = SvgExtensions.NewBlankSvg(Width, Height);

            RenderRectoBackground(svg);

            //CMC7 with separator chars
            string[] dataread = fields.Dataread.Split(" ");
            svg.DrawText($"{{{dataread[0]} {{{dataread[1]}}} {dataread[2]}[", "dataread", 6f, 74f, "CMC7", 4f, Color.Black);
            //RLMC Key
            svg.DrawText($"({fields.Rlmc})", "rlmc", 163f, 62f, "Arial", 3f, Color.Black);

            //NumCheque
            svg.DrawText($"N° {dataread[0]}", "numcheque", 8f, 63f, "TimesNewRoman", 3f, Color.Black);

            //Ocrb1
            svg.DrawText($"{dataread[0]}{dataread[1]}", "ocrb1", 138f, 5f, "OCRB", 2.5f, Color.Black);
            //Ocrb2
            svg.DrawText(dataread[2], "ocrb2", 150.5f, 9f, "OCRB", 2.5f, Color.Black);

            //Bank Name
            svg.DrawText(fields.BankName, "bank-name", 8f, 42.5f, "TimesNewRoman", 2f, Color.DimGray);
            //Bank Address
            svg.DrawText(fields.BankAddress, "bank-address", 8f, 45f, "TimesNewRoman", 2f, Color.DimGray);
            //Bank ZipCode and City
            svg.DrawText(fields.BankZipCity, "bank-zipcity", 8f, 47.5f, "TimesNewRoman", 2f, Color.DimGray);
            //Bank Phone
            svg.DrawText($"TEL {fields.BankPhone}", "bank-phone", 8f, 50f, "TimesNewRoman", 2f, Color.DimGray);

            //Payor Name
            svg.DrawText(fields.PayorName.ToUpper(), "payor-name", 61f, 42.5f, "TimesNewRoman", 2f, Color.DimGray);
            //Payor Address
            svg.DrawText(fields.PayorAddress, "payor-address", 61f, 45f, "TimesNewRoman", 2f, Color.DimGray);
            //Payor ZipCode and City
            svg.DrawText(fields.PayorZipCity, "payor-zipcity", 61f, 47.5f, "TimesNewRoman", 2f, Color.DimGray);

            //Lar
            decimal amountEuros = Convert.ToDecimal(fields.Amount.Value) / 100m;
            int euros = (int)amountEuros;
            int centimes = (int)((amountEuros - euros) * 100);
            string lar = $"{NumberToWordsRenderHelper.Convert(euros)} euros";
            lar = char.ToUpper(lar[0]) + lar[1..];
            if (centimes > 0)
                lar += $" et {NumberToWordsRenderHelper.Convert(centimes)} centimes";
            svg.DrawHandwrittenText(lar, "lar", 40f, 19.5f, 3f, Color.DarkBlue, SvgFontWeight.Bold);

            //Car
            svg.DrawHandwrittenText($"{amountEuros:F2} €", "car", 132f, 31f, 3f, Color.DarkBlue, SvgFontWeight.Bold);

            //Payee
            svg.DrawHandwrittenText(fields.PayeeName, "payee", 10f, 32f, 3f, Color.DarkBlue, SvgFontWeight.Bold);

            //Place
            svg.DrawHandwrittenText($"{string.Join(' ', fields.Place.Split(" ").Skip(1))}", "place-value", 132f, 39.5f, 3f, Color.DarkBlue, SvgFontWeight.Bold);

            //Date
            svg.DrawHandwrittenText($"{fields.Date:dd/MM/yyyy}", "date-value", 132f, 44f, 3f, Color.DarkBlue, SvgFontWeight.Bold);

            //Signature
            svg.DrawSignature(areaX: 131f, areaY: 52f, areaWidth: 38f, areaHeight: 5f, color: Color.DarkBlue);

            return svg;
        }

        private void RenderRectoBackground(SvgDocument svg)
        {
            svg.Children.Add(new SvgRectangle { ID = "background", X = 0, Y = 0, Height = Height - 15, Width = Width, Fill = new SvgColourServer(Color.LightGray) });
            svg.Children.Add(new SvgRectangle { ID = "amount-cell", X = 131, Y = 25, Height = 9, Width = 41, Fill = new SvgColourServer(Color.White), Stroke = new SvgColourServer(Color.LightSkyBlue), StrokeWidth = 1 });
            svg.Children.Add(new SvgLine { ID = "place-line", StartX = 131, StartY = 40, EndX = 173, EndY = 40, Stroke = new SvgColourServer(Color.SlateGray), StrokeWidth = 0.1f });
            svg.Children.Add(new SvgLine { ID = "date-line", StartX = 131, StartY = 44.5f, EndX = 173, EndY = 44.5f, Stroke = new SvgColourServer(Color.SlateGray), StrokeWidth = 0.2f });

            var euroglyph = new SvgText() { ID = "euroglyph", FontFamily = "Arial", FontSize = new SvgUnit(3), Fill = new SvgColourServer(Color.SlateGray) };
            euroglyph.X.Add(new SvgUnit(128));
            euroglyph.Y.Add(new SvgUnit(30.5f));
            euroglyph.Nodes.Add(new SvgContentNode { Content = $"€" });
            svg.Children.Add(euroglyph);

            var place = new SvgText() { ID = "place", FontFamily = "TimesNewRoman", FontSize = new SvgUnit(2), Fill = new SvgColourServer(Color.SlateGray) };
            place.X.Add(new SvgUnit(125.5f));
            place.Y.Add(new SvgUnit(40));
            place.Nodes.Add(new SvgContentNode { Content = $"A" });
            svg.Children.Add(place);

            var date = new SvgText() { ID = "date", FontFamily = "TimesNewRoman", FontSize = new SvgUnit(2), Fill = new SvgColourServer(Color.SlateGray) };
            date.X.Add(new SvgUnit(125.5f));
            date.Y.Add(new SvgUnit(44.5f));
            date.Nodes.Add(new SvgContentNode { Content = $"Le" });
            svg.Children.Add(date);

            var signature = new SvgText() { ID = "signature", FontFamily = "TimesNewRoman", FontSize = new SvgUnit(2), Fill = new SvgColourServer(Color.SlateGray) };
            signature.X.Add(new SvgUnit(125.5f));
            signature.Y.Add(new SvgUnit(50));
            signature.Nodes.Add(new SvgContentNode { Content = $"Signature" });
            svg.Children.Add(signature);

            var paymention1 = new SvgText() { ID = "paymention1", FontFamily = "TimesNewRoman", FontSize = new SvgUnit(2), Fill = new SvgColourServer(Color.SlateGray) };
            paymention1.X.Add(new SvgUnit(6.5f));
            paymention1.Y.Add(new SvgUnit(20));
            paymention1.Nodes.Add(new SvgContentNode { Content = $"Payez contre ce chèque en euros €" });
            svg.Children.Add(paymention1);

            svg.Children.Add(new SvgLine { ID = "lar-line1", StartX = 38.5f, StartY = 20f, EndX = 113, EndY = 20f, Stroke = new SvgColourServer(Color.SlateGray), StrokeWidth = 0.2f });

            var paymention2 = new SvgText() { ID = "paymention2", FontFamily = "TimesNewRoman", FontSize = new SvgUnit(1.8f), Fill = new SvgColourServer(Color.SlateGray) };
            paymention2.X.Add(new SvgUnit(6.5f));
            paymention2.Y.Add(new SvgUnit(22.5f));
            paymention2.Nodes.Add(new SvgContentNode { Content = $"non endossable sauf au profit d'un établissement bancaire ou assimilé" });
            svg.Children.Add(paymention2);

            svg.Children.Add(new SvgLine { ID = "lar-line2", StartX = 6.5f, StartY = 26.5f, EndX = 113, EndY = 26.5f, Stroke = new SvgColourServer(Color.SlateGray), StrokeWidth = 0.2f });

            var payee = new SvgText() { ID = "payee", FontFamily = "TimesNewRoman", FontSize = new SvgUnit(1.8f), Fill = new SvgColourServer(Color.SlateGray) };
            payee.X.Add(new SvgUnit(6.5f));
            payee.Y.Add(new SvgUnit(32.5f));
            payee.Nodes.Add(new SvgContentNode { Content = $"à" });
            svg.Children.Add(payee);

            var banklogo = new SvgText() { ID = "bank-logo", FontFamily = "TimesNewRoman", FontSize = new SvgUnit(10f), Fill = new SvgColourServer(Color.SlateGray) };
            banklogo.X.Add(new SvgUnit(6.5f));
            banklogo.Y.Add(new SvgUnit(16f));
            banklogo.Nodes.Add(new SvgContentNode { Content = $"SPECIMEN" });
            svg.Children.Add(banklogo);

            svg.DrawText("Payable en France", "paymention3", 8f, 39f, "TimesNewRoman", 2f, Color.SlateGray);

            svg.Children.Add(new SvgLine { ID = "payee-line", StartX = 8f, StartY = 32.5f, EndX = 113, EndY = 32.5f, Stroke = new SvgColourServer(Color.SlateGray), StrokeWidth = 0.2f });

            svg.Children.Add(new SvgLine { ID = "bar1", StartX = 110f, StartY = 15f, EndX = 100, EndY = 35f, Stroke = new SvgColourServer(Color.SlateGray), StrokeWidth = 0.2f });
            svg.Children.Add(new SvgLine { ID = "bar2", StartX = 105f, StartY = 15f, EndX = 95, EndY = 35f, Stroke = new SvgColourServer(Color.SlateGray), StrokeWidth = 0.2f });
        }

        public override SvgDocument RenderVerso(Document document)
        {
            var fields = new ChequeFields(document.Fields);
            var svg = SvgExtensions.NewBlankSvg(Width, Height);

            svg.DrawText("N° compte : ", "deposit-account-title", 8f, 39f, "Arial", 5f, Color.Black);
            svg.DrawText(fields.DepositAccount, "deposit-account", 40f, 39f, "Arial", 5f, Color.Black);

            return svg;
        }
    }
}
