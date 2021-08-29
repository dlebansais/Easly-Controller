namespace EaslyController.Writeable
{
    using System;
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
            Node ParentNode = operation.ParentNode;
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
            Node ParentNode = operation.ParentNode;
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
            Node ParentNode = operation.ParentNode;
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
            Node ParentNode = operation.ParentNode;
            string NewText = operation.NewText;

            IWriteableNodeState State = (IWriteableNodeState)GetState(ParentNode);
            Debug.Assert(State != null);

            string OldText = NodeTreeHelper.GetCommentText(State.Node);
            Debug.Assert(OldText != null);

            CommentHelper.Set(State.Node.Documentation, NewText);

            operation.Update(State, OldText);

            NotifyCommentChanged(operation);
        }
    }
}
