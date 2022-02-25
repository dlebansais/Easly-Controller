namespace EaslyController.Focus
{
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <summary>
    /// Operation details for moving a node in a list or block list.
    /// </summary>
    internal class FocusMoveNodeOperation : FrameMoveNodeOperation, IFocusOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusMoveNodeOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the node is moved.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where the node is moved.</param>
        /// <param name="blockIndex">Block position where the node is moved, if applicable.</param>
        /// <param name="index">The current position before move.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusMoveNodeOperation(Node parentNode, string propertyName, int blockIndex, int index, int direction, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, index, direction, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// State moved.
        /// </summary>
        public new IFocusPlaceholderNodeState State { get { return (IFocusPlaceholderNodeState)base.State; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxxMoveNodeOperation object.
        /// </summary>
        private protected override WriteableMoveNodeOperation CreateMoveNodeOperation(int blockIndex, int index, int direction, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusMoveNodeOperation>());
            return new FocusMoveNodeOperation(ParentNode, PropertyName, blockIndex, index, direction, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
