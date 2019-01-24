using EaslyController.Frame;
using EaslyController.Writeable;
using System;

namespace EaslyController.Focus
{
    /// <summary>
    /// Operation details for replacing a node.
    /// </summary>
    public interface IFocusGenericRefreshOperation : IFrameGenericRefreshOperation, IFocusOperation
    {
        /// <summary>
        /// State in the source where to start refresh.
        /// </summary>
        new IFocusNodeState RefreshState { get; }
    }

    /// <summary>
    /// Operation details for replacing a node in a list or block list.
    /// </summary>
    public class FocusGenericRefreshOperation : FrameGenericRefreshOperation, IFocusGenericRefreshOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FocusGenericRefreshOperation"/>.
        /// </summary>
        /// <param name="refreshState">State in the source where to start refresh.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusGenericRefreshOperation(IFocusNodeState refreshState, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(refreshState, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// State in the source where to start refresh.
        /// </summary>
        public new IFocusNodeState RefreshState { get { return (IFocusNodeState)base.RefreshState; } }
        #endregion
    }
}
