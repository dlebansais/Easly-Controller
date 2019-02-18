using System;

namespace EaslyController.Controller
{
    /// <summary>
    /// Helper class dedicated to measuring and arranging cells.
    /// </summary>
    public static class RegionHelper
    {
        /// <summary>
        /// Tolerance for comparing points, sizes and rects.
        /// </summary>
        public const double Tolerance = 1e-10;

        /// <summary>
        /// An invalid location that can be used for initialization purpose.
        /// </summary>
        public static Point InvalidOrigin { get { return new Point(double.NaN, double.NaN); } }

        /// <summary>
        /// An invalid size that can be used for initialization purpose.
        /// </summary>
        public static Size InvalidSize { get { return new Size(double.NaN, double.NaN); } }

        /// <summary>
        /// Checks if a value is zero, within a tolerance.
        /// </summary>
        /// <param name="value">The value.</param>
        public static bool IsZero(double value)
        {
            return Math.Abs(value) <= Tolerance;
        }

        /// <summary>
        /// Checks that a cell location is valid.
        /// </summary>
        /// <param name="point">The location to check.</param>
        public static bool IsValid(Point point)
        {
            return !double.IsNaN(point.X) || !double.IsNaN(point.Y);
        }
        /// <summary>
        /// Checks that a cell size is valid.
        /// </summary>
        /// <param name="size">The size to check.</param>
        public static bool IsValid(Size size)
        {
            return !double.IsNaN(size.Width) || !double.IsNaN(size.Height);
        }

        /// <summary>
        /// Checks that a cell location is fixed.
        /// </summary>
        /// <param name="point">The location to check.</param>
        public static bool IsFixed(Point point)
        {
            return !double.IsNaN(point.X) && !double.IsNaN(point.Y);
        }

        /// <summary>
        /// Checks that a cell size is fixed.
        /// </summary>
        /// <param name="size">The size to check.</param>
        public static bool IsFixed(Size size)
        {
            return !double.IsNaN(size.Width) && !double.IsNaN(size.Height);
        }

        /// <summary>
        /// Checks that a cell size is floating horizontally.
        /// </summary>
        /// <param name="point">The location to check.</param>
        public static bool IsFloatingHorizontally(Point point)
        {
            return double.IsNaN(point.X) && !double.IsNaN(point.Y);
        }

        /// <summary>
        /// Checks that a cell size is streched horizontally.
        /// </summary>
        /// <param name="size">The size to check.</param>
        public static bool IsStretchedHorizontally(Size size)
        {
            return double.IsNaN(size.Width) && !double.IsNaN(size.Height);
        }

        /// <summary>
        /// Checks that a cell size is floating vertically.
        /// </summary>
        /// <param name="point">The location to check.</param>
        public static bool IsFloatingVertically(Point point)
        {
            return !double.IsNaN(point.X) && double.IsNaN(point.Y);
        }

        /// <summary>
        /// Checks that a cell size is streched vertically.
        /// </summary>
        /// <param name="size">The size to check.</param>
        public static bool IsStretchedVertically(Size size)
        {
            return !double.IsNaN(size.Width) && double.IsNaN(size.Height);
        }
    }
}
