namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Frame visibility that depends if the current state is not the first in the parent.
    /// </summary>
    public interface ILayoutNotFirstItemFrameVisibility : IFocusNotFirstItemFrameVisibility, ILayoutFrameVisibility, ILayoutNodeFrameVisibility
    {
    }

    /// <summary>
    /// Frame visibility that depends if the current state is not the first in the parent.
    /// </summary>
    public class LayoutNotFirstItemFrameVisibility : FocusNotFirstItemFrameVisibility, ILayoutNotFirstItemFrameVisibility
    {
    }
}
