namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;

    /// <summary>
    /// Index for a node.
    /// </summary>
    public interface IReadOnlyBrowsingPlaceholderNodeIndex : IReadOnlyBrowsingChildIndex, IReadOnlyNodeIndex, IEqualComparable
    {
    }

    /// <inheritdoc/>
    internal class ReadOnlyBrowsingPlaceholderNodeIndex : IReadOnlyBrowsingPlaceholderNodeIndex, IEqualComparable
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingPlaceholderNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed node.</param>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed node.</param>
        public ReadOnlyBrowsingPlaceholderNodeIndex(Node parentNode, Node node, string propertyName)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(NodeTreeHelperChild.IsChildNode(parentNode, propertyName, node));

            Node = node;
            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        /// <inheritdoc/>
        public Node Node { get; }

        /// <inheritdoc/>
        public string PropertyName { get; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out ReadOnlyBrowsingPlaceholderNodeIndex AsPlaceholderNodeIndex))
                return comparer.Failed();

            if (!comparer.IsSameReference(Node, AsPlaceholderNodeIndex.Node))
                return comparer.Failed();

            if (!comparer.IsSameString(PropertyName, AsPlaceholderNodeIndex.PropertyName))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
