namespace EaslyController.Writeable
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports operations to modify the tree.
    /// </summary>
    public partial class WriteableController : ReadOnlyController
    {
        #region Descendant Interface
        private protected override void CheckContextConsistency(ReadOnlyBrowseContext browseContext)
        {
            ((WriteableBrowseContext)browseContext).CheckConsistency();
        }

        private protected virtual void PruneState(IWriteableNodeState state)
        {
            PruneStateChildren(state);
            RemoveState(state.ParentIndex);
        }

        private protected virtual void PruneStateChildren(IWriteableNodeState state)
        {
            foreach (string Key in state.InnerTable.Keys)
            {
                IWriteableInner Value = (IWriteableInner)state.InnerTable[Key];
                bool IsHandled = false;

                if (Value is WriteablePlaceholderInner<WriteableBrowsingPlaceholderNodeIndex> AsPlaceholderInner)
                {
                    PrunePlaceholderInner(AsPlaceholderInner);
                    IsHandled = true;
                }
                else if (Value is WriteableOptionalInner<WriteableBrowsingOptionalNodeIndex> AsOptionalInner)
                {
                    PruneOptionalInner(AsOptionalInner);
                    IsHandled = true;
                }
                else if (Value is WriteableListInner<WriteableBrowsingListNodeIndex> AsListInner)
                {
                    PruneListInner(AsListInner);
                    IsHandled = true;
                }
                else if (Value is WriteableBlockListInner<WriteableBrowsingBlockNodeIndex> AsBlockListInner)
                {
                    PruneBlockListInner(AsBlockListInner);
                    IsHandled = true;
                }

                Debug.Assert(IsHandled);
            }
        }

        private protected virtual void PrunePlaceholderInner(WriteablePlaceholderInner<WriteableBrowsingPlaceholderNodeIndex> inner)
        {
            PruneState(inner.ChildState);

            Stats.PlaceholderNodeCount--;
        }

        private protected virtual void PruneOptionalInner(WriteableOptionalInner<WriteableBrowsingOptionalNodeIndex> inner)
        {
            PruneState(inner.ChildState);

            Stats.OptionalNodeCount--;
            if (inner.IsAssigned)
                Stats.AssignedOptionalNodeCount--;
        }

        private protected virtual void PruneListInner(WriteableListInner<WriteableBrowsingListNodeIndex> inner)
        {
            foreach (IWriteableNodeState State in inner.StateList)
            {
                PruneState(State);

                Stats.PlaceholderNodeCount--;
            }

            Stats.ListCount--;
        }

        private protected virtual void PruneBlockListInner(WriteableBlockListInner<WriteableBrowsingBlockNodeIndex> inner)
        {
            for (int BlockIndex = inner.BlockStateList.Count; BlockIndex > 0; BlockIndex--)
            {
                Action<WriteableOperation> HandlerRedo = (WriteableOperation operation) => RedoRemoveBlockView(operation);
                Action<WriteableOperation> HandlerUndo = (WriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                WriteableRemoveBlockViewOperation Operation = CreateRemoveBlockViewOperation(inner.Owner.Node, inner.PropertyName, BlockIndex - 1, HandlerRedo, HandlerUndo, isNested: true);

                Operation.Redo();
            }

            Stats.BlockListCount--;
        }

        private protected virtual void RedoRemoveBlockView(WriteableOperation operation)
        {
            WriteableRemoveBlockViewOperation RemoveBlockViewOperation = (WriteableRemoveBlockViewOperation)operation;
            ExecuteRemoveBlockView(RemoveBlockViewOperation);
        }

        private protected virtual void ExecuteRemoveBlockView(WriteableRemoveBlockViewOperation operation)
        {
            Node ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<WriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<WriteableBrowsingBlockNodeIndex>;
            IWriteableBlockState RemovedBlockState = (IWriteableBlockState)Inner.BlockStateList[operation.BlockIndex];

            for (int Index = 0; Index < RemovedBlockState.StateList.Count; Index++)
            {
                IWriteableNodeState State = (IWriteableNodeState)RemovedBlockState.StateList[Index];

                PruneState(State);
                Stats.PlaceholderNodeCount--;
            }

            IWriteableBrowsingPatternIndex PatternIndex = RemovedBlockState.PatternIndex;
            IWriteableBrowsingSourceIndex SourceIndex = RemovedBlockState.SourceIndex;

            Debug.Assert(PatternIndex != null);
            Debug.Assert(StateTable.ContainsKey(PatternIndex));
            Debug.Assert(SourceIndex != null);
            Debug.Assert(StateTable.ContainsKey(SourceIndex));

            RemoveState(PatternIndex);
            Stats.PlaceholderNodeCount--;

            RemoveState(SourceIndex);
            Stats.PlaceholderNodeCount--;

            Inner.NotifyBlockStateRemoved(RemovedBlockState);

            Stats.BlockCount--;

            operation.Update(RemovedBlockState);

            NotifyBlockViewRemoved(operation);
        }

        private protected virtual void NotifyBlockStateInserted(WriteableInsertBlockOperation operation)
        {
            BlockStateInsertedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyBlockStateRemoved(WriteableRemoveBlockOperation operation)
        {
            BlockStateRemovedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyBlockViewRemoved(WriteableRemoveBlockViewOperation operation)
        {
            BlockViewRemovedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyStateInserted(WriteableInsertNodeOperation operation)
        {
            StateInsertedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyStateRemoved(WriteableRemoveNodeOperation operation)
        {
            StateRemovedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyStateReplaced(WriteableReplaceOperation operation)
        {
            StateReplacedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyStateAssigned(WriteableAssignmentOperation operation)
        {
            StateAssignedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyStateUnassigned(WriteableAssignmentOperation operation)
        {
            StateUnassignedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyDiscreteValueChanged(WriteableChangeDiscreteValueOperation operation)
        {
            DiscreteValueChangedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyTextChanged(WriteableChangeTextOperation operation)
        {
            TextChangedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyCommentChanged(WriteableChangeCommentOperation operation)
        {
            CommentChangedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyBlockStateChanged(WriteableChangeBlockOperation operation)
        {
            BlockStateChangedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyStateMoved(WriteableMoveNodeOperation operation)
        {
            StateMovedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyBlockStateMoved(WriteableMoveBlockOperation operation)
        {
            BlockStateMovedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyBlockSplit(WriteableSplitBlockOperation operation)
        {
            BlockSplitHandler?.Invoke(operation);
        }

        private protected virtual void NotifyBlocksMerged(WriteableMergeBlocksOperation operation)
        {
            BlocksMergedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyGenericRefresh(WriteableGenericRefreshOperation operation)
        {
            GenericRefreshHandler?.Invoke(operation);
        }

        private protected virtual void SetLastOperation(WriteableOperation operation)
        {
            WriteableOperationList OperationList = CreateOperationList();
            OperationList.Add(operation);
            WriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
            WriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, null);

            SetLastOperation(OperationGroup);
        }

        private protected virtual void SetLastOperation(WriteableOperationGroup operationGroup)
        {
            Debug.Assert(RedoIndex >= 0 && RedoIndex <= _OperationStack.Count);

            while (_OperationStack.Count > RedoIndex)
                _OperationStack.RemoveAt(RedoIndex);

            Debug.Assert(RedoIndex == _OperationStack.Count);

            _OperationStack.Add(operationGroup);
            RedoIndex = _OperationStack.Count;

            Debug.Assert(RedoIndex == _OperationStack.Count);
        }
        #endregion
    }
}
