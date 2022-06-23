using System.Collections.Generic;

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

        IEnumerable<ColoredPolyline> GetColoredPolyline();

        void IncreaseFractalDepth();
    }
}
