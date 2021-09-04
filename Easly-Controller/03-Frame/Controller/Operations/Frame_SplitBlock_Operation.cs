namespace EaslyController.Frame
{
    using System;
    using BaseNode;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    internal class FrameSplitBlockOperation : WriteableSplitBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameSplitBlockOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the block is split.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where the block is split.</param>
        /// <param name="blockIndex">Position of the split block.</param>
        /// <param name="index">Position of the last node to stay in the old block.</param>
        /// <param name="newBlock">The inserted block.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameSplitBlockOperation(Node parentNode, string propertyName, int blockIndex, int index, IBlock newBlock, Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, index, newBlock, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The inserted block state.
        /// </summary>
        public new IFrameBlockState BlockState { get { return (IFrameBlockState)base.BlockState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxxMergeBlocksOperation object.
        /// </summary>
        private protected override WriteableMergeBlocksOperation CreateMergeBlocksOperation(int blockIndex, Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameSplitBlockOperation));
            return new FrameMergeBlocksOperation(ParentNode, PropertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
