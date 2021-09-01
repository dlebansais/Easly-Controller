namespace EaslyController.Writeable
{
    using System;
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// Operation details for changing a discrete value.
    /// </summary>
    public class WriteableChangeDiscreteValueOperation : WriteableOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableChangeDiscreteValueOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the change is taking place.</param>
        /// <param name="propertyName">Name of the property to change.</param>
        /// <param name="value">The new value.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableChangeDiscreteValueOperation(Node parentNode, string propertyName, int value, Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            ParentNode = parentNode;
            PropertyName = propertyName;
            NewValue = value;
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
        /// The old value.
        /// </summary>
        public int OldValue { get; private set; }

        /// <summary>
        /// The new value.
        /// </summary>
        public int NewValue { get; }

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
        /// <param name="oldValue">The old value.</param>
        public virtual void Update(IWriteableNodeState state, int oldValue)
        {
            Debug.Assert(state != null);

            State = state;
            OldValue = oldValue;
        }

        /// <summary>
        /// Creates an operation to undo the change value operation.
        /// </summary>
        public virtual WriteableChangeDiscreteValueOperation ToInverseChange()
        {
            return CreateChangeDiscreteValueOperation(OldValue, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxChangeDiscreteValueOperation object.
        /// </summary>
        private protected virtual WriteableChangeDiscreteValueOperation CreateChangeDiscreteValueOperation(int value, Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableChangeDiscreteValueOperation));
            return new WriteableChangeDiscreteValueOperation(ParentNode, PropertyName, value, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
