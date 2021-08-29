namespace EaslyController.Writeable
{
    using System;
    using BaseNode;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports operations to modify the tree.
    /// </summary>
    public partial class WriteableController : ReadOnlyController, IWriteableController
    {
        #region Init
        /// <summary>
        /// Creates and initializes a new instance of a <see cref="WriteableController"/> object.
        /// </summary>
        /// <param name="nodeIndex">Index of the root of the node tree.</param>
        public static IWriteableController Create(IWriteableRootNodeIndex nodeIndex)
        {
            WriteableController Controller = new WriteableController();
            Controller.SetRoot(nodeIndex);
            Controller.SetInitialized();
            return Controller;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableController"/> class.
        /// </summary>
        private protected WriteableController()
        {
            _OperationStack = CreateOperationGroupStack();
            DebugObjects.AddReference(_OperationStack);
            OperationStack = _OperationStack.ToReadOnly();
            RedoIndex = 0;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        private protected override IReadOnlyIndexNodeStateDictionary CreateStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableIndexNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxInnerDictionary{string} object.
        /// </summary>
        private protected override IReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableInnerDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        private protected override IReadOnlyIndexNodeStateDictionary CreateChildStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableIndexNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxxBrowseContext object.
        /// </summary>
        private protected override IReadOnlyBrowseContext CreateBrowseContext(IReadOnlyBrowseContext parentBrowseContext, IReadOnlyNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableBrowseContext((IWriteableNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreatePlaceholderInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingPlaceholderNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteablePlaceholderInner<IWriteableBrowsingPlaceholderNodeIndex, WriteableBrowsingPlaceholderNodeIndex>((IWriteableNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxOptionalInner{IxxxBrowsingOptionalNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex> CreateOptionalInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingOptionalNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex, WriteableBrowsingOptionalNodeIndex>((IWriteableNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxListInner{IxxxBrowsingListNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyListInner<IReadOnlyBrowsingListNodeIndex> CreateListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingListNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableListInner<IWriteableBrowsingListNodeIndex, WriteableBrowsingListNodeIndex>((IWriteableNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxBlockListInner{IxxxBrowsingBlockNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex> CreateBlockListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingBlockNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableBlockListInner<IWriteableBrowsingBlockNodeIndex, WriteableBrowsingBlockNodeIndex>((IWriteableNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeState CreateRootNodeState(IReadOnlyRootNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteablePlaceholderNodeState<IWriteableInner<IWriteableBrowsingChildIndex>>((IWriteableRootNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxWriteableInsertionOptionalNodeIndex object.
        /// </summary>
        private protected virtual IWriteableInsertionOptionalNodeIndex CreateNewOptionalNodeIndex(Node parentNode, string propertyName, Node node)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableInsertionOptionalNodeIndex(parentNode, propertyName, node);
        }

        /// <summary>
        /// Creates a IxxxInsertNodeOperation object.
        /// </summary>
        private protected virtual IWriteableInsertNodeOperation CreateInsertNodeOperation(Node parentNode, string propertyName, int blockIndex, int index, Node node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableInsertNodeOperation(parentNode, propertyName, blockIndex, index, node, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxInsertBlockOperation object.
        /// </summary>
        private protected virtual IWriteableInsertBlockOperation CreateInsertBlockOperation(Node parentNode, string propertyName, int blockIndex, IBlock block, Node node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableInsertBlockOperation(parentNode, propertyName, blockIndex, block, node, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveBlockOperation object.
        /// </summary>
        private protected virtual IWriteableRemoveBlockOperation CreateRemoveBlockOperation(Node parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableRemoveBlockOperation(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveBlockViewOperation object.
        /// </summary>
        private protected virtual IWriteableRemoveBlockViewOperation CreateRemoveBlockViewOperation(Node parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableRemoveBlockViewOperation(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveNodeOperation object.
        /// </summary>
        private protected virtual IWriteableRemoveNodeOperation CreateRemoveNodeOperation(Node parentNode, string propertyName, int blockIndex, int index, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableRemoveNodeOperation(parentNode, propertyName, blockIndex, index, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxReplaceOperation object.
        /// </summary>
        private protected virtual IWriteableReplaceOperation CreateReplaceOperation(Node parentNode, string propertyName, int blockIndex, int index, Node newNode, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableReplaceOperation(parentNode, propertyName, blockIndex, index, newNode, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxAssignmentOperation object.
        /// </summary>
        private protected virtual IWriteableAssignmentOperation CreateAssignmentOperation(Node parentNode, string propertyName, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableAssignmentOperation(parentNode, propertyName, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeDiscreteValueOperation object.
        /// </summary>
        private protected virtual IWriteableChangeDiscreteValueOperation CreateChangeDiscreteValueOperation(Node parentNode, string propertyName, int value, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableChangeDiscreteValueOperation(parentNode, propertyName, value, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeTextOperation object.
        /// </summary>
        private protected virtual IWriteableChangeTextOperation CreateChangeTextOperation(Node parentNode, string propertyName, string text, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableChangeTextOperation(parentNode, propertyName, text, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeCommentOperation object.
        /// </summary>
        private protected virtual IWriteableChangeCommentOperation CreateChangeCommentOperation(Node parentNode, string text, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableChangeCommentOperation(parentNode, text, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeBlockOperation object.
        /// </summary>
        private protected virtual IWriteableChangeBlockOperation CreateChangeBlockOperation(Node parentNode, string propertyName, int blockIndex, ReplicationStatus replication, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableChangeBlockOperation(parentNode, propertyName, blockIndex, replication, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxSplitBlockOperation object.
        /// </summary>
        private protected virtual IWriteableSplitBlockOperation CreateSplitBlockOperation(Node parentNode, string propertyName, int blockIndex, int index, IBlock newBlock, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableSplitBlockOperation(parentNode, propertyName, blockIndex, index, newBlock, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMergeBlocksOperation object.
        /// </summary>
        private protected virtual IWriteableMergeBlocksOperation CreateMergeBlocksOperation(Node parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableMergeBlocksOperation(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMoveNodeOperation object.
        /// </summary>
        private protected virtual IWriteableMoveNodeOperation CreateMoveNodeOperation(Node parentNode, string propertyName, int blockIndex, int index, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableMoveNodeOperation(parentNode, propertyName, blockIndex, index, direction, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMoveBlockOperation object.
        /// </summary>
        private protected virtual IWriteableMoveBlockOperation CreateMoveBlockOperation(Node parentNode, string propertyName, int blockIndex, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableMoveBlockOperation(parentNode, propertyName, blockIndex, direction, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxExpandArgumentOperation object.
        /// </summary>
        private protected virtual IWriteableExpandArgumentOperation CreateExpandArgumentOperation(Node parentNode, string propertyName, IBlock block, Node node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableExpandArgumentOperation(parentNode, propertyName, block, node, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxGenericRefreshOperation object.
        /// </summary>
        private protected virtual IWriteableGenericRefreshOperation CreateGenericRefreshOperation(IWriteableNodeState refreshState, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableGenericRefreshOperation(refreshState, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxOperationGroupList object.
        /// </summary>
        private protected virtual IWriteableOperationGroupList CreateOperationGroupStack()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableOperationGroupList();
        }

        /// <summary>
        /// Creates a IxxxOperationList object.
        /// </summary>
        private protected virtual IWriteableOperationList CreateOperationList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableOperationList();
        }

        /// <summary>
        /// Creates a IxxxOperationGroup object.
        /// </summary>
        private protected virtual IWriteableOperationGroup CreateOperationGroup(IWriteableOperationReadOnlyList operationList, IWriteableGenericRefreshOperation refresh)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableOperationGroup(operationList, refresh);
        }
        #endregion
    }
}
