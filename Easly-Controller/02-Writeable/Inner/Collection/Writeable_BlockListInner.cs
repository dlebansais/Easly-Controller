﻿namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.ReadOnly;
    using NotNullReflection;

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    public interface IWriteableBlockListInner : IReadOnlyBlockListInner, IWriteableCollectionInner
    {
        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        new WriteableBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new IWriteablePlaceholderNodeState FirstNodeState { get; }

        /// <summary>
        /// Inserts a new block with one node in a block list.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void InsertNewBlock(IWriteableInsertBlockOperation operation);

        /// <summary>
        /// Removes a node from a block list. This method is allowed to remove the last node of a block.
        /// </summary>
        /// <param name="blockOperation">Details of the operation performed.</param>
        void RemoveWithBlock(IWriteableRemoveBlockOperation blockOperation);

        /// <summary>
        /// Changes the replication state of a block.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void ChangeReplication(WriteableChangeBlockOperation operation);

        /// <summary>
        /// Checks whether a block can be split at the given index.
        /// </summary>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        bool IsSplittable(IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Splits a block in two at the given index.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void SplitBlock(WriteableSplitBlockOperation operation);

        /// <summary>
        /// Checks whether a block can be merged at the given index.
        /// </summary>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        bool IsMergeable(IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Merges two blocks at the given index.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void MergeBlocks(WriteableMergeBlocksOperation operation);

        /// <summary>
        /// Moves a block around in a block list.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void MoveBlock(WriteableMoveBlockOperation operation);
    }

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal interface IWriteableBlockListInner<out IIndex> : IReadOnlyBlockListInner<IIndex>, IWriteableCollectionInner<IIndex>
        where IIndex : IWriteableBrowsingBlockNodeIndex
    {
        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        new WriteableBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new IWriteablePlaceholderNodeState FirstNodeState { get; }

        /// <summary>
        /// Inserts a new block with one node in a block list.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void InsertNewBlock(IWriteableInsertBlockOperation operation);

        /// <summary>
        /// Removes a node from a block list. This method is allowed to remove the last node of a block.
        /// </summary>
        /// <param name="blockOperation">Details of the operation performed.</param>
        void RemoveWithBlock(IWriteableRemoveBlockOperation blockOperation);

        /// <summary>
        /// Changes the replication state of a block.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void ChangeReplication(WriteableChangeBlockOperation operation);

        /// <summary>
        /// Checks whether a block can be split at the given index.
        /// </summary>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        bool IsSplittable(IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Splits a block in two at the given index.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void SplitBlock(WriteableSplitBlockOperation operation);

        /// <summary>
        /// Checks whether a block can be merged at the given index.
        /// </summary>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        bool IsMergeable(IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Merges two blocks at the given index.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void MergeBlocks(WriteableMergeBlocksOperation operation);

        /// <summary>
        /// Moves a block around in a block list.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        void MoveBlock(WriteableMoveBlockOperation operation);
    }

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index as class.</typeparam>
    internal class WriteableBlockListInner<IIndex> : ReadOnlyBlockListInner<IIndex>, IWriteableBlockListInner<IIndex>, IWriteableBlockListInner
        where IIndex : IWriteableBrowsingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="WriteableBlockListInner{IIndex}"/> object.
        /// </summary>
        public static new WriteableBlockListInner<IIndex> Empty { get; } = new WriteableBlockListInner<IIndex>();

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBlockListInner{IIndex}"/> class.
        /// </summary>
        protected WriteableBlockListInner()
            : this(WriteableNodeState<IWriteableInner<IWriteableBrowsingChildIndex>>.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBlockListInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        protected WriteableBlockListInner(IWriteableNodeState owner)
            : base(owner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBlockListInner{IIndex}"/> class.
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
        public new WriteableBlockStateReadOnlyList BlockStateList { get { return (WriteableBlockStateReadOnlyList)base.BlockStateList; } }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        public new IWriteablePlaceholderNodeState FirstNodeState { get { return (IWriteablePlaceholderNodeState)base.FirstNodeState; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Inserts a new node in a block list.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void Insert(WriteableInsertNodeOperation operation)
        {
            int BlockIndex = operation.BlockIndex;
            Debug.Assert(BlockIndex >= 0 && BlockIndex < BlockStateList.Count);

            IWriteableBlockState BlockState = (IWriteableBlockState)BlockStateList[BlockIndex];

            int Index = operation.Index;
            Debug.Assert(Index >= 0 && Index <= BlockState.StateList.Count);

            Node ParentNode = Owner.Node;
            IBlock ChildBlock = BlockState.ChildBlock;
            Node Node = operation.Node;

            NodeTreeHelperBlockList.InsertIntoBlock(ChildBlock, Index, Node);

            IWriteableBrowsingBlockNodeIndex BrowsingBlockIndex = CreateBrowsingNodeIndex(Node, BlockIndex, Index);
            IWriteablePlaceholderNodeState ChildState = (IWriteablePlaceholderNodeState)CreateNodeState(BrowsingBlockIndex);

            BlockState.Insert(BrowsingBlockIndex, Index, ChildState);

            operation.Update(BrowsingBlockIndex, ChildState);

            while (++Index < BlockState.StateList.Count)
            {
                IWriteablePlaceholderNodeState State = (IWriteablePlaceholderNodeState)BlockState.StateList[Index];

                IWriteableBrowsingExistingBlockNodeIndex NodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Debug.Assert(NodeIndex != null);
                Debug.Assert(NodeIndex.BlockIndex == BrowsingBlockIndex.BlockIndex);
                Debug.Assert(NodeIndex.Index == Index - 1);

                NodeIndex.MoveUp();
            }
        }

        /// <summary>
        /// Inserts a new block with one node in a block list.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void InsertNewBlock(IWriteableInsertBlockOperation operation)
        {
            int BlockIndex = operation.BlockIndex;
            Debug.Assert(BlockIndex >= 0 && BlockIndex <= BlockStateList.Count);

            IBlock NewBlock = operation.Block;
            Debug.Assert(NewBlock != null);

            Node NewNode = operation.Node;
            Debug.Assert(NewBlock != null);

            Node ParentNode = Owner.Node;
            NodeTreeHelperBlockList.InsertIntoBlockList(ParentNode, PropertyName, BlockIndex, NewBlock);
            NodeTreeHelperBlockList.InsertIntoBlock(NewBlock, 0, NewNode);

            IWriteableBrowsingNewBlockNodeIndex BrowsingNewBlockIndex = CreateNewBlockNodeIndex(NewNode, BlockIndex);
            IWriteableBrowsingExistingBlockNodeIndex BrowsingExistingBlockIndex = (IWriteableBrowsingExistingBlockNodeIndex)BrowsingNewBlockIndex.ToExistingBlockIndex();

            IWriteableBlockState BlockState = (IWriteableBlockState)CreateBlockState(BrowsingNewBlockIndex, NewBlock);
            InsertInBlockStateList(BlockIndex, BlockState);

            IWriteablePlaceholderNodeState ChildState = (IWriteablePlaceholderNodeState)CreateNodeState(BrowsingExistingBlockIndex);
            BlockState.Insert(BrowsingExistingBlockIndex, 0, ChildState);

            operation.Update(BrowsingExistingBlockIndex, BlockState, ChildState);

            while (++BlockIndex < BlockStateList.Count)
            {
                IWriteableBlockState NextBlockState = (IWriteableBlockState)BlockStateList[BlockIndex];

                foreach (IWriteablePlaceholderNodeState State in NextBlockState.StateList)
                {
                    IWriteableBrowsingExistingBlockNodeIndex NodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                    Debug.Assert(NodeIndex != null);
                    Debug.Assert(NodeIndex.BlockIndex == BlockIndex - 1);

                    NodeIndex.MoveBlockUp();
                }
            }
        }

        /// <summary>
        /// Removes a node from a block list. This method is not allowed to remove the last node of a block.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void Remove(WriteableRemoveNodeOperation operation)
        {
            // Only the safe case where the block isn't removed is allowed for this version of Remove().
            Remove(null, operation, operation.BlockIndex, operation.Index);
        }

        /// <summary>
        /// Removes a node from a block list. This method is allowed to remove the last node of a block.
        /// </summary>
        /// <param name="blockOperation">Details of the operation performed.</param>
        public virtual void RemoveWithBlock(IWriteableRemoveBlockOperation blockOperation)
        {
            Remove(blockOperation, null, blockOperation.BlockIndex, 0);
        }

        private protected virtual void Remove(IWriteableRemoveBlockOperation blockOperation, WriteableRemoveNodeOperation nodeOperation, int blockIndex, int index)
        {
            Debug.Assert(blockIndex >= 0 && blockIndex < BlockStateList.Count);

            IWriteableBlockState BlockState = (IWriteableBlockState)BlockStateList[blockIndex];

            Debug.Assert(index >= 0 && index < BlockState.StateList.Count);

            IBlock ChildBlock = BlockState.ChildBlock;
            Node ParentNode = Owner.Node;
            int i;

            IWriteablePlaceholderNodeState OldChildState = (IWriteablePlaceholderNodeState)BlockState.StateList[index];
            Node RemovedNode = OldChildState.Node;

            BlockState.Remove((IWriteableBrowsingBlockNodeIndex)OldChildState.ParentIndex, index);

            NodeTreeHelperBlockList.RemoveFromBlock(ParentNode, PropertyName, blockIndex, index, out bool IsBlockRemoved);

            if (IsBlockRemoved)
            {
                Debug.Assert(blockOperation != null);

                RemoveFromBlockStateList(blockIndex);

                blockOperation.Update(BlockState, OldChildState);

                for (i = blockIndex; i < BlockStateList.Count; i++)
                {
                    IWriteableBlockState NextBlockState = (IWriteableBlockState)BlockStateList[i];

                    foreach (IWriteablePlaceholderNodeState State in NextBlockState.StateList)
                    {
                        IWriteableBrowsingExistingBlockNodeIndex NodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                        Debug.Assert(NodeIndex != null);
                        Debug.Assert(NodeIndex.BlockIndex == i + 1);

                        NodeIndex.MoveBlockDown();
                    }
                }
            }
            else
            {
                Debug.Assert(nodeOperation != null);

                nodeOperation.Update(OldChildState);
            }

            i = index;
            while (i < BlockState.StateList.Count)
            {
                IWriteablePlaceholderNodeState State = (IWriteablePlaceholderNodeState)BlockState.StateList[i];

                IWriteableBrowsingExistingBlockNodeIndex NodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Debug.Assert(NodeIndex != null);
                Debug.Assert(NodeIndex.BlockIndex == blockIndex);
                Debug.Assert(NodeIndex.Index == i + 1);

                NodeIndex.MoveDown();

                i++;
            }
        }

        /// <summary>
        /// Replaces a node.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void Replace(IWriteableReplaceOperation operation)
        {
            int BlockIndex = operation.BlockIndex;
            Debug.Assert(BlockIndex >= 0 && BlockIndex < BlockStateList.Count);

            IWriteableBlockState BlockState = (IWriteableBlockState)BlockStateList[BlockIndex];

            int Index = operation.Index;
            Debug.Assert(Index >= 0 && Index < BlockState.StateList.Count);

            IBlock ChildBlock = BlockState.ChildBlock;
            Node ParentNode = Owner.Node;

            IWriteableNodeState OldChildState = (IWriteableNodeState)BlockState.StateList[Index];
            Node OldNode = OldChildState.Node;
            IWriteableBrowsingBlockNodeIndex OldBrowsingIndex = (IWriteableBrowsingBlockNodeIndex)OldChildState.ParentIndex;
            BlockState.Remove(OldBrowsingIndex, Index);

            NodeTreeHelperBlockList.ReplaceInBlock(ChildBlock, Index, operation.NewNode);

            IWriteableBrowsingExistingBlockNodeIndex NewBrowsingIndex = CreateBrowsingNodeIndex(operation.NewNode, BlockIndex, Index);
            IWriteablePlaceholderNodeState NewChildState = (IWriteablePlaceholderNodeState)CreateNodeState(NewBrowsingIndex);
            BlockState.Insert(NewBrowsingIndex, Index, NewChildState);

            operation.Update(OldBrowsingIndex, NewBrowsingIndex, OldNode, NewChildState);
        }

        /// <summary>
        /// Changes the replication state of a block.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void ChangeReplication(WriteableChangeBlockOperation operation)
        {
            ReplicationStatus Replication = operation.Replication;
            int BlockIndex = operation.BlockIndex;

            Debug.Assert(BlockIndex >= 0 && BlockIndex < BlockStateList.Count);

            IWriteableBlockState BlockState = (IWriteableBlockState)BlockStateList[BlockIndex];
            NodeTreeHelperBlockList.SetReplication(BlockState.ChildBlock, Replication);

            operation.Update(BlockState);
        }

        /// <summary>
        /// Checks whether a block can be split at the given index.
        /// </summary>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        public virtual bool IsSplittable(IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex.BlockIndex >= 0 && nodeIndex.BlockIndex < BlockStateList.Count);

            int SplitBlockIndex = nodeIndex.BlockIndex;
            int SplitIndex = nodeIndex.Index;
            IWriteableBlockState BlockState = (IWriteableBlockState)BlockStateList[SplitBlockIndex];

            Debug.Assert(SplitIndex < BlockState.StateList.Count);

            return SplitIndex > 0;
        }

        /// <summary>
        /// Splits a block in two at the given index.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void SplitBlock(WriteableSplitBlockOperation operation)
        {
            int SplitBlockIndex = operation.BlockIndex;
            Debug.Assert(SplitBlockIndex >= 0 && SplitBlockIndex < BlockStateList.Count);

            int SplitIndex = operation.Index;
            Debug.Assert(SplitIndex > 0);

            IWriteableBlockState BlockState = (IWriteableBlockState)BlockStateList[SplitBlockIndex];
            Debug.Assert(SplitIndex < BlockState.StateList.Count);

            IBlock NewBlock = operation.NewBlock;
            Debug.Assert(NewBlock != null);

            NodeTreeHelperBlockList.SplitBlock(Owner.Node, PropertyName, SplitBlockIndex, SplitIndex, NewBlock);

            NodeTreeHelperBlockList.GetChildNode(NewBlock, 0, out Node NewBlockFirstNode);
            IWriteableBrowsingNewBlockNodeIndex NewBlockIndex = CreateNewBlockNodeIndex(NewBlockFirstNode, SplitBlockIndex);

            IWriteableBlockState NewBlockState = (IWriteableBlockState)CreateBlockState(NewBlockIndex, NewBlock);
            ((IWriteableBlockState<IWriteableInner<IWriteableBrowsingChildIndex>>)NewBlockState).InitBlockState();
            InsertInBlockStateList(NewBlockIndex.BlockIndex, NewBlockState);

            for (int i = 0; i < SplitIndex; i++)
            {
                IWriteablePlaceholderNodeState State = (IWriteablePlaceholderNodeState)BlockState.StateList[0];
                IWriteableBrowsingExistingBlockNodeIndex ChildNodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Debug.Assert(ChildNodeIndex != null);

                BlockState.Remove(ChildNodeIndex, 0);
                NewBlockState.Insert(ChildNodeIndex, i, State);
            }

            operation.Update(NewBlockState);

            for (int i = 0; i < BlockState.StateList.Count; i++)
            {
                IWriteablePlaceholderNodeState State = (IWriteablePlaceholderNodeState)BlockState.StateList[i];
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
            Debug.Assert(nodeIndex.BlockIndex >= 0 && nodeIndex.BlockIndex < BlockStateList.Count);
            Debug.Assert(nodeIndex.Index >= 0);

            return nodeIndex.BlockIndex > 0 && nodeIndex.Index == 0;
        }

        /// <summary>
        /// Merges two blocks at the given index.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void MergeBlocks(WriteableMergeBlocksOperation operation)
        {
            int MergeBlockIndex = operation.BlockIndex;
            Debug.Assert(MergeBlockIndex > 0 && MergeBlockIndex < BlockStateList.Count);

            IWriteableBlockState FirstBlockState = (IWriteableBlockState)BlockStateList[MergeBlockIndex - 1];
            IWriteableBlockState SecondBlockState = (IWriteableBlockState)BlockStateList[MergeBlockIndex];
            int MergeIndex = FirstBlockState.StateList.Count;
            Debug.Assert(MergeIndex > 0);

            NodeTreeHelperBlockList.MergeBlocks(Owner.Node, PropertyName, MergeBlockIndex, out IBlock mergedBlock);
            Debug.Assert(FirstBlockState.ChildBlock == mergedBlock);

            RemoveFromBlockStateList(MergeBlockIndex - 1);

            operation.Update(FirstBlockState, MergeIndex);

            int i;
            for (i = 0; i < MergeIndex; i++)
            {
                IWriteablePlaceholderNodeState State = (IWriteablePlaceholderNodeState)FirstBlockState.StateList[0];
                IWriteableBrowsingExistingBlockNodeIndex ChildNodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                Debug.Assert(ChildNodeIndex != null);

                FirstBlockState.Remove(ChildNodeIndex, 0);
                SecondBlockState.Insert(ChildNodeIndex, i, State);
            }

            for (; i < SecondBlockState.StateList.Count; i++)
            {
                IWriteablePlaceholderNodeState State = (IWriteablePlaceholderNodeState)SecondBlockState.StateList[i];
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
            bool IsHandled = false;
            bool Result = false;

            if (nodeIndex is IWriteableBrowsingExistingBlockNodeIndex AsExistingBlockNodeIndex)
            {
                Debug.Assert(AsExistingBlockNodeIndex != null);

                int BlockIndex = AsExistingBlockNodeIndex.BlockIndex;
                Debug.Assert(BlockIndex >= 0 && BlockIndex < BlockStateList.Count);

                IWriteableBlockState BlockState = (IWriteableBlockState)BlockStateList[BlockIndex];
                WriteablePlaceholderNodeStateReadOnlyList StateList = BlockState.StateList;

                int NewPosition = AsExistingBlockNodeIndex.Index + direction;
                Result = NewPosition >= 0 && NewPosition < StateList.Count;

                IsHandled = true;
            }

            Debug.Assert(IsHandled);
            return Result;
        }

        /// <summary>
        /// Moves a node around in a list or block list. In a block list, the node stays in same block.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void Move(WriteableMoveNodeOperation operation)
        {
            int BlockIndex = operation.BlockIndex;
            Debug.Assert(BlockIndex >= 0 && BlockIndex < BlockStateList.Count);

            IWriteableBlockState BlockState = (IWriteableBlockState)BlockStateList[BlockIndex];
            WriteablePlaceholderNodeStateReadOnlyList StateList = BlockState.StateList;

            int MoveIndex = operation.Index;
            int Direction = operation.Direction;
            Debug.Assert(MoveIndex >= 0 && MoveIndex < StateList.Count);
            Debug.Assert(MoveIndex + Direction >= 0 && MoveIndex + Direction < StateList.Count);

            IWriteableBrowsingExistingBlockNodeIndex ExistingBlockNodeIndex = StateList[MoveIndex].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
            Debug.Assert(ExistingBlockNodeIndex != null);

            BlockState.Move(ExistingBlockNodeIndex, MoveIndex, Direction);

            IBlock ChildBlock = BlockState.ChildBlock;
            NodeTreeHelperBlockList.MoveNode(ChildBlock, MoveIndex, Direction);

            operation.Update((IWriteablePlaceholderNodeState)StateList[MoveIndex + Direction]);

            if (Direction > 0)
            {
                for (int i = MoveIndex; i < MoveIndex + Direction; i++)
                {
                    IWriteableBrowsingExistingBlockNodeIndex ChildNodeIndex = StateList[i].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                    Debug.Assert(ChildNodeIndex != null);

                    ChildNodeIndex.MoveDown();
                    ExistingBlockNodeIndex.MoveUp();
                }
            }
            else if (Direction < 0)
            {
                for (int i = MoveIndex; i > MoveIndex + Direction; i--)
                {
                    IWriteableBrowsingExistingBlockNodeIndex ChildNodeIndex = StateList[i].ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                    Debug.Assert(ChildNodeIndex != null);

                    ChildNodeIndex.MoveUp();
                    ExistingBlockNodeIndex.MoveDown();
                }
            }
        }

        /// <summary>
        /// Moves a block around in a block list.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void MoveBlock(WriteableMoveBlockOperation operation)
        {
            int BlockIndex = operation.BlockIndex;
            int Direction = operation.Direction;

            Debug.Assert(BlockIndex >= 0 && BlockIndex < BlockStateList.Count);
            Debug.Assert(BlockIndex + Direction >= 0 && BlockIndex + Direction < BlockStateList.Count);

            int MoveIndex = BlockIndex;
            IWriteableBlockState BlockState = (IWriteableBlockState)BlockStateList[MoveIndex];

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
        private protected override ReadOnlyBlockStateList CreateBlockStateList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableBlockListInner<IIndex>>());
            return new WriteableBlockStateList();
        }

        /// <summary>
        /// Creates a IxxxBlockState object.
        /// </summary>
        private protected override IReadOnlyBlockState CreateBlockState(IReadOnlyBrowsingNewBlockNodeIndex nodeIndex, IBlock childBlock)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableBlockListInner<IIndex>>());
            return new WriteableBlockState<IWriteableInner<IWriteableBrowsingChildIndex>>(this, (IWriteableBrowsingNewBlockNodeIndex)nodeIndex, childBlock);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableBlockListInner<IIndex>>());
            return new WriteablePlaceholderNodeState<IWriteableInner<IWriteableBrowsingChildIndex>>((IWriteableNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingBlockNodeIndexList.
        /// </summary>
        private protected override ReadOnlyBrowsingBlockNodeIndexList CreateBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableBlockListInner<IIndex>>());
            return new WriteableBrowsingBlockNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected virtual IWriteableBrowsingExistingBlockNodeIndex CreateBrowsingNodeIndex(Node node, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableBlockListInner<IIndex>>());
            return new WriteableBrowsingExistingBlockNodeIndex(Owner.Node, node, PropertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        private protected virtual IWriteableBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(Node node, int blockIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableBlockListInner<IIndex>>());
            return new WriteableBrowsingNewBlockNodeIndex(Owner.Node, node, PropertyName, blockIndex);
        }
        #endregion
    }
}
