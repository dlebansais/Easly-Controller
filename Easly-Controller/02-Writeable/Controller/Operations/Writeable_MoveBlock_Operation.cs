﻿namespace EaslyController.Writeable
{
    using System;
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// Operation details for moving a block in a block list.
    /// </summary>
    public interface IWriteableMoveBlockOperation : IWriteableOperation
    {
        /// <summary>
        /// Node where the block is moved.
        /// </summary>
        INode ParentNode { get; }

        /// <summary>
        /// Property of <see cref="ParentNode"/> where the block is moved.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Index of the moved block.
        /// </summary>
        int BlockIndex { get; }

        /// <summary>
        /// The change in position, relative to the current position.
        /// </summary>
        int Direction { get; }

        /// <summary>
        /// The moved block state.
        /// </summary>
        IWriteableBlockState BlockState { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="blockState">The moved block state.</param>
        void Update(IWriteableBlockState blockState);

        /// <summary>
        /// Creates an operation to undo the move block operation.
        /// </summary>
        IWriteableMoveBlockOperation ToInverseMoveBlock();
    }

    /// <summary>
    /// Operation details for moving a block in a block list.
    /// </summary>
    internal class WriteableMoveBlockOperation : WriteableOperation, IWriteableMoveBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableMoveBlockOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the block is moved.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where the block is moved.</param>
        /// <param name="blockIndex">Index of the moved block.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableMoveBlockOperation(INode parentNode, string propertyName, int blockIndex, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            ParentNode = parentNode;
            PropertyName = propertyName;
            BlockIndex = blockIndex;
            Direction = direction;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Node where the block is moved.
        /// </summary>
        public INode ParentNode { get; }

        /// <summary>
        /// Property of <see cref="ParentNode"/> where the block is moved.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Index of the moved block.
        /// </summary>
        public int BlockIndex { get; }

        /// <summary>
        /// The change in position, relative to the current position.
        /// </summary>
        public int Direction { get; }

        /// <summary>
        /// The moved block state.
        /// </summary>
        public IWriteableBlockState BlockState { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="blockState">The moved block state.</param>
        public virtual void Update(IWriteableBlockState blockState)
        {
            Debug.Assert(blockState != null);

            BlockState = blockState;
        }

        /// <summary>
        /// Creates an operation to undo the move block operation.
        /// </summary>
        public virtual IWriteableMoveBlockOperation ToInverseMoveBlock()
        {
            return CreateMoveBlockOperation(BlockIndex + Direction, -Direction, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxxMoveBlockOperation object.
        /// </summary>
        private protected virtual IWriteableMoveBlockOperation CreateMoveBlockOperation(int blockIndex, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableMoveBlockOperation));
            return new WriteableMoveBlockOperation(ParentNode, PropertyName, blockIndex, direction, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
