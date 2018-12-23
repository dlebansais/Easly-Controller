﻿using BaseNode;
using BaseNodeHelper;
using EaslyController.ReadOnly;
using System.Diagnostics;

namespace EaslyController.Writeable
{
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
        /// Index that was used to create the block state.
        /// </summary>
        new IWriteableBrowsingNewBlockNodeIndex ParentIndex { get; }

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
        new IWriteablePlaceholderNodeStateReadOnlyList StateList { get; }

        /// <summary>
        /// Inserts a new node in a list or block list.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to insert.</param>
        /// <param name="childState">Node state.</param>
        /// <param name="index">Position of the inserted node in the list.</param>
        void Insert(IWriteableBrowsingCollectionNodeIndex nodeIndex, IReadOnlyPlaceholderNodeState childState, int index);
    }

    /// <summary>
    /// State of a block in a block list.
    /// </summary>
    public class WriteableBlockState : ReadOnlyBlockState, IWriteableBlockState
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBlockState"/> class.
        /// </summary>
        /// <param name="parentInner">Inner containing the block state.</param>
        /// <param name="parentIndex">Index that was used to create the block state.</param>
        /// <param name="childBlock">The block.</param>
        public WriteableBlockState(IWriteableBlockListInner parentInner, IWriteableBrowsingNewBlockNodeIndex parentIndex, IBlock childBlock)
            : base(parentInner, parentIndex, childBlock)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The parent inner containing this state.
        /// </summary>
        public new IWriteableBlockListInner ParentInner { get { return (IWriteableBlockListInner)base.ParentInner; } }

        /// <summary>
        /// Index that was used to create the block state.
        /// </summary>
        public new IWriteableBrowsingNewBlockNodeIndex ParentIndex { get { return (IWriteableBrowsingNewBlockNodeIndex)base.ParentIndex; } }

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
        public new IWriteablePlaceholderNodeStateReadOnlyList StateList { get { return (IWriteablePlaceholderNodeStateReadOnlyList)base.StateList; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Inserts a new node in a list or block list.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to insert.</param>
        /// <param name="childState">Node state.</param>
        /// <param name="index">Position of the inserted node in the list.</param>
        public virtual void Insert(IWriteableBrowsingCollectionNodeIndex nodeIndex, IReadOnlyPlaceholderNodeState childState, int index)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(index >= 0 && index <= StateList.Count);

            InsertState(index, childState);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateList object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeStateList CreateStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockState));
            return new WriteablePlaceholderNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateReadOnlyList object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeStateReadOnlyList CreateStateListReadOnly(IReadOnlyPlaceholderNodeStateList stateList)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockState));
            return new WriteablePlaceholderNodeStateReadOnlyList((IWriteablePlaceholderNodeStateList)stateList);
        }

        /// <summary>
        /// Creates a IxxxInnerDictionary{string} object.
        /// </summary>
        protected override IReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockState));
            return new WriteableInnerDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxInnerReadOnlyDictionary{string} object.
        /// </summary>
        protected override IReadOnlyInnerReadOnlyDictionary<string> CreateInnerTableReadOnly(IReadOnlyInnerDictionary<string> innerTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockState));
            return new WriteableInnerReadOnlyDictionary<string>((IWriteableInnerDictionary<string>)innerTable);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        protected override IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreatePatternInner(IReadOnlyNodeState owner)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockState));
            return new WriteablePlaceholderInner<IWriteableBrowsingPlaceholderNodeIndex, WriteableBrowsingPlaceholderNodeIndex>((IWriteableNodeState)owner, nameof(IBlock.ReplicationPattern));
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        protected override IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreateSourceInner(IReadOnlyNodeState owner)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockState));
            return new WriteablePlaceholderInner<IWriteableBrowsingPlaceholderNodeIndex, WriteableBrowsingPlaceholderNodeIndex>((IWriteableNodeState)owner, nameof(IBlock.SourceIdentifier));
        }

        /// <summary>
        /// Creates a IxxxBrowsingPatternIndex object.
        /// </summary>
        protected override IReadOnlyBrowsingPatternIndex CreateExistingPatternIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockState));
            return new WriteableBrowsingPatternIndex(ChildBlock);
        }

        /// <summary>
        /// Creates a IxxxBrowsingSourceIndex object.
        /// </summary>
        protected override IReadOnlyBrowsingSourceIndex CreateExistingSourceIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockState));
            return new WriteableBrowsingSourceIndex(ChildBlock);
        }

        /// <summary>
        /// Creates a IxxxPatternState object.
        /// </summary>
        protected override IReadOnlyPatternState CreatePatternState(IReadOnlyBrowsingPatternIndex patternIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockState));
            return new WriteablePatternState(this, (IWriteableBrowsingPatternIndex)patternIndex);
        }

        /// <summary>
        /// Creates a IxxxSourceState object.
        /// </summary>
        protected override IReadOnlySourceState CreateSourceState(IReadOnlyBrowsingSourceIndex sourceIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockState));
            return new WriteableSourceState(this, (IWriteableBrowsingSourceIndex)sourceIndex);
        }
        #endregion
    }
}