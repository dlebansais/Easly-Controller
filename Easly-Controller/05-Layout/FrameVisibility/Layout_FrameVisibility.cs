namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Base frame visibility.
    /// </summary>
    public interface ILayoutFrameVisibility : IFocusFrameVisibility
    {
    }

    /// <summary>
    /// Base frame visibility.
    /// </summary>
    public abstract class LayoutFrameVisibility : FocusFrameVisibility, ILayoutFrameVisibility
    {
    }
}
