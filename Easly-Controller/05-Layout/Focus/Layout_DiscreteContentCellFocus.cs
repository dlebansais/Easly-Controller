namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Focus on a discrete content focusable cell view.
    /// </summary>
    public interface ILayoutDiscreteContentCellFocus : IFocusDiscreteContentCellFocus, ILayoutCellFocus
    {
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        new ILayoutDiscreteContentFocusableCellView CellView { get; }
    }

    /// <summary>
    /// Focus on a discrete content focusable cell view.
    /// </summary>
    public class LayoutDiscreteContentCellFocus : FocusDiscreteContentCellFocus, ILayoutDiscreteContentCellFocus
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutDiscreteContentCellFocus"/> class.
        /// </summary>
        public LayoutDiscreteContentCellFocus(ILayoutDiscreteContentFocusableCellView cellView)
            : base(cellView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        public new ILayoutDiscreteContentFocusableCellView CellView { get { return (ILayoutDiscreteContentFocusableCellView)base.CellView; } }
        ILayoutFocusableCellView ILayoutCellFocus.CellView { get { return CellView; } }
        #endregion
    }
}
