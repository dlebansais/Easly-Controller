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
        public new Action<IWriteableNodeState> NodeStateAttachedHandler { get; set; }
        public new Action<IWriteableBlockListInner> BlockListInnerAttachedHandler { get; set; }
        public new Action<IWriteableBlockState> BlockStateAttachedHandler { get; set; }
        #endregion

        #region Client Interface
        public override void OnNodeStateAttached(IReadOnlyNodeState state)
        {
            NodeStateAttachedHandler((IWriteableNodeState)state);
        }

        public override void OnBlockListInnerAttached(IReadOnlyBlockListInner inner)
        {
            BlockListInnerAttachedHandler((IWriteableBlockListInner)inner);
        }

        public override void OnBlockStateAttached(IReadOnlyBlockState blockState)
        {
            BlockStateAttachedHandler((IWriteableBlockState)blockState);
        }
        #endregion
    }
}
