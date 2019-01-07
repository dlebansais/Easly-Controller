using BaseNode;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System;
using System.Diagnostics;

namespace EaslyController.Frame
{
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
        /// Called when a block list inner is created
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
        new event Action<IFrameBrowsingChildIndex, IFrameNodeState, int> StateMoved;

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        new event Action<IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>, int, int> BlockStateMoved;

        /// <summary>
        /// Called when a block is split.
        /// </summary>
        new event Action<IFrameSplitBlockOperation> BlockSplit;

        /// <summary>
        /// Called when two blocks are merged.
        /// </summary>
        new event Action<IFrameMergeBlocksOperation> BlocksMerged;
    }

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
        /// Called when a block list inner is created
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
        /// Called when a state is moved.
        /// </summary>
        public new event Action<IFrameBrowsingChildIndex, IFrameNodeState, int> StateMoved
        {
            add { AddStateMovedDelegate((Action<IWriteableBrowsingChildIndex, IWriteableNodeState, int>)value); }
            remove { RemoveStateMovedDelegate((Action<IWriteableBrowsingChildIndex, IWriteableNodeState, int>)value); }
        }

        /// <summary>
        /// Called when a block state is moved.
        /// </summary>
        public new event Action<IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>, int, int> BlockStateMoved
        {
            add { AddBlockStateMovedDelegate((Action<IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>, int, int>)value); }
            remove { RemoveBlockStateMovedDelegate((Action<IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>, int, int>)value); }
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
        /// Creates a IxxxInsertionNewBlockNodeIndex object.
        /// </summary>
        protected override IWriteableInsertionNewBlockNodeIndex CreateNewBlockNodeIndex(INode parentNode, string propertyName, INode node, int blockIndex, IPattern patternNode, IIdentifier sourceNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameInsertionNewBlockNodeIndex(parentNode, propertyName, node, 0, patternNode, sourceNode);
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
        protected override IWriteableInsertNodeOperation CreateInsertNodeOperation(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableInsertionCollectionNodeIndex insertionIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameInsertNodeOperation((IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex>)inner, (IFrameInsertionCollectionNodeIndex)insertionIndex);
        }

        /// <summary>
        /// Creates a IxxxInsertBlockOperation object.
        /// </summary>
        protected override IWriteableInsertBlockOperation CreateInsertBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameInsertBlockOperation((IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)inner, blockIndex);
        }

        /// <summary>
        /// Creates a IxxxRemoveBlockOperation object.
        /// </summary>
        protected override IWriteableRemoveBlockOperation CreateRemoveBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex blockIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameRemoveBlockOperation((IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)inner, (IFrameBrowsingExistingBlockNodeIndex)blockIndex);
        }

        /// <summary>
        /// Creates a IxxxRemoveNodeOperation object.
        /// </summary>
        protected override IWriteableRemoveNodeOperation CreateRemoveNodeOperation(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameRemoveNodeOperation((IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex>)inner, (IFrameBrowsingCollectionNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxReplaceOperation object.
        /// </summary>
        protected override IWriteableReplaceOperation CreateReplaceOperation(IWriteableInner<IWriteableBrowsingChildIndex> inner, IWriteableInsertionChildIndex replacementIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameReplaceOperation((IFrameInner<IFrameBrowsingChildIndex>)inner, (IFrameInsertionChildIndex)replacementIndex);
        }

        /// <summary>
        /// Creates a IxxxAssignmentOperation object.
        /// </summary>
        protected override IWriteableAssignmentOperation CreateAssignmentOperation(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> inner, IWriteableBrowsingOptionalNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameAssignmentOperation((IFrameOptionalInner<IFrameBrowsingOptionalNodeIndex>)inner, (IFrameBrowsingOptionalNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxSplitBlockOperation object.
        /// </summary>
        protected override IWriteableSplitBlockOperation CreateSplitBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameSplitBlockOperation((IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)inner, (IFrameBrowsingExistingBlockNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxExpandArgumentOperation object.
        /// </summary>
        protected override IWriteableExpandArgumentOperation CreateExpandArgumentOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameController));
            return new FrameExpandArgumentOperation((IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)inner, blockIndex);
        }
        #endregion
    }
}
