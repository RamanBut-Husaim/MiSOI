using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier.Core
{
    public interface ILabelingService
    {
        IList<Polygon> Process(ImageWrapper image);
    }
}
