using SeedGenerator.Lib.Fonts;
using SkiaSharp;
using Svg;

namespace SeedGenerator.Lib.Image.Composers
{
    public class ChequeComposer
    {
        public int Resolution { get; }
        public int WidthMM { get; } = 180;
        public int HeightMM { get; } = 85;

        public ChequeComposer(int resolution)
        {
            Resolution = resolution;
        }


        public void Compose(Dictionary<string, string> metaData)
        {
            ComposeSvg();
        }

        public void ComposeSkia()
        {
            using (var surface = ImageTools.CreateSurface(Resolution, WidthMM, HeightMM))
            {
                surface.Canvas.Clear(SKColors.White);

                using (var paint = new SKPaint())
                {
                    paint.IsAntialias = true;
                    paint.Typeface = Font.GetFontFromResource("Cmc7.ttf");
                    paint.Color = SKColors.Black;
                    paint.TextSize = 150;
                    paint.MeasureText("1234567 123456789012 123456789012");
                    var em = paint.Typeface.UnitsPerEm;
                    surface.Canvas.DrawText("1234567 123456789012 123456789012", 100, 2000, paint);
                }

                using (var image = surface.Snapshot())
                using (var encoded = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var stream = File.OpenWrite(Path.Combine(@"C:\Users\Ruben\Documents\Dev\Work", "1.png")))
                {
                    encoded.SaveTo(stream);
                }
            }
        }

        public void ComposeSvg()
        {
            var fontData = Font.GetFont("Cmc7.ttf");
            SvgFontManager.PrivateFontDataList.Add(fontData);

            var doc = new SvgDocument();
            doc.ViewBox = new SvgViewBox(0, 0, WidthMM, HeightMM);
            doc.Fill = new SvgColourServer(System.Drawing.ColorTranslator.FromHtml("#000000"));

            var group = new SvgGroup();
            doc.Children.Add(group);

            group.Children.Add(new SvgCircle() { ID = "circle", CenterX = 50, CenterY = 50, Radius = 10, Fill = new SvgColourServer(System.Drawing.ColorTranslator.FromHtml("#FFFFFF")) });
            var txt = new SvgText() { ID = "cmc7" };
            var txtContent = new SvgContentNode { Content = "1234567 123456789012 123456789012" };
            txt.Nodes.Add(txtContent);
            group.Children.Add(txt);


            doc.Draw().Save(@"C:\Users\Ruben\Documents\Dev\Work\svg.png", System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
