using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Interfaces;
using SeedGenerator.Lib.Tools;
using SeedGenerator.Plugins.Image.Fonts;
using Svg;
using System.Drawing;
using System.Reflection;
using System.Reflection.Metadata;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace SeedGenerator.Plugins.Image.Composers
{
    public class ChequeRedComposer : IImageComposer
    {
        public int Width { get; } = 175;
        public int Height { get; } = 80;

        static ChequeRedComposer()
        {
            SvgFontManager.PrivateFontDataList.Add(FontUtility.GetFontBytes("Cmc7.ttf"));
            SvgFontManager.PrivateFontDataList.Add(FontUtility.GetFontBytes("OcrbRegular.ttf"));
            //SvgFontManager.PrivateFontDataList.Add(FontUtility.GetFontBytes("BeautyWind.ttf"));
        }

        public void ComposeDocumentImagesAsync(PacketData packet)
        {
            foreach (var doc in packet.Documents)
            {
                ComposeDocumentImage(doc);
            }
        }

        private void ComposeDocumentImage(DocumentData document)
        {
            if (document.Name == "cheque")
            {
                var svg = new SvgDocument();
                svg.ViewBox = new SvgViewBox(0, 0, Width, Height);

                DrawBackground(svg);

                //CMC7 with separator chars
                string[] cmc7 = document.Fields["cmc7"].Value.Split(" ");
                AddText(svg, $"{{{cmc7[0]} {{{cmc7[1]}}} {cmc7[2]}[", "cmc7", 6f, 74f, "CMC7", 4f, Color.Black);
                //RLMC Key
                AddText(svg, $"({document.Fields["rlmc"].Value})", "rlmc", 163f, 62f, "Arial", 3f, Color.Black);
                
                //NumCheque
                AddText(svg, $"N° {cmc7[0]}", "numcheque", 8f, 63f, "TimesNewRoman", 3f, Color.Black);
                
                //Ocrb1
                AddText(svg, $"{cmc7[0]}{cmc7[1]}", "ocrb1", 138f, 5f, "OCRB", 2.5f, Color.Black);
                //Ocrb2
                AddText(svg, cmc7[2], "ocrb2", 150.5f, 9f, "OCRB", 2.5f, Color.Black);
                
                //Bank Name
                AddText(svg, document.Fields["bank-name"].Value, "bank-name", 8f, 42.5f, "TimesNewRoman", 2f, Color.Black);
                //Bank Address
                AddText(svg, document.Fields["bank-address"].Value, "bank-address", 8f, 45f, "TimesNewRoman", 2f, Color.Black);
                //Bank ZipCode and City
                AddText(svg, document.Fields["bank-zip-city"].Value, "bank-zipcity", 8f, 47.5f, "TimesNewRoman", 2f, Color.Black);
                //Bank Phone
                AddText(svg, $"TEL {document.Fields["bank-phone"].Value}", "bank-phone", 8f, 50f, "TimesNewRoman", 2f, Color.Black);

                //Payor Name
                AddText(svg, $"{document.Fields["payor-name"].Value.ToUpper()}", "payor-name", 61f, 42.5f, "TimesNewRoman", 2f, Color.Black);
                //Payor Address
                AddText(svg, document.Fields["payor-address"].Value, "payor-address", 61f, 45f, "TimesNewRoman", 2f, Color.Black);
                //Payor ZipCode and City
                AddText(svg, document.Fields["payor-zip-city"].Value, "payor-zipcity", 61f, 47.5f, "TimesNewRoman", 2f, Color.Black);

                //Lar
                var amountparts = document.Fields["amount"].Value.Split(",");
                string lar = $"{NumberToWords.Convert(int.Parse(amountparts[0]))} euros";
                if (amountparts.Length > 1)
                    lar += $" et {NumberToWords.Convert(int.Parse(amountparts[1]))} centimes";
                AddText(svg, lar, "lar", 40f, 19.5f, "Arial", 3f, Color.Black);

                //Car
                AddText(svg, $"{document.Fields["amount"].Value} €", "car", 132f, 31f, "Arial", 3f, Color.Black);

                //Payee
                AddText(svg, $"{document.Fields["payee-name"].Value}", "payee", 10f, 32f, "Arial", 3f, Color.Black);

                //Place
                AddText(svg, $"{string.Join(' ', document.Fields["place"].Value.Split(" ").Skip(1))}", "payee", 132f, 39.5f, "Arial", 2f, Color.Black);

                //Date
                AddText(svg, $"{document.Fields["date"].Value}", "date", 132f, 44f, "Arial", 2f, Color.Black);

                document.Image = svg;
            }
        }

        private void AddText(SvgDocument svg, string text, string id, float x, float y, string fontFamilly, float fontSize, Color color)
        {
            var tag = new SvgText() { ID = id, FontFamily = fontFamilly, FontSize = new SvgUnit(fontSize), Fill = new SvgColourServer(color) };
            tag.X.Add(new SvgUnit(x));
            tag.Y.Add(new SvgUnit(y));
            tag.Nodes.Add(new SvgContentNode { Content = text });
            svg.Children.Add(tag);
        }

        private void DrawBackground(SvgDocument svg)
        {
            svg.Fill = new SvgColourServer(Color.White);

            svg.Children.Add(new SvgRectangle { ID = "background", X = 0, Y = 0, Height = this.Height - 15, Width = this.Width, Fill = new SvgColourServer(Color.LightGray) });
            svg.Children.Add(new SvgRectangle { ID = "amount-cell", X = 131, Y = 25, Height = 9, Width = 41, Fill = new SvgColourServer(Color.White), Stroke = new SvgColourServer(Color.LightSkyBlue), StrokeWidth = 1 });
            svg.Children.Add(new SvgLine { ID = "place-line", StartX = 131, StartY = 40, EndX = 173, EndY = 40, Stroke = new SvgColourServer(Color.DarkRed), StrokeWidth = 0.1f });
            svg.Children.Add(new SvgLine { ID = "date-line", StartX = 131, StartY = 44.5f, EndX = 173, EndY = 44.5f, Stroke = new SvgColourServer(Color.DarkRed), StrokeWidth = 0.2f });

            var euroglyph = new SvgText() { ID = "euroglyph", FontFamily = "Arial", FontSize = new SvgUnit(3), Fill = new SvgColourServer(Color.DarkRed) };
            euroglyph.X.Add(new SvgUnit(128));
            euroglyph.Y.Add(new SvgUnit(30.5f));
            euroglyph.Nodes.Add(new SvgContentNode { Content = $"€" });
            svg.Children.Add(euroglyph);

            var place = new SvgText() { ID = "place", FontFamily = "TimesNewRoman", FontSize = new SvgUnit(2), Fill = new SvgColourServer(Color.DarkRed) };
            place.X.Add(new SvgUnit(125.5f));
            place.Y.Add(new SvgUnit(40));
            place.Nodes.Add(new SvgContentNode { Content = $"A" });
            svg.Children.Add(place);

            var date = new SvgText() { ID = "date", FontFamily = "TimesNewRoman", FontSize = new SvgUnit(2), Fill = new SvgColourServer(Color.DarkRed) };
            date.X.Add(new SvgUnit(125.5f));
            date.Y.Add(new SvgUnit(44.5f));
            date.Nodes.Add(new SvgContentNode { Content = $"Le" });
            svg.Children.Add(date);

            var signature = new SvgText() { ID = "signature", FontFamily = "TimesNewRoman", FontSize = new SvgUnit(2), Fill = new SvgColourServer(Color.DarkRed) };
            signature.X.Add(new SvgUnit(125.5f));
            signature.Y.Add(new SvgUnit(50));
            signature.Nodes.Add(new SvgContentNode { Content = $"Signature" });
            svg.Children.Add(signature);

            var paymention1 = new SvgText() { ID = "paymention1", FontFamily = "TimesNewRoman", FontSize = new SvgUnit(2), Fill = new SvgColourServer(Color.DarkRed) };
            paymention1.X.Add(new SvgUnit(6.5f));
            paymention1.Y.Add(new SvgUnit(20));
            paymention1.Nodes.Add(new SvgContentNode { Content = $"Payez contre ce chèque en euros €" });
            svg.Children.Add(paymention1);

            svg.Children.Add(new SvgLine { ID = "lar-line1", StartX = 38.5f, StartY = 20f, EndX = 113, EndY = 20f, Stroke = new SvgColourServer(Color.DarkRed), StrokeWidth = 0.2f });

            var paymention2 = new SvgText() { ID = "paymention2", FontFamily = "TimesNewRoman", FontSize = new SvgUnit(1.8f), Fill = new SvgColourServer(Color.DarkRed) };
            paymention2.X.Add(new SvgUnit(6.5f));
            paymention2.Y.Add(new SvgUnit(22.5f));
            paymention2.Nodes.Add(new SvgContentNode { Content = $"non endossable sauf au profit d'un établissement bancaire ou assimilé" });
            svg.Children.Add(paymention2);

            svg.Children.Add(new SvgLine { ID = "lar-line2", StartX = 6.5f, StartY = 26.5f, EndX = 113, EndY = 26.5f, Stroke = new SvgColourServer(Color.DarkRed), StrokeWidth = 0.2f });

            var payee = new SvgText() { ID = "payee", FontFamily = "TimesNewRoman", FontSize = new SvgUnit(1.8f), Fill = new SvgColourServer(Color.DarkRed) };
            payee.X.Add(new SvgUnit(6.5f));
            payee.Y.Add(new SvgUnit(32.5f));
            payee.Nodes.Add(new SvgContentNode { Content = $"à" });
            svg.Children.Add(payee);

            var banklogo = new SvgText() { ID = "bank-logo", FontFamily = "TimesNewRoman", FontSize = new SvgUnit(10f), Fill = new SvgColourServer(Color.DarkRed) };
            banklogo.X.Add(new SvgUnit(6.5f));
            banklogo.Y.Add(new SvgUnit(16f));
            banklogo.Nodes.Add(new SvgContentNode { Content = $"SPECIMEN" });
            svg.Children.Add(banklogo);

            AddText(svg, "Payable en France", "paymention3", 8f, 39f, "TimesNewRoman", 2f, Color.Black);

            svg.Children.Add(new SvgLine { ID = "payee-line", StartX = 8f, StartY = 32.5f, EndX = 113, EndY = 32.5f, Stroke = new SvgColourServer(Color.DarkRed), StrokeWidth = 0.2f });

            svg.Children.Add(new SvgLine { ID = "bar1", StartX = 110f, StartY = 15f, EndX = 100, EndY = 35f, Stroke = new SvgColourServer(Color.DarkRed), StrokeWidth = 0.2f });
            svg.Children.Add(new SvgLine { ID = "bar2", StartX = 105f, StartY = 15f, EndX = 95, EndY = 35f, Stroke = new SvgColourServer(Color.DarkRed), StrokeWidth = 0.2f });
        }
    }
}
