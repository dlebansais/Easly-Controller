namespace EaslyController.Frame
{
    using EaslyController.Writeable;

    /// <summary>
    /// Group of operations to make some tasks atomic.
    /// </summary>
    public interface IFrameOperationGroup : IWriteableOperationGroup
    {
        /// <summary>
        /// List of operations belonging to this group.
        /// </summary>
        new IFrameOperationReadOnlyList OperationList { get; }

        /// <summary>
        /// The main operation for this group.
        /// </summary>
        new IFrameOperation MainOperation { get; }

        /// <summary>
        /// Optional refresh operation to execute at the end of undo and redo.
        /// </summary>
        new IFrameGenericRefreshOperation Refresh { get; }
    }

    /// <summary>
    /// Group of operations to make some tasks atomic.
    /// </summary>
    internal class FrameOperationGroup : WriteableOperationGroup, IFrameOperationGroup
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameOperationGroup"/> class.
        /// </summary>
        /// <param name="operationList">List of operations belonging to this group.</param>
        /// <param name="refresh">Optional refresh operation to execute at the end of undo and redo.</param>
        public FrameOperationGroup(IFrameOperationReadOnlyList operationList, IFrameGenericRefreshOperation refresh)
            : base(operationList, refresh)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// List of operations belonging to this group.
        /// </summary>
        public new IFrameOperationReadOnlyList OperationList { get { return (IFrameOperationReadOnlyList)base.OperationList; } }

        /// <summary>
        /// The main operation for this group.
        /// </summary>
        public new IFrameOperation MainOperation { get { return (IFrameOperation)base.MainOperation; } }

        /// <summary>
        /// Optional refresh operation to execute at the end of undo and redo.
        /// </summary>
        public new IFrameGenericRefreshOperation Refresh { get { return (IFrameGenericRefreshOperation)base.Refresh; } }
        #endregion
    }
}
