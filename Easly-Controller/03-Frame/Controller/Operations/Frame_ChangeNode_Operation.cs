using EaslyController.Writeable;
using System;

namespace EaslyController.Frame
{
    /// <summary>
    /// Operation details for changing a node.
    /// </summary>
    public interface IFrameChangeNodeOperation : IWriteableChangeNodeOperation, IFrameOperation
    {
        /// <summary>
        /// Index of the changed node.
        /// </summary>
        new IFrameIndex NodeIndex { get; }

        /// <summary>
        /// State changed.
        /// </summary>
        new IFramePlaceholderNodeState State { get; }
    }

    /// <summary>
    /// Operation details for moving a node in a list or block list.
    /// </summary>
    public class FrameChangeNodeOperation : WriteableChangeNodeOperation, IFrameChangeNodeOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FrameChangeNodeOperation"/>.
        /// </summary>
        /// <param name="nodeIndex">Index of the changed node.</param>
        /// <param name="propertyName">Name of the property to change.</param>
        /// <param name="value">The new value.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameChangeNodeOperation(IFrameIndex nodeIndex, string propertyName, int value, Action<IWriteableOperation> handlerRedo, bool isNested)
            : base(nodeIndex, propertyName, value, handlerRedo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Index of the changed node.
        /// </summary>
        public new IFrameIndex NodeIndex { get { return (IFrameIndex)base.NodeIndex; } }

        /// <summary>
        /// State changed.
        /// </summary>
        public new IFramePlaceholderNodeState State { get { return (IFramePlaceholderNodeState)base.State; } }
        #endregion
    }
}
