namespace EaslyController.Focus
{
    /// <summary>
    /// Focus on a discrete content focusable cell view.
    /// </summary>
    public interface IFocusDiscreteContentCellFocus : IFocusCellFocus
    {
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        new IFocusDiscreteContentFocusableCellView CellView { get; }
    }

    /// <summary>
    /// Focus on a discrete content focusable cell view.
    /// </summary>
    public class FocusDiscreteContentCellFocus : FocusCellFocus, IFocusDiscreteContentCellFocus
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusDiscreteContentCellFocus"/> class.
        /// </summary>
        public FocusDiscreteContentCellFocus(IFocusDiscreteContentFocusableCellView cellView)
            : base(cellView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        public new IFocusDiscreteContentFocusableCellView CellView { get { return (IFocusDiscreteContentFocusableCellView)base.CellView; } }
        IFocusFocusableCellView IFocusCellFocus.CellView { get { return CellView; } }
        #endregion
    }
}
