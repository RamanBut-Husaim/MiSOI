using System;

namespace ImageProcessing.Service
{
    internal sealed class SolarizationFilter : LinearFilterBase
    {
        private readonly double _kCoef;

        public SolarizationFilter(double k = 2)
        {
            this._kCoef = k;
        }


        protected override void ApplyInternal(byte[] imageBytes, int rowWidth)
        {
            double maxIntensity = 0;

            for (int i = 0; i < imageBytes.Length - 2; i += 3)
            {
                double intensity = (imageBytes[i] * BFactor) + (imageBytes[i + 1] * GFactor) + (imageBytes[i + 2] * RFactor);
                maxIntensity = intensity > maxIntensity ? intensity : maxIntensity;
            }

            for (int i = 0; i < imageBytes.Length - 2; i += 3)
            {
                double intensity = (imageBytes[i] * BFactor) + (imageBytes[i + 1] * GFactor) + (imageBytes[i + 2] * RFactor);
                double newIntensity = this._kCoef * intensity * (maxIntensity - intensity);
                imageBytes[i] = (byte) (newIntensity%byte.MaxValue);
                imageBytes[i + 1] = (byte) (newIntensity%byte.MaxValue);                                               
                imageBytes[i + 2] = (byte) (newIntensity%byte.MaxValue);
            }
        }
    }
}
