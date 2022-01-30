namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// Index for replacing a child a node.
    /// </summary>
    public interface IWriteableInsertionEmptyNodeIndex : IWriteableInsertionChildNodeIndex, IWriteableNodeIndex, IEqualComparable
    {
    }

    /// <summary>
    /// Index for replacing a child a node.
    /// </summary>
    public class WriteableInsertionEmptyNodeIndex : IWriteableInsertionEmptyNodeIndex, IEqualComparable
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInsertionEmptyNodeIndex"/> class.
        /// </summary>
        public WriteableInsertionEmptyNodeIndex()
        {
            ParentNode = Node.Empty;
            PropertyName = string.Empty;
            Node = Node.Empty;
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
        /// The assigned node.
        /// </summary>
        public Node Node { get; }
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
        /// Compares two <see cref="WriteableInsertionEmptyNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out WriteableInsertionEmptyNodeIndex AsInsertionEmptyNodeIndex))
                return comparer.Failed();

            if (!comparer.IsSameReference(ParentNode, AsInsertionEmptyNodeIndex.ParentNode))
                return comparer.Failed();

            if (!comparer.IsSameString(PropertyName, AsInsertionEmptyNodeIndex.PropertyName))
                return comparer.Failed();

            if (!comparer.IsSameReference(Node, AsInsertionEmptyNodeIndex.Node))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingEmptyNodeIndex object.
        /// </summary>
        private protected virtual IWriteableBrowsingPlaceholderNodeIndex CreateBrowsingIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableInsertionEmptyNodeIndex));
            return new WriteableBrowsingPlaceholderNodeIndex(ParentNode, Node, PropertyName);
        }
        #endregion
    }
}
