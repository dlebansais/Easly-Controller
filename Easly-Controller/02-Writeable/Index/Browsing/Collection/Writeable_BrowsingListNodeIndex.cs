namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Index for a node in a list of nodes.
    /// </summary>
    public interface IWriteableBrowsingListNodeIndex : IReadOnlyBrowsingListNodeIndex, IWriteableBrowsingCollectionNodeIndex
    {
        /// <summary>
        /// Modifies the index to address the next position in a list.
        /// </summary>
        void MoveUp();

        /// <summary>
        /// Modifies the index to address the previous position in a list.
        /// </summary>
        void MoveDown();
    }

    /// <summary>
    /// Index for a node in a list of nodes.
    /// </summary>
    public class WriteableBrowsingListNodeIndex : ReadOnlyBrowsingListNodeIndex, IWriteableBrowsingListNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBrowsingListNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the list.</param>
        /// <param name="node">Indexed node in the list</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the list.</param>
        /// <param name="index">Position of the node in the list.</param>
        public WriteableBrowsingListNodeIndex(INode parentNode, INode node, string propertyName, int index)
            : base(parentNode, node, propertyName, index)
        {
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Modifies the index to address the next position in a list.
        /// </summary>
        public virtual void MoveUp()
        {
            Debug.Assert(NodeTreeHelperList.GetLastListIndex(ParentNode, PropertyName, out int LastIndex) && Index + 1 < LastIndex);

            Index++;
        }

        /// <summary>
        /// Modifies the index to address the previous position in a list.
        /// </summary>
        public virtual void MoveDown()
        {
            Debug.Assert(Index > 0);

            Index--;
        }

        /// <summary>
        /// Creates an insertion index from this instance, that can be used to replace it.
        /// </summary>
        /// <param name="parentNode">The parent node where the index would be used to replace a node.</param>
        /// <param name="node">The node inserted.</param>
        public virtual IWriteableInsertionChildIndex ToInsertionIndex(INode parentNode, INode node)
        {
            return CreateInsertionIndex(parentNode, node);
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

            if (!(other is IWriteableBrowsingListNodeIndex AsBrowsingListNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBrowsingListNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertionListNodeIndex object.
        /// </summary>
        protected virtual IWriteableInsertionListNodeIndex CreateInsertionIndex(INode parentNode, INode node)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBrowsingListNodeIndex));
            return new WriteableInsertionListNodeIndex(parentNode, PropertyName, node, Index);
        }
        #endregion
    }
}
