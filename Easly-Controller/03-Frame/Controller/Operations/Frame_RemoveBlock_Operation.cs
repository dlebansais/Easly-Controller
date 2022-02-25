namespace EaslyController.Frame
{
    using BaseNode;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <summary>
    /// Operation details for removing a block from a block list.
    /// </summary>
    public interface IFrameRemoveBlockOperation : IWriteableRemoveBlockOperation, IFrameRemoveOperation
    {
        /// <summary>
        /// The removed block state.
        /// </summary>
        new IFrameBlockState BlockState { get; }

        /// <summary>
        /// The removed state.
        /// </summary>
        new IFrameNodeState RemovedState { get; }
    }

    /// <inheritdoc/>
    internal class FrameRemoveBlockOperation : WriteableRemoveBlockOperation, IFrameRemoveBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameRemoveBlockOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the block removal is taking place.</param>
        /// <param name="propertyName">Block list property of <paramref name="parentNode"/> where a block is removed.</param>
        /// <param name="blockIndex">index of the removed block.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameRemoveBlockOperation(Node parentNode, string propertyName, int blockIndex, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The removed block state.
        /// </summary>
        public new IFrameBlockState BlockState { get { return (IFrameBlockState)base.BlockState; } }

        /// <summary>
        /// The removed state.
        /// </summary>
        public new IFrameNodeState RemovedState { get { return (IFrameNodeState)base.RemovedState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertBlockOperation object.
        /// </summary>
        private protected override IWriteableInsertBlockOperation CreateInsertBlockOperation(int blockIndex, IBlock block, Node node, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameRemoveBlockOperation>());
            return new FrameInsertBlockOperation(ParentNode, PropertyName, blockIndex, block, node, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
