namespace EaslyController.Layout
{
    using System;
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <summary>
    /// Operation details for removing a block from a block list.
    /// </summary>
    internal class LayoutRemoveBlockOperation : FocusRemoveBlockOperation, ILayoutRemoveOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutRemoveBlockOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the block removal is taking place.</param>
        /// <param name="propertyName">Block list property of <paramref name="parentNode"/> where a block is removed.</param>
        /// <param name="blockIndex">index of the removed block.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public LayoutRemoveBlockOperation(Node parentNode, string propertyName, int blockIndex, Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The removed block state.
        /// </summary>
        public new ILayoutBlockState BlockState { get { return (ILayoutBlockState)base.BlockState; } }

        /// <summary>
        /// The removed state.
        /// </summary>
        public new ILayoutNodeState RemovedState { get { return (ILayoutNodeState)base.RemovedState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertBlockOperation object.
        /// </summary>
        private protected override IWriteableInsertBlockOperation CreateInsertBlockOperation(int blockIndex, IBlock block, Node node, Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutRemoveBlockOperation));
            return new LayoutInsertBlockOperation(ParentNode, PropertyName, blockIndex, block, node, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
