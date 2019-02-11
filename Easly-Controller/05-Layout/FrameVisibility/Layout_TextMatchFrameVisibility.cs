namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Focus visibility that depends on the IsTextMatch template property.
    /// </summary>
    public interface ILayoutTextMatchFrameVisibility : IFocusTextMatchFrameVisibility, ILayoutFrameVisibility, ILayoutNodeFrameVisibility
    {
    }

    /// <summary>
    /// Focus visibility that depends on the IsTextMatch template property.
    /// </summary>
    public class LayoutTextMatchFrameVisibility : FocusTextMatchFrameVisibility, ILayoutTextMatchFrameVisibility
    {
    }
}
