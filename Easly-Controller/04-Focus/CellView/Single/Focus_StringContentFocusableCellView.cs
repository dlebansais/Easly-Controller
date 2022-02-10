namespace EaslyController.Focus
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Frame;

    /// <summary>
    /// Cell view for text components that can receive the focus and be modified (identifiers).
    /// </summary>
    public interface IFocusStringContentFocusableCellView : IFrameStringContentFocusableCellView, IFocusContentFocusableCellView, IFocusTextFocusableCellView
    {
    }

    /// <summary>
    /// Cell view for text components that can receive the focus and be modified (identifiers).
    /// </summary>
    internal class FocusStringContentFocusableCellView : FrameStringContentFocusableCellView, IFocusStringContentFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusStringContentFocusableCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        /// <param name="propertyName">Property corresponding to the component of the node.</param>
        public FocusStringContentFocusableCellView(IFocusNodeStateView stateView, IFocusCellViewCollection parentCellView, IFocusFrame frame, string propertyName)
            : base(stateView, parentCellView, frame, propertyName)
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
            IFocusStringContentFocus NewFocus = CreateFocus();
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
        /// Compares two <see cref="FocusStringContentFocusableCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out FocusStringContentFocusableCellView AsTextFocusableCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsTextFocusableCellView))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxTextFocus object.
        /// </summary>
        protected virtual IFocusStringContentFocus CreateFocus()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusStringContentFocusableCellView));
            return new FocusStringContentFocus(this);
        }
        #endregion
    }
}
