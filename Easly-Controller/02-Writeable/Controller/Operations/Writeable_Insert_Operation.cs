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
        /// Initializes a new instance of a <see cref="WriteableInsertOperation"/> object.
        /// </summary>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableInsertOperation(bool isNested)
            : base(isNested)
        {
        }
        #endregion
    }
}
