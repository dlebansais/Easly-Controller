using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System;

namespace EaslyController.Frame
{
    /// <summary>
    /// Handlers to call during enumeration of states, when attaching a view.
    /// </summary>
    public interface IFrameAttachCallbackSet : IWriteableAttachCallbackSet
    {
    }

    /// <summary>
    /// Handlers to call during enumeration of states, when attaching a view.
    /// </summary>
    public class FrameAttachCallbackSet : WriteableAttachCallbackSet, IFrameAttachCallbackSet
    {
        #region Properties
        /// <summary>
        /// Handler to call when attaching a state.
        /// </summary>
        public new Action<IFrameNodeState> NodeStateAttachedHandler { get; set; }

        /// <summary>
        /// Handler to call when detaching a state.
        /// </summary>
        public new Action<IFrameNodeState> NodeStateDetachedHandler { get; set; }

        /// <summary>
        /// Handler to call when attaching a block list inner.
        /// </summary>
        public new Action<IFrameBlockListInner> BlockListInnerAttachedHandler { get; set; }

        /// <summary>
        /// Handler to call when detaching a block list inner.
        /// </summary>
        public new Action<IFrameBlockListInner> BlockListInnerDetachedHandler { get; set; }

        /// <summary>
        /// Handler to call when attaching a block state.
        /// </summary>
        public new Action<IFrameBlockState> BlockStateAttachedHandler { get; set; }

        /// <summary>
        /// Handler to call when detaching a block state.
        /// </summary>
        public new Action<IFrameBlockState> BlockStateDetachedHandler { get; set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// A state has been attached.
        /// </summary>
        /// <param name="state">The attached state.</param>
        public override void OnNodeStateAttached(IReadOnlyNodeState state)
        {
            NodeStateAttachedHandler((IFrameNodeState)state);
        }

        /// <summary>
        /// A state has been detached.
        /// </summary>
        /// <param name="state">The detached state.</param>
        public override void OnNodeStateDetached(IReadOnlyNodeState state)
        {
            NodeStateDetachedHandler((IFrameNodeState)state);
        }

        /// <summary>
        /// A block list inner has been attached.
        /// </summary>
        /// <param name="inner">The inner attached.</param>
        public override void OnBlockListInnerAttached(IReadOnlyBlockListInner inner)
        {
            BlockListInnerAttachedHandler((IFrameBlockListInner)inner);
        }

        /// <summary>
        /// A block list inner has been detached.
        /// </summary>
        /// <param name="inner">The inner detached.</param>
        public override void OnBlockListInnerDetached(IReadOnlyBlockListInner inner)
        {
            BlockListInnerDetachedHandler((IFrameBlockListInner)inner);
        }

        /// <summary>
        /// A block state has been attached.
        /// </summary>
        /// <param name="blockState">The attached block state.</param>
        public override void OnBlockStateAttached(IReadOnlyBlockState blockState)
        {
            BlockStateAttachedHandler((IFrameBlockState)blockState);
        }

        /// <summary>
        /// A block state has been detached.
        /// </summary>
        /// <param name="blockState">The detached block state.</param>
        public override void OnBlockStateDetached(IReadOnlyBlockState blockState)
        {
            BlockStateDetachedHandler((IFrameBlockState)blockState);
        }
        #endregion
    }
}
