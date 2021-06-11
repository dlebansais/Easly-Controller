namespace EaslyController.Writeable
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.Controller;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports operations to modify the tree.
    /// </summary>
    public partial class WriteableController : ReadOnlyController, IWriteableController
    {
        /// <summary>
        /// Inserts a new node in a list or block list.
        /// </summary>
        /// <param name="inner">The inner for the list or block list where the node is inserted.</param>
        /// <param name="insertedIndex">Index for the insertion operation.</param>
        /// <param name="nodeIndex">Index of the inserted node upon return.</param>
        public virtual void Insert(IWriteableCollectionInner inner, IWriteableInsertionCollectionNodeIndex insertedIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(insertedIndex != null);
            IWriteableNodeState Owner = inner.Owner;
            IWriteableIndex ParentIndex = Owner.ParentIndex;
            Debug.Assert(Contains(ParentIndex));
            Debug.Assert(IndexToState(ParentIndex) == Owner);
            IWriteableInnerReadOnlyDictionary<string> InnerTable = Owner.InnerTable;
            Debug.Assert(InnerTable.ContainsKey(inner.PropertyName));
            Debug.Assert(InnerTable[inner.PropertyName] == inner);

            bool IsHandled = false;
            nodeIndex = null;

            if (inner is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner && insertedIndex is IWriteableInsertionNewBlockNodeIndex AsNewBlockIndex)
            {
                InsertNewBlock(AsBlockListInner, AsNewBlockIndex, out nodeIndex);
                IsHandled = true;
            }
            else if (inner is IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> AsCollectionInner && insertedIndex is IWriteableInsertionCollectionNodeIndex AsCollectionIndex)
            {
                InsertNewNode(AsCollectionInner, AsCollectionIndex, out nodeIndex);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);
        }

        private protected virtual void InsertNewBlock(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> blockListInner, IWriteableInsertionNewBlockNodeIndex newBlockIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            IBlock NewBlock = NodeTreeHelperBlockList.CreateBlock(blockListInner.Owner.Node, blockListInner.PropertyName, ReplicationStatus.Normal, newBlockIndex.PatternNode, newBlockIndex.SourceNode);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoInsertNewBlock(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoInsertNewBlock(operation);
            IWriteableInsertBlockOperation Operation = CreateInsertBlockOperation(blockListInner.Owner.Node, blockListInner.PropertyName, newBlockIndex.BlockIndex, NewBlock, newBlockIndex.Node, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();

            nodeIndex = Operation.BrowsingIndex;
        }

        private protected virtual void RedoInsertNewBlock(IWriteableOperation operation)
        {
            IWriteableInsertBlockOperation InsertBlockOperation = (IWriteableInsertBlockOperation)operation;
            ExecuteInsertNewBlock(InsertBlockOperation);
        }

        private protected virtual void ExecuteInsertNewBlock(IWriteableInsertBlockOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;

            Inner.InsertNewBlock(operation);

            IWriteableBrowsingExistingBlockNodeIndex BrowsingIndex = operation.BrowsingIndex;
            IWriteableBlockState BlockState = operation.BlockState;
            IWriteablePlaceholderNodeState ChildState = operation.ChildState;

            Debug.Assert(BlockState.StateList.Count == 1);
            Debug.Assert(BlockState.StateList[0] == ChildState);
            ((IWriteableBlockState<IWriteableInner<IWriteableBrowsingChildIndex>>)BlockState).InitBlockState();
            Stats.BlockCount++;

            IWriteableBrowsingPatternIndex PatternIndex = BlockState.PatternIndex;
            IWriteablePatternState PatternState = BlockState.PatternState;
            AddState(PatternIndex, PatternState);
            Stats.PlaceholderNodeCount++;

            IWriteableBrowsingSourceIndex SourceIndex = BlockState.SourceIndex;
            IWriteableSourceState SourceState = BlockState.SourceState;
            AddState(SourceIndex, SourceState);
            Stats.PlaceholderNodeCount++;

            AddState(BrowsingIndex, ChildState);
            Stats.PlaceholderNodeCount++;
            BuildStateTable(Inner, null, BrowsingIndex, ChildState);

            Debug.Assert(Contains(BrowsingIndex));

            NotifyBlockStateInserted(operation);
        }

        private protected virtual void UndoInsertNewBlock(IWriteableOperation operation)
        {
            IWriteableInsertBlockOperation InsertBlockOperation = (IWriteableInsertBlockOperation)operation;
            IWriteableRemoveBlockOperation RemoveBlockOperation = InsertBlockOperation.ToRemoveBlockOperation();

            ExecuteRemoveBlock(RemoveBlockOperation);
        }

        private protected virtual void InsertNewNode(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableInsertionCollectionNodeIndex insertedIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            IndexToPositionAndNode(insertedIndex, out int BlockIndex, out int Index, out INode Node);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoInsertNewNode(operation);
            IWriteableInsertNodeOperation Operation = CreateInsertNodeOperation(inner.Owner.Node, inner.PropertyName, BlockIndex, Index, Node, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();

            nodeIndex = Operation.BrowsingIndex;
        }

        private protected virtual void RedoInsertNewNode(IWriteableOperation operation)
        {
            IWriteableInsertNodeOperation InsertNodeOperation = (IWriteableInsertNodeOperation)operation;
            ExecuteInsertNewNode(InsertNodeOperation);
        }

        private protected virtual void ExecuteInsertNewNode(IWriteableInsertNodeOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex>;

            Inner.Insert(operation);

            IWriteableBrowsingCollectionNodeIndex BrowsingIndex = operation.BrowsingIndex;
            IWriteablePlaceholderNodeState ChildState = operation.ChildState;

            AddState(BrowsingIndex, ChildState);
            Stats.PlaceholderNodeCount++;
            BuildStateTable(Inner, null, BrowsingIndex, ChildState);

            Debug.Assert(Contains(BrowsingIndex));

            NotifyStateInserted(operation);
        }

        private protected virtual void UndoInsertNewNode(IWriteableOperation operation)
        {
            IWriteableInsertNodeOperation InsertNodeOperation = (IWriteableInsertNodeOperation)operation;
            IWriteableRemoveNodeOperation RemoveNodeOperation = InsertNodeOperation.ToRemoveNodeOperation();

            ExecuteRemoveNode(RemoveNodeOperation);
        }

        /// <summary>
        /// Inserts a range of blocks in a block list.
        /// </summary>
        /// <param name="inner">The inner for the block list in which blocks are inserted.</param>
        /// <param name="insertedIndex">Index where to insert the first block.</param>
        /// <param name="indexList">List of nodes in blocks to insert.</param>
        public virtual void InsertBlockRange(IWriteableBlockListInner inner, int insertedIndex, IList<IWriteableInsertionBlockNodeIndex> indexList)
        {
            Debug.Assert(inner != null);
            Debug.Assert(insertedIndex >= 0 && insertedIndex <= inner.BlockStateList.Count);
            Debug.Assert(indexList != null);

            int BlockIndex = insertedIndex - 1;
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

            IWriteableOperationList OperationList = CreateOperationList();

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
        /// Inserts a range of nodes in a list or block list.
        /// </summary>
        /// <param name="inner">The inner for the list or block list in which nodes are inserted.</param>
        /// <param name="blockIndex">Index of the block where to insert nodes, for a block list. -1 for a list.</param>
        /// <param name="insertedIndex">Index of the first node to insert.</param>
        /// <param name="indexList">List of nodes to insert.</param>
        public virtual void InsertNodeRange(IWriteableCollectionInner inner, int blockIndex, int insertedIndex, IList<IWriteableInsertionCollectionNodeIndex> indexList)
        {
            Debug.Assert(inner != null);

            bool IsHandled = false;

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                InsertNodeRange(AsBlockListInner, blockIndex, insertedIndex, indexList);
                IsHandled = true;
            }

            else if (inner is IWriteableListInner AsListInner)
            {
                Debug.Assert(blockIndex == -1);
                InsertNodeRange(AsListInner, insertedIndex, indexList);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);
        }

        /// <summary></summary>
        public virtual void InsertNodeRange(IWriteableBlockListInner inner, int blockIndex, int insertedIndex, IList<IWriteableInsertionCollectionNodeIndex> indexList)
        {
            Debug.Assert(inner != null);
            Debug.Assert(blockIndex >= 0 && blockIndex < inner.BlockStateList.Count);

            IWriteableBlockState BlockState = inner.BlockStateList[blockIndex];
            Debug.Assert(insertedIndex >= 0 && insertedIndex <= BlockState.StateList.Count);

            int BlockNodeIndex = insertedIndex;

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

            Action<IWriteableOperation> HandlerRedoInsertNode = (IWriteableOperation operation) => RedoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerUndoInsertNode = (IWriteableOperation operation) => UndoInsertNewNode(operation);

            IWriteableOperationList OperationList = CreateOperationList();

            foreach (IWriteableInsertionCollectionNodeIndex NodeIndex in indexList)
                if (NodeIndex is IWriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex)
                {
                    IndexToPositionAndNode(AsExistingBlockNodeIndex, out blockIndex, out int Index, out INode Node);
                    IWriteableInsertNodeOperation OperationInsertNode = CreateInsertNodeOperation(inner.Owner.Node, inner.PropertyName, blockIndex, Index, Node, HandlerRedoInsertNode, HandlerUndoInsertNode, isNested: true);
                    OperationList.Add(OperationInsertNode);
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
        public virtual void InsertNodeRange(IWriteableListInner inner, int insertedIndex, IList<IWriteableInsertionCollectionNodeIndex> indexList)
        {
            Debug.Assert(inner != null);
            Debug.Assert(insertedIndex >= 0 && insertedIndex <= inner.StateList.Count);

            int BlockNodeIndex = insertedIndex;

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

            IWriteableOperationList OperationList = CreateOperationList();

            foreach (IWriteableInsertionCollectionNodeIndex NodeIndex in indexList)
                if (NodeIndex is IWriteableInsertionListNodeIndex AsListNodeIndex)
                {
                    IndexToPositionAndNode(AsListNodeIndex, out int BlockIndex, out int Index, out INode Node);
                    IWriteableInsertNodeOperation OperationInsertNode = CreateInsertNodeOperation(inner.Owner.Node, inner.PropertyName, BlockIndex, Index, Node, HandlerRedoInsertNode, HandlerUndoInsertNode, isNested: true);
                    OperationList.Add(OperationInsertNode);
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

            INode Node = inner.Owner.Node;
            string PropertyName = inner.PropertyName;
            Debug.Assert(Node != null);

            bool Result = true;

            Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(inner.Owner.Node.GetType());
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
            IWriteableInnerReadOnlyDictionary<string> InnerTable = Owner.InnerTable;
            Debug.Assert(InnerTable.ContainsKey(inner.PropertyName));
            Debug.Assert(InnerTable[inner.PropertyName] == inner);

            IndexToPositionAndNode(nodeIndex, out int BlockIndex, out int Index, out INode Node);

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
            INode ParentNode = operation.ParentNode;
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
            IWriteableRemoveNodeOperation Operation = CreateRemoveNodeOperation(inner.Owner.Node, inner.PropertyName, blockIndex, index, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        private protected virtual void RedoRemoveNode(IWriteableOperation operation)
        {
            IWriteableRemoveNodeOperation RemoveNodeOperation = (IWriteableRemoveNodeOperation)operation;
            ExecuteRemoveNode(RemoveNodeOperation);
        }

        private protected virtual void ExecuteRemoveNode(IWriteableRemoveNodeOperation operation)
        {
            INode ParentNode = operation.ParentNode;
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
            IWriteableRemoveNodeOperation RemoveNodeOperation = (IWriteableRemoveNodeOperation)operation;
            IWriteableInsertNodeOperation InsertNodeOperation = RemoveNodeOperation.ToInsertNodeOperation();

            ExecuteInsertNewNode(InsertNodeOperation);
        }

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

            INode Node = inner.Owner.Node;
            string PropertyName = inner.PropertyName;
            Debug.Assert(Node != null);

            bool Result = true;

            Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(inner.Owner.Node.GetType());
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

            IWriteableOperationList OperationList = CreateOperationList();

            for (int i = firstBlockIndex; i < lastBlockIndex; i++)
            {
                IWriteableBlockState BlockState = inner.BlockStateList[i];
                Debug.Assert(BlockState.StateList.Count >= 1);

                // Remove at firstBlockIndex since subsequent blocks are moved as the block at firstBlockIndex is deleted.
                // Same for nodes inside blokcks, delete them at 0.
                for (int j = 1; j < BlockState.StateList.Count; j++)
                {
                    IWriteableRemoveNodeOperation OperationNode = CreateRemoveNodeOperation(inner.Owner.Node, inner.PropertyName, firstBlockIndex, 0, HandlerRedoNode, HandlerUndoNode, isNested: true);
                    OperationList.Add(OperationNode);
                }

                IWriteableRemoveBlockOperation OperationBlock = CreateRemoveBlockOperation(inner.Owner.Node, inner.PropertyName, firstBlockIndex, HandlerRedoBlock, HandlerUndoBlock, isNested: true);
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

                INode Node = inner.Owner.Node;
                string PropertyName = inner.PropertyName;
                Debug.Assert(Node != null);

                Result = true;

                Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(inner.Owner.Node.GetType());
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

            IWriteableBlockState BlockState = inner.BlockStateList[blockIndex];
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

            IWriteableBlockState BlockState = inner.BlockStateList[blockIndex];
            Debug.Assert(firstNodeIndex >= 0 && firstNodeIndex < BlockState.StateList.Count);
            Debug.Assert(lastNodeIndex >= 0 && lastNodeIndex <= BlockState.StateList.Count);
            Debug.Assert(firstNodeIndex <= lastNodeIndex);

            int DeletedCount = lastNodeIndex - firstNodeIndex;

            Action<IWriteableOperation> HandlerRedoRemoveNode = (IWriteableOperation operation) => RedoRemoveNode(operation);
            Action<IWriteableOperation> HandlerUndoRemoveNode = (IWriteableOperation operation) => UndoRemoveNode(operation);
            Action<IWriteableOperation> HandlerRedoRemoveBlock = (IWriteableOperation operation) => RedoRemoveBlock(operation);
            Action<IWriteableOperation> HandlerUndoRemoveBlock = (IWriteableOperation operation) => UndoRemoveBlock(operation);

            IWriteableOperationList OperationList = CreateOperationList();

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
        public virtual void RemoveNodeRange(IWriteableListInner inner, int firstNodeIndex, int lastNodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(firstNodeIndex >= 0 && firstNodeIndex < inner.StateList.Count);
            Debug.Assert(lastNodeIndex >= 0 && lastNodeIndex <= inner.StateList.Count);
            Debug.Assert(firstNodeIndex <= lastNodeIndex);

            Action<IWriteableOperation> HandlerRedoNode = (IWriteableOperation operation) => RedoRemoveNode(operation);
            Action<IWriteableOperation> HandlerUndoNode = (IWriteableOperation operation) => UndoRemoveNode(operation);

            IWriteableOperationList OperationList = CreateOperationList();

            for (int i = firstNodeIndex; i < lastNodeIndex; i++)
            {
                IWriteableRemoveNodeOperation OperationNode = CreateRemoveNodeOperation(inner.Owner.Node, inner.PropertyName, -1, firstNodeIndex, HandlerRedoNode, HandlerUndoNode, isNested: true);
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

        /// <summary>
        /// Assign the optional node.
        /// </summary>
        /// <param name="nodeIndex">Index of the optional node.</param>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already assigned.</param>
        public virtual void Assign(IWriteableBrowsingOptionalNodeIndex nodeIndex, out bool isChanged)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(StateTable.ContainsKey(nodeIndex));

            IWriteableOptionalNodeState State = StateTable[nodeIndex] as IWriteableOptionalNodeState;
            Debug.Assert(State != null);

            IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> Inner = State.ParentInner as IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex>;
            Debug.Assert(Inner != null);

            if (!Inner.IsAssigned)
            {
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoAssign(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoAssign(operation);
                IWriteableAssignmentOperation Operation = CreateAssignmentOperation(Inner.Owner.Node, Inner.PropertyName, HandlerRedo, HandlerUndo, isNested: false);

                Operation.Redo();
                SetLastOperation(Operation);
                CheckInvariant();

                isChanged = true;
            }
            else
                isChanged = false;
        }

        private protected virtual void RedoAssign(IWriteableOperation operation)
        {
            IWriteableAssignmentOperation AssignmentOperation = (IWriteableAssignmentOperation)operation;
            ExecuteAssign(AssignmentOperation);
        }

        private protected virtual void ExecuteAssign(IWriteableAssignmentOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex>;

            Inner.Assign(operation);

            Stats.AssignedOptionalNodeCount++;

            NotifyStateAssigned(operation);
        }

        private protected virtual void UndoAssign(IWriteableOperation operation)
        {
            IWriteableAssignmentOperation AssignmentOperation = (IWriteableAssignmentOperation)operation;
            AssignmentOperation = AssignmentOperation.ToInverseAssignment();

            ExecuteUnassign(AssignmentOperation);
        }

        /// <summary>
        /// Unassign the optional node.
        /// </summary>
        /// <param name="nodeIndex">Index of the optional node.</param>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already not assigned.</param>
        public virtual void Unassign(IWriteableBrowsingOptionalNodeIndex nodeIndex, out bool isChanged)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(StateTable.ContainsKey(nodeIndex));

            IWriteableOptionalNodeState State = StateTable[nodeIndex] as IWriteableOptionalNodeState;
            Debug.Assert(State != null);

            IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> Inner = State.ParentInner as IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex>;
            Debug.Assert(Inner != null);

            if (Inner.IsAssigned)
            {
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoUnassign(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoUnassign(operation);
                IWriteableAssignmentOperation Operation = CreateAssignmentOperation(Inner.Owner.Node, Inner.PropertyName, HandlerRedo, HandlerUndo, isNested: false);

                Operation.Redo();
                SetLastOperation(Operation);
                CheckInvariant();

                isChanged = true;
            }
            else
                isChanged = false;
        }

        private protected virtual void RedoUnassign(IWriteableOperation operation)
        {
            IWriteableAssignmentOperation AssignmentOperation = (IWriteableAssignmentOperation)operation;
            ExecuteUnassign(AssignmentOperation);
        }

        private protected virtual void ExecuteUnassign(IWriteableAssignmentOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex>;

            Inner.Unassign(operation);

            Stats.AssignedOptionalNodeCount--;

            NotifyStateUnassigned(operation);
        }

        private protected virtual void UndoUnassign(IWriteableOperation operation)
        {
            IWriteableAssignmentOperation AssignmentOperation = (IWriteableAssignmentOperation)operation;
            AssignmentOperation = AssignmentOperation.ToInverseAssignment();

            ExecuteAssign(AssignmentOperation);
        }

        /// <summary>
        /// Changes the replication state of a block.
        /// </summary>
        /// <param name="inner">The inner where the blok is changed.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="replication">New replication value.</param>
        public virtual void ChangeReplication(IWriteableBlockListInner inner, int blockIndex, ReplicationStatus replication)
        {
            Debug.Assert(inner != null);
            Debug.Assert(blockIndex >= 0 && blockIndex < inner.BlockStateList.Count);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoChangeReplication(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoChangeReplication(operation);
            IWriteableChangeBlockOperation Operation = CreateChangeBlockOperation(inner.Owner.Node, inner.PropertyName, blockIndex, replication, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        private protected virtual void RedoChangeReplication(IWriteableOperation operation)
        {
            IWriteableChangeBlockOperation ChangeBlockOperation = (IWriteableChangeBlockOperation)operation;
            ExecuteChangeReplication(ChangeBlockOperation);
        }

        private protected virtual void UndoChangeReplication(IWriteableOperation operation)
        {
            IWriteableChangeBlockOperation ChangeBlockOperation = (IWriteableChangeBlockOperation)operation;
            ChangeBlockOperation = ChangeBlockOperation.ToInverseChange();

            ExecuteChangeReplication(ChangeBlockOperation);
        }

        private protected virtual void ExecuteChangeReplication(IWriteableChangeBlockOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;

            Inner.ChangeReplication(operation);

            NotifyBlockStateChanged(operation);
        }

        /// <summary>
        /// Changes the value of an enum or boolean.
        /// If the value exceeds allowed values, it is rounded to fit.
        /// </summary>
        /// <param name="nodeIndex">Index of the state with the enum to change.</param>
        /// <param name="propertyName">Name of the property to change.</param>
        /// <param name="value">The new value.</param>
        public virtual void ChangeDiscreteValue(IWriteableIndex nodeIndex, string propertyName, int value)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(StateTable.ContainsKey(nodeIndex));
            Debug.Assert(value >= 0);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoChangeDiscreteValue(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoChangeDiscreteValue(operation);
            IWriteableNodeState State = StateTable[nodeIndex];
            IWriteableChangeDiscreteValueOperation Operation = CreateChangeDiscreteValueOperation(State.Node, propertyName, value, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        private protected virtual void RedoChangeDiscreteValue(IWriteableOperation operation)
        {
            IWriteableChangeDiscreteValueOperation ChangeDiscreteValueOperation = (IWriteableChangeDiscreteValueOperation)operation;
            ExecuteChangeDiscreteValue(ChangeDiscreteValueOperation);
        }

        private protected virtual void UndoChangeDiscreteValue(IWriteableOperation operation)
        {
            IWriteableChangeDiscreteValueOperation ChangeDiscreteValueOperation = (IWriteableChangeDiscreteValueOperation)operation;
            ChangeDiscreteValueOperation = ChangeDiscreteValueOperation.ToInverseChange();

            ExecuteChangeDiscreteValue(ChangeDiscreteValueOperation);
        }

        private protected virtual void ExecuteChangeDiscreteValue(IWriteableChangeDiscreteValueOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            int NewValue = operation.NewValue;

            IWriteableNodeState State = (IWriteableNodeState)GetState(ParentNode);
            Debug.Assert(State != null);
            Debug.Assert(State.ValuePropertyTypeTable.ContainsKey(PropertyName));
            Debug.Assert(State.ValuePropertyTypeTable[PropertyName] == Constants.ValuePropertyType.Boolean || State.ValuePropertyTypeTable[PropertyName] == Constants.ValuePropertyType.Enum);

            int OldValue = NodeTreeHelper.GetEnumValue(State.Node, PropertyName);

            NodeTreeHelper.GetEnumRange(State.Node.GetType(), PropertyName, out int Min, out int Max);

            Debug.Assert(NewValue >= Min && NewValue <= Max);

            NodeTreeHelper.SetEnumValue(State.Node, PropertyName, NewValue);

            operation.Update(State, OldValue);

            NotifyDiscreteValueChanged(operation);
        }

        /// <summary>
        /// Changes the value of a text.
        /// </summary>
        /// <param name="nodeIndex">Index of the state with the string to change.</param>
        /// <param name="propertyName">Name of the property to change.</param>
        /// <param name="text">The new text.</param>
        public virtual void ChangeText(IWriteableIndex nodeIndex, string propertyName, string text)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(StateTable.ContainsKey(nodeIndex));
            Debug.Assert(text != null);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoChangeText(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoChangeText(operation);
            IWriteableNodeState State = StateTable[nodeIndex];
            IWriteableChangeTextOperation Operation = CreateChangeTextOperation(State.Node, propertyName, text, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        private protected virtual void RedoChangeText(IWriteableOperation operation)
        {
            IWriteableChangeTextOperation ChangeTextOperation = (IWriteableChangeTextOperation)operation;
            ExecuteChangeText(ChangeTextOperation);
        }

        private protected virtual void UndoChangeText(IWriteableOperation operation)
        {
            IWriteableChangeTextOperation ChangeTextOperation = (IWriteableChangeTextOperation)operation;
            ChangeTextOperation = ChangeTextOperation.ToInverseChange();

            ExecuteChangeText(ChangeTextOperation);
        }

        private protected virtual void ExecuteChangeText(IWriteableChangeTextOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            string NewText = operation.NewText;

            IWriteableNodeState State = (IWriteableNodeState)GetState(ParentNode);
            Debug.Assert(State != null);
            Debug.Assert(State.ValuePropertyTypeTable.ContainsKey(PropertyName));
            Debug.Assert(State.ValuePropertyTypeTable[PropertyName] == Constants.ValuePropertyType.String);

            string OldText = NodeTreeHelper.GetString(State.Node, PropertyName);
            Debug.Assert(OldText != null);

            NodeTreeHelper.SetString(State.Node, PropertyName, NewText);

            operation.Update(State, OldText);

            NotifyTextChanged(operation);
        }

        /// <summary>
        /// Changes the value of a text.
        /// </summary>
        /// <param name="nodeIndex">Index of the state with the comment to change.</param>
        /// <param name="text">The new comment.</param>
        public virtual void ChangeComment(IWriteableIndex nodeIndex, string text)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(StateTable.ContainsKey(nodeIndex));
            Debug.Assert(text != null);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoChangeComment(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoChangeComment(operation);
            IWriteableNodeState State = StateTable[nodeIndex];
            IWriteableChangeCommentOperation Operation = CreateChangeCommentOperation(State.Node, text, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        private protected virtual void RedoChangeComment(IWriteableOperation operation)
        {
            IWriteableChangeCommentOperation ChangeCommentOperation = (IWriteableChangeCommentOperation)operation;
            ExecuteChangeComment(ChangeCommentOperation);
        }

        private protected virtual void UndoChangeComment(IWriteableOperation operation)
        {
            IWriteableChangeCommentOperation ChangeCommentOperation = (IWriteableChangeCommentOperation)operation;
            ChangeCommentOperation = ChangeCommentOperation.ToInverseChange();

            ExecuteChangeComment(ChangeCommentOperation);
        }

        private protected virtual void ExecuteChangeComment(IWriteableChangeCommentOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string NewText = operation.NewText;

            IWriteableNodeState State = (IWriteableNodeState)GetState(ParentNode);
            Debug.Assert(State != null);

            string OldText = NodeTreeHelper.GetCommentText(State.Node);
            Debug.Assert(OldText != null);

            CommentHelper.Set(State.Node.Documentation, NewText);

            operation.Update(State, OldText);

            NotifyCommentChanged(operation);
        }

        /// <summary>
        /// Checks whether a block can be split at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block would be split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        public virtual bool IsSplittable(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            return inner.IsSplittable(nodeIndex);
        }

        /// <summary>
        /// Splits a block in two at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block is split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        public virtual void SplitBlock(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);
            Debug.Assert(inner.IsSplittable(nodeIndex));

            IWriteableBlockState BlockState = inner.BlockStateList[nodeIndex.BlockIndex];
            ReplicationStatus Replication = BlockState.ChildBlock.Replication;
            IPattern NewPatternNode = NodeHelper.CreateSimplePattern(BlockState.ChildBlock.ReplicationPattern.Text);
            IIdentifier NewSourceNode = NodeHelper.CreateSimpleIdentifier(BlockState.ChildBlock.SourceIdentifier.Text);
            IBlock NewBlock = NodeTreeHelperBlockList.CreateBlock(inner.Owner.Node, inner.PropertyName, Replication, NewPatternNode, NewSourceNode);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoSplitBlock(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoSplitBlock(operation);
            IWriteableSplitBlockOperation Operation = CreateSplitBlockOperation(inner.Owner.Node, inner.PropertyName, nodeIndex.BlockIndex, nodeIndex.Index, NewBlock, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        private protected virtual void RedoSplitBlock(IWriteableOperation operation)
        {
            IWriteableSplitBlockOperation SplitBlockOperation = (IWriteableSplitBlockOperation)operation;
            ExecuteSplitBlock(SplitBlockOperation);
        }

        private protected virtual void ExecuteSplitBlock(IWriteableSplitBlockOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;

            IWriteableBlockState OldBlockState = Inner.BlockStateList[operation.BlockIndex];
            Debug.Assert(operation.Index < OldBlockState.StateList.Count);

            int OldNodeCount = OldBlockState.StateList.Count;

            Inner.SplitBlock(operation);
            Stats.BlockCount++;

            IWriteableBlockState NewBlockState = operation.BlockState;

            Debug.Assert(OldBlockState.StateList.Count + NewBlockState.StateList.Count == OldNodeCount);
            Debug.Assert(NewBlockState.StateList.Count > 0);

            IReadOnlyBrowsingPatternIndex PatternIndex = NewBlockState.PatternIndex;
            IReadOnlyPatternState PatternState = NewBlockState.PatternState;
            AddState(PatternIndex, PatternState);
            Stats.PlaceholderNodeCount++;

            IReadOnlyBrowsingSourceIndex SourceIndex = NewBlockState.SourceIndex;
            IReadOnlySourceState SourceState = NewBlockState.SourceState;
            AddState(SourceIndex, SourceState);
            Stats.PlaceholderNodeCount++;

            NotifyBlockSplit(operation);
        }

        private protected virtual void UndoSplitBlock(IWriteableOperation operation)
        {
            IWriteableSplitBlockOperation SplitBlockOperation = (IWriteableSplitBlockOperation)operation;
            IWriteableMergeBlocksOperation MergeBlocksOperation = SplitBlockOperation.ToMergeBlocksOperation();

            ExecuteMergeBlocks(MergeBlocksOperation);
        }

        /// <summary>
        /// Checks whether a block can be merged at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block would be split.</param>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        public virtual bool IsMergeable(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            return inner.IsMergeable(nodeIndex);
        }

        /// <summary>
        /// Merges two blocks at the given index.
        /// </summary>
        /// <param name="inner">The inner where blocks are merged.</param>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        public virtual void MergeBlocks(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);
            Debug.Assert(inner.IsMergeable(nodeIndex));

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoMergeBlocks(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoMergeBlocks(operation);
            IWriteableMergeBlocksOperation Operation = CreateMergeBlocksOperation(inner.Owner.Node, inner.PropertyName, nodeIndex.BlockIndex, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        private protected virtual void RedoMergeBlocks(IWriteableOperation operation)
        {
            IWriteableMergeBlocksOperation MergeBlocksOperation = (IWriteableMergeBlocksOperation)operation;
            ExecuteMergeBlocks(MergeBlocksOperation);
        }

        private protected virtual void ExecuteMergeBlocks(IWriteableMergeBlocksOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;

            int BlockIndex = operation.BlockIndex;
            IWriteableBlockState FirstBlockState = Inner.BlockStateList[BlockIndex - 1];
            IWriteableBlockState SecondBlockState = Inner.BlockStateList[BlockIndex];

            IReadOnlyBrowsingSourceIndex SourceIndex = FirstBlockState.SourceIndex;
            RemoveState(SourceIndex);
            Stats.PlaceholderNodeCount--;

            IReadOnlyBrowsingPatternIndex PatternIndex = FirstBlockState.PatternIndex;
            RemoveState(PatternIndex);
            Stats.PlaceholderNodeCount--;

            int OldNodeCount = FirstBlockState.StateList.Count + SecondBlockState.StateList.Count;
            int FirstNodeIndex = FirstBlockState.StateList.Count;

            Inner.MergeBlocks(operation);
            Stats.BlockCount--;

            IWriteableBlockState BlockState = Inner.BlockStateList[BlockIndex - 1];

            Debug.Assert(BlockState.StateList.Count == OldNodeCount);
            Debug.Assert(FirstNodeIndex < BlockState.StateList.Count);

            NotifyBlocksMerged(operation);
        }

        private protected virtual void UndoMergeBlocks(IWriteableOperation operation)
        {
            IWriteableMergeBlocksOperation MergeBlocksOperation = (IWriteableMergeBlocksOperation)operation;
            IWriteableSplitBlockOperation SplitBlockOperation = MergeBlocksOperation.ToSplitBlockOperation();

            ExecuteSplitBlock(SplitBlockOperation);
        }

        /// <summary>
        /// Checks whether a node can be moved in a list.
        /// </summary>
        /// <param name="inner">The inner where the node is.</param>
        /// <param name="nodeIndex">Index of the node that would be moved.</param>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        public virtual bool IsMoveable(IWriteableCollectionInner inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IWriteableNodeState State = StateTable[nodeIndex];
            Debug.Assert(State != null);

            bool Result = inner.IsMoveable(nodeIndex, direction);

            return Result;
        }

        /// <summary>
        /// Moves a node around in a list or block list. In a block list, the node stays in same block.
        /// </summary>
        /// <param name="inner">The inner for the list or block list in which the node is moved.</param>
        /// <param name="nodeIndex">Index for the moved node.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        public virtual void Move(IWriteableCollectionInner inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IWriteableNodeState State = StateTable[nodeIndex];
            Debug.Assert(State != null);

            IndexToPositionAndNode(nodeIndex, out int BlockIndex, out int Index, out INode Node);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoMove(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoMove(operation);
            IWriteableMoveNodeOperation Operation = CreateMoveNodeOperation(inner.Owner.Node, inner.PropertyName, BlockIndex, Index, direction, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        private protected virtual void RedoMove(IWriteableOperation operation)
        {
            IWriteableMoveNodeOperation MoveNodeOperation = (IWriteableMoveNodeOperation)operation;
            ExecuteMove(MoveNodeOperation);
        }

        private protected virtual void UndoMove(IWriteableOperation operation)
        {
            IWriteableMoveNodeOperation MoveNodeOperation = (IWriteableMoveNodeOperation)operation;
            MoveNodeOperation = MoveNodeOperation.ToInverseMove();

            ExecuteMove(MoveNodeOperation);
        }

        private protected virtual void ExecuteMove(IWriteableMoveNodeOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex>;

            Inner.Move(operation);
            NotifyStateMoved(operation);
        }

        /// <summary>
        /// Checks whether a block can be moved in a block list.
        /// </summary>
        /// <param name="inner">The inner where the block is.</param>
        /// <param name="blockIndex">Index of the block that would be moved.</param>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        public virtual bool IsBlockMoveable(IWriteableBlockListInner inner, int blockIndex, int direction)
        {
            Debug.Assert(inner != null);
            Debug.Assert(blockIndex >= 0 && blockIndex < inner.BlockStateList.Count);

            return blockIndex + direction >= 0 && blockIndex + direction < inner.BlockStateList.Count;
        }

        /// <summary>
        /// Moves a block around in a block list.
        /// </summary>
        /// <param name="inner">The inner where the block is moved.</param>
        /// <param name="blockIndex">Index of the block to move.</param>
        /// <param name="direction">The change in position, relative to the current block position.</param>
        public virtual void MoveBlock(IWriteableBlockListInner inner, int blockIndex, int direction)
        {
            Debug.Assert(inner != null);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoMoveBlock(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoMoveBlock(operation);
            IWriteableMoveBlockOperation Operation = CreateMoveBlockOperation(inner.Owner.Node, inner.PropertyName, blockIndex, direction, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        private protected virtual void RedoMoveBlock(IWriteableOperation operation)
        {
            IWriteableMoveBlockOperation MoveBlockOperation = (IWriteableMoveBlockOperation)operation;
            ExecuteMoveBlock(MoveBlockOperation);
        }

        private protected virtual void UndoMoveBlock(IWriteableOperation operation)
        {
            IWriteableMoveBlockOperation MoveBlockOperation = (IWriteableMoveBlockOperation)operation;
            MoveBlockOperation = MoveBlockOperation.ToInverseMoveBlock();

            ExecuteMoveBlock(MoveBlockOperation);
        }

        private protected virtual void ExecuteMoveBlock(IWriteableMoveBlockOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;

            IWriteableBlockState BlockState = Inner.BlockStateList[operation.BlockIndex];
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockState.StateList.Count > 0);

            IWriteableNodeState State = BlockState.StateList[0];
            Debug.Assert(State != null);

            IWriteableBrowsingExistingBlockNodeIndex NodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
            Debug.Assert(NodeIndex != null);

            Inner.MoveBlock(operation);

            NotifyBlockStateMoved(operation);
        }

        /// <summary>
        /// Expands an existing node. In the node:
        /// * All optional children are assigned if they aren't
        /// * If the node is a feature call, with no arguments, an empty argument is inserted.
        /// </summary>
        /// <param name="expandedIndex">Index of the expanded node.</param>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already expanded.</param>
        public virtual void Expand(IWriteableNodeIndex expandedIndex, out bool isChanged)
        {
            Debug.Assert(expandedIndex != null);
            Debug.Assert(StateTable.ContainsKey(expandedIndex));
            Debug.Assert(StateTable[expandedIndex] is IWriteablePlaceholderNodeState);

            IWriteableOperationList OperationList = CreateOperationList();

            DebugObjects.AddReference(OperationList);

            Expand(expandedIndex, OperationList);

            if (OperationList.Count > 0)
            {
                IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, null);

                SetLastOperation(OperationGroup);
                CheckInvariant();

                isChanged = true;
            }
            else
                isChanged = false;
        }

        private protected virtual void Expand(IWriteableNodeIndex expandedIndex, IWriteableOperationList operationList)
        {
            IWriteablePlaceholderNodeState State = StateTable[expandedIndex] as IWriteablePlaceholderNodeState;
            State = FindBestExpandReduceState(State);
            Debug.Assert(State != null);

            IWriteableInnerReadOnlyDictionary<string> InnerTable = State.InnerTable;

            foreach (KeyValuePair<string, IWriteableInner> Entry in InnerTable)
            {
                if (Entry.Value is IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> AsOptionalInner)
                    ExpandOptional(AsOptionalInner, operationList);
                else if (Entry.Value is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner)
                    ExpandBlockList(AsBlockListInner, operationList);
            }
        }

        private protected virtual IWriteablePlaceholderNodeState FindBestExpandReduceState(IWriteablePlaceholderNodeState state)
        {
            Debug.Assert(state != null);

            while (state.InnerTable.Count == 0 && state.ParentState is IWriteablePlaceholderNodeState AsPlaceholderNodeState)
                state = AsPlaceholderNodeState;

            return state;
        }

        /// <summary>
        /// Expands the optional node.
        /// * If assigned, does nothing.
        /// * If it has an item, assign it.
        /// * Otherwise, assign the item to a default node.
        /// </summary>
        private protected virtual void ExpandOptional(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> optionalInner, IWriteableOperationList operationList)
        {
            if (optionalInner.IsAssigned)
                return;

            IWriteableBrowsingOptionalNodeIndex ParentIndex = optionalInner.ChildState.ParentIndex;
            if (ParentIndex.Optional.HasItem)
            {
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoAssign(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoAssign(operation);
                IWriteableAssignmentOperation Operation = CreateAssignmentOperation(optionalInner.Owner.Node, optionalInner.PropertyName, HandlerRedo, HandlerUndo, isNested: false);

                Operation.Redo();

                operationList.Add(Operation);
            }
            else
            {
                INode NewNode = NodeHelper.CreateDefaultFromInterface(optionalInner.InterfaceType);
                Debug.Assert(NewNode != null);

                IWriteableInsertionOptionalNodeIndex NewOptionalNodeIndex = CreateNewOptionalNodeIndex(optionalInner.Owner.Node, optionalInner.PropertyName, NewNode);

                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoReplace(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoReplace(operation);
                IWriteableReplaceOperation Operation = CreateReplaceOperation(optionalInner.Owner.Node, optionalInner.PropertyName, -1, -1, NewNode, HandlerRedo, HandlerUndo, isNested: false);

                Operation.Redo();

                operationList.Add(Operation);
            }
        }

        /// <summary>
        /// Expands the block list.
        /// * Only expand block list of arguments
        /// * Only expand if the list is empty. In that case, add a single default argument.
        /// </summary>
        private protected virtual void ExpandBlockList(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> blockListInner, IWriteableOperationList operationList)
        {
            if (!blockListInner.IsEmpty)
                return;

            if (!NodeHelper.IsCollectionWithExpand(blockListInner.Owner.Node, blockListInner.PropertyName))
                return;

            INode NewItem = NodeHelper.CreateDefaultFromInterface(blockListInner.InterfaceType);
            IPattern NewPattern = NodeHelper.CreateEmptyPattern();
            IIdentifier NewSource = NodeHelper.CreateEmptyIdentifier();
            IBlock NewBlock = NodeTreeHelperBlockList.CreateBlock(blockListInner.Owner.Node, blockListInner.PropertyName, ReplicationStatus.Normal, NewPattern, NewSource);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoExpandBlockList(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoExpandBlockList(operation);
            IWriteableExpandArgumentOperation Operation = CreateExpandArgumentOperation(blockListInner.Owner.Node, blockListInner.PropertyName, NewBlock, NewItem, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();

            operationList.Add(Operation);
        }

        private protected virtual void RedoExpandBlockList(IWriteableOperation operation)
        {
            IWriteableExpandArgumentOperation ExpandArgumentOperation = (IWriteableExpandArgumentOperation)operation;
            ExecuteInsertNewBlock(ExpandArgumentOperation);
        }

        private protected virtual void UndoExpandBlockList(IWriteableOperation operation)
        {
            IWriteableExpandArgumentOperation ExpandArgumentOperation = (IWriteableExpandArgumentOperation)operation;
            IWriteableRemoveBlockOperation RemoveBlockOperation = ExpandArgumentOperation.ToRemoveBlockOperation();

            ExecuteRemoveBlock(RemoveBlockOperation);
        }

        /// <summary>
        /// Reduces an existing node. Opposite of Expand.
        /// </summary>
        /// <param name="reducedIndex">Index of the reduced node.</param>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already reduced.</param>
        public virtual void Reduce(IWriteableNodeIndex reducedIndex, out bool isChanged)
        {
            Debug.Assert(reducedIndex != null);
            Debug.Assert(StateTable.ContainsKey(reducedIndex));
            Debug.Assert(StateTable[reducedIndex] is IWriteablePlaceholderNodeState);

            IWriteableOperationList OperationList = CreateOperationList();

            Reduce(reducedIndex, OperationList, isNested: false);

            if (OperationList.Count > 0)
            {
                IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, null);

                SetLastOperation(OperationGroup);
                CheckInvariant();

                isChanged = true;
            }
            else
                isChanged = false;
        }

        private protected virtual void Reduce(IWriteableNodeIndex reducedIndex, IWriteableOperationList operationList, bool isNested)
        {
            IWriteablePlaceholderNodeState State = StateTable[reducedIndex] as IWriteablePlaceholderNodeState;
            State = FindBestExpandReduceState(State);
            Debug.Assert(State != null);

            IWriteableInnerReadOnlyDictionary<string> InnerTable = State.InnerTable;

            foreach (KeyValuePair<string, IWriteableInner> Entry in InnerTable)
            {
                if (Entry.Value is IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> AsOptionalInner)
                    ReduceOptional(AsOptionalInner, operationList, isNested);
                else if (Entry.Value is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner)
                    ReduceBlockList(AsBlockListInner, operationList, isNested);
            }
        }

        /// <summary>
        /// Reduces the optional node.
        /// </summary>
        private protected virtual void ReduceOptional(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> optionalInner, IWriteableOperationList operationList, bool isNested)
        {
            if (optionalInner.IsAssigned && NodeHelper.IsOptionalAssignedToDefault(optionalInner.ChildState.Optional))
            {
                IWriteableBrowsingOptionalNodeIndex ParentIndex = optionalInner.ChildState.ParentIndex;

                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoUnassign(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoUnassign(operation);
                IWriteableAssignmentOperation Operation = CreateAssignmentOperation(optionalInner.Owner.Node, optionalInner.PropertyName, HandlerRedo, HandlerUndo, isNested);

                Operation.Redo();

                operationList.Add(Operation);
            }
        }

        /// <summary>
        /// Reduces the block list.
        /// </summary>
        private protected virtual void ReduceBlockList(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> blockListInner, IWriteableOperationList operationList, bool isNested)
        {
            if (!blockListInner.IsSingle)
                return;

            if (NodeHelper.IsCollectionNeverEmpty(blockListInner.Owner.Node, blockListInner.PropertyName))
                return;

            if (!NodeHelper.IsCollectionWithExpand(blockListInner.Owner.Node, blockListInner.PropertyName))
                return;

            Debug.Assert(blockListInner.BlockStateList.Count == 1);
            Debug.Assert(blockListInner.BlockStateList[0].StateList.Count == 1);
            IWriteableNodeState FirstState = blockListInner.BlockStateList[0].StateList[0];

            if (!NodeHelper.IsDefaultNode(FirstState.Node))
                return;

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRemoveBlock(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoRemoveBlock(operation);
            IWriteableRemoveBlockOperation Operation = CreateRemoveBlockOperation(blockListInner.Owner.Node, blockListInner.PropertyName, 0, HandlerRedo, HandlerUndo, isNested);

            Operation.Redo();

            operationList.Add(Operation);
        }

        /// <summary>
        /// Reduces all expanded nodes, and clear all unassigned optional nodes.
        /// </summary>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already canonic.</param>
        public virtual void Canonicalize(out bool isChanged)
        {
            IWriteableOperationList OperationList = CreateOperationList();
            Canonicalize(RootState, OperationList);

            if (OperationList.Count > 0)
            {
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRefresh(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                IWriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedo, HandlerUndo, isNested: false);

                RefreshOperation.Redo();

                IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);

                SetLastOperation(OperationGroup);
                CheckInvariant();

                isChanged = true;
            }
            else
                isChanged = false;
        }

        private protected virtual void Canonicalize(IWriteableNodeState state, IWriteableOperationList operationList)
        {
            IWriteableNodeIndex NodeIndex = state.ParentIndex as IWriteableNodeIndex;
            Debug.Assert(NodeIndex != null);

            CanonicalizeChildren(state, operationList);

            Reduce(NodeIndex, operationList, isNested: state != RootState);
        }

        private protected virtual void CanonicalizeChildren(IWriteableNodeState state, IWriteableOperationList operationList)
        {
            List<IWriteableNodeState> ChildStateList = new List<IWriteableNodeState>();
            foreach (KeyValuePair<string, IWriteableInner> Entry in state.InnerTable)
            {
                switch (Entry.Value)
                {
                    case IWriteablePlaceholderInner<IWriteableBrowsingPlaceholderNodeIndex> AsPlaceholderInner:
                        ChildStateList.Add(AsPlaceholderInner.ChildState);
                        break;

                    case IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> AsOptionalInner:
                        if (AsOptionalInner.IsAssigned)
                            CanonicalizeChildren(AsOptionalInner.ChildState, operationList);
                        break;

                    case IWriteableListInner<IWriteableBrowsingListNodeIndex> AsListlInner:
                        foreach (IWriteablePlaceholderNodeState ChildState in AsListlInner.StateList)
                            ChildStateList.Add(ChildState);
                        break;

                    case IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListlInner:
                        foreach (IWriteableBlockState BlockState in AsBlockListlInner.BlockStateList)
                            foreach (IWriteablePlaceholderNodeState ChildState in BlockState.StateList)
                                ChildStateList.Add(ChildState);
                        break;
                }
            }

            foreach (IWriteableNodeState ChildState in ChildStateList)
                Canonicalize(ChildState, operationList);
        }

        private protected virtual void RedoRefresh(IWriteableOperation operation)
        {
            IWriteableGenericRefreshOperation GenericRefreshOperation = (IWriteableGenericRefreshOperation)operation;
            ExecuteRefresh(GenericRefreshOperation);
        }

        private protected virtual void ExecuteRefresh(IWriteableGenericRefreshOperation operation)
        {
            NotifyGenericRefresh(operation);
        }

        /// <summary>
        /// Undo the last operation.
        /// </summary>
        public virtual void Undo()
        {
            Debug.Assert(CanUndo);

            RedoIndex--;
            IWriteableOperationGroup OperationGroup = OperationStack[RedoIndex];
            OperationGroup.Undo();

            CheckInvariant();
        }

        /// <summary>
        /// Redo the last operation undone.
        /// </summary>
        public virtual void Redo()
        {
            Debug.Assert(CanRedo);

            IWriteableOperationGroup OperationGroup = OperationStack[RedoIndex];
            OperationGroup.Redo();
            RedoIndex++;

            CheckInvariant();
        }

        /// <summary>
        /// Split an identifier with replace and insert indexes.
        /// </summary>
        /// <param name="inner">The inner where the node is replaced.</param>
        /// <param name="replaceIndex">Index for the replace operation.</param>
        /// <param name="insertIndex">Index for the insert operation.</param>
        /// <param name="firstIndex">Index of the replacing node upon return.</param>
        /// <param name="secondIndex">Index of the inserted node upon return.</param>
        public virtual void SplitIdentifier(IWriteableListInner inner, IWriteableInsertionListNodeIndex replaceIndex, IWriteableInsertionListNodeIndex insertIndex, out IWriteableBrowsingListNodeIndex firstIndex, out IWriteableBrowsingListNodeIndex secondIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(replaceIndex != null);
            Debug.Assert(insertIndex != null);
            IWriteableNodeState Owner = inner.Owner;
            IWriteableIndex ParentIndex = Owner.ParentIndex;
            Debug.Assert(Contains(ParentIndex));
            Debug.Assert(IndexToState(ParentIndex) == Owner);
            IWriteableInnerReadOnlyDictionary<string> InnerTable = Owner.InnerTable;
            Debug.Assert(InnerTable.ContainsKey(inner.PropertyName));
            Debug.Assert(InnerTable[inner.PropertyName] == inner);

            int Index = replaceIndex.Index;
            Debug.Assert(insertIndex.Index == Index + 1);
            INode ReplacingNode = replaceIndex.Node;
            INode InsertedNode = insertIndex.Node;

            Action<IWriteableOperation> HandlerRedoReplace = (IWriteableOperation operation) => RedoReplace(operation);
            Action<IWriteableOperation> HandlerUndoReplace = (IWriteableOperation operation) => UndoReplace(operation);
            IWriteableReplaceOperation ReplaceOperation = CreateReplaceOperation(inner.Owner.Node, inner.PropertyName, -1, Index, ReplacingNode, HandlerRedoReplace, HandlerUndoReplace, isNested: true);

            Action<IWriteableOperation> HandlerRedoInsert = (IWriteableOperation operation) => RedoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerUndoInsert = (IWriteableOperation operation) => UndoInsertNewNode(operation);
            IWriteableInsertNodeOperation InsertOperation = CreateInsertNodeOperation(inner.Owner.Node, inner.PropertyName, -1, Index + 1, InsertedNode, HandlerRedoInsert, HandlerUndoInsert, isNested: true);

            ReplaceOperation.Redo();
            InsertOperation.Redo();

            Action<IWriteableOperation> HandlerRedoRefresh = (IWriteableOperation operation) => RedoRefresh(operation);
            Action<IWriteableOperation> HandlerUndoRefresh = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
            IWriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedoRefresh, HandlerUndoRefresh, isNested: false);

            RefreshOperation.Redo();

            IWriteableOperationList OperationList = CreateOperationList();
            OperationList.Add(ReplaceOperation);
            OperationList.Add(InsertOperation);
            IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
            IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);
            SetLastOperation(OperationGroup);

            CheckInvariant();

            firstIndex = ReplaceOperation.NewBrowsingIndex as IWriteableBrowsingListNodeIndex;
            Debug.Assert(firstIndex != null);

            secondIndex = InsertOperation.BrowsingIndex as IWriteableBrowsingListNodeIndex;
            Debug.Assert(secondIndex != null);
        }

        private protected virtual void IndexToPositionAndNode(IWriteableIndex nodeIndex, out int blockIndex, out int index, out INode node)
        {
            blockIndex = -1;
            index = -1;
            node = null;
            bool IsHandled = false;

            switch (nodeIndex)
            {
                case IWriteableInsertionPlaceholderNodeIndex AsPlaceholderNodeIndex:
                    node = AsPlaceholderNodeIndex.Node;
                    IsHandled = true;
                    break;

                case IWriteableInsertionOptionalNodeIndex AsOptionalNodeIndex:
                    node = AsOptionalNodeIndex.Node;
                    IsHandled = true;
                    break;

                case IWriteableInsertionOptionalClearIndex AsOptionalClearIndex:
                    IsHandled = true;
                    break;

                case IWriteableInsertionListNodeIndex AsListNodeIndex:
                    index = AsListNodeIndex.Index;
                    node = AsListNodeIndex.Node;
                    IsHandled = true;
                    break;

                case IWriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex:
                    blockIndex = AsExistingBlockNodeIndex.BlockIndex;
                    index = AsExistingBlockNodeIndex.Index;
                    node = AsExistingBlockNodeIndex.Node;
                    IsHandled = true;
                    break;

                case IWriteableBrowsingListNodeIndex AsListNodeIndex:
                    index = AsListNodeIndex.Index;
                    node = AsListNodeIndex.Node;
                    IsHandled = true;
                    break;

                case IWriteableBrowsingExistingBlockNodeIndex AsExistingBlockNodeIndex:
                    blockIndex = AsExistingBlockNodeIndex.BlockIndex;
                    index = AsExistingBlockNodeIndex.Index;
                    node = AsExistingBlockNodeIndex.Node;
                    IsHandled = true;
                    break;
            }

            Debug.Assert(IsHandled);
        }
    }
}
