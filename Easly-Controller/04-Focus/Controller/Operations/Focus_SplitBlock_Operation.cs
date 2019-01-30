namespace EaslyController.Focus
{
    using System;
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// Operation details for splitting a block in a block list.
    /// </summary>
    public interface IFocusSplitBlockOperation : IFrameSplitBlockOperation, IFocusOperation
    {
        /// <summary>
        /// The inserted block state.
        /// </summary>
        new IFocusBlockState BlockState { get; }
    }

    /// <summary>
    /// Operation details for splitting a block in a block list.
    /// </summary>
    public class FocusSplitBlockOperation : FrameSplitBlockOperation, IFocusSplitBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusSplitBlockOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the block is split.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where the block is split.</param>
        /// <param name="blockIndex">Position of the split block.</param>
        /// <param name="index">Position of the last node to stay in the old block.</param>
        /// <param name="newBlock">The inserted block.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusSplitBlockOperation(INode parentNode, string propertyName, int blockIndex, int index, IBlock newBlock, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, index, newBlock, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The inserted block state.
        /// </summary>
        public new IFocusBlockState BlockState { get { return (IFocusBlockState)base.BlockState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxxMergeBlocksOperation object.
        /// </summary>
        private protected override IWriteableMergeBlocksOperation CreateMergeBlocksOperation(int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSplitBlockOperation));
            return new FocusMergeBlocksOperation(ParentNode, PropertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
