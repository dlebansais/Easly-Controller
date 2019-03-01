namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// A selection of a property of a node, or a partial selection for text.
    /// </summary>
    public interface ILayoutContentSelection : IFocusContentSelection, ILayoutSelection
    {
    }
}
