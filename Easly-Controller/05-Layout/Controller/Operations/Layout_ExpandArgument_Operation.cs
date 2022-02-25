namespace EaslyController.Layout
{
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <summary>
    /// Operation details for inserting a single argument in a block list.
    /// </summary>
    internal class LayoutExpandArgumentOperation : FocusExpandArgumentOperation, ILayoutInsertBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutExpandArgumentOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the block insertion is taking place.</param>
        /// <param name="propertyName">Block list property of <paramref name="parentNode"/> where a block is inserted.</param>
        /// <param name="block">The inserted block.</param>
        /// <param name="node">The inserted item.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public LayoutExpandArgumentOperation(Node parentNode, string propertyName, IBlock block, Node node, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, block, node, handlerRedo, handlerUndo, isNested)
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
        private protected override IWriteableRemoveBlockOperation CreateRemoveBlockOperation(int blockIndex, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutExpandArgumentOperation>());
            return new LayoutRemoveBlockOperation(ParentNode, PropertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
