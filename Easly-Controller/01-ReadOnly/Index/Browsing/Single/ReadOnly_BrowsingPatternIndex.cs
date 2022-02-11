namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using BaseNode;
    using Contracts;

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

    /// <inheritdoc/>
    public class ReadOnlyBrowsingPatternIndex : IReadOnlyBrowsingPatternIndex, IEqualComparable
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
        /// <inheritdoc/>
        public Pattern Node { get { return Block.ReplicationPattern; } }
        Node IReadOnlyNodeIndex.Node { get { return Node; } }

        /// <inheritdoc/>
        public string PropertyName { get { return nameof(IBlock.ReplicationPattern); } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out ReadOnlyBrowsingPatternIndex AsPatternIndex))
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
