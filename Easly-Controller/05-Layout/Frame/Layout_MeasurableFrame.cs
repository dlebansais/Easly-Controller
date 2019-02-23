namespace EaslyController.Layout
{
    using EaslyController.Controller;

    /// <summary>
    /// Frames that have associated cells that can be measured and arranged.
    /// </summary>
    public interface ILayoutMeasurableFrame : ILayoutFrame
    {
        /// <summary>
        /// Measures a cell created with this frame.
        /// </summary>
        /// <param name="drawContext">The context used to measure the cell.</param>
        /// <param name="cellView">The cell to measure.</param>
        /// <param name="collectionWithSeparator">A collection that can draw separators around the cell.</param>
        /// <param name="referenceContainer">The cell view in <paramref name="collectionWithSeparator"/> that contains this cell.</param>
        /// <param name="separatorLength">The length of the separator in <paramref name="collectionWithSeparator"/>.</param>
        /// <param name="size">The cell size upon return, padding included.</param>
        /// <param name="padding">The cell padding.</param>
        void Measure(ILayoutDrawContext drawContext, ILayoutCellView cellView, ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer, double separatorLength, out Size size, out Padding padding);
    }
}
