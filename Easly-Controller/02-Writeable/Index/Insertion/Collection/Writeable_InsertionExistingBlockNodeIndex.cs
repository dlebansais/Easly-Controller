using BaseNode;
using BaseNodeHelper;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Index for a node in a block that is not the first.
    /// </summary>
    public interface IWriteableInsertionExistingBlockNodeIndex : IWriteableInsertionBlockNodeIndex
    {
        /// <summary>
        /// Position of the block in the block list.
        /// </summary>
        int BlockIndex { get; }

        /// <summary>
        /// Position of the node in the block.
        /// </summary>
        int Index { get; }
    }

    /// <summary>
    /// Index for a node in a block that is not the first.
    /// </summary>
    public class WriteableInsertionExistingBlockNodeIndex : WriteableInsertionBlockNodeIndex, IWriteableInsertionExistingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInsertionExistingBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="node">Indexed node in the block.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list.
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="index">Position of the node in the block.</param>
        public WriteableInsertionExistingBlockNodeIndex(INode parentNode, INode node, string propertyName, int blockIndex, int index)
            : base(node, propertyName)
        {
            Debug.Assert(parentNode != null);
            Debug.Assert(node != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(blockIndex >= 0);
            Debug.Assert(index >= 0); // You can insert at position 0, contrary to a browsing index that only supports positions other than 0.
            Debug.Assert(NodeTreeHelper.GetLastBlockChildIndex(parentNode, propertyName, blockIndex, out int LastIndex) && index <= LastIndex);

            BlockIndex = blockIndex;
            Index = index;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Position of the block in the block list.
        /// </summary>
        public int BlockIndex { get; }

        /// <summary>
        /// Position of the node in the block.
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
            return new WriteableBrowsingExistingBlockNodeIndex(parentNode, Node, PropertyName, BlockIndex, Index);
        }
        #endregion
    }
}
