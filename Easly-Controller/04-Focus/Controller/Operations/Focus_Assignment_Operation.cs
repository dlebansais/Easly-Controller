namespace EaslyController.Focus
{
    using System;
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// Operation details for assigning or unassigning a node.
    /// </summary>
    public interface IFocusAssignmentOperation : IFrameAssignmentOperation, IFocusOperation
    {
        /// <summary>
        /// The modified state.
        /// </summary>
        new IFocusOptionalNodeState State { get; }
    }

    /// <summary>
    /// Operation details for assigning or unassigning a node.
    /// </summary>
    public class FocusAssignmentOperation : FrameAssignmentOperation, IFocusAssignmentOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusAssignmentOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the assignment is taking place.</param>
        /// <param name="propertyName">Optional property of <paramref name="parentNode"/> for which assignment is changed.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusAssignmentOperation(INode parentNode, string propertyName, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The modified state.
        /// </summary>
        public new IFocusOptionalNodeState State { get { return (IFocusOptionalNodeState)base.State; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxAssignmentOperation object.
        /// </summary>
        protected override IWriteableAssignmentOperation CreateAssignmentOperation(Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusAssignmentOperation));
            return new FocusAssignmentOperation(ParentNode, PropertyName, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
