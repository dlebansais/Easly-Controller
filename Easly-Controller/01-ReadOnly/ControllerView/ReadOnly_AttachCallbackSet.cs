using System;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyAttachCallbackSet
    {
        Action<IReadOnlyNodeState> NodeStateAttachedHandler { get; }
        Action<IReadOnlyBlockListInner> BlockListInnerAttachedHandler { get; }
        Action<IReadOnlyBlockState> BlockStateAttachedHandler { get; }
    }

    public class ReadOnlyAttachCallbackSet : IReadOnlyAttachCallbackSet
    {
        public Action<IReadOnlyNodeState> NodeStateAttachedHandler { get; set; }
        public Action<IReadOnlyBlockListInner> BlockListInnerAttachedHandler { get; set; }
        public Action<IReadOnlyBlockState> BlockStateAttachedHandler { get; set; }
    }
}
