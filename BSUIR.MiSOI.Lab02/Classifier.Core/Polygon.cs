using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Classifier.Core
{
    public sealed class Polygon
    {
        private readonly IList<Pixel> _pixels;
        private readonly int _index;
        private double _massCenterX = -1;
        private double _massCenterY = -1;
        private int _perimeter = -1;
        private double _elongation = -1;
        private double _orientiation = -Math.PI - 1;

        public Polygon(int index)
        {
            this._pixels = new List<Pixel>();
            this._index = index;
            this.Color = Color.White;
        }

        public IList<Pixel> Pixels
        {
            get { return this._pixels; }
        }

        public int Index
        {
            get { return this._index; }
        }

        public int Square
        {
            get { return this._pixels.Count; }
        }

        public double CenterMassX
        {
            get
            {
                if (this._massCenterX < 0)
                {
                    this._massCenterX = this.Square > 0 ? ((double)this.Pixels.Sum(p => p.X) / this.Square) : 0.0d;
                }

                return this._massCenterX;
            }
        }

        public double CenterMassY
        {
            get
            {
                if (this._massCenterY < 0)
                {
                    this._massCenterY = this.Square > 0 ? ((double)this.Pixels.Sum(p => p.Y) / this.Square) : 0.0d;
                }

                return this._massCenterY;
            }
        }

        public int Perimeter
        {
            get
            {
                if (this._perimeter < 0)
                {
                    this._perimeter = this.CalculatePerimeter();
                }

                return this._perimeter;
            }
        }

        public double Compaction
        {
            get { return this.Square > 0 ? (double)this.Perimeter * this.Perimeter / this.Square : 0.0d; }
        }

        public double Elongation
        {
            get
            {
                if (this._elongation < 0)
                {
                    double m20 = this.CalculateDiscreteCentralMoment(2, 0);
                    double m02 = this.CalculateDiscreteCentralMoment(0, 2);
                    double m11 = this.CalculateDiscreteCentralMoment(1, 1);
                    this._elongation = (m20 + m02 + Math.Sqrt(Math.Pow(m20 - m02, 2) + (4 * m11 * m11)))
                                       / (m20 + m02 - Math.Sqrt(Math.Pow(m20 - m02, 2) + (4 * m11 * m11)));
                }

                return this._elongation;
            }
        }

        public double Orientation
        {
            get
            {
                if (this._orientiation < Math.PI)
                {
                    double m11 = this.CalculateDiscreteCentralMoment(1, 1);
                    double m20 = this.CalculateDiscreteCentralMoment(2, 0);
                    double m02 = this.CalculateDiscreteCentralMoment(0, 2);
                    this._orientiation = 0.5d * Math.Atan(2 * m11 / (m20 - m02));
                }

                return Math.Abs(this._orientiation);
            }
        }

        public Color Color { get; set; }

        private double CalculateDiscreteCentralMoment(int i, int j)
        {
            return this.Pixels.Sum(pixel => Math.Pow(pixel.X - this.CenterMassX, i) * Math.Pow(pixel.Y - this.CenterMassY, j));
        }

        private int CalculatePerimeter()
        {
            HashSet<Pixel> horizontalBorderPixels = this.GetDimensionBorderPixels(p => p.X, p => p.Y);
            HashSet<Pixel> verticalBorderPixels = this.GetDimensionBorderPixels(p => p.Y, p => p.X);
            horizontalBorderPixels.UnionWith(verticalBorderPixels);

            return horizontalBorderPixels.Count;
        }

        private HashSet<Pixel> GetDimensionBorderPixels(Func<Pixel, int> keySelector, Func<Pixel, int> valueSelector)
        {
            var groups = this.Pixels.GroupBy(keySelector).Select(p => new { p.Key, Elements = p.OrderBy(valueSelector).ToList() });
            var borderPixels = new HashSet<Pixel>();
            foreach (var @group in groups)
            {
                IList<Pixel> pixels = this.SelectBorderPixels(@group.Elements, valueSelector);
                borderPixels.UnionWith(pixels);
            }

            return borderPixels;
        }

        private IList<Pixel> SelectBorderPixels(IList<Pixel> pixels, Func<Pixel, int> valueSelector)
        {
            List<Pixel> result = new List<Pixel>();
            IList<Pixel> buffer = new List<Pixel>();

            if (pixels.Count <= 1)
            {
                result.AddRange(pixels);
                return result;
            }

            for (int i = 0; i < pixels.Count - 1; ++i)
            {
                buffer.Add(pixels[i]);
                if (Math.Abs(valueSelector(pixels[i]) - valueSelector(pixels[i + 1])) > 1)
                {
                    result.Add(buffer.First());
                    result.Add(buffer.Last());
                    buffer.Clear();
                }
            }

            buffer.Add(pixels.Last());
            result.Add(buffer.First());
            result.Add(buffer.Last());

            return result;
        } 
    }
}
