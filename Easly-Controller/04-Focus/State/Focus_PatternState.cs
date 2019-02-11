namespace EaslyController.Focus
{
    using EaslyController.Frame;
    using EaslyController.ReadOnly;

    /// <summary>
    /// State of an replication pattern node.
    /// </summary>
    public interface IFocusPatternState : IFramePatternState, IFocusNodeState
    {
        /// <summary>
        /// The parent block state.
        /// </summary>
        new IFocusBlockState ParentBlockState { get; }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IFocusBrowsingPatternIndex ParentIndex { get; }
    }

    /// <summary>
    /// State of an replication pattern node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal interface IFocusPatternState<out IInner> : IFramePatternState<IInner>, IFocusPlaceholderNodeState<IInner>
        where IInner : IFocusInner<IFocusBrowsingChildIndex>
    {
    }

    /// <summary>
    /// State of an replication pattern node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal class FocusPatternState<IInner> : FramePatternState<IInner>, IFocusPatternState<IInner>, IFocusPatternState
        where IInner : IFocusInner<IFocusBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusPatternState{IInner}"/> class.
        /// </summary>
        /// <param name="parentBlockState">The parent block state.</param>
        /// <param name="index">The index used to create the state.</param>
        public FocusPatternState(IFocusBlockState parentBlockState, IFocusBrowsingPatternIndex index)
            : base(parentBlockState, index)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The parent block state.
        /// </summary>
        public new IFocusBlockState ParentBlockState { get { return (IFocusBlockState)base.ParentBlockState; } }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new IFocusBrowsingPatternIndex ParentIndex { get { return (IFocusBrowsingPatternIndex)base.ParentIndex; } }
        IFocusIndex IFocusNodeState.ParentIndex { get { return ParentIndex; } }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        public new IFocusInner ParentInner { get { return (IFocusInner)base.ParentInner; } }

        /// <summary>
        /// State of the parent.
        /// </summary>
        public new IFocusNodeState ParentState { get { return (IFocusNodeState)base.ParentState; } }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        public new IFocusInnerReadOnlyDictionary<string> InnerTable { get { return (IFocusInnerReadOnlyDictionary<string>)base.InnerTable; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeStateList object.
        /// </summary>
        private protected override IReadOnlyNodeStateList CreateNodeStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPatternState<IInner>));
            return new FocusNodeStateList();
        }
        #endregion
    }
}
