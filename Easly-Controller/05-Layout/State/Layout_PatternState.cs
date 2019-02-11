namespace EaslyController.Layout
{
    using EaslyController.Focus;
    using EaslyController.ReadOnly;

    /// <summary>
    /// State of an replication pattern node.
    /// </summary>
    public interface ILayoutPatternState : IFocusPatternState, ILayoutNodeState
    {
        /// <summary>
        /// The parent block state.
        /// </summary>
        new ILayoutBlockState ParentBlockState { get; }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new ILayoutBrowsingPatternIndex ParentIndex { get; }
    }

    /// <summary>
    /// State of an replication pattern node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal interface ILayoutPatternState<out IInner> : IFocusPatternState<IInner>, ILayoutPlaceholderNodeState<IInner>
        where IInner : ILayoutInner<ILayoutBrowsingChildIndex>
    {
    }

    /// <summary>
    /// State of an replication pattern node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal class LayoutPatternState<IInner> : FocusPatternState<IInner>, ILayoutPatternState<IInner>, ILayoutPatternState
        where IInner : ILayoutInner<ILayoutBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutPatternState{IInner}"/> class.
        /// </summary>
        /// <param name="parentBlockState">The parent block state.</param>
        /// <param name="index">The index used to create the state.</param>
        public LayoutPatternState(ILayoutBlockState parentBlockState, ILayoutBrowsingPatternIndex index)
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
        public new ILayoutBrowsingPatternIndex ParentIndex { get { return (ILayoutBrowsingPatternIndex)base.ParentIndex; } }
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
        public new ILayoutInnerReadOnlyDictionary<string> InnerTable { get { return (ILayoutInnerReadOnlyDictionary<string>)base.InnerTable; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeStateList object.
        /// </summary>
        private protected override IReadOnlyNodeStateList CreateNodeStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutPatternState<IInner>));
            return new LayoutNodeStateList();
        }
        #endregion
    }
}
