namespace EaslyController.Writeable
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using Contracts;

    /// <summary>
    /// Operation details for splitting a block in a block list.
    /// </summary>
    public class WriteableSplitBlockOperation : WriteableOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableSplitBlockOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the block is split.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where the block is split.</param>
        /// <param name="blockIndex">Position of the split block.</param>
        /// <param name="index">Position of the last node to stay in the old block.</param>
        /// <param name="newBlock">The inserted block.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableSplitBlockOperation(Node parentNode, string propertyName, int blockIndex, int index, IBlock newBlock, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            ParentNode = parentNode;
            PropertyName = propertyName;
            BlockIndex = blockIndex;
            Index = index;
            NewBlock = newBlock;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Node where the block is split.
        /// </summary>
        public Node ParentNode { get; }

        /// <summary>
        /// Property of <see cref="ParentNode"/> where the block is split.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Position of the split block.
        /// </summary>
        public int BlockIndex { get; }

        /// <summary>
        /// Position of the last node to stay in the old block.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// The inserted block.
        /// </summary>
        public IBlock NewBlock { get; }

        /// <summary>
        /// The inserted block state.
        /// </summary>
        public IWriteableBlockState BlockState { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="blockState">Block state inserted.</param>
        public virtual void Update(IWriteableBlockState blockState)
        {
            Contract.RequireNotNull(blockState, out IWriteableBlockState BlockState);
            Debug.Assert(BlockState.ChildBlock == NewBlock);

            this.BlockState = BlockState;
        }

        /// <summary>
        /// Creates an operation to undo the split block operation.
        /// </summary>
        public virtual WriteableMergeBlocksOperation ToMergeBlocksOperation()
        {
            return CreateMergeBlocksOperation(BlockIndex + 1, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxxMergeBlocksOperation object.
        /// </summary>
        private protected virtual WriteableMergeBlocksOperation CreateMergeBlocksOperation(int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableSplitBlockOperation));
            return new WriteableMergeBlocksOperation(ParentNode, PropertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
