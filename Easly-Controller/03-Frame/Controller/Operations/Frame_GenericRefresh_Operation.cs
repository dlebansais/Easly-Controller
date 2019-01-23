using EaslyController.Writeable;
using System;

namespace EaslyController.Frame
{
    /// <summary>
    /// Operation details for replacing a node.
    /// </summary>
    public interface IFrameGenericRefreshOperation : IWriteableGenericRefreshOperation, IFrameOperation
    {
        /// <summary>
        /// State in the source where to start refresh.
        /// </summary>
        new IFrameNodeState RefreshState { get; }
    }

    /// <summary>
    /// Operation details for replacing a node in a list or block list.
    /// </summary>
    public class FrameGenericRefreshOperation : WriteableGenericRefreshOperation, IFrameGenericRefreshOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FrameGenericRefreshOperation"/>.
        /// </summary>
        /// <param name="refreshState">State in the source where to start refresh.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameGenericRefreshOperation(IFrameNodeState refreshState, Action<IWriteableOperation> handlerRedo, bool isNested)
            : base(refreshState, handlerRedo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// State in the source where to start refresh.
        /// </summary>
        public new IFrameNodeState RefreshState { get { return (IFrameNodeState)base.RefreshState; } }
        #endregion
    }
}
