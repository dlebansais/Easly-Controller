namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// Index for the replication pattern node of a block.
    /// </summary>
    public interface IReadOnlyBrowsingPatternIndex : IReadOnlyBrowsingChildIndex, IReadOnlyNodeIndex, IEqualComparable
    {
        /// <summary>
        /// The indexed replication pattern node.
        /// </summary>
        new Pattern Node { get; }
    }

    /// <summary>
    /// Index for the replication pattern node of a block.
    /// </summary>
    internal class ReadOnlyBrowsingPatternIndex : IReadOnlyBrowsingPatternIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingPatternIndex"/> class.
        /// </summary>
        /// <param name="block">The block containing the indexed replication pattern node.</param>
        public ReadOnlyBrowsingPatternIndex(IBlock block)
        {
            Debug.Assert(block != null);

            Block = block;
        }

        private IBlock Block;
        #endregion

        #region Properties
        /// <summary>
        /// The indexed replication pattern node.
        /// </summary>
        public Pattern Node { get { return Block.ReplicationPattern; } }
        Node IReadOnlyNodeIndex.Node { get { return Node; } }

        /// <summary>
        /// Property indexed for <see cref="Node"/>.
        /// </summary>
        public string PropertyName { get { return nameof(IBlock.ReplicationPattern); } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ReadOnlyBrowsingPatternIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyBrowsingPatternIndex AsPatternIndex))
                return comparer.Failed();

            if (!comparer.IsSameReference(Node, AsPatternIndex.Node))
                return comparer.Failed();

            if (!comparer.IsSameString(PropertyName, AsPatternIndex.PropertyName))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
