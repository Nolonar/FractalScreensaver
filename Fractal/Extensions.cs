using System;
using System.Drawing;
using System.Numerics;

namespace FractalScreenSaver
{
    public static class Vector2Extensions
    {
        public static Vector2 Cross(this Vector2 a, Vector2 b) =>
            new(b.Y - a.Y, a.X - b.X);
    }

    public static class RandomExtensions
    {
        public static bool NextBool(this Random r) =>
            r.Next(2) == 0;

        public static bool NextBool(this Random r, double probability) =>
            r.NextDouble() <= probability;

        public static double NextDouble(this Random r, double min, double max) =>
            r.NextDouble() * (max - min) + min;
    }

    public static class DoubleExtensions
    {
        public static double ToRadians(this double degree) =>
            degree * Math.PI / 180d;
    }
}
