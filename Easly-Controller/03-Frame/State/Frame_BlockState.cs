﻿namespace EaslyController.Frame
{
    using BaseNode;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <summary>
    /// State of a block in a block list.
    /// </summary>
    public interface IFrameBlockState : IWriteableBlockState
    {
        /// <summary>
        /// The parent inner containing this state.
        /// </summary>
        new IFrameBlockListInner ParentInner { get; }

        /// <summary>
        /// Index that was used to create the pattern state for this block.
        /// </summary>
        new IFrameBrowsingPatternIndex PatternIndex { get; }

        /// <summary>
        /// The pattern state for this block.
        /// </summary>
        new IFramePatternState PatternState { get; }

        /// <summary>
        /// Index that was used to create the source state for this block.
        /// </summary>
        new IFrameBrowsingSourceIndex SourceIndex { get; }

        /// <summary>
        /// The source state for this block.
        /// </summary>
        new IFrameSourceState SourceState { get; }

        /// <summary>
        /// States for nodes in the block.
        /// </summary>
        new FramePlaceholderNodeStateReadOnlyList StateList { get; }
    }

    /// <summary>
    /// State of a block in a block list.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the block state.</typeparam>
    internal interface IFrameBlockState<out IInner> : IWriteableBlockState<IInner>
        where IInner : IFrameInner<IFrameBrowsingChildIndex>
    {
    }

    /// <summary>
    /// State of a block in a block list.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the block state.</typeparam>
    internal class FrameBlockState<IInner> : WriteableBlockState<IInner>, IFrameBlockState<IInner>, IFrameBlockState
        where IInner : IFrameInner<IFrameBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FrameBlockState{IInner}"/> object.
        /// </summary>
        public static new FrameBlockState<IInner> Empty { get; } = new FrameBlockState<IInner>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBlockState{IInner}"/> class.
        /// </summary>
        private FrameBlockState()
            : this(FrameBlockListInner<IFrameBrowsingBlockNodeIndex>.Empty, FrameBrowsingNewBlockNodeIndex.Empty, (IBlock)Block<Node>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBlockState{IInner}"/> class.
        /// </summary>
        /// <param name="parentInner">Inner containing the block state.</param>
        /// <param name="newBlockIndex">Index that was used to create the block state.</param>
        /// <param name="childBlock">The block.</param>
        public FrameBlockState(IFrameBlockListInner parentInner, IFrameBrowsingNewBlockNodeIndex newBlockIndex, IBlock childBlock)
            : base(parentInner, newBlockIndex, childBlock)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The parent inner containing this state.
        /// </summary>
        public new IFrameBlockListInner ParentInner { get { return (IFrameBlockListInner)base.ParentInner; } }

        /// <summary>
        /// Index that was used to create the pattern state for this block.
        /// </summary>
        public new IFrameBrowsingPatternIndex PatternIndex { get { return (IFrameBrowsingPatternIndex)base.PatternIndex; } }

        /// <summary>
        /// The pattern state for this block.
        /// </summary>
        public new IFramePatternState PatternState { get { return (IFramePatternState)base.PatternState; } }

        /// <summary>
        /// Index that was used to create the source state for this block.
        /// </summary>
        public new IFrameBrowsingSourceIndex SourceIndex { get { return (IFrameBrowsingSourceIndex)base.SourceIndex; } }

        /// <summary>
        /// The source state for this block.
        /// </summary>
        public new IFrameSourceState SourceState { get { return (IFrameSourceState)base.SourceState; } }

        /// <summary>
        /// States for nodes in the block.
        /// </summary>
        public new FramePlaceholderNodeStateReadOnlyList StateList { get { return (FramePlaceholderNodeStateReadOnlyList)base.StateList; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateList object.
        /// </summary>
        private protected override ReadOnlyPlaceholderNodeStateList CreateStateList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameBlockState<IInner>>());
            return new FramePlaceholderNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxInnerDictionary{string} object.
        /// </summary>
        private protected override ReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameBlockState<IInner>>());
            return new FrameInnerDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreatePatternInner(IReadOnlyNodeState owner)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameBlockState<IInner>>());
            return new FramePlaceholderInner<IFrameBrowsingPlaceholderNodeIndex>((IFrameNodeState)owner, nameof(IBlock.ReplicationPattern));
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreateSourceInner(IReadOnlyNodeState owner)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameBlockState<IInner>>());
            return new FramePlaceholderInner<IFrameBrowsingPlaceholderNodeIndex>((IFrameNodeState)owner, nameof(IBlock.SourceIdentifier));
        }

        /// <summary>
        /// Creates a IxxxBrowsingPatternIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingPatternIndex CreateExistingPatternIndex()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameBlockState<IInner>>());
            return new FrameBrowsingPatternIndex(ChildBlock);
        }

        /// <summary>
        /// Creates a IxxxBrowsingSourceIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingSourceIndex CreateExistingSourceIndex()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameBlockState<IInner>>());
            return new FrameBrowsingSourceIndex(ChildBlock);
        }

        /// <summary>
        /// Creates a IxxxPatternState object.
        /// </summary>
        private protected override IReadOnlyPatternState CreatePatternState(IReadOnlyBrowsingPatternIndex patternIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameBlockState<IInner>>());
            return new FramePatternState<IInner>(this, (IFrameBrowsingPatternIndex)patternIndex);
        }

        /// <summary>
        /// Creates a IxxxSourceState object.
        /// </summary>
        private protected override IReadOnlySourceState CreateSourceState(IReadOnlyBrowsingSourceIndex sourceIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameBlockState<IInner>>());
            return new FrameSourceState<IInner>(this, (IFrameBrowsingSourceIndex)sourceIndex);
        }
        #endregion
    }
}
