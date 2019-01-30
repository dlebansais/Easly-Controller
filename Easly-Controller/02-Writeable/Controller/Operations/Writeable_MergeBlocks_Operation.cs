namespace EaslyController.Writeable
{
    using System;
    using BaseNode;

    /// <summary>
    /// Operation details for merging blocks in a block list.
    /// </summary>
    public interface IWriteableMergeBlocksOperation : IWriteableOperation
    {
        /// <summary>
        /// Node where the blocks are merged.
        /// </summary>
        INode ParentNode { get; }

        /// <summary>
        /// Property of <see cref="ParentNode"/> where blocks are merged.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Index of the block merged.
        /// </summary>
        int BlockIndex { get; }

        /// <summary>
        /// The merged block state.
        /// </summary>
        IWriteableBlockState BlockState { get; }

        /// <summary>
        /// The merged block.
        /// </summary>
        IBlock MergedBlock { get; }

        /// <summary>
        /// Position of the first node that was merged, relative to the merged block.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="blockState">The merged block state.</param>
        /// <param name="index">Position of the first node that was merged, relative to the merged block.</param>
        void Update(IWriteableBlockState blockState, int index);

        /// <summary>
        /// Creates an operation to undo the merge blocks operation.
        /// </summary>
        IWriteableSplitBlockOperation ToSplitBlockOperation();
    }

    /// <summary>
    /// Operation details for merging blocks in a block list.
    /// </summary>
    internal class WriteableMergeBlocksOperation : WriteableOperation, IWriteableMergeBlocksOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableMergeBlocksOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the blocks are merged.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where blocks are merged.</param>
        /// <param name="blockIndex">Position of the merged block.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableMergeBlocksOperation(INode parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            ParentNode = parentNode;
            PropertyName = propertyName;
            BlockIndex = blockIndex;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Node where the blocks are merged.
        /// </summary>
        public INode ParentNode { get; }

        /// <summary>
        /// Property of <see cref="ParentNode"/> where blocks are merged.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Index of the block merged.
        /// </summary>
        public int BlockIndex { get; }

        /// <summary>
        /// The merged block state.
        /// </summary>
        public IWriteableBlockState BlockState { get; private set; }

        /// <summary>
        /// The merged block.
        /// </summary>
        public IBlock MergedBlock { get; private set; }

        /// <summary>
        /// Position of the first node that was merged, relative to the merged block.
        /// </summary>
        public int Index { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="blockState">The merged block state.</param>
        /// <param name="index">Position of the first node that was merged, relative to the merged block.</param>
        public virtual void Update(IWriteableBlockState blockState, int index)
        {
            BlockState = blockState;
            MergedBlock = blockState.ChildBlock;
            Index = index;
        }

        /// <summary>
        /// Creates an operation to undo the merge blocks operation.
        /// </summary>
        public virtual IWriteableSplitBlockOperation ToSplitBlockOperation()
        {
            return CreateSplitBlockOperation(BlockIndex - 1, Index, MergedBlock, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxSplitBlockOperation object.
        /// </summary>
        private protected virtual IWriteableSplitBlockOperation CreateSplitBlockOperation(int blockIndex, int index, IBlock block, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableMergeBlocksOperation));
            return new WriteableSplitBlockOperation(ParentNode, PropertyName, blockIndex, index, block, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
