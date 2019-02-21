namespace EaslyController.Focus
{
    /// <summary>
    /// Base focus.
    /// </summary>
    public interface IFocusFocus
    {
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        IFocusFocusableCellView CellView { get; }
    }

    /// <summary>
    /// Base focus.
    /// </summary>
    public class FocusFocus : IFocusFocus
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusFocus"/> class.
        /// </summary>
        public FocusFocus(IFocusFocusableCellView cellView)
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
