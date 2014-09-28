using System.Collections.Generic;

namespace Classifier.Core
{
    public interface IImageProcessor
    {
        void ApplyBinarization(IImageBinarizer imageBinarizer);

        IList<Polygon> ApplyLabeling(ILabelingService labelingService);

        void Save(string filePath);

        void Save(string filePath, IList<Polygon> polygons);
    }
}
