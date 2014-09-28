using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier.Core
{
    public interface IClassifiable
    {
        int Class { get; set; }

        IReadOnlyList<double> Criterion { get; } 
    }
}
