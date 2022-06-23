using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace FractalScreenSaver.Fractals
{
    internal class Tree : IFractal
    {
        public int EdgeCount => Vertices.Length - 1;

        protected readonly Random random = new();
        private PointF boundaryMin, boundaryMax;

        private readonly int width;
        private readonly int height;
        private PointF center;

        private readonly int singleColorHue;
        private readonly float bumpFactor;
        private readonly bool isInvertedBump;

        protected PointF[] Vertices;

        public Tree(Rectangle clientRectangle)
        {
            width = clientRectangle.Width;
            height = clientRectangle.Height;
            center = new PointF(width, height).Div(2);
            if (Screensaver.Settings.IsRainbow == false)
                singleColorHue = GetHueFromFactor((float)random.NextDouble());

            float minBumpLength = (float)Screensaver.Settings.MinBumpLength;
            float maxBumpLength = (float)Screensaver.Settings.MaxBumpLength;
            bumpFactor = (float)random.NextDouble() * (maxBumpLength - minBumpLength) + minBumpLength;
            isInvertedBump = random.NextBool();

            Vertices = new[] { GetFirstPoint(), GetSecondPoint() };
        }

        private PointF GetFirstPoint() => new((float)random.NextDouble() * width / 10, random.Next(0, height));
        private PointF GetSecondPoint() => GetFirstPoint().Add(new PointF((float)width * 9 / 10, 0));

        public void IncreaseFractalDepth()
        {
            var result = new PointF[EdgeCount * 4 + 1];

            Parallel.For(0, EdgeCount, i => SplitLine(result, i));
            result[^1] = Vertices[EdgeCount];

            if (Screensaver.Settings.KeepInViewport)
                FitToViewport(result);

            Vertices = result;
        }

        private void SplitLine(PointF[] destinationArray, int i)
        {
            PointF start = Vertices[i], end = Vertices[i + 1];
            int newIndex = i * 4; // Each line receives 3 additional points.
            destinationArray[newIndex] = start;

            PointF lineVector = end.Sub(start);
            var a = lineVector.Div(3); // Break at 1/3...
            var b = a.Mult(2); // ... and 2/3 of the line.
            var norm = isInvertedBump ? a.Norm(b) : b.Norm(a); // Get the normal.
            norm = norm.Mult(bumpFactor).Add(lineVector.Div(2)); // Put normal at 1/2 of the line.

            // Move vectors to origin.
            a = a.Add(start);
            b = b.Add(start);
            norm = norm.Add(start);

            destinationArray[newIndex + 1] = a;
            destinationArray[newIndex + 2] = norm;
            destinationArray[newIndex + 3] = b;

            AdjustBoundary(a);
            AdjustBoundary(norm);
            AdjustBoundary(b);
        }

        public static int GetHueFromFactor(float factor) => (int)(FractalForm.HlsMaxValue * factor);

        protected void FitToViewport(PointF[] vertices)
        {
            PointF boundaryMove = boundaryMin.Sub(boundaryMax).Div(2);
            PointF resize = boundaryMin.Add(boundaryMax).Div(2).Div(center);
            float resizeFactor = Math.Max(resize.X, resize.Y) + 1;

            Parallel.For(0, vertices.Length, i =>
                vertices[i] = vertices[i].Add(boundaryMove).Sub(center).Div(resizeFactor).Add(center)
            );

            boundaryMin = PointF.Empty;
            boundaryMax = PointF.Empty;
        }

        protected void AdjustBoundary(PointF point)
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

        public IEnumerable<ColoredPolyline> GetColoredPolyline()
        {
            if (Screensaver.Settings.IsRainbow == false)
            {
                yield return new ColoredPolyline(singleColorHue, Vertices);
                yield break;
            }

            int previousHue = 0, groupStart = 0;
            for (int i = 1; i < Vertices.Length; i++)
            {
                int hue = GetHueFromFactor(i / (float)Vertices.Length);
                if (hue == previousHue)
                    continue;

                yield return new ColoredPolyline(previousHue, Vertices[groupStart..(i + 1)]);
                previousHue = hue;
                groupStart = i;
            }

            if (groupStart != Vertices.Length - 1) // In case there's a group at the end which we haven't yielded yet.
                yield return new ColoredPolyline(previousHue, Vertices[groupStart..Vertices.Length]);
        }
    }
}
