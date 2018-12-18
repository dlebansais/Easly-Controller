using BaseNode;
using BaseNodeHelper;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Index for a node in a list of nodes.
    /// </summary>
    public interface IWriteableInsertionListNodeIndex : IWriteableInsertionCollectionNodeIndex
    {
        /// <summary>
        /// Position of the node in the list.
        /// </summary>
        int Index { get; }
    }

    /// <summary>
    /// Index for a node in a list of nodes.
    /// </summary>
    public class WriteableInsertionListNodeIndex : WriteableInsertionCollectionNodeIndex, IWriteableInsertionListNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInsertionListNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the list.</param>
        /// <param name="node">Indexed node in the list</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the list.
        /// <param name="index">Position of the node in the list.</param>
        public WriteableInsertionListNodeIndex(INode parentNode, INode node, string propertyName, int index)
            : base(node, propertyName)
        {
            Debug.Assert(parentNode != null);
            Debug.Assert(index >= 0);
            Debug.Assert(NodeTreeHelper.IsListChildNode(parentNode, propertyName, index, node));

            Index = index;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Position of the node in the list.
        /// </summary>
        public int Index { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Creates a browsing index from an insertion index.
        /// To call after the insertion operation has been completed.
        /// </summary>
        /// <param name="parentNode">The node in which the insertion operation is taking place.</param>
        public override IWriteableBrowsingCollectionNodeIndex ToBrowsingIndex(INode parentNode)
        {
            return new WriteableBrowsingListNodeIndex(parentNode, Node, PropertyName, Index);
        }
        #endregion
    }
}
