namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Frame that can have a property associated to a selector (or children).
    /// </summary>
    public interface ILayoutSelectorPropertyFrame : IFocusSelectorPropertyFrame, ILayoutNodeFrame
    {
    }
}
