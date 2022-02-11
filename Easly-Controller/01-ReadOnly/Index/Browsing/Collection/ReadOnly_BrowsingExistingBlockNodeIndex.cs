namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using Contracts;

    /// <summary>
    /// Index for a node in a block.
    /// </summary>
    public interface IReadOnlyBrowsingExistingBlockNodeIndex : IReadOnlyBrowsingBlockNodeIndex, IEqualComparable
    {
        /// <summary>
        /// The parent node.
        /// </summary>
        Node ParentNode { get; }

        /// <summary>
        /// Position of the node in the block.
        /// </summary>
        int Index { get; }
    }

    /// <inheritdoc/>
    public class ReadOnlyBrowsingExistingBlockNodeIndex : ReadOnlyBrowsingBlockNodeIndex, IReadOnlyBrowsingExistingBlockNodeIndex, IEqualComparable
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
        public ReadOnlyBrowsingExistingBlockNodeIndex(Node parentNode, Node node, string propertyName, int blockIndex, int index)
            : base(node, propertyName, blockIndex)
        {
            Contract.RequireNotNull(parentNode, out Node ParentNode);
            Contract.RequireNotNull(node, out Node Node);
            Contract.RequireNotNull(propertyName, out string PropertyName);
            Debug.Assert(PropertyName.Length > 0);
            Debug.Assert(blockIndex >= 0);
            Debug.Assert(index >= 0);
            Debug.Assert(NodeTreeHelperBlockList.IsBlockChildNode(ParentNode, PropertyName, blockIndex, index, Node));

            this.ParentNode = ParentNode;
            Index = index;
        }
        #endregion

        #region Properties
        /// <inheritdoc/>
        public Node ParentNode { get; }

        /// <inheritdoc/>
        public int Index { get; private protected set; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out ReadOnlyBrowsingExistingBlockNodeIndex AsExistingBlockNodeIndex))
                return comparer.Failed();

            if (!comparer.IsSameReference(ParentNode, AsExistingBlockNodeIndex.ParentNode))
                return comparer.Failed();

            if (!comparer.IsSameString(PropertyName, AsExistingBlockNodeIndex.PropertyName))
                return comparer.Failed();

            if (!comparer.IsSameInteger(BlockIndex, AsExistingBlockNodeIndex.BlockIndex))
                return comparer.Failed();

            if (!comparer.IsSameInteger(Index, AsExistingBlockNodeIndex.Index))
                return comparer.Failed();

            if (!comparer.IsSameReference(Node, AsExistingBlockNodeIndex.Node))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
