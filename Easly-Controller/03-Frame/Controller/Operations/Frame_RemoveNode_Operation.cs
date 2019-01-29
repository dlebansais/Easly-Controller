using BaseNode;
using EaslyController.Writeable;
using System;

namespace EaslyController.Frame
{
    /// <summary>
    /// Operation details for removing a node in a list or block list.
    /// </summary>
    public interface IFrameRemoveNodeOperation : IWriteableRemoveNodeOperation, IFrameRemoveOperation
    {
        /// <summary>
        /// State removed.
        /// </summary>
        new IFramePlaceholderNodeState RemovedState { get; }
    }

    /// <summary>
    /// Operation details for removing a node in a list or block list.
    /// </summary>
    public class FrameRemoveNodeOperation : WriteableRemoveNodeOperation, IFrameRemoveNodeOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FrameRemoveNodeOperation"/>.
        /// </summary>
        /// <param name="parentNode">Node where the removal is taking place.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where a node is removed.</param>
        /// <param name="blockIndex">Block position where the node is removed, if applicable.</param>
        /// <param name="index">Position of the removed node.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameRemoveNodeOperation(INode parentNode, string propertyName, int blockIndex, int index, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, index, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// State removed.
        /// </summary>
        public new IFramePlaceholderNodeState RemovedState { get { return (IFramePlaceholderNodeState)base.RemovedState; } }
        #endregion
    }
}
