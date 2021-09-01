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
        /// <summary>
        /// Assign the optional node.
        /// </summary>
        /// <param name="nodeIndex">Index of the optional node.</param>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already assigned.</param>
        public virtual void Assign(WriteableBrowsingOptionalNodeIndex nodeIndex, out bool isChanged)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(StateTable.ContainsKey(nodeIndex));

            IWriteableOptionalNodeState State = StateTable[nodeIndex] as IWriteableOptionalNodeState;
            Debug.Assert(State != null);

            IWriteableOptionalInner<WriteableBrowsingOptionalNodeIndex> Inner = State.ParentInner as IWriteableOptionalInner<WriteableBrowsingOptionalNodeIndex>;
            Debug.Assert(Inner != null);

            if (!Inner.IsAssigned)
            {
                Action<WriteableOperation> HandlerRedo = (WriteableOperation operation) => RedoAssign(operation);
                Action<WriteableOperation> HandlerUndo = (WriteableOperation operation) => UndoAssign(operation);
                WriteableAssignmentOperation Operation = CreateAssignmentOperation(Inner.Owner.Node, Inner.PropertyName, HandlerRedo, HandlerUndo, isNested: false);

                Operation.Redo();
                SetLastOperation(Operation);
                CheckInvariant();

                isChanged = true;
            }
            else
                isChanged = false;
        }

        private protected virtual void RedoAssign(WriteableOperation operation)
        {
            WriteableAssignmentOperation AssignmentOperation = (WriteableAssignmentOperation)operation;
            ExecuteAssign(AssignmentOperation);
        }

        private protected virtual void ExecuteAssign(WriteableAssignmentOperation operation)
        {
            Node ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableOptionalInner<WriteableBrowsingOptionalNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableOptionalInner<WriteableBrowsingOptionalNodeIndex>;

            Inner.Assign(operation);

            Stats.AssignedOptionalNodeCount++;

            NotifyStateAssigned(operation);
        }

        private protected virtual void UndoAssign(WriteableOperation operation)
        {
            WriteableAssignmentOperation AssignmentOperation = (WriteableAssignmentOperation)operation;
            AssignmentOperation = AssignmentOperation.ToInverseAssignment();

            ExecuteUnassign(AssignmentOperation);
        }

        /// <summary>
        /// Unassign the optional node.
        /// </summary>
        /// <param name="nodeIndex">Index of the optional node.</param>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already not assigned.</param>
        public virtual void Unassign(WriteableBrowsingOptionalNodeIndex nodeIndex, out bool isChanged)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(StateTable.ContainsKey(nodeIndex));

            IWriteableOptionalNodeState State = StateTable[nodeIndex] as IWriteableOptionalNodeState;
            Debug.Assert(State != null);

            IWriteableOptionalInner<WriteableBrowsingOptionalNodeIndex> Inner = State.ParentInner as IWriteableOptionalInner<WriteableBrowsingOptionalNodeIndex>;
            Debug.Assert(Inner != null);

            if (Inner.IsAssigned)
            {
                Action<WriteableOperation> HandlerRedo = (WriteableOperation operation) => RedoUnassign(operation);
                Action<WriteableOperation> HandlerUndo = (WriteableOperation operation) => UndoUnassign(operation);
                WriteableAssignmentOperation Operation = CreateAssignmentOperation(Inner.Owner.Node, Inner.PropertyName, HandlerRedo, HandlerUndo, isNested: false);

                Operation.Redo();
                SetLastOperation(Operation);
                CheckInvariant();

                isChanged = true;
            }
            else
                isChanged = false;
        }

        private protected virtual void RedoUnassign(WriteableOperation operation)
        {
            WriteableAssignmentOperation AssignmentOperation = (WriteableAssignmentOperation)operation;
            ExecuteUnassign(AssignmentOperation);
        }

        private protected virtual void ExecuteUnassign(WriteableAssignmentOperation operation)
        {
            Node ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableOptionalInner<WriteableBrowsingOptionalNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableOptionalInner<WriteableBrowsingOptionalNodeIndex>;

            Inner.Unassign(operation);

            Stats.AssignedOptionalNodeCount--;

            NotifyStateUnassigned(operation);
        }

        private protected virtual void UndoUnassign(WriteableOperation operation)
        {
            WriteableAssignmentOperation AssignmentOperation = (WriteableAssignmentOperation)operation;
            AssignmentOperation = AssignmentOperation.ToInverseAssignment();

            ExecuteAssign(AssignmentOperation);
        }
    }
}
