namespace EaslyController.Focus
{
    using System;
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// Operation details for removing a block from a block list.
    /// </summary>
    public interface IFocusRemoveBlockOperation : IFrameRemoveBlockOperation, IFocusRemoveOperation
    {
        /// <summary>
        /// The removed block state.
        /// </summary>
        new IFocusBlockState BlockState { get; }

        /// <summary>
        /// The removed state.
        /// </summary>
        new IFocusNodeState RemovedState { get; }
    }

    /// <summary>
    /// Operation details for removing a block from a block list.
    /// </summary>
    public class FocusRemoveBlockOperation : FrameRemoveBlockOperation, IFocusRemoveBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusRemoveBlockOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the block removal is taking place.</param>
        /// <param name="propertyName">Block list property of <paramref name="parentNode"/> where a block is removed.</param>
        /// <param name="blockIndex">index of the removed block.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusRemoveBlockOperation(INode parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The removed block state.
        /// </summary>
        public new IFocusBlockState BlockState { get { return (IFocusBlockState)base.BlockState; } }

        /// <summary>
        /// The removed state.
        /// </summary>
        public new IFocusNodeState RemovedState { get { return (IFocusNodeState)base.RemovedState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertBlockOperation object.
        /// </summary>
        protected override IWriteableInsertBlockOperation CreateInsertBlockOperation(int blockIndex, IBlock block, INode node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusRemoveBlockOperation));
            return new FocusInsertBlockOperation(ParentNode, PropertyName, blockIndex, block, node, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
