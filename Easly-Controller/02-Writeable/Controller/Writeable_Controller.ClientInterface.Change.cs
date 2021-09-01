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
    public partial class WriteableController : ReadOnlyController
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

            Action<WriteableOperation> HandlerRedo = (WriteableOperation operation) => RedoChangeReplication(operation);
            Action<WriteableOperation> HandlerUndo = (WriteableOperation operation) => UndoChangeReplication(operation);
            WriteableChangeBlockOperation Operation = CreateChangeBlockOperation(inner.Owner.Node, inner.PropertyName, blockIndex, replication, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        private protected virtual void RedoChangeReplication(WriteableOperation operation)
        {
            WriteableChangeBlockOperation ChangeBlockOperation = (WriteableChangeBlockOperation)operation;
            ExecuteChangeReplication(ChangeBlockOperation);
        }

        private protected virtual void UndoChangeReplication(WriteableOperation operation)
        {
            WriteableChangeBlockOperation ChangeBlockOperation = (WriteableChangeBlockOperation)operation;
            ChangeBlockOperation = ChangeBlockOperation.ToInverseChange();

            ExecuteChangeReplication(ChangeBlockOperation);
        }

        private protected virtual void ExecuteChangeReplication(WriteableChangeBlockOperation operation)
        {
            Node ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<WriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<WriteableBrowsingBlockNodeIndex>;

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

            Action<WriteableOperation> HandlerRedo = (WriteableOperation operation) => RedoChangeDiscreteValue(operation);
            Action<WriteableOperation> HandlerUndo = (WriteableOperation operation) => UndoChangeDiscreteValue(operation);
            IWriteableNodeState State = (IWriteableNodeState)StateTable[nodeIndex];
            WriteableChangeDiscreteValueOperation Operation = CreateChangeDiscreteValueOperation(State.Node, propertyName, value, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        private protected virtual void RedoChangeDiscreteValue(WriteableOperation operation)
        {
            WriteableChangeDiscreteValueOperation ChangeDiscreteValueOperation = (WriteableChangeDiscreteValueOperation)operation;
            ExecuteChangeDiscreteValue(ChangeDiscreteValueOperation);
        }

        private protected virtual void UndoChangeDiscreteValue(WriteableOperation operation)
        {
            WriteableChangeDiscreteValueOperation ChangeDiscreteValueOperation = (WriteableChangeDiscreteValueOperation)operation;
            ChangeDiscreteValueOperation = ChangeDiscreteValueOperation.ToInverseChange();

            ExecuteChangeDiscreteValue(ChangeDiscreteValueOperation);
        }

        private protected virtual void ExecuteChangeDiscreteValue(WriteableChangeDiscreteValueOperation operation)
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

            Action<WriteableOperation> HandlerRedo = (WriteableOperation operation) => RedoChangeText(operation);
            Action<WriteableOperation> HandlerUndo = (WriteableOperation operation) => UndoChangeText(operation);
            IWriteableNodeState State = (IWriteableNodeState)StateTable[nodeIndex];
            WriteableChangeTextOperation Operation = CreateChangeTextOperation(State.Node, propertyName, text, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        private protected virtual void RedoChangeText(WriteableOperation operation)
        {
            WriteableChangeTextOperation ChangeTextOperation = (WriteableChangeTextOperation)operation;
            ExecuteChangeText(ChangeTextOperation);
        }

        private protected virtual void UndoChangeText(WriteableOperation operation)
        {
            WriteableChangeTextOperation ChangeTextOperation = (WriteableChangeTextOperation)operation;
            ChangeTextOperation = ChangeTextOperation.ToInverseChange();

            ExecuteChangeText(ChangeTextOperation);
        }

        private protected virtual void ExecuteChangeText(WriteableChangeTextOperation operation)
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

            Action<WriteableOperation> HandlerRedo = (WriteableOperation operation) => RedoChangeComment(operation);
            Action<WriteableOperation> HandlerUndo = (WriteableOperation operation) => UndoChangeComment(operation);
            IWriteableNodeState State = (IWriteableNodeState)StateTable[nodeIndex];
            WriteableChangeCommentOperation Operation = CreateChangeCommentOperation(State.Node, text, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        private protected virtual void RedoChangeComment(WriteableOperation operation)
        {
            WriteableChangeCommentOperation ChangeCommentOperation = (WriteableChangeCommentOperation)operation;
            ExecuteChangeComment(ChangeCommentOperation);
        }

        private protected virtual void UndoChangeComment(WriteableOperation operation)
        {
            WriteableChangeCommentOperation ChangeCommentOperation = (WriteableChangeCommentOperation)operation;
            ChangeCommentOperation = ChangeCommentOperation.ToInverseChange();

            ExecuteChangeComment(ChangeCommentOperation);
        }

        private protected virtual void ExecuteChangeComment(WriteableChangeCommentOperation operation)
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
