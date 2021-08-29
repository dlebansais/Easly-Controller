namespace EaslyController.Layout
{
    using System;
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <summary>
    /// Operation details for removing a block from a block list.
    /// </summary>
    public interface ILayoutRemoveBlockViewOperation : IFocusRemoveBlockViewOperation, ILayoutRemoveOperation
    {
        /// <summary>
        /// Block state removed.
        /// </summary>
        new ILayoutBlockState BlockState { get; }
    }

    /// <summary>
    /// Operation details for removing a block from a block list.
    /// </summary>
    internal class LayoutRemoveBlockViewOperation : FocusRemoveBlockViewOperation, ILayoutRemoveBlockViewOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutRemoveBlockViewOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the block removal is taking place.</param>
        /// <param name="propertyName">Block list property of <paramref name="parentNode"/> where a block is removed.</param>
        /// <param name="blockIndex">index of the removed block.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public LayoutRemoveBlockViewOperation(Node parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Block state removed.
        /// </summary>
        public new ILayoutBlockState BlockState { get { return (ILayoutBlockState)base.BlockState; } }
        #endregion
    }
}
