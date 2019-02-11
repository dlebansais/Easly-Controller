namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Cell view that can be assigned to a property in a state view.
    /// </summary>
    public interface ILayoutAssignableCellView : IFocusAssignableCellView, IEqualComparable
    {
    }
}
