using BaseNode;
using EaslyController.Frame;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System;

namespace EaslyController.Focus
{
    /// <summary>
    /// Controller for a node tree.
    /// This controller supports:
    /// * Operations to modify the tree.
    /// * Organizing nodes and their content in cells, assigning line and column numbers.
    /// </summary>
    public interface IFocusController : IFrameController
    {
        /// <summary>
        /// Index of the root node.
        /// </summary>
        new IFocusRootNodeIndex RootIndex { get; }

        /// <summary>
        /// State of the root node.
        /// </summary>
        new IFocusPlaceholderNodeState RootState { get; }

        /// <summary>
        /// Called when a state is created.
        /// </summary>
        new event Action<IFocusNodeState> NodeStateCreated;

        /// <summary>
        /// Called when a state is initialized.
        /// </summary>
        new event Action<IFocusNodeState> NodeStateInitialized;

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        new event Action<IFocusNodeState> NodeStateRemoved;

        /// <summary>
        /// Called when a block list inner is created
        /// </summary>
        new event Action<IFocusBlockListInner> BlockListInnerCreated;

        /// <summary>
        /// Called when a block state is inserted.
        /// </summary>
        new event Action<IFocusInsertBlockOperation> BlockStateInserted;

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        new event Action<IFocusRemoveBlockOperation> BlockStateRemoved;

        /// <summary>
        /// Called when a state is inserted.
        /// </summary>
        new event Action<IFocusInsertNodeOperation> StateInserted;

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        new event Action<IFocusRemoveNodeOperation> StateRemoved;

        /// <summary>
        /// Called when a state is replaced.
        /// </summary>
        new event Action<IFocusReplaceOperation> StateReplaced;

        /// <summary>
        /// Called when a state is assigned.
        /// </summary>
        new event Action<IFocusAssignmentOperation> StateAssigned;

        /// <summary>
        /// Called when a state is unassigned.
        /// </summary>
        new event Action<IFocusAssignmentOperation> StateUnassigned;

        /// <summary>
        /// Called when a state is moved.
        /// </summary>
        new event Action<IFocusMoveNodeOperation> StateMoved;

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        new event Action<IFocusMoveBlockOperation> BlockStateMoved;

        /// <summary>
        /// Called when a block is split.
        /// </summary>
        new event Action<IFocusSplitBlockOperation> BlockSplit;

