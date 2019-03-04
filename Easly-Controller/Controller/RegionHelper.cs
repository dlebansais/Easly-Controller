namespace EaslyController.Controller
{
    using System;

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
        public static Point InvalidOrigin { get { return new Point(Measure.Floating, Measure.Floating); } }

        /// <summary>
        /// An invalid size that can be used for initialization purpose.
        /// </summary>
        public static Size InvalidSize { get { return new Size(Measure.Floating, Measure.Floating); } }

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
            return !point.X.IsFloating || !point.Y.IsFloating;
        }

        /// <summary>
        /// Checks that a cell size is valid.
        /// </summary>
        /// <param name="size">The size to check.</param>
        public static bool IsValid(Size size)
        {
            return !size.Width.IsFloating || !size.Height.IsFloating;
        }

        /// <summary>
        /// Checks that a cell location is fixed.
        /// </summary>
        /// <param name="point">The location to check.</param>
        public static bool IsFixed(Point point)
        {
            return !point.X.IsFloating && !point.Y.IsFloating;
        }

        /// <summary>
        /// Checks that a cell size is fixed.
        /// </summary>
        /// <param name="size">The size to check.</param>
        public static bool IsFixed(Size size)
        {
            return !size.Width.IsFloating && !size.Height.IsFloating;
        }

        /// <summary>
        /// Checks that a rectangle is fixed.
        /// </summary>
        /// <param name="rect">The rectangle to check.</param>
        public static bool IsFixed(Rect rect)
        {
            return !double.IsNaN(rect.X) && !double.IsNaN(rect.Y) && !double.IsNaN(rect.Width) && !double.IsNaN(rect.Height);
        }

        /// <summary>
        /// Checks that a cell size is floating horizontally.
        /// </summary>
        /// <param name="point">The location to check.</param>
        public static bool IsFloatingHorizontally(Point point)
        {
            return point.X.IsFloating && !point.Y.IsFloating;
        }

        /// <summary>
        /// Checks that a cell size is streched horizontally.
        /// </summary>
        /// <param name="size">The size to check.</param>
        public static bool IsStretchedHorizontally(Size size)
        {
            return size.Width.IsFloating && !size.Height.IsFloating;
        }

        /// <summary>
        /// Checks that a cell size is floating vertically.
        /// </summary>
        /// <param name="point">The location to check.</param>
        public static bool IsFloatingVertically(Point point)
        {
            return !point.X.IsFloating && point.Y.IsFloating;
        }

        /// <summary>
        /// Checks that a cell size is streched vertically.
        /// </summary>
        /// <param name="size">The size to check.</param>
        public static bool IsStretchedVertically(Size size)
        {
            return !size.Width.IsFloating && size.Height.IsFloating;
        }
    }
}
