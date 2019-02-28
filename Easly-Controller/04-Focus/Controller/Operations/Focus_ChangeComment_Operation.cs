namespace EaslyController.Focus
{
    using System;
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// Operation details for changing a comment.
    /// </summary>
    public interface IFocusChangeCommentOperation : IFrameChangeCommentOperation, IFocusChangeCaretOperation, IFocusOperation
    {
        /// <summary>
        /// State changed.
        /// </summary>
        new IFocusNodeState State { get; }
    }

    /// <summary>
    /// Operation details for changing a comment.
    /// </summary>
    internal class FocusChangeCommentOperation : FrameChangeCommentOperation, IFocusChangeCommentOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusChangeCommentOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the change is taking place.</param>
        /// <param name="text">The new comment.</param>
        /// <param name="oldCaretPosition">The old caret position.</param>
        /// <param name="newCaretPosition">The new caret position.</param>
        /// <param name="changeCaretBeforeText">True if the caret should be changed before the text, to preserve the caret invariant.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusChangeCommentOperation(INode parentNode, string text, int oldCaretPosition, int newCaretPosition, bool changeCaretBeforeText, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, text, handlerRedo, handlerUndo, isNested)
        {
            OldCaretPosition = oldCaretPosition;
            NewCaretPosition = newCaretPosition;
            ChangeCaretBeforeText = changeCaretBeforeText;
        }
        #endregion

        #region Properties
        /// <summary>
        /// State changed.
        /// </summary>
        public new IFocusNodeState State { get { return (IFocusNodeState)base.State; } }

        /// <summary>
        /// The old caret position.
        /// </summary>
        public int OldCaretPosition { get; }

        /// <summary>
        /// The new caret position.
        /// </summary>
        public int NewCaretPosition { get; }

        /// <summary>
        /// True if the caret should be changed before the text, to preserve the caret invariant.
        /// </summary>
        public bool ChangeCaretBeforeText { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Creates an operation to undo the change commment operation.
        /// </summary>
        public override IWriteableChangeCommentOperation ToInverseChange()
        {
            return CreateChangeCommentOperation(OldText, NewCaretPosition, OldCaretPosition, !ChangeCaretBeforeText, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxChangeCommentOperation object.
        /// </summary>
        private protected override IWriteableChangeCommentOperation CreateChangeCommentOperation(string text, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            return CreateChangeCommentOperation(text, -1, -1, false, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeCommentOperation object.
        /// </summary>
        private protected virtual IFocusChangeCommentOperation CreateChangeCommentOperation(string text, int oldCaretPosition, int newCaretPosition, bool changeCaretBeforeText, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusChangeCommentOperation));
            return new FocusChangeCommentOperation(ParentNode, text, oldCaretPosition, newCaretPosition, changeCaretBeforeText, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
