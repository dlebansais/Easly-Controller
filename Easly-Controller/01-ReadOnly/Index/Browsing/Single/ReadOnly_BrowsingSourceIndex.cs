namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// Index for the source identifier node of a block.
    /// </summary>
    public interface IReadOnlyBrowsingSourceIndex : IReadOnlyBrowsingChildIndex, IReadOnlyNodeIndex
    {
        /// <summary>
        /// The indexed source identifier node.
        /// </summary>
        new IIdentifier Node { get; }
    }

    /// <summary>
    /// Index for the source identifier node of a block.
    /// </summary>
    internal class ReadOnlyBrowsingSourceIndex : IReadOnlyBrowsingSourceIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingSourceIndex"/> class.
        /// </summary>
        /// <param name="block">The block containing the indexed source identifier node.</param>
        public ReadOnlyBrowsingSourceIndex(IBlock block)
        {
            Debug.Assert(block != null);

            Block = block;
        }

        private IBlock Block;
        #endregion

        #region Properties
        /// <summary>
        /// The indexed source identifier node.
        /// </summary>
        public IIdentifier Node { get { return Block.SourceIdentifier; } }
        INode IReadOnlyNodeIndex.Node { get { return Node; } }

        /// <summary>
        /// Property indexed for <see cref="Node"/>.
        /// </summary>
        public string PropertyName { get { return nameof(IBlock.SourceIdentifier); } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyBrowsingSourceIndex AsSourceIndex))
                return comparer.Failed();

            if (Node != AsSourceIndex.Node)
                return comparer.Failed();

            if (PropertyName != AsSourceIndex.PropertyName)
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
