using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier.Core
{
    public interface IDistanceCalculator
    {
        double CalculateDouble(double[] first, double[] second);
    }

    public interface IDistanceCalculator<in T> : IDistanceCalculator where T : IClassifiable
    {
        double Calculate(T first, T second);
    }
}
