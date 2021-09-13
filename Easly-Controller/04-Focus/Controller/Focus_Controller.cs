namespace EaslyController.Focus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports:
    /// * Operations to modify the tree.
    /// * Organizing nodes and their content in cells, assigning line and column numbers.
    /// * Keeping the focus in a cell.
    /// </summary>
    public class FocusController : FrameController
    {
        #region Init
        /// <summary>
        /// Creates and initializes a new instance of a <see cref="FocusController"/> object.
        /// </summary>
        /// <param name="nodeIndex">Index of the root of the node tree.</param>
        public static FocusController Create(IFocusRootNodeIndex nodeIndex)
        {
            FocusController Controller = new FocusController();
            Controller.SetRoot(nodeIndex);
            Controller.SetCycleManagerList();
            Controller.SetInitialized();

            return Controller;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusController"/> class.
        /// </summary>
        private protected FocusController()
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
        /// State table.
        /// </summary>
        public new FocusNodeStateReadOnlyDictionary StateTable { get { return (FocusNodeStateReadOnlyDictionary)base.StateTable; } }

        /// <summary>
        /// List of operations that have been performed, and can be undone or redone.
        /// </summary>
        public new FocusOperationGroupReadOnlyList OperationStack { get { return (FocusOperationGroupReadOnlyList)base.OperationStack; } }

        /// <summary>
        /// List of supported cycle managers.
        /// </summary>
        public FocusCycleManagerList CycleManagerList { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks whether a node is member of a supported cycle.
        /// </summary>
        /// <param name="state">State corresponding to the node to check.</param>
        /// <param name="cycleManager">The cycle manager for this node type upon return. Null if none.</param>
        /// <returns>True if a cycle manager exists for the node.</returns>
        public virtual bool IsMemberOfCycle(IFocusNodeState state, out IFocusCycleManager cycleManager)
        {
            cycleManager = null;

            /*
            List<Type> Interfaces = new List<Type>(state.Node.GetType().GetInterfaces());

            foreach (FocusCycleManager Item in CycleManagerList)
                if (Interfaces.Contains(Item.InterfaceType))
                    cycleManager = Item;
            */

            Type NodeType = state.Node.GetType();
            foreach (IFocusCycleManager Item in CycleManagerList)
                if (Item.InterfaceType.IsAssignableFrom(NodeType))
                    cycleManager = Item;

            return cycleManager != null;
        }

        /// <summary>
        /// Replace an existing node with a new one, keeping its cycle.
        /// </summary>
        /// <param name="inner">The inner where the node is replaced.</param>
        /// <param name="cycleIndexList">Cycle of nodes that can replace the current node.</param>
        /// <param name="cyclePosition">New position in the cycle.</param>
        /// <param name="nodeIndex">Index of the replacing node upon return.</param>
        public virtual void Replace(IFocusInner inner, FocusInsertionChildNodeIndexList cycleIndexList, int cyclePosition, out IFocusBrowsingChildIndex nodeIndex)
        {
            Debug.Assert(cycleIndexList != null);
            Debug.Assert(cycleIndexList.Count >= 2);
            Debug.Assert(cyclePosition >= 0 && cyclePosition < cycleIndexList.Count);

            IFocusInsertionChildNodeIndex ReplacementIndex = cycleIndexList[cyclePosition];

            IndexToPositionAndNode(ReplacementIndex, out int BlockIndex, out int Index, out Node Node);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => ExecuteReplaceWithCycle(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoReplaceWithCycle(operation);
            IFocusReplaceWithCycleOperation Operation = CreateReplaceWithCycleOperation(inner.Owner.Node, inner.PropertyName, BlockIndex, Index, cycleIndexList, cyclePosition, HandlerRedo, HandlerUndo, isNested: false);

            ExecuteReplaceWithCycle(Operation);

            nodeIndex = Operation.NewBrowsingIndex;
            SetLastOperation(Operation);

            CheckInvariant();
        }

        private protected virtual void ExecuteReplaceWithCycle(IWriteableOperation operation)
        {
            IFocusReplaceWithCycleOperation ReplaceWithCycleOperation = (IFocusReplaceWithCycleOperation)operation;

            Node ParentNode = ReplaceWithCycleOperation.ParentNode;
            string PropertyName = ReplaceWithCycleOperation.PropertyName;
            IFocusInner<IFocusBrowsingChildIndex> Inner = GetInner(ParentNode, PropertyName) as IFocusInner<IFocusBrowsingChildIndex>;

            ReplaceState(ReplaceWithCycleOperation, Inner);

            IFocusCyclableNodeState NewState = StateTable[ReplaceWithCycleOperation.NewBrowsingIndex] as IFocusCyclableNodeState;
            Debug.Assert(NewState != null);

            NewState.RestoreCycleIndexList(ReplaceWithCycleOperation.CycleIndexList);

            NotifyStateReplaced(ReplaceWithCycleOperation);
        }

        private protected virtual void UndoReplaceWithCycle(IWriteableOperation operation)
        {
            IFocusReplaceWithCycleOperation ReplaceWithCycleOperation = (IFocusReplaceWithCycleOperation)operation;
            ReplaceWithCycleOperation = ReplaceWithCycleOperation.ToInverseReplace() as IFocusReplaceWithCycleOperation;

            Node ParentNode = ReplaceWithCycleOperation.ParentNode;
            string PropertyName = ReplaceWithCycleOperation.PropertyName;
            IFocusInner<IFocusBrowsingChildIndex> Inner = GetInner(ParentNode, PropertyName) as IFocusInner<IFocusBrowsingChildIndex>;

            ReplaceState(ReplaceWithCycleOperation, Inner);

            IFocusCyclableNodeState NewState = StateTable[ReplaceWithCycleOperation.NewBrowsingIndex] as IFocusCyclableNodeState;
            Debug.Assert(NewState != null);

            NewState.RestoreCycleIndexList(ReplaceWithCycleOperation.CycleIndexList);

            NotifyStateReplaced(ReplaceWithCycleOperation);
        }

        /// <summary>
        /// Changes the value of a text.
        /// </summary>
        /// <param name="nodeIndex">Index of the state with the string to change.</param>
        /// <param name="propertyName">Name of the property to change.</param>
        /// <param name="text">The new text.</param>
        /// <param name="oldCaretPosition">The old caret position.</param>
        /// <param name="newCaretPosition">The new caret position.</param>
        /// <param name="changeCaretBeforeText">True if the caret should be changed before the text, to preserve the caret invariant.</param>
        public virtual void ChangeTextAndCaretPosition(IFocusIndex nodeIndex, string propertyName, string text, int oldCaretPosition, int newCaretPosition, bool changeCaretBeforeText)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(StateTable.ContainsKey(nodeIndex));
            Debug.Assert(text != null);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoChangeText(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoChangeText(operation);
            IFocusNodeState State = (IFocusNodeState)StateTable[nodeIndex];
            FocusChangeTextOperation Operation = CreateChangeTextOperation(State.Node, propertyName, text, oldCaretPosition, newCaretPosition, changeCaretBeforeText, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        /// <summary>
        /// Changes the value of a comment.
        /// </summary>
        /// <param name="nodeIndex">Index of the state with the comment to change.</param>
        /// <param name="text">The new text.</param>
        /// <param name="oldCaretPosition">The old caret position.</param>
        /// <param name="newCaretPosition">The new caret position.</param>
        /// <param name="changeCaretBeforeText">True if the caret should be changed before the text, to preserve the caret invariant.</param>
        public virtual void ChangeCommentAndCaretPosition(IFocusIndex nodeIndex, string text, int oldCaretPosition, int newCaretPosition, bool changeCaretBeforeText)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(StateTable.ContainsKey(nodeIndex));
            Debug.Assert(text != null);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoChangeComment(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoChangeComment(operation);
            IFocusNodeState State = (IFocusNodeState)StateTable[nodeIndex];
            FocusChangeCommentOperation Operation = CreateChangeCommentOperation(State.Node, text, oldCaretPosition, newCaretPosition, changeCaretBeforeText, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }
        #endregion

        #region Implementation
        private protected virtual void SetCycleManagerList()
        {
            Debug.Assert(!IsInitialized); // Must be called during initialization

            CycleManagerList = CreateCycleManagerList();
            CycleManagerList.Add(CreateCycleManagerBody());
            CycleManagerList.Add(CreateCycleManagerFeature());
        }

        private protected override void CheckContextConsistency(ReadOnlyBrowseContext browseContext)
        {
            ((FocusBrowseContext)browseContext).CheckConsistency();
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        private protected override ReadOnlyNodeStateDictionary CreateStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxInnerDictionary{string} object.
        /// </summary>
        private protected override ReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusInnerDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        private protected override ReadOnlyNodeStateDictionary CreateChildStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxxBrowseContext object.
        /// </summary>
        private protected override ReadOnlyBrowseContext CreateBrowseContext(ReadOnlyBrowseContext parentBrowseContext, IReadOnlyNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusBrowseContext((IFocusNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreatePlaceholderInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingPlaceholderNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusPlaceholderInner<IFocusBrowsingPlaceholderNodeIndex>((IFocusNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxOptionalInner{IxxxBrowsingOptionalNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex> CreateOptionalInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingOptionalNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusOptionalInner<IFocusBrowsingOptionalNodeIndex>((IFocusNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxListInner{IxxxBrowsingListNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyListInner<IReadOnlyBrowsingListNodeIndex> CreateListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingListNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusListInner<IFocusBrowsingListNodeIndex>((IFocusNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxBlockListInner{IxxxBrowsingBlockNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex> CreateBlockListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingBlockNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusBlockListInner<IFocusBrowsingBlockNodeIndex>((IFocusNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeState CreateRootNodeState(IReadOnlyRootNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusPlaceholderNodeState<IFocusInner<IFocusBrowsingChildIndex>>((IFocusRootNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxWriteableInsertionOptionalNodeIndex object.
        /// </summary>
        private protected override IWriteableInsertionOptionalNodeIndex CreateNewOptionalNodeIndex(Node parentNode, string propertyName, Node node)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusInsertionOptionalNodeIndex(parentNode, propertyName, node);
        }

        /// <summary>
        /// Creates a IxxxInsertNodeOperation object.
        /// </summary>
        private protected override WriteableInsertNodeOperation CreateInsertNodeOperation(Node parentNode, string propertyName, int blockIndex, int index, Node node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusInsertNodeOperation(parentNode, propertyName, blockIndex, index, node, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxInsertBlockOperation object.
        /// </summary>
        private protected override IWriteableInsertBlockOperation CreateInsertBlockOperation(Node parentNode, string propertyName, int blockIndex, IBlock block, Node node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusInsertBlockOperation(parentNode, propertyName, blockIndex, block, node, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveBlockOperation object.
        /// </summary>
        private protected override IWriteableRemoveBlockOperation CreateRemoveBlockOperation(Node parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusRemoveBlockOperation(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveBlockViewOperation object.
        /// </summary>
        private protected override WriteableRemoveBlockViewOperation CreateRemoveBlockViewOperation(Node parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusRemoveBlockViewOperation(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveNodeOperation object.
        /// </summary>
        private protected override WriteableRemoveNodeOperation CreateRemoveNodeOperation(Node parentNode, string propertyName, int blockIndex, int index, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusRemoveNodeOperation(parentNode, propertyName, blockIndex, index, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxReplaceOperation object.
        /// </summary>
        private protected override IWriteableReplaceOperation CreateReplaceOperation(Node parentNode, string propertyName, int blockIndex, int index, Node newNode, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusReplaceOperation(parentNode, propertyName, blockIndex, index, newNode, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxAssignmentOperation object.
        /// </summary>
        private protected override WriteableAssignmentOperation CreateAssignmentOperation(Node parentNode, string propertyName, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusAssignmentOperation(parentNode, propertyName, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeDiscreteValueOperation object.
        /// </summary>
        private protected override WriteableChangeDiscreteValueOperation CreateChangeDiscreteValueOperation(Node parentNode, string propertyName, int value, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusChangeDiscreteValueOperation(parentNode, propertyName, value, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeTextOperation object.
        /// </summary>
        private protected override WriteableChangeTextOperation CreateChangeTextOperation(Node parentNode, string propertyName, string text, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            return CreateChangeTextOperation(parentNode, propertyName, text, -1, -1, false, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeTextOperation object.
        /// </summary>
        private protected virtual FocusChangeTextOperation CreateChangeTextOperation(Node parentNode, string propertyName, string text, int oldCaretPosition, int newCaretPosition, bool changeCaretBeforeText, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusChangeTextOperation(parentNode, propertyName, text, oldCaretPosition, newCaretPosition, changeCaretBeforeText, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeCommentOperation object.
        /// </summary>
        private protected override WriteableChangeCommentOperation CreateChangeCommentOperation(Node parentNode, string text, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            return CreateChangeCommentOperation(parentNode, text, -1, -1, false, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeCommentOperation object.
        /// </summary>
        private protected virtual FocusChangeCommentOperation CreateChangeCommentOperation(Node parentNode, string text, int oldCaretPosition, int newCaretPosition, bool changeCaretBeforeText, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusChangeCommentOperation(parentNode, text, oldCaretPosition, newCaretPosition, changeCaretBeforeText, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeBlockOperation object.
        /// </summary>
        private protected override WriteableChangeBlockOperation CreateChangeBlockOperation(Node parentNode, string propertyName, int blockIndex, ReplicationStatus replication, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusChangeBlockOperation(parentNode, propertyName, blockIndex, replication, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxSplitBlockOperation object.
        /// </summary>
        private protected override WriteableSplitBlockOperation CreateSplitBlockOperation(Node parentNode, string propertyName, int blockIndex, int index, IBlock newBlock, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusSplitBlockOperation(parentNode, propertyName, blockIndex, index, newBlock, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMergeBlocksOperation object.
        /// </summary>
        private protected override WriteableMergeBlocksOperation CreateMergeBlocksOperation(Node parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusMergeBlocksOperation(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMoveNodeOperation object.
        /// </summary>
        private protected override WriteableMoveNodeOperation CreateMoveNodeOperation(Node parentNode, string propertyName, int blockIndex, int index, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusMoveNodeOperation(parentNode, propertyName, blockIndex, index, direction, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMoveBlockOperation object.
        /// </summary>
        private protected override WriteableMoveBlockOperation CreateMoveBlockOperation(Node parentNode, string propertyName, int blockIndex, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusMoveBlockOperation(parentNode, propertyName, blockIndex, direction, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxExpandArgumentOperation object.
        /// </summary>
        private protected override WriteableExpandArgumentOperation CreateExpandArgumentOperation(Node parentNode, string propertyName, IBlock block, Node node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusExpandArgumentOperation(parentNode, propertyName, block, node, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxGenericRefreshOperation object.
        /// </summary>
        private protected override WriteableGenericRefreshOperation CreateGenericRefreshOperation(IWriteableNodeState refreshState, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusGenericRefreshOperation((IFocusNodeState)refreshState, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxOperationGroupList object.
        /// </summary>
        private protected override WriteableOperationGroupList CreateOperationGroupStack()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusOperationGroupList();
        }

        /// <summary>
        /// Creates a IxxxOperationList object.
        /// </summary>
        private protected override WriteableOperationList CreateOperationList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusOperationList();
        }

        /// <summary>
        /// Creates a IxxxOperationGroup object.
        /// </summary>
        private protected override WriteableOperationGroup CreateOperationGroup(WriteableOperationReadOnlyList operationList, WriteableGenericRefreshOperation refresh)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusOperationGroup((FocusOperationReadOnlyList)operationList, (FocusGenericRefreshOperation)refresh);
        }

        /// <summary>
        /// Creates a IxxxReplaceWithCycleOperation object.
        /// </summary>
        private protected virtual IFocusReplaceWithCycleOperation CreateReplaceWithCycleOperation(Node parentNode, string propertyName, int blockIndex, int index, FocusInsertionChildNodeIndexList cycleIndexList, int cyclePosition, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusReplaceWithCycleOperation(parentNode, propertyName, blockIndex, index, cycleIndexList, cyclePosition, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxCycleManagerList object.
        /// </summary>
        private protected virtual FocusCycleManagerList CreateCycleManagerList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusCycleManagerList();
        }

        /// <summary>
        /// Creates a IxxxCycleManagerBody object.
        /// </summary>
        private protected virtual FocusCycleManagerBody CreateCycleManagerBody()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusCycleManagerBody();
        }

        /// <summary>
        /// Creates a IxxxCycleManagerFeature object.
        /// </summary>
        private protected virtual FocusCycleManagerFeature CreateCycleManagerFeature()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusCycleManagerFeature();
        }
        #endregion
    }
}
