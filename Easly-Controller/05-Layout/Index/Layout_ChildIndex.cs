namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Base interface for any index representing the child node of a parent node.
    /// </summary>
    public interface ILayoutChildIndex : IFocusChildIndex, ILayoutIndex
    {
    }
}
