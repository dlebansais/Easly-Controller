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
        void InsertNew(IWriteableInsertionNewBlockNodeIndex newBlockIndex, out IWriteableBrowsingExistingBlockNodeIndex browsingIndex, out IWriteableBlockState blockState, out IWriteablePlaceholderNodeState childState);

        /// <summary>
        /// Removes a node from a block list. This method is allowed to remove the last node of a block.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to remove.</param>
        /// <param name="oldBlockState">If the node is the last in the block, contains upon return the block that was removed as well, null otherwise.</param>
        void RemoveWithBlock(IWriteableBrowsingBlockNodeIndex nodeIndex, out IWriteableBlockState oldBlockState);

        /// <summary>
        /// Changes the replication state of a block.
        /// </summary>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="replication">New replication value.</param>
        void ChangeReplication(int blockIndex, ReplicationStatus replication);

        /// <summary>
        /// Checks whether a block can be split at the given index.
        /// </summary>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        bool IsSplittable(IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Splits a block in two at the given index.
        /// </summary>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        /// <param name="newBlockState">The created block state upon return.</param>
        void SplitBlock(IWriteableBrowsingExistingBlockNodeIndex nodeIndex, out IWriteableBlockState newBlockState);

        /// <summary>
        /// Checks whether a block can be merged at the given index.
        /// </summary>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        bool IsMergeable(IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Merges two blocks at the given index.
        /// </summary>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        void MergeBlocks(IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Moves a block around in a block list.
        /// </summary>
        /// <param name="blockIndex">Index of the block to move.</param>
        /// <param name="direction">The change in position, relative to the current block position.</param>
        void MoveBlock(int blockIndex, int direction);
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
        void InsertNew(IWriteableInsertionNewBlockNodeIndex newBlockIndex, out IWriteableBrowsingExistingBlockNodeIndex browsingIndex, out IWriteableBlockState blockState, out IWriteablePlaceholderNodeState childState);

        /// <summary>
        /// Removes a node from a block list. This method is allowed to remove the last node of a block.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to remove.</param>
        /// <param name="oldBlockState">If the node is the last in the block, contains upon return the block that was removed as well, null otherwise.</param>
        void RemoveWithBlock(IWriteableBrowsingBlockNodeIndex nodeIndex, out IWriteableBlockState oldBlockState);

        /// <summary>
        /// Changes the replication state of a block.
        /// </summary>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="replication">New replication value.</param>
        void ChangeReplication(int blockIndex, ReplicationStatus replication);

        /// <summary>
        /// Checks whether a block can be split at the given index.
        /// </summary>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        bool IsSplittable(IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Splits a block in two at the given index.
        /// </summary>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        /// <param name="newBlockState">The created block state upon return.</param>
        void SplitBlock(IWriteableBrowsingExistingBlockNodeIndex nodeIndex, out IWriteableBlockState newBlockState);

        /// <summary>
        /// Checks whether a block can be merged at the given index.
        /// </summary>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        bool IsMergeable(IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Merges two blocks at the given index.
        /// </summary>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        void MergeBlocks(IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Moves a block around in a block list.
        /// </summary>
        /// <param name="blockIndex">Index of the block to move.</param>
        /// <param name="direction">The change in position, relative to the current block position.</param>
        void MoveBlock(int blockIndex, int direction);
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
        public new event Action<IWriteableBlockState> BlockStateCreated
        {
            add { AddBlockStateCreatedDelegate((Action<IReadOnlyBlockState>)value); }
            remove { RemoveBlockStateCreatedDelegate((Action<IReadOnlyBlockState>)value); }
        }

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        public new event Action<IWriteableBlockState> BlockStateRemoved
        {
            add { AddBlockStateRemovedDelegate((Action<IReadOnlyBlockState>)value); }
            remove { RemoveBlockStateRemovedDelegate((Action<IReadOnlyBlockState>)value); }
        }
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
        public virtual void InsertNew(IWriteableInsertionNewBlockNodeIndex newBlockIndex, out IWriteableBrowsingExistingBlockNodeIndex browsingIndex, out IWriteableBlockState blockState, out IWriteablePlaceholderNodeState childState)
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

            for (int BlockIndex = BrowsingExistingBlockIndex.BlockIndex + 1; BlockIndex < BlockStateList.Count; BlockIndex++)
            {
                IWriteableBlockState NextBlockState = BlockStateList[BlockIndex];
                
                foreach (IWriteablePlaceholderNodeState State in NextBlockState.StateList)
                {
                    IWriteableBrowsingExistingBlockNodeIndex NodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                    Debug.Assert(NodeIndex != null);
                    Debug.Assert(NodeIndex.BlockIndex == BlockIndex - 1);

                    NodeIndex.MoveBlockUp();
                }
            }
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

            while (++Index < BlockState.StateList.Count)
            {
                IWriteablePlaceholderNodeState State = BlockState.StateList[Index];

                IWriteableBrowsingExistingBlockNodeIndex NodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Debug.Assert(NodeIndex != null);
                Debug.Assert(NodeIndex.BlockIndex == BrowsingBlockIndex.BlockIndex);
                Debug.Assert(NodeIndex.Index == Index - 1);

                NodeIndex.MoveUp();
            }
        }

        /// <summary>
        /// Removes a node from a block list. This method is not allowed to remove the last node of a block.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to remove.</param>
        public virtual void Remove(IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex != null);

            if (nodeIndex is IWriteableBrowsingBlockNodeIndex AsBlockIndex)
            {
                Remove(AsBlockIndex, out IWriteableBlockState OldBlockState);

                // Only the safe case where the block isn't removed is allowed for this version of Remove().
                Debug.Assert(OldBlockState == null);
            }
            else
                throw new ArgumentOutOfRangeException(nameof(nodeIndex));
        }

        /// <summary>
        /// Removes a node from a block list. This method is allowed to remove the last node of a block.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to remove.</param>
        /// <param name="oldBlockState">If the node is the last in the block, contains upon return the block that was removed as well, null otherwise.</param>
        public virtual void RemoveWithBlock(IWriteableBrowsingBlockNodeIndex nodeIndex, out IWriteableBlockState oldBlockState)
        {
            Remove(nodeIndex, out oldBlockState);
        }

        protected virtual void Remove(IWriteableBrowsingBlockNodeIndex blockNodeIndex, out IWriteableBlockState oldBlockState)
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

            NodeTreeHelperBlockList.RemoveFromBlock(ParentNode, PropertyName, blockNodeIndex.BlockIndex, Index, out bool IsBlockRemoved);

            if (IsBlockRemoved)
            {
                oldBlockState = BlockState;
                RemoveFromBlockStateList(blockNodeIndex.BlockIndex);

                for (int BlockIndex = blockNodeIndex.BlockIndex; BlockIndex < BlockStateList.Count; BlockIndex++)
                {
                    IWriteableBlockState NextBlockState = BlockStateList[BlockIndex];

                    foreach (IWriteablePlaceholderNodeState State in NextBlockState.StateList)
                    {
                        IWriteableBrowsingExistingBlockNodeIndex NodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                        Debug.Assert(NodeIndex != null);
                        Debug.Assert(NodeIndex.BlockIndex == BlockIndex + 1);

                        NodeIndex.MoveBlockDown();
                    }
                }
            }
            else
                oldBlockState = null;

            while (Index < BlockState.StateList.Count)
            {
                IWriteablePlaceholderNodeState State = BlockState.StateList[Index];

                IWriteableBrowsingExistingBlockNodeIndex NodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Debug.Assert(NodeIndex != null);
                Debug.Assert(NodeIndex.BlockIndex == blockNodeIndex.BlockIndex);
                Debug.Assert(NodeIndex.Index == Index + 1);

                NodeIndex.MoveDown();

                Index++;
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

        /// <summary>
        /// Changes the replication state of a block.
        /// </summary>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="replication">New replication value.</param>
        public virtual void ChangeReplication(int blockIndex, ReplicationStatus replication)
        {
            Debug.Assert(blockIndex >= 0 && blockIndex < BlockStateList.Count);

            IWriteableBlockState BlockState = BlockStateList[blockIndex];
            NodeTreeHelperBlockList.SetReplication(BlockState.ChildBlock, replication);
        }

        /// <summary>
        /// Checks whether a block can be split at the given index.
        /// </summary>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        public virtual bool IsSplittable(IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(nodeIndex.BlockIndex >= 0 && nodeIndex.BlockIndex < BlockStateList.Count);

            int SplitBlockIndex = nodeIndex.BlockIndex;
            int SplitIndex = nodeIndex.Index;
            IWriteableBlockState BlockState = BlockStateList[SplitBlockIndex];

            Debug.Assert(SplitIndex < BlockState.StateList.Count);

            return SplitIndex > 0;
        }

        /// <summary>
        /// Splits a block in two at the given index.
        /// </summary>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        /// <param name="newBlockState">The created block state upon return.</param>
        public virtual void SplitBlock(IWriteableBrowsingExistingBlockNodeIndex nodeIndex, out IWriteableBlockState newBlockState)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(nodeIndex.BlockIndex >= 0 && nodeIndex.BlockIndex < BlockStateList.Count);

            int SplitBlockIndex = nodeIndex.BlockIndex;
            int SplitIndex = nodeIndex.Index;
            Debug.Assert(SplitIndex > 0);

            IWriteableBlockState BlockState = BlockStateList[SplitBlockIndex];
            Debug.Assert(SplitIndex < BlockState.StateList.Count);

            ReplicationStatus Replication = BlockState.ChildBlock.Replication;
            IPattern NewPatternNode = NodeHelper.CreateSimplePattern(BlockState.ChildBlock.ReplicationPattern.Text);
            IIdentifier NewSourceNode = NodeHelper.CreateSimpleIdentifier(BlockState.ChildBlock.SourceIdentifier.Text);

            NodeTreeHelperBlockList.SplitBlock(Owner.Node, PropertyName, SplitBlockIndex, SplitIndex, Replication, NewPatternNode, NewSourceNode, out IBlock ChildBlock);

            NodeTreeHelperBlockList.GetChildNode(ChildBlock, 0, out INode NewBlockFirstNode);
            IWriteableBrowsingNewBlockNodeIndex NewBlockIndex = CreateNewBlockNodeIndex(NewBlockFirstNode, SplitBlockIndex, NewPatternNode, NewSourceNode);

            newBlockState = (IWriteableBlockState)CreateBlockState(NewBlockIndex, ChildBlock);
            newBlockState.InitBlockState();
            InsertInBlockStateList(NewBlockIndex.BlockIndex, newBlockState);

            for (int i = 0; i < SplitIndex; i++)
            {
                IWriteablePlaceholderNodeState State = BlockState.StateList[0];
                IWriteableBrowsingExistingBlockNodeIndex ChildNodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Debug.Assert(ChildNodeIndex != null);

                BlockState.Remove(ChildNodeIndex, 0);
                newBlockState.Insert(ChildNodeIndex, i, State);
            }

            for (int i = 0; i < BlockState.StateList.Count; i++)
            {
                IWriteablePlaceholderNodeState State = BlockState.StateList[i];
                IWriteableBrowsingExistingBlockNodeIndex ChildNodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Debug.Assert(ChildNodeIndex != null);

                ChildNodeIndex.MoveBlockUp();

                for (int j = 0; j < SplitIndex; j++)
                    ChildNodeIndex.MoveDown();
            }

            for (int i = SplitBlockIndex + 2; i < BlockStateList.Count; i++)
                foreach (IWriteablePlaceholderNodeState State in BlockStateList[i].StateList)
                {
                    IWriteableBrowsingExistingBlockNodeIndex ChildNodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                    Debug.Assert(ChildNodeIndex != null);

                    ChildNodeIndex.MoveBlockUp();
                }
        }

        /// <summary>
        /// Checks whether a block can be merged at the given index.
        /// </summary>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        public virtual bool IsMergeable(IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(nodeIndex.BlockIndex >= 0 && nodeIndex.BlockIndex < BlockStateList.Count);
            Debug.Assert(nodeIndex.Index >= 0);

            return nodeIndex.BlockIndex > 0 && nodeIndex.Index == 0;
        }

        /// <summary>
        /// Merges two blocks at the given index.
        /// </summary>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        public virtual void MergeBlocks(IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(nodeIndex.BlockIndex > 0 && nodeIndex.BlockIndex < BlockStateList.Count);
            Debug.Assert(nodeIndex.Index == 0);

            int MergeBlockIndex = nodeIndex.BlockIndex;
            IWriteableBlockState FirstBlockState = BlockStateList[MergeBlockIndex - 1];
            IWriteableBlockState SecondBlockState = BlockStateList[MergeBlockIndex];
            int MergeIndex = FirstBlockState.StateList.Count;
            Debug.Assert(MergeIndex > 0);

            NodeTreeHelperBlockList.MergeBlocks(Owner.Node, PropertyName, MergeBlockIndex);

            RemoveFromBlockStateList(MergeBlockIndex - 1);

            int i;
            for (i = 0; i < MergeIndex; i++)
            {
                IWriteablePlaceholderNodeState State = FirstBlockState.StateList[0];
                IWriteableBrowsingExistingBlockNodeIndex ChildNodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Debug.Assert(ChildNodeIndex != null);

                FirstBlockState.Remove(ChildNodeIndex, 0);
                SecondBlockState.Insert(ChildNodeIndex, i, State);
            }

            for (; i < SecondBlockState.StateList.Count; i++)
            {
                IWriteablePlaceholderNodeState State = SecondBlockState.StateList[i];
                IWriteableBrowsingExistingBlockNodeIndex ChildNodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Debug.Assert(ChildNodeIndex != null);

                ChildNodeIndex.MoveBlockDown();

                for (int j = 0; j < MergeIndex; j++)
                    ChildNodeIndex.MoveUp();
            }

            for (i = MergeBlockIndex; i < BlockStateList.Count; i++)
                foreach (IWriteablePlaceholderNodeState State in BlockStateList[i].StateList)
                {
                    IWriteableBrowsingExistingBlockNodeIndex ChildNodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                    Debug.Assert(ChildNodeIndex != null);

                    ChildNodeIndex.MoveBlockDown();
                }
        }

        /// <summary>
        /// Moves a node around in a list or block list. In a block list, the node stays in same block.
        /// </summary>
        /// <param name="nodeIndex">Index for the moved node.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        public virtual void Move(IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction)
        {
            Debug.Assert(nodeIndex != null);

            if (nodeIndex is IWriteableBrowsingExistingBlockNodeIndex AsExistingBlockNodeIndex)
                Move(AsExistingBlockNodeIndex, direction);
            else
                throw new ArgumentOutOfRangeException(nameof(nodeIndex));
        }

        protected virtual void Move(IWriteableBrowsingExistingBlockNodeIndex existingBlockIndex, int direction)
        {
            Debug.Assert(existingBlockIndex != null);
            Debug.Assert(existingBlockIndex.BlockIndex >= 0 && existingBlockIndex.BlockIndex < BlockStateList.Count);

            int MoveIndex = existingBlockIndex.Index;
            IWriteableBlockState BlockState = BlockStateList[existingBlockIndex.BlockIndex];
            IWriteablePlaceholderNodeStateReadOnlyList StateList = BlockState.StateList;

            Debug.Assert(MoveIndex >= 0 && MoveIndex < StateList.Count);
            Debug.Assert(MoveIndex + direction >= 0 && MoveIndex + direction < StateList.Count);

            BlockState.Move(existingBlockIndex, existingBlockIndex.Index, direction);

            IBlock ChildBlock = BlockState.ChildBlock;
            NodeTreeHelperBlockList.MoveNode(ChildBlock, existingBlockIndex.Index, direction);

            if (direction > 0)
            {
                for (int i = MoveIndex; i < MoveIndex + direction; i++)
                {
                    IWriteableBrowsingExistingBlockNodeIndex ChildNodeIndex = StateList[i].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                    Debug.Assert(ChildNodeIndex != null);

                    ChildNodeIndex.MoveDown();
                    existingBlockIndex.MoveUp();
                }
            }

            else if (direction < 0)
            {
                for (int i = MoveIndex; i > MoveIndex + direction; i--)
                {
                    IWriteableBrowsingExistingBlockNodeIndex ChildNodeIndex = StateList[i].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                    Debug.Assert(ChildNodeIndex != null);

                    ChildNodeIndex.MoveUp();
                    existingBlockIndex.MoveDown();
                }
            }
        }

        /// <summary>
        /// Moves a block around in a block list.
        /// </summary>
        /// <param name="blockIndex">Index of the block to move.</param>
        /// <param name="direction">The change in position, relative to the current block position.</param>
        public virtual void MoveBlock(int blockIndex, int direction)
        {
            Debug.Assert(blockIndex >= 0 && blockIndex < BlockStateList.Count);
            Debug.Assert(blockIndex + direction >= 0 && blockIndex + direction < BlockStateList.Count);

            int MoveIndex = blockIndex;
            IWriteableBlockState BlockState = BlockStateList[MoveIndex];

            MoveInBlockStateList(MoveIndex, direction);
            NodeTreeHelperBlockList.MoveBlock(Owner.Node, PropertyName, MoveIndex, direction);

            if (direction > 0)
            {
                for (int i = MoveIndex; i < MoveIndex + direction; i++)
                {
                    for (int j = 0; j < BlockStateList[i].StateList.Count; j++)
                    {
                        IWriteableBrowsingExistingBlockNodeIndex ChildNodeIndex = BlockStateList[i].StateList[j].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                        Debug.Assert(ChildNodeIndex != null);

                        ChildNodeIndex.MoveBlockDown();
                    }

                    for (int j = 0; j < BlockState.StateList.Count; j++)
                    {
                        IWriteableBrowsingExistingBlockNodeIndex ChildNodeIndex = BlockState.StateList[j].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                        Debug.Assert(ChildNodeIndex != null);

                        ChildNodeIndex.MoveBlockUp();
                    }
                }
            }

            else if (direction < 0)
            {
                for (int i = MoveIndex; i > MoveIndex + direction; i--)
                {
                    for (int j = 0; j < BlockStateList[i].StateList.Count; j++)
                    {
                        IWriteableBrowsingExistingBlockNodeIndex ChildNodeIndex = BlockStateList[i].StateList[j].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                        Debug.Assert(ChildNodeIndex != null);

                        ChildNodeIndex.MoveBlockUp();
                    }

                    for (int j = 0; j < BlockState.StateList.Count; j++)
                    {
                        IWriteableBrowsingExistingBlockNodeIndex ChildNodeIndex = BlockState.StateList[j].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                        Debug.Assert(ChildNodeIndex != null);

                        ChildNodeIndex.MoveBlockDown();
                    }
                }
            }
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

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        protected virtual IWriteableBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(INode node, int blockIndex, IPattern patternNode, IIdentifier sourceNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockListInner<IIndex, TIndex>));
            return new WriteableBrowsingNewBlockNodeIndex(Owner.Node, node, PropertyName, blockIndex, patternNode, sourceNode);
        }
        #endregion
    }
}
