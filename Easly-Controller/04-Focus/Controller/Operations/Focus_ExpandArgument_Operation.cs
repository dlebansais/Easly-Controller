using EaslyController.Frame;
using EaslyController.Writeable;
using System;

namespace EaslyController.Focus
{
    /// <summary>
    /// Operation details for inserting a single argument in a block list.
    /// </summary>
    public interface IFocusExpandArgumentOperation : IFrameExpandArgumentOperation, IFocusInsertBlockOperation
    {
        /// <summary>
        /// Inner where the block insertion is taking place.
        /// </summary>
        new IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the inserted block.
        /// </summary>
        new IFocusInsertionNewBlockNodeIndex BlockIndex { get; }

        /// <summary>
        /// Index of the state after it's inserted.
        /// </summary>
        new IFocusBrowsingExistingBlockNodeIndex BrowsingIndex { get; }

        /// <summary>
        /// Block state inserted.
        /// </summary>
        new IFocusBlockState BlockState { get; }

        /// <summary>
        /// State inserted.
        /// </summary>
        new IFocusPlaceholderNodeState ChildState { get; }
    }

    /// <summary>
    /// Operation details for inserting a single argument in a block list.
    /// </summary>
    public class FocusExpandArgumentOperation : FrameExpandArgumentOperation, IFocusExpandArgumentOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FocusExpandArgumentOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block insertion is taking place.</param>
        /// <param name="blockIndex">Position where the block is inserted.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusExpandArgumentOperation(IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> inner, IFocusInsertionNewBlockNodeIndex blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(inner, blockIndex, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the block insertion is taking place.
        /// </summary>
        public new IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> Inner { get { return (IFocusBlockListInner<IFocusBrowsingBlockNodeIndex>)base.Inner; } }

        /// <summary>
        /// Index of the inserted block.
        /// </summary>
        public new IFocusInsertionNewBlockNodeIndex BlockIndex { get { return (IFocusInsertionNewBlockNodeIndex)base.BlockIndex; } }

        /// <summary>
        /// Index of the state after it's inserted.
        /// </summary>
        public new IFocusBrowsingExistingBlockNodeIndex BrowsingIndex { get { return (IFocusBrowsingExistingBlockNodeIndex)base.BrowsingIndex; } }

        /// <summary>
        /// Block state inserted.
        /// </summary>
        public new IFocusBlockState BlockState { get { return (IFocusBlockState)base.BlockState; } }

        /// <summary>
        /// State inserted.
        /// </summary>
        public new IFocusPlaceholderNodeState ChildState { get { return (IFocusPlaceholderNodeState)base.ChildState; } }
        #endregion
    }
}
