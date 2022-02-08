namespace EaslyController.Frame
{
    using System;
    using BaseNode;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public interface IFrameReplaceOperation : IWriteableReplaceOperation, IFrameOperation
    {
        /// <summary>
        /// Index of the state before it's replaced.
        /// </summary>
        new IFrameBrowsingChildIndex OldBrowsingIndex { get; }

        /// <summary>
        /// Index of the state after it's replaced.
        /// </summary>
        new IFrameBrowsingChildIndex NewBrowsingIndex { get; }

        /// <summary>
        /// The new state.
        /// </summary>
        new IFrameNodeState NewChildState { get; }
    }

    /// <inheritdoc/>
    public class FrameReplaceOperation : WriteableReplaceOperation, IFrameReplaceOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameReplaceOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the replacement is taking place.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where the node is replaced.</param>
        /// <param name="blockIndex">Block position where the node is replaced, if applicable.</param>
        /// <param name="index">Position where the node is replaced, if applicable.</param>
        /// <param name="clearNode">A value indicating whether the node is cleared and not replaced</param>
        /// <param name="newNode">The new node. Null to clear an optional node.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameReplaceOperation(Node parentNode, string propertyName, int blockIndex, int index, bool clearNode, Node newNode, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, index, clearNode, newNode, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Index of the state before it's replaced.
        /// </summary>
        public new IFrameBrowsingChildIndex OldBrowsingIndex { get { return (IFrameBrowsingChildIndex)base.OldBrowsingIndex; } }

        /// <summary>
        /// Index of the state after it's replaced.
        /// </summary>
        public new IFrameBrowsingChildIndex NewBrowsingIndex { get { return (IFrameBrowsingChildIndex)base.NewBrowsingIndex; } }

        /// <summary>
        /// The new state.
        /// </summary>
        public new IFrameNodeState NewChildState { get { return (IFrameNodeState)base.NewChildState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxReplaceOperation object.
        /// </summary>
        private protected override IWriteableReplaceOperation CreateReplaceOperation(int blockIndex, int index, bool clearNode, Node node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameReplaceOperation));
            return new FrameReplaceOperation(ParentNode, PropertyName, blockIndex, index, clearNode, node, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
