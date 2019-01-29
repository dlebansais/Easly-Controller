using BaseNode;
using EaslyController.Frame;
using EaslyController.Writeable;
using System;

namespace EaslyController.Focus
{
    /// <summary>
    /// Operation details for inserting a block with a single node in a block list.
    /// </summary>
    public interface IFocusInsertBlockOperation : IFrameInsertBlockOperation, IFocusInsertOperation
    {
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
    /// Operation details for inserting a block with a single node in a block list.
    /// </summary>
    public class FocusInsertBlockOperation : FrameInsertBlockOperation, IFocusInsertBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FocusInsertBlockOperation"/>.
        /// </summary>
        /// <param name="parentNode">Node where the block insertion is taking place.</param>
        /// <param name="propertyName">Block list property of <paramref name="parentNode"/> where a block is inserted.</param>
        /// <param name="blockIndex">Index of the inserted block.</param>
        /// <param name="block">The inserted block.</param>
        /// <param name="node">The inserted node.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusInsertBlockOperation(INode parentNode, string propertyName, int blockIndex, IBlock block, INode node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, block, node, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
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
