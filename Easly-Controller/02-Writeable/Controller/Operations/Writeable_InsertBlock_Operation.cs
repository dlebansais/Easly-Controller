using System;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Operation details for inserting a block with a single node in a block list.
    /// </summary>
    public interface IWriteableInsertBlockOperation : IWriteableInsertOperation
    {
        /// <summary>
        /// Inner where the block insertion is taking place.
        /// </summary>
        IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the inserted block.
        /// </summary>
        IWriteableInsertionNewBlockNodeIndex BlockIndex { get; }

        /// <summary>
        /// Index of the state after it's inserted.
        /// </summary>
        IWriteableBrowsingExistingBlockNodeIndex BrowsingIndex { get; }

        /// <summary>
        /// Block state inserted.
        /// </summary>
        IWriteableBlockState BlockState { get; }

        /// <summary>
        /// State inserted.
        /// </summary>
        IWriteablePlaceholderNodeState ChildState { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="browsingIndex">Index of the state after it's inserted.</param>
        /// <param name="blockState">Block state inserted.</param>
        /// <param name="childState">State inserted.</param>
        void Update(IWriteableBrowsingExistingBlockNodeIndex browsingIndex, IWriteableBlockState blockState, IWriteablePlaceholderNodeState childState);
    }

    /// <summary>
    /// Operation details for inserting a block with a single node in a block list.
    /// </summary>
    public class WriteableInsertBlockOperation : WriteableInsertOperation, IWriteableInsertBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableInsertBlockOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block insertion is taking place.</param>
        /// <param name="blockIndex">Index of the inserted block.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableInsertBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableInsertionNewBlockNodeIndex blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            Inner = inner;
            BlockIndex = blockIndex;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the block insertion is taking place.
        /// </summary>
        public IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the inserted block.
        /// </summary>
        public IWriteableInsertionNewBlockNodeIndex BlockIndex { get; }

        /// <summary>
        /// Index of the state after it's inserted.
        /// </summary>
        public IWriteableBrowsingExistingBlockNodeIndex BrowsingIndex { get; private set; }

        /// <summary>
        /// Block state inserted.
        /// </summary>
        public IWriteableBlockState BlockState { get; private set; }

        /// <summary>
        /// State inserted.
        /// </summary>
        public IWriteablePlaceholderNodeState ChildState { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="browsingIndex">Index of the state after it's inserted.</param>
        /// <param name="blockState">Block state inserted.</param>
        /// <param name="childState">State inserted.</param>
        public virtual void Update(IWriteableBrowsingExistingBlockNodeIndex browsingIndex, IWriteableBlockState blockState, IWriteablePlaceholderNodeState childState)
        {
            Debug.Assert(browsingIndex != null);
            Debug.Assert(blockState != null);
            Debug.Assert(childState != null);

            BrowsingIndex = browsingIndex;
            BlockState = blockState;
            ChildState = childState;
        }
        #endregion
    }
}
