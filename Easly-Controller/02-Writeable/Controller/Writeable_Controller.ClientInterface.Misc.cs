namespace EaslyController.Writeable
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports operations to modify the tree.
    /// </summary>
    public partial class WriteableController : ReadOnlyController
    {
        private protected virtual void RedoRefresh(WriteableOperation operation)
        {
            WriteableGenericRefreshOperation GenericRefreshOperation = (WriteableGenericRefreshOperation)operation;
            ExecuteRefresh(GenericRefreshOperation);
        }

        private protected virtual void ExecuteRefresh(WriteableGenericRefreshOperation operation)
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
            WriteableOperationGroup OperationGroup = OperationStack[RedoIndex];
            OperationGroup.Undo();

            CheckInvariant();
        }

        /// <summary>
        /// Redo the last operation undone.
        /// </summary>
        public virtual void Redo()
        {
            Debug.Assert(CanRedo);

            WriteableOperationGroup OperationGroup = OperationStack[RedoIndex];
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
        public virtual void SplitIdentifier(IWriteableListInner inner, WriteableInsertionListNodeIndex replaceIndex, WriteableInsertionListNodeIndex insertIndex, out IWriteableBrowsingListNodeIndex firstIndex, out IWriteableBrowsingListNodeIndex secondIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(replaceIndex != null);
            Debug.Assert(insertIndex != null);
            IWriteableNodeState Owner = inner.Owner;
            IWriteableIndex ParentIndex = Owner.ParentIndex;
            Debug.Assert(Contains(ParentIndex));
            Debug.Assert(IndexToState(ParentIndex) == Owner);
            WriteableInnerReadOnlyDictionary<string> InnerTable = Owner.InnerTable;
            Debug.Assert(InnerTable.ContainsKey(inner.PropertyName));
            Debug.Assert(InnerTable[inner.PropertyName] == inner);

            int Index = replaceIndex.Index;
            Debug.Assert(insertIndex.Index == Index + 1);
            Node ReplacingNode = replaceIndex.Node;
            Node InsertedNode = insertIndex.Node;

            Action<WriteableOperation> HandlerRedoReplace = (WriteableOperation operation) => RedoReplace(operation);
            Action<WriteableOperation> HandlerUndoReplace = (WriteableOperation operation) => UndoReplace(operation);
            IWriteableReplaceOperation ReplaceOperation = CreateReplaceOperation(inner.Owner.Node, inner.PropertyName, -1, Index, ReplacingNode, HandlerRedoReplace, HandlerUndoReplace, isNested: true);

            Action<WriteableOperation> HandlerRedoInsert = (WriteableOperation operation) => RedoInsertNewNode(operation);
            Action<WriteableOperation> HandlerUndoInsert = (WriteableOperation operation) => UndoInsertNewNode(operation);
            WriteableInsertNodeOperation InsertOperation = CreateInsertNodeOperation(inner.Owner.Node, inner.PropertyName, -1, Index + 1, InsertedNode, HandlerRedoInsert, HandlerUndoInsert, isNested: true);

            ReplaceOperation.Redo();
            InsertOperation.Redo();

            Action<WriteableOperation> HandlerRedoRefresh = (WriteableOperation operation) => RedoRefresh(operation);
            Action<WriteableOperation> HandlerUndoRefresh = (WriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
            WriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedoRefresh, HandlerUndoRefresh, isNested: false);

            RefreshOperation.Redo();

            WriteableOperationList OperationList = CreateOperationList();
            OperationList.Add(ReplaceOperation);
            OperationList.Add(InsertOperation);
            WriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
            WriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);
            SetLastOperation(OperationGroup);

            CheckInvariant();

            firstIndex = ReplaceOperation.NewBrowsingIndex as IWriteableBrowsingListNodeIndex;
            Debug.Assert(firstIndex != null);

            secondIndex = InsertOperation.BrowsingIndex as IWriteableBrowsingListNodeIndex;
            Debug.Assert(secondIndex != null);
        }

        private protected virtual void IndexToPositionAndNode(IWriteableIndex nodeIndex, out int blockIndex, out int index, out Node node)
        {
            blockIndex = -1;
            index = -1;
            node = null;
            bool IsHandled = false;

            switch (nodeIndex)
            {
                case WriteableInsertionPlaceholderNodeIndex AsPlaceholderNodeIndex:
                    node = AsPlaceholderNodeIndex.Node;
                    IsHandled = true;
                    break;

                case WriteableInsertionOptionalNodeIndex AsOptionalNodeIndex:
                    node = AsOptionalNodeIndex.Node;
                    IsHandled = true;
                    break;

                case WriteableInsertionOptionalClearIndex AsOptionalClearIndex:
                    IsHandled = true;
                    break;

                case WriteableInsertionListNodeIndex AsListNodeIndex:
                    index = AsListNodeIndex.Index;
                    node = AsListNodeIndex.Node;
                    IsHandled = true;
                    break;

                case WriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex:
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

                case WriteableBrowsingExistingBlockNodeIndex AsExistingBlockNodeIndex:
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
