using BaseNode;
using EaslyController.Frame;
using EaslyController.Writeable;
using System;

namespace EaslyController.Focus
{
    /// <summary>
    /// Operation details for removing a node in a list or block list.
    /// </summary>
    public interface IFocusRemoveNodeOperation : IFrameRemoveNodeOperation, IFocusRemoveOperation
    {
        /// <summary>
        /// State removed.
        /// </summary>
        new IFocusPlaceholderNodeState RemovedState { get; }
    }

    /// <summary>
    /// Operation details for removing a node in a list or block list.
    /// </summary>
    public class FocusRemoveNodeOperation : FrameRemoveNodeOperation, IFocusRemoveNodeOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FocusRemoveNodeOperation"/>.
        /// </summary>
        /// <param name="parentNode">Node where the removal is taking place.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where a node is removed.</param>
        /// <param name="blockIndex">Block position where the node is removed, if applicable.</param>
        /// <param name="index">Position of the removed node.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusRemoveNodeOperation(INode parentNode, string propertyName, int blockIndex, int index, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, index, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// State removed.
        /// </summary>
        public new IFocusPlaceholderNodeState RemovedState { get { return (IFocusPlaceholderNodeState)base.RemovedState; } }
        #endregion
    }
}
