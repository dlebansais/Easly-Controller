namespace EaslyController.Writeable
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Index for the first node in a block.
    /// </summary>
    public interface IWriteableBrowsingNewBlockNodeIndex : IReadOnlyBrowsingNewBlockNodeIndex, IWriteableBrowsingBlockNodeIndex
    {
    }

    /// <summary>
    /// Index for the first node in a block.
    /// </summary>
    internal class WriteableBrowsingNewBlockNodeIndex : ReadOnlyBrowsingNewBlockNodeIndex, IWriteableBrowsingNewBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBrowsingNewBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="node">First node in the block.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="patternNode">Replication pattern in the block.</param>
        /// <param name="sourceNode">Source identifier in the block.</param>
        public WriteableBrowsingNewBlockNodeIndex(INode parentNode, INode node, string propertyName, int blockIndex, IPattern patternNode, IIdentifier sourceNode)
            : base(parentNode, node, propertyName, blockIndex, patternNode, sourceNode)
        {
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Creates an insertion index from this instance, that can be used to replace it.
        /// </summary>
        /// <param name="parentNode">The parent node where the index would be used to replace a node.</param>
        /// <param name="node">The node inserted.</param>
        public virtual IWriteableInsertionChildIndex ToInsertionIndex(INode parentNode, INode node)
        {
            throw new InvalidOperationException();
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

            if (!comparer.IsSameType(other, out WriteableBrowsingNewBlockNodeIndex AsBrowsingNewBlockNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBrowsingNewBlockNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBrowsingNewBlockNodeIndex));
            return new WriteableBrowsingExistingBlockNodeIndex(ParentNode, Node, PropertyName, BlockIndex, 0);
        }
        #endregion
    }
}
