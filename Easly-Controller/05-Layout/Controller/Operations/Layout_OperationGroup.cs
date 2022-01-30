namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Group of operations to make some tasks atomic.
    /// </summary>
    public class LayoutOperationGroup : FocusOperationGroup
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="LayoutOperationGroup"/> object.
        /// </summary>
        public static new LayoutOperationGroup Empty { get; } = new LayoutOperationGroup();

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutOperationGroup"/> class.
        /// </summary>
        protected LayoutOperationGroup()
            : this(new LayoutOperationList(), LayoutGenericRefreshOperation.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutOperationGroup"/> class.
        /// </summary>
        /// <param name="operationList">List of operations belonging to this group.</param>
        /// <param name="refresh">Optional refresh operation to execute at the end of undo and redo.</param>
        protected LayoutOperationGroup(LayoutOperationList operationList, LayoutGenericRefreshOperation refresh)
            : base(operationList, refresh)
        {
        }

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
        public new ILayoutOperation MainOperation { get { return (ILayoutOperation)base.MainOperation; } }

        /// <summary>
        /// Optional refresh operation to execute at the end of undo and redo.
        /// </summary>
        public new LayoutGenericRefreshOperation Refresh { get { return (LayoutGenericRefreshOperation)base.Refresh; } }
        #endregion
    }
}
