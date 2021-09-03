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

    /// <summary>
    /// Index for a node.
    /// </summary>
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
            Debug.Assert(parentNode != null);
            Debug.Assert(node != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(NodeTreeHelperChild.IsChildNode(parentNode, propertyName, node));

            Node = node;
            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The indexed node.
        /// </summary>
        public Node Node { get; }

        /// <summary>
        /// Property indexed for <see cref="Node"/>.
        /// </summary>
        public string PropertyName { get; }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ReadOnlyBrowsingPlaceholderNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

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
