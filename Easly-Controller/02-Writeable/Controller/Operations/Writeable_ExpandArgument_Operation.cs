using System;
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
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableExpandArgumentOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableInsertionNewBlockNodeIndex blockIndex, Action<IWriteableOperation> handlerRedo, bool isNested)
            : base(inner, blockIndex, handlerRedo, isNested)
        {
            Debug.Assert(blockIndex.BlockIndex == 0);
        }
        #endregion
    }
}
