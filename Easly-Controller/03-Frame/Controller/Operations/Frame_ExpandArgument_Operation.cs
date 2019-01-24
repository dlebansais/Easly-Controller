using EaslyController.Writeable;
using System;

namespace EaslyController.Frame
{
    /// <summary>
    /// Operation details for inserting a single argument in a block list.
    /// </summary>
    public interface IFrameExpandArgumentOperation : IWriteableExpandArgumentOperation, IFrameInsertBlockOperation
    {
        /// <summary>
        /// Inner where the block insertion is taking place.
        /// </summary>
        new IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the inserted block.
        /// </summary>
        new IFrameInsertionNewBlockNodeIndex BlockIndex { get; }

        /// <summary>
        /// Index of the state after it's inserted.
        /// </summary>
        new IFrameBrowsingExistingBlockNodeIndex BrowsingIndex { get; }

        /// <summary>
        /// Block state inserted.
        /// </summary>
        new IFrameBlockState BlockState { get; }

        /// <summary>
        /// State inserted.
        /// </summary>
        new IFramePlaceholderNodeState ChildState { get; }
    }

    /// <summary>
    /// Operation details for inserting a single argument in a block list.
    /// </summary>
    public class FrameExpandArgumentOperation : WriteableExpandArgumentOperation, IFrameExpandArgumentOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FrameExpandArgumentOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block insertion is taking place.</param>
        /// <param name="blockIndex">Position where the block is inserted.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameExpandArgumentOperation(IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> inner, IFrameInsertionNewBlockNodeIndex blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(inner, blockIndex, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the block insertion is taking place.
        /// </summary>
        public new IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> Inner { get { return (IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)base.Inner; } }

        /// <summary>
        /// Index of the inserted block.
        /// </summary>
        public new IFrameInsertionNewBlockNodeIndex BlockIndex { get { return (IFrameInsertionNewBlockNodeIndex)base.BlockIndex; } }

        /// <summary>
        /// Index of the state after it's inserted.
        /// </summary>
        public new IFrameBrowsingExistingBlockNodeIndex BrowsingIndex { get { return (IFrameBrowsingExistingBlockNodeIndex)base.BrowsingIndex; } }

        /// <summary>
        /// Block state inserted.
        /// </summary>
        public new IFrameBlockState BlockState { get { return (IFrameBlockState)base.BlockState; } }

        /// <summary>
        /// State inserted.
        /// </summary>
        public new IFramePlaceholderNodeState ChildState { get { return (IFramePlaceholderNodeState)base.ChildState; } }
        #endregion
    }
}
