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

    /// <summary>
    /// Focus on a text focusable cell view.
    /// </summary>
    public class FocusTextFocus : FocusFocus, IFocusTextFocus
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusTextFocus"/> class.
        /// </summary>
        public FocusTextFocus(IFocusTextFocusableCellView cellView)
            : base(cellView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        public new IFocusTextFocusableCellView CellView { get { return (IFocusTextFocusableCellView)base.CellView; } }
        IFocusFocusableCellView IFocusFocus.CellView { get { return CellView; } }
        #endregion
    }
}
