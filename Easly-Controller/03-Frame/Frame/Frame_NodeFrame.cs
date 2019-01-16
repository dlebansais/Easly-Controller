namespace EaslyController.Frame
{
    /// <summary>
    /// Frame for cells within a single node.
    /// </summary>
    public interface IFrameNodeFrame : IFrameFrame
    {
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView);
    }
}
