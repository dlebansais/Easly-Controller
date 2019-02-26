namespace EaslyController.Focus
{
    using System;
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// Operation details for changing a comment.
    /// </summary>
    public interface IFocusChangeCommentOperation : IFrameChangeCommentOperation, IFocusOperation
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
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusChangeCommentOperation(INode parentNode, string text, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, text, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// State changed.
        /// </summary>
        public new IFocusNodeState State { get { return (IFocusNodeState)base.State; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxChangeCommentOperation object.
        /// </summary>
        private protected override IWriteableChangeCommentOperation CreateChangeCommentOperation(string text, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusChangeCommentOperation));
            return new FocusChangeCommentOperation(ParentNode, text, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
