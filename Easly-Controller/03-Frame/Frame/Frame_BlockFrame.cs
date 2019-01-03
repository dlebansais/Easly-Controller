namespace EaslyController.Frame
{
    /// <summary>
    /// Frame for cells within a block.
    /// </summary>
    public interface IFrameBlockFrame
    {
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="controllerView">The view in cells are created.</param>
        /// <param name="stateView">The state view containing <paramref name="blockStateView"/> for which to create cells.</param>
        /// <param name="blockStateView">The block state view for which to create cells.</param>
        IFrameCellView BuildBlockCells(IFrameControllerView controllerView, IFrameNodeStateView stateView, IFrameBlockStateView blockStateView);
    }
}
