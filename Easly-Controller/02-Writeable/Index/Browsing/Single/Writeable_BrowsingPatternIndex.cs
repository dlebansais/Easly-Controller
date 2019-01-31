namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Index for the replication pattern node of a block.
    /// </summary>
    public interface IWriteableBrowsingPatternIndex : IReadOnlyBrowsingPatternIndex, IWriteableBrowsingChildIndex, IWriteableNodeIndex
    {
    }

    /// <summary>
    /// Index for the replication pattern node of a block.
    /// </summary>
    internal class WriteableBrowsingPatternIndex : ReadOnlyBrowsingPatternIndex, IWriteableBrowsingPatternIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBrowsingPatternIndex"/> class.
        /// </summary>
        /// <param name="block">The block containing the indexed replication pattern node.</param>
        public WriteableBrowsingPatternIndex(IBlock block)
            : base(block)
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

            if (!comparer.IsSameType(other, out WriteableBrowsingPatternIndex AsBrowsingPatternIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBrowsingPatternIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertionPlaceholderNodeIndex object.
        /// </summary>
        private protected virtual IWriteableInsertionPlaceholderNodeIndex CreateInsertionIndex(INode parentNode, INode node)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBrowsingPatternIndex));
            return new WriteableInsertionPlaceholderNodeIndex(parentNode, PropertyName, node);
        }
        #endregion
    }
}
