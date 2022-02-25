namespace EaslyController.Layout
{
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;
    using NotNullReflection;

    /// <summary>
    /// State of a block in a block list.
    /// </summary>
    public interface ILayoutBlockState : IFocusBlockState
    {
        /// <summary>
        /// The parent inner containing this state.
        /// </summary>
        new ILayoutBlockListInner ParentInner { get; }

        /// <summary>
        /// Index that was used to create the pattern state for this block.
        /// </summary>
        new ILayoutBrowsingPatternIndex PatternIndex { get; }

        /// <summary>
        /// The pattern state for this block.
        /// </summary>
        new ILayoutPatternState PatternState { get; }

        /// <summary>
        /// Index that was used to create the source state for this block.
        /// </summary>
        new ILayoutBrowsingSourceIndex SourceIndex { get; }

        /// <summary>
        /// The source state for this block.
        /// </summary>
        new ILayoutSourceState SourceState { get; }

        /// <summary>
        /// States for nodes in the block.
        /// </summary>
        new LayoutPlaceholderNodeStateReadOnlyList StateList { get; }
    }

    /// <summary>
    /// State of a block in a block list.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the block state.</typeparam>
    internal interface ILayoutBlockState<out IInner> : IFocusBlockState<IInner>
        where IInner : ILayoutInner<ILayoutBrowsingChildIndex>
    {
    }

    /// <summary>
    /// State of a block in a block list.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the block state.</typeparam>
    internal class LayoutBlockState<IInner> : FocusBlockState<IInner>, ILayoutBlockState<IInner>, ILayoutBlockState
        where IInner : ILayoutInner<ILayoutBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="LayoutBlockState{IInner}"/> object.
        /// </summary>
        public static new LayoutBlockState<IInner> Empty { get; } = new LayoutBlockState<IInner>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBlockState{IInner}"/> class.
        /// </summary>
        private LayoutBlockState()
            : this(LayoutBlockListInner<ILayoutBrowsingBlockNodeIndex>.Empty, LayoutBrowsingNewBlockNodeIndex.Empty, (IBlock)Block<Node>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBlockState{IInner}"/> class.
        /// </summary>
        /// <param name="parentInner">Inner containing the block state.</param>
        /// <param name="newBlockIndex">Index that was used to create the block state.</param>
        /// <param name="childBlock">The block.</param>
        public LayoutBlockState(ILayoutBlockListInner parentInner, ILayoutBrowsingNewBlockNodeIndex newBlockIndex, IBlock childBlock)
            : base(parentInner, newBlockIndex, childBlock)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The parent inner containing this state.
        /// </summary>
        public new ILayoutBlockListInner ParentInner { get { return (ILayoutBlockListInner)base.ParentInner; } }

        /// <summary>
        /// Index that was used to create the pattern state for this block.
        /// </summary>
        public new ILayoutBrowsingPatternIndex PatternIndex { get { return (ILayoutBrowsingPatternIndex)base.PatternIndex; } }

        /// <summary>
        /// The pattern state for this block.
        /// </summary>
        public new ILayoutPatternState PatternState { get { return (ILayoutPatternState)base.PatternState; } }

        /// <summary>
        /// Index that was used to create the source state for this block.
        /// </summary>
        public new ILayoutBrowsingSourceIndex SourceIndex { get { return (ILayoutBrowsingSourceIndex)base.SourceIndex; } }

        /// <summary>
        /// The source state for this block.
        /// </summary>
        public new ILayoutSourceState SourceState { get { return (ILayoutSourceState)base.SourceState; } }

        /// <summary>
        /// States for nodes in the block.
        /// </summary>
        public new LayoutPlaceholderNodeStateReadOnlyList StateList { get { return (LayoutPlaceholderNodeStateReadOnlyList)base.StateList; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateList object.
        /// </summary>
        private protected override ReadOnlyPlaceholderNodeStateList CreateStateList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutBlockState<IInner>>());
            return new LayoutPlaceholderNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxInnerDictionary{string} object.
        /// </summary>
        private protected override ReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutBlockState<IInner>>());
            return new LayoutInnerDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreatePatternInner(IReadOnlyNodeState owner)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutBlockState<IInner>>());
            return new LayoutPlaceholderInner<ILayoutBrowsingPlaceholderNodeIndex>((ILayoutNodeState)owner, nameof(IBlock.ReplicationPattern));
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreateSourceInner(IReadOnlyNodeState owner)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutBlockState<IInner>>());
            return new LayoutPlaceholderInner<ILayoutBrowsingPlaceholderNodeIndex>((ILayoutNodeState)owner, nameof(IBlock.SourceIdentifier));
        }

        /// <summary>
        /// Creates a IxxxBrowsingPatternIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingPatternIndex CreateExistingPatternIndex()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutBlockState<IInner>>());
            return new LayoutBrowsingPatternIndex(ChildBlock);
        }

        /// <summary>
        /// Creates a IxxxBrowsingSourceIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingSourceIndex CreateExistingSourceIndex()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutBlockState<IInner>>());
            return new LayoutBrowsingSourceIndex(ChildBlock);
        }

        /// <summary>
        /// Creates a IxxxPatternState object.
        /// </summary>
        private protected override IReadOnlyPatternState CreatePatternState(IReadOnlyBrowsingPatternIndex patternIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutBlockState<IInner>>());
            return new LayoutPatternState<IInner>(this, (ILayoutBrowsingPatternIndex)patternIndex);
        }

        /// <summary>
        /// Creates a IxxxSourceState object.
        /// </summary>
        private protected override IReadOnlySourceState CreateSourceState(IReadOnlyBrowsingSourceIndex sourceIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutBlockState<IInner>>());
            return new LayoutSourceState<IInner>(this, (ILayoutBrowsingSourceIndex)sourceIndex);
        }
        #endregion
    }
}
