namespace EaslyController.Frame
{
    /// <summary>
    /// Frame for cells within a single node.
    /// </summary>
    public interface IFrameNodeFrame
    {
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="controllerView">The view in cells are created.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        IFrameCellView BuildNodeCells(IFrameControllerView controllerView, IFrameNodeStateView stateView);
    }
}
