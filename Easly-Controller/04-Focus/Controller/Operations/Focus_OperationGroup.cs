using EaslyController.Frame;

namespace EaslyController.Focus
{
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
    }

    /// <summary>
    /// Group of operations to make some tasks atomic.
    /// </summary>
    public class FocusOperationGroup : FrameOperationGroup, IFocusOperationGroup
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of a <see cref="FocusOperationGroup"/> object.
        /// </summary>
        /// <param name="operationList">List of operations belonging to this group.</param>
        public FocusOperationGroup(IFocusOperationReadOnlyList operationList)
            : base(operationList)
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
        #endregion
    }
}
