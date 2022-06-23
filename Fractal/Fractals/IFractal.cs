using System.Collections.Generic;
using System.Drawing;

namespace FractalScreenSaver.Fractals
{
    internal interface IFractal
    {
        public enum Type
        {
            Tree,
            Snowflake
        }

        int EdgeCount { get; }

        IEnumerable<(int hue, PointF[] vertices)> GetColoredPolyline();

        void IncreaseFractalDepth();
    }
}
