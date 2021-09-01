namespace EaslyController.Writeable
{
    using System;

    /// <summary>
    /// Operation details for refreshing views.
    /// </summary>
    public class WriteableGenericRefreshOperation : WriteableOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableGenericRefreshOperation"/> class.
        /// </summary>
        /// <param name="refreshState">State in the source where to start refresh.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableGenericRefreshOperation(IWriteableNodeState refreshState, Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            RefreshState = refreshState;
        }
        #endregion

        #region Properties
        /// <summary>
        /// State in the source where to start refresh.
        /// </summary>
        public IWriteableNodeState RefreshState { get; }
        #endregion
    }
}
