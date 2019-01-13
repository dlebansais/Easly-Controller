namespace EaslyController.Writeable
{
    /// <summary>
    /// Base for all operations modifying the node tree.
    /// </summary>
    public interface IWriteableOperation
    {
        /// <summary>
        /// True if the operation is nested within another more general one.
        /// </summary>
        bool IsNested { get; }
    }

    /// <summary>
    /// Base for all operations modifying the node tree.
    /// </summary>
    public class WriteableOperation : IWriteableOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of a <see cref="WriteableOperation"/> object.
        /// </summary>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableOperation(bool isNested)
        {
            IsNested = isNested;
        }
        #endregion

        #region Properties
        /// <summary>
        /// True if the operation is nested within another more general one.
        /// </summary>
        public bool IsNested { get; }
        #endregion
    }
}
