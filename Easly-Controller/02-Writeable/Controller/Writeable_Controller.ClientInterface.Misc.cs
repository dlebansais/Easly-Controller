namespace EaslyController.Writeable
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using Contracts;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports operations to modify the tree.
    /// </summary>
    public partial class WriteableController : ReadOnlyController
    {
        private protected virtual void RedoRefresh(IWriteableOperation operation)
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
        public virtual void SplitIdentifier(IWriteableListInner inner, IWriteableInsertionListNodeIndex replaceIndex, IWriteableInsertionListNodeIndex insertIndex, out IWriteableBrowsingListNodeIndex firstIndex, out IWriteableBrowsingListNodeIndex secondIndex)
        {
            Contract.RequireNotNull(inner, out IWriteableListInner Inner);
            Contract.RequireNotNull(replaceIndex, out IWriteableInsertionListNodeIndex ReplaceIndex);
            Contract.RequireNotNull(insertIndex, out IWriteableInsertionListNodeIndex InsertIndex);
            IWriteableNodeState Owner = Inner.Owner;
            IWriteableIndex ParentIndex = Owner.ParentIndex;
            Debug.Assert(Contains(ParentIndex));
            Debug.Assert(IndexToState(ParentIndex) == Owner);
            WriteableInnerReadOnlyDictionary<string> InnerTable = Owner.InnerTable;
            Debug.Assert(InnerTable.ContainsKey(Inner.PropertyName));
            Debug.Assert(InnerTable[Inner.PropertyName] == Inner);

            int Index = ReplaceIndex.Index;
            Debug.Assert(InsertIndex.Index == Index + 1);
            Node ReplacingNode = ReplaceIndex.Node;
            Node InsertedNode = InsertIndex.Node;

            Action<IWriteableOperation> HandlerRedoReplace = (IWriteableOperation operation) => RedoReplace(operation);
            Action<IWriteableOperation> HandlerUndoReplace = (IWriteableOperation operation) => UndoReplace(operation);
            IWriteableReplaceOperation ReplaceOperation = CreateReplaceOperation(Inner.Owner.Node, Inner.PropertyName, -1, Index, clearNode: false, ReplacingNode, HandlerRedoReplace, HandlerUndoReplace, isNested: true);

            Action<IWriteableOperation> HandlerRedoInsert = (IWriteableOperation operation) => RedoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerUndoInsert = (IWriteableOperation operation) => UndoInsertNewNode(operation);
            WriteableInsertNodeOperation InsertOperation = CreateInsertNodeOperation(Inner.Owner.Node, Inner.PropertyName, -1, Index + 1, InsertedNode, HandlerRedoInsert, HandlerUndoInsert, isNested: true);

            ReplaceOperation.Redo();
            InsertOperation.Redo();

            Action<IWriteableOperation> HandlerRedoRefresh = (IWriteableOperation operation) => RedoRefresh(operation);
            Action<IWriteableOperation> HandlerUndoRefresh = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
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

        private protected virtual void IndexToPositionAndNode(IWriteableIndex nodeIndex, out int blockIndex, out int index, out bool clearNode, out Node node)
        {
            blockIndex = -1;
            index = -1;
            clearNode = false;
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
                    //Type NodeType = NodeTreeHelperOptional.OptionalItemType(AsOptionalClearIndex.ParentNode, AsOptionalClearIndex.PropertyName);
                    //Node Item = NodeHelper.CreateDefaultFromType(NodeType);
                    //node = Item;
                    clearNode = true;
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
