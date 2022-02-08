namespace EaslyController.Focus
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Writeable;

    /// <summary>
    /// Operation details for replacing a node with another from a cycle.
    /// </summary>
    public interface IFocusReplaceWithCycleOperation : IFocusReplaceOperation
    {
        /// <summary>
        /// Cycle of nodes that can replace the current node.
        /// </summary>
        FocusInsertionChildNodeIndexList CycleIndexList { get; }

        /// <summary>
        /// New position in the cycle.
        /// </summary>
        int NewCyclePosition { get; }

        /// <summary>
        /// Old position in the cycle.
        /// </summary>
        int OldCyclePosition { get; }
    }

    /// <summary>
    /// Operation details for replacing a node with another from a cycle.
    /// </summary>
    public class FocusReplaceWithCycleOperation : FocusReplaceOperation, IFocusReplaceWithCycleOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusReplaceWithCycleOperation"/> class.
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
        public FocusReplaceWithCycleOperation(Node parentNode, string propertyName, int blockIndex, int index, FocusInsertionChildNodeIndexList cycleIndexList, int cyclePosition, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, index, clearNode: false, cycleIndexList[cyclePosition].Node, handlerRedo, handlerUndo, isNested)
        {
            CycleIndexList = cycleIndexList;
            NewCyclePosition = cyclePosition;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Cycle of nodes that can replace the current node.
        /// </summary>
        public FocusInsertionChildNodeIndexList CycleIndexList { get; }

        /// <summary>
        /// New position in the cycle.
        /// </summary>
        public int NewCyclePosition { get; }

        /// <summary>
        /// Old position in the cycle.
        /// </summary>
        public int OldCyclePosition { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="oldBrowsingIndex">Index of the state before it's replaced.</param>
        /// <param name="newBrowsingIndex">Index of the state after it's replaced.</param>
        /// <param name="oldNode">The old node. Can be null if optional and replaced.</param>
        /// <param name="newChildState">The new state.</param>
        public override void Update(IWriteableBrowsingChildIndex oldBrowsingIndex, IWriteableBrowsingChildIndex newBrowsingIndex, Node oldNode, IWriteableNodeState newChildState)
        {
            base.Update(oldBrowsingIndex, newBrowsingIndex, oldNode, newChildState);

            int i;
            for (i = 0; i < CycleIndexList.Count; i++)
            {
                IFocusInsertionChildNodeIndex NodeIndex = CycleIndexList[i];
                if (NodeIndex.Node == oldNode)
                {
                    OldCyclePosition = i;
                    break;
                }
            }

            Debug.Assert(OldCyclePosition < CycleIndexList.Count);
            Debug.Assert(NewCyclePosition < CycleIndexList.Count);
        }

        /// <summary>
        /// Creates an operation to undo the replace operation.
        /// </summary>
        public override IWriteableReplaceOperation ToInverseReplace()
        {
            return CreateReplaceWithCycleOperation(BlockIndex, Index, CycleIndexList, OldCyclePosition, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxReplaceOperation object.
        /// </summary>
        private protected virtual IFocusReplaceWithCycleOperation CreateReplaceWithCycleOperation(int blockIndex, int index, FocusInsertionChildNodeIndexList cycleIndexList, int cyclePosition, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusReplaceWithCycleOperation));
            return new FocusReplaceWithCycleOperation(ParentNode, PropertyName, blockIndex, index, cycleIndexList, cyclePosition, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
