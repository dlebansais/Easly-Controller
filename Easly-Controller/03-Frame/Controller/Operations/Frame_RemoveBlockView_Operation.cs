using EaslyController.Writeable;
using System;

namespace EaslyController.Frame
{
    /// <summary>
    /// Operation details for removing a block from a block list.
    /// </summary>
    public interface IFrameRemoveBlockViewOperation : IWriteableRemoveBlockViewOperation, IFrameRemoveOperation
    {
        /// <summary>
        /// Inner where the block removal is taking place.
        /// </summary>
        new IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Block state removed.
        /// </summary>
        new IFrameBlockState BlockState { get; }
    }

    /// <summary>
    /// Operation details for removing a block from a block list.
    /// </summary>
    public class FrameRemoveBlockViewOperation : WriteableRemoveBlockViewOperation, IFrameRemoveBlockViewOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FrameRemoveBlockViewOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block removal is taking place.</param>
        /// <param name="blockIndex">index of the removed block.</param>
        /// <param name="cleanupBlockList">True if the block list should be cleared.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameRemoveBlockViewOperation(IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> inner, int blockIndex, bool cleanupBlockList, Action<IWriteableOperation> handlerRedo, bool isNested)
            : base(inner, blockIndex, cleanupBlockList, handlerRedo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the block removal is taking place.
        /// </summary>
        public new IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> Inner { get { return (IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)base.Inner; } }

        /// <summary>
        /// Block state removed.
        /// </summary>
        public new IFrameBlockState BlockState { get { return (IFrameBlockState)base.BlockState; } }
        #endregion
    }
}
