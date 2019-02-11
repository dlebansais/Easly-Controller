namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Group of operations to make some tasks atomic.
    /// </summary>
    public interface ILayoutOperationGroup : IFocusOperationGroup
    {
        /// <summary>
        /// List of operations belonging to this group.
        /// </summary>
        new ILayoutOperationReadOnlyList OperationList { get; }

        /// <summary>
        /// The main operation for this group.
        /// </summary>
        new ILayoutOperation MainOperation { get; }

        /// <summary>
        /// Optional refresh operation to execute at the end of undo and redo.
        /// </summary>
        new ILayoutGenericRefreshOperation Refresh { get; }
    }

    /// <summary>
    /// Group of operations to make some tasks atomic.
    /// </summary>
    internal class LayoutOperationGroup : FocusOperationGroup, ILayoutOperationGroup
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutOperationGroup"/> class.
        /// </summary>
        /// <param name="operationList">List of operations belonging to this group.</param>
        /// <param name="refresh">Optional refresh operation to execute at the end of undo and redo.</param>
        public LayoutOperationGroup(ILayoutOperationReadOnlyList operationList, ILayoutGenericRefreshOperation refresh)
            : base(operationList, refresh)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// List of operations belonging to this group.
        /// </summary>
        public new ILayoutOperationReadOnlyList OperationList { get { return (ILayoutOperationReadOnlyList)base.OperationList; } }

        /// <summary>
        /// The main operation for this group.
        /// </summary>
        public new ILayoutOperation MainOperation { get { return (ILayoutOperation)base.MainOperation; } }

        /// <summary>
        /// Optional refresh operation to execute at the end of undo and redo.
        /// </summary>
        public new ILayoutGenericRefreshOperation Refresh { get { return (ILayoutGenericRefreshOperation)base.Refresh; } }
        #endregion
    }
}
