namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Focus on a comment cell view.
    /// </summary>
    public interface ILayoutCommentFocus : IFocusCommentFocus, ILayoutFocus
    {
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        new ILayoutCommentCellView CellView { get; }
    }

    /// <summary>
    /// Focus on a comment cell view.
    /// </summary>
    public class LayoutCommentFocus : FocusCommentFocus, ILayoutCommentFocus
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutCommentFocus"/> class.
        /// </summary>
        public LayoutCommentFocus(ILayoutCommentCellView cellView)
            : base(cellView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        public new ILayoutCommentCellView CellView { get { return (ILayoutCommentCellView)base.CellView; } }
        ILayoutFocusableCellView ILayoutFocus.CellView { get { return CellView; } }
        #endregion
    }
}
