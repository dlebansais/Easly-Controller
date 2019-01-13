namespace EaslyController.Focus
{
    /// <summary>
    /// Base frame visibility for node frames.
    /// </summary>
    public interface IFocusNodeFrameVisibility : IFocusFrameVisibility
    {
        /// <summary>
        /// Is the associated frame visible.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="frame">The frame with the associated visibility.</param>
        bool IsVisible(IFocusCellViewTreeContext context, IFocusNodeFrame frame);
    }
}
