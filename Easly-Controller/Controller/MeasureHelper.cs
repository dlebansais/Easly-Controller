namespace EaslyController.Controller
{
    /// <summary>
    /// Helper class dedicated to cell measure.
    /// </summary>
    public static class MeasureHelper
    {
        /// <summary>
        /// An invalid size that can be used for initialization purpose.
        /// </summary>
        public static Size InvalidSize { get { return new Size(double.NaN, double.NaN); } }

        /// <summary>
        /// Checks that a cell size is valid.
        /// </summary>
        /// <param name="size">The size to check.</param>
        public static bool IsValid(Size size)
        {
            return !double.IsNaN(size.Width) || !double.IsNaN(size.Height);
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
        /// Checks that a cell size is streched horizontally.
        /// </summary>
        /// <param name="size">The size to check.</param>
        public static bool IsStretchedHorizontally(Size size)
        {
            return double.IsNaN(size.Width) && !double.IsNaN(size.Height);
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
