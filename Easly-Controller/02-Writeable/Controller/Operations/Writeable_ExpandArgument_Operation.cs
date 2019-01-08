using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Operation details for inserting a single argument in a block list.
    /// </summary>
    public interface IWriteableExpandArgumentOperation : IWriteableInsertBlockOperation
    {
    }

    /// <summary>
    /// Operation details for inserting a single argument in a block list.
    /// </summary>
    public class WriteableExpandArgumentOperation : WriteableInsertBlockOperation, IWriteableExpandArgumentOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableExpandArgumentOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block insertion is taking place.</param>
        /// <param name="blockIndex">Position where the block is inserted.</param>
        public WriteableExpandArgumentOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableInsertionNewBlockNodeIndex blockIndex)
            : base(inner, blockIndex)
        {
            Debug.Assert(blockIndex.BlockIndex == 0);
        }
        #endregion
    }
}
