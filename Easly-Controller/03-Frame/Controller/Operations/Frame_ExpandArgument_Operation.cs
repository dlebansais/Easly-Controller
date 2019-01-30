namespace EaslyController.Frame
{
    using System;
    using BaseNode;
    using EaslyController.Writeable;

    /// <summary>
    /// Operation details for inserting a single argument in a block list.
    /// </summary>
    public interface IFrameExpandArgumentOperation : IWriteableExpandArgumentOperation, IFrameInsertBlockOperation
    {
        /// <summary>
        /// Index of the state after it's inserted.
        /// </summary>
        new IFrameBrowsingExistingBlockNodeIndex BrowsingIndex { get; }

        /// <summary>
        /// Block state inserted.
        /// </summary>
        new IFrameBlockState BlockState { get; }

        /// <summary>
        /// State inserted.
        /// </summary>
        new IFramePlaceholderNodeState ChildState { get; }
    }

    /// <summary>
    /// Operation details for inserting a single argument in a block list.
    /// </summary>
    public class FrameExpandArgumentOperation : WriteableExpandArgumentOperation, IFrameExpandArgumentOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameExpandArgumentOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the block insertion is taking place.</param>
        /// <param name="propertyName">Block list property of <paramref name="parentNode"/> where a block is inserted.</param>
        /// <param name="block">The inserted block.</param>
        /// <param name="argument">The inserted argument.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameExpandArgumentOperation(INode parentNode, string propertyName, IBlock block, IArgument argument, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, block, argument, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Index of the state after it's inserted.
        /// </summary>
        public new IFrameBrowsingExistingBlockNodeIndex BrowsingIndex { get { return (IFrameBrowsingExistingBlockNodeIndex)base.BrowsingIndex; } }

        /// <summary>
        /// Block state inserted.
        /// </summary>
        public new IFrameBlockState BlockState { get { return (IFrameBlockState)base.BlockState; } }

        /// <summary>
        /// State inserted.
        /// </summary>
        public new IFramePlaceholderNodeState ChildState { get { return (IFramePlaceholderNodeState)base.ChildState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxRemoveBlockOperation object.
        /// </summary>
        private protected override IWriteableRemoveBlockOperation CreateRemoveBlockOperation(int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameExpandArgumentOperation));
            return new FrameRemoveBlockOperation(ParentNode, PropertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
