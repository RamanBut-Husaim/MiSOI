using System;

namespace Classifier.Core
{
    public sealed class MinimumErrorThresholder : IImageBinarizer
    {
        private byte[] _histogramm;

        public void Process(ImageWrapper imageWrapper)
        {
            this._histogramm = imageWrapper.GetHistogramm();

            int threshold = this.GetBestThreshold();

            for (int i = 0; i < imageWrapper.Height; ++i)
            {
                for (int j = 0; j < imageWrapper.Width; ++j)
                {
                    imageWrapper[i, j] = imageWrapper[i, j] < threshold ? (byte)0 : byte.MaxValue;
                }
            }
        }

        private int GetBestThreshold()
        {
            var factors = new double[byte.MaxValue + 1];
            int best = 1;

            for (int i = 1; i < 256; i++)
            {
                factors[i] = this.J(i);
                if (factors[i] < factors[best])
                {
                    best = i;
                }
            }

            return best;
        }

        private double J(int t)
        {
            double a = this.P1(t); 
            double b = this.S1(t);
            double c = this.P2(t); 
            double d = this.S2(t);

            double result = 1.0d + (2.0d * ((a * this.Flog(b)) + (c * this.Flog(d)))) - (2.0d * ((a * this.Flog(a)) + (c * this.Flog(c))));
            return result;
        }

        private double Flog(double x)
        {
            return x > 0.0d ? Math.Log(x) : 0.0d;
        }

        private int P1(int t)
        {
            int sum = 0;

            for (int i = 0; i <= t; i++)
            {
                sum += this._histogramm[i];
            }

            return sum;
        }

        private int P2(int t)
        {
            int sum = 0;

            for (int i = t + 1; i <= byte.MaxValue; i++)
            {
                sum += this._histogramm[i];
            }

            return sum;
        }

        private double U1(int t)
        {
            int sum = 0;
            double p = this.P1(t);

            if (p <= 0.0d)
            {
                return 0.0d;
            }

            for (int i = 0; i <= t; i++)
            {
                sum += this._histogramm[i] * i;
            }

            return sum / p;
        }

        private double U2(int t)
        {
            int sum = 0;
            double p = this.P2(t);
            if (p <= 0.0d)
            {
                return 0.0d;
            }

            for (int i = t + 1; i <= byte.MaxValue; i++)
            {
                sum += this._histogramm[i] * i;
            }

            return sum / p;
        }

        private double S1(int t)
        {
            double sum = 0;
            double p = this.P1(t);

            if (p <= 0.0d)
            {
                return 0.0d;
            }

            double u = this.U1(t);

            for (int i = 0; i <= t; ++i)
            {
                double x = (i - u) * (i - u);
                sum += x * this._histogramm[i];
            }

            return sum / p;
        }

        private double S2(int t)
        {
            double sum = 0.0d;
            double p = this.P2(t);

            if (p <= 0.0d)
            {
                return 0.0d;
            }

            double u = this.U2(t);
            for (int i = t + 1; i <= byte.MaxValue; ++i)
            {
                double x = (i - u) * (i - u);
                sum += x * this._histogramm[i];
            }

            return sum / p;
        }
    }
}
