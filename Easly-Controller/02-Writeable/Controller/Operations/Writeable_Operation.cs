namespace EaslyController.Writeable
{
    /// <summary>
    /// Base for all operations modifying the node tree.
    /// </summary>
    public interface IWriteableOperation
    {
    }

    /// <summary>
    /// Base for all operations modifying the node tree.
    /// </summary>
    public class WriteableOperation : IWriteableOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableOperation"/>.
        /// </summary>
        public WriteableOperation()
        {
        }
        #endregion
    }
}
