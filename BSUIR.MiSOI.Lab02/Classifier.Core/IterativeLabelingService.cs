using System;
using System.Collections.Generic;
using System.Linq;

namespace Classifier.Core
{
    public sealed class IterativeLabelingService : ILabelingService
    {
        private readonly IList<Tuple<int, int>> _equalityClasses;
        private readonly IList<HashSet<int>> _equalityChains;
        private readonly IDictionary<int, Polygon> _polygons;
        private int _areaCount;
        private int _objectColor;
        private int[][] _areaMatrix;

        public IterativeLabelingService()
        {
            this._equalityClasses = new List<Tuple<int, int>>();
            this._equalityChains = new List<HashSet<int>>();
            this._polygons = new Dictionary<int, Polygon>();
        }

        public IList<Polygon> Process(ImageWrapper image)
        {
            this.InitializeAreaMatrix(image);
            this.InitializeObjectColor(image);
            this.PerformLabeling(image);
            this.ResolveEqualityChains();
            this.LinkPolygons();
            this.FillPolygons(image);

            IList<Polygon> result = this.GetPolygons();

            this._equalityClasses.Clear();
            this._equalityChains.Clear();
            this._polygons.Clear();

            return result;
        }

        private void InitializeAreaMatrix(ImageWrapper image)
        {
            this._areaMatrix = new int[image.Height][];
            for (int i = 0; i < image.Height; ++i)
            {
                this._areaMatrix[i] = new int[image.Width];
            }
        }

        private void InitializeObjectColor(ImageWrapper imageWrapper)
        {
            int blackPixels = imageWrapper.GetColoredPixels(0);
            int whitePixels = imageWrapper.GetColoredPixels(byte.MaxValue);
            this._objectColor = Math.Min(blackPixels, whitePixels) == blackPixels ? 0 : byte.MaxValue;
        }

        private void FillPolygons(ImageWrapper image)
        {
            for (int i = 0; i < image.Height; ++i)
            {
                for (int j = 0; j < image.Width; ++j)
                {
                    if (this._areaMatrix[i][j] != 0)
                    {
                        this._polygons[this._areaMatrix[i][j]].Pixels.Add(new Pixel(i, j, image[i, j]));
                    }
                }
            }
        }

        private IList<Polygon> GetPolygons()
        {
            return this._polygons.Values.GroupBy(p => p.Index).Select(p => p.First()).Where(p => p.Pixels.Count > 0).ToList();
        }

        private void LinkPolygons()
        {
            int polygonIndex = 0;
            for (int i = 1; i <= this._areaCount; ++i)
            {
                if (this._polygons.ContainsKey(i) == false)
                {
                    var polygon = new Polygon(polygonIndex++);
                    foreach (HashSet<int> equalityChain in this._equalityChains)
                    {
                        if (equalityChain.Contains(i))
                        {
                            foreach (int equalityClass in equalityChain)
                            {
                                this._polygons.Add(equalityClass, polygon);
                            }
                        }
                    }

                    if (this._polygons.ContainsKey(i) == false)
                    {
                        this._polygons.Add(i, polygon);
                    }
                }
            }
        }

        private void PerformLabeling(ImageWrapper image)
        {
            this._areaCount = 1;
            for (int i = 1; i < image.Height; ++i)
            {
                for (int j = 1; j < image.Width; ++j)
                {
                    if (image[i, j] == this._objectColor)
                    {
                        int upperNeighbor = this._areaMatrix[i - 1][j];
                        int leftNeighbor = this._areaMatrix[i][j - 1];
                        if (upperNeighbor == 0 && leftNeighbor == 0)
                        {
                            this._areaMatrix[i][j] = this._areaCount++;
                            continue;
                        }

                        if (upperNeighbor != 0 && leftNeighbor == 0)
                        {
                            this._areaMatrix[i][j] = upperNeighbor;
                            continue;
                        }

                        if (upperNeighbor == 0 && leftNeighbor != 0)
                        {
                            this._areaMatrix[i][j] = leftNeighbor;
                            continue;
                        }

                        if (upperNeighbor != 0 && leftNeighbor != 0 && upperNeighbor == leftNeighbor)
                        {
                            this._areaMatrix[i][j] = upperNeighbor;
                            continue;
                        }

                        this._areaMatrix[i][j] = upperNeighbor;
                        this._equalityClasses.Add(new Tuple<int, int>(upperNeighbor, leftNeighbor));
                    }
                }
            }
        }

        private void ResolveEqualityChains()
        {
            var buffer = new List<Tuple<int, int>>();
            buffer.AddRange(this._equalityClasses);

            do
            {
                Tuple<int, int> currentClass = buffer[0];
                this._equalityChains.Add(new HashSet<int>());
                int index = this._equalityChains.Count - 1;
                this._equalityChains[index].Add(currentClass.Item1);
                this._equalityChains[index].Add(currentClass.Item2);

                bool filled;
                do
                {
                    var temp = new List<Tuple<int, int>>();
                    filled = true;

                    for (int j = 1; j < buffer.Count; ++j)
                    {
                        Tuple<int, int> nextClass = buffer[j];
                        if (this._equalityChains[index].Contains(nextClass.Item1) || this._equalityChains[index].Contains(nextClass.Item2))
                        {
                            this._equalityChains[index].Add(nextClass.Item1);
                            this._equalityChains[index].Add(nextClass.Item2);
                            filled = false;
                        }
                        else
                        {
                            temp.Add(nextClass);
                        }
                    }

                    buffer.Clear();
                    buffer.AddRange(temp);
                }
                while (filled == false && buffer.Count > 0);
            }
            while (buffer.Count > 0);
        }
    }
}
