namespace EaslyController.Focus
{
    /// <summary>
    /// Focus on a text focusable cell view.
    /// </summary>
    public interface IFocusTextCellFocus : IFocusCellFocus
    {
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        new IFocusTextFocusableCellView CellView { get; }
    }

    /// <summary>
    /// Focus on a text focusable cell view.
    /// </summary>
    public class FocusTextCellFocus : FocusCellFocus, IFocusTextCellFocus
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusTextCellFocus"/> class.
        /// </summary>
        public FocusTextCellFocus(IFocusTextFocusableCellView cellView)
            : base(cellView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        public new IFocusTextFocusableCellView CellView { get { return (IFocusTextFocusableCellView)base.CellView; } }
        IFocusFocusableCellView IFocusCellFocus.CellView { get { return CellView; } }
        #endregion
    }
}
