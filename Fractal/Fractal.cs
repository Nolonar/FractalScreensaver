using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace FractalScreenSaver
{
    class Fractal
    {
        public enum Type
        {
            Tree,
            Snowflake
        }

        public PointF[] Vertices { get; private set; }
        public int EdgeCount { get { return Vertices.Length - 1; } }

        private readonly Random random = new Random();
        private PointF boundaryMin, boundaryMax;

        private readonly int width;
        private readonly int height;
        private PointF center;

        private readonly int singleColorHue;
        private readonly float bumpFactor;
        private readonly bool isInvertedBump;

        public Fractal(Rectangle clientRectangle)
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

            Initialize((Type)Screensaver.Settings.FractalType);
        }

        private void Initialize(Type type)
        {
            Vertices = new PointF[GetEdgeCount(type) + 1];
            Vertices[0] = GetFirstPoint();
            Vertices[1] = GetSecondPoint();

            switch (type)
            {
                case Type.Tree:
                    InitializeTree();
                    break;

                case Type.Snowflake:
                    InitializeSnowflake();
                    break;
            }

            if (Screensaver.Settings.KeepInViewport)
                FitToViewport(Vertices);
        }

        private int GetEdgeCount(Type type)
        {
            if (type == Type.Tree)
                return 1;

            return Screensaver.Settings.IsRandomCount ?
                random.Next(GetMinLineCount(type), 100) :
                Screensaver.Settings.EdgeCount;
        }

        private int GetMinLineCount(Type type)
        {
            return type switch
            {
                Type.Snowflake => 2,
                _ => 1,
            };
        }

        private PointF GetFirstPoint()
        {
            return new PointF((float)random.NextDouble() * width / 10, random.Next(0, height));
        }

        private PointF GetSecondPoint()
        {
            PointF basePoint = GetFirstPoint();
            return new PointF(width - basePoint.X, basePoint.Y);
        }

        private void InitializeTree()
        {
            for (int i = 2; i < Vertices.Length; i++)
                Vertices[i] = new PointF(random.Next(0, width), random.Next(0, height));
        }

        private void InitializeSnowflake()
        {
            int edges = EdgeCount;
            bool isConcave = IsConcavePolygonPossible(edges) && random.NextBool(1 - 1 / (edges - 3));
            double deg = isConcave
                ? GetConcavePolygonDegree(edges, GetRandomDensityForConcavePolygon(edges))
                : GetConvexPolygonDegree(edges);

            double rad = deg.ToRadians();
            double theta = rad;
            PointF currentLine = Vertices[1].Sub(Vertices[0]);
            if (currentLine.X == 0)
                theta += Math.PI / 2;
            else
                theta += Math.Atan(currentLine.Y / currentLine.X);

            for (int i = 2; i < Vertices.Length; i++)
            {
                PointF previous = Vertices[i - 1];
                PointF a = previous.Sub(Vertices[i - 2]);
                double length = Math.Sqrt(a.X * a.X + a.Y * a.Y);
                a.X = (float)(length * Math.Cos(theta));
                a.Y = (float)(length * Math.Sin(theta));
                a = a.Add(previous);
                Vertices[i] = a;
                theta += rad;

                if (theta > 2 * Math.PI)
                    theta -= 2 * Math.PI;

                AdjustBoundary(a);
            }
        }

        private double GetConvexPolygonDegree(int edges)
        {
            return 360d / edges;
        }

        private double GetConcavePolygonDegree(int edges, int density)
        {
            return 360d * density / edges;
        }

        private bool IsConcavePolygonPossible(int edges)
        {
            // A concave polygon is defined by its edges and density.
            // The density must be at least 2, and less than half the edges.
            // Density and edges must be coprime (GCD == 1).
            // Therefore, 5 edges is the minimum required for a concave polygon,
            // and 6 edges cannot form a concave polygon, as 6 is divisible by 2.
            return edges == 5
                || edges >= 7;
        }

        private int GetRandomDensityForConcavePolygon(int edges)
        {
            int maxDensity = edges / 2 + edges % 2;
            var potentialDensities = Enumerable.Range(2, edges) // Must be at least 2.
                .TakeWhile(i => i < maxDensity) // Must be less than half the edges.
                .Where(i => GCD(edges, i) == 1) // Must be coprime.
                .ToList();

            return potentialDensities[random.Next(potentialDensities.Count)];
        }

        private static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

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

        public static int GetHueFromFactor(float factor)
        {
            return (int)(FractalForm.HlsMaxValue * factor);
        }

        private void FitToViewport(PointF[] vertices)
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

        private void AdjustBoundary(PointF point)
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

        public IEnumerable<SameColorLines> GetSameColorLines()
        {
            if (Screensaver.Settings.IsRainbow == false)
            {
                yield return new SameColorLines(singleColorHue, Vertices, 0, Vertices.Length - 1);
                yield break;
            }

            int previousHue = 0, groupStart = 0;
            for (int i = 1; i < Vertices.Length; i++)
            {
                int hue = GetHueFromFactor(i / (float)Vertices.Length);
                if (hue == previousHue)
                    continue;

                yield return new SameColorLines(previousHue, Vertices, groupStart, i);
                previousHue = hue;
                groupStart = i;
            }

            if (groupStart != EdgeCount) // In case there's a group at the end which we haven't yielded yet.
                yield return new SameColorLines(previousHue, Vertices, groupStart, EdgeCount);
        }

        internal class SameColorLines
        {
            public int Hue { get; private set; }
            public PointF[] Vertices { get; private set; }

            public SameColorLines(int hue, PointF[] vertices, int from, int to)
            {
                Hue = hue;
                Vertices = new PointF[to - from + 1];
                for (int i = 0; i < Vertices.Length; i++)
                    Vertices[i] = vertices[i + from];
            }
        }
    }

    public static class PointFExtensions
    {
        public static PointF Add(this PointF a, PointF b)
        {
            return new PointF(a.X + b.X, a.Y + b.Y);
        }

        public static PointF Sub(this PointF a, PointF b)
        {
            return new PointF(a.X - b.X, a.Y - b.Y);
        }

        public static PointF Mult(this PointF p, float f)
        {
            return new PointF(p.X * f, p.Y * f);
        }

        public static PointF Div(this PointF p, float d)
        {
            return new PointF(p.X / d, p.Y / d);
        }

        public static PointF Div(this PointF p, PointF b)
        {
            return new PointF(p.X / b.X, p.Y / b.Y);
        }

        public static PointF Norm(this PointF a, PointF b)
        {
            return new PointF(b.Y - a.Y, a.X - b.X);
        }
    }

    public static class RandomExtensions
    {
        public static bool NextBool(this Random r)
        {
            return r.Next(2) == 0;
        }

        public static bool NextBool(this Random r, double probability)
        {
            return r.NextDouble() <= probability;
        }
    }

    public static class DoubleExtensions
    {
        public static double ToRadians(this double degree)
        {
            return degree * Math.PI / 180d;
        }
    }
}
