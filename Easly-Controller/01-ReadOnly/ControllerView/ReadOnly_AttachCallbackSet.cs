using System;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Handlers to call during enumeration of states, when attaching a view.
    /// </summary>
    public interface IReadOnlyAttachCallbackSet
    {
        Action<IReadOnlyNodeState> NodeStateAttachedHandler { get; }
        Action<IReadOnlyBlockListInner> BlockListInnerAttachedHandler { get; }
        Action<IReadOnlyBlockState> BlockStateAttachedHandler { get; }
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
    }
}
