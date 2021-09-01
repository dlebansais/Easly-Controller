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
        /// Inserts a new node in a list or block list.
        /// </summary>
        /// <param name="inner">The inner for the list or block list where the node is inserted.</param>
        /// <param name="insertedIndex">Index for the insertion operation.</param>
        /// <param name="nodeIndex">Index of the inserted node upon return.</param>
        public virtual void Insert(IWriteableCollectionInner inner, WriteableInsertionCollectionNodeIndex insertedIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(insertedIndex != null);
            IWriteableNodeState Owner = inner.Owner;
            IWriteableIndex ParentIndex = Owner.ParentIndex;
            Debug.Assert(Contains(ParentIndex));
            Debug.Assert(IndexToState(ParentIndex) == Owner);
            WriteableInnerReadOnlyDictionary<string> InnerTable = Owner.InnerTable;
            Debug.Assert(InnerTable.ContainsKey(inner.PropertyName));
            Debug.Assert(InnerTable[inner.PropertyName] == inner);

            bool IsHandled = false;
            nodeIndex = null;

            if (inner is WriteableBlockListInner<WriteableBrowsingBlockNodeIndex> AsBlockListInner && insertedIndex is WriteableInsertionNewBlockNodeIndex AsNewBlockIndex)
            {
                InsertNewBlock(AsBlockListInner, AsNewBlockIndex, out nodeIndex);
                IsHandled = true;
            }
            else if (inner is WriteableCollectionInner<WriteableBrowsingCollectionNodeIndex> AsCollectionInner && insertedIndex is WriteableInsertionCollectionNodeIndex AsCollectionIndex)
            {
                InsertNewNode(AsCollectionInner, AsCollectionIndex, out nodeIndex);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);
        }

        private protected virtual void InsertNewBlock(WriteableBlockListInner<WriteableBrowsingBlockNodeIndex> blockListInner, WriteableInsertionNewBlockNodeIndex newBlockIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            IBlock NewBlock = NodeTreeHelperBlockList.CreateBlock(blockListInner.Owner.Node, blockListInner.PropertyName, ReplicationStatus.Normal, newBlockIndex.PatternNode, newBlockIndex.SourceNode);

            Action<WriteableOperation> HandlerRedo = (WriteableOperation operation) => RedoInsertNewBlock(operation);
            Action<WriteableOperation> HandlerUndo = (WriteableOperation operation) => UndoInsertNewBlock(operation);
            WriteableInsertBlockOperation Operation = CreateInsertBlockOperation(blockListInner.Owner.Node, blockListInner.PropertyName, newBlockIndex.BlockIndex, NewBlock, newBlockIndex.Node, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();

            nodeIndex = Operation.BrowsingIndex;
        }

        private protected virtual void RedoInsertNewBlock(WriteableOperation operation)
        {
            WriteableInsertBlockOperation InsertBlockOperation = (WriteableInsertBlockOperation)operation;
            ExecuteInsertNewBlock(InsertBlockOperation);
        }

        private protected virtual void ExecuteInsertNewBlock(WriteableInsertBlockOperation operation)
        {
            Node ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<WriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<WriteableBrowsingBlockNodeIndex>;

            Inner.InsertNewBlock(operation);

            WriteableBrowsingExistingBlockNodeIndex BrowsingIndex = operation.BrowsingIndex;
            IWriteableBlockState BlockState = operation.BlockState;
            IWriteablePlaceholderNodeState ChildState = operation.ChildState;

            Debug.Assert(BlockState.StateList.Count == 1);
            Debug.Assert(BlockState.StateList[0] == ChildState);
            ((IWriteableBlockState<IWriteableInner<IWriteableBrowsingChildIndex>>)BlockState).InitBlockState();
            Stats.BlockCount++;

            WriteableBrowsingPatternIndex PatternIndex = BlockState.PatternIndex;
            IWriteablePatternState PatternState = BlockState.PatternState;
            AddState(PatternIndex, PatternState);
            Stats.PlaceholderNodeCount++;

            WriteableBrowsingSourceIndex SourceIndex = BlockState.SourceIndex;
            IWriteableSourceState SourceState = BlockState.SourceState;
            AddState(SourceIndex, SourceState);
            Stats.PlaceholderNodeCount++;

            AddState(BrowsingIndex, ChildState);
            Stats.PlaceholderNodeCount++;
            BuildStateTable(Inner, null, BrowsingIndex, ChildState);

            Debug.Assert(Contains(BrowsingIndex));

            NotifyBlockStateInserted(operation);
        }

        private protected virtual void UndoInsertNewBlock(WriteableOperation operation)
        {
            WriteableInsertBlockOperation InsertBlockOperation = (WriteableInsertBlockOperation)operation;
            WriteableRemoveBlockOperation RemoveBlockOperation = InsertBlockOperation.ToRemoveBlockOperation();

            ExecuteRemoveBlock(RemoveBlockOperation);
        }

        private protected virtual void InsertNewNode(WriteableCollectionInner<WriteableBrowsingCollectionNodeIndex> inner, WriteableInsertionCollectionNodeIndex insertedIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            IndexToPositionAndNode(insertedIndex, out int BlockIndex, out int Index, out Node Node);

            Action<WriteableOperation> HandlerRedo = (WriteableOperation operation) => RedoInsertNewNode(operation);
            Action<WriteableOperation> HandlerUndo = (WriteableOperation operation) => UndoInsertNewNode(operation);
            WriteableInsertNodeOperation Operation = CreateInsertNodeOperation(inner.Owner.Node, inner.PropertyName, BlockIndex, Index, Node, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();

            nodeIndex = Operation.BrowsingIndex;
        }

        private protected virtual void RedoInsertNewNode(WriteableOperation operation)
        {
            WriteableInsertNodeOperation InsertNodeOperation = (WriteableInsertNodeOperation)operation;
            ExecuteInsertNewNode(InsertNodeOperation);
        }

        private protected virtual void ExecuteInsertNewNode(WriteableInsertNodeOperation operation)
        {
            Node ParentNode = operation.ParentNode;
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

        private protected virtual void UndoInsertNewNode(WriteableOperation operation)
        {
            WriteableInsertNodeOperation InsertNodeOperation = (WriteableInsertNodeOperation)operation;
            WriteableRemoveNodeOperation RemoveNodeOperation = InsertNodeOperation.ToRemoveNodeOperation();

            ExecuteRemoveNode(RemoveNodeOperation);
        }

        /// <summary>
        /// Inserts a range of blocks in a block list.
        /// </summary>
        /// <param name="inner">The inner for the block list in which blocks are inserted.</param>
        /// <param name="insertedIndex">Index where to insert the first block.</param>
        /// <param name="indexList">List of nodes in blocks to insert.</param>
        public virtual void InsertBlockRange(IWriteableBlockListInner inner, int insertedIndex, IList<WriteableInsertionBlockNodeIndex> indexList)
        {
            Debug.Assert(inner != null);
            Debug.Assert(insertedIndex >= 0 && insertedIndex <= inner.BlockStateList.Count);
            Debug.Assert(indexList != null);

            int BlockIndex = insertedIndex - 1;
            int BlockNodeIndex = 0;

            foreach (WriteableInsertionBlockNodeIndex NodeIndex in indexList)
            {
                bool IsHandled = false;

                if (NodeIndex is WriteableInsertionNewBlockNodeIndex AsNewBlockNodeIndex)
                {
                    BlockIndex++;
                    BlockNodeIndex = 0;

                    Debug.Assert(AsNewBlockNodeIndex.BlockIndex == BlockIndex);

                    IsHandled = true;
                }
                else if (NodeIndex is WriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex)
                {
                    BlockNodeIndex++;

                    Debug.Assert(AsExistingBlockNodeIndex.BlockIndex == BlockIndex);
                    Debug.Assert(AsExistingBlockNodeIndex.Index == BlockNodeIndex);

                    IsHandled = true;
                }

                Debug.Assert(IsHandled);
            }

            int FinalBlockIndex = BlockIndex + 1;

            Action<WriteableOperation> HandlerRedoInsertNode = (WriteableOperation operation) => RedoInsertNewNode(operation);
            Action<WriteableOperation> HandlerUndoInsertNode = (WriteableOperation operation) => UndoInsertNewNode(operation);
            Action<WriteableOperation> HandlerRedoInsertBlock = (WriteableOperation operation) => RedoInsertNewBlock(operation);
            Action<WriteableOperation> HandlerUndoInsertBlock = (WriteableOperation operation) => UndoInsertNewBlock(operation);

            WriteableOperationList OperationList = CreateOperationList();

            foreach (WriteableInsertionBlockNodeIndex NodeIndex in indexList)
                if (NodeIndex is WriteableInsertionNewBlockNodeIndex AsNewBlockNodeIndex)
                {
                    IBlock NewBlock = NodeTreeHelperBlockList.CreateBlock(inner.Owner.Node, inner.PropertyName, ReplicationStatus.Normal, AsNewBlockNodeIndex.PatternNode, AsNewBlockNodeIndex.SourceNode);
                    WriteableInsertBlockOperation OperationInsertBlock = CreateInsertBlockOperation(inner.Owner.Node, inner.PropertyName, AsNewBlockNodeIndex.BlockIndex, NewBlock, AsNewBlockNodeIndex.Node, HandlerRedoInsertBlock, HandlerUndoInsertBlock, isNested: true);
                    OperationList.Add(OperationInsertBlock);
                }
                else if (NodeIndex is WriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex)
                {
                    IndexToPositionAndNode(AsExistingBlockNodeIndex, out BlockIndex, out int Index, out Node Node);
                    WriteableInsertNodeOperation OperationInsertNode = CreateInsertNodeOperation(inner.Owner.Node, inner.PropertyName, BlockIndex, Index, Node, HandlerRedoInsertNode, HandlerUndoInsertNode, isNested: true);
                    OperationList.Add(OperationInsertNode);
                }

            Debug.Assert(BlockIndex + 1 == FinalBlockIndex);

            if (OperationList.Count > 0)
            {
                WriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                Action<WriteableOperation> HandlerRedo = (WriteableOperation operation) => RedoRefresh(operation);
                Action<WriteableOperation> HandlerUndo = (WriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                WriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedo, HandlerUndo, isNested: false);
                WriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);

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
        public virtual void InsertNodeRange(IWriteableCollectionInner inner, int blockIndex, int insertedIndex, IList<WriteableInsertionCollectionNodeIndex> indexList)
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
        public virtual void InsertNodeRange(IWriteableBlockListInner inner, int blockIndex, int insertedIndex, IList<WriteableInsertionCollectionNodeIndex> indexList)
        {
            Debug.Assert(inner != null);
            Debug.Assert(blockIndex >= 0 && blockIndex < inner.BlockStateList.Count);

            IWriteableBlockState BlockState = (IWriteableBlockState)inner.BlockStateList[blockIndex];
            Debug.Assert(insertedIndex >= 0 && insertedIndex <= BlockState.StateList.Count);

            int BlockNodeIndex = insertedIndex;

            foreach (WriteableInsertionCollectionNodeIndex NodeIndex in indexList)
            {
                bool IsHandled = false;

                if (NodeIndex is WriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex)
                {
                    Debug.Assert(AsExistingBlockNodeIndex.BlockIndex == blockIndex);
                    Debug.Assert(AsExistingBlockNodeIndex.Index == BlockNodeIndex);

                    BlockNodeIndex++;
                    IsHandled = true;
                }

                Debug.Assert(IsHandled);
            }

            int FinalNodeIndex = BlockNodeIndex;

            Action<WriteableOperation> HandlerRedoInsertNode = (WriteableOperation operation) => RedoInsertNewNode(operation);
            Action<WriteableOperation> HandlerUndoInsertNode = (WriteableOperation operation) => UndoInsertNewNode(operation);

            WriteableOperationList OperationList = CreateOperationList();

            foreach (WriteableInsertionCollectionNodeIndex NodeIndex in indexList)
                if (NodeIndex is WriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex)
                {
                    IndexToPositionAndNode(AsExistingBlockNodeIndex, out blockIndex, out int Index, out Node Node);
                    WriteableInsertNodeOperation OperationInsertNode = CreateInsertNodeOperation(inner.Owner.Node, inner.PropertyName, blockIndex, Index, Node, HandlerRedoInsertNode, HandlerUndoInsertNode, isNested: true);
                    OperationList.Add(OperationInsertNode);
                }

            if (OperationList.Count > 0)
            {
                WriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                Action<WriteableOperation> HandlerRedo = (WriteableOperation operation) => RedoRefresh(operation);
                Action<WriteableOperation> HandlerUndo = (WriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                WriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedo, HandlerUndo, isNested: false);
                WriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);

                OperationGroup.Redo();
                SetLastOperation(OperationGroup);
                CheckInvariant();
            }
        }

        /// <summary></summary>
        public virtual void InsertNodeRange(IWriteableListInner inner, int insertedIndex, IList<WriteableInsertionCollectionNodeIndex> indexList)
        {
            Debug.Assert(inner != null);
            Debug.Assert(insertedIndex >= 0 && insertedIndex <= inner.StateList.Count);

            int BlockNodeIndex = insertedIndex;

            foreach (WriteableInsertionCollectionNodeIndex NodeIndex in indexList)
            {
                bool IsHandled = false;

                if (NodeIndex is WriteableInsertionListNodeIndex AsListNodeIndex)
                {
                    Debug.Assert(AsListNodeIndex.Index == BlockNodeIndex);

                    BlockNodeIndex++;
                    IsHandled = true;
                }

                Debug.Assert(IsHandled);
            }

            int FinalNodeIndex = BlockNodeIndex;

            Action<WriteableOperation> HandlerRedoInsertNode = (WriteableOperation operation) => RedoInsertNewNode(operation);
            Action<WriteableOperation> HandlerUndoInsertNode = (WriteableOperation operation) => UndoInsertNewNode(operation);

            WriteableOperationList OperationList = CreateOperationList();

            foreach (WriteableInsertionCollectionNodeIndex NodeIndex in indexList)
                if (NodeIndex is WriteableInsertionListNodeIndex AsListNodeIndex)
                {
                    IndexToPositionAndNode(AsListNodeIndex, out int BlockIndex, out int Index, out Node Node);
                    WriteableInsertNodeOperation OperationInsertNode = CreateInsertNodeOperation(inner.Owner.Node, inner.PropertyName, BlockIndex, Index, Node, HandlerRedoInsertNode, HandlerUndoInsertNode, isNested: true);
                    OperationList.Add(OperationInsertNode);
                }

            if (OperationList.Count > 0)
            {
                WriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                Action<WriteableOperation> HandlerRedo = (WriteableOperation operation) => RedoRefresh(operation);
                Action<WriteableOperation> HandlerUndo = (WriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                WriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedo, HandlerUndo, isNested: false);
                WriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);

                OperationGroup.Redo();
                SetLastOperation(OperationGroup);
                CheckInvariant();
            }
        }
    }
}
