namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Index for an optional node.
    /// </summary>
    public interface IWriteableBrowsingOptionalNodeIndex : IReadOnlyBrowsingOptionalNodeIndex, IWriteableBrowsingChildIndex, IWriteableBrowsingInsertableIndex
    {
    }

    /// <summary>
    /// Index for an optional node.
    /// </summary>
    internal class WriteableBrowsingOptionalNodeIndex : ReadOnlyBrowsingOptionalNodeIndex, IWriteableBrowsingOptionalNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBrowsingOptionalNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed optional node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed optional node.</param>
        public WriteableBrowsingOptionalNodeIndex(Node parentNode, string propertyName)
            : base(parentNode, propertyName)
        {
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Creates an insertion index from this instance, that can be used to replace it.
        /// </summary>
        /// <param name="parentNode">The parent node where the index would be used to replace a node.</param>
        /// <param name="node">The node inserted.</param>
        public virtual IWriteableInsertionChildIndex ToInsertionIndex(Node parentNode, Node node)
        {
            if (node != null)
                return CreateInsertionOptionalNodeIndex(parentNode, node);
            else
                return CreateInsertionOptionalClearIndex(parentNode);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="WriteableBrowsingOptionalNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out WriteableBrowsingOptionalNodeIndex AsBrowsingOptionalNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBrowsingOptionalNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertionOptionalNodeIndex object.
        /// </summary>
        private protected virtual IWriteableInsertionOptionalNodeIndex CreateInsertionOptionalNodeIndex(Node parentNode, Node node)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBrowsingOptionalNodeIndex));
            return new WriteableInsertionOptionalNodeIndex(parentNode, PropertyName, node);
        }

        /// <summary>
        /// Creates a IxxxInsertionOptionalClearIndex object.
        /// </summary>
        private protected virtual IWriteableInsertionOptionalClearIndex CreateInsertionOptionalClearIndex(Node parentNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBrowsingOptionalNodeIndex));
            return new WriteableInsertionOptionalClearIndex(parentNode, PropertyName);
        }
        #endregion
    }
}
