using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier.Core
{
    public interface IImageProcessor
    {
        void ApplyBinarization(IImageBinarizer imageBinarizer);

        IList<Polygon> ApplyLabeling(ILabelingService labelingService);

        void Save(string filePath);
    }
}
