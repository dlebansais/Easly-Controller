namespace EaslyController.Layout
{
    using EaslyController.Controller;

    /// <summary>
    /// Base frame.
    /// </summary>
    public interface ILayoutMeasurableFrame : ILayoutFrame
    {
        /// <summary>
        /// Measures a cell created with this frame.
        /// </summary>
        /// <param name="drawContext">The context used to measure the cell.</param>
        /// <param name="cellView">The cell to measure.</param>
        Size Measure(ILayoutDrawContext drawContext, ILayoutCellView cellView);
    }
}
