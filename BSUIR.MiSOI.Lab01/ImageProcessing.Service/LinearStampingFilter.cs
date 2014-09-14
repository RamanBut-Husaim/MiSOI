using System;
using System.Linq;

namespace ImageProcessing.Service
{
    internal sealed class LinearStampingFilter : LinearFilterBase
    {
        private static readonly int[] Kernel = { 0, 1, 0, 1, 0, -1, 0, -1, 0 };

        protected override void ApplyInternal(byte[] imageBytes, int rowWidth)
        {
            int height = imageBytes.Length / rowWidth;
            int pixelRowWidth = (rowWidth - 1) / 3;

            var sourceBytes = imageBytes.Clone() as byte[];

            for (int i = 1; i < height - 1; ++i)
            {
                for (int j = 1; j < pixelRowWidth; ++j)
                {
                    this.ApplyTransform(sourceBytes, imageBytes, i, j, rowWidth);
                }
            }

            this.FixSideEffects(imageBytes, rowWidth, height);
        }

        private void FixSideEffects(byte[] imageBytes, int rowWidth, int height)
        {
            //upper bound
            for (int i = 0; i < rowWidth; ++i)
            {
                imageBytes[i] = 0;
            }

            //right Bound
            for (int i = 0; i < height; ++i)
            {
                imageBytes[i * rowWidth + rowWidth - 3] = 0;
                imageBytes[i * rowWidth + rowWidth - 2] = 0;
                imageBytes[i * rowWidth + rowWidth - 1] = 0;
            }

            //lower bound
            for (int i = 0; i < rowWidth; ++i)
            {
                imageBytes[(height - 1) * rowWidth + i] = 0;
            }

            //left bound
            for (int i = 0; i < height; ++i)
            {
                imageBytes[i * rowWidth] = 0;
                imageBytes[i * rowWidth + 1] = 0;
                imageBytes[i * rowWidth + 2] = 0;
            }
        }

        private void ApplyTransform(byte[] srcBytes, byte[] destBytes, int i, int j, int width)
        {
            double[] intensityMatrix = this.GetIntensityMatrix(srcBytes, i, j, width);
            double intensity = Kernel.Select((elem, i1) => intensityMatrix[i1] * elem).Sum();
            intensity = Math.Abs(intensity);

            byte rChannel = (byte)((intensity / RFactor) % byte.MaxValue);
            byte bChannel = (byte)((intensity / BFactor) % byte.MaxValue);
            byte gChannel = (byte)((intensity / GFactor) % byte.MaxValue);

            destBytes[i * width + j * 3] = bChannel;
            destBytes[i * width + j * 3 + 1] = gChannel;
            destBytes[i * width + j * 3 + 2] = rChannel;
        }

        private double[] GetIntensityMatrix(byte[] srcBytes, int i, int j, int width)
        {
            var result = new double[Kernel.Length];

            result[0] = (srcBytes[(i - 1) * width + (j - 1) * 3] * BFactor) +
                (srcBytes[(i - 1) * width + (j - 1) * 3 + 1] * GFactor) + 
                (srcBytes[(i - 1) * width + (j - 1) * 3 + 2] * RFactor);

            result[1] = (srcBytes[(i) * width + (j - 1) * 3] * BFactor) +
               (srcBytes[(i) * width + (j - 1) * 3 + 1] * GFactor) +
               (srcBytes[(i) * width + (j - 1) * 3 + 2] * RFactor);

            result[2] = (srcBytes[(i + 1) * width + (j - 1) * 3] * BFactor) +
               (srcBytes[(i + 1) * width + (j - 1) * 3 + 1] * GFactor) +
               (srcBytes[(i + 1) * width + (j - 1) * 3 + 2] * RFactor);

            result[3] = (srcBytes[(i - 1) * width + (j) * 3] * BFactor) +
                (srcBytes[(i - 1) * width + (j) * 3 + 1] * GFactor) +
                (srcBytes[(i - 1) * width + (j) * 3 + 2] * RFactor);

            result[4] = (srcBytes[(i) * width + (j) * 3] * BFactor) +
               (srcBytes[(i) * width + (j) * 3 + 1] * GFactor) +
               (srcBytes[(i) * width + (j) * 3 + 2] * RFactor);

            result[5] = (srcBytes[(i + 1) * width + (j) * 3] * BFactor) +
               (srcBytes[(i + 1) * width + (j) * 3 + 1] * GFactor) +
               (srcBytes[(i + 1) * width + (j) * 3 + 2] * RFactor);

            result[6] = (srcBytes[(i - 1) * width + (j + 1) * 3] * BFactor) +
                (srcBytes[(i - 1) * width + (j + 1) * 3 + 1] * GFactor) +
                (srcBytes[(i - 1) * width + (j + 1) * 3 + 2] * RFactor);

            result[7] = (srcBytes[(i) * width + (j + 1) * 3] * BFactor) +
               (srcBytes[(i) * width + (j + 1) * 3 + 1] * GFactor) +
               (srcBytes[(i) * width + (j + 1) * 3 + 2] * RFactor);

            result[8] = (srcBytes[(i + 1) * width + (j + 1) * 3] * BFactor) +
               (srcBytes[(i + 1) * width + (j + 1) * 3 + 1] * GFactor) +
               (srcBytes[(i + 1) * width + (j + 1) * 3 + 2] * RFactor);


            return result;
        }
    }
}
