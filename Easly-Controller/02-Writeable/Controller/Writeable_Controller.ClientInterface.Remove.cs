namespace EaslyController.Writeable
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports operations to modify the tree.
    /// </summary>
    public partial class WriteableController : ReadOnlyController
    {
        /// <summary>
        /// Checks whether a node can be removed from a list.
        /// </summary>
        /// <param name="inner">The inner where the node is.</param>
        /// <param name="nodeIndex">Index of the node that would be removed.</param>
        public bool IsRemoveable(IWriteableCollectionInner inner, IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            if (inner.Count > 1)
                return true;

            Debug.Assert(inner.Count == 1);
            Debug.Assert(inner.Owner != null);

            Node Node = inner.Owner.Node;
            string PropertyName = inner.PropertyName;
            Debug.Assert(Node != null);

            bool Result = true;

            //Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(inner.Owner.Node.GetType());
            Type InterfaceType = inner.Owner.Node.GetType();

            IReadOnlyDictionary<Type, string[]> NeverEmptyCollectionTable = NodeHelper.NeverEmptyCollectionTable;
            if (NeverEmptyCollectionTable.ContainsKey(InterfaceType))
            {
                foreach (string Item in NeverEmptyCollectionTable[InterfaceType])
                    if (Item == PropertyName)
                        Result = false;
            }

            return Result;
        }

        /// <summary>
        /// Removes a node from a list or block list.
        /// </summary>
        /// <param name="inner">The inner for the list or block list from which the node is removed.</param>
        /// <param name="nodeIndex">Index for the removed node.</param>
        public virtual void Remove(IWriteableCollectionInner inner, IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);
            IWriteableNodeState Owner = inner.Owner;
            IWriteableIndex ParentIndex = Owner.ParentIndex;
            Debug.Assert(Contains(ParentIndex));
            Debug.Assert(IndexToState(ParentIndex) == Owner);
            WriteableInnerReadOnlyDictionary<string> InnerTable = Owner.InnerTable;
            Debug.Assert(InnerTable.ContainsKey(inner.PropertyName));
            Debug.Assert(InnerTable[inner.PropertyName] == inner);

            IndexToPositionAndNode(nodeIndex, out int BlockIndex, out int Index, out _, out Node Node);

            bool IsHandled = false;

            if (inner is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner && nodeIndex is IWriteableBrowsingExistingBlockNodeIndex ExistingBlockIndex)
            {
                if (AsBlockListInner.BlockStateList[ExistingBlockIndex.BlockIndex].StateList.Count == 1)
                    RemoveBlock(AsBlockListInner, BlockIndex);
                else
                    RemoveNode(AsBlockListInner, BlockIndex, Index);

                IsHandled = true;
            }
            else if (inner is IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> AsCollectionInner)
            {
                RemoveNode(AsCollectionInner, BlockIndex, Index);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);
        }

        private protected virtual void RemoveBlock(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> blockListInner, int blockIndex)
        {
            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRemoveBlock(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoRemoveBlock(operation);
            IWriteableRemoveBlockOperation Operation = CreateRemoveBlockOperation(blockListInner.Owner.Node, blockListInner.PropertyName, blockIndex, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        private protected virtual void RedoRemoveBlock(IWriteableOperation operation)
        {
            IWriteableRemoveBlockOperation RemoveBlockOperation = (IWriteableRemoveBlockOperation)operation;
            ExecuteRemoveBlock(RemoveBlockOperation);
        }

        private protected virtual void ExecuteRemoveBlock(IWriteableRemoveBlockOperation operation)
        {
            Node ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;

            Inner.RemoveWithBlock(operation);

            IWriteableBlockState RemovedBlockState = operation.BlockState;
            Debug.Assert(RemovedBlockState != null);

            IWriteableBrowsingPatternIndex PatternIndex = RemovedBlockState.PatternIndex;
            IWriteableBrowsingSourceIndex SourceIndex = RemovedBlockState.SourceIndex;

            Debug.Assert(PatternIndex != null);
            Debug.Assert(StateTable.ContainsKey(PatternIndex));
            Debug.Assert(SourceIndex != null);
            Debug.Assert(StateTable.ContainsKey(SourceIndex));

            Stats.BlockCount--;

            RemoveState(PatternIndex);
            Stats.PlaceholderNodeCount--;

            RemoveState(SourceIndex);
            Stats.PlaceholderNodeCount--;

            IWriteableNodeState RemovedState = operation.RemovedState;
            Debug.Assert(RemovedState != null);

            PruneState(RemovedState);
            Stats.PlaceholderNodeCount--;

            NotifyBlockStateRemoved(operation);
        }

        private protected virtual void UndoRemoveBlock(IWriteableOperation operation)
        {
            IWriteableRemoveBlockOperation RemoveBlockOperation = (IWriteableRemoveBlockOperation)operation;
            IWriteableInsertBlockOperation InsertBlockOperation = RemoveBlockOperation.ToInsertBlockOperation();

            ExecuteInsertNewBlock(InsertBlockOperation);
        }

        private protected virtual void RemoveNode(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, int blockIndex, int index)
        {
            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRemoveNode(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoRemoveNode(operation);
            WriteableRemoveNodeOperation Operation = CreateRemoveNodeOperation(inner.Owner.Node, inner.PropertyName, blockIndex, index, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        private protected virtual void RedoRemoveNode(IWriteableOperation operation)
        {
            WriteableRemoveNodeOperation RemoveNodeOperation = (WriteableRemoveNodeOperation)operation;
            ExecuteRemoveNode(RemoveNodeOperation);
        }

        private protected virtual void ExecuteRemoveNode(WriteableRemoveNodeOperation operation)
        {
            Node ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex>;

            Inner.Remove(operation);

            IWriteableNodeState RemovedState = operation.RemovedState;
            PruneState(RemovedState);
            Stats.PlaceholderNodeCount--;

            NotifyStateRemoved(operation);
        }

        private protected virtual void UndoRemoveNode(IWriteableOperation operation)
        {
            WriteableRemoveNodeOperation RemoveNodeOperation = (WriteableRemoveNodeOperation)operation;
            WriteableInsertNodeOperation InsertNodeOperation = RemoveNodeOperation.ToInsertNodeOperation();

            ExecuteInsertNewNode(InsertNodeOperation);
        }

        /// <summary>
        /// Checks whether a range of blocks can be removed from a block list.
        /// </summary>
        /// <param name="inner">The inner with blocks to remove.</param>
        /// <param name="firstBlockIndex">Index of the first block to remove.</param>
        /// <param name="lastBlockIndex">Index following the last block to remove.</param>
        public virtual bool IsBlockRangeRemoveable(IWriteableBlockListInner inner, int firstBlockIndex, int lastBlockIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(firstBlockIndex >= 0 && firstBlockIndex < inner.BlockStateList.Count);
            Debug.Assert(lastBlockIndex >= 0 && lastBlockIndex <= inner.BlockStateList.Count);
            Debug.Assert(firstBlockIndex <= lastBlockIndex);

            int DeletedCount = lastBlockIndex - firstBlockIndex;
            if (inner.BlockStateList.Count > DeletedCount)
                return true;

            Debug.Assert(inner.BlockStateList.Count == DeletedCount);
            Debug.Assert(inner.Owner != null);

            Node Node = inner.Owner.Node;
            string PropertyName = inner.PropertyName;
            Debug.Assert(Node != null);

            bool Result = true;

            //Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(inner.Owner.Node.GetType());
            Type InterfaceType = inner.Owner.Node.GetType();

            IReadOnlyDictionary<Type, string[]> NeverEmptyCollectionTable = NodeHelper.NeverEmptyCollectionTable;
            if (NeverEmptyCollectionTable.ContainsKey(InterfaceType))
            {
                foreach (string Item in NeverEmptyCollectionTable[InterfaceType])
                    if (Item == PropertyName)
                        Result = false;
            }

            return Result;
        }

        /// <summary>
        /// Removes a range of blocks from a block list.
        /// </summary>
        /// <param name="inner">The inner for the block list from which blocks are removed.</param>
        /// <param name="firstBlockIndex">Index of the first block to remove.</param>
        /// <param name="lastBlockIndex">Index following the last block to remove.</param>
        public virtual void RemoveBlockRange(IWriteableBlockListInner inner, int firstBlockIndex, int lastBlockIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(firstBlockIndex >= 0 && firstBlockIndex < inner.BlockStateList.Count);
            Debug.Assert(lastBlockIndex >= 0 && lastBlockIndex <= inner.BlockStateList.Count);
            Debug.Assert(firstBlockIndex <= lastBlockIndex);

            Action<IWriteableOperation> HandlerRedoNode = (IWriteableOperation operation) => RedoRemoveNode(operation);
            Action<IWriteableOperation> HandlerUndoNode = (IWriteableOperation operation) => UndoRemoveNode(operation);
            Action<IWriteableOperation> HandlerRedoBlock = (IWriteableOperation operation) => RedoRemoveBlock(operation);
            Action<IWriteableOperation> HandlerUndoBlock = (IWriteableOperation operation) => UndoRemoveBlock(operation);

            WriteableOperationList OperationList = CreateOperationList();

            for (int i = firstBlockIndex; i < lastBlockIndex; i++)
            {
                IWriteableBlockState BlockState = (IWriteableBlockState)inner.BlockStateList[i];
                Debug.Assert(BlockState.StateList.Count >= 1);

                // Remove at firstBlockIndex since subsequent blocks are moved as the block at firstBlockIndex is deleted.
                // Same for nodes inside blokcks, delete them at 0.
                for (int j = 1; j < BlockState.StateList.Count; j++)
                {
                    WriteableRemoveNodeOperation OperationNode = CreateRemoveNodeOperation(inner.Owner.Node, inner.PropertyName, firstBlockIndex, 0, HandlerRedoNode, HandlerUndoNode, isNested: true);
                    OperationList.Add(OperationNode);
                }

                IWriteableRemoveBlockOperation OperationBlock = CreateRemoveBlockOperation(inner.Owner.Node, inner.PropertyName, firstBlockIndex, HandlerRedoBlock, HandlerUndoBlock, isNested: true);
                OperationList.Add(OperationBlock);
            }

            if (OperationList.Count > 0)
            {
                WriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRefresh(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                WriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedo, HandlerUndo, isNested: false);
                WriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);

                OperationGroup.Redo();
                SetLastOperation(OperationGroup);
                CheckInvariant();
            }
        }

        /// <summary>
        /// Checks whether a range of nodes can be removed from a list or block list.
        /// </summary>
        /// <param name="inner">The inner with nodes to remove.</param>
        /// <param name="blockIndex">Index of the block where to remove nodes, for a block list. -1 for a list.</param>
        /// <param name="firstNodeIndex">Index of the first node to remove.</param>
        /// <param name="lastNodeIndex">Index following the last node to remove.</param>
        public virtual bool IsNodeRangeRemoveable(IWriteableCollectionInner inner, int blockIndex, int firstNodeIndex, int lastNodeIndex)
        {
            Debug.Assert(inner != null);

            bool IsHandled = false;
            bool Result = false;

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                Result = IsNodeRangeRemoveable(AsBlockListInner, blockIndex, firstNodeIndex, lastNodeIndex);
                IsHandled = true;
            }
            else if (inner is IWriteableListInner AsListInner)
            {
                Debug.Assert(blockIndex == -1);
                Result = IsNodeRangeRemoveable(AsListInner, firstNodeIndex, lastNodeIndex);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);

            if (!Result)
            {
                Debug.Assert(inner.Owner != null);

                Node Node = inner.Owner.Node;
                string PropertyName = inner.PropertyName;
                Debug.Assert(Node != null);

                Result = true;

                //Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(inner.Owner.Node.GetType());
                Type InterfaceType = inner.Owner.Node.GetType();

                IReadOnlyDictionary<Type, string[]> NeverEmptyCollectionTable = NodeHelper.NeverEmptyCollectionTable;
                if (NeverEmptyCollectionTable.ContainsKey(InterfaceType))
                {
                    foreach (string Item in NeverEmptyCollectionTable[InterfaceType])
                        if (Item == PropertyName)
                            Result = false;
                }
            }

            return Result;
        }

        private protected virtual bool IsNodeRangeRemoveable(IWriteableBlockListInner inner, int blockIndex, int firstNodeIndex, int lastNodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(blockIndex >= 0 && blockIndex < inner.BlockStateList.Count);

            IWriteableBlockState BlockState = (IWriteableBlockState)inner.BlockStateList[blockIndex];
            Debug.Assert(firstNodeIndex >= 0 && firstNodeIndex < BlockState.StateList.Count);
            Debug.Assert(lastNodeIndex >= 0 && lastNodeIndex <= BlockState.StateList.Count);
            Debug.Assert(firstNodeIndex <= lastNodeIndex);

            int DeletedCount = lastNodeIndex - firstNodeIndex;
            return inner.BlockStateList.Count > 1 || BlockState.StateList.Count > DeletedCount;
        }

        private protected virtual bool IsNodeRangeRemoveable(IWriteableListInner inner, int firstNodeIndex, int lastNodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(firstNodeIndex >= 0 && firstNodeIndex < inner.StateList.Count);
            Debug.Assert(lastNodeIndex >= 0 && lastNodeIndex <= inner.StateList.Count);
            Debug.Assert(firstNodeIndex <= lastNodeIndex);

            int DeletedCount = lastNodeIndex - firstNodeIndex;
            return inner.StateList.Count > DeletedCount;
        }

        /// <summary>
        /// Removes a range of nodes from a list or block list.
        /// </summary>
        /// <param name="inner">The inner with nodes to remove.</param>
        /// <param name="blockIndex">Index of the block where to remove nodes, for a block list. -1 for a list.</param>
        /// <param name="firstNodeIndex">Index of the first node to remove.</param>
        /// <param name="lastNodeIndex">Index following the last node to remove.</param>
        public virtual void RemoveNodeRange(IWriteableCollectionInner inner, int blockIndex, int firstNodeIndex, int lastNodeIndex)
        {
            Debug.Assert(inner != null);

            bool IsHandled = false;

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                RemoveNodeRange(AsBlockListInner, blockIndex, firstNodeIndex, lastNodeIndex);
                IsHandled = true;
            }
            else if (inner is IWriteableListInner AsListInner)
            {
                Debug.Assert(blockIndex == -1);
                RemoveNodeRange(AsListInner, firstNodeIndex, lastNodeIndex);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);
        }

        /// <summary></summary>
        public virtual void RemoveNodeRange(IWriteableBlockListInner inner, int blockIndex, int firstNodeIndex, int lastNodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(blockIndex >= 0 && blockIndex < inner.BlockStateList.Count);

            IWriteableBlockState BlockState = (IWriteableBlockState)inner.BlockStateList[blockIndex];
            Debug.Assert(firstNodeIndex >= 0 && firstNodeIndex < BlockState.StateList.Count);
            Debug.Assert(lastNodeIndex >= 0 && lastNodeIndex <= BlockState.StateList.Count);
            Debug.Assert(firstNodeIndex <= lastNodeIndex);

            int DeletedCount = lastNodeIndex - firstNodeIndex;

            Action<IWriteableOperation> HandlerRedoRemoveNode = (IWriteableOperation operation) => RedoRemoveNode(operation);
            Action<IWriteableOperation> HandlerUndoRemoveNode = (IWriteableOperation operation) => UndoRemoveNode(operation);
            Action<IWriteableOperation> HandlerRedoRemoveBlock = (IWriteableOperation operation) => RedoRemoveBlock(operation);
            Action<IWriteableOperation> HandlerUndoRemoveBlock = (IWriteableOperation operation) => UndoRemoveBlock(operation);

           WriteableOperationList OperationList = CreateOperationList();

            for (int i = firstNodeIndex; i < lastNodeIndex; i++)
            {
                IWriteableRemoveOperation Operation;

                if (DeletedCount < BlockState.StateList.Count || i + 1 < lastNodeIndex)
                    Operation = CreateRemoveNodeOperation(inner.Owner.Node, inner.PropertyName, blockIndex, firstNodeIndex, HandlerRedoRemoveNode, HandlerUndoRemoveNode, isNested: true);
                else
                    Operation = CreateRemoveBlockOperation(inner.Owner.Node, inner.PropertyName, blockIndex, HandlerRedoRemoveBlock, HandlerUndoRemoveBlock, isNested: true);

                OperationList.Add(Operation);
            }

            if (OperationList.Count > 0)
            {
                WriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRefresh(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                WriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedo, HandlerUndo, isNested: false);
                WriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);

                OperationGroup.Redo();
                SetLastOperation(OperationGroup);
                CheckInvariant();
            }
        }

        /// <summary></summary>
        public virtual void RemoveNodeRange(IWriteableListInner inner, int firstNodeIndex, int lastNodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(firstNodeIndex >= 0 && firstNodeIndex < inner.StateList.Count);
            Debug.Assert(lastNodeIndex >= 0 && lastNodeIndex <= inner.StateList.Count);
            Debug.Assert(firstNodeIndex <= lastNodeIndex);

            Action<IWriteableOperation> HandlerRedoNode = (IWriteableOperation operation) => RedoRemoveNode(operation);
            Action<IWriteableOperation> HandlerUndoNode = (IWriteableOperation operation) => UndoRemoveNode(operation);

            WriteableOperationList OperationList = CreateOperationList();

            for (int i = firstNodeIndex; i < lastNodeIndex; i++)
            {
                WriteableRemoveNodeOperation OperationNode = CreateRemoveNodeOperation(inner.Owner.Node, inner.PropertyName, -1, firstNodeIndex, HandlerRedoNode, HandlerUndoNode, isNested: true);
                OperationList.Add(OperationNode);
            }

            if (OperationList.Count > 0)
            {
                WriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRefresh(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                WriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedo, HandlerUndo, isNested: false);
                WriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);

                OperationGroup.Redo();
                SetLastOperation(OperationGroup);
                CheckInvariant();
            }
        }
    }
}
