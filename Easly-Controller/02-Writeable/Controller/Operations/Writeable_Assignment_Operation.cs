using System;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Operation details for assigning or unassigning a node.
    /// </summary>
    public interface IWriteableAssignmentOperation : IWriteableOperation
    {
        /// <summary>
        /// Inner where the assignment is taking place.
        /// </summary>
        IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> Inner { get; }

        /// <summary>
        /// Position of the assigned or unassigned node.
        /// </summary>
        IWriteableBrowsingOptionalNodeIndex NodeIndex { get; }

        /// <summary>
        /// The modified state.
        /// </summary>
        IWriteableOptionalNodeState State { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="state">The modified state.</param>
        void Update(IWriteableOptionalNodeState state);

        /// <summary>
        /// Creates an operation to undo the assignment operation.
        /// </summary>
        IWriteableAssignmentOperation ToInverseAssignment();
    }

    /// <summary>
    /// Operation details for assigning or unassigning a node.
    /// </summary>
    public class WriteableAssignmentOperation : WriteableOperation, IWriteableAssignmentOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableAssignmentOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the assignment is taking place.</param>
        /// <param name="nodeIndex">Position of the assigned or unassigned node.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableAssignmentOperation(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> inner, IWriteableBrowsingOptionalNodeIndex nodeIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            Inner = inner;
            NodeIndex = nodeIndex;
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="state">The new state.</param>
        public virtual void Update(IWriteableOptionalNodeState state)
        {
            Debug.Assert(state != null);

            State = state;
        }

        /// <summary>
        /// Creates an operation to undo the assignment operation.
        /// </summary>
        public virtual IWriteableAssignmentOperation ToInverseAssignment()
        {
            IWriteableBrowsingOptionalNodeIndex BrowsingIndex = State.ParentIndex;
            return CreateAssignmentOperation(Inner, BrowsingIndex, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the assignment is taking place.
        /// </summary>
        public IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> Inner { get; }

        /// <summary>
        /// Position of the assigned or unassigned node.
        /// </summary>
        public IWriteableBrowsingOptionalNodeIndex NodeIndex { get; }

        /// <summary>
        /// The modified state.
        /// </summary>
        public IWriteableOptionalNodeState State { get; private set; }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxAssignmentOperation object.
        /// </summary>
        protected virtual IWriteableAssignmentOperation CreateAssignmentOperation(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> inner, IWriteableBrowsingOptionalNodeIndex nodeIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableAssignmentOperation));
            return new WriteableAssignmentOperation(inner, nodeIndex, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
