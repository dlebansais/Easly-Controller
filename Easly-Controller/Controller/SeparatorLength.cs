namespace EaslyController.Controller
{
    /// <summary>
    /// Measure for a separator, for drawing and printing.
    /// </summary>
    public struct SeparatorLength
    {
        /// <summary>
        /// The empty separator.
        /// </summary>
        public static SeparatorLength Empty = default;

        /// <summary>
        /// Measure for drawing.
        /// </summary>
        public double Draw;

        /// <summary>
        /// Measure for printing.
        /// </summary>
        public int Print;
    }
}
