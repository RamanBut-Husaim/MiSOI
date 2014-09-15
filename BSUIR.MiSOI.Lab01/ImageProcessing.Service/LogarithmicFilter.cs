using System;

namespace ImageProcessing.Service
{
    internal sealed class LogarithmicFilter : LinearFilterBase
    {
        private readonly double _c;

        public LogarithmicFilter(double c)
        {
            this._c = c;
        }

        protected override void ApplyInternal(byte[] imageBytes, int rowWidth)
        {
            for (int i = 0; i < imageBytes.Length - 2; i += 3)
            {
                double intensity = (imageBytes[i] * BFactor) + (imageBytes[i + 1] * GFactor) + (imageBytes[i + 2] * RFactor);
                double newIntensity = this._c * Math.Log(intensity + 1);
                imageBytes[i] = (byte)(newIntensity / BFactor);
                imageBytes[i + 1] = (byte)(newIntensity / GFactor);
                imageBytes[i + 2] = (byte)(newIntensity / RFactor);
            }
        }
    }
}
