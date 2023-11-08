using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Interfaces;
using SeedGenerator.Plugins.Image.Fonts;
using SkiaSharp;
using Svg;
using System.Drawing;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace SeedGenerator.Plugins.Image.Composers
{
    public class ChequeComposer : IImageComposer
    {
        public int WidthMM { get; } = 180;
        public int HeightMM { get; } = 85;

        //public readonly string _privateFontsBasePath;

        public ChequeComposer()
        {
            string basePath = Assembly.GetExecutingAssembly().Location;
            //SvgFontManager.PrivateFontPathList.Add($"{basePath}\\Image\\Fonts\\Resources\\Cmc7.ttf");
            //_privateFontsBasePath = $"{basePath}\\Image\\Fonts\\Resources\\";
        }


        //public void ComposeSkia()
        //{
        //    using (var surface = ImageTools.CreateSurface(Resolution, WidthMM, HeightMM))
        //    {
        //        surface.Canvas.Clear(SKColors.White);

        //        using (var paint = new SKPaint())
        //        {
        //            paint.IsAntialias = true;
        //            paint.Typeface = Font.GetFontFromResource("Cmc7.ttf");
        //            paint.Color = SKColors.Black;
        //            paint.TextSize = 150;
        //            paint.MeasureText("1234567 123456789012 123456789012");
        //            var em = paint.Typeface.UnitsPerEm;
        //            surface.Canvas.DrawText("1234567 123456789012 123456789012", 100, 2000, paint);
        //        }

        //        using (var image = surface.Snapshot())
        //        using (var encoded = image.Encode(SKEncodedImageFormat.Png, 100))
        //        using (var stream = File.OpenWrite(Path.Combine(@"C:\Users\Ruben\Documents\Dev\Work", "1.png")))
        //        {
        //            encoded.SaveTo(stream);
        //        }
        //    }
        //}
        public async Task ComposeDocumentImagesAsync(PacketData packet)
        {
            var fontData = FontUtility.GetFont("Cmc7.ttf");
            SvgFontManager.PrivateFontDataList.Add(fontData);

            foreach (var doc in packet.Documents)
            {
                ComposeDocumentImage(doc);
            }

            await Task.CompletedTask;
        }

        private void ComposeDocumentImage(DocumentData document)
        {
            if (document.Name == "cheque" && document.Fields is not null)
            {

                var svg = new SvgDocument();
                svg.ViewBox = new SvgViewBox(0, 0, WidthMM, HeightMM);
                svg.Fill = new SvgColourServer(Color.White);

                //var group = new SvgGroup();
                //svg.Children.Add(group);

                svg.Children.Add(new SvgCircle() { ID = "circle", CenterX = 50, CenterY = 50, Radius = 10, Fill = new SvgColourServer(Color.Black) });

                var cmc7 = new SvgText() { ID = "cmc7" };
                cmc7.X.Add(new SvgUnit(SvgUnitType.Pixel, 10));
                cmc7.Y.Add(new SvgUnit(SvgUnitType.Pixel, 10));
                cmc7.FontFamily = "CMC7";
                cmc7.FontSize = new SvgUnit(6);
                cmc7.Fill = new SvgColourServer(Color.Black);
                var txtContent = new SvgContentNode { Content = document.Fields["cmc7"].Value };
                cmc7.Nodes.Add(txtContent);
                svg.Children.Add(cmc7);

                document.Image = svg;

                float resolution = 200f;
                float rasterX = resolution * WidthMM / 25.4f;
                float rasterY = resolution * HeightMM / 25.4f;

                using (var b = new Bitmap((int)rasterX, (int)rasterY))
                {
                    using (var g = Graphics.FromImage(b))
                    {
                        g.Clear(Color.White);
                        var b2 = svg.Draw((int)rasterX, (int)rasterY);
                        g.DrawImage(b2, 0, 0);
                    }

                    b.SetResolution(resolution, resolution);

                    // Now save b as a JPEG like you normally would
                    b.Save(@"C:\Users\Ruben\source\repos\SeedGenerator\Output\svg.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }

        }



    }
}
