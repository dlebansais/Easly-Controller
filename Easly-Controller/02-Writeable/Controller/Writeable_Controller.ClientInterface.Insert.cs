namespace EaslyController.Writeable
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using Contracts;
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
        public virtual void Insert(IWriteableCollectionInner inner, IWriteableInsertionCollectionNodeIndex insertedIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            Contract.RequireNotNull(inner, out IWriteableCollectionInner Inner);
            Contract.RequireNotNull(insertedIndex, out IWriteableInsertionCollectionNodeIndex InsertedIndex);
            IWriteableNodeState Owner = Inner.Owner;
            IWriteableIndex ParentIndex = Owner.ParentIndex;
            Debug.Assert(Contains(ParentIndex));
            Debug.Assert(IndexToState(ParentIndex) == Owner);
            WriteableInnerReadOnlyDictionary<string> InnerTable = Owner.InnerTable;
            Debug.Assert(InnerTable.ContainsKey(Inner.PropertyName));
            Debug.Assert(InnerTable[Inner.PropertyName] == Inner);

            bool IsHandled = false;
            nodeIndex = null;

            if (Inner is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner && InsertedIndex is IWriteableInsertionNewBlockNodeIndex AsNewBlockIndex)
            {
                InsertNewBlock(AsBlockListInner, AsNewBlockIndex, out nodeIndex);
                IsHandled = true;
            }
            else if (Inner is IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> AsCollectionInner && InsertedIndex is IWriteableInsertionCollectionNodeIndex AsCollectionIndex)
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
            Node ParentNode = operation.ParentNode;
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
            IndexToPositionAndNode(insertedIndex, out int BlockIndex, out int Index, out _, out Node Node);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoInsertNewNode(operation);
            WriteableInsertNodeOperation Operation = CreateInsertNodeOperation(inner.Owner.Node, inner.PropertyName, BlockIndex, Index, Node, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();

            nodeIndex = Operation.BrowsingIndex;
        }

        private protected virtual void RedoInsertNewNode(IWriteableOperation operation)
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

        private protected virtual void UndoInsertNewNode(IWriteableOperation operation)
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
        public virtual void InsertBlockRange(IWriteableBlockListInner inner, int insertedIndex, IList<IWriteableInsertionBlockNodeIndex> indexList)
        {
            Contract.RequireNotNull(inner, out IWriteableBlockListInner Inner);
            Debug.Assert(insertedIndex >= 0 && insertedIndex <= Inner.BlockStateList.Count);
            Contract.RequireNotNull(indexList, out IList<IWriteableInsertionBlockNodeIndex> IndexList);

            int BlockIndex = insertedIndex - 1;
            int BlockNodeIndex = 0;

            foreach (IWriteableInsertionBlockNodeIndex NodeIndex in IndexList)
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

            WriteableOperationList OperationList = CreateOperationList();

            foreach (IWriteableInsertionBlockNodeIndex NodeIndex in IndexList)
                if (NodeIndex is IWriteableInsertionNewBlockNodeIndex AsNewBlockNodeIndex)
                {
                    IBlock NewBlock = NodeTreeHelperBlockList.CreateBlock(Inner.Owner.Node, Inner.PropertyName, ReplicationStatus.Normal, AsNewBlockNodeIndex.PatternNode, AsNewBlockNodeIndex.SourceNode);
                    IWriteableInsertBlockOperation OperationInsertBlock = CreateInsertBlockOperation(Inner.Owner.Node, Inner.PropertyName, AsNewBlockNodeIndex.BlockIndex, NewBlock, AsNewBlockNodeIndex.Node, HandlerRedoInsertBlock, HandlerUndoInsertBlock, isNested: true);
                    OperationList.Add(OperationInsertBlock);
                }
                else if (NodeIndex is IWriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex)
                {
                    IndexToPositionAndNode(AsExistingBlockNodeIndex, out BlockIndex, out int Index, out _, out Node Node);
                    WriteableInsertNodeOperation OperationInsertNode = CreateInsertNodeOperation(Inner.Owner.Node, Inner.PropertyName, BlockIndex, Index, Node, HandlerRedoInsertNode, HandlerUndoInsertNode, isNested: true);
                    OperationList.Add(OperationInsertNode);
                }

            Debug.Assert(BlockIndex + 1 == FinalBlockIndex);

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
        /// Inserts a range of nodes in a list or block list.
        /// </summary>
        /// <param name="inner">The inner for the list or block list in which nodes are inserted.</param>
        /// <param name="blockIndex">Index of the block where to insert nodes, for a block list. -1 for a list.</param>
        /// <param name="insertedIndex">Index of the first node to insert.</param>
        /// <param name="indexList">List of nodes to insert.</param>
        public virtual void InsertNodeRange(IWriteableCollectionInner inner, int blockIndex, int insertedIndex, IList<IWriteableInsertionCollectionNodeIndex> indexList)
        {
            Contract.RequireNotNull(inner, out IWriteableCollectionInner Inner);

            bool IsHandled = false;

            if (Inner is IWriteableBlockListInner AsBlockListInner)
            {
                InsertNodeRange(AsBlockListInner, blockIndex, insertedIndex, indexList);
                IsHandled = true;
            }
            else if (Inner is IWriteableListInner AsListInner)
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
            Contract.RequireNotNull(inner, out IWriteableBlockListInner Inner);
            Debug.Assert(blockIndex >= 0 && blockIndex < Inner.BlockStateList.Count);

            IWriteableBlockState BlockState = (IWriteableBlockState)Inner.BlockStateList[blockIndex];
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

            WriteableOperationList OperationList = CreateOperationList();

            foreach (IWriteableInsertionCollectionNodeIndex NodeIndex in indexList)
                if (NodeIndex is IWriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex)
                {
                    IndexToPositionAndNode(AsExistingBlockNodeIndex, out blockIndex, out int Index, out _, out Node Node);
                    WriteableInsertNodeOperation OperationInsertNode = CreateInsertNodeOperation(Inner.Owner.Node, Inner.PropertyName, blockIndex, Index, Node, HandlerRedoInsertNode, HandlerUndoInsertNode, isNested: true);
                    OperationList.Add(OperationInsertNode);
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
        public virtual void InsertNodeRange(IWriteableListInner inner, int insertedIndex, IList<IWriteableInsertionCollectionNodeIndex> indexList)
        {
            Contract.RequireNotNull(inner, out IWriteableListInner Inner);
            Debug.Assert(insertedIndex >= 0 && insertedIndex <= Inner.StateList.Count);

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

            WriteableOperationList OperationList = CreateOperationList();

            foreach (IWriteableInsertionCollectionNodeIndex NodeIndex in indexList)
                if (NodeIndex is IWriteableInsertionListNodeIndex AsListNodeIndex)
                {
                    IndexToPositionAndNode(AsListNodeIndex, out int BlockIndex, out int Index, out _, out Node Node);
                    WriteableInsertNodeOperation OperationInsertNode = CreateInsertNodeOperation(Inner.Owner.Node, Inner.PropertyName, BlockIndex, Index, Node, HandlerRedoInsertNode, HandlerUndoInsertNode, isNested: true);
                    OperationList.Add(OperationInsertNode);
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
