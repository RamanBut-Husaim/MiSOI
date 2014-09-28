using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Classifier.Core
{
    public sealed class ClassificationService : IDisposable
    {
        private static Color[] Colors = new[]
        {
            Color.Aqua, 
            Color.Blue, 
            Color.BlueViolet, 
            Color.Red, 
            Color.OrangeRed, 
            Color.MediumVioletRed, 
            Color.LawnGreen, 
            Color.Green, 
            Color.Coral,
            Color.Yellow
        };

        private readonly IImageBinarizer _imageBinarizer;
        private readonly IImageProcessor _imageProcessor;
        private readonly ILabelingService _labelingService;
        private readonly IList<Polygon> _polygons;
        private readonly IDistanceCalculator<ClassificationUnit> _distanceCalculator;
        private readonly IList<ClassificationUnit> _classificationUnits;
        private readonly IList<Kernel> _kernels; 
        private readonly RNGCryptoServiceProvider _random;
        private readonly int _classNumber;
        private bool _disposed;

        public ClassificationService(
            IImageBinarizer imageBinarizer, 
            IImageProcessor imageProcessor,
            ILabelingService labelingService,
            IDistanceCalculator<ClassificationUnit> distanceCalculator,
            int classNumber)
        {
            this._imageBinarizer = imageBinarizer;
            this._imageProcessor = imageProcessor;
            this._labelingService = labelingService;
            this._polygons = new List<Polygon>();
            this._classificationUnits = new List<ClassificationUnit>();
            this._distanceCalculator = distanceCalculator;
            this._classNumber = classNumber;
            this._random = new RNGCryptoServiceProvider();
            this._kernels = new List<Kernel>(this._classNumber);
        }

        public void Classify()
        {
            this._imageProcessor.ApplyBinarization(this._imageBinarizer);
            IList<Polygon> polygons = this._imageProcessor.ApplyLabeling(this._labelingService);
            this.CopyPolygons(polygons);
            this.SelectKernels();
            this.PerformClusterization();
            this.ColorizePolygons();
        }

        private void PerformClusterization()
        {
            int kernelChanged = 0;

            do
            {
                kernelChanged = 0;
                Parallel.ForEach(this._classificationUnits.Where(p => p.Kernel == false), (elem) => elem.DetermineClass(this._kernels));
                Parallel.ForEach(
                    this._classificationUnits.GroupBy(p => p.Class)
                        .Select(p => new { Class = p.Key, Elements = p.ToList(), Kernel = this._kernels.FirstOrDefault(s => s.Unit.Class == p.Key) }),
                    (@class, state) =>
                    {
                        IList<ClassificationUnit> units = @class.Elements;
                        double[] medianVector = this.CalculateMedianVector(units).ToArray();
                        int minIndex = units.Select(p => p.CalculateDistance(medianVector.ToArray())).ToList().MinIndex();
                        int kernelIndex = units.KernelIndex();
                        if (minIndex != kernelIndex)
                        {
                            @class.Kernel.Unit.Kernel = false;
                            units[minIndex].Kernel = true;
                            @class.Kernel.Unit = units[minIndex];
                            Interlocked.Increment(ref kernelChanged);
                        }
                    });
            }
            while (kernelChanged != 0);
        }

        private IList<double> CalculateMedianVector(IList<ClassificationUnit> units)
        {
            var criteriaCount = units[0].Criterion.Count;
            IList<double> result = new List<double>(criteriaCount);

            for (int i = 0; i < criteriaCount; ++i)
            {
                result.Add(this.CalculateMedian(units, i));
            }

            return result;
        }

        private double CalculateMedian(IList<ClassificationUnit> unit, int criteriaIndex)
        {
            double result;
            double[] features = unit.Select(p => p.Criterion[criteriaIndex]).OrderBy(p => p).ToArray();
            result = features.Length % 2 == 0
                         ? (features[(features.Length / 2) - 1] + features[features.Length / 2]) / 2
                         : features[((features.Length + 1) / 2) - 1];
            return result;
        }

        private void SelectKernels()
        {
            IList<int> kernels = this.GetKernelIndices();
            for (int i = 0; i < kernels.Count; ++i)
            {
                this._classificationUnits[kernels[i]].Kernel = true;
                this._classificationUnits[kernels[i]].Class = i;
                this._kernels.Add(new Kernel { Unit = this._classificationUnits[kernels[i]], Changed = false });
            }
        }

        private IList<int> GetKernelIndices()
        {
            IList<int> kernels = new List<int>(this._classNumber);
            bool completed = false;
            while (completed == false)
            {
                for (int i = 0; i < this._classNumber; ++i)
                {
                    var buffer = new byte[sizeof(int)];
                    this._random.GetNonZeroBytes(buffer);
                    kernels.Add(Math.Abs(buffer.ToInt()) % this._classNumber);
                }

                completed = kernels.Count == kernels.Distinct().Count();
                if (completed == false)
                {
                    kernels.Clear();
                }
            }

            return kernels;
        }

        private void ColorizePolygons()
        {
            foreach (var @class in this._classificationUnits.GroupBy(p => p.Class))
            {
                foreach (var unit in @class)
                {
                    unit.Polygon.Color = Colors[@class.Key];
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public void Save(string fileName)
        {
            //this._imageProcessor.Save(fileName);
            this._imageProcessor.Save(fileName, this._polygons);
        }

        private void CopyPolygons(IEnumerable<Polygon> polygons)
        {
            this._polygons.Clear();
            this._classificationUnits.Clear();

            foreach (var polygon in polygons)
            {
                this._polygons.Add(polygon);
                this._classificationUnits.Add(new ClassificationUnit(polygon).WithDistanceDalculator(this._distanceCalculator));
            }
        }

        private void Dispose(bool disposing)
        {
            if (this._disposed == false)
            {
                if (disposing)
                {
                    ((IDisposable)this._imageProcessor).Dispose();
                    this._random.Dispose();
                    this._disposed = true;
                }
            }
        }
    }
}
