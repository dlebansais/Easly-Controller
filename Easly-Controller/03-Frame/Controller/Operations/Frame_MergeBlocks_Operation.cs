using EaslyController.Writeable;
using System;

namespace EaslyController.Frame
{
    /// <summary>
    /// Operation details for merging blocks in a block list.
    /// </summary>
    public interface IFrameMergeBlocksOperation : IWriteableMergeBlocksOperation, IFrameOperation
    {
        /// <summary>
        /// Inner where blocks are merged.
        /// </summary>
        new IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the first node in the merged block.
        /// </summary>
        new IFrameBrowsingExistingBlockNodeIndex NodeIndex { get; }
    }

    /// <summary>
    /// Operation details for merging blocks in a block list.
    /// </summary>
    public class FrameMergeBlocksOperation : WriteableMergeBlocksOperation, IFrameMergeBlocksOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FrameMergeBlocksOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block is split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameMergeBlocksOperation(IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> inner, IFrameBrowsingExistingBlockNodeIndex nodeIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(inner, nodeIndex, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where blocks are merged.
        /// </summary>
        public new IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> Inner { get { return (IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)base.Inner; } }

        /// <summary>
        /// Index of the first node in the merged block.
        /// </summary>
        public new IFrameBrowsingExistingBlockNodeIndex NodeIndex { get { return (IFrameBrowsingExistingBlockNodeIndex)base.NodeIndex; } }

        /// <summary>
        /// Block state inserted.
        /// </summary>
        public new IFrameBlockState BlockState { get { return (IFrameBlockState)base.BlockState; } }
        #endregion
    }
}
