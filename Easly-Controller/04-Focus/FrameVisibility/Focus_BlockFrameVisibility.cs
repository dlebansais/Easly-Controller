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
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="frame">The frame with the associated visibility.</param>
        bool IsBlockVisible(IFocusCellViewTreeContext context, IFocusBlockFrame frame);
    }
}
