namespace EaslyController.Frame
{
    /// <summary>
    /// Frame for cells within a block.
    /// </summary>
    public interface IFrameBlockFrame : IFrameFrame
    {
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        IFrameCellView BuildBlockCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView);
    }
}
