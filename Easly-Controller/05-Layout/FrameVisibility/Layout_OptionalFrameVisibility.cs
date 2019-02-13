namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Frame visibility that depends if an optional node is assigned.
    /// </summary>
    public interface ILayoutOptionalFrameVisibility : IFocusOptionalFrameVisibility, ILayoutFrameVisibility, ILayoutNodeFrameVisibility
    {
    }

    /// <summary>
    /// Frame visibility that depends if an optional node is assigned.
    /// </summary>
    public class LayoutOptionalFrameVisibility : FocusOptionalFrameVisibility, ILayoutOptionalFrameVisibility
    {
    }
}
