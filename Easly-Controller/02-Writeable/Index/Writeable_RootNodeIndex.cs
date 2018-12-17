using BaseNode;
using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Index for the root node of the node tree.
    /// </summary>
    public interface IWriteableRootNodeIndex : IReadOnlyRootNodeIndex, IWriteableNodeIndex
    {
    }

    /// <summary>
    /// Index for the root node of the node tree.
    /// </summary>
    public class WriteableRootNodeIndex : ReadOnlyRootNodeIndex, IWriteableRootNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableRootNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed root node.</param>
        public WriteableRootNodeIndex(INode node)
            : base(node)
        {
        }
        #endregion
    }
}
