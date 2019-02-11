namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Selects specific frames in the remaining of the cell view tree.
    /// </summary>
    public interface ILayoutFrameSelector : IFocusFrameSelector
    {
    }

    /// <summary>
    /// Selects specific frames in the remaining of the cell view tree.
    /// </summary>
    public class LayoutFrameSelector : FocusFrameSelector, ILayoutFrameSelector
    {
    }
}
