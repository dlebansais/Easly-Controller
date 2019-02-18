namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Base focus.
    /// </summary>
    public interface ILayoutFocus : IFocusFocus
    {
    }

    /// <summary>
    /// Base frame.
    /// </summary>
    public abstract class LayoutFocus : FocusFocus, IFocusFocus
    {
    }
}
