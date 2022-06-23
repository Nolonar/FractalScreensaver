using System;
using System.Drawing;

namespace FractalScreenSaver
{
    public static class PointFExtensions
    {
        public static PointF Add(this PointF a, PointF b) =>
            new(a.X + b.X, a.Y + b.Y);

        public static PointF Sub(this PointF a, PointF b) =>
            new(a.X - b.X, a.Y - b.Y);

        public static PointF Mult(this PointF p, float f) =>
            new(p.X * f, p.Y * f);

        public static PointF Div(this PointF p, float d) =>
            new(p.X / d, p.Y / d);

        public static PointF Div(this PointF p, PointF b) =>
            new(p.X / b.X, p.Y / b.Y);

        public static PointF Norm(this PointF a, PointF b) =>
            new(b.Y - a.Y, a.X - b.X);
    }

    public static class RandomExtensions
    {
        public static bool NextBool(this Random r) =>
            r.Next(2) == 0;

        public static bool NextBool(this Random r, double probability) =>
            r.NextDouble() <= probability;
    }

    public static class DoubleExtensions
    {
        public static double ToRadians(this double degree) =>
            degree * Math.PI / 180d;
    }
}
