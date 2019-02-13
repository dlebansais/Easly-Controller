namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Frame visibility that depends on the IsTextMatch template property.
    /// </summary>
    public interface ILayoutTextMatchFrameVisibility : IFocusTextMatchFrameVisibility, ILayoutFrameVisibility, ILayoutNodeFrameVisibility
    {
    }

    /// <summary>
    /// Frame visibility that depends on the IsTextMatch template property.
    /// </summary>
    public class LayoutTextMatchFrameVisibility : FocusTextMatchFrameVisibility, ILayoutTextMatchFrameVisibility
    {
    }
}
