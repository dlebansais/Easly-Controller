﻿namespace EaslyController.Focus
{
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using NotNullReflection;

    /// <summary>
    /// State of a block in a block list.
    /// </summary>
    public interface IFocusBlockState : IFrameBlockState
    {
        /// <summary>
        /// The parent inner containing this state.
        /// </summary>
        new IFocusBlockListInner ParentInner { get; }

        /// <summary>
        /// Index that was used to create the pattern state for this block.
        /// </summary>
        new IFocusBrowsingPatternIndex PatternIndex { get; }

        /// <summary>
        /// The pattern state for this block.
        /// </summary>
        new IFocusPatternState PatternState { get; }

        /// <summary>
        /// Index that was used to create the source state for this block.
        /// </summary>
        new IFocusBrowsingSourceIndex SourceIndex { get; }

        /// <summary>
        /// The source state for this block.
        /// </summary>
        new IFocusSourceState SourceState { get; }

        /// <summary>
        /// States for nodes in the block.
        /// </summary>
        new FocusPlaceholderNodeStateReadOnlyList StateList { get; }
    }

    /// <summary>
    /// State of a block in a block list.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the block state.</typeparam>
    internal interface IFocusBlockState<out IInner> : IFrameBlockState<IInner>
        where IInner : IFocusInner<IFocusBrowsingChildIndex>
    {
    }

    /// <summary>
    /// State of a block in a block list.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the block state.</typeparam>
    internal class FocusBlockState<IInner> : FrameBlockState<IInner>, IFocusBlockState<IInner>, IFocusBlockState
        where IInner : IFocusInner<IFocusBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FocusBlockState{IInner}"/> object.
        /// </summary>
        public static new FocusBlockState<IInner> Empty { get; } = new FocusBlockState<IInner>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBlockState{IInner}"/> class.
        /// </summary>
        private FocusBlockState()
            : this(FocusBlockListInner<IFocusBrowsingBlockNodeIndex>.Empty, FocusBrowsingNewBlockNodeIndex.Empty, (IBlock)Block<Node>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBlockState{IInner}"/> class.
        /// </summary>
        /// <param name="parentInner">Inner containing the block state.</param>
        /// <param name="newBlockIndex">Index that was used to create the block state.</param>
        /// <param name="childBlock">The block.</param>
        public FocusBlockState(IFocusBlockListInner parentInner, IFocusBrowsingNewBlockNodeIndex newBlockIndex, IBlock childBlock)
            : base(parentInner, newBlockIndex, childBlock)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The parent inner containing this state.
        /// </summary>
        public new IFocusBlockListInner ParentInner { get { return (IFocusBlockListInner)base.ParentInner; } }

        /// <summary>
        /// Index that was used to create the pattern state for this block.
        /// </summary>
        public new IFocusBrowsingPatternIndex PatternIndex { get { return (IFocusBrowsingPatternIndex)base.PatternIndex; } }

        /// <summary>
        /// The pattern state for this block.
        /// </summary>
        public new IFocusPatternState PatternState { get { return (IFocusPatternState)base.PatternState; } }

        /// <summary>
        /// Index that was used to create the source state for this block.
        /// </summary>
        public new IFocusBrowsingSourceIndex SourceIndex { get { return (IFocusBrowsingSourceIndex)base.SourceIndex; } }

        /// <summary>
        /// The source state for this block.
        /// </summary>
        public new IFocusSourceState SourceState { get { return (IFocusSourceState)base.SourceState; } }

        /// <summary>
        /// States for nodes in the block.
        /// </summary>
        public new FocusPlaceholderNodeStateReadOnlyList StateList { get { return (FocusPlaceholderNodeStateReadOnlyList)base.StateList; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateList object.
        /// </summary>
        private protected override ReadOnlyPlaceholderNodeStateList CreateStateList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusBlockState<IInner>>());
            return new FocusPlaceholderNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxInnerDictionary{string} object.
        /// </summary>
        private protected override ReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusBlockState<IInner>>());
            return new FocusInnerDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreatePatternInner(IReadOnlyNodeState owner)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusBlockState<IInner>>());
            return new FocusPlaceholderInner<IFocusBrowsingPlaceholderNodeIndex>((IFocusNodeState)owner, nameof(IBlock.ReplicationPattern));
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreateSourceInner(IReadOnlyNodeState owner)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusBlockState<IInner>>());
            return new FocusPlaceholderInner<IFocusBrowsingPlaceholderNodeIndex>((IFocusNodeState)owner, nameof(IBlock.SourceIdentifier));
        }

        /// <summary>
        /// Creates a IxxxBrowsingPatternIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingPatternIndex CreateExistingPatternIndex()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusBlockState<IInner>>());
            return new FocusBrowsingPatternIndex(ChildBlock);
        }

        /// <summary>
        /// Creates a IxxxBrowsingSourceIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingSourceIndex CreateExistingSourceIndex()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusBlockState<IInner>>());
            return new FocusBrowsingSourceIndex(ChildBlock);
        }

        /// <summary>
        /// Creates a IxxxPatternState object.
        /// </summary>
        private protected override IReadOnlyPatternState CreatePatternState(IReadOnlyBrowsingPatternIndex patternIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusBlockState<IInner>>());
            return new FocusPatternState<IInner>(this, (IFocusBrowsingPatternIndex)patternIndex);
        }

        /// <summary>
        /// Creates a IxxxSourceState object.
        /// </summary>
        private protected override IReadOnlySourceState CreateSourceState(IReadOnlyBrowsingSourceIndex sourceIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusBlockState<IInner>>());
            return new FocusSourceState<IInner>(this, (IFocusBrowsingSourceIndex)sourceIndex);
        }
        #endregion
    }
}