        /// <summary>
        /// Called when two blocks are merged.
        /// </summary>
        new event Action<IFocusMergeBlocksOperation> BlocksMerged;
    }

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports:
    /// * Operations to modify the tree.
    /// * Organizing nodes and their content in cells, assigning line and column numbers.
    /// * Keeping the focus in a cell.
    /// </summary>
    public class FocusController : FrameController, IFocusController
    {
        #region Init
        /// <summary>
        /// Creates and initializes a new instance of a <see cref="FocusController"/> object.
        /// </summary>
        /// <param name="nodeIndex">Index of the root of the node tree.</param>
        public static IFocusController Create(IFocusRootNodeIndex nodeIndex)
        {
            FocusController Controller = new FocusController();
            Controller.SetRoot(nodeIndex);
            return Controller;
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="FocusController"/> object.
        /// </summary>
        protected FocusController()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Index of the root node.
        /// </summary>
        public new IFocusRootNodeIndex RootIndex { get { return (IFocusRootNodeIndex)base.RootIndex; } }

        /// <summary>
        /// State of the root node.
        /// </summary>
        public new IFocusPlaceholderNodeState RootState { get { return (IFocusPlaceholderNodeState)base.RootState; } }

        /// <summary>
        /// Called when a state is created.
        /// </summary>
        public new event Action<IFocusNodeState> NodeStateCreated
        {
            add { AddNodeStateCreatedDelegate((Action<IReadOnlyNodeState>)value); }
            remove { RemoveNodeStateCreatedDelegate((Action<IReadOnlyNodeState>)value); }
        }

        /// <summary>
        /// Called when a state is initialized.
        /// </summary>
        public new event Action<IFocusNodeState> NodeStateInitialized
        {
            add { AddNodeStateInitializedDelegate((Action<IReadOnlyNodeState>)value); }
            remove { RemoveNodeStateInitializedDelegate((Action<IReadOnlyNodeState>)value); }
        }

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        public new event Action<IFocusNodeState> NodeStateRemoved
        {
            add { AddNodeStateRemovedDelegate((Action<IReadOnlyNodeState>)value); }
            remove { RemoveNodeStateRemovedDelegate((Action<IReadOnlyNodeState>)value); }
        }

        /// <summary>
        /// Called when a block list inner is created
        /// </summary>
        public new event Action<IFocusBlockListInner> BlockListInnerCreated
        {
            add { AddBlockListInnerCreatedDelegate((Action<IReadOnlyBlockListInner>)value); }
            remove { RemoveBlockListInnerCreatedDelegate((Action<IReadOnlyBlockListInner>)value); }
        }

        /// <summary>
        /// Called when a block state is inserted.
        /// </summary>
        public new event Action<IFocusInsertBlockOperation> BlockStateInserted
        {
            add { AddBlockStateInsertedDelegate((Action<IWriteableInsertBlockOperation>)value); }
            remove { RemoveBlockStateInsertedDelegate((Action<IWriteableInsertBlockOperation>)value); }
        }

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        public new event Action<IFocusRemoveBlockOperation> BlockStateRemoved
        {
            add { AddBlockStateRemovedDelegate((Action<IWriteableRemoveBlockOperation>)value); }
            remove { RemoveBlockStateRemovedDelegate((Action<IWriteableRemoveBlockOperation>)value); }
        }

        /// <summary>
        /// Called when a state is inserted.
        /// </summary>
        public new event Action<IFocusInsertNodeOperation> StateInserted
        {
            add { AddStateInsertedDelegate((Action<IWriteableInsertNodeOperation>)value); }
            remove { RemoveStateInsertedDelegate((Action<IWriteableInsertNodeOperation>)value); }
        }

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        public new event Action<IFocusRemoveNodeOperation> StateRemoved
        {
            add { AddStateRemovedDelegate((Action<IWriteableRemoveNodeOperation>)value); }
            remove { RemoveStateRemovedDelegate((Action<IWriteableRemoveNodeOperation>)value); }
        }

        /// <summary>
        /// Called when a state is replaced.
        /// </summary>
        public new event Action<IFocusReplaceOperation> StateReplaced
        {
            add { AddStateReplacedDelegate((Action<IWriteableReplaceOperation>)value); }
            remove { RemoveStateReplacedDelegate((Action<IWriteableReplaceOperation>)value); }
        }

        /// <summary>
        /// Called when a state is assigned.
        /// </summary>
        public new event Action<IFocusAssignmentOperation> StateAssigned
        {
            add { AddStateAssignedDelegate((Action<IWriteableAssignmentOperation>)value); }
            remove { RemoveStateAssignedDelegate((Action<IWriteableAssignmentOperation>)value); }
        }

        /// <summary>
        /// Called when a state is unassigned.
        /// </summary>
        public new event Action<IFocusAssignmentOperation> StateUnassigned
        {
            add { AddStateUnassignedDelegate((Action<IWriteableAssignmentOperation>)value); }
            remove { RemoveStateUnassignedDelegate((Action<IWriteableAssignmentOperation>)value); }
        }

        /// <summary>
        /// Called when a state is moved.
        /// </summary>
        public new event Action<IFocusMoveNodeOperation> StateMoved
        {
            add { AddStateMovedDelegate((Action<IWriteableMoveNodeOperation>)value); }
            remove { RemoveStateMovedDelegate((Action<IWriteableMoveNodeOperation>)value); }
        }

        /// <summary>
        /// Called when a block state is moved.
        /// </summary>
        public new event Action<IFocusMoveBlockOperation> BlockStateMoved
        {
            add { AddBlockStateMovedDelegate((Action<IWriteableMoveBlockOperation>)value); }
            remove { RemoveBlockStateMovedDelegate((Action<IWriteableMoveBlockOperation>)value); }
        }

        /// <summary>
        /// Called when a block is split.
        /// </summary>
        public new event Action<IFocusSplitBlockOperation> BlockSplit
        {
            add { AddBlockSplitDelegate((Action<IWriteableSplitBlockOperation>)value); }
            remove { RemoveBlockSplitDelegate((Action<IWriteableSplitBlockOperation>)value); }
        }

        /// <summary>
        /// Called when two blocks are merged.
        /// </summary>
        public new event Action<IFocusMergeBlocksOperation> BlocksMerged
        {
            add { AddBlocksMergedDelegate((Action<IWriteableMergeBlocksOperation>)value); }
            remove { RemoveBlocksMergedDelegate((Action<IWriteableMergeBlocksOperation>)value); }
        }

        /// <summary>
        /// State table.
        /// </summary>
        protected new IFocusIndexNodeStateReadOnlyDictionary StateTable { get { return (IFocusIndexNodeStateReadOnlyDictionary)base.StateTable; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        protected override IReadOnlyIndexNodeStateDictionary CreateStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusIndexNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxIndexNodeStateReadOnlyDictionary object.
        /// </summary>
        protected override IReadOnlyIndexNodeStateReadOnlyDictionary CreateStateTableReadOnly(IReadOnlyIndexNodeStateDictionary stateTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusIndexNodeStateReadOnlyDictionary((IFocusIndexNodeStateDictionary)stateTable);
        }

        /// <summary>
        /// Creates a IxxxInnerDictionary{string} object.
        /// </summary>
        protected override IReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusInnerDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxInnerReadOnlyDictionary{string} object.
        /// </summary>
        protected override IReadOnlyInnerReadOnlyDictionary<string> CreateInnerTableReadOnly(IReadOnlyInnerDictionary<string> innerTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusInnerReadOnlyDictionary<string>((IFocusInnerDictionary<string>)innerTable);
        }

        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        protected override IReadOnlyIndexNodeStateDictionary CreateChildStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusIndexNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxxBrowseContext object.
        /// </summary>
        protected override IReadOnlyBrowseContext CreateBrowseContext(IReadOnlyBrowseContext parentBrowseContext, IReadOnlyNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusBrowseContext((IFocusNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        protected override IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreatePlaceholderInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingPlaceholderNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusPlaceholderInner<IFocusBrowsingPlaceholderNodeIndex, FocusBrowsingPlaceholderNodeIndex>((IFocusNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxOptionalInner{IxxxBrowsingOptionalNodeIndex} object.
        /// </summary>
        protected override IReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex> CreateOptionalInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingOptionalNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusOptionalInner<IFocusBrowsingOptionalNodeIndex, FocusBrowsingOptionalNodeIndex>((IFocusNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxListInner{IxxxBrowsingListNodeIndex} object.
        /// </summary>
        protected override IReadOnlyListInner<IReadOnlyBrowsingListNodeIndex> CreateListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingListNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusListInner<IFocusBrowsingListNodeIndex, FocusBrowsingListNodeIndex>((IFocusNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxBlockListInner{IxxxBrowsingBlockNodeIndex} object.
        /// </summary>
        protected override IReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex> CreateBlockListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingBlockNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusBlockListInner<IFocusBrowsingBlockNodeIndex, FocusBrowsingBlockNodeIndex>((IFocusNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeState CreateRootNodeState(IReadOnlyRootNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusPlaceholderNodeState((IFocusRootNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxInsertionNewBlockNodeIndex object.
        /// </summary>
        protected override IWriteableInsertionNewBlockNodeIndex CreateNewBlockNodeIndex(INode parentNode, string propertyName, INode node, int blockIndex, IPattern patternNode, IIdentifier sourceNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusInsertionNewBlockNodeIndex(parentNode, propertyName, node, 0, patternNode, sourceNode);
        }

        /// <summary>
        /// Creates a IxxxWriteableInsertionOptionalNodeIndex object.
        /// </summary>
        protected override IWriteableInsertionOptionalNodeIndex CreateNewOptionalNodeIndex(INode parentNode, string propertyName, INode node)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusInsertionOptionalNodeIndex(parentNode, propertyName, node);
        }

        /// <summary>
        /// Creates a IxxxInsertNodeOperation object.
        /// </summary>
        protected override IWriteableInsertNodeOperation CreateInsertNodeOperation(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableInsertionCollectionNodeIndex insertionIndex, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusInsertNodeOperation((IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex>)inner, (IFocusInsertionCollectionNodeIndex)insertionIndex, isNested);
        }

        /// <summary>
        /// Creates a IxxxInsertBlockOperation object.
        /// </summary>
        protected override IWriteableInsertBlockOperation CreateInsertBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableInsertionNewBlockNodeIndex blockIndex, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusInsertBlockOperation((IFocusBlockListInner<IFocusBrowsingBlockNodeIndex>)inner, (IFocusInsertionNewBlockNodeIndex)blockIndex, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveBlockOperation object.
        /// </summary>
        protected override IWriteableRemoveBlockOperation CreateRemoveBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex blockIndex, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusRemoveBlockOperation((IFocusBlockListInner<IFocusBrowsingBlockNodeIndex>)inner, (IFocusBrowsingExistingBlockNodeIndex)blockIndex, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveNodeOperation object.
        /// </summary>
        protected override IWriteableRemoveNodeOperation CreateRemoveNodeOperation(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusRemoveNodeOperation((IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex>)inner, (IFocusBrowsingCollectionNodeIndex)nodeIndex, isNested);
        }

        /// <summary>
        /// Creates a IxxxReplaceOperation object.
        /// </summary>
        protected override IWriteableReplaceOperation CreateReplaceOperation(IWriteableInner<IWriteableBrowsingChildIndex> inner, IWriteableInsertionChildIndex replacementIndex, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusReplaceOperation((IFocusInner<IFocusBrowsingChildIndex>)inner, (IFocusInsertionChildIndex)replacementIndex, isNested);
        }

        /// <summary>
        /// Creates a IxxxAssignmentOperation object.
        /// </summary>
        protected override IWriteableAssignmentOperation CreateAssignmentOperation(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> inner, IWriteableBrowsingOptionalNodeIndex nodeIndex, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusAssignmentOperation((IFocusOptionalInner<IFocusBrowsingOptionalNodeIndex>)inner, (IFocusBrowsingOptionalNodeIndex)nodeIndex, isNested);
        }

        /// <summary>
        /// Creates a IxxxSplitBlockOperation object.
        /// </summary>
        protected override IWriteableSplitBlockOperation CreateSplitBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusSplitBlockOperation((IFocusBlockListInner<IFocusBrowsingBlockNodeIndex>)inner, (IFocusBrowsingExistingBlockNodeIndex)nodeIndex, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMergeBlocksOperation object.
        /// </summary>
        protected override IWriteableMergeBlocksOperation CreateMergeBlocksOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusMergeBlocksOperation((IFocusBlockListInner<IFocusBrowsingBlockNodeIndex>)inner, (IFocusBrowsingExistingBlockNodeIndex)nodeIndex, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMoveNodeOperation object.
        /// </summary>
        protected override IWriteableMoveNodeOperation CreateMoveNodeOperation(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusMoveNodeOperation((IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex>)inner, (IFocusBrowsingCollectionNodeIndex)nodeIndex, direction, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMoveBlockOperation object.
        /// </summary>
        protected override IWriteableMoveBlockOperation CreateMoveBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex, int direction, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusMoveBlockOperation((IFocusBlockListInner<IFocusBrowsingBlockNodeIndex>)inner, blockIndex, direction, isNested);
        }

        /// <summary>
        /// Creates a IxxxExpandArgumentOperation object.
        /// </summary>
        protected override IWriteableExpandArgumentOperation CreateExpandArgumentOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableInsertionNewBlockNodeIndex blockIndex, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusExpandArgumentOperation((IFocusBlockListInner<IFocusBrowsingBlockNodeIndex>)inner, (IFocusInsertionNewBlockNodeIndex)blockIndex, isNested);
        }

        /// <summary>
        /// Creates a IxxxGenericRefreshOperation object.
        /// </summary>
        protected override IWriteableGenericRefreshOperation CreateGenericRefreshOperation(IWriteableNodeState refreshState, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusGenericRefreshOperation((IFocusNodeState)refreshState, isNested);
        }
        #endregion
    }
}
