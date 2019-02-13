namespace EaslyController.Layout
{
    using EaslyController.Constants;
    using EaslyController.Controller;

    /// <summary>
    /// Frames that have associated cells that can be measured and arranged.
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
        /// <param name="size">The cell size upon return, padding included.</param>
        /// <param name="padding">The cell padding.</param>
        void Measure(ILayoutDrawContext drawContext, ILayoutCellView cellView, out Size size, out Padding padding);
    }
}
