namespace EaslyController.Layout
{
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <summary>
    /// Operation details for removing a node in a list or block list.
    /// </summary>
    internal class LayoutRemoveNodeOperation : FocusRemoveNodeOperation, ILayoutRemoveOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutRemoveNodeOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the removal is taking place.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where a node is removed.</param>
        /// <param name="blockIndex">Block position where the node is removed, if applicable.</param>
        /// <param name="index">Position of the removed node.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public LayoutRemoveNodeOperation(Node parentNode, string propertyName, int blockIndex, int index, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, index, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// State removed.
        /// </summary>
        public new ILayoutPlaceholderNodeState RemovedState { get { return (ILayoutPlaceholderNodeState)base.RemovedState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertNodeOperation object.
        /// </summary>
        private protected override WriteableInsertNodeOperation CreateInsertNodeOperation(int blockIndex, int index, Node node, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutRemoveNodeOperation>());
            return new LayoutInsertNodeOperation(ParentNode, PropertyName, blockIndex, index, node, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
