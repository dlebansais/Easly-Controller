namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using Contracts;

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

    /// <inheritdoc/>
    public class ReadOnlyBrowsingListNodeIndex : ReadOnlyBrowsingCollectionNodeIndex, IReadOnlyBrowsingListNodeIndex, IReadOnlyBrowsingCollectionNodeIndex, IEqualComparable
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="ReadOnlyBrowsingListNodeIndex"/> object.
        /// </summary>
        public static new ReadOnlyBrowsingListNodeIndex Empty { get; } = new ReadOnlyBrowsingListNodeIndex();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingListNodeIndex"/> class.
        /// </summary>
        protected ReadOnlyBrowsingListNodeIndex()
        {
            ParentNode = Node.Default;
        }

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
            Contract.RequireNotNull(parentNode, out Node ParentNode);
            Debug.Assert(index >= 0);
            Debug.Assert(NodeTreeHelperList.IsListChildNode(ParentNode, propertyName, index, node));

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

            if (!comparer.IsSameType(Other, out ReadOnlyBrowsingListNodeIndex AsListNodeIndex))
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
