namespace EaslyController.Focus
{
    /// <summary>
    /// Base frame visibility for block frames.
    /// </summary>
    public interface IFocusBlockFrameVisibility : IFocusFrameVisibility
    {
        /// <summary>
        /// Is the associated frame visible.
        /// </summary>
        /// <param name="controllerView">The view in cells are created.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        /// <param name="blockStateView">The block state view for which to create cells.</param>
        /// <param name="frame">The frame with the associated visibility.</param>
        bool IsBlockVisible(IFocusControllerView controllerView, IFocusNodeStateView stateView, IFocusBlockStateView blockStateView, IFocusBlockFrame frame);
    }
}
