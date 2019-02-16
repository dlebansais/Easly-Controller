namespace EaslyDraw
{
    /// <summary>
    /// A scale factor for a geometry, with a separate value for the low and high edges
    /// </summary>
    public class GeometryScale
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryScale"/> class.
        /// </summary>
        public GeometryScale()
        {
            Low = 0;
            High = 0;
            Scale = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryScale"/> class.
        /// </summary>
        /// <param name="low">The low edge scale.</param>
        /// <param name="high">The high edge scale.</param>
        public GeometryScale(double low, double high)
        {
            Low = low;
            High = high;
            Scale = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryScale"/> class.
        /// </summary>
        /// <param name="other">The other instance to take scales from.</param>
        /// <param name="scale">The scale for this geometry.</param>
        public GeometryScale(GeometryScale other, double scale)
        {
            Low = other.Low;
            High = other.High;
            Scale = scale;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Scale for the low edge.
        /// </summary>
        public double Low { get; private set; }

        /// <summary>
        /// Scale for the high edge.
        /// </summary>
        public double High { get; private set; }

        /// <summary>
        /// Geometry scale.
        /// </summary>
        public double Scale { get; private set; }
        #endregion
    }
}
