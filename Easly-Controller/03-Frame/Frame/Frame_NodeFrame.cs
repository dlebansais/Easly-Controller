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
        /// <param name="parentCellView">The parent cell view.</param>
        IFrameCellView BuildNodeCells(IFrameControllerView controllerView, IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView);

        /// <summary>
        /// Clears the cell view tree for this view.
        /// </summary>
        /// <param name="controllerView">The view in which the cell tree is cleared.</param>
        void ClearRootCellView(IFrameControllerView controllerView, IFrameNodeStateView stateView);
    }
}
