namespace EaslyController.Focus
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Frame;

    /// <summary>
    /// Cell view for text components that can receive the focus and be modified (identifiers).
    /// </summary>
    public interface IFocusCommentCellView : IFrameCommentCellView, IFocusFocusableCellView, IFocusTextFocusableCellView
    {
    }

    /// <summary>
    /// Cell view for text components that can receive the focus and be modified (identifiers).
    /// </summary>
    internal class FocusCommentCellView : FrameCommentCellView, IFocusCommentCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusCommentCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        /// <param name="documentation">The comment this cell is displaying.</param>
        public FocusCommentCellView(IFocusNodeStateView stateView, IFocusCellViewCollection parentCellView, IFocusFrame frame, Document documentation)
            : base(stateView, parentCellView, frame, documentation)
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
            IFocusCommentFocus NewFocus = CreateFocus();
            focusChain.Add(NewFocus);

            if (focusedFrame == Frame)
            {
                IFocusOptionalNodeState AsOptionalNodeState = StateView.State as IFocusOptionalNodeState;
                Debug.Assert(AsOptionalNodeState == null || AsOptionalNodeState.ParentInner.IsAssigned);

                if (focusedNode == StateView.State.Node)
                {
                    Debug.Assert(matchingFocus == null);
                    matchingFocus = NewFocus;
                }
            }
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FocusCommentCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out FocusCommentCellView AsCommentCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsCommentCellView))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCommentFocus object.
        /// </summary>
        protected virtual IFocusCommentFocus CreateFocus()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusCommentCellView));
            return new FocusCommentFocus(this);
        }
        #endregion
    }
}
