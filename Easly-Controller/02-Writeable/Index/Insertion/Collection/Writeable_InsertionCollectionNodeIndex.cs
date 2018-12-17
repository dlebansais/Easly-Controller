using BaseNode;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Base for list and block list index classes.
    /// </summary>
    public interface IWriteableInsertionCollectionNodeIndex : IWriteableInsertionChildIndex, IWriteableNodeIndex
    {
    }

    /// <summary>
    /// Base for list and block list index classes.
    /// </summary>
    public abstract class WriteableInsertionCollectionNodeIndex : IWriteableInsertionCollectionNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInsertionCollectionNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">The property for the index.</param>
        public WriteableInsertionCollectionNodeIndex(INode node, string propertyName)
        {
            Debug.Assert(node != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));

            Node = node;
            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Node indexed.
        /// </summary>
        public INode Node { get; }

        /// <summary>
        /// Property indexed for <see cref="Node"/>.
        /// </summary>
        public string PropertyName { get; }
        #endregion
    }
}
