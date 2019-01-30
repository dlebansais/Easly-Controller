namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using Easly;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Index for clearing an optional node.
    /// </summary>
    public interface IWriteableInsertionOptionalClearIndex : IWriteableInsertionChildIndex
    {
        /// <summary>
        /// Interface to the optional object for the node.
        /// </summary>
        IOptionalReference Optional { get; }
    }

    /// <summary>
    /// Index for clearing an optional node.
    /// </summary>
    public class WriteableInsertionOptionalClearIndex : IWriteableInsertionOptionalClearIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInsertionOptionalClearIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed optional node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed optional node.</param>
        public WriteableInsertionOptionalClearIndex(INode parentNode, string propertyName)
        {
            Debug.Assert(parentNode != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));

            ParentNode = parentNode;
            PropertyName = propertyName;

            Optional = NodeTreeHelperOptional.GetOptionalReference(parentNode, propertyName);
            Debug.Assert(Optional != null);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Node in which the insertion operation is taking place.
        /// </summary>
        public INode ParentNode { get; }

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
        /// Compares two <see cref="IReadOnlyIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IWriteableInsertionOptionalClearIndex AsInsertionOptionalClearIndex))
                return comparer.Failed();

            if (ParentNode != AsInsertionOptionalClearIndex.ParentNode)
                return comparer.Failed();

            if (PropertyName != AsInsertionOptionalClearIndex.PropertyName)
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
            ControllerTools.AssertNoOverride(this, typeof(WriteableInsertionOptionalClearIndex));
            return new WriteableBrowsingOptionalNodeIndex(ParentNode, PropertyName);
        }
        #endregion
    }
}
