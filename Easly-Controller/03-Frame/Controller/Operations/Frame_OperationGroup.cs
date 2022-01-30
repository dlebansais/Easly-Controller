namespace EaslyController.Frame
{
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameOperationGroup : WriteableOperationGroup
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FrameOperationGroup"/> object.
        /// </summary>
        public static new FrameOperationGroup Empty { get; } = new FrameOperationGroup();

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameOperationGroup"/> class.
        /// </summary>
        protected FrameOperationGroup()
            : this(new FrameOperationList(), FrameGenericRefreshOperation.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableOperationGroup"/> class.
        /// </summary>
        /// <param name="operationList">List of operations belonging to this group.</param>
        /// <param name="refresh">Optional refresh operation to execute at the end of undo and redo.</param>
        protected FrameOperationGroup(FrameOperationList operationList, FrameGenericRefreshOperation refresh)
            : base(operationList, refresh)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameOperationGroup"/> class.
        /// </summary>
        /// <param name="operationList">List of operations belonging to this group.</param>
        /// <param name="refresh">Optional refresh operation to execute at the end of undo and redo.</param>
        public FrameOperationGroup(FrameOperationReadOnlyList operationList, FrameGenericRefreshOperation refresh)
            : base(operationList, refresh)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// List of operations belonging to this group.
        /// </summary>
        public new FrameOperationReadOnlyList OperationList { get { return (FrameOperationReadOnlyList)base.OperationList; } }

        /// <summary>
        /// The main operation for this group.
        /// </summary>
        public new IFrameOperation MainOperation { get { return (IFrameOperation)base.MainOperation; } }

        /// <summary>
        /// Optional refresh operation to execute at the end of undo and redo.
        /// </summary>
        public new FrameGenericRefreshOperation Refresh { get { return (FrameGenericRefreshOperation)base.Refresh; } }
        #endregion
    }
}
