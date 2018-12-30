using BaseNode;
using EaslyController.ReadOnly;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Index for a node.
    /// </summary>
    public interface IWriteableBrowsingPlaceholderNodeIndex : IReadOnlyBrowsingPlaceholderNodeIndex, IWriteableBrowsingChildIndex, IWriteableNodeIndex
    {
    }

    /// <summary>
    /// Index for a node.
    /// </summary>
    public class WriteableBrowsingPlaceholderNodeIndex : ReadOnlyBrowsingPlaceholderNodeIndex, IWriteableBrowsingPlaceholderNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBrowsingPlaceholderNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed node.</param>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed node.
        public WriteableBrowsingPlaceholderNodeIndex(INode parentNode, INode node, string propertyName)
            : base(parentNode, node, propertyName)
        {
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

            if (!(other is IWriteableBrowsingPlaceholderNodeIndex AsBrowsingPlaceholderNodeIndex))
                return false;

            if (!base.IsEqual(comparer, AsBrowsingPlaceholderNodeIndex))
                return false;

            return true;
        }
        #endregion
    }
}
