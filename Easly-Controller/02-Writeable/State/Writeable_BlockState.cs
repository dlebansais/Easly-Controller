namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.ReadOnly;

    /// <summary>
    /// State of a block in a block list.
    /// </summary>
    public interface IWriteableBlockState : IReadOnlyBlockState
    {
        /// <summary>
        /// The parent inner containing this state.
        /// </summary>
        new IWriteableBlockListInner ParentInner { get; }

        /// <summary>
        /// Index that was used to create the pattern state for this block.
        /// </summary>
        new IWriteableBrowsingPatternIndex PatternIndex { get; }

        /// <summary>
        /// The pattern state for this block.
        /// </summary>
        new IWriteablePatternState PatternState { get; }

        /// <summary>
        /// Index that was used to create the source state for this block.
        /// </summary>
        new IWriteableBrowsingSourceIndex SourceIndex { get; }

        /// <summary>
        /// The source state for this block.
        /// </summary>
        new IWriteableSourceState SourceState { get; }

        /// <summary>
        /// States for nodes in the block.
        /// </summary>
        new WriteablePlaceholderNodeStateReadOnlyList StateList { get; }

        /// <summary>
        /// Inserts a new node in a block.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to insert.</param>
        /// <param name="index">Position of the inserted node in the block.</param>
        /// <param name="childState">Node state.</param>
        void Insert(IWriteableBrowsingBlockNodeIndex nodeIndex, int index, IReadOnlyPlaceholderNodeState childState);

        /// <summary>
        /// Removes a node from a block.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to remove.</param>
        /// <param name="index">Position of the removed node in the block.</param>
        void Remove(IWriteableBrowsingBlockNodeIndex nodeIndex, int index);

        /// <summary>
        /// Moves a node around in a block.
        /// </summary>
        /// <param name="nodeIndex">Index for the moved node.</param>
        /// <param name="index">Position of the moved node in the block.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        void Move(IWriteableBrowsingCollectionNodeIndex nodeIndex, int index, int direction);
    }

    /// <summary>
    /// State of a block in a block list.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the block state.</typeparam>
    internal interface IWriteableBlockState<out IInner> : IReadOnlyBlockState<IInner>
        where IInner : IWriteableInner<IWriteableBrowsingChildIndex>
    {
    }

    /// <summary>
    /// State of a block in a block list.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the block state.</typeparam>
    internal class WriteableBlockState<IInner> : ReadOnlyBlockState<IInner>, IWriteableBlockState<IInner>, IWriteableBlockState
        where IInner : IWriteableInner<IWriteableBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBlockState{IInner}"/> class.
        /// </summary>
        /// <param name="parentInner">Inner containing the block state.</param>
        /// <param name="newBlockIndex">Index that was used to create the block state.</param>
        /// <param name="childBlock">The block.</param>
        public WriteableBlockState(IWriteableBlockListInner parentInner, IWriteableBrowsingNewBlockNodeIndex newBlockIndex, IBlock childBlock)
            : base(parentInner, newBlockIndex, childBlock)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The parent inner containing this state.
        /// </summary>
        public new IWriteableBlockListInner ParentInner { get { return (IWriteableBlockListInner)base.ParentInner; } }

        /// <summary>
        /// Index that was used to create the pattern state for this block.
        /// </summary>
        public new IWriteableBrowsingPatternIndex PatternIndex { get { return (IWriteableBrowsingPatternIndex)base.PatternIndex; } }

        /// <summary>
        /// The pattern state for this block.
        /// </summary>
        public new IWriteablePatternState PatternState { get { return (IWriteablePatternState)base.PatternState; } }

        /// <summary>
        /// Index that was used to create the source state for this block.
        /// </summary>
        public new IWriteableBrowsingSourceIndex SourceIndex { get { return (IWriteableBrowsingSourceIndex)base.SourceIndex; } }

        /// <summary>
        /// The source state for this block.
        /// </summary>
        public new IWriteableSourceState SourceState { get { return (IWriteableSourceState)base.SourceState; } }

        /// <summary>
        /// States for nodes in the block.
        /// </summary>
        public new WriteablePlaceholderNodeStateReadOnlyList StateList { get { return (WriteablePlaceholderNodeStateReadOnlyList)base.StateList; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Inserts a new node in a block.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to insert.</param>
        /// <param name="index">Position of the inserted node in the block.</param>
        /// <param name="childState">Node state.</param>
        public virtual void Insert(IWriteableBrowsingBlockNodeIndex nodeIndex, int index, IReadOnlyPlaceholderNodeState childState)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(index >= 0 && index <= StateList.Count);

            InsertState(index, childState);
        }

        /// <summary>
        /// Removes a node from a block.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to remove.</param>
        /// <param name="index">Position of the removed node in the block.</param>
        public virtual void Remove(IWriteableBrowsingBlockNodeIndex nodeIndex, int index)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(index >= 0 && index < StateList.Count);

            RemoveState(index);
        }

        /// <summary>
        /// Moves a node around in a block.
        /// </summary>
        /// <param name="nodeIndex">Index for the moved node.</param>
        /// <param name="index">Position of the moved node in the block.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        public virtual void Move(IWriteableBrowsingCollectionNodeIndex nodeIndex, int index, int direction)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(index >= 0 && index < StateList.Count);
            Debug.Assert(index + direction >= 0 && index + direction < StateList.Count);

            MoveState(index, direction);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateList object.
        /// </summary>
        private protected override ReadOnlyPlaceholderNodeStateList CreateStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockState<IInner>));
            return new WriteablePlaceholderNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxInnerDictionary{string} object.
        /// </summary>
        private protected override ReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockState<IInner>));
            return new WriteableInnerDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreatePatternInner(IReadOnlyNodeState owner)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockState<IInner>));
            return new WriteablePlaceholderInner<IWriteableBrowsingPlaceholderNodeIndex>((IWriteableNodeState)owner, nameof(IBlock.ReplicationPattern));
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreateSourceInner(IReadOnlyNodeState owner)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockState<IInner>));
            return new WriteablePlaceholderInner<IWriteableBrowsingPlaceholderNodeIndex>((IWriteableNodeState)owner, nameof(IBlock.SourceIdentifier));
        }

        /// <summary>
        /// Creates a IxxxBrowsingPatternIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingPatternIndex CreateExistingPatternIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockState<IInner>));
            return new WriteableBrowsingPatternIndex(ChildBlock);
        }

        /// <summary>
        /// Creates a IxxxBrowsingSourceIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingSourceIndex CreateExistingSourceIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockState<IInner>));
            return new WriteableBrowsingSourceIndex(ChildBlock);
        }

        /// <summary>
        /// Creates a IxxxPatternState object.
        /// </summary>
        private protected override IReadOnlyPatternState CreatePatternState(IReadOnlyBrowsingPatternIndex patternIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockState<IInner>));
            return new WriteablePatternState<IInner>(this, (IWriteableBrowsingPatternIndex)patternIndex);
        }

        /// <summary>
        /// Creates a IxxxSourceState object.
        /// </summary>
        private protected override IReadOnlySourceState CreateSourceState(IReadOnlyBrowsingSourceIndex sourceIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockState<IInner>));
            return new WriteableSourceState<IInner>(this, (IWriteableBrowsingSourceIndex)sourceIndex);
        }
        #endregion
    }
}
