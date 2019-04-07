namespace EaslyController.Layout
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports:
    /// * Operations to modify the tree.
    /// * Organizing nodes and their content in cells, assigning line and column numbers.
    /// </summary>
    public interface ILayoutController : IFocusController
    {
        /// <summary>
        /// Index of the root node.
        /// </summary>
        new ILayoutRootNodeIndex RootIndex { get; }

        /// <summary>
        /// State of the root node.
        /// </summary>
        new ILayoutPlaceholderNodeState RootState { get; }

        /// <summary>
        /// State table.
        /// </summary>
        new ILayoutIndexNodeStateReadOnlyDictionary StateTable { get; }

        /// <summary>
        /// List of operations that have been performed, and can be undone or redone.
        /// </summary>
        new ILayoutOperationGroupReadOnlyList OperationStack { get; }

        /// <summary>
        /// List of supported cycle managers.
        /// </summary>
        new ILayoutCycleManagerList CycleManagerList { get; }
    }

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports:
    /// * Operations to modify the tree.
    /// * Organizing nodes and their content in cells, assigning line and column numbers.
    /// * Keeping the focus in a cell.
    /// * Measuring and arranging cells on a canva.
    /// </summary>
    public class LayoutController : FocusController, ILayoutController
    {
        #region Init
        /// <summary>
        /// Creates and initializes a new instance of a <see cref="LayoutController"/> object.
        /// </summary>
        /// <param name="nodeIndex">Index of the root of the node tree.</param>
        public static ILayoutController Create(ILayoutRootNodeIndex nodeIndex)
        {
            LayoutController Controller = new LayoutController();
            Controller.SetRoot(nodeIndex);
            Controller.SetCycleManagerList();
            Controller.SetInitialized();

            return Controller;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutController"/> class.
        /// </summary>
        private protected LayoutController()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Index of the root node.
        /// </summary>
        public new ILayoutRootNodeIndex RootIndex { get { return (ILayoutRootNodeIndex)base.RootIndex; } }

        /// <summary>
        /// State of the root node.
        /// </summary>
        public new ILayoutPlaceholderNodeState RootState { get { return (ILayoutPlaceholderNodeState)base.RootState; } }

        /// <summary>
        /// State table.
        /// </summary>
        public new ILayoutIndexNodeStateReadOnlyDictionary StateTable { get { return (ILayoutIndexNodeStateReadOnlyDictionary)base.StateTable; } }

        /// <summary>
        /// List of operations that have been performed, and can be undone or redone.
        /// </summary>
        public new ILayoutOperationGroupReadOnlyList OperationStack { get { return (ILayoutOperationGroupReadOnlyList)base.OperationStack; } }

        /// <summary>
        /// List of supported cycle managers.
        /// </summary>
        public new ILayoutCycleManagerList CycleManagerList { get { return (ILayoutCycleManagerList)base.CycleManagerList; } }
        #endregion

        #region Implementation
        private protected override void CheckContextConsistency(IReadOnlyBrowseContext browseContext)
        {
            ((LayoutBrowseContext)browseContext).CheckConsistency();
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        private protected override IReadOnlyIndexNodeStateDictionary CreateStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutIndexNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxInnerDictionary{string} object.
        /// </summary>
        private protected override IReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutInnerDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        private protected override IReadOnlyIndexNodeStateDictionary CreateChildStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutIndexNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxxBrowseContext object.
        /// </summary>
        private protected override IReadOnlyBrowseContext CreateBrowseContext(IReadOnlyBrowseContext parentBrowseContext, IReadOnlyNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutBrowseContext((ILayoutNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreatePlaceholderInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingPlaceholderNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutPlaceholderInner<ILayoutBrowsingPlaceholderNodeIndex, LayoutBrowsingPlaceholderNodeIndex>((ILayoutNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxOptionalInner{IxxxBrowsingOptionalNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex> CreateOptionalInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingOptionalNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutOptionalInner<ILayoutBrowsingOptionalNodeIndex, LayoutBrowsingOptionalNodeIndex>((ILayoutNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxListInner{IxxxBrowsingListNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyListInner<IReadOnlyBrowsingListNodeIndex> CreateListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingListNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutListInner<ILayoutBrowsingListNodeIndex, LayoutBrowsingListNodeIndex>((ILayoutNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxBlockListInner{IxxxBrowsingBlockNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex> CreateBlockListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingBlockNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutBlockListInner<ILayoutBrowsingBlockNodeIndex, LayoutBrowsingBlockNodeIndex>((ILayoutNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeState CreateRootNodeState(IReadOnlyRootNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutPlaceholderNodeState<ILayoutInner<ILayoutBrowsingChildIndex>>((ILayoutRootNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxWriteableInsertionOptionalNodeIndex object.
        /// </summary>
        private protected override IWriteableInsertionOptionalNodeIndex CreateNewOptionalNodeIndex(INode parentNode, string propertyName, INode node)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutInsertionOptionalNodeIndex(parentNode, propertyName, node);
        }

        /// <summary>
        /// Creates a IxxxInsertNodeOperation object.
        /// </summary>
        private protected override IWriteableInsertNodeOperation CreateInsertNodeOperation(INode parentNode, string propertyName, int blockIndex, int index, INode node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutInsertNodeOperation(parentNode, propertyName, blockIndex, index, node, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxInsertBlockOperation object.
        /// </summary>
        private protected override IWriteableInsertBlockOperation CreateInsertBlockOperation(INode parentNode, string propertyName, int blockIndex, IBlock block, INode node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutInsertBlockOperation(parentNode, propertyName, blockIndex, block, node, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveBlockOperation object.
        /// </summary>
        private protected override IWriteableRemoveBlockOperation CreateRemoveBlockOperation(INode parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutRemoveBlockOperation(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveBlockViewOperation object.
        /// </summary>
        private protected override IWriteableRemoveBlockViewOperation CreateRemoveBlockViewOperation(INode parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutRemoveBlockViewOperation(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveNodeOperation object.
        /// </summary>
        private protected override IWriteableRemoveNodeOperation CreateRemoveNodeOperation(INode parentNode, string propertyName, int blockIndex, int index, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutRemoveNodeOperation(parentNode, propertyName, blockIndex, index, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxReplaceOperation object.
        /// </summary>
        private protected override IWriteableReplaceOperation CreateReplaceOperation(INode parentNode, string propertyName, int blockIndex, int index, INode newNode, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutReplaceOperation(parentNode, propertyName, blockIndex, index, newNode, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxAssignmentOperation object.
        /// </summary>
        private protected override IWriteableAssignmentOperation CreateAssignmentOperation(INode parentNode, string propertyName, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutAssignmentOperation(parentNode, propertyName, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeDiscreteValueOperation object.
        /// </summary>
        private protected override IWriteableChangeDiscreteValueOperation CreateChangeDiscreteValueOperation(INode parentNode, string propertyName, int value, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutChangeDiscreteValueOperation(parentNode, propertyName, value, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeTextOperation object.
        /// </summary>
        private protected override IFocusChangeTextOperation CreateChangeTextOperation(INode parentNode, string propertyName, string text, int oldCaretPosition, int newCaretPosition, bool changeCaretBeforeText, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutChangeTextOperation(parentNode, propertyName, text, oldCaretPosition, newCaretPosition, changeCaretBeforeText, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeCommentOperation object.
        /// </summary>
        private protected override IFocusChangeCommentOperation CreateChangeCommentOperation(INode parentNode, string text, int oldCaretPosition, int newCaretPosition, bool changeCaretBeforeText, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutChangeCommentOperation(parentNode, text, oldCaretPosition, newCaretPosition, changeCaretBeforeText, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeBlockOperation object.
        /// </summary>
        private protected override IWriteableChangeBlockOperation CreateChangeBlockOperation(INode parentNode, string propertyName, int blockIndex, ReplicationStatus replication, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutChangeBlockOperation(parentNode, propertyName, blockIndex, replication, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxSplitBlockOperation object.
        /// </summary>
        private protected override IWriteableSplitBlockOperation CreateSplitBlockOperation(INode parentNode, string propertyName, int blockIndex, int index, IBlock newBlock, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutSplitBlockOperation(parentNode, propertyName, blockIndex, index, newBlock, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMergeBlocksOperation object.
        /// </summary>
        private protected override IWriteableMergeBlocksOperation CreateMergeBlocksOperation(INode parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutMergeBlocksOperation(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMoveNodeOperation object.
        /// </summary>
        private protected override IWriteableMoveNodeOperation CreateMoveNodeOperation(INode parentNode, string propertyName, int blockIndex, int index, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutMoveNodeOperation(parentNode, propertyName, blockIndex, index, direction, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMoveBlockOperation object.
        /// </summary>
        private protected override IWriteableMoveBlockOperation CreateMoveBlockOperation(INode parentNode, string propertyName, int blockIndex, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutMoveBlockOperation(parentNode, propertyName, blockIndex, direction, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxExpandArgumentOperation object.
        /// </summary>
        private protected override IWriteableExpandArgumentOperation CreateExpandArgumentOperation(INode parentNode, string propertyName, IBlock block, INode node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutExpandArgumentOperation(parentNode, propertyName, block, node, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxGenericRefreshOperation object.
        /// </summary>
        private protected override IWriteableGenericRefreshOperation CreateGenericRefreshOperation(IWriteableNodeState refreshState, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutGenericRefreshOperation((ILayoutNodeState)refreshState, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxOperationGroupList object.
        /// </summary>
        private protected override IWriteableOperationGroupList CreateOperationGroupStack()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutOperationGroupList();
        }

        /// <summary>
        /// Creates a IxxxOperationList object.
        /// </summary>
        private protected override IWriteableOperationList CreateOperationList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutOperationList();
        }

        /// <summary>
        /// Creates a IxxxOperationGroup object.
        /// </summary>
        private protected override IWriteableOperationGroup CreateOperationGroup(IWriteableOperationReadOnlyList operationList, IWriteableGenericRefreshOperation refresh)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutOperationGroup((ILayoutOperationReadOnlyList)operationList, (ILayoutGenericRefreshOperation)refresh);
        }

        /// <summary>
        /// Creates a IxxxReplaceWithCycleOperation object.
        /// </summary>
        private protected override IFocusReplaceWithCycleOperation CreateReplaceWithCycleOperation(INode parentNode, string propertyName, int blockIndex, int index, IFocusInsertionChildNodeIndexList cycleIndexList, int cyclePosition, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutReplaceWithCycleOperation(parentNode, propertyName, blockIndex, index, (ILayoutInsertionChildNodeIndexList)cycleIndexList, cyclePosition, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxCycleManagerList object.
        /// </summary>
        private protected override IFocusCycleManagerList CreateCycleManagerList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutCycleManagerList();
        }

        /// <summary>
        /// Creates a IxxxCycleManagerBody object.
        /// </summary>
        private protected override IFocusCycleManagerBody CreateCycleManagerBody()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutCycleManagerBody();
        }

        /// <summary>
        /// Creates a IxxxCycleManagerFeature object.
        /// </summary>
        private protected override IFocusCycleManagerFeature CreateCycleManagerFeature()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutController));
            return new LayoutCycleManagerFeature();
        }
        #endregion
    }
}
