using BaseNode;
using System;
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
        /// The inserted block.
        /// </summary>
        IBlock NewBlock { get; }

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

        /// <summary>
        /// Creates an operation to undo the split block operation.
        /// </summary>
        IWriteableMergeBlocksOperation ToMergeBlocksOperation();
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
        /// <param name="newBlock">The inserted block.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableSplitBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex, IBlock newBlock, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            Inner = inner;
            NodeIndex = nodeIndex;
            NewBlock = newBlock;
            BlockIndex = nodeIndex.BlockIndex;
            Index = nodeIndex.Index;
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
        /// The inserted block.
        /// </summary>
        public IBlock NewBlock { get; }

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

        /// <summary>
        /// Creates an operation to undo the split block operation.
        /// </summary>
        public virtual IWriteableMergeBlocksOperation ToMergeBlocksOperation()
        {
            return CreateMergeBlocksOperation(Inner, NodeIndex, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxxMergeBlocksOperation object.
        /// </summary>
        protected virtual IWriteableMergeBlocksOperation CreateMergeBlocksOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableSplitBlockOperation));
            return new WriteableMergeBlocksOperation(inner, nodeIndex, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
