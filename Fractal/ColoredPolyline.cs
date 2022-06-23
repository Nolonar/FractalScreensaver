using System.Drawing;

namespace FractalScreenSaver
{
    internal class ColoredPolyline
    {
        public int Hue { get; private set; }
        public PointF[] Vertices { get; private set; }

        public ColoredPolyline(int hue, PointF[] vertices)
        {
            Hue = hue;
            Vertices = vertices;
        }
    }
}
