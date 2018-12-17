using BaseNode;
using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Index for a node in a list of nodes.
    /// </summary>
    public interface IWriteableBrowsingListNodeIndex : IReadOnlyBrowsingListNodeIndex, IWriteableBrowsingCollectionNodeIndex
    {
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
    }
}
