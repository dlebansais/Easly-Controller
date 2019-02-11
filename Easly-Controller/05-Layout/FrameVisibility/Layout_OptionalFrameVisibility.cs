namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Focus visibility that depends if an optional node is assigned.
    /// </summary>
    public interface ILayoutOptionalFrameVisibility : IFocusOptionalFrameVisibility, ILayoutFrameVisibility, ILayoutNodeFrameVisibility
    {
    }

    /// <summary>
    /// Focus visibility that depends if an optional node is assigned.
    /// </summary>
    public class LayoutOptionalFrameVisibility : FocusOptionalFrameVisibility, ILayoutOptionalFrameVisibility
    {
    }
}
