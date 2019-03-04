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
        public static Point InvalidOrigin { get { return new Point(double.NaN, double.NaN); } }

        /// <summary>
        /// An invalid size that can be used for initialization purpose.
        /// </summary>
        public static Size InvalidSize { get { return new Size(double.NaN, double.NaN); } }

        /// <summary>
        /// An invalid location that can be used for initialization purpose.
        /// </summary>
        public static Corner InvalidCorner { get { return new Corner(int.MinValue, int.MinValue); } }

        /// <summary>
        /// An invalid size that can be used for initialization purpose.
        /// </summary>
        public static Plane InvalidPlane { get { return new Plane(-1, -1); } }

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
        /// Checks that a cell location is valid.
        /// </summary>
        /// <param name="corner">The location to check.</param>
        public static bool IsValid(Corner corner)
        {
            return corner.X != int.MinValue || corner.Y != int.MinValue;
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
        /// Checks that a cell size is valid.
        /// </summary>
        /// <param name="plane">The size to check.</param>
        public static bool IsValid(Plane plane)
        {
            return plane.Width >= 0 || plane.Height >= 0;
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
        /// Checks that a cell location is fixed.
        /// </summary>
        /// <param name="corner">The location to check.</param>
        public static bool IsFixed(Corner corner)
        {
            return corner.X != int.MinValue && corner.Y != int.MinValue;
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
        /// Checks that a cell size is fixed.
        /// </summary>
        /// <param name="plane">The size to check.</param>
        public static bool IsFixed(Plane plane)
        {
            return plane.Width >= 0 && plane.Height >= 0;
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
