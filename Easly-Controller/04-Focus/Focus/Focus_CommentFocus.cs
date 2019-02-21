namespace EaslyController.Focus
{
    /// <summary>
    /// Focus on a comment cell view.
    /// </summary>
    public interface IFocusCommentFocus : IFocusFocus
    {
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        new IFocusCommentCellView CellView { get; }
    }

    /// <summary>
    /// Focus on a comment cell view.
    /// </summary>
    public class FocusCommentFocus : FocusFocus, IFocusCommentFocus
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusCommentFocus"/> class.
        /// </summary>
        public FocusCommentFocus(IFocusCommentCellView cellView)
            : base(cellView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        public new IFocusCommentCellView CellView { get { return (IFocusCommentCellView)base.CellView; } }
        IFocusFocusableCellView IFocusFocus.CellView { get { return CellView; } }
        #endregion
    }
}
