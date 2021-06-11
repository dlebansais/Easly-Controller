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
    public partial class WriteableController : ReadOnlyController, IWriteableController
    {
        /// <summary>
        /// Replace an existing node with a new one.
        /// </summary>
        /// <param name="inner">The inner where the node is replaced.</param>
        /// <param name="replaceIndex">Index for the replace operation.</param>
        /// <param name="nodeIndex">Index of the replacing node upon return.</param>
        public void Replace(IWriteableInner inner, IWriteableInsertionChildIndex replaceIndex, out IWriteableBrowsingChildIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(replaceIndex != null);
            IWriteableNodeState Owner = inner.Owner;
            IWriteableIndex ParentIndex = Owner.ParentIndex;
            Debug.Assert(Contains(ParentIndex));
            Debug.Assert(IndexToState(ParentIndex) == Owner);
            IWriteableInnerReadOnlyDictionary<string> InnerTable = Owner.InnerTable;
            Debug.Assert(InnerTable.ContainsKey(inner.PropertyName));
            Debug.Assert(InnerTable[inner.PropertyName] == inner);

            IndexToPositionAndNode(replaceIndex, out int BlockIndex, out int Index, out INode NewNode);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoReplace(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoReplace(operation);
            IWriteableReplaceOperation Operation = CreateReplaceOperation(inner.Owner.Node, inner.PropertyName, BlockIndex, Index, NewNode, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();

            nodeIndex = Operation.NewBrowsingIndex;
        }

        private protected virtual void RedoReplace(IWriteableOperation operation)
        {
            IWriteableReplaceOperation ReplaceOperation = (IWriteableReplaceOperation)operation;
            ExecuteReplace(ReplaceOperation);
        }

        private protected virtual void UndoReplace(IWriteableOperation operation)
        {
            IWriteableReplaceOperation ReplaceOperation = (IWriteableReplaceOperation)operation;
            ReplaceOperation = ReplaceOperation.ToInverseReplace();

            ExecuteReplace(ReplaceOperation);
        }

        private protected virtual void ExecuteReplace(IWriteableReplaceOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableInner<IWriteableBrowsingChildIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableInner<IWriteableBrowsingChildIndex>;

            ReplaceState(operation, Inner);
            Debug.Assert(Contains(operation.NewBrowsingIndex));

            NotifyStateReplaced(operation);
        }

        private protected virtual void ReplaceState(IWriteableReplaceOperation operation, IWriteableInner<IWriteableBrowsingChildIndex> inner)
        {
            Debug.Assert(inner != null);
            IWriteableNodeState Owner = inner.Owner;
            IWriteableIndex ParentIndex = Owner.ParentIndex;
            Debug.Assert(Contains(ParentIndex));
            Debug.Assert(IndexToState(ParentIndex) == Owner);
            IWriteableInnerReadOnlyDictionary<string> InnerTable = Owner.InnerTable;
            Debug.Assert(InnerTable.ContainsKey(inner.PropertyName));
            Debug.Assert(InnerTable[inner.PropertyName] == inner);

            IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> AsOptionalInner = inner as IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex>;
            if (AsOptionalInner != null)
            {
                IWriteableNodeState OldState = AsOptionalInner.ChildState;
                PruneStateChildren(OldState);

                if (AsOptionalInner.IsAssigned)
                    Stats.AssignedOptionalNodeCount--;
            }

            inner.Replace(operation);

            IWriteableBrowsingChildIndex OldBrowsingIndex = operation.OldBrowsingIndex;
            IWriteableBrowsingChildIndex NewBrowsingIndex = operation.NewBrowsingIndex;
            IWriteableNodeState ChildState = operation.NewChildState;

            if (AsOptionalInner != null)
            {
                if (AsOptionalInner.IsAssigned)
                    Stats.AssignedOptionalNodeCount++;
            }
            else
            {
                Debug.Assert(Contains(OldBrowsingIndex));
                IWriteableNodeState OldState = StateTable[OldBrowsingIndex];

                PruneStateChildren(OldState);
            }

            RemoveState(OldBrowsingIndex);
            AddState(NewBrowsingIndex, ChildState);

            BuildStateTable(inner, null, NewBrowsingIndex, ChildState);
        }

        /// <summary>
        /// Removes a range of blocks from a block list and replace them with other blocks.
        /// </summary>
        /// <param name="inner">The inner for the block list from which blocks are replaced.</param>
        /// <param name="firstBlockIndex">Index of the first block to remove.</param>
        /// <param name="lastBlockIndex">Index following the last block to remove.</param>
        /// <param name="indexList">List of nodes in blocks to insert.</param>
        public virtual void ReplaceBlockRange(IWriteableBlockListInner inner, int firstBlockIndex, int lastBlockIndex, IList<IWriteableInsertionBlockNodeIndex> indexList)
        {
            Debug.Assert(inner != null);
            Debug.Assert(firstBlockIndex >= 0 && firstBlockIndex < inner.BlockStateList.Count);
            Debug.Assert(lastBlockIndex >= 0 && lastBlockIndex <= inner.BlockStateList.Count);
            Debug.Assert(firstBlockIndex <= lastBlockIndex);
            Debug.Assert(indexList != null);

            int BlockIndex = firstBlockIndex - 1;
            int BlockNodeIndex = 0;

            foreach (IWriteableInsertionBlockNodeIndex NodeIndex in indexList)
            {
                bool IsHandled = false;

                if (NodeIndex is IWriteableInsertionNewBlockNodeIndex AsNewBlockNodeIndex)
                {
                    BlockIndex++;
                    BlockNodeIndex = 0;

                    Debug.Assert(AsNewBlockNodeIndex.BlockIndex == BlockIndex);

                    IsHandled = true;
                }
                else if (NodeIndex is IWriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex)
                {
                    BlockNodeIndex++;

                    Debug.Assert(AsExistingBlockNodeIndex.BlockIndex == BlockIndex);
                    Debug.Assert(AsExistingBlockNodeIndex.Index == BlockNodeIndex);

                    IsHandled = true;
                }

                Debug.Assert(IsHandled);
            }

            int FinalBlockIndex = BlockIndex + 1;

            Action<IWriteableOperation> HandlerRedoInsertNode = (IWriteableOperation operation) => RedoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerUndoInsertNode = (IWriteableOperation operation) => UndoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerRedoInsertBlock = (IWriteableOperation operation) => RedoInsertNewBlock(operation);
            Action<IWriteableOperation> HandlerUndoInsertBlock = (IWriteableOperation operation) => UndoInsertNewBlock(operation);
            Action<IWriteableOperation> HandlerRedoRemoveNode = (IWriteableOperation operation) => RedoRemoveNode(operation);
            Action<IWriteableOperation> HandlerUndoRemoveNode = (IWriteableOperation operation) => UndoRemoveNode(operation);
            Action<IWriteableOperation> HandlerRedoRemoveBlock = (IWriteableOperation operation) => RedoRemoveBlock(operation);
            Action<IWriteableOperation> HandlerUndoRemoveBlock = (IWriteableOperation operation) => UndoRemoveBlock(operation);

            IWriteableOperationList OperationList = CreateOperationList();

            // Insert first to prevent empty block lists.
            foreach (IWriteableInsertionBlockNodeIndex NodeIndex in indexList)
                if (NodeIndex is IWriteableInsertionNewBlockNodeIndex AsNewBlockNodeIndex)
                {
                    IBlock NewBlock = NodeTreeHelperBlockList.CreateBlock(inner.Owner.Node, inner.PropertyName, ReplicationStatus.Normal, AsNewBlockNodeIndex.PatternNode, AsNewBlockNodeIndex.SourceNode);
                    IWriteableInsertBlockOperation OperationInsertBlock = CreateInsertBlockOperation(inner.Owner.Node, inner.PropertyName, AsNewBlockNodeIndex.BlockIndex, NewBlock, AsNewBlockNodeIndex.Node, HandlerRedoInsertBlock, HandlerUndoInsertBlock, isNested: true);
                    OperationList.Add(OperationInsertBlock);
                }
                else if (NodeIndex is IWriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex)
                {
                    IndexToPositionAndNode(AsExistingBlockNodeIndex, out BlockIndex, out int Index, out INode Node);
                    IWriteableInsertNodeOperation OperationInsertNode = CreateInsertNodeOperation(inner.Owner.Node, inner.PropertyName, BlockIndex, Index, Node, HandlerRedoInsertNode, HandlerUndoInsertNode, isNested: true);
                    OperationList.Add(OperationInsertNode);
                }

            Debug.Assert(BlockIndex + 1 == FinalBlockIndex);

            for (int i = FinalBlockIndex; i < FinalBlockIndex + lastBlockIndex - firstBlockIndex; i++)
            {
                IWriteableBlockState BlockState = inner.BlockStateList[i + firstBlockIndex - FinalBlockIndex];
                Debug.Assert(BlockState.StateList.Count >= 1);

                // Remove at FinalBlockIndex since subsequent blocks are moved as the block at FinalBlockIndex is deleted.
                // Same for nodes inside blokcks, delete them at 0.
                for (int j = 1; j < BlockState.StateList.Count; j++)
                {
                    IWriteableRemoveNodeOperation OperationNode = CreateRemoveNodeOperation(inner.Owner.Node, inner.PropertyName, FinalBlockIndex, 0, HandlerRedoRemoveNode, HandlerUndoRemoveNode, isNested: true);
                    OperationList.Add(OperationNode);
                }

                IWriteableRemoveBlockOperation OperationBlock = CreateRemoveBlockOperation(inner.Owner.Node, inner.PropertyName, FinalBlockIndex, HandlerRedoRemoveBlock, HandlerUndoRemoveBlock, isNested: true);
                OperationList.Add(OperationBlock);
            }

            if (OperationList.Count > 0)
            {
                IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRefresh(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                IWriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedo, HandlerUndo, isNested: false);
                IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);

                OperationGroup.Redo();
                SetLastOperation(OperationGroup);
                CheckInvariant();
            }
        }

        /// <summary>
        /// Removes a range of nodes from a list or block list and replace them with other nodes.
        /// </summary>
        /// <param name="inner">The inner for the list or block list from which nodes are replaced.</param>
        /// <param name="blockIndex">Index of the block where to remove nodes, for a block list. -1 for a list.</param>
        /// <param name="firstNodeIndex">Index of the first node to remove.</param>
        /// <param name="lastNodeIndex">Index following the last node to remove.</param>
        /// <param name="indexList">List of nodes to insert.</param>
        public virtual void ReplaceNodeRange(IWriteableCollectionInner inner, int blockIndex, int firstNodeIndex, int lastNodeIndex, IList<IWriteableInsertionCollectionNodeIndex> indexList)
        {
            Debug.Assert(inner != null);

            bool IsHandled = false;

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                ReplaceNodeRange(AsBlockListInner, blockIndex, firstNodeIndex, lastNodeIndex, indexList);
                IsHandled = true;
            }

            else if (inner is IWriteableListInner AsListInner)
            {
                Debug.Assert(blockIndex == -1);
                ReplaceNodeRange(AsListInner, firstNodeIndex, lastNodeIndex, indexList);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);
        }

        /// <summary></summary>
        public virtual void ReplaceNodeRange(IWriteableBlockListInner inner, int blockIndex, int firstNodeIndex, int lastNodeIndex, IList<IWriteableInsertionCollectionNodeIndex> indexList)
        {
            Debug.Assert(inner != null);
            Debug.Assert(blockIndex >= 0 && blockIndex < inner.BlockStateList.Count);

            IWriteableBlockState BlockState = inner.BlockStateList[blockIndex];
            Debug.Assert(firstNodeIndex >= 0 && firstNodeIndex < BlockState.StateList.Count);
            Debug.Assert(lastNodeIndex >= 0 && lastNodeIndex <= BlockState.StateList.Count);
            Debug.Assert(firstNodeIndex <= lastNodeIndex);

            int BlockNodeIndex = firstNodeIndex;

            foreach (IWriteableInsertionCollectionNodeIndex NodeIndex in indexList)
            {
                bool IsHandled = false;

                if (NodeIndex is IWriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex)
                {
                    Debug.Assert(AsExistingBlockNodeIndex.BlockIndex == blockIndex);
                    Debug.Assert(AsExistingBlockNodeIndex.Index == BlockNodeIndex);

                    BlockNodeIndex++;
                    IsHandled = true;
                }

                Debug.Assert(IsHandled);
            }

            int FinalNodeIndex = BlockNodeIndex;
            int DeletedCount = lastNodeIndex - firstNodeIndex;

            Action<IWriteableOperation> HandlerRedoInsertNode = (IWriteableOperation operation) => RedoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerUndoInsertNode = (IWriteableOperation operation) => UndoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerRedoRemoveNode = (IWriteableOperation operation) => RedoRemoveNode(operation);
            Action<IWriteableOperation> HandlerUndoRemoveNode = (IWriteableOperation operation) => UndoRemoveNode(operation);
            Action<IWriteableOperation> HandlerRedoRemoveBlock = (IWriteableOperation operation) => RedoRemoveBlock(operation);
            Action<IWriteableOperation> HandlerUndoRemoveBlock = (IWriteableOperation operation) => UndoRemoveBlock(operation);

            IWriteableOperationList OperationList = CreateOperationList();

            // Insert first to prevent empty block lists.
            foreach (IWriteableInsertionCollectionNodeIndex NodeIndex in indexList)
                if (NodeIndex is IWriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex)
                {
                    IndexToPositionAndNode(AsExistingBlockNodeIndex, out blockIndex, out int Index, out INode Node);
                    IWriteableInsertNodeOperation OperationInsertNode = CreateInsertNodeOperation(inner.Owner.Node, inner.PropertyName, blockIndex, Index, Node, HandlerRedoInsertNode, HandlerUndoInsertNode, isNested: true);
                    OperationList.Add(OperationInsertNode);
                }

            for (int i = FinalNodeIndex; i < FinalNodeIndex + lastNodeIndex - firstNodeIndex; i++)
            {
                IWriteableRemoveOperation Operation;

                if (DeletedCount < BlockState.StateList.Count || indexList.Count > 0 || i + 1 < FinalNodeIndex + lastNodeIndex - firstNodeIndex)
                    Operation = CreateRemoveNodeOperation(inner.Owner.Node, inner.PropertyName, blockIndex, FinalNodeIndex, HandlerRedoRemoveNode, HandlerUndoRemoveNode, isNested: true);
                else
                    Operation = CreateRemoveBlockOperation(inner.Owner.Node, inner.PropertyName, blockIndex, HandlerRedoRemoveBlock, HandlerUndoRemoveBlock, isNested: true);

                OperationList.Add(Operation);
            }

            if (OperationList.Count > 0)
            {
                IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRefresh(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                IWriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedo, HandlerUndo, isNested: false);
                IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);

                OperationGroup.Redo();
                SetLastOperation(OperationGroup);
                CheckInvariant();
            }
        }

        /// <summary></summary>
        public virtual void ReplaceNodeRange(IWriteableListInner inner, int firstNodeIndex, int lastNodeIndex, IList<IWriteableInsertionCollectionNodeIndex> indexList)
        {
            Debug.Assert(inner != null);
            Debug.Assert(firstNodeIndex >= 0 && firstNodeIndex < inner.StateList.Count);
            Debug.Assert(lastNodeIndex >= 0 && lastNodeIndex <= inner.StateList.Count);
            Debug.Assert(firstNodeIndex <= lastNodeIndex);

            int BlockNodeIndex = firstNodeIndex;

            foreach (IWriteableInsertionCollectionNodeIndex NodeIndex in indexList)
            {
                bool IsHandled = false;

                if (NodeIndex is IWriteableInsertionListNodeIndex AsListNodeIndex)
                {
                    Debug.Assert(AsListNodeIndex.Index == BlockNodeIndex);

                    BlockNodeIndex++;
                    IsHandled = true;
                }

                Debug.Assert(IsHandled);
            }

            int FinalNodeIndex = BlockNodeIndex;

            Action<IWriteableOperation> HandlerRedoInsertNode = (IWriteableOperation operation) => RedoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerUndoInsertNode = (IWriteableOperation operation) => UndoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerRedoRemoveNode = (IWriteableOperation operation) => RedoRemoveNode(operation);
            Action<IWriteableOperation> HandlerUndoRemoveNode = (IWriteableOperation operation) => UndoRemoveNode(operation);

            IWriteableOperationList OperationList = CreateOperationList();

            // Insert first to prevent empty block lists.
            foreach (IWriteableInsertionCollectionNodeIndex NodeIndex in indexList)
                if (NodeIndex is IWriteableInsertionListNodeIndex AsListNodeIndex)
                {
                    IndexToPositionAndNode(AsListNodeIndex, out int BlockIndex, out int Index, out INode Node);
                    IWriteableInsertNodeOperation OperationInsertNode = CreateInsertNodeOperation(inner.Owner.Node, inner.PropertyName, BlockIndex, Index, Node, HandlerRedoInsertNode, HandlerUndoInsertNode, isNested: true);
                    OperationList.Add(OperationInsertNode);
                }

            for (int i = FinalNodeIndex; i < FinalNodeIndex + lastNodeIndex - firstNodeIndex; i++)
            {
                IWriteableRemoveNodeOperation OperationNode = CreateRemoveNodeOperation(inner.Owner.Node, inner.PropertyName, -1, FinalNodeIndex, HandlerRedoRemoveNode, HandlerUndoRemoveNode, isNested: true);
                OperationList.Add(OperationNode);
            }

            if (OperationList.Count > 0)
            {
                IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRefresh(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                IWriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedo, HandlerUndo, isNested: false);
                IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);

                OperationGroup.Redo();
                SetLastOperation(OperationGroup);
                CheckInvariant();
            }
        }
    }
}
