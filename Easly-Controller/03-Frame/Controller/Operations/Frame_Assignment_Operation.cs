namespace EaslyController.Frame
{
    using BaseNode;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <inheritdoc/>
    internal class FrameAssignmentOperation : WriteableAssignmentOperation, IFrameOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameAssignmentOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the assignment is taking place.</param>
        /// <param name="propertyName">Optional property of <paramref name="parentNode"/> for which assignment is changed.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameAssignmentOperation(Node parentNode, string propertyName, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The modified state.
        /// </summary>
        public new IFrameOptionalNodeState State { get { return (IFrameOptionalNodeState)base.State; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxAssignmentOperation object.
        /// </summary>
        private protected override WriteableAssignmentOperation CreateAssignmentOperation(System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameAssignmentOperation>());
            return new FrameAssignmentOperation(ParentNode, PropertyName, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
