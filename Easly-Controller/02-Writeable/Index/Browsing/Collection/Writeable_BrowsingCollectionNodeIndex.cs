using BaseNode;
using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Base for list and block list index classes.
    /// </summary>
    public interface IWriteableBrowsingCollectionNodeIndex : IReadOnlyBrowsingCollectionNodeIndex, IWriteableBrowsingChildIndex, IWriteableNodeIndex
    {
    }

    /// <summary>
    /// Base for list and block list index classes.
    /// </summary>
    public abstract class WriteableBrowsingCollectionNodeIndex : ReadOnlyBrowsingCollectionNodeIndex, IWriteableBrowsingCollectionNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBrowsingCollectionNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">The property for the index.</param>
        public WriteableBrowsingCollectionNodeIndex(INode node, string propertyName)
            : base(node, propertyName)
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
