namespace EaslyController.Focus
{
    using EaslyController.Frame;

    /// <summary>
    /// Group of operations to make some tasks atomic.
    /// </summary>
    public interface IFocusOperationGroup : IFrameOperationGroup
    {
        /// <summary>
        /// List of operations belonging to this group.
        /// </summary>
        new IFocusOperationReadOnlyList OperationList { get; }

        /// <summary>
        /// The main operation for this group.
        /// </summary>
        new IFocusOperation MainOperation { get; }

        /// <summary>
        /// Optional refresh operation to execute at the end of undo and redo.
        /// </summary>
        new IFocusGenericRefreshOperation Refresh { get; }
    }

    /// <summary>
    /// Group of operations to make some tasks atomic.
    /// </summary>
    public class FocusOperationGroup : FrameOperationGroup, IFocusOperationGroup
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusOperationGroup"/> class.
        /// </summary>
        /// <param name="operationList">List of operations belonging to this group.</param>
        /// <param name="refresh">Optional refresh operation to execute at the end of undo and redo.</param>
        public FocusOperationGroup(IFocusOperationReadOnlyList operationList, IFocusGenericRefreshOperation refresh)
            : base(operationList, refresh)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// List of operations belonging to this group.
        /// </summary>
        public new IFocusOperationReadOnlyList OperationList { get { return (IFocusOperationReadOnlyList)base.OperationList; } }

        /// <summary>
        /// The main operation for this group.
        /// </summary>
        public new IFocusOperation MainOperation { get { return (IFocusOperation)base.MainOperation; } }

        /// <summary>
        /// Optional refresh operation to execute at the end of undo and redo.
        /// </summary>
        public new IFocusGenericRefreshOperation Refresh { get { return (IFocusGenericRefreshOperation)base.Refresh; } }
        #endregion
    }
}
