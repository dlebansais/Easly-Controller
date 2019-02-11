namespace EaslyController.Controller
{
    using System;

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
            X = x;
            Y = x;
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
        /// <param name="pt1">The first point.</param>
        /// <param name="pt2">The second point.</param>
        public static bool IsEqual(Point pt1, Point pt2)
        {
            return pt1.X == pt2.X && pt1.Y == pt2.Y;
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
