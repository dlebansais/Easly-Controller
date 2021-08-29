namespace EaslyController.Frame
{
    using System;
    using BaseNode;
    using EaslyController.Writeable;

    /// <summary>
    /// Operation details for moving a node in a list or block list.
    /// </summary>
    public interface IFrameMoveNodeOperation : IWriteableMoveNodeOperation, IFrameOperation
    {
        /// <summary>
        /// State moved.
        /// </summary>
        new IFramePlaceholderNodeState State { get; }
    }

    /// <summary>
    /// Operation details for moving a node in a list or block list.
    /// </summary>
    internal class FrameMoveNodeOperation : WriteableMoveNodeOperation, IFrameMoveNodeOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameMoveNodeOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the node is moved.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where the node is moved.</param>
        /// <param name="blockIndex">Block position where the node is moved, if applicable.</param>
        /// <param name="index">The current position before move.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameMoveNodeOperation(Node parentNode, string propertyName, int blockIndex, int index, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, index, direction, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// State moved.
        /// </summary>
        public new IFramePlaceholderNodeState State { get { return (IFramePlaceholderNodeState)base.State; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxxMoveNodeOperation object.
        /// </summary>
        private protected override IWriteableMoveNodeOperation CreateMoveNodeOperation(int blockIndex, int index, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameMoveNodeOperation));
            return new FrameMoveNodeOperation(ParentNode, PropertyName, blockIndex, index, direction, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
