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
        /// <param name="operation">Details of the operation performed.</param>
        void InsertNew(IWriteableInsertBlockOperation operation);

        /// <summary>
        /// Removes a node from a block list. This method is allowed to remove the last node of a block.
        /// </summary>
        /// <param name="blockOperation">Details of the operation performed.</param>
        void RemoveWithBlock(IWriteableRemoveBlockOperation blockOperation);

        /// <summary>
        /// Changes the replication state of a block.
        /// </summary>
        /// <param name="blockOperation">Details of the operation performed.</param>
        /// <param name="replication">New replication value.</param>
        void ChangeReplication(IWriteableChangeBlockOperation blockOperation, ReplicationStatus replication);

        /// <summary>
        /// Checks whether a block can be split at the given index.
        /// </summary>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        bool IsSplittable(IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Splits a block in two at the given index.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void SplitBlock(IWriteableSplitBlockOperation operation);

        /// <summary>
        /// Checks whether a block can be merged at the given index.
        /// </summary>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        bool IsMergeable(IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Merges two blocks at the given index.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void MergeBlocks(IWriteableMergeBlocksOperation operation);

        /// <summary>
        /// Moves a block around in a block list.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void MoveBlock(IWriteableMoveBlockOperation operation);
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
        /// <param name="operation">Details of the operation performed.</param>
        void InsertNew(IWriteableInsertBlockOperation operation);

        /// <summary>
        /// Removes a node from a block list. This method is allowed to remove the last node of a block.
        /// </summary>
        /// <param name="blockOperation">Details of the operation performed.</param>
        void RemoveWithBlock(IWriteableRemoveBlockOperation blockOperation);

        /// <summary>
        /// Changes the replication state of a block.
        /// </summary>
        /// <param name="blockOperation">Details of the operation performed.</param>
        /// <param name="replication">New replication value.</param>
        void ChangeReplication(IWriteableChangeBlockOperation blockOperation, ReplicationStatus replication);

        /// <summary>
        /// Checks whether a block can be split at the given index.
        /// </summary>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        bool IsSplittable(IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Splits a block in two at the given index.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void SplitBlock(IWriteableSplitBlockOperation operation);

        /// <summary>
        /// Checks whether a block can be merged at the given index.
        /// </summary>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        bool IsMergeable(IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Merges two blocks at the given index.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void MergeBlocks(IWriteableMergeBlocksOperation operation);

        /// <summary>
        /// Moves a block around in a block list.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void MoveBlock(IWriteableMoveBlockOperation operation);
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
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void Insert(IWriteableInsertNodeOperation operation)
        {
            Debug.Assert(operation != null);

            if (operation.InsertionIndex is IWriteableInsertionExistingBlockNodeIndex AsExistingBlockIndex)
                InsertExisting(operation, AsExistingBlockIndex);
            else // The case of a new block is already handled.
                throw new ArgumentOutOfRangeException(nameof(operation));
        }

        /// <summary>
        /// Inserts a new block with one node in a block list.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void InsertNew(IWriteableInsertBlockOperation operation)
        {
            Debug.Assert(operation != null);

            int BlockIndex = operation.BlockIndex;
            Debug.Assert(BlockIndex >= 0 && BlockIndex <= BlockStateList.Count);

            IBlock NewBlock = operation.Block;
            Debug.Assert(NewBlock != null);

            INode NewNode = operation.Node;
            Debug.Assert(NewBlock != null);

            INode ParentNode = Owner.Node;
            NodeTreeHelperBlockList.InsertIntoBlockList(ParentNode, PropertyName, BlockIndex, NewBlock);
            NodeTreeHelperBlockList.InsertIntoBlock(NewBlock, 0, NewNode);

            IWriteableBrowsingNewBlockNodeIndex BrowsingNewBlockIndex = CreateNewBlockNodeIndex(NewNode, BlockIndex, NewBlock.ReplicationPattern, NewBlock.SourceIdentifier);
            IWriteableBrowsingExistingBlockNodeIndex BrowsingExistingBlockIndex = (IWriteableBrowsingExistingBlockNodeIndex)BrowsingNewBlockIndex.ToExistingBlockIndex();

            IWriteableBlockState BlockState = (IWriteableBlockState)CreateBlockState(BrowsingNewBlockIndex, NewBlock);
            InsertInBlockStateList(BlockIndex, BlockState);

            IWriteablePlaceholderNodeState ChildState = (IWriteablePlaceholderNodeState)CreateNodeState(BrowsingExistingBlockIndex);
            BlockState.Insert(BrowsingExistingBlockIndex, 0, ChildState);

            operation.Update(BrowsingExistingBlockIndex, BlockState, ChildState);

            while (++BlockIndex < BlockStateList.Count)
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

        /// <summary></summary>
        protected virtual void InsertExisting(IWriteableInsertNodeOperation operation, IWriteableInsertionExistingBlockNodeIndex existingBlockIndex)
        {
            int BlockIndex = existingBlockIndex.BlockIndex;
            int Index = existingBlockIndex.Index;

            Debug.Assert(existingBlockIndex != null);
            Debug.Assert(BlockIndex >= 0 && BlockIndex < BlockStateList.Count);

            IWriteableBlockState BlockState = BlockStateList[BlockIndex];

            Debug.Assert(Index >= 0 && Index <= BlockState.StateList.Count);

            INode ParentNode = Owner.Node;
            IBlock ChildBlock = BlockState.ChildBlock;

            NodeTreeHelperBlockList.InsertIntoBlock(ChildBlock, Index, existingBlockIndex.Node);

            IWriteableBrowsingBlockNodeIndex BrowsingBlockIndex = (IWriteableBrowsingBlockNodeIndex)existingBlockIndex.ToBrowsingIndex();

            IWriteablePlaceholderNodeState ChildState = (IWriteablePlaceholderNodeState)CreateNodeState(BrowsingBlockIndex);
            BlockState.Insert(BrowsingBlockIndex, Index, ChildState);

            operation.Update(BrowsingBlockIndex, ChildState);

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
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void Remove(IWriteableRemoveNodeOperation operation)
        {
            Debug.Assert(operation != null);

            if (operation.NodeIndex is IWriteableBrowsingExistingBlockNodeIndex AsExistingBlockIndex)
            {
                // Only the safe case where the block isn't removed is allowed for this version of Remove().
                Remove(null, operation, AsExistingBlockIndex);
            }
            else
                throw new ArgumentOutOfRangeException(nameof(operation));
        }

        /// <summary>
        /// Removes a node from a block list. This method is allowed to remove the last node of a block.
        /// </summary>
        /// <param name="blockOperation">Details of the operation performed.</param>
        public virtual void RemoveWithBlock(IWriteableRemoveBlockOperation blockOperation)
        {
            Remove(blockOperation, null, blockOperation.BlockIndex);
        }

        /// <summary></summary>
        protected virtual void Remove(IWriteableRemoveBlockOperation blockOperation, IWriteableRemoveNodeOperation nodeOperation, IWriteableBrowsingExistingBlockNodeIndex existingBlockIndex)
        {
            Debug.Assert(existingBlockIndex != null);

            int BlockIndex = existingBlockIndex.BlockIndex;
            Debug.Assert(BlockIndex >= 0 && BlockIndex < BlockStateList.Count);

            IWriteableBlockState BlockState = BlockStateList[BlockIndex];

            int Index = existingBlockIndex.Index;
            Debug.Assert(Index >= 0 && Index < BlockState.StateList.Count);

            IBlock ChildBlock = BlockState.ChildBlock;
            INode ParentNode = Owner.Node;

            IWriteablePlaceholderNodeState OldChildState = BlockState.StateList[Index];
            INode RemovedNode = OldChildState.Node;

            BlockState.Remove((IWriteableBrowsingBlockNodeIndex)OldChildState.ParentIndex, Index);

            NodeTreeHelperBlockList.RemoveFromBlock(ParentNode, PropertyName, BlockIndex, Index, out bool IsBlockRemoved);

            if (IsBlockRemoved)
            {
                Debug.Assert(blockOperation != null);

                RemoveFromBlockStateList(BlockIndex);

                blockOperation.Update(BlockState, RemovedNode);

                for (; BlockIndex < BlockStateList.Count; BlockIndex++)
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
            {
                Debug.Assert(nodeOperation != null);

                nodeOperation.Update(OldChildState);
            }

            while (Index < BlockState.StateList.Count)
            {
                IWriteablePlaceholderNodeState State = BlockState.StateList[Index];

                IWriteableBrowsingExistingBlockNodeIndex NodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Debug.Assert(NodeIndex != null);
                Debug.Assert(NodeIndex.BlockIndex == existingBlockIndex.BlockIndex);
                Debug.Assert(NodeIndex.Index == Index + 1);

                NodeIndex.MoveDown();

                Index++;
            }
        }

        /// <summary>
        /// Replaces a node.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void Replace(IWriteableReplaceOperation operation)
        {
            Debug.Assert(operation != null);

            if (operation.ReplacementIndex is IWriteableInsertionExistingBlockNodeIndex AsExistingBlockIndex)
                Replace(operation, AsExistingBlockIndex);
            else
                throw new ArgumentOutOfRangeException(nameof(operation));
        }

        /// <summary></summary>
        protected virtual void Replace(IWriteableReplaceOperation operation, IWriteableInsertionExistingBlockNodeIndex existingBlockIndex)
        {
            Debug.Assert(existingBlockIndex != null);

            int BlockIndex = existingBlockIndex.BlockIndex;
            Debug.Assert(BlockIndex >= 0 && BlockIndex < BlockStateList.Count);

            IWriteableBlockState BlockState = BlockStateList[BlockIndex];

            int Index = existingBlockIndex.Index;
            Debug.Assert(Index >= 0 && Index < BlockState.StateList.Count);

            IBlock ChildBlock = BlockState.ChildBlock;
            INode ParentNode = Owner.Node;

            IWriteableNodeState OldChildState = BlockState.StateList[Index];
            IWriteableBrowsingBlockNodeIndex OldBrowsingIndex = (IWriteableBrowsingBlockNodeIndex)OldChildState.ParentIndex;
            BlockState.Remove(OldBrowsingIndex, Index);

            NodeTreeHelperBlockList.ReplaceInBlock(ChildBlock, Index, existingBlockIndex.Node);

            IWriteableBrowsingExistingBlockNodeIndex NewBrowsingIndex = (IWriteableBrowsingExistingBlockNodeIndex)existingBlockIndex.ToBrowsingIndex();
            IWriteablePlaceholderNodeState NewChildState = (IWriteablePlaceholderNodeState)CreateNodeState(NewBrowsingIndex);
            BlockState.Insert(NewBrowsingIndex, Index, NewChildState);

            operation.Update(OldBrowsingIndex, NewBrowsingIndex, OldChildState, NewChildState);
        }

        /// <summary>
        /// Changes the replication state of a block.
        /// </summary>
        /// <param name="blockOperation">Details of the operation performed.</param>
        /// <param name="replication">New replication value.</param>
        public virtual void ChangeReplication(IWriteableChangeBlockOperation blockOperation, ReplicationStatus replication)
        {
            int BlockIndex = blockOperation.BlockIndex;

            Debug.Assert(BlockIndex >= 0 && BlockIndex < BlockStateList.Count);

            IWriteableBlockState BlockState = BlockStateList[BlockIndex];
            NodeTreeHelperBlockList.SetReplication(BlockState.ChildBlock, replication);

            blockOperation.Update(BlockState);
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
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void SplitBlock(IWriteableSplitBlockOperation operation)
        {
            Debug.Assert(operation != null);

            IWriteableBrowsingExistingBlockNodeIndex NodeIndex = operation.NodeIndex;
            Debug.Assert(NodeIndex != null);

            int SplitBlockIndex = NodeIndex.BlockIndex;
            Debug.Assert(SplitBlockIndex >= 0 && SplitBlockIndex < BlockStateList.Count);

            int SplitIndex = NodeIndex.Index;
            Debug.Assert(SplitIndex > 0);

            IWriteableBlockState BlockState = BlockStateList[SplitBlockIndex];
            Debug.Assert(SplitIndex < BlockState.StateList.Count);

            ReplicationStatus Replication = BlockState.ChildBlock.Replication;
            IPattern NewPatternNode = NodeHelper.CreateSimplePattern(BlockState.ChildBlock.ReplicationPattern.Text);
            IIdentifier NewSourceNode = NodeHelper.CreateSimpleIdentifier(BlockState.ChildBlock.SourceIdentifier.Text);

            NodeTreeHelperBlockList.SplitBlock(Owner.Node, PropertyName, SplitBlockIndex, SplitIndex, Replication, NewPatternNode, NewSourceNode, out IBlock ChildBlock);

            NodeTreeHelperBlockList.GetChildNode(ChildBlock, 0, out INode NewBlockFirstNode);
            IWriteableBrowsingNewBlockNodeIndex NewBlockIndex = CreateNewBlockNodeIndex(NewBlockFirstNode, SplitBlockIndex, NewPatternNode, NewSourceNode);

            IWriteableBlockState NewBlockState = (IWriteableBlockState)CreateBlockState(NewBlockIndex, ChildBlock);
            NewBlockState.InitBlockState();
            InsertInBlockStateList(NewBlockIndex.BlockIndex, NewBlockState);

            for (int i = 0; i < SplitIndex; i++)
            {
                IWriteablePlaceholderNodeState State = BlockState.StateList[0];
                IWriteableBrowsingExistingBlockNodeIndex ChildNodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Debug.Assert(ChildNodeIndex != null);

                BlockState.Remove(ChildNodeIndex, 0);
                NewBlockState.Insert(ChildNodeIndex, i, State);
            }

            operation.Update(NewBlockState);

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
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void MergeBlocks(IWriteableMergeBlocksOperation operation)
        {
            Debug.Assert(operation != null);

            IWriteableBrowsingExistingBlockNodeIndex NodeIndex = operation.NodeIndex;
            Debug.Assert(NodeIndex != null);
            Debug.Assert(NodeIndex.Index == 0);

            int MergeBlockIndex = NodeIndex.BlockIndex;
            Debug.Assert(MergeBlockIndex > 0 && MergeBlockIndex < BlockStateList.Count);

            IWriteableBlockState FirstBlockState = BlockStateList[MergeBlockIndex - 1];
            IWriteableBlockState SecondBlockState = BlockStateList[MergeBlockIndex];
            int MergeIndex = FirstBlockState.StateList.Count;
            Debug.Assert(MergeIndex > 0);

            NodeTreeHelperBlockList.MergeBlocks(Owner.Node, PropertyName, MergeBlockIndex);

            RemoveFromBlockStateList(MergeBlockIndex - 1);

            operation.Update();

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
        /// Checks whether a node can be moved in a block list.
        /// </summary>
        /// <param name="nodeIndex">Index of the node that would be moved.</param>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        public virtual bool IsMoveable(IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction)
        {
            if (nodeIndex is IWriteableBrowsingExistingBlockNodeIndex AsExistingBlockNodeIndex)
                return IsMoveable(AsExistingBlockNodeIndex, direction);
            else
                throw new ArgumentOutOfRangeException(nameof(nodeIndex));
        }

        /// <summary></summary>
        protected virtual bool IsMoveable(IWriteableBrowsingExistingBlockNodeIndex existingBlockIndex, int direction)
        {
            Debug.Assert(existingBlockIndex != null);

            int BlockIndex = existingBlockIndex.BlockIndex;
            Debug.Assert(BlockIndex >= 0 && BlockIndex < BlockStateList.Count);

            IWriteableBlockState BlockState = BlockStateList[BlockIndex];
            IWriteablePlaceholderNodeStateReadOnlyList StateList = BlockState.StateList;

            int NewPosition = existingBlockIndex.Index + direction;
            bool Result = NewPosition >= 0 && NewPosition < StateList.Count;

            return Result;
        }

        /// <summary>
        /// Moves a node around in a list or block list. In a block list, the node stays in same block.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void Move(IWriteableMoveNodeOperation operation)
        {
            Debug.Assert(operation != null);

            if (operation.NodeIndex is IWriteableBrowsingExistingBlockNodeIndex AsExistingBlockNodeIndex)
                Move(operation, AsExistingBlockNodeIndex);
            else
                throw new ArgumentOutOfRangeException(nameof(operation));
        }

        /// <summary></summary>
        protected virtual void Move(IWriteableMoveNodeOperation operation, IWriteableBrowsingExistingBlockNodeIndex existingBlockIndex)
        {
            Debug.Assert(operation != null);
            Debug.Assert(existingBlockIndex != null);

            int BlockIndex = existingBlockIndex.BlockIndex;
            Debug.Assert(BlockIndex >= 0 && BlockIndex < BlockStateList.Count);

            IWriteableBlockState BlockState = BlockStateList[BlockIndex];
            IWriteablePlaceholderNodeStateReadOnlyList StateList = BlockState.StateList;

            int MoveIndex = operation.Index;
            int Direction = operation.Direction;
            Debug.Assert(MoveIndex >= 0 && MoveIndex < StateList.Count);
            Debug.Assert(MoveIndex + Direction >= 0 && MoveIndex + Direction < StateList.Count);

            BlockState.Move(existingBlockIndex, MoveIndex, Direction);

            IBlock ChildBlock = BlockState.ChildBlock;
            NodeTreeHelperBlockList.MoveNode(ChildBlock, MoveIndex, Direction);

            operation.Update(StateList[MoveIndex + Direction]);

            if (Direction > 0)
            {
                for (int i = MoveIndex; i < MoveIndex + Direction; i++)
                {
                    IWriteableBrowsingExistingBlockNodeIndex ChildNodeIndex = StateList[i].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                    Debug.Assert(ChildNodeIndex != null);

                    ChildNodeIndex.MoveDown();
                    existingBlockIndex.MoveUp();
                }
            }
            else if (Direction < 0)
            {
                for (int i = MoveIndex; i > MoveIndex + Direction; i--)
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
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void MoveBlock(IWriteableMoveBlockOperation operation)
        {
            int BlockIndex = operation.BlockIndex;
            int Direction = operation.Direction;

            Debug.Assert(BlockIndex >= 0 && BlockIndex < BlockStateList.Count);
            Debug.Assert(BlockIndex + Direction >= 0 && BlockIndex + Direction < BlockStateList.Count);

            int MoveIndex = BlockIndex;
            IWriteableBlockState BlockState = BlockStateList[MoveIndex];

            MoveInBlockStateList(MoveIndex, Direction);
            NodeTreeHelperBlockList.MoveBlock(Owner.Node, PropertyName, MoveIndex, Direction);

            if (Direction > 0)
            {
                for (int i = MoveIndex; i < MoveIndex + Direction; i++)
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
            else if (Direction < 0)
            {
                for (int i = MoveIndex; i > MoveIndex + Direction; i--)
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

            operation.Update(BlockState);
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
