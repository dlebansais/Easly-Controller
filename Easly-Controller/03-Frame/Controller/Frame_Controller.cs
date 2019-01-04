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
        new event Action<IFrameBrowsingExistingBlockNodeIndex, IFrameBlockState> BlockStateInserted;

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        new event Action<IFrameBrowsingExistingBlockNodeIndex, IFrameBlockState> BlockStateRemoved;

        /// <summary>
        /// Called when a state is inserted.
        /// </summary>
        new event Action<IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex>, IFrameBrowsingCollectionNodeIndex, IFrameNodeState, bool> StateInserted;

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        new event Action<IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex>, IFrameBrowsingCollectionNodeIndex, IFrameNodeState> StateRemoved;

        /// <summary>
        /// Called when a state is replaced.
        /// </summary>
        new event Action<IFrameInner<IFrameBrowsingChildIndex>, IFrameBrowsingChildIndex, IFrameNodeState> StateReplaced;
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
        public new event Action<IFrameBrowsingExistingBlockNodeIndex, IFrameBlockState> BlockStateInserted
        {
            add { AddBlockStateInsertedDelegate((Action<IWriteableBrowsingExistingBlockNodeIndex, IWriteableBlockState>)value); }
            remove { RemoveBlockStateInsertedDelegate((Action<IWriteableBrowsingExistingBlockNodeIndex, IWriteableBlockState>)value); }
        }

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        public new event Action<IFrameBrowsingExistingBlockNodeIndex, IFrameBlockState> BlockStateRemoved
        {
            add { AddBlockStateRemovedDelegate((Action<IWriteableBrowsingExistingBlockNodeIndex, IWriteableBlockState>)value); }
            remove { RemoveBlockStateRemovedDelegate((Action<IWriteableBrowsingExistingBlockNodeIndex, IWriteableBlockState>)value); }
        }

        /// <summary>
        /// Called when a state is inserted.
        /// </summary>
        public new event Action<IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex>, IFrameBrowsingCollectionNodeIndex, IFrameNodeState, bool> StateInserted
        {
            add { AddStateInsertedDelegate((Action<IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex>, IWriteableBrowsingCollectionNodeIndex, IWriteableNodeState, bool>)value); }
            remove { RemoveStateInsertedDelegate((Action<IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex>, IWriteableBrowsingCollectionNodeIndex, IWriteableNodeState, bool>)value); }
        }

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        public new event Action<IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex>, IFrameBrowsingCollectionNodeIndex, IFrameNodeState> StateRemoved
        {
            add { AddStateRemovedDelegate((Action<IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex>, IWriteableBrowsingCollectionNodeIndex, IWriteableNodeState>)value); }
            remove { RemoveStateRemovedDelegate((Action<IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex>, IWriteableBrowsingCollectionNodeIndex, IWriteableNodeState>)value); }
        }

        /// <summary>
        /// Called when a state is replaced.
        /// </summary>
        public new event Action<IFrameInner<IFrameBrowsingChildIndex>, IFrameBrowsingChildIndex, IFrameNodeState> StateReplaced
        {
            add { AddStateReplacedDelegate((Action<IWriteableInner<IWriteableBrowsingChildIndex>, IWriteableBrowsingChildIndex, IWriteableNodeState>)value); }
            remove { RemoveStateReplacedDelegate((Action<IWriteableInner<IWriteableBrowsingChildIndex>, IWriteableBrowsingChildIndex, IWriteableNodeState>)value); }
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
        #endregion
    }
}
