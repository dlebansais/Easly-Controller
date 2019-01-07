using System;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Handlers to call during enumeration of states, when attaching a view.
    /// </summary>
    public interface IReadOnlyAttachCallbackSet
    {
        /// <summary>
        /// Handler to call when attaching a state.
        /// </summary>
        /// <param name="state">The attached state.</param>
        void OnNodeStateAttached(IReadOnlyNodeState state);

        /// <summary>
        /// Handler to call when attaching a block list inner.
        /// </summary>
        /// <param name="inner">The inner attached.</param>
        void OnBlockListInnerAttached(IReadOnlyBlockListInner inner);

        /// <summary>
        /// Handler to call when attaching a block state.
        /// </summary>
        /// <param name="blockState">The attached block state.</param>
        void OnBlockStateAttached(IReadOnlyBlockState blockState);
    }

    /// <summary>
    /// Handlers to call during enumeration of states, when attaching a view.
    /// </summary>
    public class ReadOnlyAttachCallbackSet : IReadOnlyAttachCallbackSet
    {
        #region Properties
        /// <summary>
        /// Handler to call when attaching a state.
        /// </summary>
        public Action<IReadOnlyNodeState> NodeStateAttachedHandler { get; set; }

        /// <summary>
        /// Handler to call when attaching a block list inner.
        /// </summary>
        public Action<IReadOnlyBlockListInner> BlockListInnerAttachedHandler { get; set; }

        /// <summary>
        /// Handler to call when attaching a block state.
        /// </summary>
        public Action<IReadOnlyBlockState> BlockStateAttachedHandler { get; set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// A state has been attached.
        /// </summary>
        /// <param name="state">The attached state.</param>
        public virtual void OnNodeStateAttached(IReadOnlyNodeState state)
        {
            NodeStateAttachedHandler(state);
        }

        /// <summary>
        /// A block list inner has been attached.
        /// </summary>
        /// <param name="inner">The inner attached.</param>
        public virtual void OnBlockListInnerAttached(IReadOnlyBlockListInner inner)
        {
            BlockListInnerAttachedHandler(inner);
        }

        /// <summary>
        /// A block state has been attached.
        /// </summary>
        /// <param name="blockState">The attached block state.</param>
        public virtual void OnBlockStateAttached(IReadOnlyBlockState blockState)
        {
            BlockStateAttachedHandler(blockState);
        }
        #endregion
    }
}
