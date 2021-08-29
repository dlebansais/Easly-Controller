namespace EaslyController.Focus
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Frame;

    /// <summary>
    /// Atomic cell view of a component in a node.
    /// </summary>
    public interface IFocusCellView : IFrameCellView
    {
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        new IFocusNodeStateView StateView { get; }

        /// <summary>
        /// The collection of cell views containing this view. Null for the root of the cell tree.
        /// </summary>
        new IFocusCellViewCollection ParentCellView { get; }

        /// <summary>
        /// Updates the focus chain with cells in the tree.
        /// </summary>
        /// <param name="focusChain">The list of focusable cell views found in the tree.</param>
        /// <param name="focusedNode">The currently focused node.</param>
        /// <param name="focusedFrame">The currently focused frame in the template associated to <paramref name="focusedNode"/>.</param>
        /// <param name="matchingFocus">The focus in <paramref name="focusChain"/> that match <paramref name="focusedNode"/> and <paramref name="focusedFrame"/> upon return.</param>
        void UpdateFocusChain(IFocusFocusList focusChain, Node focusedNode, IFocusFrame focusedFrame, ref IFocusFocus matchingFocus);
    }

    /// <summary>
    /// Atomic cell view of a component in a node.
    /// </summary>
    internal abstract class FocusCellView : FrameCellView, IFocusCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        public FocusCellView(IFocusNodeStateView stateView, IFocusCellViewCollection parentCellView)
            : base(stateView, parentCellView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public new IFocusNodeStateView StateView { get { return (IFocusNodeStateView)base.StateView; } }

        /// <summary>
        /// The collection of cell views containing this view. Null for the root of the cell tree.
        /// </summary>
        public new IFocusCellViewCollection ParentCellView { get { return (IFocusCellViewCollection)base.ParentCellView; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Updates the focus chain with cells in the tree.
        /// </summary>
        /// <param name="focusChain">The list of focusable cell views found in the tree.</param>
        /// <param name="focusedNode">The currently focused node.</param>
        /// <param name="focusedFrame">The currently focused frame in the template associated to <paramref name="focusedNode"/>.</param>
        /// <param name="matchingFocus">The focus in <paramref name="focusChain"/> that match <paramref name="focusedNode"/> and <paramref name="focusedFrame"/> upon return.</param>
        public abstract void UpdateFocusChain(IFocusFocusList focusChain, Node focusedNode, IFocusFrame focusedFrame, ref IFocusFocus matchingFocus);
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FocusCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public abstract override bool IsEqual(CompareEqual comparer, IEqualComparable other);
        #endregion
    }
}
