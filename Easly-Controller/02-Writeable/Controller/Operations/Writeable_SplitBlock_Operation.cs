using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Operation details for splitting a block in a block list.
    /// </summary>
    public interface IWriteableSplitBlockOperation : IWriteableOperation
    {
        /// <summary>
        /// Inner where the block is split.
        /// </summary>
        IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the last node to stay in the old block.
        /// </summary>
        IWriteableBrowsingExistingBlockNodeIndex NodeIndex { get; }

        /// <summary>
        /// Index of the block split.
        /// </summary>
        int BlockIndex { get; }

        /// <summary>
        /// Index of the last node to remain in the old block.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Block state inserted.
        /// </summary>
        IWriteableBlockState BlockState { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="blockState">Block state inserted.</param>
        void Update(IWriteableBlockState blockState);
    }

    /// <summary>
    /// Operation details for splitting a block in a block list.
    /// </summary>
    public class WriteableSplitBlockOperation : WriteableOperation, IWriteableSplitBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableSplitBlockOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block is split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        public WriteableSplitBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
            : base()
        {
            Inner = inner;
            NodeIndex = nodeIndex;
            BlockIndex = nodeIndex.BlockIndex;
            Index = nodeIndex.Index;
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="blockState">Block state inserted.</param>
        public virtual void Update(IWriteableBlockState blockState)
        {
            Debug.Assert(blockState != null);

            BlockState = blockState;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the block is split.
        /// </summary>
        public IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the last node to stay in the old block.
        /// </summary>
        public IWriteableBrowsingExistingBlockNodeIndex NodeIndex { get; }

        /// <summary>
        /// Block state inserted.
        /// </summary>
        public IWriteableBlockState BlockState { get; private set; }

        /// <summary>
        /// Index of the block split.
        /// </summary>
        public int BlockIndex { get; }

        /// <summary>
        /// Index of the last node to remain in the old block.
        /// </summary>
        public int Index { get; }
        #endregion
    }
}
