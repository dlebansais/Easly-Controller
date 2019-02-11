namespace EaslyController.Controller
{
    /// <summary>
    /// Helper class dedicated to arranging cells.
    /// </summary>
    public static class ArrangeHelper
    {
        /// <summary>
        /// An invalid location that can be used for initialization purpose.
        /// </summary>
        public static Point InvalidOrigin { get { return new Point(double.NaN, double.NaN); } }

        /// <summary>
        /// Checks that a cell location is valid.
        /// </summary>
        /// <param name="point">The location to check.</param>
        public static bool IsValid(Point point)
        {
            return !double.IsNaN(point.X) || !double.IsNaN(point.Y);
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
        /// Checks that a cell size is floating horizontally.
        /// </summary>
        /// <param name="point">The location to check.</param>
        public static bool IsFloatingHorizontally(Point point)
        {
            return double.IsNaN(point.X) && !double.IsNaN(point.Y);
        }

        /// <summary>
        /// Checks that a cell size is floating vertically.
        /// </summary>
        /// <param name="point">The location to check.</param>
        public static bool IsFloatingVertically(Point point)
        {
            return !double.IsNaN(point.X) && double.IsNaN(point.Y);
        }
    }
}
