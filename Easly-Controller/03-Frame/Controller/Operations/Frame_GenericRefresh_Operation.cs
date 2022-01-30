namespace EaslyController.Frame
{
    using System;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameGenericRefreshOperation : WriteableGenericRefreshOperation, IFrameOperation
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FrameGenericRefreshOperation"/> object.
        /// </summary>
        public static new FrameGenericRefreshOperation Empty { get; } = new FrameGenericRefreshOperation();

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameGenericRefreshOperation"/> class.
        /// </summary>
        protected FrameGenericRefreshOperation()
            : this(FrameNodeState<IFrameInner<IFrameBrowsingChildIndex>>.Empty, (IWriteableOperation operation) => { }, (IWriteableOperation operation) => { })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameGenericRefreshOperation"/> class.
        /// </summary>
        /// <param name="refreshState">State in the source where to start refresh.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        protected FrameGenericRefreshOperation(IFrameNodeState refreshState, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo)
            : base(refreshState, handlerRedo, handlerUndo)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameGenericRefreshOperation"/> class.
        /// </summary>
        /// <param name="refreshState">State in the source where to start refresh.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameGenericRefreshOperation(IFrameNodeState refreshState, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(refreshState, handlerRedo, handlerUndo, isNested)
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
