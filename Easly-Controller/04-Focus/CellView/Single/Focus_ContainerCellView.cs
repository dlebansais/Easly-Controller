namespace EaslyController.Focus
{
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// A leaf of the cell view tree for a child state.
    /// </summary>
    public interface IFocusContainerCellView : IFrameContainerCellView, IFocusCellView, IFocusAssignableCellView
    {
        /// <summary>
        /// The collection of cell views containing this view.
        /// </summary>
        new IFocusCellViewCollection ParentCellView { get; }

        /// <summary>
        /// The state view of the state associated to this cell.
        /// </summary>
        new IFocusNodeStateView ChildStateView { get; }

        /// <summary>
        /// The frame that was used to create this cell.
        /// </summary>
        new IFocusFrame Frame { get; }
    }

    /// <summary>
    /// A leaf of the cell view tree for a child state.
    /// </summary>
    internal class FocusContainerCellView : FrameContainerCellView, IFocusContainerCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusContainerCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view.</param>
        /// <param name="childStateView">The state view of the state associated to this cell.</param>
        /// <param name="frame">The frame that was used to create this cell.</param>
        public FocusContainerCellView(IFocusNodeStateView stateView, IFocusCellViewCollection parentCellView, IFocusNodeStateView childStateView, IFrameFrame frame)
            : base(stateView, parentCellView, childStateView, frame)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The collection of cell views containing this view.
        /// </summary>
        public new IFocusCellViewCollection ParentCellView { get { return (IFocusCellViewCollection)base.ParentCellView; } }

        /// <summary>
        /// The state view of the state associated to this cell.
        /// </summary>
        public new IFocusNodeStateView ChildStateView { get { return (IFocusNodeStateView)base.ChildStateView; } }

        /// <summary>
        /// The frame that was used to create this cell.
        /// </summary>
        public new IFocusFrame Frame { get { return (IFocusFrame)base.Frame; } }

        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public new IFocusNodeStateView StateView { get { return (IFocusNodeStateView)base.StateView; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Updates the focus chain with cells in the tree.
        /// </summary>
        /// <param name="focusChain">The list of focusable cell views found in the tree.</param>
        public virtual void UpdateFocusChain(IFocusFocusableCellViewList focusChain)
        {
            ChildStateView.UpdateFocusChain(focusChain);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusContainerCellView AsContainerCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsContainerCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
