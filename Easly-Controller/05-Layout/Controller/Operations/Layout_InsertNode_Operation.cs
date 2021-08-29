namespace EaslyController.Layout
{
    using System;
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <summary>
    /// Operation details for inserting a node in a list or block list.
    /// </summary>
    public interface ILayoutInsertNodeOperation : IFocusInsertNodeOperation, ILayoutInsertOperation
    {
        /// <summary>
        /// Index of the state after it's inserted.
        /// </summary>
        new ILayoutBrowsingCollectionNodeIndex BrowsingIndex { get; }

        /// <summary>
        /// State inserted.
        /// </summary>
        new ILayoutPlaceholderNodeState ChildState { get; }
    }

    /// <summary>
    /// Operation details for inserting a node in a list or block list.
    /// </summary>
    internal class LayoutInsertNodeOperation : FocusInsertNodeOperation, ILayoutInsertNodeOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutInsertNodeOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the insertion is taking place.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where a node is inserted.</param>
        /// <param name="blockIndex">Block position where the node is inserted, if applicable.</param>
        /// <param name="index">Position where the node is inserted.</param>
        /// <param name="node">The inserted node.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public LayoutInsertNodeOperation(Node parentNode, string propertyName, int blockIndex, int index, Node node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, index, node, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Index of the state after it's inserted.
        /// </summary>
        public new ILayoutBrowsingCollectionNodeIndex BrowsingIndex { get { return (ILayoutBrowsingCollectionNodeIndex)base.BrowsingIndex; } }

        /// <summary>
        /// State inserted.
        /// </summary>
        public new ILayoutPlaceholderNodeState ChildState { get { return (ILayoutPlaceholderNodeState)base.ChildState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxRemoveNodeOperation object.
        /// </summary>
        private protected override IWriteableRemoveNodeOperation CreateRemoveNodeOperation(int blockIndex, int index, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutInsertNodeOperation));
            return new LayoutRemoveNodeOperation(ParentNode, PropertyName, blockIndex, index, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
