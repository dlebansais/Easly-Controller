namespace EaslyController.Writeable
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Group of operations to make some tasks atomic.
    /// </summary>
    public class WriteableOperationGroup
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableOperationGroup"/> class.
        /// </summary>
        /// <param name="operationList">List of operations belonging to this group.</param>
        /// <param name="refresh">Optional refresh operation to execute at the end of undo and redo.</param>
        public WriteableOperationGroup(WriteableOperationReadOnlyList operationList, WriteableGenericRefreshOperation refresh)
        {
            Debug.Assert(operationList != null);
            Debug.Assert(operationList.Count > 0);

            OperationList = operationList;
            Refresh = refresh;
        }
        #endregion

        #region Properties
        /// <summary>
        /// List of operations belonging to this group.
        /// </summary>
        public WriteableOperationReadOnlyList OperationList { get; }

        /// <summary>
        /// The main operation for this group.
        /// </summary>
        public WriteableOperation MainOperation { get { return OperationList[0]; } }

        /// <summary>
        /// Optional refresh operation to execute at the end of undo and redo.
        /// </summary>
        public WriteableGenericRefreshOperation Refresh { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Execute all operations in the group.
        /// </summary>
        public virtual void Redo()
        {
            for (int i = 0; i < OperationList.Count; i++)
            {
                WriteableOperation Operation = OperationList[i];
                Operation.Redo();
            }

            if (Refresh != null)
                Refresh.Redo();
        }

        /// <summary>
        /// Undo all operations in the group.
        /// </summary>
        public virtual void Undo()
        {
            for (int i = OperationList.Count; i > 0; i--)
            {
                WriteableOperation Operation = OperationList[i - 1];
                Operation.Undo();
            }

            if (Refresh != null)
                Refresh.Redo();
        }
        #endregion
    }
}
