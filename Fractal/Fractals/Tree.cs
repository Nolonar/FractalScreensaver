using System;
using System.Numerics;
using System.Threading.Tasks;

namespace FractalScreenSaver.Fractals
{
    internal class Tree : IFractal
    {
        public int EdgeCount => vertices.Length - 1;
        public Vector2[] Vertices => vertices;

        private Vector2 boundaryMin = Vector2.Zero, boundaryMax = Vector2.Zero;

        private readonly int width;
        private readonly int height;
        private Vector2 center;

        private readonly float bumpFactor;
        private readonly bool isInvertedBump;

        protected Vector2[] vertices;

        public Tree((int width, int height) dimensions)
        {
            width = dimensions.width;
            height = dimensions.height;
            center = new Vector2(width, height) / 2;

            float minBumpLength = (float)Screensaver.Settings.MinBumpLength;
            float maxBumpLength = (float)Screensaver.Settings.MaxBumpLength;
            bumpFactor = (float)Screensaver.Random.NextDouble() * (maxBumpLength - minBumpLength) + minBumpLength;
            isInvertedBump = Screensaver.Random.NextBool();

            vertices = new[] { GetFirstPoint(), GetSecondPoint() };
        }

        private Vector2 GetFirstPoint() => new((float)Screensaver.Random.NextDouble() * width / 10, Screensaver.Random.Next(0, height));
        private Vector2 GetSecondPoint() => GetFirstPoint() + new Vector2((float)width * 9 / 10, 0);

        public void IncreaseFractalDepth()
        {
            var result = new Vector2[EdgeCount * 4 + 1];

            Parallel.For(0, EdgeCount, i => SplitLine(result, i));
            result[^1] = vertices[EdgeCount];

            if (Screensaver.Settings.KeepInViewport)
                FitToViewport(result);

            vertices = result;
        }

        private void SplitLine(Vector2[] destinationArray, int i)
        {
            Vector2 start = vertices[i], end = vertices[i + 1];
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
    }
}
