namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using Contracts;

    /// <summary>
    /// Group of operations to make some tasks atomic.
    /// </summary>
    public class WriteableOperationGroup
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="WriteableOperationGroup"/> object.
        /// </summary>
        public static WriteableOperationGroup Empty { get; } = new WriteableOperationGroup();

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableOperationGroup"/> class.
        /// </summary>
        protected WriteableOperationGroup()
            : this(new WriteableOperationList(), WriteableGenericRefreshOperation.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableOperationGroup"/> class.
        /// </summary>
        /// <param name="operationList">List of operations belonging to this group.</param>
        /// <param name="refresh">Optional refresh operation to execute at the end of undo and redo.</param>
        protected WriteableOperationGroup(WriteableOperationList operationList, WriteableGenericRefreshOperation refresh)
        {
            OperationList = new WriteableOperationReadOnlyList(operationList);
            Refresh = refresh;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableOperationGroup"/> class.
        /// </summary>
        /// <param name="operationList">List of operations belonging to this group.</param>
        /// <param name="refresh">Optional refresh operation to execute at the end of undo and redo.</param>
        public WriteableOperationGroup(WriteableOperationReadOnlyList operationList, WriteableGenericRefreshOperation refresh)
        {
            Contract.RequireNotNull(operationList, out WriteableOperationReadOnlyList OperationList);
            Debug.Assert(OperationList.Count > 0);

            this.OperationList = OperationList;
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
        public IWriteableOperation MainOperation { get { return OperationList[0]; } }

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
                IWriteableOperation Operation = OperationList[i];
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
                IWriteableOperation Operation = OperationList[i - 1];
                Operation.Undo();
            }

            if (Refresh != null)
                Refresh.Redo();
        }
        #endregion
    }
}
