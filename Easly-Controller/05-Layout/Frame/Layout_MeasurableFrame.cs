namespace EaslyController.Layout
{
    using EaslyController.Controller;
    using NodeController;

    /// <summary>
    /// Base frame.
    /// </summary>
    public interface ILayoutMeasurableFrame : ILayoutFrame
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

        /// <summary>
        /// Measures a cell created with this frame.
        /// </summary>
        /// <param name="drawContext">The context used to measure the cell.</param>
        /// <param name="cellView">The cell to measure.</param>
        Size Measure(ILayoutDrawContext drawContext, ILayoutCellView cellView);
    }
}
