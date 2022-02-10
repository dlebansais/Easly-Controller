namespace EaslyController.Focus
{
    using System.Diagnostics;
    using BaseNode;
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
        /// <param name="focusedNode">The currently focused node.</param>
        /// <param name="focusedFrame">The currently focused frame in the template associated to <paramref name="focusedNode"/>.</param>
        /// <param name="matchingFocus">The focus in <paramref name="focusChain"/> that match <paramref name="focusedNode"/> and <paramref name="focusedFrame"/> upon return.</param>
        public virtual void UpdateFocusChain(FocusFocusList focusChain, Node focusedNode, IFocusFrame focusedFrame, ref IFocusFocus matchingFocus)
        {
            ChildStateView.UpdateFocusChain(focusChain, focusedNode, focusedFrame, ref matchingFocus);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FocusContainerCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out FocusContainerCellView AsContainerCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsContainerCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
