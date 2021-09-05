namespace EaslyController.Layout
{
    using System;
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <summary>
    /// Operation details for inserting a block with a single node in a block list.
    /// </summary>
    public interface ILayoutInsertBlockOperation : IFocusInsertBlockOperation, ILayoutInsertOperation
    {
        /// <summary>
        /// Index of the state after it's inserted.
        /// </summary>
        new ILayoutBrowsingExistingBlockNodeIndex BrowsingIndex { get; }

        /// <summary>
        /// Block state inserted.
        /// </summary>
        new ILayoutBlockState BlockState { get; }

        /// <summary>
        /// State inserted.
        /// </summary>
        new ILayoutPlaceholderNodeState ChildState { get; }
    }

    /// <summary>
    /// Operation details for inserting a block with a single node in a block list.
    /// </summary>
    internal class LayoutInsertBlockOperation : FocusInsertBlockOperation, ILayoutInsertBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutInsertBlockOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the block insertion is taking place.</param>
        /// <param name="propertyName">Block list property of <paramref name="parentNode"/> where a block is inserted.</param>
        /// <param name="blockIndex">Index of the inserted block.</param>
        /// <param name="block">The inserted block.</param>
        /// <param name="node">The inserted node.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public LayoutInsertBlockOperation(Node parentNode, string propertyName, int blockIndex, IBlock block, Node node, Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, block, node, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Index of the state after it's inserted.
        /// </summary>
        public new ILayoutBrowsingExistingBlockNodeIndex BrowsingIndex { get { return (ILayoutBrowsingExistingBlockNodeIndex)base.BrowsingIndex; } }

        /// <summary>
        /// Block state inserted.
        /// </summary>
        public new ILayoutBlockState BlockState { get { return (ILayoutBlockState)base.BlockState; } }

        /// <summary>
        /// State inserted.
        /// </summary>
        public new ILayoutPlaceholderNodeState ChildState { get { return (ILayoutPlaceholderNodeState)base.ChildState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxRemoveBlockOperation object.
        /// </summary>
        private protected override WriteableRemoveBlockOperation CreateRemoveBlockOperation(int blockIndex, Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutInsertBlockOperation));
            return new LayoutRemoveBlockOperation(ParentNode, PropertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
