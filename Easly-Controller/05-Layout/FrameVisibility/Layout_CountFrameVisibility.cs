namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Frame visibility that depends if a collection has at least one item.
    /// </summary>
    public interface ILayoutCountFrameVisibility : IFocusCountFrameVisibility, ILayoutFrameVisibility, ILayoutNodeFrameVisibility
    {
    }

    /// <summary>
    /// Frame visibility that depends if a collection has at least one item.
    /// </summary>
    public class LayoutCountFrameVisibility : FocusCountFrameVisibility, ILayoutCountFrameVisibility
    {
    }
}
