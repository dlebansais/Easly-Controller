namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Frame visibility that depends if an enum or boolean has the default value.
    /// </summary>
    public interface ILayoutDefaultDiscreteFrameVisibility : IFocusDefaultDiscreteFrameVisibility, ILayoutFrameVisibility, ILayoutNodeFrameVisibility
    {
    }

    /// <summary>
    /// Frame visibility that depends if an enum or boolean has the default value.
    /// </summary>
    public class LayoutDefaultDiscreteFrameVisibility : FocusDefaultDiscreteFrameVisibility, ILayoutDefaultDiscreteFrameVisibility
    {
    }
}
