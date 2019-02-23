namespace EaslyController.Layout
{
    using EaslyController.Constants;

    /// <summary>
    /// Frame that can have left and right margins.
    /// </summary>
    public interface ILayoutFrameWithMargins
    {
        /// <summary>
        /// Margin at the left side of the cell.
        /// (Set in Xaml)
        /// </summary>
        Margins LeftMargin { get; }

        /// <summary>
        /// Margin at the right side of the cell.
        /// (Set in Xaml)
        /// </summary>
        Margins RightMargin { get; }
    }
}
