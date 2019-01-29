using BaseNode;
using EaslyController.Writeable;
using System;

namespace EaslyController.Frame
{
    /// <summary>
    /// Operation details for merging blocks in a block list.
    /// </summary>
    public interface IFrameMergeBlocksOperation : IWriteableMergeBlocksOperation, IFrameOperation
    {
        /// <summary>
        /// The merged block state.
        /// </summary>
        new IFrameBlockState BlockState { get; }
    }

    /// <summary>
    /// Operation details for merging blocks in a block list.
    /// </summary>
    public class FrameMergeBlocksOperation : WriteableMergeBlocksOperation, IFrameMergeBlocksOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FrameMergeBlocksOperation"/>.
        /// </summary>
        /// <param name="parentNode">Node where the blocks are merged.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where blocks are merged.</param>
        /// <param name="blockIndex">Position of the merged block.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameMergeBlocksOperation(INode parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The merged block state.
        /// </summary>
        public new IFrameBlockState BlockState { get { return (IFrameBlockState)base.BlockState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxSplitBlockOperation object.
        /// </summary>
        protected override IWriteableSplitBlockOperation CreateSplitBlockOperation(int blockIndex, int index, IBlock block, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameMergeBlocksOperation));
            return new FrameSplitBlockOperation(ParentNode, PropertyName, blockIndex, index, block, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
