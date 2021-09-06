namespace EaslyController.Writeable
{
    using System;
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// Operation details for changing text.
    /// </summary>
    public class WriteableChangeTextOperation : WriteableOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableChangeTextOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the change is taking place.</param>
        /// <param name="propertyName">Name of the property to change.</param>
        /// <param name="text">The new text.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableChangeTextOperation(Node parentNode, string propertyName, string text, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            ParentNode = parentNode;
            PropertyName = propertyName;
            NewText = text;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Node where the change is taking place.
        /// </summary>
        public Node ParentNode { get; }

        /// <summary>
        /// Name of the property to change.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// The old text.
        /// </summary>
        public string OldText { get; private set; }

        /// <summary>
        /// The new text.
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
        /// <param name="oldText">The old text.</param>
        public virtual void Update(IWriteableNodeState state, string oldText)
        {
            Debug.Assert(state != null);

            State = state;
            OldText = oldText;
        }

        /// <summary>
        /// Creates an operation to undo the change text operation.
        /// </summary>
        public virtual WriteableChangeTextOperation ToInverseChange()
        {
            return CreateChangeTextOperation(OldText, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxChangeTextOperation object.
        /// </summary>
        private protected virtual WriteableChangeTextOperation CreateChangeTextOperation(string text, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableChangeTextOperation));
            return new WriteableChangeTextOperation(ParentNode, PropertyName, text, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
