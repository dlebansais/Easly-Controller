namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Index for replacing a child a node.
    /// </summary>
    public interface IWriteableInsertionPlaceholderNodeIndex : IWriteableInsertionChildNodeIndex, IWriteableNodeIndex, IEqualComparable
    {
    }

    /// <summary>
    /// Index for replacing a child a node.
    /// </summary>
    public class WriteableInsertionPlaceholderNodeIndex : IWriteableInsertionPlaceholderNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInsertionPlaceholderNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the replaced node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed node.</param>
        /// <param name="node">The assigned node.</param>
        public WriteableInsertionPlaceholderNodeIndex(INode parentNode, string propertyName, INode node)
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
        public INode ParentNode { get; }

        /// <summary>
        /// Property indexed for <see cref="Node"/>.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// The assigned node.
        /// </summary>
        public INode Node { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Creates a browsing index from an insertion index.
        /// To call after the insertion operation has been completed.
        /// </summary>
        public virtual IWriteableBrowsingChildIndex ToBrowsingIndex()
        {
            return CreateBrowsingIndex();
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="WriteableInsertionPlaceholderNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out WriteableInsertionPlaceholderNodeIndex AsInsertionPlaceholderNodeIndex))
                return comparer.Failed();

            if (!comparer.IsSameReference(ParentNode, AsInsertionPlaceholderNodeIndex.ParentNode))
                return comparer.Failed();

            if (!comparer.IsSameString(PropertyName, AsInsertionPlaceholderNodeIndex.PropertyName))
                return comparer.Failed();

            if (!comparer.IsSameReference(Node, AsInsertionPlaceholderNodeIndex.Node))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingPlaceholderNodeIndex object.
        /// </summary>
        private protected virtual IWriteableBrowsingPlaceholderNodeIndex CreateBrowsingIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableInsertionPlaceholderNodeIndex));
            return new WriteableBrowsingPlaceholderNodeIndex(ParentNode, Node, PropertyName);
        }
        #endregion
    }
}
