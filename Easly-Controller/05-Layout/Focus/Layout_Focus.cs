namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Base focus.
    /// </summary>
    public interface ILayoutFocus : IFocusFocus
    {
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        new ILayoutFocusableCellView CellView { get; }
    }

    /// <summary>
    /// Base focus.
    /// </summary>
    public class LayoutFocus : FocusFocus, ILayoutFocus
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutFocus"/> class.
        /// </summary>
        public LayoutFocus(ILayoutFocusableCellView cellView)
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
