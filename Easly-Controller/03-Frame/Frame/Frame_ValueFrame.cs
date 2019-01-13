namespace EaslyController.Frame
{
    /// <summary>
    /// Frame describing a value property (or string) in a node.
    /// </summary>
    public interface IFrameValueFrame : IFrameNamedFrame, IFrameNodeFrame
    {
    }

    /// <summary>
    /// Frame describing a value property (or string) in a node.
    /// </summary>
    public abstract class FrameValueFrame : FrameNamedFrame, IFrameValueFrame
    {
        #region Client Interface
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public abstract IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView);
        #endregion
    }
}
