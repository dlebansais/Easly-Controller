namespace EaslyController.Writeable
{
    using BaseNode;

    /// <summary>
    /// Base for block list insertion index classes.
    /// </summary>
    public interface IWriteableInsertionBlockNodeIndex : IWriteableInsertionCollectionNodeIndex
    {
    }

    /// <summary>
    /// Base for block list insertion index classes.
    /// </summary>
    public abstract class WriteableInsertionBlockNodeIndex : WriteableInsertionCollectionNodeIndex, IWriteableInsertionBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInsertionBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">The node in which the insertion operation is taking place.</param>
        /// <param name="propertyName">The property for the index.</param>
        /// <param name="node">The inserted node.</param>
        public WriteableInsertionBlockNodeIndex(Node parentNode, string propertyName, Node node)
            : base(parentNode, propertyName, node)
        {
        }
        #endregion
    }
}
