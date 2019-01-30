namespace EaslyController.Frame
{
    using System;
    using BaseNode;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports:
    /// * Operations to modify the tree.
    /// * Organizing nodes and their content in cells, assigning line and column numbers.
    /// </summary>
    public interface IFrameController : IWriteableController
    {
        /// <summary>
        /// Index of the root node.
        /// </summary>
        new IFrameRootNodeIndex RootIndex { get; }

        /// <summary>
        /// State of the root node.
        /// </summary>
        new IFramePlaceholderNodeState RootState { get; }

        /// <summary>
        /// List of operations that have been performed, and can be undone or redone.
        /// </summary>
        new IFrameOperationReadOnlyList OperationStack { get; }

        /// <summary>
        /// Called when a state is created.
        /// </summary>
        new event Action<IFrameNodeState> NodeStateCreated;

        /// <summary>
        /// Called when a state is initialized.
        /// </summary>
        new event Action<IFrameNodeState> NodeStateInitialized;

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        new event Action<IFrameNodeState> NodeStateRemoved;

        /// <summary>
        /// Called when a block list inner is created.
        /// </summary>
        new event Action<IFrameBlockListInner> BlockListInnerCreated;

        /// <summary>
        /// Called when a block state is inserted.
        /// </summary>
        new event Action<IFrameInsertBlockOperation> BlockStateInserted;

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        new event Action<IFrameRemoveBlockOperation> BlockStateRemoved;

        /// <summary>
        /// Called when a block view must be removed.
        /// </summary>
        new event Action<IFrameRemoveBlockViewOperation> BlockViewRemoved;

        /// <summary>
        /// Called when a state is inserted.
        /// </summary>
        new event Action<IFrameInsertNodeOperation> StateInserted;

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        new event Action<IFrameRemoveNodeOperation> StateRemoved;

        /// <summary>
        /// Called when a state is replaced.
        /// </summary>
        new event Action<IFrameReplaceOperation> StateReplaced;

        /// <summary>
        /// Called when a state is assigned.
        /// </summary>
        new event Action<IFrameAssignmentOperation> StateAssigned;

        /// <summary>
        /// Called when a state is unassigned.
        /// </summary>
        new event Action<IFrameAssignmentOperation> StateUnassigned;

        /// <summary>
        /// Called when a state is moved.
        /// </summary>
        new event Action<IFrameChangeNodeOperation> StateChanged;

        /// <summary>
        /// Called when a state is moved.
        /// </summary>
        new event Action<IFrameMoveNodeOperation> StateMoved;

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        new event Action<IFrameMoveBlockOperation> BlockStateMoved;

        /// <summary>
        /// Called when a block is split.
        /// </summary>
        new event Action<IFrameSplitBlockOperation> BlockSplit;

        /// <summary>
        /// Called when two blocks are merged.
        /// </summary>
        new event Action<IFrameMergeBlocksOperation> BlocksMerged;
    }

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports:
    /// * Operations to modify the tree.
    /// * Organizing nodes and their content in cells, assigning line and column numbers.
    /// </summary>
    public class FrameController : WriteableController, IFrameController
    {
        #region Init
        /// <summary>
        /// Creates and initializes a new instance of a <see cref="FrameController"/> object.
        /// </summary>
        /// <param name="nodeIndex">Index of the root of the node tree.</param>
        public static IFrameController Create(IFrameRootNodeIndex nodeIndex)
        {
            FrameController Controller = new FrameController();
            Controller.SetRoot(nodeIndex);
            Controller.SetInitialized();
            return Controller;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameController"/> class.
        /// </summary>
        private protected FrameController()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Index of the root node.
        /// </summary>
        public new IFrameRootNodeIndex RootIndex { get { return (IFrameRootNodeIndex)base.RootIndex; } }

        /// <summary>
        /// State of the root node.
        /// </summary>
        public new IFramePlaceholderNodeState RootState { get { return (IFramePlaceholderNodeState)base.RootState; } }

        /// <summary>
        /// List of operations that have been performed, and can be undone or redone.
        /// </summary>
        public new IFrameOperationReadOnlyList OperationStack { get { return (IFrameOperationReadOnlyList)base.OperationStack; } }

        /// <summary>
        /// Called when a state is created.
        /// </summary>
        public new event Action<IFrameNodeState> NodeStateCreated
        {
            add { AddNodeStateCreatedDelegate((Action<IReadOnlyNodeState>)value); }
            remove { RemoveNodeStateCreatedDelegate((Action<IReadOnlyNodeState>)value); }
        }

        /// <summary>
        /// Called when a state is initialized.
        /// </summary>
        public new event Action<IFrameNodeState> NodeStateInitialized
        {
            add { AddNodeStateInitializedDelegate((Action<IReadOnlyNodeState>)value); }
            remove { RemoveNodeStateInitializedDelegate((Action<IReadOnlyNodeState>)value); }
        }

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        public new event Action<IFrameNodeState> NodeStateRemoved
        {
            add { AddNodeStateRemovedDelegate((Action<IReadOnlyNodeState>)value); }
            remove { RemoveNodeStateRemovedDelegate((Action<IReadOnlyNodeState>)value); }
        }

        /// <summary>
        /// Called when a block list inner is created.
        /// </summary>
        public new event Action<IFrameBlockListInner> BlockListInnerCreated
        {
            add { AddBlockListInnerCreatedDelegate((Action<IReadOnlyBlockListInner>)value); }
            remove { RemoveBlockListInnerCreatedDelegate((Action<IReadOnlyBlockListInner>)value); }
        }

        /// <summary>
        /// Called when a block state is inserted.
        /// </summary>
        public new event Action<IFrameInsertBlockOperation> BlockStateInserted
        {
            add { AddBlockStateInsertedDelegate((Action<IWriteableInsertBlockOperation>)value); }
            remove { RemoveBlockStateInsertedDelegate((Action<IWriteableInsertBlockOperation>)value); }
        }

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        public new event Action<IFrameRemoveBlockOperation> BlockStateRemoved
        {
            add { AddBlockStateRemovedDelegate((Action<IWriteableRemoveBlockOperation>)value); }
            remove { RemoveBlockStateRemovedDelegate((Action<IWriteableRemoveBlockOperation>)value); }
        }

        /// <summary>
        /// Called when a block view must be removed.
        /// </summary>
        public new event Action<IFrameRemoveBlockViewOperation> BlockViewRemoved
        {
            add { AddBlockViewRemovedDelegate((Action<IWriteableRemoveBlockViewOperation>)value); }
            remove { RemoveBlockViewRemovedDelegate((Action<IWriteableRemoveBlockViewOperation>)value); }
        }

        /// <summary>
        /// Called when a state is inserted.
        /// </summary>
        public new event Action<IFrameInsertNodeOperation> StateInserted
        {
            add { AddStateInsertedDelegate((Action<IWriteableInsertNodeOperation>)value); }
            remove { RemoveStateInsertedDelegate((Action<IWriteableInsertNodeOperation>)value); }
        }

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        public new event Action<IFrameRemoveNodeOperation> StateRemoved
        {
            add { AddStateRemovedDelegate((Action<IWriteableRemoveNodeOperation>)value); }
            remove { RemoveStateRemovedDelegate((Action<IWriteableRemoveNodeOperation>)value); }
        }

        /// <summary>
        /// Called when a state is replaced.
        /// </summary>
        public new event Action<IFrameReplaceOperation> StateReplaced
        {
            add { AddStateReplacedDelegate((Action<IWriteableReplaceOperation>)value); }
            remove { RemoveStateReplacedDelegate((Action<IWriteableReplaceOperation>)value); }
        }

        /// <summary>
        /// Called when a state is assigned.
        /// </summary>
        public new event Action<IFrameAssignmentOperation> StateAssigned
        {
            add { AddStateAssignedDelegate((Action<IWriteableAssignmentOperation>)value); }
            remove { RemoveStateAssignedDelegate((Action<IWriteableAssignmentOperation>)value); }
        }

        /// <summary>
        /// Called when a state is unassigned.
        /// </summary>
        public new event Action<IFrameAssignmentOperation> StateUnassigned
        {
            add { AddStateUnassignedDelegate((Action<IWriteableAssignmentOperation>)value); }
            remove { RemoveStateUnassignedDelegate((Action<IWriteableAssignmentOperation>)value); }
        }

        /// <summary>
        /// Called when a state is changed.
        /// </summary>
        public new event Action<IFrameChangeNodeOperation> StateChanged
        {
            add { AddStateChangedDelegate((Action<IWriteableChangeNodeOperation>)value); }
            remove { RemoveStateChangedDelegate((Action<IWriteableChangeNodeOperation>)value); }
        }

        /// <summary>
        /// Called when a state is moved.
        /// </summary>
        public new event Action<IFrameMoveNodeOperation> StateMoved
        {
            add { AddStateMovedDelegate((Action<IWriteableMoveNodeOperation>)value); }
            remove { RemoveStateMovedDelegate((Action<IWriteableMoveNodeOperation>)value); }
        }

        /// <summary>
        /// Called when a block state is moved.
        /// </summary>
        public new event Action<IFrameMoveBlockOperation> BlockStateMoved
        {
            add { AddBlockStateMovedDelegate((Action<IWriteableMoveBlockOperation>)value); }
            remove { RemoveBlockStateMovedDelegate((Action<IWriteableMoveBlockOperation>)value); }
        }

        /// <summary>
        /// Called when a block is split.
        /// </summary>
        public new event Action<IFrameSplitBlockOperation> BlockSplit
        {
            add { AddBlockSplitDelegate((Action<IWriteableSplitBlockOperation>)value); }
            remove { RemoveBlockSplitDelegate((Action<IWriteableSplitBlockOperation>)value); }
        }

        /// <summary>
        /// Called when two blocks are merged.
        /// </summary>
        public new event Action<IFrameMergeBlocksOperation> BlocksMerged
        {
            add { AddBlocksMergedDelegate((Action<IWriteableMergeBlocksOperation>)value); }
            remove { RemoveBlocksMergedDelegate((Action<IWriteableMergeBlocksOperation>)value); }
        }

        /// <summary>
        /// State table.
        /// </summary>
        private protected new IFrameIndexNodeStateReadOnlyDictionary StateTable { get { return (IFrameIndexNodeStateReadOnlyDictionary)base.StateTable; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        private protected override IReadOnlyIndexNodeStateDictionary CreateStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameIndexNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxIndexNodeStateReadOnlyDictionary object.
        /// </summary>
        private protected override IReadOnlyIndexNodeStateReadOnlyDictionary CreateStateTableReadOnly(IReadOnlyIndexNodeStateDictionary stateTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameIndexNodeStateReadOnlyDictionary((IFrameIndexNodeStateDictionary)stateTable);
        }

        /// <summary>
        /// Creates a IxxxInnerDictionary{string} object.
        /// </summary>
        private protected override IReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameInnerDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxInnerReadOnlyDictionary{string} object.
        /// </summary>
        private protected override IReadOnlyInnerReadOnlyDictionary<string> CreateInnerTableReadOnly(IReadOnlyInnerDictionary<string> innerTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameInnerReadOnlyDictionary<string>((IFrameInnerDictionary<string>)innerTable);
        }

        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        private protected override IReadOnlyIndexNodeStateDictionary CreateChildStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameIndexNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxxBrowseContext object.
        /// </summary>
        private protected override IReadOnlyBrowseContext CreateBrowseContext(IReadOnlyBrowseContext parentBrowseContext, IReadOnlyNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameBrowseContext((IFrameNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreatePlaceholderInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingPlaceholderNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FramePlaceholderInner<IFrameBrowsingPlaceholderNodeIndex, FrameBrowsingPlaceholderNodeIndex>((IFrameNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxOptionalInner{IxxxBrowsingOptionalNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex> CreateOptionalInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingOptionalNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameOptionalInner<IFrameBrowsingOptionalNodeIndex, FrameBrowsingOptionalNodeIndex>((IFrameNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxListInner{IxxxBrowsingListNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyListInner<IReadOnlyBrowsingListNodeIndex> CreateListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingListNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameListInner<IFrameBrowsingListNodeIndex, FrameBrowsingListNodeIndex>((IFrameNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxBlockListInner{IxxxBrowsingBlockNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex> CreateBlockListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingBlockNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameBlockListInner<IFrameBrowsingBlockNodeIndex, FrameBrowsingBlockNodeIndex>((IFrameNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeState CreateRootNodeState(IReadOnlyRootNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FramePlaceholderNodeState((IFrameRootNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxWriteableInsertionOptionalNodeIndex object.
        /// </summary>
        private protected override IWriteableInsertionOptionalNodeIndex CreateNewOptionalNodeIndex(INode parentNode, string propertyName, INode node)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameInsertionOptionalNodeIndex(parentNode, propertyName, node);
        }

        /// <summary>
        /// Creates a IxxxInsertNodeOperation object.
        /// </summary>
        private protected override IWriteableInsertNodeOperation CreateInsertNodeOperation(INode parentNode, string propertyName, int blockIndex, int index, INode node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameInsertNodeOperation(parentNode, propertyName, blockIndex, index, node, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxInsertBlockOperation object.
        /// </summary>
        private protected override IWriteableInsertBlockOperation CreateInsertBlockOperation(INode parentNode, string propertyName, int blockIndex, IBlock block, INode node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameInsertBlockOperation(parentNode, propertyName, blockIndex, block, node, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveBlockOperation object.
        /// </summary>
        private protected override IWriteableRemoveBlockOperation CreateRemoveBlockOperation(INode parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameRemoveBlockOperation(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveBlockViewOperation object.
        /// </summary>
        private protected override IWriteableRemoveBlockViewOperation CreateRemoveBlockViewOperation(INode parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameRemoveBlockViewOperation(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveNodeOperation object.
        /// </summary>
        private protected override IWriteableRemoveNodeOperation CreateRemoveNodeOperation(INode parentNode, string propertyName, int blockIndex, int index, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameRemoveNodeOperation(parentNode, propertyName, blockIndex, index, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxReplaceOperation object.
        /// </summary>
        private protected override IWriteableReplaceOperation CreateReplaceOperation(INode parentNode, string propertyName, int blockIndex, int index, INode newNode, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameReplaceOperation(parentNode, propertyName, blockIndex, index, newNode, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxAssignmentOperation object.
        /// </summary>
        private protected override IWriteableAssignmentOperation CreateAssignmentOperation(INode parentNode, string propertyName, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameAssignmentOperation(parentNode, propertyName, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeNodeOperation object.
        /// </summary>
        private protected override IWriteableChangeNodeOperation CreateChangeNodeOperation(INode parentNode, string propertyName, int value, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameChangeNodeOperation(parentNode, propertyName, value, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeBlockOperation object.
        /// </summary>
        private protected override IWriteableChangeBlockOperation CreateChangeBlockOperation(INode parentNode, string propertyName, int blockIndex, ReplicationStatus replication, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameChangeBlockOperation(parentNode, propertyName, blockIndex, replication, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxSplitBlockOperation object.
        /// </summary>
        private protected override IWriteableSplitBlockOperation CreateSplitBlockOperation(INode parentNode, string propertyName, int blockIndex, int index, IBlock newBlock, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameSplitBlockOperation(parentNode, propertyName, blockIndex, index, newBlock, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMergeBlocksOperation object.
        /// </summary>
        private protected override IWriteableMergeBlocksOperation CreateMergeBlocksOperation(INode parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameMergeBlocksOperation(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMoveNodeOperation object.
        /// </summary>
        private protected override IWriteableMoveNodeOperation CreateMoveNodeOperation(INode parentNode, string propertyName, int blockIndex, int index, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameMoveNodeOperation(parentNode, propertyName, blockIndex, index, direction, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMoveBlockOperation object.
        /// </summary>
        private protected override IWriteableMoveBlockOperation CreateMoveBlockOperation(INode parentNode, string propertyName, int blockIndex, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameMoveBlockOperation(parentNode, propertyName, blockIndex, direction, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxExpandArgumentOperation object.
        /// </summary>
        private protected override IWriteableExpandArgumentOperation CreateExpandArgumentOperation(INode parentNode, string propertyName, IBlock block, IArgument argument, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameExpandArgumentOperation(parentNode, propertyName, block, argument, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxGenericRefreshOperation object.
        /// </summary>
        private protected override IWriteableGenericRefreshOperation CreateGenericRefreshOperation(IWriteableNodeState refreshState, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameGenericRefreshOperation((IFrameNodeState)refreshState, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxOperationGroupList object.
        /// </summary>
        private protected override IWriteableOperationGroupList CreateOperationGroupStack()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameOperationGroupList();
        }

        /// <summary>
        /// Creates a IxxxOperationGroupReadOnlyList object.
        /// </summary>
        private protected override IWriteableOperationGroupReadOnlyList CreateOperationGroupReadOnlyStack(IWriteableOperationGroupList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameOperationGroupReadOnlyList((IFrameOperationGroupList)list);
        }

        /// <summary>
        /// Creates a IxxxOperationList object.
        /// </summary>
        private protected override IWriteableOperationList CreateOperationList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameOperationList();
        }

        /// <summary>
        /// Creates a IxxxOperationReadOnlyList object.
        /// </summary>
        private protected override IWriteableOperationReadOnlyList CreateOperationReadOnlyList(IWriteableOperationList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameOperationReadOnlyList((IFrameOperationList)list);
        }

        /// <summary>
        /// Creates a IxxxOperationGroup object.
        /// </summary>
        private protected override IWriteableOperationGroup CreateOperationGroup(IWriteableOperationReadOnlyList operationList, IWriteableGenericRefreshOperation refresh)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameOperationGroup((IFrameOperationReadOnlyList)operationList, (IFrameGenericRefreshOperation)refresh);
        }
        #endregion
    }
}
