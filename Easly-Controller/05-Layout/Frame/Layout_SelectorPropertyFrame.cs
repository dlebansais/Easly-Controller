namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Focus that can have a property associated to a selector (or children).
    /// </summary>
    public interface ILayoutSelectorPropertyFrame : IFocusSelectorPropertyFrame, ILayoutNodeFrame
    {
    }
}
