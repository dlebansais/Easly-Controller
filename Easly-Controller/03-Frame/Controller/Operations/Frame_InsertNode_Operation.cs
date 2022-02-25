namespace EaslyController.Frame
{
    using BaseNode;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <inheritdoc/>
    internal class FrameInsertNodeOperation : WriteableInsertNodeOperation, IFrameInsertOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameInsertNodeOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the insertion is taking place.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where a node is inserted.</param>
        /// <param name="blockIndex">Block position where the node is inserted, if applicable.</param>
        /// <param name="index">Position where the node is inserted.</param>
        /// <param name="node">The inserted node.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameInsertNodeOperation(Node parentNode, string propertyName, int blockIndex, int index, Node node, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, index, node, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Index of the state after it's inserted.
        /// </summary>
        public new IFrameBrowsingCollectionNodeIndex BrowsingIndex { get { return (IFrameBrowsingCollectionNodeIndex)base.BrowsingIndex; } }

        /// <summary>
        /// State inserted.
        /// </summary>
        public new IFramePlaceholderNodeState ChildState { get { return (IFramePlaceholderNodeState)base.ChildState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxRemoveNodeOperation object.
        /// </summary>
        private protected override WriteableRemoveNodeOperation CreateRemoveNodeOperation(int blockIndex, int index, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameInsertNodeOperation>());
            return new FrameRemoveNodeOperation(ParentNode, PropertyName, blockIndex, index, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
