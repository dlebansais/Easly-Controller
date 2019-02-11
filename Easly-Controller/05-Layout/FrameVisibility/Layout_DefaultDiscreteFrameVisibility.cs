namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Focus visibility that depends if an enum or boolean has the default value.
    /// </summary>
    public interface ILayoutDefaultDiscreteFrameVisibility : IFocusDefaultDiscreteFrameVisibility, ILayoutFrameVisibility, ILayoutNodeFrameVisibility
    {
    }

    /// <summary>
    /// Focus visibility that depends if an enum or boolean has the default value.
    /// </summary>
    public class LayoutDefaultDiscreteFrameVisibility : FocusDefaultDiscreteFrameVisibility, ILayoutDefaultDiscreteFrameVisibility
    {
    }
}
