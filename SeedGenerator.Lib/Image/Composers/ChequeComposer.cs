using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace SeedGenerator.Lib.Image.Composers
{
    public class ChequeComposer
    {
        public int Resolution { get; }
        public int WidthMM { get; }
        public int HeightMM { get; }

        public ChequeComposer(int resolution, int widthMM, int heightMM)
        {
            Resolution = resolution;
            WidthMM = widthMM;
            HeightMM = heightMM;
        }


        public void Compose(Dictionary<string, string> metaData)
        {
            using (var surface = ImageTools.CreateSurface(Resolution, WidthMM, HeightMM))
            {
                surface.Canvas.Clear(SKColor.Parse("0xFFFFFFFF"));

                using (var paint = new SKPaint())
                {
                    paint.IsAntialias = true;
                    surface.Canvas.DrawText("1234567 123456789012 123456789012", paint);
                }
            }
        }
    }
}
