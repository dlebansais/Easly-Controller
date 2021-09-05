namespace EaslyController.Layout
{
    using System;
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <summary>
    /// Operation details for changing text.
    /// </summary>
    internal class LayoutChangeTextOperation : FocusChangeTextOperation, ILayoutChangeCaretOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutChangeTextOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the change is taking place.</param>
        /// <param name="propertyName">Name of the property to change.</param>
        /// <param name="text">The new text.</param>
        /// <param name="oldCaretPosition">The old caret position.</param>
        /// <param name="newCaretPosition">The new caret position.</param>
        /// <param name="changeCaretBeforeText">True if the caret should be changed before the text, to preserve the caret invariant.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public LayoutChangeTextOperation(Node parentNode, string propertyName, string text, int oldCaretPosition, int newCaretPosition, bool changeCaretBeforeText, Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, text, oldCaretPosition, newCaretPosition, changeCaretBeforeText, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// State changed.
        /// </summary>
        public new ILayoutNodeState State { get { return (ILayoutNodeState)base.State; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxChangeTextOperation object.
        /// </summary>
        private protected override FocusChangeTextOperation CreateChangeTextOperation(string text, int oldCaretPosition, int newCaretPosition, bool changeCaretBeforeText, Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutChangeTextOperation));
            return new LayoutChangeTextOperation(ParentNode, PropertyName, text, oldCaretPosition, newCaretPosition, changeCaretBeforeText, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
