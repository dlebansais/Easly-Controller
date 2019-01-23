using System;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Operation details for merging blocks in a block list.
    /// </summary>
    public interface IWriteableMergeBlocksOperation : IWriteableOperation
    {
        /// <summary>
        /// Inner where blocks are merged.
        /// </summary>
        IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the first node in the merged block.
        /// </summary>
        IWriteableBrowsingExistingBlockNodeIndex NodeIndex { get; }

        /// <summary>
        /// Index of the block merged.
        /// </summary>
        int BlockIndex { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        void Update();
    }

    /// <summary>
    /// Operation details for merging blocks in a block list.
    /// </summary>
    public class WriteableMergeBlocksOperation : WriteableOperation, IWriteableMergeBlocksOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableMergeBlocksOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block is split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableMergeBlocksOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex, Action<IWriteableOperation> handlerRedo, bool isNested)
            : base(handlerRedo, isNested)
        {
            Inner = inner;
            NodeIndex = nodeIndex;
            BlockIndex = nodeIndex.BlockIndex;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where blocks are merged.
        /// </summary>
        public IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the first node in the merged block.
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
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        public virtual void Update()
        {
        }
        #endregion
    }
}
