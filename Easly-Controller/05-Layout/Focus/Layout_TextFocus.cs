namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Focus on a text focusable cell view.
    /// </summary>
    public interface ILayoutTextFocus : IFocusTextFocus, ILayoutFocus
    {
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        new ILayoutTextFocusableCellView CellView { get; }
    }

    /// <summary>
    /// Focus on a text focusable cell view.
    /// </summary>
    public class LayoutTextFocus : FocusTextFocus, ILayoutTextFocus
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutTextFocus"/> class.
        /// </summary>
        public LayoutTextFocus(ILayoutTextFocusableCellView cellView)
            : base(cellView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        public new ILayoutTextFocusableCellView CellView { get { return (ILayoutTextFocusableCellView)base.CellView; } }
        ILayoutFocusableCellView ILayoutFocus.CellView { get { return CellView; } }
        #endregion
    }
}
