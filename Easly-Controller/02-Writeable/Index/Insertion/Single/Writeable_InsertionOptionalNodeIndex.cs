namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using Contracts;
    using Easly;

    /// <summary>
    /// Index for replacing an optional node.
    /// </summary>
    public interface IWriteableInsertionOptionalNodeIndex : IWriteableInsertionChildNodeIndex, IWriteableNodeIndex, IEqualComparable
    {
        /// <summary>
        /// Interface to the optional object for the node.
        /// </summary>
        IOptionalReference Optional { get; }
    }

    /// <summary>
    /// Index for replacing an optional node.
    /// </summary>
    public class WriteableInsertionOptionalNodeIndex : IWriteableInsertionOptionalNodeIndex, IEqualComparable
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInsertionOptionalNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed optional node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed optional node.</param>
        /// <param name="node">The assigned node.</param>
        public WriteableInsertionOptionalNodeIndex(Node parentNode, string propertyName, Node node)
        {
            Contract.RequireNotNull(parentNode, out Node ParentNode);
            Contract.RequireNotNull(propertyName, out string PropertyName);
            Contract.RequireNotNull(node, out Node Node);
            Debug.Assert(PropertyName.Length > 0);
            Debug.Assert(NodeTreeHelper.IsAssignable(ParentNode, PropertyName, Node));

            this.ParentNode = ParentNode;
            this.PropertyName = PropertyName;
            this.Node = Node;

            Optional = NodeTreeHelperOptional.GetOptionalReference(ParentNode, PropertyName);
            Debug.Assert(Optional != null);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Node in which the insertion operation is taking place.
        /// </summary>
        public Node ParentNode { get; }

        /// <summary>
        /// Property indexed for <see cref="Optional"/>.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// The assigned node.
        /// </summary>
        public Node Node { get; }

        /// <summary>
        /// Interface to the optional object for the node.
        /// </summary>
        public IOptionalReference Optional { get; }
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
        /// Compares two <see cref="WriteableInsertionOptionalNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out WriteableInsertionOptionalNodeIndex AsInsertionOptionalNodeIndex))
                return comparer.Failed();

            if (!comparer.IsSameReference(ParentNode, AsInsertionOptionalNodeIndex.ParentNode))
                return comparer.Failed();

            if (!comparer.IsSameString(PropertyName, AsInsertionOptionalNodeIndex.PropertyName))
                return comparer.Failed();

            if (!comparer.IsSameReference(Node, AsInsertionOptionalNodeIndex.Node))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        private protected virtual IWriteableBrowsingOptionalNodeIndex CreateBrowsingIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableInsertionOptionalNodeIndex));
            return new WriteableBrowsingOptionalNodeIndex(ParentNode, PropertyName);
        }
        #endregion
    }
}
