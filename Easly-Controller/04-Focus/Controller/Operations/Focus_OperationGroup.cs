namespace EaslyController.Focus
{
    using EaslyController.Frame;

    /// <summary>
    /// Group of operations to make some tasks atomic.
    /// </summary>
    public class FocusOperationGroup : FrameOperationGroup
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusOperationGroup"/> class.
        /// </summary>
        /// <param name="operationList">List of operations belonging to this group.</param>
        /// <param name="refresh">Optional refresh operation to execute at the end of undo and redo.</param>
        public FocusOperationGroup(FocusOperationReadOnlyList operationList, FocusGenericRefreshOperation refresh)
            : base(operationList, refresh)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// List of operations belonging to this group.
        /// </summary>
        public new FocusOperationReadOnlyList OperationList { get { return (FocusOperationReadOnlyList)base.OperationList; } }

        /// <summary>
        /// The main operation for this group.
        /// </summary>
        public new FocusOperation MainOperation { get { return (FocusOperation)base.MainOperation; } }

        /// <summary>
        /// Optional refresh operation to execute at the end of undo and redo.
        /// </summary>
        public new FocusGenericRefreshOperation Refresh { get { return (FocusGenericRefreshOperation)base.Refresh; } }
        #endregion
    }
}
