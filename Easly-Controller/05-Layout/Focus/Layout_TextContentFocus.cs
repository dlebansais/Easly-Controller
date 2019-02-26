namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Focus on a cell view for a string property.
    /// </summary>
    public interface ILayoutStringContentFocus : IFocusStringContentFocus, ILayoutFocus, ILayoutTextFocus
    {
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        new ILayoutStringContentFocusableCellView CellView { get; }
    }

    /// <summary>
    /// Focus on a cell view for a string property.
    /// </summary>
    public class LayoutStringContentFocus : FocusStringContentFocus, ILayoutStringContentFocus
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutStringContentFocus"/> class.
        /// </summary>
        public LayoutStringContentFocus(ILayoutStringContentFocusableCellView cellView)
            : base(cellView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        public new ILayoutStringContentFocusableCellView CellView { get { return (ILayoutStringContentFocusableCellView)base.CellView; } }
        ILayoutFocusableCellView ILayoutFocus.CellView { get { return CellView; } }
        ILayoutTextFocusableCellView ILayoutTextFocus.CellView { get { return CellView; } }
        #endregion
    }
}
