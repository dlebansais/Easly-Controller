namespace EaslyController.Layout
{
    using System;
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <summary>
    /// Operation details for moving a node in a list or block list.
    /// </summary>
    public interface ILayoutMoveNodeOperation : IFocusMoveNodeOperation, ILayoutOperation
    {
        /// <summary>
        /// State moved.
        /// </summary>
        new ILayoutPlaceholderNodeState State { get; }
    }

    /// <summary>
    /// Operation details for moving a node in a list or block list.
    /// </summary>
    internal class LayoutMoveNodeOperation : FocusMoveNodeOperation, ILayoutMoveNodeOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutMoveNodeOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the node is moved.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where the node is moved.</param>
        /// <param name="blockIndex">Block position where the node is moved, if applicable.</param>
        /// <param name="index">The current position before move.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public LayoutMoveNodeOperation(INode parentNode, string propertyName, int blockIndex, int index, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, index, direction, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// State moved.
        /// </summary>
        public new ILayoutPlaceholderNodeState State { get { return (ILayoutPlaceholderNodeState)base.State; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxxMoveNodeOperation object.
        /// </summary>
        private protected override IWriteableMoveNodeOperation CreateMoveNodeOperation(int blockIndex, int index, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutMoveNodeOperation));
            return new LayoutMoveNodeOperation(ParentNode, PropertyName, blockIndex, index, direction, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
