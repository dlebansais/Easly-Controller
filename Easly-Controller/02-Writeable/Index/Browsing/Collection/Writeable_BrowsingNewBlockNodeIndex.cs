using BaseNode;
using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Index for the first node in a block.
    /// </summary>
    public interface IWriteableBrowsingNewBlockNodeIndex : IReadOnlyBrowsingNewBlockNodeIndex, IWriteableBrowsingBlockNodeIndex
    {
    }

    /// <summary>
    /// Index for the first node in a block.
    /// </summary>
    public class WriteableBrowsingNewBlockNodeIndex : ReadOnlyBrowsingNewBlockNodeIndex, IWriteableBrowsingNewBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBrowsingNewBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="node">First node in the block.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list.
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="patternNode">Replication pattern in the block.</param>
        /// <param name="sourceNode">Source identifier in the block.</param>
        public WriteableBrowsingNewBlockNodeIndex(INode parentNode, INode node, string propertyName, int blockIndex, IPattern patternNode, IIdentifier sourceNode)
            : base(parentNode, node, propertyName, blockIndex, patternNode, sourceNode)
        {
        }
        #endregion
    }
}
