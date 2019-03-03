namespace EaslyController.Controller
{
    using System;

    /// <summary>
    /// Implements a structure that is used to describe the location of a character in a 2-dimensional plane.
    /// </summary>
    public struct Corner : IFormattable
    {
        #region Init
        static Corner()
        {
            Origin = new Corner(0, 0);
        }

        /// <summary>
        /// The origin (0,0) location.
        /// </summary>
        public static Corner Origin { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Corner"/> struct.
        /// </summary>
        /// <param name="x">The horizontal coordinate.</param>
        /// <param name="y">The vertical coordinate.</param>
        public Corner(int x, int y)
        {
            X = x;
            Y = y;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Horizontal coordinate.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Vertical coordinate.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// True if the object is the origin location.
        /// </summary>
        public bool IsOrigin { get { return X == 0 && Y == 0; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Returns a location corresponding to this point moved by the specified distance.
        /// </summary>
        /// <param name="distanceX">The horizontal distance.</param>
        /// <param name="distanceY">The vertical distance.</param>
        public Corner Moved(int distanceX, int distanceY)
        {
            return new Corner(X + distanceX, Y + distanceY);
        }

        /// <summary>
        /// Compares two corners.
        /// </summary>
        /// <param name="corner1">The first corner.</param>
        /// <param name="corner2">The second corner.</param>
        public static bool IsEqual(Corner corner1, Corner corner2)
        {
            return corner1.X == corner2.X && corner1.Y == corner2.Y;
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
