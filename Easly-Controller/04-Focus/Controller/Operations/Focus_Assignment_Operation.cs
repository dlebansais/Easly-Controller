using EaslyController.Frame;
using EaslyController.Writeable;
using System;

namespace EaslyController.Focus
{
    /// <summary>
    /// Operation details for assigning or unassigning a node.
    /// </summary>
    public interface IFocusAssignmentOperation : IFrameAssignmentOperation, IFocusOperation
    {
        /// <summary>
        /// Inner where the assignment is taking place.
        /// </summary>
        new IFocusOptionalInner<IFocusBrowsingOptionalNodeIndex> Inner { get; }

        /// <summary>
        /// Position of the assigned or unassigned node.
        /// </summary>
        new IFocusBrowsingOptionalNodeIndex NodeIndex { get; }

        /// <summary>
        /// The modified state.
        /// </summary>
        new IFocusOptionalNodeState State { get; }
    }

    /// <summary>
    /// Operation details for assigning or unassigning a node.
    /// </summary>
    public class FocusAssignmentOperation : FrameAssignmentOperation, IFocusAssignmentOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FocusAssignmentOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the assignment is taking place.</param>
        /// <param name="nodeIndex">Position of the assigned or unassigned node.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusAssignmentOperation(IFocusOptionalInner<IFocusBrowsingOptionalNodeIndex> inner, IFocusBrowsingOptionalNodeIndex nodeIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(inner, nodeIndex, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the assignment is taking place.
        /// </summary>
        public new IFocusOptionalInner<IFocusBrowsingOptionalNodeIndex> Inner { get { return (IFocusOptionalInner<IFocusBrowsingOptionalNodeIndex>)base.Inner; } }

        /// <summary>
        /// Position of the assigned or unassigned node.
        /// </summary>
        public new IFocusBrowsingOptionalNodeIndex NodeIndex { get { return (IFocusBrowsingOptionalNodeIndex)base.NodeIndex; } }

        /// <summary>
        /// The modified state.
        /// </summary>
        public new IFocusOptionalNodeState State { get { return (IFocusOptionalNodeState)base.State; } }
        #endregion
    }
}
