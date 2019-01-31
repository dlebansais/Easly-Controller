namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Index for inserting a node in a list of nodes.
    /// </summary>
    public interface IWriteableInsertionListNodeIndex : IWriteableInsertionCollectionNodeIndex
    {
        /// <summary>
        /// Position where to insert in the list.
        /// </summary>
        int Index { get; }
    }

    /// <summary>
    /// Index for inserting a node in a list of nodes.
    /// </summary>
    public class WriteableInsertionListNodeIndex : WriteableInsertionCollectionNodeIndex, IWriteableInsertionListNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInsertionListNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the list.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the list.</param>
        /// <param name="node">Inserted node.</param>
        /// <param name="index">Position where to insert <paramref name="node"/> in the list.</param>
        public WriteableInsertionListNodeIndex(INode parentNode, string propertyName, INode node, int index)
            : base(parentNode, propertyName, node)
        {
            Debug.Assert(index >= 0);
            Debug.Assert(NodeTreeHelperList.GetLastListIndex(parentNode, propertyName, out int LastIndex) && index <= LastIndex);

            Index = index;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Position where to insert in the list.
        /// </summary>
        public int Index { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Creates a browsing index from an insertion index.
        /// To call after the insertion operation has been completed.
        /// </summary>
        public override IWriteableBrowsingChildIndex ToBrowsingIndex()
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
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out WriteableInsertionListNodeIndex AsInsertionListNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsInsertionListNodeIndex))
                return comparer.Failed();

            if (Index != AsInsertionListNodeIndex.Index)
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        private protected virtual IWriteableBrowsingListNodeIndex CreateBrowsingIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableInsertionListNodeIndex));
            return new WriteableBrowsingListNodeIndex(ParentNode, Node, PropertyName, Index);
        }
        #endregion
    }
}
