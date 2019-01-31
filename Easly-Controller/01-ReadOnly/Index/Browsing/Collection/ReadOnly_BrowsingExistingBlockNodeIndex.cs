namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;

    /// <summary>
    /// Index for a node in a block.
    /// </summary>
    public interface IReadOnlyBrowsingExistingBlockNodeIndex : IReadOnlyBrowsingBlockNodeIndex
    {
        /// <summary>
        /// The parent node.
        /// </summary>
        INode ParentNode { get; }

        /// <summary>
        /// Position of the node in the block.
        /// </summary>
        int Index { get; }
    }

    /// <summary>
    /// Index for a node in a block.
    /// </summary>
    internal class ReadOnlyBrowsingExistingBlockNodeIndex : ReadOnlyBrowsingBlockNodeIndex, IReadOnlyBrowsingExistingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingExistingBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="node">Indexed node in the block.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="index">Position of the node in the block.</param>
        public ReadOnlyBrowsingExistingBlockNodeIndex(INode parentNode, INode node, string propertyName, int blockIndex, int index)
            : base(node, propertyName, blockIndex)
        {
            Debug.Assert(parentNode != null);
            Debug.Assert(node != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(blockIndex >= 0);
            Debug.Assert(index >= 0);
            Debug.Assert(NodeTreeHelperBlockList.IsBlockChildNode(parentNode, propertyName, blockIndex, index, node));

            ParentNode = parentNode;
            Index = index;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The parent node.
        /// </summary>
        public INode ParentNode { get; }

        /// <summary>
        /// Position of the node in the block.
        /// </summary>
        public int Index { get; private protected set; }
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

            if (!comparer.IsSameType(other, out ReadOnlyBrowsingExistingBlockNodeIndex AsExistingBlockNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsExistingBlockNodeIndex))
                return comparer.Failed();

            if (ParentNode != AsExistingBlockNodeIndex.ParentNode)
                return comparer.Failed();

            if (Index != AsExistingBlockNodeIndex.Index)
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
