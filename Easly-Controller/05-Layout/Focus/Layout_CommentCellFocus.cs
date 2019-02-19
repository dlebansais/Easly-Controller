namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Focus on a text focusable cell view.
    /// </summary>
    public interface ILayoutCommentCellFocus : IFocusCommentCellFocus, ILayoutCellFocus
    {
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        new ILayoutCommentCellView CellView { get; }
    }

    /// <summary>
    /// Focus on a text focusable cell view.
    /// </summary>
    public class LayoutCommentCellFocus : FocusCommentCellFocus, ILayoutCommentCellFocus
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutCommentCellFocus"/> class.
        /// </summary>
        public LayoutCommentCellFocus(ILayoutCommentCellView cellView)
            : base(cellView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        public new ILayoutCommentCellView CellView { get { return (ILayoutCommentCellView)base.CellView; } }
        ILayoutFocusableCellView ILayoutCellFocus.CellView { get { return CellView; } }
        #endregion
    }
}
