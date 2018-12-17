using BaseNode;
using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Index for the source identifier node of a block.
    /// </summary>
    public interface IWriteableBrowsingSourceIndex : IReadOnlyBrowsingSourceIndex, IWriteableBrowsingChildIndex, IWriteableNodeIndex
    {
    }

    /// <summary>
    /// Index for the source identifier node of a block.
    /// </summary>
    public class WriteableBrowsingSourceIndex : ReadOnlyBrowsingSourceIndex, IWriteableBrowsingSourceIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBrowsingSourceIndex"/> class.
        /// </summary>
        /// <param name="block">The block containing the indexed source identifier node.</param>
        public WriteableBrowsingSourceIndex(IBlock block)
            : base(block)
        {
        }
        #endregion
    }
}
