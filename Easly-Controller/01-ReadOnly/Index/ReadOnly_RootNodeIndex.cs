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
    }
}
