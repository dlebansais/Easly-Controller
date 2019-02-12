namespace EaslyController.Controller
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Implements a structure that is used to describe the location of an object.
    /// </summary>
    public struct Point : IFormattable
    {
        #region Init
        static Point()
        {
            Origin = new Point(0, 0);
        }

        /// <summary>
        /// The origin (0,0) location.
        /// </summary>
        public static Point Origin { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> struct.
        /// </summary>
        /// <param name="x">The horizontal coordinate.</param>
        /// <param name="y">The vertical coordinate.</param>
        public Point(double x, double y)
        {
            bool IsInvalidOrigin = double.IsNaN(x) && double.IsNaN(y);
            Debug.Assert(IsInvalidOrigin || (x >= 0 && y >= 0));

            X = x;
            Y = y;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Horizontal coordinate.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Vertical coordinate.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// True if the object is the origin location.
        /// </summary>
        public bool IsOrigin { get { return X == 0 && Y == 0; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Compares two points.
        /// </summary>
        /// <param name="point1">The first point.</param>
        /// <param name="point2">The second point.</param>
        public static bool IsEqual(Point point1, Point point2)
        {
            double DiffX = Math.Abs(point2.X - point1.X);
            double DiffY = Math.Abs(point2.Y - point1.Y);

            return DiffX <= 1e-10 && DiffY <= 1e-10;
        }

        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// Returns a formatted string representation of this instance.
        /// </summary>
        /// <param name="provider">A format provider.</param>
        public string ToString(IFormatProvider provider)
        {
            return ToString(null, provider);
        }

        /// <summary>
        /// Returns a formatted string representation of this instance.
        /// </summary>
        /// <param name="format">A format.</param>
        /// <param name="formatProvider">A format provider.</param>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return $"{X},{Y}";
        }
        #endregion
    }
}
