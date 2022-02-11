namespace EaslyController.Writeable
{
    using System;
    using BaseNode;
    using Contracts;

    /// <summary>
    /// Operation details for changing a comment.
    /// </summary>
    public class WriteableChangeCommentOperation : WriteableOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableChangeCommentOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the change is taking place.</param>
        /// <param name="text">The new comment.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableChangeCommentOperation(Node parentNode, string text, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            ParentNode = parentNode;
            NewText = text;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Node where the change is taking place.
        /// </summary>
        public Node ParentNode { get; }

        /// <summary>
        /// The old comment.
        /// </summary>
        public string OldText { get; private set; }

        /// <summary>
        /// The new comment.
        /// </summary>
        public string NewText { get; }

        /// <summary>
        /// State changed.
        /// </summary>
        public IWriteableNodeState State { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="state">State changed.</param>
        /// <param name="oldText">The old comment.</param>
        public virtual void Update(IWriteableNodeState state, string oldText)
        {
            Contract.RequireNotNull(state, out IWriteableNodeState State);

            this.State = State;
            OldText = oldText;
        }

        /// <summary>
        /// Creates an operation to undo the change commment operation.
        /// </summary>
        public virtual WriteableChangeCommentOperation ToInverseChange()
        {
            return CreateChangeCommentOperation(OldText, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxChangeCommentOperation object.
        /// </summary>
        private protected virtual WriteableChangeCommentOperation CreateChangeCommentOperation(string text, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableChangeCommentOperation));
            return new WriteableChangeCommentOperation(ParentNode, text, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
