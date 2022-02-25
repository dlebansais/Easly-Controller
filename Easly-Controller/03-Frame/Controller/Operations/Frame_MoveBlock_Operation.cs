namespace EaslyController.Frame
{
    using BaseNode;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <inheritdoc/>
    internal class FrameMoveBlockOperation : WriteableMoveBlockOperation, IFrameOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameMoveBlockOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the block is moved.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where the block is moved.</param>
        /// <param name="blockIndex">Index of the moved block.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameMoveBlockOperation(Node parentNode, string propertyName, int blockIndex, int direction, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, direction, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The moved block state.
        /// </summary>
        public new IFrameBlockState BlockState { get { return (IFrameBlockState)base.BlockState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxxMoveBlockOperation object.
        /// </summary>
        private protected override WriteableMoveBlockOperation CreateMoveBlockOperation(int blockIndex, int direction, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameMoveBlockOperation>());
            return new FrameMoveBlockOperation(ParentNode, PropertyName, blockIndex, direction, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
