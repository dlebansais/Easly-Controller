namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Focus;

    /// <summary>
    /// Cell view for components that are displayed.
    /// </summary>
    public interface ILayoutVisibleCellView : IFocusVisibleCellView, ILayoutCellView
    {
        /// <summary>
        /// The frame that created this cell view.
        /// </summary>
        new ILayoutFrame Frame { get; }
    }

    /// <summary>
    /// Cell view for components that are displayed.
    /// </summary>
    internal class LayoutVisibleCellView : FocusVisibleCellView, ILayoutVisibleCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutVisibleCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        public LayoutVisibleCellView(ILayoutNodeStateView stateView, ILayoutFrame frame)
            : base(stateView, frame)
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

            if (!comparer.IsSameType(other, out LayoutVisibleCellView AsVisibleCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsVisibleCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
