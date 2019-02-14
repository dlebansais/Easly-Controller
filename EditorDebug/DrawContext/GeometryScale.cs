namespace EditorDebug
{
    public class GeometryScale
    {
        public GeometryScale()
        {
            Low = 0;
            High = 0;
            Scale = 0;
        }

        public GeometryScale(double low, double high)
        {
            Low = low;
            High = high;
            Scale = 0;
        }

        public GeometryScale(GeometryScale other, double scale)
        {
            Low = other.Low;
            High = other.High;
            Scale = scale;
        }

        public double Low { get; private set; }
        public double High { get; private set; }
        public double Scale { get; private set; }
    }
}
