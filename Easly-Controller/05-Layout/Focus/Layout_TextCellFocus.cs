namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Focus on a text focusable cell view.
    /// </summary>
    public interface ILayoutTextCellFocus : IFocusTextCellFocus, ILayoutCellFocus
    {
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        new ILayoutTextFocusableCellView CellView { get; }
    }

    /// <summary>
    /// Focus on a text focusable cell view.
    /// </summary>
    public class LayoutTextCellFocus : FocusTextCellFocus, ILayoutTextCellFocus
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutTextCellFocus"/> class.
        /// </summary>
        public LayoutTextCellFocus(ILayoutTextFocusableCellView cellView)
            : base(cellView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        public new ILayoutTextFocusableCellView CellView { get { return (ILayoutTextFocusableCellView)base.CellView; } }
        ILayoutFocusableCellView ILayoutCellFocus.CellView { get { return CellView; } }
        #endregion
    }
}
