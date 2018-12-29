using BaseNode;
using BaseNodeHelper;
using EaslyController.ReadOnly;
using System;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Inner for a block list.
    /// </summary>
    public interface IWriteableBlockListInner : IReadOnlyBlockListInner, IWriteableCollectionInner
    {
        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        new IWriteableBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// Called when a block state is created.
        /// </summary>
        new event Action<IWriteableBlockState> BlockStateCreated;

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        new event Action<IWriteableBlockState> BlockStateRemoved;

        /// <summary>
        /// Inserts a new block with one node in a block list.
        /// </summary>
        /// <param name="newBlockIndex">Index of the node to insert.</param>
        /// <param name="browsingIndex">Index of the inserted node upon return.</param>
        /// <param name="blockState">The inserted block state upon return.</param>
        /// <param name="childState">The inserted node state upon return.</param>
        void InsertNew(IWriteableInsertionNewBlockNodeIndex newBlockIndex, out IWriteableBrowsingCollectionNodeIndex browsingIndex, out IWriteableBlockState blockState, out IWriteablePlaceholderNodeState childState);

        /// <summary>
        /// Removes a node from a block list. This method is allowed to remove the last node of a block.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to remove.</param>
        /// <param name="isBlockRemoved">Indicates upon return if the last nodee of the block was removed, and if so, that the block was removed as well.</param>
        /// <param name="patternIndex">If <paramref name="isBlockRemoved"/> is true, index of the removed pattern upon return.</param>
        /// <param name="sourceIndex">If <paramref name="isBlockRemoved"/> is true, index of the removed source upon return.</param>
        void RemoveWithBlock(IWriteableBrowsingBlockNodeIndex nodeIndex, out bool isBlockRemoved, out IWriteableBrowsingPatternIndex patternIndex, out IWriteableBrowsingSourceIndex sourceIndex);
    }

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    public interface IWriteableBlockListInner<out IIndex> : IReadOnlyBlockListInner<IIndex>, IWriteableCollectionInner<IIndex>
        where IIndex : IWriteableBrowsingBlockNodeIndex
    {
        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        new IWriteableBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// Called when a block state is created.
        /// </summary>
        new event Action<IWriteableBlockState> BlockStateCreated;

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        new event Action<IWriteableBlockState> BlockStateRemoved;

        /// <summary>
        /// Inserts a new block with one node in a block list.
        /// </summary>
        /// <param name="newBlockIndex">Index of the node to insert.</param>
        /// <param name="browsingIndex">Index of the inserted node upon return.</param>
        /// <param name="blockState">The inserted block state upon return.</param>
        /// <param name="childState">The inserted node state upon return.</param>
        void InsertNew(IWriteableInsertionNewBlockNodeIndex newBlockIndex, out IWriteableBrowsingCollectionNodeIndex browsingIndex, out IWriteableBlockState blockState, out IWriteablePlaceholderNodeState childState);

        /// <summary>
        /// Removes a node from a block list. This method is allowed to remove the last node of a block.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to remove.</param>
        /// <param name="isBlockRemoved">Indicates upon return if the last nodee of the block was removed, and if so, that the block was removed as well.</param>
        /// <param name="patternIndex">If <paramref name="isBlockRemoved"/> is true, index of the removed pattern upon return.</param>
        /// <param name="sourceIndex">If <paramref name="isBlockRemoved"/> is true, index of the removed source upon return.</param>
        void RemoveWithBlock(IWriteableBrowsingBlockNodeIndex nodeIndex, out bool isBlockRemoved, out IWriteableBrowsingPatternIndex patternIndex, out IWriteableBrowsingSourceIndex sourceIndex);
    }

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    public class WriteableBlockListInner<IIndex, TIndex> : ReadOnlyBlockListInner<IIndex, TIndex>, IWriteableBlockListInner<IIndex>, IWriteableBlockListInner
        where IIndex : IWriteableBrowsingBlockNodeIndex
        where TIndex : WriteableBrowsingBlockNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBlockListInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public WriteableBlockListInner(IWriteableNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        public new IWriteableNodeState Owner { get { return (IWriteableNodeState)base.Owner; } }

        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        public new IWriteableBlockStateReadOnlyList BlockStateList { get { return (IWriteableBlockStateReadOnlyList)base.BlockStateList; } }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        public new IWriteablePlaceholderNodeState FirstNodeState { get { return (IWriteablePlaceholderNodeState)base.FirstNodeState; } }

        /// <summary>
        /// Called when a block state is created.
        /// </summary>
        public new event Action<IWriteableBlockState> BlockStateCreated;

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        public new event Action<IWriteableBlockState> BlockStateRemoved;
        #endregion

        #region Client Interface
        /// <summary>
        /// Inserts a new node in a block list.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to insert.</param>
        /// <param name="browsingIndex">Index of the inserted node upon return.</param>
        /// <param name="childState">The inserted node state upon return.</param>
        public virtual void Insert(IWriteableInsertionCollectionNodeIndex nodeIndex, out IWriteableBrowsingCollectionNodeIndex browsingIndex, out IWriteablePlaceholderNodeState childState)
        {
            Debug.Assert(nodeIndex != null);

            if (nodeIndex is IWriteableInsertionExistingBlockNodeIndex AsExistingBlockIndex)
                InsertExisting(AsExistingBlockIndex, out browsingIndex, out childState);
            else // The case of a new block is already handled.
                throw new ArgumentOutOfRangeException(nameof(nodeIndex));
        }

        /// <summary>
        /// Inserts a new block with one node in a block list.
        /// </summary>
        /// <param name="newBlockIndex">Index of the node to insert.</param>
        /// <param name="browsingIndex">Index of the inserted node upon return.</param>
        /// <param name="blockState">The inserted block state upon return.</param>
        /// <param name="childState">The inserted node state upon return.</param>
        public virtual void InsertNew(IWriteableInsertionNewBlockNodeIndex newBlockIndex, out IWriteableBrowsingCollectionNodeIndex browsingIndex, out IWriteableBlockState blockState, out IWriteablePlaceholderNodeState childState)
        {
            Debug.Assert(newBlockIndex != null);
            Debug.Assert(newBlockIndex.BlockIndex >= 0);
            Debug.Assert(newBlockIndex.BlockIndex <= BlockStateList.Count);

            INode ParentNode = Owner.Node;
            NodeTreeHelperBlockList.InsertIntoBlockList(ParentNode, PropertyName, newBlockIndex.BlockIndex, ReplicationStatus.Normal, newBlockIndex.PatternNode, newBlockIndex.SourceNode, out IBlock ChildBlock);
            NodeTreeHelperBlockList.InsertIntoBlock(ChildBlock, 0, newBlockIndex.Node);

            IWriteableBrowsingNewBlockNodeIndex BrowsingNewBlockIndex = (IWriteableBrowsingNewBlockNodeIndex)newBlockIndex.ToBrowsingIndex();
            IWriteableBrowsingExistingBlockNodeIndex BrowsingExistingBlockIndex = (IWriteableBrowsingExistingBlockNodeIndex)BrowsingNewBlockIndex.ToExistingBlockIndex();
            browsingIndex = BrowsingExistingBlockIndex;

            blockState = (IWriteableBlockState)CreateBlockState(BrowsingNewBlockIndex, ChildBlock);
            InsertInBlockStateList(newBlockIndex.BlockIndex, blockState);

            childState = (IWriteablePlaceholderNodeState)CreateNodeState(BrowsingExistingBlockIndex);
            blockState.Insert(BrowsingExistingBlockIndex, 0, childState);
        }

        public virtual void InsertExisting(IWriteableInsertionExistingBlockNodeIndex existingBlockIndex, out IWriteableBrowsingCollectionNodeIndex browsingIndex, out IWriteablePlaceholderNodeState childState)
        {
            Debug.Assert(existingBlockIndex != null);
            Debug.Assert(existingBlockIndex.BlockIndex < BlockStateList.Count);

            IWriteableBlockState BlockState = BlockStateList[existingBlockIndex.BlockIndex];

            Debug.Assert(existingBlockIndex.Index <= BlockState.StateList.Count);

            INode ParentNode = Owner.Node;
            IBlock ChildBlock = BlockState.ChildBlock;

            NodeTreeHelperBlockList.InsertIntoBlock(ChildBlock, existingBlockIndex.Index, existingBlockIndex.Node);

            IWriteableBrowsingBlockNodeIndex BrowsingBlockIndex = (IWriteableBrowsingBlockNodeIndex)existingBlockIndex.ToBrowsingIndex();
            browsingIndex = BrowsingBlockIndex;

            childState = (IWriteablePlaceholderNodeState)CreateNodeState(BrowsingBlockIndex);
            int Index = (BrowsingBlockIndex is IWriteableBrowsingExistingBlockNodeIndex AsExistingBlockNodeIndex) ? AsExistingBlockNodeIndex.Index : 0;
            BlockState.Insert(BrowsingBlockIndex, Index, childState);
        }

        /// <summary>
        /// Removes a node from a block list. This method is not allowed to remove the last node of a block.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to remove.</param>
        /// <param name="oldBrowsingIndex">Index of the removed node upon return.</param>
        public virtual void Remove(IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex != null);

            if (nodeIndex is IWriteableBrowsingBlockNodeIndex AsBlockIndex)
            {
                Remove(AsBlockIndex, out bool IsBlockRemoved, out IWriteableBrowsingPatternIndex PatternIndex, out IWriteableBrowsingSourceIndex SourceIndex);

                // Only the safe case where the block isn't removed is allowed for this version of Remove().
                Debug.Assert(!IsBlockRemoved);
                Debug.Assert(PatternIndex == null);
                Debug.Assert(SourceIndex == null);
            }
            else
                throw new ArgumentOutOfRangeException(nameof(nodeIndex));
        }

        /// <summary>
        /// Removes a node from a block list. This method is allowed to remove the last node of a block.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to remove.</param>
        /// <param name="isBlockRemoved">Indicates upon return if the last nodee of the block was removed, and if so, that the block was removed as well.</param>
        /// <param name="patternIndex">If <paramref name="isBlockRemoved"/> is true, index of the removed pattern upon return.</param>
        /// <param name="sourceIndex">If <paramref name="isBlockRemoved"/> is true, index of the removed source upon return.</param>
        public virtual void RemoveWithBlock(IWriteableBrowsingBlockNodeIndex nodeIndex, out bool isBlockRemoved, out IWriteableBrowsingPatternIndex patternIndex, out IWriteableBrowsingSourceIndex sourceIndex)
        {
            Remove(nodeIndex, out isBlockRemoved, out patternIndex, out sourceIndex);
        }

        protected virtual void Remove(IWriteableBrowsingBlockNodeIndex blockNodeIndex, out bool IsBlockRemoved, out IWriteableBrowsingPatternIndex patternIndex, out IWriteableBrowsingSourceIndex sourceIndex)
        {
            Debug.Assert(blockNodeIndex != null);
            Debug.Assert(blockNodeIndex.BlockIndex < BlockStateList.Count);

            int Index;
            if (blockNodeIndex is IWriteableBrowsingNewBlockNodeIndex AsNewBlockNodeIndex)
                Index = 0;
            else if (blockNodeIndex is IWriteableBrowsingExistingBlockNodeIndex AsExistingBlockNodeIndex)
                Index = AsExistingBlockNodeIndex.Index;
            else
                throw new ArgumentOutOfRangeException(nameof(blockNodeIndex));

            IWriteableBlockState BlockState = BlockStateList[blockNodeIndex.BlockIndex];
            Debug.Assert(Index < BlockState.StateList.Count);

            IBlock ChildBlock = BlockState.ChildBlock;
            INode ParentNode = Owner.Node;

            IWriteableNodeState OldChildState = BlockState.StateList[Index];
            BlockState.Remove((IWriteableBrowsingBlockNodeIndex)OldChildState.ParentIndex, Index);

            NodeTreeHelperBlockList.RemoveFromBlock(ParentNode, PropertyName, blockNodeIndex.BlockIndex, Index, out IsBlockRemoved);

            if (IsBlockRemoved)
            {
                patternIndex = BlockState.PatternIndex;
                sourceIndex = BlockState.SourceIndex;
                RemoveFromBlockStateList(blockNodeIndex.BlockIndex);
            }
            else
            {
                patternIndex = null;
                sourceIndex = null;
            }
        }

        /// <summary>
        /// Replaces a node.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to insert.</param>
        /// <param name="oldBrowsingIndex">Index of the replaced node upon return.</param>
        /// <param name="newBrowsingIndex">Index of the inserted node upon return.</param>
        /// <param name="childState">State of the inserted node upon return.</param>
        public virtual void Replace(IWriteableInsertionChildIndex nodeIndex, out IWriteableBrowsingChildIndex oldBrowsingIndex, out IWriteableBrowsingChildIndex newBrowsingIndex, out IWriteableNodeState childState)
        {
            Debug.Assert(nodeIndex != null);

            if (nodeIndex is IWriteableInsertionExistingBlockNodeIndex AsExistingBlockIndex)
                Replace(AsExistingBlockIndex, out oldBrowsingIndex, out newBrowsingIndex, out childState);
            else
                throw new ArgumentOutOfRangeException(nameof(nodeIndex));
        }

        protected virtual void Replace(IWriteableInsertionExistingBlockNodeIndex existingBlockIndex, out IWriteableBrowsingChildIndex oldBrowsingIndex, out IWriteableBrowsingChildIndex newBrowsingIndex, out IWriteableNodeState childState)
        {
            Debug.Assert(existingBlockIndex != null);
            Debug.Assert(existingBlockIndex.BlockIndex < BlockStateList.Count);

            IWriteableBlockState BlockState = BlockStateList[existingBlockIndex.BlockIndex];
            Debug.Assert(existingBlockIndex.Index < BlockState.StateList.Count);

            IBlock ChildBlock = BlockState.ChildBlock;
            INode ParentNode = Owner.Node;

            IWriteableNodeState OldChildState = BlockState.StateList[existingBlockIndex.Index];
            oldBrowsingIndex = (IWriteableBrowsingBlockNodeIndex)OldChildState.ParentIndex;
            BlockState.Remove((IWriteableBrowsingBlockNodeIndex)OldChildState.ParentIndex, existingBlockIndex.Index);

            NodeTreeHelperBlockList.ReplaceInBlock(ChildBlock, existingBlockIndex.Index, existingBlockIndex.Node);

            IWriteableBrowsingBlockNodeIndex BrowsingBlockIndex = (IWriteableBrowsingBlockNodeIndex)existingBlockIndex.ToBrowsingIndex();
            newBrowsingIndex = BrowsingBlockIndex;

            IWriteablePlaceholderNodeState NewChildState = (IWriteablePlaceholderNodeState)CreateNodeState(BrowsingBlockIndex);
            int Index = (BrowsingBlockIndex is IWriteableBrowsingExistingBlockNodeIndex AsExistingBlockNodeIndex) ? AsExistingBlockNodeIndex.Index : 0;
            BlockState.Insert(BrowsingBlockIndex, Index, NewChildState);

            childState = NewChildState;
        }
        #endregion

        #region Descendant Interface
        protected override void NotifyBlockStateCreated(IReadOnlyBlockState blockState)
        {
            BlockStateCreated?.Invoke((IWriteableBlockState)blockState);
        }

        protected override void NotifyBlockStateRemoved(IReadOnlyBlockState blockState)
        {
            BlockStateRemoved?.Invoke((IWriteableBlockState)blockState);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBlockStateList object.
        /// </summary>
        protected override IReadOnlyBlockStateList CreateBlockStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockListInner<IIndex, TIndex>));
            return new WriteableBlockStateList();
        }

        /// <summary>
        /// Creates a IxxxBlockStateReadOnlyList object.
        /// </summary>
        protected override IReadOnlyBlockStateReadOnlyList CreateBlockStateListReadOnly(IReadOnlyBlockStateList blockStateList)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockListInner<IIndex, TIndex>));
            return new WriteableBlockStateReadOnlyList((IWriteableBlockStateList)blockStateList);
        }

        /// <summary>
        /// Creates a IxxxBlockState object.
        /// </summary>
        protected override IReadOnlyBlockState CreateBlockState(IReadOnlyBrowsingNewBlockNodeIndex nodeIndex, IBlock childBlock)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockListInner<IIndex, TIndex>));
            return new WriteableBlockState(this, (IWriteableBrowsingNewBlockNodeIndex)nodeIndex, childBlock);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockListInner<IIndex, TIndex>));
            return new WriteablePlaceholderNodeState((IWriteableNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates an index object.
        /// </summary>
        protected override IIndex CreateNodeIndex(IReadOnlyPlaceholderNodeState state, string propertyName, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockListInner<IIndex, TIndex>));
            return (TIndex)(IWriteableBrowsingBlockNodeIndex)new WriteableBrowsingExistingBlockNodeIndex(Owner.Node, state.Node, propertyName, blockIndex, index);
        }
        #endregion
    }
}
