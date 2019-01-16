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
        IFrameCellView BuildBlockCells(IFrameCellViewTreeContext context);
    }
}
