namespace EaslyController.Writeable
{
    /// <summary>
    /// Details for removal operations.
    /// </summary>
    public interface IWriteableRemoveOperation : IWriteableOperation
    {
    }

    /// <summary>
    /// Details for removal operations.
    /// </summary>
    public abstract class WriteableRemoveOperation : WriteableOperation, IWriteableRemoveOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableRemoveOperation"/>.
        /// </summary>
        public WriteableRemoveOperation()
            : base()
        {
        }
        #endregion
    }
}
