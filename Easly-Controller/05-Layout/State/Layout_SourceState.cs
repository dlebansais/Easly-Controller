namespace EaslyController.Layout
{
    using EaslyController.Focus;
    using EaslyController.ReadOnly;
    using NotNullReflection;

    /// <summary>
    /// State of a source identifier node.
    /// </summary>
    public interface ILayoutSourceState : IFocusSourceState, ILayoutNodeState
    {
        /// <summary>
        /// The parent block state.
        /// </summary>
        new ILayoutBlockState ParentBlockState { get; }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new ILayoutBrowsingSourceIndex ParentIndex { get; }
    }

    /// <summary>
    /// State of a source identifier node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal interface ILayoutSourceState<out IInner> : IFocusSourceState<IInner>, ILayoutPlaceholderNodeState<IInner>
        where IInner : ILayoutInner<ILayoutBrowsingChildIndex>
    {
    }

    /// <summary>
    /// State of a source identifier node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal class LayoutSourceState<IInner> : FocusSourceState<IInner>, ILayoutSourceState<IInner>, ILayoutSourceState
        where IInner : ILayoutInner<ILayoutBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutSourceState{IInner}"/> class.
        /// </summary>
        /// <param name="parentBlockState">The parent block state.</param>
        /// <param name="index">The index used to create the state.</param>
        public LayoutSourceState(ILayoutBlockState parentBlockState, ILayoutBrowsingSourceIndex index)
            : base(parentBlockState, index)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The parent block state.
        /// </summary>
        public new ILayoutBlockState ParentBlockState { get { return (ILayoutBlockState)base.ParentBlockState; } }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new ILayoutBrowsingSourceIndex ParentIndex { get { return (ILayoutBrowsingSourceIndex)base.ParentIndex; } }
        ILayoutIndex ILayoutNodeState.ParentIndex { get { return ParentIndex; } }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        public new ILayoutInner ParentInner { get { return (ILayoutInner)base.ParentInner; } }

        /// <summary>
        /// State of the parent.
        /// </summary>
        public new ILayoutNodeState ParentState { get { return (ILayoutNodeState)base.ParentState; } }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        public new LayoutInnerReadOnlyDictionary<string> InnerTable { get { return (LayoutInnerReadOnlyDictionary<string>)base.InnerTable; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeStateList object.
        /// </summary>
        private protected override ReadOnlyNodeStateList CreateNodeStateList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutSourceState<IInner>>());
            return new LayoutNodeStateList();
        }
        #endregion
    }
}
