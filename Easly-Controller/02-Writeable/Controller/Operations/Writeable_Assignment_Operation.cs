namespace EaslyController.Writeable
{
    using System;
    using BaseNode;
    using Contracts;

    /// <summary>
    /// Operation details for assigning or unassigning a node.
    /// </summary>
    public class WriteableAssignmentOperation : WriteableOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableAssignmentOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the assignment is taking place.</param>
        /// <param name="propertyName">Optional property of <paramref name="parentNode"/> for which assignment is changed.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableAssignmentOperation(Node parentNode, string propertyName, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            ParentNode = parentNode;
            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Node where the assignment is taking place.
        /// </summary>
        public Node ParentNode { get; }

        /// <summary>
        /// Optional property of <see cref="ParentNode"/> for which assignment is changed.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// The modified state.
        /// </summary>
        public IWriteableOptionalNodeState State { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="state">The new state.</param>
        public virtual void Update(IWriteableOptionalNodeState state)
        {
            Contract.RequireNotNull(state, out IWriteableOptionalNodeState State);

            this.State = State;
        }

        /// <summary>
        /// Creates an operation to undo the assignment operation.
        /// </summary>
        public virtual WriteableAssignmentOperation ToInverseAssignment()
        {
            return CreateAssignmentOperation(HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxAssignmentOperation object.
        /// </summary>
        private protected virtual WriteableAssignmentOperation CreateAssignmentOperation(Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableAssignmentOperation));
            return new WriteableAssignmentOperation(ParentNode, PropertyName, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
