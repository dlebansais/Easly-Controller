﻿namespace EaslyController.Controller
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
        /// Returns a location corresponding to this point moved by the specified distance.
        /// </summary>
        /// <param name="distanceX">The horizontal distance.</param>
        /// <param name="distanceY">The vertical distance.</param>
        public Point Moved(double distanceX, double distanceY)
        {
            return new Point(X + distanceX, Y + distanceY);
        }

        /// <summary>
        /// Euclidean distance between two points.
        /// </summary>
        /// <param name="point1">The first point.</param>
        /// <param name="point2">The second point.</param>
        public static double Distance(Point point1, Point point2)
        {
            return Math.Sqrt(((point2.X - point1.X) * (point2.X - point1.X)) + ((point2.Y - point1.Y) * (point2.Y - point1.Y)));
        }

        /// <summary>
        /// Euclidean distance between two points, squared.
        /// </summary>
        /// <param name="point1">The first point.</param>
        /// <param name="point2">The second point.</param>
        public static double SquaredDistance(Point point1, Point point2)
        {
            return ((point2.X - point1.X) * (point2.X - point1.X)) + ((point2.Y - point1.Y) * (point2.Y - point1.Y));
        }

        /// <summary>
        /// Compares two points.
        /// </summary>
        /// <param name="point1">The first point.</param>
        /// <param name="point2">The second point.</param>
        public static bool IsEqual(Point point1, Point point2)
        {
            double DiffX = Math.Abs(point2.X - point1.X);
            double DiffY = Math.Abs(point2.Y - point1.Y);

            return RegionHelper.IsZero(DiffX) && RegionHelper.IsZero(DiffY);
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
