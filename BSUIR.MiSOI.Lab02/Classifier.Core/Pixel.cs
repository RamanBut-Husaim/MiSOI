using System;

namespace Classifier.Core
{
    public struct Pixel : IEquatable<Pixel>
    {
        private readonly int _x;
        private readonly int _y;
        private readonly byte _color;

        public Pixel(int x, int y, byte color)
        {
            this._x = x;
            this._y = y;
            this._color = color;
        }

        public int X
        {
            get { return this._x; }
        }

        public int Y
        {
            get { return this._y; }
        }

        public byte Color
        {
            get { return this._color; }
        }

        public bool Equals(Pixel other)
        {
            return this.X.Equals(other.X) && this.Y.Equals(other.Y);
        }

        public override int GetHashCode()
        {
            return this.X ^ this.Y;
        }
    }
}
