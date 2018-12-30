using BaseNode;
using BaseNodeHelper;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Index for a node in a list of nodes.
    /// </summary>
    public interface IReadOnlyBrowsingListNodeIndex : IReadOnlyBrowsingCollectionNodeIndex
    {
        /// <summary>
        /// The parent node.
        /// </summary>
        INode ParentNode { get; }

        /// <summary>
        /// Position of the node in the list.
        /// </summary>
        int Index { get; }
    }

    /// <summary>
    /// Index for a node in a list of nodes.
    /// </summary>
    public class ReadOnlyBrowsingListNodeIndex : ReadOnlyBrowsingCollectionNodeIndex, IReadOnlyBrowsingListNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingListNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the list.</param>
        /// <param name="node">Indexed node in the list</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the list.
        /// <param name="index">Position of the node in the list.</param>
        public ReadOnlyBrowsingListNodeIndex(INode parentNode, INode node, string propertyName, int index)
            : base(node, propertyName)
        {
            Debug.Assert(parentNode != null);
            Debug.Assert(index >= 0);
            Debug.Assert(NodeTreeHelperList.IsListChildNode(parentNode, propertyName, index, node));

            ParentNode = parentNode;
            Index = index;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The parent node.
        /// </summary>
        public INode ParentNode { get; }

        /// <summary>
        /// Position of the node in the list.
        /// </summary>
        public int Index { get; protected set; }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyIndex"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IReadOnlyBrowsingListNodeIndex AsListNodeIndex))
                return false;

            if (!base.IsEqual(comparer, AsListNodeIndex))
                return false;

            if (ParentNode != AsListNodeIndex.ParentNode)
                return false;

            if (Index != AsListNodeIndex.Index)
                return false;

            return true;
        }
        #endregion
    }
}
