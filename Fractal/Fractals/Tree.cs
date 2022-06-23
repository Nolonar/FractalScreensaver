using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace FractalScreenSaver.Fractals
{
    internal class Tree : IFractal
    {
        public int EdgeCount => Vertices.Length - 1;

        protected readonly Random random = new();
        private Vector2 boundaryMin = Vector2.Zero, boundaryMax = Vector2.Zero;

        private readonly int width;
        private readonly int height;
        private Vector2 center;

        private readonly int singleColorHue;
        private readonly float bumpFactor;
        private readonly bool isInvertedBump;

        protected Vector2[] Vertices;

        public Tree((int width, int height) dimensions)
        {
            width = dimensions.width;
            height = dimensions.height;
            center = new Vector2(width, height) / 2;
            if (Screensaver.Settings.IsRainbow == false)
                singleColorHue = GetHueFromFactor((float)random.NextDouble());

            float minBumpLength = (float)Screensaver.Settings.MinBumpLength;
            float maxBumpLength = (float)Screensaver.Settings.MaxBumpLength;
            bumpFactor = (float)random.NextDouble() * (maxBumpLength - minBumpLength) + minBumpLength;
            isInvertedBump = random.NextBool();

            Vertices = new[] { GetFirstPoint(), GetSecondPoint() };
        }

        private Vector2 GetFirstPoint() => new((float)random.NextDouble() * width / 10, random.Next(0, height));
        private Vector2 GetSecondPoint() => GetFirstPoint() + new Vector2((float)width * 9 / 10, 0);

        public void IncreaseFractalDepth()
        {
            var result = new Vector2[EdgeCount * 4 + 1];

            Parallel.For(0, EdgeCount, i => SplitLine(result, i));
            result[^1] = Vertices[EdgeCount];

            if (Screensaver.Settings.KeepInViewport)
                FitToViewport(result);

            Vertices = result;
        }

        private void SplitLine(Vector2[] destinationArray, int i)
        {
            Vector2 start = Vertices[i], end = Vertices[i + 1];
            int newIndex = i * 4; // Each line receives 3 additional points.
            destinationArray[newIndex] = start;

            Vector2 lineVector = end - start;
            var a = lineVector / 3; // Break at 1/3...
            var b = a * 2; // ... and 2/3 of the line.
            var norm = isInvertedBump ? a.Cross(b) : b.Cross(a); // Get the normal.
            norm = norm * bumpFactor + lineVector / 2; // Put normal at 1/2 of the line.

            // Move vectors to origin.
            a += start;
            b += start;
            norm += start;

            destinationArray[newIndex + 1] = a;
            destinationArray[newIndex + 2] = norm;
            destinationArray[newIndex + 3] = b;

            AdjustBoundary(a);
            AdjustBoundary(norm);
            AdjustBoundary(b);
        }

        public static int GetHueFromFactor(float factor) => (int)(FractalForm.HlsMaxValue * factor);

        protected void FitToViewport(Vector2[] vertices)
        {
            Vector2 boundaryMove = (boundaryMin - boundaryMax) / 2;
            Vector2 resize = ((boundaryMin + boundaryMax) / 2) / center;
            float resizeFactor = Math.Max(resize.X, resize.Y) + 1;

            Parallel.For(0, vertices.Length, i =>
                vertices[i] = (vertices[i] + boundaryMove - center) / resizeFactor + center
            );

            boundaryMin = Vector2.Zero;
            boundaryMax = Vector2.Zero;
        }

        protected void AdjustBoundary(Vector2 point)
        {
            int offset = 10;
            int offsetW = width - offset;
            int offsetH = height - offset;

            if (point.X < offset && -point.X > boundaryMin.X)
                boundaryMin.X = offset - point.X;
            else if (point.X > offsetW && point.X - offsetW > boundaryMax.X)
                boundaryMax.X = point.X - offsetW;

            if (point.Y < offset && -point.Y > boundaryMin.Y)
                boundaryMin.Y = offset - point.Y;
            else if (point.Y > offsetH && point.Y - offsetH > boundaryMax.Y)
                boundaryMax.Y = point.Y - offsetH;
        }

        public IEnumerable<(int hue, Vector2[] vertices)> GetColoredPolyline()
        {
            if (Screensaver.Settings.IsRainbow == false)
            {
                yield return (singleColorHue, Vertices);
                yield break;
            }

            int previousHue = 0, groupStart = 0;
            for (int i = 1; i < Vertices.Length; i++)
            {
                int hue = GetHueFromFactor(i / (float)Vertices.Length);
                if (hue == previousHue)
                    continue;

                yield return (previousHue, Vertices[groupStart..(i + 1)]);
                previousHue = hue;
                groupStart = i;
            }

            if (groupStart != Vertices.Length - 1) // In case there's a group at the end which we haven't yielded yet.
                yield return (previousHue, Vertices[groupStart..Vertices.Length]);
        }
    }
}
