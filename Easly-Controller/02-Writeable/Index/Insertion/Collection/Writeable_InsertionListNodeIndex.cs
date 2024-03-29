﻿namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using Contracts;
    using NotNullReflection;

    /// <summary>
    /// Index for inserting a node in a list of nodes.
    /// </summary>
    public interface IWriteableInsertionListNodeIndex : IWriteableInsertionCollectionNodeIndex, IEqualComparable
    {
        /// <summary>
        /// Position where to insert in the list.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Modifies the index to address the next position in a list.
        /// </summary>
        void MoveUp();
    }

    /// <summary>
    /// Index for inserting a node in a list of nodes.
    /// </summary>
    public class WriteableInsertionListNodeIndex : WriteableInsertionCollectionNodeIndex, IWriteableInsertionListNodeIndex, IEqualComparable
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInsertionListNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the list.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the list.</param>
        /// <param name="node">Inserted node.</param>
        /// <param name="index">Position where to insert <paramref name="node"/> in the list.</param>
        public WriteableInsertionListNodeIndex(Node parentNode, string propertyName, Node node, int index)
            : base(parentNode, propertyName, node)
        {
            Debug.Assert(index >= 0);

            NodeTreeHelperList.GetLastListIndex(parentNode, propertyName, out int LastIndex);
            Debug.Assert(index <= LastIndex);

            Index = index;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Position where to insert in the list.
        /// </summary>
        public int Index { get; private set; }
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

        /// <summary>
        /// Modifies the index to address the next position in a list.
        /// </summary>
        public virtual void MoveUp()
        {
            NodeTreeHelperList.GetLastListIndex(ParentNode, PropertyName, out int LastIndex);
            Debug.Assert(Index < LastIndex);

            Index++;
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="WriteableInsertionListNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out WriteableInsertionListNodeIndex AsInsertionListNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsInsertionListNodeIndex))
                return comparer.Failed();

            if (!comparer.IsSameInteger(Index, AsInsertionListNodeIndex.Index))
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
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableInsertionListNodeIndex>());
            return new WriteableBrowsingListNodeIndex(ParentNode, Node, PropertyName, Index);
        }
        #endregion
    }
}
