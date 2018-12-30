using BaseNode;
using BaseNodeHelper;
using EaslyController.ReadOnly;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Index for a node in a list of nodes.
    /// </summary>
    public interface IWriteableBrowsingListNodeIndex : IReadOnlyBrowsingListNodeIndex, IWriteableBrowsingCollectionNodeIndex
    {
        void MoveUp();
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
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the list.
        /// <param name="index">Position of the node in the list.</param>
        public WriteableBrowsingListNodeIndex(INode parentNode, INode node, string propertyName, int index)
            : base(parentNode, node, propertyName, index)
        {
        }
        #endregion

        #region Client Interface
        public virtual void MoveUp()
        {
            Debug.Assert(NodeTreeHelperList.GetLastListIndex(ParentNode, PropertyName, out int LastIndex) && Index + 1 < LastIndex);

            Index++;
        }

        public virtual void MoveDown()
        {
            Debug.Assert(Index > 0);

            Index--;
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyIndex"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IWriteableBrowsingListNodeIndex AsBrowsingListNodeIndex))
                return false;

            if (!base.IsEqual(comparer, AsBrowsingListNodeIndex))
                return false;

            return true;
        }
        #endregion
    }
}
