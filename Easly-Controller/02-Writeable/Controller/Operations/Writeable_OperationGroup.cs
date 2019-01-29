using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Group of operations to make some tasks atomic.
    /// </summary>
    public interface IWriteableOperationGroup
    {
        /// <summary>
        /// List of operations belonging to this group.
        /// </summary>
        IWriteableOperationReadOnlyList OperationList { get; }

        /// <summary>
        /// The main operation for this group.
        /// </summary>
        IWriteableOperation MainOperation { get; }

        /// <summary>
        /// Execute all operations in the group.
        /// </summary>
        void Redo();

        /// <summary>
        /// Undo all operations in the group.
        /// </summary>
        void Undo();
    }

    /// <summary>
    /// Group of operations to make some tasks atomic.
    /// </summary>
    public class WriteableOperationGroup : IWriteableOperationGroup
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of a <see cref="WriteableOperationGroup"/> object.
        /// </summary>
        /// <param name="operationList">List of operations belonging to this group.</param>
        public WriteableOperationGroup(IWriteableOperationReadOnlyList operationList)
        {
            Debug.Assert(operationList != null);
            Debug.Assert(operationList.Count > 0);

            OperationList = operationList;
        }
        #endregion

        #region Properties
        /// <summary>
        /// List of operations belonging to this group.
        /// </summary>
        public IWriteableOperationReadOnlyList OperationList { get; }

        /// <summary>
        /// The main operation for this group.
        /// </summary>
        public IWriteableOperation MainOperation { get { return OperationList[0]; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Execute all operations in the group.
        /// </summary>
        public void Redo()
        {
            for (int i = 0; i < OperationList.Count; i++)
            {
                IWriteableOperation Operation = OperationList[i];
                Operation.Redo();
            }
        }

        /// <summary>
        /// Undo all operations in the group.
        /// </summary>
        public void Undo()
        {
            for (int i = OperationList.Count; i > 0; i--)
            {
                IWriteableOperation Operation = OperationList[i - 1];
                Operation.Undo();
            }
        }
        #endregion
    }
}
