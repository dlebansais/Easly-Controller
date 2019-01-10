using EaslyController.ReadOnly;
using EaslyController.Frame;
using System;

namespace EaslyController.Focus
{
    /// <summary>
    /// Handlers to call during enumeration of states, when attaching a view.
    /// </summary>
    public interface IFocusAttachCallbackSet : IFrameAttachCallbackSet
    {
    }

    /// <summary>
    /// Handlers to call during enumeration of states, when attaching a view.
    /// </summary>
    public class FocusAttachCallbackSet : FrameAttachCallbackSet, IFocusAttachCallbackSet
    {
        #region Properties
        /// <summary>
        /// Handler to call when attaching a state.
        /// </summary>
        public new Action<IFocusNodeState> NodeStateAttachedHandler { get; set; }

        /// <summary>
        /// Handler to call when detaching a state.
        /// </summary>
        public new Action<IFocusNodeState> NodeStateDetachedHandler { get; set; }

        /// <summary>
        /// Handler to call when attaching a block list inner.
        /// </summary>
        public new Action<IFocusBlockListInner> BlockListInnerAttachedHandler { get; set; }

        /// <summary>
        /// Handler to call when detaching a block list inner.
        /// </summary>
        public new Action<IFocusBlockListInner> BlockListInnerDetachedHandler { get; set; }

        /// <summary>
        /// Handler to call when attaching a block state.
        /// </summary>
        public new Action<IFocusBlockState> BlockStateAttachedHandler { get; set; }

        /// <summary>
        /// Handler to call when detaching a block state.
        /// </summary>
        public new Action<IFocusBlockState> BlockStateDetachedHandler { get; set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// A state has been attached.
        /// </summary>
        /// <param name="state">The attached state.</param>
        public override void OnNodeStateAttached(IReadOnlyNodeState state)
        {
            NodeStateAttachedHandler((IFocusNodeState)state);
        }

        /// <summary>
        /// A state has been detached.
        /// </summary>
        /// <param name="state">The detached state.</param>
        public override void OnNodeStateDetached(IReadOnlyNodeState state)
        {
            NodeStateDetachedHandler((IFocusNodeState)state);
        }

        /// <summary>
        /// A block list inner has been attached.
        /// </summary>
        /// <param name="inner">The inner attached.</param>
        public override void OnBlockListInnerAttached(IReadOnlyBlockListInner inner)
        {
            BlockListInnerAttachedHandler((IFocusBlockListInner)inner);
        }

        /// <summary>
        /// A block list inner has been detached.
        /// </summary>
        /// <param name="inner">The inner detached.</param>
        public override void OnBlockListInnerDetached(IReadOnlyBlockListInner inner)
        {
            BlockListInnerDetachedHandler((IFocusBlockListInner)inner);
        }

        /// <summary>
        /// A block state has been attached.
        /// </summary>
        /// <param name="blockState">The attached block state.</param>
        public override void OnBlockStateAttached(IReadOnlyBlockState blockState)
        {
            BlockStateAttachedHandler((IFocusBlockState)blockState);
        }

        /// <summary>
        /// A block state has been detached.
        /// </summary>
        /// <param name="blockState">The detached block state.</param>
        public override void OnBlockStateDetached(IReadOnlyBlockState blockState)
        {
            BlockStateDetachedHandler((IFocusBlockState)blockState);
        }
        #endregion
    }
}
