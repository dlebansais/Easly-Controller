namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;

    /// <summary>
    /// Index for a node in a list of nodes.
    /// </summary>
    public interface IReadOnlyBrowsingListNodeIndex : IReadOnlyBrowsingCollectionNodeIndex, IEqualComparable
    {
        /// <summary>
        /// The parent node.
        /// </summary>
        Node ParentNode { get; }

        /// <summary>
        /// Position of the node in the list.
        /// </summary>
        int Index { get; }
    }

    /// <summary>
    /// Index for a node in a list of nodes.
    /// </summary>
    internal class ReadOnlyBrowsingListNodeIndex : ReadOnlyBrowsingCollectionNodeIndex, IReadOnlyBrowsingListNodeIndex, IReadOnlyBrowsingCollectionNodeIndex, IEqualComparable
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingListNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the list.</param>
        /// <param name="node">Indexed node in the list</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the list.</param>
        /// <param name="index">Position of the node in the list.</param>
        public ReadOnlyBrowsingListNodeIndex(Node parentNode, Node node, string propertyName, int index)
            : base(node, propertyName)
        {
            Debug.Assert(parentNode != null);
            Debug.Assert(index >= 0);
            Debug.Assert(NodeTreeHelperList.IsListChildNode(parentNode, propertyName, index, node));

            ParentNode = parentNode;
            Index = index;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The parent node.
        /// </summary>
        public Node ParentNode { get; }

        /// <summary>
        /// Position of the node in the list.
        /// </summary>
        public int Index { get; private protected set; }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ReadOnlyBrowsingListNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyBrowsingListNodeIndex AsListNodeIndex))
                return comparer.Failed();

            if (!comparer.IsSameString(PropertyName, AsListNodeIndex.PropertyName))
                return comparer.Failed();

            if (!comparer.IsSameReference(ParentNode, AsListNodeIndex.ParentNode))
                return comparer.Failed();

            if (!comparer.IsSameInteger(Index, AsListNodeIndex.Index))
                return comparer.Failed();

            if (!comparer.IsSameReference(Node, AsListNodeIndex.Node))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
