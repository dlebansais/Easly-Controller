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
        /// Gets the empty <see cref="WriteableGenericRefreshOperation"/> object.
        /// </summary>
        public static new WriteableGenericRefreshOperation Empty { get; } = new WriteableGenericRefreshOperation();

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableGenericRefreshOperation"/> class.
        /// </summary>
        protected WriteableGenericRefreshOperation()
            : this(WriteableNodeState<IWriteableInner<IWriteableBrowsingChildIndex>>.Empty, (IWriteableOperation operation) => { }, (IWriteableOperation operation) => { })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableGenericRefreshOperation"/> class.
        /// </summary>
        /// <param name="refreshState">State in the source where to start refresh.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        protected WriteableGenericRefreshOperation(IWriteableNodeState refreshState, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo)
            : base(handlerRedo, handlerUndo, false)
        {
            RefreshState = refreshState;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableGenericRefreshOperation"/> class.
        /// </summary>
        /// <param name="refreshState">State in the source where to start refresh.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableGenericRefreshOperation(IWriteableNodeState refreshState, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
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
