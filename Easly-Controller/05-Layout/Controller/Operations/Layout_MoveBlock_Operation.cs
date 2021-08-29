namespace EaslyController.Layout
{
    using System;
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <summary>
    /// Operation details for moving a block in a block list.
    /// </summary>
    public interface ILayoutMoveBlockOperation : IFocusMoveBlockOperation, ILayoutOperation
    {
        /// <summary>
        /// The moved block state.
        /// </summary>
        new ILayoutBlockState BlockState { get; }
    }

    /// <summary>
    /// Operation details for moving a block in a block list.
    /// </summary>
    internal class LayoutMoveBlockOperation : FocusMoveBlockOperation, ILayoutMoveBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutMoveBlockOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the block is moved.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where the block is moved.</param>
        /// <param name="blockIndex">Index of the moved block.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public LayoutMoveBlockOperation(Node parentNode, string propertyName, int blockIndex, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, direction, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The moved block state.
        /// </summary>
        public new ILayoutBlockState BlockState { get { return (ILayoutBlockState)base.BlockState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxxMoveBlockOperation object.
        /// </summary>
        private protected override IWriteableMoveBlockOperation CreateMoveBlockOperation(int blockIndex, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutMoveBlockOperation));
            return new LayoutMoveBlockOperation(ParentNode, PropertyName, blockIndex, direction, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
