using EaslyController.Writeable;
using System;

namespace EaslyController.Frame
{
    /// <summary>
    /// Operation details for removing a block from a block list.
    /// </summary>
    public interface IFrameRemoveBlockOperation : IWriteableRemoveBlockOperation, IFrameRemoveOperation
    {
        /// <summary>
        /// Inner where the block removal is taking place.
        /// </summary>
        new IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the removed block.
        /// </summary>
        new IFrameBrowsingExistingBlockNodeIndex BlockIndex { get; }

        /// <summary>
        /// Block state removed.
        /// </summary>
        new IFrameBlockState BlockState { get; }
    }

    /// <summary>
    /// Operation details for removing a block from a block list.
    /// </summary>
    public class FrameRemoveBlockOperation : WriteableRemoveBlockOperation, IFrameRemoveBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FrameRemoveBlockOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block removal is taking place.</param>
        /// <param name="blockIndex">index of the removed block.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameRemoveBlockOperation(IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> inner, IFrameBrowsingExistingBlockNodeIndex blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(inner, blockIndex, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the block removal is taking place.
        /// </summary>
        public new IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> Inner { get { return (IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)base.Inner; } }

        /// <summary>
        /// Index of the removed block.
        /// </summary>
        public new IFrameBrowsingExistingBlockNodeIndex BlockIndex { get { return (IFrameBrowsingExistingBlockNodeIndex)base.BlockIndex; } }

        /// <summary>
        /// Block state removed.
        /// </summary>
        public new IFrameBlockState BlockState { get { return (IFrameBlockState)base.BlockState; } }
        #endregion
    }
}
