namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// Base for block list index classes.
    /// </summary>
    public interface IReadOnlyBrowsingBlockNodeIndex : IReadOnlyBrowsingCollectionNodeIndex
    {
        /// <summary>
        /// Position of the block in the block list.
        /// </summary>
        int BlockIndex { get; }
    }

    /// <summary>
    /// Base for block list index classes.
    /// </summary>
    internal abstract class ReadOnlyBrowsingBlockNodeIndex : ReadOnlyBrowsingCollectionNodeIndex, IReadOnlyBrowsingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">The property for the index.</param>
        /// <param name="blockIndex">The position of the block in the block list.</param>
        public ReadOnlyBrowsingBlockNodeIndex(INode node, string propertyName, int blockIndex)
            : base(node, propertyName)
        {
            Debug.Assert(blockIndex >= 0);

            BlockIndex = blockIndex;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Position of the block in the block list.
        /// </summary>
        public virtual int BlockIndex { get; private protected set; }
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

            if (!comparer.IsSameType(other, out ReadOnlyBrowsingBlockNodeIndex AsBlockNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBlockNodeIndex))
                return comparer.Failed();

            if (!comparer.IsSameInteger(BlockIndex, AsBlockNodeIndex.BlockIndex))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
