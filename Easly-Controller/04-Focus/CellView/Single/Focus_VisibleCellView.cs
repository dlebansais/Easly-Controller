namespace EaslyController.Focus
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Frame;

    /// <summary>
    /// Cell view for components that are displayed.
    /// </summary>
    public interface IFocusVisibleCellView : IFrameVisibleCellView, IFocusCellView
    {
        /// <summary>
        /// The frame that created this cell view.
        /// </summary>
        new IFocusFrame Frame { get; }
    }

    /// <summary>
    /// Cell view for components that are displayed.
    /// </summary>
    internal class FocusVisibleCellView : FrameVisibleCellView, IFocusVisibleCellView
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FocusVisibleCellView"/> object.
        /// </summary>
        public static new FocusVisibleCellView Empty { get; } = new FocusVisibleCellView(FocusNodeStateView.Empty, FocusCellViewCollection.Empty, FocusFrame.FocusRoot);

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusVisibleCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        public FocusVisibleCellView(IFocusNodeStateView stateView, IFocusCellViewCollection parentCellView, IFocusFrame frame)
            : base(stateView, parentCellView, frame)
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

        /// <summary>
        /// The frame that created this cell view.
        /// </summary>
        public new IFocusFrame Frame { get { return (IFocusFrame)base.Frame; } }
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
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FocusVisibleCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out FocusVisibleCellView AsVisibleCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsVisibleCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
