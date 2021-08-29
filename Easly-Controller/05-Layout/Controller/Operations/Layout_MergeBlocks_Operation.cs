namespace EaslyController.Layout
{
    using System;
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <summary>
    /// Operation details for merging blocks in a block list.
    /// </summary>
    public interface ILayoutMergeBlocksOperation : IFocusMergeBlocksOperation, ILayoutOperation
    {
        /// <summary>
        /// The merged block state.
        /// </summary>
        new ILayoutBlockState BlockState { get; }
    }

    /// <summary>
    /// Operation details for merging blocks in a block list.
    /// </summary>
    internal class LayoutMergeBlocksOperation : FocusMergeBlocksOperation, ILayoutMergeBlocksOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutMergeBlocksOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the blocks are merged.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where blocks are merged.</param>
        /// <param name="blockIndex">Position of the merged block.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public LayoutMergeBlocksOperation(Node parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The merged block state.
        /// </summary>
        public new ILayoutBlockState BlockState { get { return (ILayoutBlockState)base.BlockState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxSplitBlockOperation object.
        /// </summary>
        private protected override IWriteableSplitBlockOperation CreateSplitBlockOperation(int blockIndex, int index, IBlock block, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutMergeBlocksOperation));
            return new LayoutSplitBlockOperation(ParentNode, PropertyName, blockIndex, index, block, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
