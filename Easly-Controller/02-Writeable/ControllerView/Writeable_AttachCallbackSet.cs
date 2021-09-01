namespace EaslyController.Writeable
{
    using System;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Handlers to call during enumeration of states, when attaching a view.
    /// </summary>
    internal class WriteableAttachCallbackSet : ReadOnlyAttachCallbackSet
    {
        #region Properties
        /// <summary>
        /// Handler to call when attaching a state.
        /// </summary>
        public new Action<IWriteableNodeState> NodeStateAttachedHandler { get; set; }

        /// <summary>
        /// Handler to call when detaching a state.
        /// </summary>
        public new Action<IWriteableNodeState> NodeStateDetachedHandler { get; set; }

        /// <summary>
        /// Handler to call when attaching a block list inner.
        /// </summary>
        public new Action<IWriteableBlockListInner> BlockListInnerAttachedHandler { get; set; }

        /// <summary>
        /// Handler to call when detaching a block list inner.
        /// </summary>
        public new Action<IWriteableBlockListInner> BlockListInnerDetachedHandler { get; set; }

        /// <summary>
        /// Handler to call when attaching a block state.
        /// </summary>
        public new Action<IWriteableBlockState> BlockStateAttachedHandler { get; set; }

        /// <summary>
        /// Handler to call when detaching a block state.
        /// </summary>
        public new Action<IWriteableBlockState> BlockStateDetachedHandler { get; set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// A state has been attached.
        /// </summary>
        /// <param name="state">The attached state.</param>
        public override void OnNodeStateAttached(IReadOnlyNodeState state)
        {
            NodeStateAttachedHandler((IWriteableNodeState)state);
        }

        /// <summary>
        /// A state has been detached.
        /// </summary>
        /// <param name="state">The detached state.</param>
        public override void OnNodeStateDetached(IReadOnlyNodeState state)
        {
            NodeStateDetachedHandler((IWriteableNodeState)state);
        }

        /// <summary>
        /// A block list inner has been attached.
        /// </summary>
        /// <param name="inner">The inner attached.</param>
        public override void OnBlockListInnerAttached(IReadOnlyBlockListInner inner)
        {
            BlockListInnerAttachedHandler((IWriteableBlockListInner)inner);
        }

        /// <summary>
        /// A block list inner has been detached.
        /// </summary>
        /// <param name="inner">The inner detached.</param>
        public override void OnBlockListInnerDetached(IReadOnlyBlockListInner inner)
        {
            BlockListInnerDetachedHandler((IWriteableBlockListInner)inner);
        }

        /// <summary>
        /// A block state has been attached.
        /// </summary>
        /// <param name="blockState">The attached block state.</param>
        public override void OnBlockStateAttached(IReadOnlyBlockState blockState)
        {
            BlockStateAttachedHandler((IWriteableBlockState)blockState);
        }

        /// <summary>
        /// A block state has been detached.
        /// </summary>
        /// <param name="blockState">The detached block state.</param>
        public override void OnBlockStateDetached(IReadOnlyBlockState blockState)
        {
            BlockStateDetachedHandler((IWriteableBlockState)blockState);
        }
        #endregion
    }
}
