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
        /// Handler to call when attaching a block list inner.
        /// </summary>
        public new Action<IFrameBlockListInner> BlockListInnerAttachedHandler { get; set; }

        /// <summary>
        /// Handler to call when attaching a block state.
        /// </summary>
        public new Action<IFrameBlockState> BlockStateAttachedHandler { get; set; }
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
        /// A block list inner has been attached.
        /// </summary>
        /// <param name="inner">The inner attached.</param>
        public override void OnBlockListInnerAttached(IReadOnlyBlockListInner inner)
        {
            BlockListInnerAttachedHandler((IFrameBlockListInner)inner);
        }

        /// <summary>
        /// A block state has been attached.
        /// </summary>
        /// <param name="blockState">The attached block state.</param>
        public override void OnBlockStateAttached(IReadOnlyBlockState blockState)
        {
            BlockStateAttachedHandler((IFrameBlockState)blockState);
        }
        #endregion
    }
}
