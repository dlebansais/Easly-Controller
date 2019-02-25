namespace EaslyController.Writeable
{
    using System;
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// Operation details for changing a discrete value.
    /// </summary>
    public interface IWriteableChangeDiscreteValueOperation : IWriteableOperation
    {
        /// <summary>
        /// Node where the change is taking place.
        /// </summary>
        INode ParentNode { get; }

        /// <summary>
        /// Name of the property to change.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// The old value.
        /// </summary>
        int OldValue { get; }

        /// <summary>
        /// The new value.
        /// </summary>
        int NewValue { get; }

        /// <summary>
        /// State changed.
        /// </summary>
        IWriteableNodeState State { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="state">State changed.</param>
        /// <param name="oldValue">The old value.</param>
        void Update(IWriteableNodeState state, int oldValue);

        /// <summary>
        /// Creates an operation to undo the change value operation.
        /// </summary>
        IWriteableChangeDiscreteValueOperation ToInverseChange();
    }

    /// <summary>
    /// Operation details for changing a discrete value.
    /// </summary>
    internal class WriteableChangeDiscreteValueOperation : WriteableOperation, IWriteableChangeDiscreteValueOperation
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
        public WriteableChangeDiscreteValueOperation(INode parentNode, string propertyName, int value, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
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
        public INode ParentNode { get; }

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
        public virtual IWriteableChangeDiscreteValueOperation ToInverseChange()
        {
            return CreateChangeDiscreteValueOperation(OldValue, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxChangeDiscreteValueOperation object.
        /// </summary>
        private protected virtual IWriteableChangeDiscreteValueOperation CreateChangeDiscreteValueOperation(int value, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableChangeDiscreteValueOperation));
            return new WriteableChangeDiscreteValueOperation(ParentNode, PropertyName, value, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
