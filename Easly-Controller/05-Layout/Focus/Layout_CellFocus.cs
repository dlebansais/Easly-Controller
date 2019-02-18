namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Focus on a focusable cell view.
    /// </summary>
    public interface ILayoutCellFocus : IFocusCellFocus, ILayoutFocus
    {
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        new ILayoutFocusableCellView CellView { get; }
    }

    /// <summary>
    /// Focus on a focusable cell view.
    /// </summary>
    public class LayoutCellFocus : FocusCellFocus, ILayoutCellFocus
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutCellFocus"/> class.
        /// </summary>
        public LayoutCellFocus(ILayoutFocusableCellView cellView)
            : base(cellView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        public new ILayoutFocusableCellView CellView { get { return (ILayoutFocusableCellView)base.CellView; } }
        #endregion
    }
}
