namespace EaslyController.Focus
{
    /// <summary>
    /// Focus on a text focusable cell view.
    /// </summary>
    public interface IFocusTextFocus : IFocusFocus
    {
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        new IFocusTextFocusableCellView CellView { get; }
    }
}
