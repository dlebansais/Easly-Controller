namespace EaslyController.Focus
{
    using System.Diagnostics;

    /// <summary>
    /// Focus on a discrete content focusable cell view.
    /// </summary>
    public interface IFocusDiscreteContentFocus : IFocusFocus
    {
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        new IFocusDiscreteContentFocusableCellView CellView { get; }
    }

    /// <summary>
    /// Focus on a discrete content focusable cell view.
    /// </summary>
    [DebuggerDisplay("Discrete: {CellView.StateView.State}")]
    public class FocusDiscreteContentFocus : FocusFocus, IFocusDiscreteContentFocus
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusDiscreteContentFocus"/> class.
        /// </summary>
        public FocusDiscreteContentFocus(IFocusDiscreteContentFocusableCellView cellView)
            : base(cellView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The cell view with the focus.
        /// </summary>
        public new IFocusDiscreteContentFocusableCellView CellView { get { return (IFocusDiscreteContentFocusableCellView)base.CellView; } }
        IFocusFocusableCellView IFocusFocus.CellView { get { return CellView; } }
        #endregion
    }
}
