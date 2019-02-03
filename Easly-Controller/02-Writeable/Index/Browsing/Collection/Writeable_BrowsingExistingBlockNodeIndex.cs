namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Index for a node in a block that is not the first.
    /// </summary>
    public interface IWriteableBrowsingExistingBlockNodeIndex : IReadOnlyBrowsingExistingBlockNodeIndex, IWriteableBrowsingBlockNodeIndex, IWriteableBrowsingInsertableIndex
    {
        /// <summary>
        /// Modifies the index to address the next position in a list.
        /// </summary>
        void MoveUp();

        /// <summary>
        /// Modifies the index to address the previous position in a list.
        /// </summary>
        void MoveDown();

        /// <summary>
        /// Modifies the index to address the next position in a block list.
        /// </summary>
        void MoveBlockUp();

        /// <summary>
        /// Modifies the index to address the previous position in a block list.
        /// </summary>
        void MoveBlockDown();
    }

    /// <summary>
    /// Index for a node in a block that is not the first.
    /// </summary>
    internal class WriteableBrowsingExistingBlockNodeIndex : ReadOnlyBrowsingExistingBlockNodeIndex, IWriteableBrowsingExistingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBrowsingExistingBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="node">Indexed node in the block.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="index">Position of the node in the block.</param>
        public WriteableBrowsingExistingBlockNodeIndex(INode parentNode, INode node, string propertyName, int blockIndex, int index)
            : base(parentNode, node, propertyName, blockIndex, index)
        {
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Modifies the index to address the next position in a list.
        /// </summary>
        public virtual void MoveUp()
        {
            Debug.Assert(NodeTreeHelperBlockList.GetLastBlockChildIndex(ParentNode, PropertyName, BlockIndex, out int LastIndex) && Index + 1 < LastIndex);

            Index++;
        }

        /// <summary>
        /// Modifies the index to address the previous position in a list.
        /// </summary>
        public virtual void MoveDown()
        {
            Debug.Assert(Index > 0);

            Index--;
        }

        /// <summary>
        /// Modifies the index to address the next position in a block list.
        /// </summary>
        public virtual void MoveBlockUp()
        {
            Debug.Assert(NodeTreeHelperBlockList.GetLastBlockIndex(ParentNode, PropertyName, out int LastBlockIndex) && BlockIndex + 1 < LastBlockIndex);

            BlockIndex++;
        }

        /// <summary>
        /// Modifies the index to address the previous position in a block list.
        /// </summary>
        public virtual void MoveBlockDown()
        {
            Debug.Assert(BlockIndex > 0);

            BlockIndex--;
        }

        /// <summary>
        /// Creates an insertion index from this instance, that can be used to replace it.
        /// </summary>
        /// <param name="parentNode">The parent node where the index would be used to replace a node.</param>
        /// <param name="node">The node inserted.</param>
        public virtual IWriteableInsertionChildIndex ToInsertionIndex(INode parentNode, INode node)
        {
            return CreateInsertionIndex(parentNode, node);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out WriteableBrowsingExistingBlockNodeIndex AsBrowsingExistingBlockNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBrowsingExistingBlockNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertionExistingBlockNodeIndex object.
        /// </summary>
        private protected virtual IWriteableInsertionExistingBlockNodeIndex CreateInsertionIndex(INode parentNode, INode node)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBrowsingExistingBlockNodeIndex));
            return new WriteableInsertionExistingBlockNodeIndex(parentNode, PropertyName, node, BlockIndex, Index);
        }
        #endregion
    }
}
