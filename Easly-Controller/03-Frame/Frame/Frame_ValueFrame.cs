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
        /// <param name="controllerView">The view in cells are created.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public abstract IFrameCellView BuildNodeCells(IFrameControllerView controllerView, IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView);
        #endregion
    }
}
