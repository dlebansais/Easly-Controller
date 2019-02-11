namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Focus;

    /// <summary>
    /// Cell view for text components that can receive the focus and be modified (identifiers).
    /// </summary>
    public interface ILayoutTextFocusableCellView : IFocusTextFocusableCellView, ILayoutContentFocusableCellView
    {
    }

    /// <summary>
    /// Cell view for text components that can receive the focus and be modified (identifiers).
    /// </summary>
    internal class LayoutTextFocusableCellView : FocusTextFocusableCellView, ILayoutTextFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutTextFocusableCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        /// <param name="propertyName">Property corresponding to the component of the node.</param>
        public LayoutTextFocusableCellView(ILayoutNodeStateView stateView, ILayoutFrame frame, string propertyName)
            : base(stateView, frame, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public new ILayoutNodeStateView StateView { get { return (ILayoutNodeStateView)base.StateView; } }

        /// <summary>
        /// The frame that created this cell view.
        /// </summary>
        public new ILayoutFrame Frame { get { return (ILayoutFrame)base.Frame; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ILayoutCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutTextFocusableCellView AsTextFocusableCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsTextFocusableCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
