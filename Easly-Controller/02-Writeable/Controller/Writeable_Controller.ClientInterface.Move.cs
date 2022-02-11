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
        /// <summary>
        /// Checks whether a node can be moved in a list.
        /// </summary>
        /// <param name="inner">The inner where the node is.</param>
        /// <param name="nodeIndex">Index of the node that would be moved.</param>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        public virtual bool IsMoveable(IWriteableCollectionInner inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction)
        {
            Contract.RequireNotNull(inner, out IWriteableCollectionInner Inner);
            Contract.RequireNotNull(nodeIndex, out IWriteableBrowsingCollectionNodeIndex NodeIndex);

            IWriteableNodeState State = (IWriteableNodeState)StateTable[NodeIndex];
            Debug.Assert(State != null);

            bool Result = Inner.IsMoveable(NodeIndex, direction);

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
            Contract.RequireNotNull(inner, out IWriteableCollectionInner Inner);
            Contract.RequireNotNull(nodeIndex, out IWriteableBrowsingCollectionNodeIndex NodeIndex);

            IWriteableNodeState State = (IWriteableNodeState)StateTable[NodeIndex];
            Debug.Assert(State != null);

            IndexToPositionAndNode(NodeIndex, out int BlockIndex, out int Index, out _, out Node Node);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoMove(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoMove(operation);
            WriteableMoveNodeOperation Operation = CreateMoveNodeOperation(Inner.Owner.Node, Inner.PropertyName, BlockIndex, Index, direction, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        private protected virtual void RedoMove(IWriteableOperation operation)
        {
            WriteableMoveNodeOperation MoveNodeOperation = (WriteableMoveNodeOperation)operation;
            ExecuteMove(MoveNodeOperation);
        }

        private protected virtual void UndoMove(IWriteableOperation operation)
        {
            WriteableMoveNodeOperation MoveNodeOperation = (WriteableMoveNodeOperation)operation;
            MoveNodeOperation = MoveNodeOperation.ToInverseMove();

            ExecuteMove(MoveNodeOperation);
        }

        private protected virtual void ExecuteMove(WriteableMoveNodeOperation operation)
        {
            Node ParentNode = operation.ParentNode;
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
            Contract.RequireNotNull(inner, out IWriteableBlockListInner Inner);
            Debug.Assert(blockIndex >= 0 && blockIndex < Inner.BlockStateList.Count);

            return blockIndex + direction >= 0 && blockIndex + direction < Inner.BlockStateList.Count;
        }

        /// <summary>
        /// Moves a block around in a block list.
        /// </summary>
        /// <param name="inner">The inner where the block is moved.</param>
        /// <param name="blockIndex">Index of the block to move.</param>
        /// <param name="direction">The change in position, relative to the current block position.</param>
        public virtual void MoveBlock(IWriteableBlockListInner inner, int blockIndex, int direction)
        {
            Contract.RequireNotNull(inner, out IWriteableBlockListInner Inner);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoMoveBlock(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoMoveBlock(operation);
            WriteableMoveBlockOperation Operation = CreateMoveBlockOperation(Inner.Owner.Node, Inner.PropertyName, blockIndex, direction, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        private protected virtual void RedoMoveBlock(IWriteableOperation operation)
        {
            WriteableMoveBlockOperation MoveBlockOperation = (WriteableMoveBlockOperation)operation;
            ExecuteMoveBlock(MoveBlockOperation);
        }

        private protected virtual void UndoMoveBlock(IWriteableOperation operation)
        {
            WriteableMoveBlockOperation MoveBlockOperation = (WriteableMoveBlockOperation)operation;
            MoveBlockOperation = MoveBlockOperation.ToInverseMoveBlock();

            ExecuteMoveBlock(MoveBlockOperation);
        }

        private protected virtual void ExecuteMoveBlock(WriteableMoveBlockOperation operation)
        {
            Node ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;

            IWriteableBlockState BlockState = (IWriteableBlockState)Inner.BlockStateList[operation.BlockIndex];
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockState.StateList.Count > 0);

            IWriteableNodeState State = (IWriteableNodeState)BlockState.StateList[0];
            Debug.Assert(State != null);

            IWriteableBrowsingExistingBlockNodeIndex NodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
            Debug.Assert(NodeIndex != null);

            Inner.MoveBlock(operation);

            NotifyBlockStateMoved(operation);
        }
    }
}
