using System;
using System.Linq;
using System.Numerics;

namespace FractalScreenSaver.Fractals
{
    internal class Snowflake : Tree
    {
        public Snowflake((int width, int height) dimensions) : base(dimensions)
        {
            int edgeCount = Screensaver.Settings.EdgeCount;
            if (Screensaver.Settings.IsRandomCount)
                edgeCount = random.Next(2, 100);

            Array.Resize(ref Vertices, edgeCount + 1);

            Initialize();

            if (Screensaver.Settings.KeepInViewport)
                FitToViewport(Vertices);
        }

        private void Initialize()
        {
            int edges = EdgeCount;
            bool isConcave = IsConcavePolygonPossible(edges) && random.NextBool(1 - 1 / (edges - 3));
            double deg = isConcave
                ? GetConcavePolygonDegree(edges, GetRandomDensityForConcavePolygon(edges))
                : GetConvexPolygonDegree(edges);

            double rad = deg.ToRadians();
            double theta = rad;
            Vector2 currentLine = Vertices[1] - Vertices[0];
            if (currentLine.X == 0)
                theta += Math.PI / 2;
            else
                theta += Math.Atan(currentLine.Y / currentLine.X);

            for (int i = 2; i < Vertices.Length; i++)
            {
                Vector2 previous = Vertices[i - 1];
                Vector2 a = previous - Vertices[i - 2];
                double length = Math.Sqrt(a.X * a.X + a.Y * a.Y);
                a.X = (float)(length * Math.Cos(theta));
                a.Y = (float)(length * Math.Sin(theta));
                a += previous;
                Vertices[i] = a;
                theta += rad;

                if (theta > 2 * Math.PI)
                    theta -= 2 * Math.PI;

                AdjustBoundary(a);
            }
        }

        private static int GCD(int a, int b) => b == 0 ? a : GCD(b, a % b);
        private static double GetConvexPolygonDegree(int edges) => 360d / edges;
        private static double GetConcavePolygonDegree(int edges, int density) => 360d * density / edges;

        private static bool IsConcavePolygonPossible(int edges)
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
    }
}
