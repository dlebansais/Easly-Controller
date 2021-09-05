namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Group of operations to make some tasks atomic.
    /// </summary>
    internal class LayoutOperationGroup : FocusOperationGroup
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutOperationGroup"/> class.
        /// </summary>
        /// <param name="operationList">List of operations belonging to this group.</param>
        /// <param name="refresh">Optional refresh operation to execute at the end of undo and redo.</param>
        public LayoutOperationGroup(LayoutOperationReadOnlyList operationList, LayoutGenericRefreshOperation refresh)
            : base(operationList, refresh)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// List of operations belonging to this group.
        /// </summary>
        public new LayoutOperationReadOnlyList OperationList { get { return (LayoutOperationReadOnlyList)base.OperationList; } }

        /// <summary>
        /// The main operation for this group.
        /// </summary>
        public new LayoutOperation MainOperation { get { return (LayoutOperation)base.MainOperation; } }

        /// <summary>
        /// Optional refresh operation to execute at the end of undo and redo.
        /// </summary>
        public new LayoutGenericRefreshOperation Refresh { get { return (LayoutGenericRefreshOperation)base.Refresh; } }
        #endregion
    }
}
