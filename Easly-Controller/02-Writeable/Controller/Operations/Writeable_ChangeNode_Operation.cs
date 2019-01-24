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
        /// The new value.
        /// </summary>
        int Value { get; }

        /// <summary>
        /// State changed.
        /// </summary>
        IWriteablePlaceholderNodeState State { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="state">State changed.</param>
        void Update(IWriteablePlaceholderNodeState state);
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
            Value = value;
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
        /// The new value.
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// State changed.
        /// </summary>
        public IWriteablePlaceholderNodeState State { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="state">State changed.</param>
        public virtual void Update(IWriteablePlaceholderNodeState state)
        {
            Debug.Assert(state != null);

            State = state;
        }
        #endregion
    }
}
