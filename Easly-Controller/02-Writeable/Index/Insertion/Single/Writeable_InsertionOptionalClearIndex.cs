namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using Contracts;
    using Easly;
    using NotNullReflection;

    /// <summary>
    /// Index for clearing an optional node.
    /// </summary>
    public interface IWriteableInsertionOptionalClearIndex : IWriteableInsertionChildIndex, IEqualComparable
    {
        /// <summary>
        /// Interface to the optional object for the node.
        /// </summary>
        IOptionalReference Optional { get; }
    }

    /// <summary>
    /// Index for clearing an optional node.
    /// </summary>
    public class WriteableInsertionOptionalClearIndex : IWriteableInsertionOptionalClearIndex, IEqualComparable
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInsertionOptionalClearIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed optional node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed optional node.</param>
        public WriteableInsertionOptionalClearIndex(Node parentNode, string propertyName)
        {
            Contract.RequireNotNull(parentNode, out Node ParentNode);
            Contract.RequireNotNull(propertyName, out string PropertyName);
            Debug.Assert(PropertyName.Length > 0);

            this.ParentNode = ParentNode;
            this.PropertyName = PropertyName;

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
        /// Compares two <see cref="WriteableInsertionOptionalClearIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out WriteableInsertionOptionalClearIndex AsInsertionOptionalClearIndex))
                return comparer.Failed();

            if (!comparer.IsSameReference(ParentNode, AsInsertionOptionalClearIndex.ParentNode))
                return comparer.Failed();

            if (!comparer.IsSameString(PropertyName, AsInsertionOptionalClearIndex.PropertyName))
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
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableInsertionOptionalClearIndex>());
            return new WriteableBrowsingOptionalNodeIndex(ParentNode, PropertyName);
        }
        #endregion
    }
}
