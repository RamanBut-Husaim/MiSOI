using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Classifier.Core;

namespace Classifier.Runner
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            string filePath = "img5.jpg";
            string outputFile = "result.jpg";
            int classNumber = 6;

            IImageBinarizer imageBinarizer = new MinimumErrorThresholder();
            IImageProcessor imageProcessor = new ImageProcessor(filePath);
            ILabelingService labelingService = new IterativeLabelingService();
            IDistanceCalculator<ClassificationUnit> euclidian = new EuclideanDistanceCalculator<ClassificationUnit>();
            var classificationService = new ClassificationService(imageBinarizer, imageProcessor, labelingService, euclidian, classNumber);
            classificationService.Classify();
            classificationService.Save(outputFile);
        }

        private static void TestPolygon1()
        {
            var polygon = new Polygon(1);

            polygon.Pixels.Add(new Pixel(0, 6, 10));

            polygon.Pixels.Add(new Pixel(1, 5, 10));
            polygon.Pixels.Add(new Pixel(1, 6, 10));
            polygon.Pixels.Add(new Pixel(1, 7, 10));

            polygon.Pixels.Add(new Pixel(2, 2, 10));
            polygon.Pixels.Add(new Pixel(2, 3, 10));
            polygon.Pixels.Add(new Pixel(2, 4, 10));
            polygon.Pixels.Add(new Pixel(2, 5, 10));
            polygon.Pixels.Add(new Pixel(2, 6, 10));
            polygon.Pixels.Add(new Pixel(2, 7, 10));
            polygon.Pixels.Add(new Pixel(2, 8, 10));
            polygon.Pixels.Add(new Pixel(2, 9, 10));
            polygon.Pixels.Add(new Pixel(2, 10, 10));

            polygon.Pixels.Add(new Pixel(3, 2, 10));
            polygon.Pixels.Add(new Pixel(3, 3, 10));
            polygon.Pixels.Add(new Pixel(3, 4, 10));
            polygon.Pixels.Add(new Pixel(3, 5, 10));
            polygon.Pixels.Add(new Pixel(3, 6, 10));
            polygon.Pixels.Add(new Pixel(3, 7, 10));
            polygon.Pixels.Add(new Pixel(3, 8, 10));
            polygon.Pixels.Add(new Pixel(3, 9, 10));
            polygon.Pixels.Add(new Pixel(3, 10, 10));
            polygon.Pixels.Add(new Pixel(3, 11, 10));
            polygon.Pixels.Add(new Pixel(3, 12, 10));
            polygon.Pixels.Add(new Pixel(3, 13, 10));

            polygon.Pixels.Add(new Pixel(4, 1, 10));
            polygon.Pixels.Add(new Pixel(4, 2, 10));
            polygon.Pixels.Add(new Pixel(4, 3, 10));
            polygon.Pixels.Add(new Pixel(4, 4, 10));
            polygon.Pixels.Add(new Pixel(4, 5, 10));
            polygon.Pixels.Add(new Pixel(4, 6, 10));
            polygon.Pixels.Add(new Pixel(4, 7, 10));
            polygon.Pixels.Add(new Pixel(4, 8, 10));
            polygon.Pixels.Add(new Pixel(4, 9, 10));
            polygon.Pixels.Add(new Pixel(4, 10, 10));
            polygon.Pixels.Add(new Pixel(4, 11, 10));
            polygon.Pixels.Add(new Pixel(4, 12, 10));
            polygon.Pixels.Add(new Pixel(4, 13, 10));
            polygon.Pixels.Add(new Pixel(4, 14, 10));

            polygon.Pixels.Add(new Pixel(5, 3, 10));
            polygon.Pixels.Add(new Pixel(5, 4, 10));
            polygon.Pixels.Add(new Pixel(5, 6, 10));
            polygon.Pixels.Add(new Pixel(5, 7, 10));
            polygon.Pixels.Add(new Pixel(5, 8, 10));
            polygon.Pixels.Add(new Pixel(5, 9, 10));
            polygon.Pixels.Add(new Pixel(5, 10, 10));
            polygon.Pixels.Add(new Pixel(5, 11, 10));
            polygon.Pixels.Add(new Pixel(5, 12, 10));
            polygon.Pixels.Add(new Pixel(5, 13, 10));
            polygon.Pixels.Add(new Pixel(5, 14, 10));
            polygon.Pixels.Add(new Pixel(5, 15, 10));

            polygon.Pixels.Add(new Pixel(6, 4, 10));
            polygon.Pixels.Add(new Pixel(6, 6, 10));
            polygon.Pixels.Add(new Pixel(6, 8, 10));
            polygon.Pixels.Add(new Pixel(6, 10, 10));

            polygon.Pixels.Add(new Pixel(7, 6, 10));

            polygon.Pixels.Add(new Pixel(8, 6, 10));

            Debug.Assert(polygon.Perimeter == 33, "Polygon 1 fails.");
        }

        private static void TestPolygon2()
        {
            var polygon = new Polygon(1);

            polygon.Pixels.Add(new Pixel(0, 11, 10));

            polygon.Pixels.Add(new Pixel(1, 9, 10));
            polygon.Pixels.Add(new Pixel(1, 11, 10));

            polygon.Pixels.Add(new Pixel(2, 9, 10));
            polygon.Pixels.Add(new Pixel(2, 11, 10));
            polygon.Pixels.Add(new Pixel(2, 13, 10));

            polygon.Pixels.Add(new Pixel(3, 7, 10));
            polygon.Pixels.Add(new Pixel(3, 9, 10));
            polygon.Pixels.Add(new Pixel(3, 11, 10));
            polygon.Pixels.Add(new Pixel(3, 13, 10));

            polygon.Pixels.Add(new Pixel(4, 6, 10));
            polygon.Pixels.Add(new Pixel(4, 7, 10));
            polygon.Pixels.Add(new Pixel(4, 8, 10));
            polygon.Pixels.Add(new Pixel(4, 9, 10));
            polygon.Pixels.Add(new Pixel(4, 10, 10));
            polygon.Pixels.Add(new Pixel(4, 11, 10));
            polygon.Pixels.Add(new Pixel(4, 12, 10));
            polygon.Pixels.Add(new Pixel(4, 13, 10));

            polygon.Pixels.Add(new Pixel(5, 1, 10));
            polygon.Pixels.Add(new Pixel(5, 2, 10));
            polygon.Pixels.Add(new Pixel(5, 3, 10));
            polygon.Pixels.Add(new Pixel(5, 4, 10));
            polygon.Pixels.Add(new Pixel(5, 5, 10));
            polygon.Pixels.Add(new Pixel(5, 6, 10));
            polygon.Pixels.Add(new Pixel(5, 7, 10));
            polygon.Pixels.Add(new Pixel(5, 8, 10));
            polygon.Pixels.Add(new Pixel(5, 9, 10));
            polygon.Pixels.Add(new Pixel(5, 10, 10));
            polygon.Pixels.Add(new Pixel(5, 11, 10));
            polygon.Pixels.Add(new Pixel(5, 12, 10));
            polygon.Pixels.Add(new Pixel(5, 13, 10));
            polygon.Pixels.Add(new Pixel(5, 14, 10));
            polygon.Pixels.Add(new Pixel(5, 15, 10));

            polygon.Pixels.Add(new Pixel(6, 5, 10));
            polygon.Pixels.Add(new Pixel(6, 6, 10));
            polygon.Pixels.Add(new Pixel(6, 7, 10));
            polygon.Pixels.Add(new Pixel(6, 8, 10));
            polygon.Pixels.Add(new Pixel(6, 9, 10));
            polygon.Pixels.Add(new Pixel(6, 10, 10));
            polygon.Pixels.Add(new Pixel(6, 11, 10));
            polygon.Pixels.Add(new Pixel(6, 12, 10));
            polygon.Pixels.Add(new Pixel(6, 13, 10));

            polygon.Pixels.Add(new Pixel(7, 2, 10));
            polygon.Pixels.Add(new Pixel(7, 3, 10));
            polygon.Pixels.Add(new Pixel(7, 4, 10));
            polygon.Pixels.Add(new Pixel(7, 5, 10));
            polygon.Pixels.Add(new Pixel(7, 6, 10));
            polygon.Pixels.Add(new Pixel(7, 7, 10));
            polygon.Pixels.Add(new Pixel(7, 8, 10));
            polygon.Pixels.Add(new Pixel(7, 9, 10));
            polygon.Pixels.Add(new Pixel(7, 10, 10));
            polygon.Pixels.Add(new Pixel(7, 11, 10));
            polygon.Pixels.Add(new Pixel(7, 12, 10));
            polygon.Pixels.Add(new Pixel(7, 13, 10));
            polygon.Pixels.Add(new Pixel(7, 14, 10));
            polygon.Pixels.Add(new Pixel(7, 15, 10));
            polygon.Pixels.Add(new Pixel(7, 16, 10));
            polygon.Pixels.Add(new Pixel(7, 17, 10));
            polygon.Pixels.Add(new Pixel(7, 18, 10));
            polygon.Pixels.Add(new Pixel(7, 19, 10));

            polygon.Pixels.Add(new Pixel(8, 5, 10));
            polygon.Pixels.Add(new Pixel(8, 6, 10));
            polygon.Pixels.Add(new Pixel(8, 7, 10));
            polygon.Pixels.Add(new Pixel(8, 9, 10));
            polygon.Pixels.Add(new Pixel(8, 11, 10));

            polygon.Pixels.Add(new Pixel(9, 3, 10));
            polygon.Pixels.Add(new Pixel(9, 4, 10));
            polygon.Pixels.Add(new Pixel(9, 5, 10));
            polygon.Pixels.Add(new Pixel(9, 6, 10));
            polygon.Pixels.Add(new Pixel(9, 7, 10));
            polygon.Pixels.Add(new Pixel(9, 11, 10));

            polygon.Pixels.Add(new Pixel(10, 7, 10));

            polygon.Pixels.Add(new Pixel(11, 7, 10));

            polygon.Pixels.Add(new Pixel(12, 7, 10));

            Debug.Assert(polygon.Perimeter == 50, "Polygon 2 fails.");
        }
    }
}
