using BaseNode;
using EaslyController.Writeable;
using System;

namespace EaslyController.Frame
{
    /// <summary>
    /// Operation details for assigning or unassigning a node.
    /// </summary>
    public interface IFrameAssignmentOperation : IWriteableAssignmentOperation, IFrameOperation
    {
        /// <summary>
        /// The modified state.
        /// </summary>
        new IFrameOptionalNodeState State { get; }
    }

    /// <summary>
    /// Operation details for assigning or unassigning a node.
    /// </summary>
    public class FrameAssignmentOperation : WriteableAssignmentOperation, IFrameAssignmentOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FrameAssignmentOperation"/>.
        /// </summary>
        /// <param name="parentNode">Node where the assignment is taking place.</param>
        /// <param name="propertyName">Optional property of <paramref name="parentNode"/> for which assignment is changed.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameAssignmentOperation(INode parentNode, string propertyName, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The modified state.
        /// </summary>
        public new IFrameOptionalNodeState State { get { return (IFrameOptionalNodeState)base.State; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxAssignmentOperation object.
        /// </summary>
        protected override IWriteableAssignmentOperation CreateAssignmentOperation(Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameAssignmentOperation));
            return new FrameAssignmentOperation(ParentNode, PropertyName, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
