using System;
using System.Collections.Generic;

namespace Classifier.Core
{
    public sealed class ClassificationService : IDisposable
    {
        private readonly IImageBinarizer _imageBinarizer;
        private readonly IImageProcessor _imageProcessor;
        private readonly ILabelingService _labelingService;
        private readonly IList<Polygon> _polygons; 
        private bool _disposed;

        public ClassificationService(
            IImageBinarizer imageBinarizer, 
            IImageProcessor imageProcessor,
            ILabelingService labelingService)
        {
            this._imageBinarizer = imageBinarizer;
            this._imageProcessor = imageProcessor;
            this._labelingService = labelingService;
            this._polygons = new List<Polygon>();
        }

        public void Classify()
        {
            this._imageProcessor.ApplyBinarization(this._imageBinarizer);
            IList<Polygon> polygons = this._imageProcessor.ApplyLabeling(this._labelingService);
            this.CopyPolygons(polygons);
        }

        private void CopyPolygons(IEnumerable<Polygon> polygons)
        {
            this._polygons.Clear();

            foreach (var polygon in polygons)
            {
                this._polygons.Add(polygon);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public void Save(string fileName)
        {
            this._imageProcessor.Save(fileName);
        }

        private void Dispose(bool disposing)
        {
            if (this._disposed == false)
            {
                if (disposing)
                {
                    ((IDisposable)this._imageProcessor).Dispose();
                    this._disposed = true;
                }
            }
        }
    }
}
