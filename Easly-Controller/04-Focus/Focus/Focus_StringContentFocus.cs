namespace EaslyController.Focus
{
    /// <summary>
    /// Focus on a cell view for a string property.
    /// </summary>
    public interface IFocusStringContentFocus : IFocusFocus, IFocusTextFocus
    {
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        new IFocusStringContentFocusableCellView CellView { get; }
    }

    /// <summary>
    /// Focus on a cell view for a string property.
    /// </summary>
    public class FocusStringContentFocus : FocusFocus, IFocusStringContentFocus
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusStringContentFocus"/> class.
        /// </summary>
        public FocusStringContentFocus(IFocusStringContentFocusableCellView cellView)
            : base(cellView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        public new IFocusStringContentFocusableCellView CellView { get { return (IFocusStringContentFocusableCellView)base.CellView; } }
        IFocusFocusableCellView IFocusFocus.CellView { get { return CellView; } }
        IFocusTextFocusableCellView IFocusTextFocus.CellView { get { return CellView; } }
        #endregion
    }
}
