namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Focus on a text focusable cell view.
    /// </summary>
    public interface ILayoutTextFocus : IFocusTextFocus, ILayoutFocus
    {
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        new ILayoutTextFocusableCellView CellView { get; }
    }
}
