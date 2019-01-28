using System;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Operation details for changing a node.
    /// </summary>
    public interface IWriteableChangeNodeOperation : IWriteableOperation
    {
        /// <summary>
        /// Index of the changed node.
        /// </summary>
        IWriteableIndex NodeIndex { get; }

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
        IWriteableChangeNodeOperation ToInverseChange();
    }

    /// <summary>
    /// Operation details for changing a node.
    /// </summary>
    public class WriteableChangeNodeOperation : WriteableOperation, IWriteableChangeNodeOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableChangeNodeOperation"/>.
        /// </summary>
        /// <param name="nodeIndex">Index of the changed node.</param>
        /// <param name="propertyName">Name of the property to change.</param>
        /// <param name="value">The new value.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableChangeNodeOperation(IWriteableIndex nodeIndex, string propertyName, int value, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            NodeIndex = nodeIndex;
            PropertyName = propertyName;
            NewValue = value;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Index of the changed node.
        /// </summary>
        public IWriteableIndex NodeIndex { get; }

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
        public virtual IWriteableChangeNodeOperation ToInverseChange()
        {
            return CreateChangeNodeOperation(NodeIndex, PropertyName, OldValue, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxChangeNodeOperation object.
        /// </summary>
        protected virtual IWriteableChangeNodeOperation CreateChangeNodeOperation(IWriteableIndex nodeIndex, string propertyName, int value, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableChangeNodeOperation));
            return new WriteableChangeNodeOperation(nodeIndex, propertyName, value, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
