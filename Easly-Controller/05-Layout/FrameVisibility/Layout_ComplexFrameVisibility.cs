namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Focus visibility that depends on the IsComplex template property.
    /// </summary>
    public interface ILayoutComplexFrameVisibility : IFocusComplexFrameVisibility, ILayoutFrameVisibility, ILayoutNodeFrameVisibility
    {
    }

    /// <summary>
    /// Focus visibility that depends on the IsComplex template property.
    /// </summary>
    public class LayoutComplexFrameVisibility : FocusComplexFrameVisibility, ILayoutComplexFrameVisibility
    {
    }
}
