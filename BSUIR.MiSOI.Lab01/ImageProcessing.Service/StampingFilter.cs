using System;
using System.Drawing;
using System.Linq;

namespace ImageProcessing.Service
{
    internal sealed class StampingFilter : KernelFilterBase
    {
        private static readonly int[] Kernel = { 0, 1, 0, 1, 0, -1, 0, -1, 0 };

        public override void Apply(Bitmap bitMap)
        {
            using (var clone = bitMap.Clone() as Bitmap)
            {
                for (int i = 1; i < bitMap.Width - 1; ++i)
                {
                    for (int j = 1; j < bitMap.Height - 1; ++j)
                    {
                        this.ApplyTransform(clone, bitMap, i, j);
                    }
                }
            }
        }

        private void ApplyTransform(Bitmap srcbitMap, Bitmap destBitMap, int i, int j)
        {
            double[] intensityMatrix = this.GetIntensityMatrix(srcbitMap, i, j);
            double intensity = Kernel.Select((elem, i1) => intensityMatrix[i1] * elem).Sum();
            intensity = Math.Abs(intensity);

            byte rChannel = (byte)((intensity / RFactor) % byte.MaxValue);
            byte bChannel = (byte)((intensity / BFactor) % byte.MaxValue);
            byte gChannel = (byte)((intensity / GFactor) % byte.MaxValue);

            destBitMap.SetPixel(i, j, Color.FromArgb(rChannel, gChannel, bChannel));
        }

        private double[] GetIntensityMatrix(Bitmap bitMap, int i, int j)
        {
           var result = new double[Kernel.Length];

            Color pixel = bitMap.GetPixel(i - 1, j - 1);
            result[0] = (pixel.R * RFactor) + (pixel.B * BFactor) + (pixel.G * GFactor);

            pixel = bitMap.GetPixel(i, j - 1);
            result[1] = (pixel.R * RFactor) + (pixel.B * BFactor) + (pixel.G * GFactor);

            pixel = bitMap.GetPixel(i + 1, j - 1);
            result[2] = (pixel.R * RFactor) + (pixel.B * BFactor) + (pixel.G * GFactor);

            pixel = bitMap.GetPixel(i - 1, j);
            result[3] = (pixel.R * RFactor) + (pixel.B * BFactor) + (pixel.G * GFactor);

            pixel = bitMap.GetPixel(i, j);
            result[4] = (pixel.R * RFactor) + (pixel.B * BFactor) + (pixel.G * GFactor);

            pixel = bitMap.GetPixel(i + 1, j);
            result[5] = (pixel.R * RFactor) + (pixel.B * BFactor) + (pixel.G * GFactor);

            pixel = bitMap.GetPixel(i - 1, j + 1);
            result[6] = (pixel.R * RFactor) + (pixel.B * BFactor) + (pixel.G * GFactor);

            pixel = bitMap.GetPixel(i, j + 1);
            result[7] = (pixel.R * RFactor) + (pixel.B * BFactor) + (pixel.G * GFactor);

            pixel = bitMap.GetPixel(i + 1, j + 1);
            result[8] = (pixel.R * RFactor) + (pixel.B * BFactor) + (pixel.G * GFactor);

            return result;
        }
    }
}
