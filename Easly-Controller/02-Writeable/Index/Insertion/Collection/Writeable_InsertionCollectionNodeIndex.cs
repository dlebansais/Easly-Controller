using BaseNode;
using BaseNodeHelper;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Base for list and block list insertion index classes.
    /// </summary>
    public interface IWriteableInsertionCollectionNodeIndex : IWriteableInsertionChildIndex, IWriteableNodeIndex
    {
    }

    /// <summary>
    /// Base for list and block list insertion index classes.
    /// </summary>
    public abstract class WriteableInsertionCollectionNodeIndex : IWriteableInsertionCollectionNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInsertionCollectionNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">The node in which the insertion operation is taking place.</param>
        /// <param name="propertyName">The property for the index.</param>
        /// <param name="node">The inserted node.</param>
        public WriteableInsertionCollectionNodeIndex(INode parentNode, string propertyName, INode node)
        {
            Debug.Assert(parentNode != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(node != null);
            Debug.Assert(NodeTreeHelper.IsAssignable(parentNode, propertyName, node));

            ParentNode = parentNode;
            PropertyName = propertyName;
            Node = node;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Node in which the insertion operation is taking place.
        /// </summary>
        public INode ParentNode { get; }

        /// <summary>
        /// Property indexed for <see cref="Node"/>.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Node inserted.
        /// </summary>
        public INode Node { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Creates a browsing index from an insertion index.
        /// To call after the insertion operation has been completed.
        /// </summary>
        public abstract IWriteableBrowsingChildIndex ToBrowsingIndex();
        #endregion
    }
}
