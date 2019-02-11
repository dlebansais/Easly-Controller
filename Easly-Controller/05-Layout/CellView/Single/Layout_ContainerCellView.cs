namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Focus;

    /// <summary>
    /// A leaf of the cell view tree for a child state.
    /// </summary>
    public interface ILayoutContainerCellView : IFocusContainerCellView, ILayoutCellView, ILayoutAssignableCellView
    {
        /// <summary>
        /// The collection of cell views containing this view.
        /// </summary>
        new ILayoutCellViewCollection ParentCellView { get; }

        /// <summary>
        /// The state view of the state associated to this cell.
        /// </summary>
        new ILayoutNodeStateView ChildStateView { get; }
    }

    /// <summary>
    /// A leaf of the cell view tree for a child state.
    /// </summary>
    internal class LayoutContainerCellView : FocusContainerCellView, ILayoutContainerCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutContainerCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view.</param>
        /// <param name="childStateView">The state view of the state associated to this cell.</param>
        public LayoutContainerCellView(ILayoutNodeStateView stateView, ILayoutCellViewCollection parentCellView, ILayoutNodeStateView childStateView)
            : base(stateView, parentCellView, childStateView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The collection of cell views containing this view.
        /// </summary>
        public new ILayoutCellViewCollection ParentCellView { get { return (ILayoutCellViewCollection)base.ParentCellView; } }

        /// <summary>
        /// The state view of the state associated to this cell.
        /// </summary>
        public new ILayoutNodeStateView ChildStateView { get { return (ILayoutNodeStateView)base.ChildStateView; } }

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

            if (!comparer.IsSameType(other, out LayoutContainerCellView AsContainerCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsContainerCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
