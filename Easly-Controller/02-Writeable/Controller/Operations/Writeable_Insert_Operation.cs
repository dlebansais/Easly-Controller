namespace EaslyController.Writeable
{
    /// <summary>
    /// Details for insertion operations.
    /// </summary>
    public interface IWriteableInsertOperation : IWriteableOperation
    {
    }

    /// <summary>
    /// Details for insertion operations.
    /// </summary>
    public abstract class WriteableInsertOperation : WriteableOperation, IWriteableInsertOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableInsertOperation"/>.
        /// </summary>
        public WriteableInsertOperation()
            : base()
        {
        }
        #endregion
    }
}
