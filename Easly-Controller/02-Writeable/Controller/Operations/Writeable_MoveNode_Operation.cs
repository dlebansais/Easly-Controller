namespace EaslyController.Writeable
{
    using System;
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// Operation details for moving a node in a list or block list.
    /// </summary>
    public class WriteableMoveNodeOperation : WriteableOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableMoveNodeOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the node is moved.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where the node is moved.</param>
        /// <param name="blockIndex">Block position where the node is moved, if applicable.</param>
        /// <param name="index">The current position before move.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableMoveNodeOperation(Node parentNode, string propertyName, int blockIndex, int index, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            ParentNode = parentNode;
            PropertyName = propertyName;
            BlockIndex = blockIndex;
            Index = index;
            Direction = direction;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Node where the node is moved.
        /// </summary>
        public Node ParentNode { get; }

        /// <summary>
        /// Property of <see cref="ParentNode"/> where the node is moved.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Block position where the node is moved, if applicable.
        /// </summary>
        public int BlockIndex { get; }

        /// <summary>
        /// The current position before move.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// The change in position, relative to the current position.
        /// </summary>
        public int Direction { get; }

        /// <summary>
        /// State moved.
        /// </summary>
        public IWriteablePlaceholderNodeState State { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="state">State moved.</param>
        public virtual void Update(IWriteablePlaceholderNodeState state)
        {
            Debug.Assert(state != null);

            State = state;
        }

        /// <summary>
        /// Creates an operation to undo the move operation.
        /// </summary>
        public virtual WriteableMoveNodeOperation ToInverseMove()
        {
            return CreateMoveNodeOperation(BlockIndex, Index + Direction, -Direction, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxxMoveNodeOperation object.
        /// </summary>
        private protected virtual WriteableMoveNodeOperation CreateMoveNodeOperation(int blockIndex, int index, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableMoveNodeOperation));
            return new WriteableMoveNodeOperation(ParentNode, PropertyName, blockIndex, index, direction, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
