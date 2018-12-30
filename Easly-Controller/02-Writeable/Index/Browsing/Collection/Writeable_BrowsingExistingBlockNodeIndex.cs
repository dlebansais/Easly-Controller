using BaseNode;
using BaseNodeHelper;
using EaslyController.ReadOnly;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Index for a node in a block that is not the first.
    /// </summary>
    public interface IWriteableBrowsingExistingBlockNodeIndex : IReadOnlyBrowsingExistingBlockNodeIndex, IWriteableBrowsingBlockNodeIndex
    {
        void MoveUp();
        void MoveDown();
        void MoveBlockUp();
        void MoveBlockDown();
    }

    /// <summary>
    /// Index for a node in a block that is not the first.
    /// </summary>
    public class WriteableBrowsingExistingBlockNodeIndex : ReadOnlyBrowsingExistingBlockNodeIndex, IWriteableBrowsingExistingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBrowsingExistingBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="node">Indexed node in the block.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list.
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="index">Position of the node in the block.</param>
        public WriteableBrowsingExistingBlockNodeIndex(INode parentNode, INode node, string propertyName, int blockIndex, int index)
            : base(parentNode, node, propertyName, blockIndex, index)
        {
        }
        #endregion

        #region Client Interface
        public virtual void MoveUp()
        {
            Debug.Assert(NodeTreeHelperBlockList.GetLastBlockChildIndex(ParentNode, PropertyName, BlockIndex, out int LastIndex) && Index + 1 < LastIndex);
            
            Index++;
        }

        public virtual void MoveDown()
        {
            Debug.Assert(Index > 0);

            Index--;
        }

        public virtual void MoveBlockUp()
        {
            Debug.Assert(NodeTreeHelperBlockList.GetLastBlockIndex(ParentNode, PropertyName, out int LastBlockIndex) && BlockIndex + 1 < LastBlockIndex);

            BlockIndex++;
        }

        public virtual void MoveBlockDown()
        {
            Debug.Assert(BlockIndex > 0);

            BlockIndex--;
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyIndex"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IWriteableBrowsingExistingBlockNodeIndex AsBrowsingExistingBlockNodeIndex))
                return false;

            if (!base.IsEqual(comparer, AsBrowsingExistingBlockNodeIndex))
                return false;

            return true;
        }
        #endregion
    }
}
