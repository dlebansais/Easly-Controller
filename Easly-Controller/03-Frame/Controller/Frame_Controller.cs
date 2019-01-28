using BaseNode;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System;

namespace EaslyController.Frame
{
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
        /// Initializes a new instance of a <see cref="FrameController"/> object.
        /// </summary>
        protected FrameController()
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
        protected new IFrameIndexNodeStateReadOnlyDictionary StateTable { get { return (IFrameIndexNodeStateReadOnlyDictionary)base.StateTable; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        protected override IReadOnlyIndexNodeStateDictionary CreateStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameIndexNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxIndexNodeStateReadOnlyDictionary object.
        /// </summary>
        protected override IReadOnlyIndexNodeStateReadOnlyDictionary CreateStateTableReadOnly(IReadOnlyIndexNodeStateDictionary stateTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameIndexNodeStateReadOnlyDictionary((IFrameIndexNodeStateDictionary)stateTable);
        }

        /// <summary>
        /// Creates a IxxxInnerDictionary{string} object.
        /// </summary>
        protected override IReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameInnerDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxInnerReadOnlyDictionary{string} object.
        /// </summary>
        protected override IReadOnlyInnerReadOnlyDictionary<string> CreateInnerTableReadOnly(IReadOnlyInnerDictionary<string> innerTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameInnerReadOnlyDictionary<string>((IFrameInnerDictionary<string>)innerTable);
        }

        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        protected override IReadOnlyIndexNodeStateDictionary CreateChildStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameIndexNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxxBrowseContext object.
        /// </summary>
        protected override IReadOnlyBrowseContext CreateBrowseContext(IReadOnlyBrowseContext parentBrowseContext, IReadOnlyNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameBrowseContext((IFrameNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        protected override IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreatePlaceholderInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingPlaceholderNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FramePlaceholderInner<IFrameBrowsingPlaceholderNodeIndex, FrameBrowsingPlaceholderNodeIndex>((IFrameNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxOptionalInner{IxxxBrowsingOptionalNodeIndex} object.
        /// </summary>
        protected override IReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex> CreateOptionalInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingOptionalNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameOptionalInner<IFrameBrowsingOptionalNodeIndex, FrameBrowsingOptionalNodeIndex>((IFrameNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxListInner{IxxxBrowsingListNodeIndex} object.
        /// </summary>
        protected override IReadOnlyListInner<IReadOnlyBrowsingListNodeIndex> CreateListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingListNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameListInner<IFrameBrowsingListNodeIndex, FrameBrowsingListNodeIndex>((IFrameNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxBlockListInner{IxxxBrowsingBlockNodeIndex} object.
        /// </summary>
        protected override IReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex> CreateBlockListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingBlockNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameBlockListInner<IFrameBrowsingBlockNodeIndex, FrameBrowsingBlockNodeIndex>((IFrameNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeState CreateRootNodeState(IReadOnlyRootNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FramePlaceholderNodeState((IFrameRootNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxWriteableInsertionOptionalNodeIndex object.
        /// </summary>
        protected override IWriteableInsertionOptionalNodeIndex CreateNewOptionalNodeIndex(INode parentNode, string propertyName, INode node)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameInsertionOptionalNodeIndex(parentNode, propertyName, node);
        }

        /// <summary>
        /// Creates a IxxxInsertNodeOperation object.
        /// </summary>
        protected override IWriteableInsertNodeOperation CreateInsertNodeOperation(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableInsertionCollectionNodeIndex insertionIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameInsertNodeOperation((IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex>)inner, (IFrameInsertionCollectionNodeIndex)insertionIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxInsertBlockOperation object.
        /// </summary>
        protected override IWriteableInsertBlockOperation CreateInsertBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex, IBlock block, INode node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameInsertBlockOperation((IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)inner, blockIndex, block, node, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveBlockOperation object.
        /// </summary>
        protected override IWriteableRemoveBlockOperation CreateRemoveBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameRemoveBlockOperation((IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)inner, (IFrameBrowsingExistingBlockNodeIndex)blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveBlockViewOperation object.
        /// </summary>
        protected override IWriteableRemoveBlockViewOperation CreateRemoveBlockViewOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameRemoveBlockViewOperation((IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)inner, blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveNodeOperation object.
        /// </summary>
        protected override IWriteableRemoveNodeOperation CreateRemoveNodeOperation(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameRemoveNodeOperation((IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex>)inner, (IFrameBrowsingCollectionNodeIndex)nodeIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxReplaceOperation object.
        /// </summary>
        protected override IWriteableReplaceOperation CreateReplaceOperation(IWriteableInner<IWriteableBrowsingChildIndex> inner, IWriteableInsertionChildIndex replacementIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameReplaceOperation((IFrameInner<IFrameBrowsingChildIndex>)inner, (IFrameInsertionChildIndex)replacementIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxAssignmentOperation object.
        /// </summary>
        protected override IWriteableAssignmentOperation CreateAssignmentOperation(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> inner, IWriteableBrowsingOptionalNodeIndex nodeIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameAssignmentOperation((IFrameOptionalInner<IFrameBrowsingOptionalNodeIndex>)inner, (IFrameBrowsingOptionalNodeIndex)nodeIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeNodeOperation object.
        /// </summary>
        protected override IWriteableChangeNodeOperation CreateChangeNodeOperation(IWriteableIndex nodeIndex, string propertyName, int value, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameChangeNodeOperation((IFrameIndex)nodeIndex, propertyName, value, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeBlockOperation object.
        /// </summary>
        protected override IWriteableChangeBlockOperation CreateChangeBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex, ReplicationStatus replication, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameChangeBlockOperation((IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)inner, blockIndex, replication, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxSplitBlockOperation object.
        /// </summary>
        protected override IWriteableSplitBlockOperation CreateSplitBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameSplitBlockOperation((IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)inner, (IFrameBrowsingExistingBlockNodeIndex)nodeIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMergeBlocksOperation object.
        /// </summary>
        protected override IWriteableMergeBlocksOperation CreateMergeBlocksOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameMergeBlocksOperation((IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)inner, (IFrameBrowsingExistingBlockNodeIndex)nodeIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMoveNodeOperation object.
        /// </summary>
        protected override IWriteableMoveNodeOperation CreateMoveNodeOperation(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameMoveNodeOperation((IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex>)inner, (IFrameBrowsingCollectionNodeIndex)nodeIndex, direction, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMoveBlockOperation object.
        /// </summary>
        protected override IWriteableMoveBlockOperation CreateMoveBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameMoveBlockOperation((IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)inner, blockIndex, direction, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxExpandArgumentOperation object.
        /// </summary>
        protected override IWriteableExpandArgumentOperation CreateExpandArgumentOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IBlock block, IArgument argument, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameExpandArgumentOperation((IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)inner, block, argument, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxGenericRefreshOperation object.
        /// </summary>
        protected override IWriteableGenericRefreshOperation CreateGenericRefreshOperation(IWriteableNodeState refreshState, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameGenericRefreshOperation((IFrameNodeState)refreshState, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxOperationList object.
        /// </summary>
        protected override IWriteableOperationList CreateOperationStack()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameOperationList();
        }

        /// <summary>
        /// Creates a IxxxOperationReadOnlyList object.
        /// </summary>
        protected override IWriteableOperationReadOnlyList CreateOperationReadOnlyStack(IWriteableOperationList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameOperationReadOnlyList((IFrameOperationList)list);
        }
        #endregion
    }
}
