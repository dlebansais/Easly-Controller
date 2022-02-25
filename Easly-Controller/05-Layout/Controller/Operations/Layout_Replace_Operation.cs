namespace EaslyController.Layout
{
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <summary>
    /// Operation details for replacing a node.
    /// </summary>
    public interface ILayoutReplaceOperation : IFocusReplaceOperation, ILayoutOperation
    {
        /// <summary>
        /// Index of the state before it's replaced.
        /// </summary>
        new ILayoutBrowsingChildIndex OldBrowsingIndex { get; }

        /// <summary>
        /// Index of the state after it's replaced.
        /// </summary>
        new ILayoutBrowsingChildIndex NewBrowsingIndex { get; }

        /// <summary>
        /// The new state.
        /// </summary>
        new ILayoutNodeState NewChildState { get; }
    }

    /// <summary>
    /// Operation details for replacing a node.
    /// </summary>
    internal class LayoutReplaceOperation : FocusReplaceOperation, ILayoutReplaceOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutReplaceOperation"/> class.
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
        public LayoutReplaceOperation(Node parentNode, string propertyName, int blockIndex, int index, bool clearNode, Node newNode, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, index, clearNode, newNode, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Index of the state before it's replaced.
        /// </summary>
        public new ILayoutBrowsingChildIndex OldBrowsingIndex { get { return (ILayoutBrowsingChildIndex)base.OldBrowsingIndex; } }

        /// <summary>
        /// Index of the state after it's replaced.
        /// </summary>
        public new ILayoutBrowsingChildIndex NewBrowsingIndex { get { return (ILayoutBrowsingChildIndex)base.NewBrowsingIndex; } }

        /// <summary>
        /// The new state.
        /// </summary>
        public new ILayoutNodeState NewChildState { get { return (ILayoutNodeState)base.NewChildState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxReplaceOperation object.
        /// </summary>
        private protected override IWriteableReplaceOperation CreateReplaceOperation(int blockIndex, int index, bool clearNode, Node node, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutReplaceOperation>());
            return new LayoutReplaceOperation(ParentNode, PropertyName, blockIndex, index, clearNode, node, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
