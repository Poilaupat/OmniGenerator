using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Interfaces;
using SeedGenerator.Plugins.Image.Fonts;
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

        static ChequeComposer()
        {
            var fontData = FontUtility.GetFontBytes("Cmc7.ttf");
            SvgFontManager.PrivateFontDataList.Add(fontData);
        }

        public async Task ComposeDocumentImagesAsync(PacketData packet)
        {
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
            }

        }



    }
}
