using EaslyController.Writeable;

namespace EaslyController.Frame
{
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
    }

    /// <summary>
    /// Group of operations to make some tasks atomic.
    /// </summary>
    public class FrameOperationGroup : WriteableOperationGroup, IFrameOperationGroup
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of a <see cref="FrameOperationGroup"/> object.
        /// </summary>
        /// <param name="operationList">List of operations belonging to this group.</param>
        public FrameOperationGroup(IFrameOperationReadOnlyList operationList)
            : base(operationList)
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
        #endregion
    }
}
