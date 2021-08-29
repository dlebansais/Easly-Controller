namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Base for list and block list insertion index classes.
    /// </summary>
    public interface IWriteableInsertionCollectionNodeIndex : IWriteableInsertionChildNodeIndex, IWriteableNodeIndex
    {
    }

    /// <summary>
    /// Base for list and block list insertion index classes.
    /// </summary>
    public abstract class WriteableInsertionCollectionNodeIndex : IWriteableInsertionCollectionNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInsertionCollectionNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">The node in which the insertion operation is taking place.</param>
        /// <param name="propertyName">The property for the index.</param>
        /// <param name="node">The inserted node.</param>
        public WriteableInsertionCollectionNodeIndex(Node parentNode, string propertyName, Node node)
        {
            Debug.Assert(parentNode != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(node != null);
            Debug.Assert(NodeTreeHelper.IsAssignable(parentNode, propertyName, node));

            ParentNode = parentNode;
            PropertyName = propertyName;
            Node = node;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Node in which the insertion operation is taking place.
        /// </summary>
        public Node ParentNode { get; }

        /// <summary>
        /// Property indexed for <see cref="Node"/>.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Node inserted.
        /// </summary>
        public Node Node { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Creates a browsing index from an insertion index.
        /// To call after the insertion operation has been completed.
        /// </summary>
        public abstract IWriteableBrowsingChildIndex ToBrowsingIndex();
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="WriteableInsertionCollectionNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out WriteableInsertionCollectionNodeIndex AsInsertionCollectionNodeIndex))
                return comparer.Failed();

            if (!comparer.IsSameReference(ParentNode, AsInsertionCollectionNodeIndex.ParentNode))
                return comparer.Failed();

            if (!comparer.IsSameString(PropertyName, AsInsertionCollectionNodeIndex.PropertyName))
                return comparer.Failed();

            if (!comparer.IsSameReference(Node, AsInsertionCollectionNodeIndex.Node))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
