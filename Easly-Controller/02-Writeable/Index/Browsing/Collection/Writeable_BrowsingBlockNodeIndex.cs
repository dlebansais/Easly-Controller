namespace EaslyController.Writeable
{
    using BaseNode;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Base for block list index classes.
    /// </summary>
    public interface IWriteableBrowsingBlockNodeIndex : IReadOnlyBrowsingBlockNodeIndex, IWriteableBrowsingCollectionNodeIndex
    {
    }

    /// <summary>
    /// Base for block list index classes.
    /// </summary>
    public abstract class WriteableBrowsingBlockNodeIndex : ReadOnlyBrowsingBlockNodeIndex, IWriteableBrowsingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBrowsingBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">The property for the index.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        public WriteableBrowsingBlockNodeIndex(INode node, string propertyName, int blockIndex)
            : base(node, propertyName, blockIndex)
        {
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Creates an insertion index from this instance, that can be used to replace it.
        /// </summary>
        /// <param name="parentNode">The parent node where the index would be used to replace a node.</param>
        /// <param name="node">The node inserted.</param>
        public abstract IWriteableInsertionChildIndex ToInsertionIndex(INode parentNode, INode node);
        #endregion
    }
}
