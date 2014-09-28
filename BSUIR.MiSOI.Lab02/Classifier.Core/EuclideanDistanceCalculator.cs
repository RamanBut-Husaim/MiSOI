using System;
using System.Linq;

namespace Classifier.Core
{
    public class EuclideanDistanceCalculator<T> : IDistanceCalculator<T> where T : IClassifiable
    {
        //Magic experimental numbers.
        private static double[] WeightCoefficients = { -0.25d, -0.1d, 0.2d, 0.2d, 0.0d };

        public double Calculate(T first, T second)
        {
            double result = first.Criterion.Select((t1, i) => t1 - second.Criterion[i] + (Math.Max(t1, second.Criterion[i]) * WeightCoefficients[i])).Sum(t => t * t);
            return Math.Sqrt(result);
        }

        public double CalculateDouble(double[] first, double[] second)
        {
            return first.Select((t1, i) => t1 - second[i] + (Math.Max(t1, second[i]) * WeightCoefficients[i])).Sum(t => t * t);
        }
    }
}
