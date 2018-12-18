using System;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Handlers to call during enumeration of states, when attaching a view.
    /// </summary>
    public interface IReadOnlyAttachCallbackSet
    {
        void OnNodeStateAttached(IReadOnlyNodeState state);
        void OnBlockListInnerAttached(IReadOnlyBlockListInner inner);
        void OnBlockStateAttached(IReadOnlyBlockState blockState);
    }

    /// <summary>
    /// Handlers to call during enumeration of states, when attaching a view.
    /// </summary>
    public class ReadOnlyAttachCallbackSet : IReadOnlyAttachCallbackSet
    {
        #region Properties
        public Action<IReadOnlyNodeState> NodeStateAttachedHandler { get; set; }
        public Action<IReadOnlyBlockListInner> BlockListInnerAttachedHandler { get; set; }
        public Action<IReadOnlyBlockState> BlockStateAttachedHandler { get; set; }
        #endregion

        #region Client Interface
        public virtual void OnNodeStateAttached(IReadOnlyNodeState state)
        {
            NodeStateAttachedHandler(state);
        }

        public virtual void OnBlockListInnerAttached(IReadOnlyBlockListInner inner)
        {
            BlockListInnerAttachedHandler(inner);
        }

        public virtual void OnBlockStateAttached(IReadOnlyBlockState blockState)
        {
            BlockStateAttachedHandler(blockState);
        }
        #endregion
    }
}
