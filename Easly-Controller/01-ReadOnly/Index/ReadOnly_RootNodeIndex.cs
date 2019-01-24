using BaseNode;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Index for the root node of the node tree.
    /// </summary>
    public interface IReadOnlyRootNodeIndex : IReadOnlyNodeIndex
    {
    }

    /// <summary>
    /// Index for the root node of the node tree.
    /// </summary>
    public class ReadOnlyRootNodeIndex : IReadOnlyRootNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRootNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed root node.</param>
        public ReadOnlyRootNodeIndex(INode node)
        {
            Debug.Assert(node != null);

            Node = node;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The indexed root node.
        /// </summary>
        public INode Node { get; }
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

            if (!(other is IReadOnlyRootNodeIndex AsRootNodeIndex))
                return comparer.Failed();

            if (Node != AsRootNodeIndex.Node)
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
