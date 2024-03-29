﻿namespace EaslyController.Layout
{
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <summary>
    /// Operation details for removing a block from a block list.
    /// </summary>
    public interface ILayoutRemoveBlockOperation : IFocusRemoveBlockOperation, ILayoutRemoveOperation
    {
        /// <summary>
        /// The removed block state.
        /// </summary>
        new ILayoutBlockState BlockState { get; }

        /// <summary>
        /// The removed state.
        /// </summary>
        new ILayoutNodeState RemovedState { get; }
    }

    /// <summary>
    /// Operation details for removing a block from a block list.
    /// </summary>
    internal class LayoutRemoveBlockOperation : FocusRemoveBlockOperation, ILayoutRemoveBlockOperation
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
        public LayoutRemoveBlockOperation(Node parentNode, string propertyName, int blockIndex, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
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
        private protected override IWriteableInsertBlockOperation CreateInsertBlockOperation(int blockIndex, IBlock block, Node node, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutRemoveBlockOperation>());
            return new LayoutInsertBlockOperation(ParentNode, PropertyName, blockIndex, block, node, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
