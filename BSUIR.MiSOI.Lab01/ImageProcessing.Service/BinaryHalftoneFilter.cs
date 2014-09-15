using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.Service
{
    internal sealed class BinaryHalftoneFilter : LinearFilterBase
    {
        private static readonly byte[][] Dither2 = 
        {
            new byte[] { 0, 2 }, 
            new byte[] { 3, 1 }
        };

        private static readonly byte[][] Dither4 = 
        { 
            new byte[] { 0, 8, 2, 10 }, 
            new byte[] { 12, 4, 14, 6 }, 
            new byte[] { 3, 11, 1, 9 }, 
            new byte[] { 15, 7, 13, 5 } 
        };

        protected override void ApplyInternal(byte[] imageBytes, int rowWidth)
        {
            int height = imageBytes.Length / rowWidth;
            int pixelRowWidth = rowWidth / 3;

            var sourceBytes = imageBytes.Clone() as byte[];

            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < pixelRowWidth; ++j)
                {
                    this.ApplyTransform(sourceBytes, imageBytes, i, j, rowWidth);
                }
            }
        }

        private void ApplyTransform(byte[] src, byte[] dest, int i, int j, int rowWidth)
        {
            double intension = (src[i * rowWidth + j * 3] * BFactor) 
                               + (src[i * rowWidth + j * 3 + 1] * GFactor)
                               + (src[i * rowWidth + j * 3 + 2] * RFactor);
            byte newValue = 0;
            double t = (Byte.MaxValue*(Dither4[i%Dither4.Rank][j%Dither4.Rank] + 0.5))/(Dither4.Rank*Dither4.Rank);

            if (intension > t)
            {
                newValue = Byte.MaxValue;
            }

            dest[i * rowWidth + j * 3] = newValue;
            dest[i * rowWidth + j * 3 + 1] = newValue;
            dest[i * rowWidth + j * 3 + 2] = newValue;
        }
    }
}
