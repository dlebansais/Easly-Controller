using BaseNode;
using System;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Operation details for merging blocks in a block list.
    /// </summary>
    public interface IWriteableMergeBlocksOperation : IWriteableOperation
    {
        /// <summary>
        /// Inner where blocks are merged.
        /// </summary>
        IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the first node in the merged block.
        /// </summary>
        IWriteableBrowsingExistingBlockNodeIndex NodeIndex { get; }

        /// <summary>
        /// Index of the block merged.
        /// </summary>
        int BlockIndex { get; }

        /// <summary>
        /// The merged block.
        /// </summary>
        IBlock MergedBlock { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="mergedBlock">The merged block.</param>
        void Update(IBlock mergedBlock);

        /// <summary>
        /// Creates an operation to undo the merge blocks operation.
        /// </summary>
        IWriteableSplitBlockOperation ToSplitBlockOperation();
    }

    /// <summary>
    /// Operation details for merging blocks in a block list.
    /// </summary>
    public class WriteableMergeBlocksOperation : WriteableOperation, IWriteableMergeBlocksOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableMergeBlocksOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block is split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableMergeBlocksOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            Inner = inner;
            NodeIndex = nodeIndex;
            BlockIndex = nodeIndex.BlockIndex;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where blocks are merged.
        /// </summary>
        public IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the first node in the merged block.
        /// </summary>
        public IWriteableBrowsingExistingBlockNodeIndex NodeIndex { get; }

        /// <summary>
        /// Index of the block split.
        /// </summary>
        public int BlockIndex { get; }

        /// <summary>
        /// The merged block.
        /// </summary>
        public IBlock MergedBlock { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="mergedBlock">The merged block.</param>
        public virtual void Update(IBlock mergedBlock)
        {
            MergedBlock = mergedBlock;
        }

        /// <summary>
        /// Creates an operation to undo the merge blocks operation.
        /// </summary>
        public virtual IWriteableSplitBlockOperation ToSplitBlockOperation()
        {
            return CreateSplitBlockOperation(Inner, NodeIndex, MergedBlock, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxSplitBlockOperation object.
        /// </summary>
        protected virtual IWriteableSplitBlockOperation CreateSplitBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex, IBlock newBlock, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableMergeBlocksOperation));
            return new WriteableSplitBlockOperation(inner, nodeIndex, newBlock, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
