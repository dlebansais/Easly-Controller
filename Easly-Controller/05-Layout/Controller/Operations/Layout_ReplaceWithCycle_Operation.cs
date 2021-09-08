namespace EaslyController.Layout
{
    using System;
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <summary>
    /// Operation details for replacing a node with another from a cycle.
    /// </summary>
    public class LayoutReplaceWithCycleOperation : FocusReplaceWithCycleOperation, ILayoutReplaceOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutReplaceWithCycleOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the replacement is taking place.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where the node is replaced.</param>
        /// <param name="blockIndex">Block position where the node is replaced, if applicable.</param>
        /// <param name="index">Position where the node is replaced, if applicable.</param>
        /// <param name="cycleIndexList">Cycle of nodes that can replace the current node.</param>
        /// <param name="cyclePosition">New position in the cycle.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public LayoutReplaceWithCycleOperation(Node parentNode, string propertyName, int blockIndex, int index, LayoutInsertionChildNodeIndexList cycleIndexList, int cyclePosition, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, index, cycleIndexList, cyclePosition, handlerRedo, handlerUndo, isNested)
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

        /// <summary>
        /// Cycle of nodes that can replace the current node.
        /// </summary>
        public new LayoutInsertionChildNodeIndexList CycleIndexList { get { return (LayoutInsertionChildNodeIndexList)base.CycleIndexList; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxReplaceOperation object.
        /// </summary>
        private protected override FocusReplaceWithCycleOperation CreateReplaceWithCycleOperation(int blockIndex, int index, FocusInsertionChildNodeIndexList cycleIndexList, int cyclePosition, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutReplaceWithCycleOperation));
            return new LayoutReplaceWithCycleOperation(ParentNode, PropertyName, blockIndex, index, (LayoutInsertionChildNodeIndexList)cycleIndexList, cyclePosition, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
