using System;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Operation details for removing a block from a block list.
    /// </summary>
    public interface IWriteableRemoveBlockOperation : IWriteableRemoveOperation
    {
        /// <summary>
        /// Inner where the block removal is taking place.
        /// </summary>
        IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the removed block.
        /// </summary>
        IWriteableBrowsingExistingBlockNodeIndex BlockIndex { get; }

        /// <summary>
        /// Block state removed.
        /// </summary>
        IWriteableBlockState BlockState { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="blockState">Block state removed.</param>
        void Update(IWriteableBlockState blockState);

        /// <summary>
        /// Creates an operation to undo the remove block operation.
        /// </summary>
        IWriteableInsertBlockOperation ToInsertBlockOperation();
    }

    /// <summary>
    /// Operation details for removing a block from a block list.
    /// </summary>
    public class WriteableRemoveBlockOperation : WriteableRemoveOperation, IWriteableRemoveBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableRemoveBlockOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block removal is taking place.</param>
        /// <param name="blockIndex">index of the removed block.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableRemoveBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            Inner = inner;
            BlockIndex = blockIndex;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the block removal is taking place.
        /// </summary>
        public IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the removed block.
        /// </summary>
        public IWriteableBrowsingExistingBlockNodeIndex BlockIndex { get; }

        /// <summary>
        /// Block state removed.
        /// </summary>
        public IWriteableBlockState BlockState { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="blockState">Block state removed.</param>
        public virtual void Update(IWriteableBlockState blockState)
        {
            Debug.Assert(blockState != null);

            BlockState = blockState;
        }

        /// <summary>
        /// Creates an operation to undo the remove block operation.
        /// </summary>
        public IWriteableInsertBlockOperation ToInsertBlockOperation()
        {
            IWriteableInsertionNewBlockNodeIndex InsertionIndex = BlockIndex.ToInsertionIndex(Inner.Owner.Node, BlockIndex.Node) as IWriteableInsertionNewBlockNodeIndex;
            Debug.Assert(InsertionIndex != null);

            return CreateInsertBlockOperation(Inner, InsertionIndex, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertBlockOperation object.
        /// </summary>
        protected virtual IWriteableInsertBlockOperation CreateInsertBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableInsertionNewBlockNodeIndex blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableRemoveBlockOperation));
            return new WriteableInsertBlockOperation(inner, blockIndex, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
