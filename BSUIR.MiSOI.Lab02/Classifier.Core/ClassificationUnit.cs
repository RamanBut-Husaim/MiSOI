using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace Classifier.Core
{
    public sealed class ClassificationUnit : IClassifiable
    {
        private readonly int _index;
        private readonly Polygon _polygon;
        private IDistanceCalculator<ClassificationUnit> _distanceCalculator;
        private int _class;
        private int _isKernel;

        public ClassificationUnit(Polygon polygon)
        {
            this._index = polygon.Index;
            this._polygon = polygon;
            this.Criterion = new ReadOnlyCollection<double>(new[]
            {
                polygon.Square, 
                polygon.Perimeter, 
                polygon.Compaction,
                polygon.Elongation, 
                polygon.Orientation
            });
        }

        public int Index
        {
            get { return this._index; }
        }

        public Polygon Polygon
        {
            get { return this._polygon; }
        }

        public int Class
        {
            get { return this._class; }
            set { Interlocked.Exchange(ref this._class, value); }
        }

        public bool Kernel
        {
            get { return this._isKernel == 1; }
            set { Interlocked.Exchange(ref this._isKernel, value ? 1 : 0); }
        }

        public IReadOnlyList<double> Criterion { get; private set; }

        public ClassificationUnit WithDistanceDalculator(IDistanceCalculator<ClassificationUnit> distanceCalculator)
        {
            this._distanceCalculator = distanceCalculator;
            return this;
        }

        public int DetermineClass(IList<Kernel> kernels)
        {
            IList<double> distances = new List<double>(kernels.Count);
            if (this.Kernel == false)
            {
                foreach (var kernel in kernels)
                {
                    distances.Add(this._distanceCalculator.Calculate(this, kernel.Unit));
                }

                double minDistance = double.MaxValue;
                int index = 0;

                for (int i = 0; i < distances.Count; ++i)
                {
                    if (distances[i] < minDistance)
                    {
                        minDistance = distances[i];
                        index = i;
                    }
                }

                 this.Class = kernels[index].Unit.Class;
            }

            return this.Class;
        }

        public double CalculateDistance(double[] criterion)
        {
            return this._distanceCalculator.CalculateDouble(this.Criterion.ToArray(), criterion);
        }
    }
}
