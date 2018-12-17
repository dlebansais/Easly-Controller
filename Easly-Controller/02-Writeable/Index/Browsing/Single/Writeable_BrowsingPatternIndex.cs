using BaseNode;
using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Index for the replication pattern node of a block.
    /// </summary>
    public interface IWriteableBrowsingPatternIndex : IReadOnlyBrowsingPatternIndex, IWriteableBrowsingChildIndex, IWriteableNodeIndex
    {
    }

    /// <summary>
    /// Index for the replication pattern node of a block.
    /// </summary>
    public class WriteableBrowsingPatternIndex : ReadOnlyBrowsingPatternIndex, IWriteableBrowsingPatternIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBrowsingPatternIndex"/> class.
        /// </summary>
        /// <param name="block">The block containing the indexed replication pattern node.</param>
        public WriteableBrowsingPatternIndex(IBlock block)
            : base(block)
        {
        }
        #endregion
    }
}
