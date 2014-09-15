using System;

namespace ImageProcessing.Service
{
    internal sealed class GammaFilter : LinearFilterBase
    {
        private readonly double _c;

        private readonly double _exp;

        public GammaFilter(double c, double exp)
        {
            this._c = c;
            this._exp = exp;
        }

        protected override void ApplyInternal(byte[] imageBytes, int rowWidth)
        {
            for (int i = 0; i < imageBytes.Length - 2; i += 3)
            {
                double intensity = (imageBytes[i] * BFactor) + (imageBytes[i + 1] * GFactor) + (imageBytes[i + 2] * RFactor);
                double newIntensity = this._c * Math.Pow(intensity, this._exp);
                imageBytes[i] = (byte)(newIntensity / BFactor);
                imageBytes[i + 1] = (byte)(newIntensity / GFactor);
                imageBytes[i + 2] = (byte)(newIntensity / RFactor);
            }
        }
    }
}
