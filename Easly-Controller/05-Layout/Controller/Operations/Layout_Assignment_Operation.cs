namespace EaslyController.Layout
{
    using System;
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <summary>
    /// Operation details for assigning or unassigning a node.
    /// </summary>
    internal class LayoutAssignmentOperation : FocusAssignmentOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutAssignmentOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the assignment is taking place.</param>
        /// <param name="propertyName">Optional property of <paramref name="parentNode"/> for which assignment is changed.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public LayoutAssignmentOperation(Node parentNode, string propertyName, Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The modified state.
        /// </summary>
        public new ILayoutOptionalNodeState State { get { return (ILayoutOptionalNodeState)base.State; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxAssignmentOperation object.
        /// </summary>
        private protected override WriteableAssignmentOperation CreateAssignmentOperation(Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutAssignmentOperation));
            return new LayoutAssignmentOperation(ParentNode, PropertyName, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
