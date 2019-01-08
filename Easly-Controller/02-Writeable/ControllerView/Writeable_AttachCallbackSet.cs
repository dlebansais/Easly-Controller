using EaslyController.ReadOnly;
using System;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Handlers to call during enumeration of states, when attaching a view.
    /// </summary>
    public interface IWriteableAttachCallbackSet : IReadOnlyAttachCallbackSet
    {
    }

    /// <summary>
    /// Handlers to call during enumeration of states, when attaching a view.
    /// </summary>
    public class WriteableAttachCallbackSet : ReadOnlyAttachCallbackSet, IWriteableAttachCallbackSet
    {
        #region Properties
        /// <summary>
        /// Handler to call when attaching a state.
        /// </summary>
        public new Action<IWriteableNodeState> NodeStateAttachedHandler { get; set; }

        /// <summary>
        /// Handler to call when attaching a block list inner.
        /// </summary>
        public new Action<IWriteableBlockListInner> BlockListInnerAttachedHandler { get; set; }

        /// <summary>
        /// Handler to call when attaching a block state.
        /// </summary>
        public new Action<IWriteableBlockState> BlockStateAttachedHandler { get; set; }
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
        /// A block list inner has been attached.
        /// </summary>
        /// <param name="inner">The inner attached.</param>
        public override void OnBlockListInnerAttached(IReadOnlyBlockListInner inner)
        {
            BlockListInnerAttachedHandler((IWriteableBlockListInner)inner);
        }

        /// <summary>
        /// A block state has been attached.
        /// </summary>
        /// <param name="blockState">The attached block state.</param>
        public override void OnBlockStateAttached(IReadOnlyBlockState blockState)
        {
            BlockStateAttachedHandler((IWriteableBlockState)blockState);
        }
        #endregion
    }
}
