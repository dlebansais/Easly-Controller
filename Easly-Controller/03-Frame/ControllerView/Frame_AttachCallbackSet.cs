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
        public new Action<IFrameNodeState> NodeStateAttachedHandler { get; set; }
        public new Action<IFrameBlockListInner> BlockListInnerAttachedHandler { get; set; }
        public new Action<IFrameBlockState> BlockStateAttachedHandler { get; set; }
        #endregion

        #region Client Interface
        public override void OnNodeStateAttached(IReadOnlyNodeState state)
        {
            NodeStateAttachedHandler((IFrameNodeState)state);
        }

        public override void OnBlockListInnerAttached(IReadOnlyBlockListInner inner)
        {
            BlockListInnerAttachedHandler((IFrameBlockListInner)inner);
        }

        public override void OnBlockStateAttached(IReadOnlyBlockState blockState)
        {
            BlockStateAttachedHandler((IFrameBlockState)blockState);
        }
        #endregion
    }
}
