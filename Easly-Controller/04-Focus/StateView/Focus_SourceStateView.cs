namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Frame;

    /// <summary>
    /// View of a source state.
    /// </summary>
    public interface IFocusSourceStateView : IFrameSourceStateView, IFocusNodeStateView
    {
        /// <summary>
        /// The pattern state.
        /// </summary>
        new IFocusSourceState State { get; }
    }

    /// <summary>
    /// View of a source state.
    /// </summary>
    internal class FocusSourceStateView : FrameSourceStateView, IFocusSourceStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusSourceStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The source state.</param>
        public FocusSourceStateView(IFocusControllerView controllerView, IFocusSourceState state)
            : base(controllerView, state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new IFocusControllerView ControllerView { get { return (IFocusControllerView)base.ControllerView; } }

        /// <summary>
        /// The pattern state.
        /// </summary>
        public new IFocusSourceState State { get { return (IFocusSourceState)base.State; } }
        IFocusNodeState IFocusNodeStateView.State { get { return State; } }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        public new IFocusTemplate Template { get { return (IFocusTemplate)base.Template; } }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        public new IFocusCellView RootCellView { get { return (IFocusCellView)base.RootCellView; } }

        /// <summary>
        /// Table of cell views that are mutable lists of cells.
        /// </summary>
        public new IFocusAssignableCellViewReadOnlyDictionary<string> CellViewTable { get { return (IFocusAssignableCellViewReadOnlyDictionary<string>)base.CellViewTable; } }

        /// <summary>
        /// The cell view that is embedding this state view. Can be null.
        /// </summary>
        public new IFocusContainerCellView ParentContainer { get { return (IFocusContainerCellView)base.ParentContainer; } }

        /// <summary>
        /// Indicates if this view has all its frames forced to visible.
        /// </summary>
        public bool IsUserVisible { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Builds the cell view tree for this view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        public override void BuildRootCellView(IFrameCellViewTreeContext context)
        {
            //Debug.Assert(((IFocusCellViewTreeContext)context).IsVisible);
            base.BuildRootCellView(context);
        }

        /// <summary>
        /// Updates the focus chain with cells in the tree.
        /// </summary>
        /// <param name="focusChain">The list of focusable cell views found in the tree.</param>
        /// <param name="focusedNode">The currently focused node.</param>
        /// <param name="focusedFrame">The currently focused frame in the template associated to <paramref name="focusedNode"/>.</param>
        /// <param name="matchingFocus">The focus in <paramref name="focusChain"/> that match <paramref name="focusedNode"/> and <paramref name="focusedFrame"/> upon return.</param>
        public virtual void UpdateFocusChain(IFocusFocusList focusChain, INode focusedNode, IFocusFrame focusedFrame, ref IFocusFocus matchingFocus)
        {
            Debug.Assert(RootCellView != null);

            RootCellView.UpdateFocusChain(focusChain, focusedNode, focusedFrame, ref matchingFocus);
        }

        /// <summary>
        /// Sets the <see cref="IsUserVisible"/> flag.
        /// </summary>
        /// <param name="isUserVisible">The new value.</param>
        public virtual void SetIsUserVisible(bool isUserVisible)
        {
            IsUserVisible = isUserVisible;
        }

        /// <summary>
        /// Gets the selector stack corresponding to this view and all its parent.
        /// </summary>
        public virtual IList<IFocusFrameSelectorList> GetSelectorStack()
        {
            List<IFocusFrameSelectorList> SelectorStack = new List<IFocusFrameSelectorList>();
            IFocusNodeStateView CurrentStateView = this;
            bool Continue = true;

            while (Continue)
            {
                bool IsHandled = false;

                switch (CurrentStateView.State)
                {
                    case IFocusPlaceholderNodeState AsPlaceholderNodeState:
                    case IFocusOptionalNodeState AsOptionalNodeState:
                        Continue = UpdateSelectorStackNodeState(SelectorStack, ref CurrentStateView);
                        IsHandled = true;
                        break;

                    case IFocusSourceState AsSourceState:
                        Continue = UpdateSelectorStackBlockState(SelectorStack, AsSourceState.ParentBlockState, AsSourceState.ParentInner, ref CurrentStateView);
                        IsHandled = true;
                        break;
                }

                Debug.Assert(IsHandled);
            }

            return SelectorStack;
        }

        /// <summary></summary>
        protected virtual bool UpdateSelectorStackNodeState(List<IFocusFrameSelectorList> selectorStack, ref IFocusNodeStateView currentStateView)
        {
            IFocusInner ParentInner = currentStateView.State.ParentInner;
            IFocusNodeState ParentState = currentStateView.State.ParentState;
            if (ParentInner == null)
            {
                Debug.Assert(ParentState == null);
                return false;
            }

            Debug.Assert(ParentState != null);

            currentStateView = ControllerView.StateViewTable[ParentState];
            IFocusNodeTemplate Template = currentStateView.Template as IFocusNodeTemplate;
            Debug.Assert(Template != null);

            if (Template.FrameSelectorForProperty(ParentInner.PropertyName, out IFocusFrameWithSelector Frame))
                if (Frame != null)
                    if (Frame.Selectors.Count > 0)
                        selectorStack.Insert(0, Frame.Selectors);

            return true;
        }

        /// <summary></summary>
        protected virtual bool UpdateSelectorStackBlockState(List<IFocusFrameSelectorList> selectorStack, IFocusBlockState blockState, IFocusInner inner, ref IFocusNodeStateView currentStateView)
        {
            Debug.Assert(ControllerView.TemplateSet.BlockTemplateTable.ContainsKey(blockState.ParentInner.BlockType));
            IFocusBlockTemplate Template = ControllerView.TemplateSet.BlockTemplateTable[blockState.ParentInner.BlockType] as IFocusBlockTemplate;
            Debug.Assert(Template != null);

            IFocusNodeState ParentState = blockState.ParentInner.Owner;
            Debug.Assert(ParentState != null);

            currentStateView = ControllerView.StateViewTable[ParentState];

            if (Template.FrameSelectorForProperty(inner.PropertyName, out IFocusFrameWithSelector Frame))
                if (Frame != null)
                    if (Frame.Selectors.Count > 0)
                        selectorStack.Insert(0, Frame.Selectors);

            return true;
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusSourceStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusSourceStateView AsSourceStateView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsSourceStateView))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxAssignableCellViewDictionary{string} object.
        /// </summary>
        private protected override IFrameAssignableCellViewDictionary<string> CreateCellViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSourceStateView));
            return new FocusAssignableCellViewDictionary<string>();
        }
        #endregion
    }
}
