namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Focus;

    /// <summary>
    /// Cell view with no content and that is not displayed.
    /// </summary>
    public interface ILayoutEmptyCellView : IFocusEmptyCellView, ILayoutCellView
    {
    }

    /// <summary>
    /// Cell view with no content and that is not displayed.
    /// </summary>
    internal class LayoutEmptyCellView : FocusEmptyCellView, ILayoutEmptyCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutEmptyCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        public LayoutEmptyCellView(ILayoutNodeStateView stateView)
            : base(stateView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public new ILayoutNodeStateView StateView { get { return (ILayoutNodeStateView)base.StateView; } }
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

            if (!comparer.IsSameType(other, out LayoutEmptyCellView AsEmptyCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsEmptyCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
