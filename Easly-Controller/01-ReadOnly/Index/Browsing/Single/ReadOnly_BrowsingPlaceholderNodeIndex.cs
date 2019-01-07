using BaseNode;
using BaseNodeHelper;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Index for a node.
    /// </summary>
    public interface IReadOnlyBrowsingPlaceholderNodeIndex : IReadOnlyBrowsingChildIndex, IReadOnlyNodeIndex
    {
    }

    /// <summary>
    /// Index for a node.
    /// </summary>
    public class ReadOnlyBrowsingPlaceholderNodeIndex : IReadOnlyBrowsingPlaceholderNodeIndex 
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingPlaceholderNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed node.</param>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed node.</param>
        public ReadOnlyBrowsingPlaceholderNodeIndex(INode parentNode, INode node, string propertyName)
        {
            Debug.Assert(parentNode != null);
            Debug.Assert(node != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(NodeTreeHelperChild.IsChildNode(parentNode, propertyName, node));

            Node = node;
            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The indexed node.
        /// </summary>
        public INode Node { get; }

        /// <summary>
        /// Property indexed for <see cref="Node"/>.
        /// </summary>
        public string PropertyName { get; }
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

            if (!(other is IReadOnlyBrowsingPlaceholderNodeIndex AsPlaceholderNodeIndex))
                return false;

            if (Node != AsPlaceholderNodeIndex.Node)
                return false;

            if (PropertyName != AsPlaceholderNodeIndex.PropertyName)
                return false;

            return true;
        }
        #endregion
    }
}
