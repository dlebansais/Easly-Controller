namespace EaslyController.Focus
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Frame;

    /// <summary>
    /// Cell view for discrete elements that can receive the focus but are not always the component of a node (insertion points, keywords and other decorations)
    /// </summary>
    public interface IFocusFocusableCellView : IFrameFocusableCellView, IFocusVisibleCellView
    {
    }

    /// <summary>
    /// Cell view for discrete elements that can receive the focus but are not always the component of a node (insertion points, keywords and other decorations)
    /// </summary>
    internal class FocusFocusableCellView : FrameFocusableCellView, IFocusFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusFocusableCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        public FocusFocusableCellView(IFocusNodeStateView stateView, IFocusCellViewCollection parentCellView, IFocusFrame frame)
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
        public virtual void UpdateFocusChain(IFocusFocusList focusChain, INode focusedNode, IFocusFrame focusedFrame, ref IFocusFocus matchingFocus)
        {
            IFocusFocus NewFocus = CreateFocus();
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
        /// Compares two <see cref="IFocusCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusFocusableCellView AsFocusableCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsFocusableCellView))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFocus object.
        /// </summary>
        protected virtual IFocusFocus CreateFocus()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusFocusableCellView));
            return new FocusFocus(this);
        }
        #endregion
    }
}
