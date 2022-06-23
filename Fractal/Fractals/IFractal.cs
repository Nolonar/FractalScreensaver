using System.Collections.Generic;
using System.Numerics;

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

        IEnumerable<(int hue, Vector2[] vertices)> GetColoredPolyline();

        void IncreaseFractalDepth();
    }
}
