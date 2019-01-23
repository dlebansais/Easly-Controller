using EaslyController.Writeable;
using System;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Operation details for assigning or unassigning a node.
    /// </summary>
    public interface IFrameAssignmentOperation : IWriteableAssignmentOperation, IFrameOperation
    {
        /// <summary>
        /// Inner where the assignment is taking place.
        /// </summary>
        new IFrameOptionalInner<IFrameBrowsingOptionalNodeIndex> Inner { get; }

        /// <summary>
        /// Position of the assigned or unassigned node.
        /// </summary>
        new IFrameBrowsingOptionalNodeIndex NodeIndex { get; }

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
        /// <param name="inner">Inner where the assignment is taking place.</param>
        /// <param name="nodeIndex">Position of the assigned or unassigned node.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameAssignmentOperation(IFrameOptionalInner<IFrameBrowsingOptionalNodeIndex> inner, IFrameBrowsingOptionalNodeIndex nodeIndex, Action<IWriteableOperation> handlerRedo, bool isNested)
            : base(inner, nodeIndex, handlerRedo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the assignment is taking place.
        /// </summary>
        public new IFrameOptionalInner<IFrameBrowsingOptionalNodeIndex> Inner { get { return (IFrameOptionalInner<IFrameBrowsingOptionalNodeIndex>)base.Inner; } }

        /// <summary>
        /// Position of the assigned or unassigned node.
        /// </summary>
        public new IFrameBrowsingOptionalNodeIndex NodeIndex { get { return (IFrameBrowsingOptionalNodeIndex)base.NodeIndex; } }

        /// <summary>
        /// The modified state.
        /// </summary>
        public new IFrameOptionalNodeState State { get { return (IFrameOptionalNodeState)base.State; } }
        #endregion
    }
}
