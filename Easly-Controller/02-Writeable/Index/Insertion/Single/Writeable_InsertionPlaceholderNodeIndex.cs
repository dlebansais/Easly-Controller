using BaseNode;
using BaseNodeHelper;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Index for a node.
    /// </summary>
    public interface IWriteableInsertionPlaceholderNodeIndex : IWriteableInsertionChildIndex, IWriteableNodeIndex
    {
    }

    /// <summary>
    /// Index for a node.
    /// </summary>
    public class WriteableInsertionPlaceholderNodeIndex : IWriteableInsertionPlaceholderNodeIndex 
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInsertionPlaceholderNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed node.</param>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed node.
        public WriteableInsertionPlaceholderNodeIndex(INode parentNode, INode node, string propertyName)
        {
            Debug.Assert(parentNode != null);
            Debug.Assert(node != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(NodeTreeHelper.IsChildNode(parentNode, propertyName, node));

            Node = node;
            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The indexed node.
        /// </summary>
        public INode Node { get; }

        /// <summary>
        /// Property indexed for <see cref="Node"/>.
        /// </summary>
        public string PropertyName { get; }
        #endregion
    }
}
