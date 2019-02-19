namespace EaslyController.Focus
{
    /// <summary>
    /// Focus on a text focusable cell view.
    /// </summary>
    public interface IFocusCommentCellFocus : IFocusCellFocus
    {
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        new IFocusCommentCellView CellView { get; }
    }

    /// <summary>
    /// Focus on a text focusable cell view.
    /// </summary>
    public class FocusCommentCellFocus : FocusCellFocus, IFocusCommentCellFocus
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusCommentCellFocus"/> class.
        /// </summary>
        public FocusCommentCellFocus(IFocusCommentCellView cellView)
            : base(cellView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        public new IFocusCommentCellView CellView { get { return (IFocusCommentCellView)base.CellView; } }
        IFocusFocusableCellView IFocusCellFocus.CellView { get { return CellView; } }
        #endregion
    }
}
