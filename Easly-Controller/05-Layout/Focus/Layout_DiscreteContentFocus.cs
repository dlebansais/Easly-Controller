namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Focus on a discrete content focusable cell view.
    /// </summary>
    public interface ILayoutDiscreteContentFocus : IFocusDiscreteContentFocus, ILayoutFocus
    {
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        new ILayoutDiscreteContentFocusableCellView CellView { get; }
    }

    /// <summary>
    /// Focus on a discrete content focusable cell view.
    /// </summary>
    public class LayoutDiscreteContentFocus : FocusDiscreteContentFocus, ILayoutDiscreteContentFocus
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutDiscreteContentFocus"/> class.
        /// </summary>
        public LayoutDiscreteContentFocus(ILayoutDiscreteContentFocusableCellView cellView)
            : base(cellView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        public new ILayoutDiscreteContentFocusableCellView CellView { get { return (ILayoutDiscreteContentFocusableCellView)base.CellView; } }
        ILayoutFocusableCellView ILayoutFocus.CellView { get { return CellView; } }
        #endregion
    }
}
