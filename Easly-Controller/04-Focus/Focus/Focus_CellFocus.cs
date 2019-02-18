namespace EaslyController.Focus
{
    /// <summary>
    /// Focus on a focusable cell view.
    /// </summary>
    public interface IFocusCellFocus : IFocusFocus
    {
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        IFocusFocusableCellView CellView { get; }
    }

    /// <summary>
    /// Focus on a focusable cell view.
    /// </summary>
    public class FocusCellFocus : FocusFocus, IFocusCellFocus
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusCellFocus"/> class.
        /// </summary>
        public FocusCellFocus(IFocusFocusableCellView cellView)
        {
            CellView = cellView;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        public IFocusFocusableCellView CellView { get; private set; }
        #endregion
    }
}
