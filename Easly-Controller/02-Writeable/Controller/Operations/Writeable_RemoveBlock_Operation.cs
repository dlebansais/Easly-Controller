namespace EaslyController.Writeable
{
    using System;
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// Operation details for removing a block from a block list.
    /// </summary>
    public class WriteableRemoveBlockOperation : WriteableRemoveOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableRemoveBlockOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the block removal is taking place.</param>
        /// <param name="propertyName">Block list property of <paramref name="parentNode"/> where a block is removed.</param>
        /// <param name="blockIndex">index of the removed block.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableRemoveBlockOperation(Node parentNode, string propertyName, int blockIndex, Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            ParentNode = parentNode;
            PropertyName = propertyName;
            BlockIndex = blockIndex;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Node where the block removal is taking place.
        /// </summary>
        public Node ParentNode { get; }

        /// <summary>
        /// Block list property of <see cref="ParentNode"/> where a block is removed.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Index of the removed block.
        /// </summary>
        public int BlockIndex { get; }

        /// <summary>
        /// The removed block state.
        /// </summary>
        public IWriteableBlockState BlockState { get; private set; }

        /// <summary>
        /// The removed block.
        /// </summary>
        public IBlock Block { get; private set; }

        /// <summary>
        /// The removed state.
        /// </summary>
        public IWriteableNodeState RemovedState { get; private set; }

        /// <summary>
        /// The removed node.
        /// </summary>
        public Node Node { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="blockState">Block state removed.</param>
        /// <param name="removedState">The state removed.</param>
        public virtual void Update(IWriteableBlockState blockState, IWriteableNodeState removedState)
        {
            Debug.Assert(blockState != null);
            Debug.Assert(blockState.StateList.Count == 0);

            BlockState = blockState;
            Block = blockState.ChildBlock;
            RemovedState = removedState;
            Node = removedState.Node;
        }

        /// <summary>
        /// Creates an operation to undo the remove block operation.
        /// </summary>
        public virtual WriteableInsertBlockOperation ToInsertBlockOperation()
        {
            return CreateInsertBlockOperation(BlockIndex, Block, Node, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertBlockOperation object.
        /// </summary>
        private protected virtual WriteableInsertBlockOperation CreateInsertBlockOperation(int blockIndex, IBlock block, Node node, Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableRemoveBlockOperation));
            return new WriteableInsertBlockOperation(ParentNode, PropertyName, blockIndex, block, node, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
