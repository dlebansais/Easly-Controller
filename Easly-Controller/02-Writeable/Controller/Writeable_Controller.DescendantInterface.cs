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
    public partial class WriteableController : ReadOnlyController, IWriteableController
    {
        #region Descendant Interface
        private protected override void CheckContextConsistency(IReadOnlyBrowseContext browseContext)
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
            foreach (KeyValuePair<string, IWriteableInner> Entry in state.InnerTable)
            {
                bool IsHandled = false;

                if (Entry.Value is IWriteablePlaceholderInner<IWriteableBrowsingPlaceholderNodeIndex> AsPlaceholderInner)
                {
                    PrunePlaceholderInner(AsPlaceholderInner);
                    IsHandled = true;
                }
                else if (Entry.Value is IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> AsOptionalInner)
                {
                    PruneOptionalInner(AsOptionalInner);
                    IsHandled = true;
                }
                else if (Entry.Value is IWriteableListInner<IWriteableBrowsingListNodeIndex> AsListInner)
                {
                    PruneListInner(AsListInner);
                    IsHandled = true;
                }
                else if (Entry.Value is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner)
                {
                    PruneBlockListInner(AsBlockListInner);
                    IsHandled = true;
                }

                Debug.Assert(IsHandled);
            }
        }

        private protected virtual void PrunePlaceholderInner(IWriteablePlaceholderInner<IWriteableBrowsingPlaceholderNodeIndex> inner)
        {
            PruneState(inner.ChildState);

            Stats.PlaceholderNodeCount--;
        }

        private protected virtual void PruneOptionalInner(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> inner)
        {
            PruneState(inner.ChildState);

            Stats.OptionalNodeCount--;
            if (inner.IsAssigned)
                Stats.AssignedOptionalNodeCount--;
        }

        private protected virtual void PruneListInner(IWriteableListInner<IWriteableBrowsingListNodeIndex> inner)
        {
            foreach (IWriteableNodeState State in inner.StateList)
            {
                PruneState(State);

                Stats.PlaceholderNodeCount--;
            }

            Stats.ListCount--;
        }

        private protected virtual void PruneBlockListInner(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner)
        {
            for (int BlockIndex = inner.BlockStateList.Count; BlockIndex > 0; BlockIndex--)
            {
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRemoveBlockView(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                IWriteableRemoveBlockViewOperation Operation = CreateRemoveBlockViewOperation(inner.Owner.Node, inner.PropertyName, BlockIndex - 1, HandlerRedo, HandlerUndo, isNested: true);

                Operation.Redo();
            }

            Stats.BlockListCount--;
        }

        private protected virtual void RedoRemoveBlockView(IWriteableOperation operation)
        {
            IWriteableRemoveBlockViewOperation RemoveBlockViewOperation = (IWriteableRemoveBlockViewOperation)operation;
            ExecuteRemoveBlockView(RemoveBlockViewOperation);
        }

        private protected virtual void ExecuteRemoveBlockView(IWriteableRemoveBlockViewOperation operation)
        {
            Node ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;
            IWriteableBlockState RemovedBlockState = Inner.BlockStateList[operation.BlockIndex];

            for (int Index = 0; Index < RemovedBlockState.StateList.Count; Index++)
            {
                IWriteableNodeState State = RemovedBlockState.StateList[Index];

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

        private protected virtual void NotifyBlockStateInserted(IWriteableInsertBlockOperation operation)
        {
            BlockStateInsertedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyBlockStateRemoved(IWriteableRemoveBlockOperation operation)
        {
            BlockStateRemovedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyBlockViewRemoved(IWriteableRemoveBlockViewOperation operation)
        {
            BlockViewRemovedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyStateInserted(IWriteableInsertNodeOperation operation)
        {
            StateInsertedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyStateRemoved(IWriteableRemoveNodeOperation operation)
        {
            StateRemovedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyStateReplaced(IWriteableReplaceOperation operation)
        {
            StateReplacedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyStateAssigned(IWriteableAssignmentOperation operation)
        {
            StateAssignedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyStateUnassigned(IWriteableAssignmentOperation operation)
        {
            StateUnassignedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyDiscreteValueChanged(IWriteableChangeDiscreteValueOperation operation)
        {
            DiscreteValueChangedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyTextChanged(IWriteableChangeTextOperation operation)
        {
            TextChangedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyCommentChanged(IWriteableChangeCommentOperation operation)
        {
            CommentChangedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyBlockStateChanged(IWriteableChangeBlockOperation operation)
        {
            BlockStateChangedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyStateMoved(IWriteableMoveNodeOperation operation)
        {
            StateMovedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyBlockStateMoved(IWriteableMoveBlockOperation operation)
        {
            BlockStateMovedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyBlockSplit(IWriteableSplitBlockOperation operation)
        {
            BlockSplitHandler?.Invoke(operation);
        }

        private protected virtual void NotifyBlocksMerged(IWriteableMergeBlocksOperation operation)
        {
            BlocksMergedHandler?.Invoke(operation);
        }

        private protected virtual void NotifyGenericRefresh(IWriteableGenericRefreshOperation operation)
        {
            GenericRefreshHandler?.Invoke(operation);
        }

        private protected virtual void SetLastOperation(IWriteableOperation operation)
        {
            IWriteableOperationList OperationList = CreateOperationList();
            OperationList.Add(operation);
            IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
            IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, null);

            SetLastOperation(OperationGroup);
        }

        private protected virtual void SetLastOperation(IWriteableOperationGroup operationGroup)
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
