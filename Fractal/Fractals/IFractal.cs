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
        Vector2[] Vertices { get; }

        void IncreaseFractalDepth();
    }
}
