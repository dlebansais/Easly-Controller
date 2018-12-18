using EaslyController.ReadOnly;
using System;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Handlers to call during enumeration of states, when attaching a view.
    /// </summary>
    public interface IWriteableAttachCallbackSet : IReadOnlyAttachCallbackSet
    {
        new Action<IWriteableNodeState> NodeStateAttachedHandler { get; set; }
        new Action<IWriteableBlockListInner> BlockListInnerAttachedHandler { get; set; }
        new Action<IWriteableBlockState> BlockStateAttachedHandler { get; set; }
    }

    /// <summary>
    /// Handlers to call during enumeration of states, when attaching a view.
    /// </summary>
    public class WriteableAttachCallbackSet : ReadOnlyAttachCallbackSet, IWriteableAttachCallbackSet
    {
        #region Properties
        public new Action<IWriteableNodeState> NodeStateAttachedHandler { get { return base.NodeStateAttachedHandler; } set { base.NodeStateAttachedHandler = (Action<IReadOnlyNodeState>)value; } }
        public new Action<IWriteableBlockListInner> BlockListInnerAttachedHandler { get { return base.BlockListInnerAttachedHandler; } set { base.BlockListInnerAttachedHandler = (Action<IReadOnlyBlockListInner>)value; } }
        public new Action<IWriteableBlockState> BlockStateAttachedHandler { get { return base.BlockStateAttachedHandler; } set { base.BlockStateAttachedHandler = (Action<IReadOnlyBlockState>)value; } }
        #endregion
    }
}
