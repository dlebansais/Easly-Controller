using BaseNode;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Base for block list index classes.
    /// </summary>
    public interface IWriteableInsertionBlockNodeIndex : IWriteableInsertionCollectionNodeIndex
    {
    }

    /// <summary>
    /// Base for block list index classes.
    /// </summary>
    public abstract class WriteableInsertionBlockNodeIndex : WriteableInsertionCollectionNodeIndex, IWriteableInsertionBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInsertionBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">The property for the index.</param>
        public WriteableInsertionBlockNodeIndex(INode node, string propertyName)
            : base(node, propertyName)
        {
        }
        #endregion
    }
}
