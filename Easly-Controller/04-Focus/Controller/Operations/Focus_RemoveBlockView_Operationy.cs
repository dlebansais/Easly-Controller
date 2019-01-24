using EaslyController.Frame;
using EaslyController.Writeable;
using System;

namespace EaslyController.Focus
{
    /// <summary>
    /// Operation details for removing a block from a block list.
    /// </summary>
    public interface IFocusRemoveBlockViewOperation : IFrameRemoveBlockViewOperation, IFocusRemoveOperation
    {
        /// <summary>
        /// Inner where the block removal is taking place.
        /// </summary>
        new IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Block state removed.
        /// </summary>
        new IFocusBlockState BlockState { get; }
    }

    /// <summary>
    /// Operation details for removing a block from a block list.
    /// </summary>
    public class FocusRemoveBlockViewOperation : FrameRemoveBlockViewOperation, IFocusRemoveBlockViewOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FocusRemoveBlockViewOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block removal is taking place.</param>
        /// <param name="blockIndex">index of the removed block.</param>
        /// <param name="cleanupBlockList">True if the block list should be cleared.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusRemoveBlockViewOperation(IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> inner, int blockIndex, bool cleanupBlockList, Action<IWriteableOperation> handlerRedo, bool isNested)
            : base(inner, blockIndex, cleanupBlockList, handlerRedo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the block removal is taking place.
        /// </summary>
        public new IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> Inner { get { return (IFocusBlockListInner<IFocusBrowsingBlockNodeIndex>)base.Inner; } }

        /// <summary>
        /// Block state removed.
        /// </summary>
        public new IFocusBlockState BlockState { get { return (IFocusBlockState)base.BlockState; } }
        #endregion
    }
}
