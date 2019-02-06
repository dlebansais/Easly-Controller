namespace EaslyController.Writeable
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports operations to modify the tree.
    /// </summary>
    public interface IWriteableController : IReadOnlyController
    {
        /// <summary>
        /// Index of the root node.
        /// </summary>
        new IWriteableRootNodeIndex RootIndex { get; }

        /// <summary>
        /// State of the root node.
        /// </summary>
        new IWriteablePlaceholderNodeState RootState { get; }

        /// <summary>
        /// State table.
        /// </summary>
        new IWriteableIndexNodeStateReadOnlyDictionary StateTable { get; }

        /// <summary>
        /// List of operations that have been performed, and can be undone or redone.
        /// </summary>
        IWriteableOperationGroupReadOnlyList OperationStack { get; }

        /// <summary>
        /// Index of the next operation that can be redone in <see cref="OperationStack"/>.
        /// </summary>
        int RedoIndex { get; }

        /// <summary>
        /// Checks if there is an operation that can be undone.
        /// </summary>
        bool CanUndo { get; }

        /// <summary>
        /// Checks if there is an operation that can be redone.
        /// </summary>
        bool CanRedo { get; }

        /// <summary>
        /// Called when a block state is inserted.
        /// </summary>
        event Action<IWriteableInsertBlockOperation> BlockStateInserted;

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        event Action<IWriteableRemoveBlockOperation> BlockStateRemoved;

        /// <summary>
        /// Called when a block view must be removed.
        /// </summary>
        event Action<IWriteableRemoveBlockViewOperation> BlockViewRemoved;

        /// <summary>
        /// Called when a state is inserted.
        /// </summary>
        event Action<IWriteableInsertNodeOperation> StateInserted;

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        event Action<IWriteableRemoveNodeOperation> StateRemoved;

        /// <summary>
        /// Called when a state is replaced.
        /// </summary>
        event Action<IWriteableReplaceOperation> StateReplaced;

        /// <summary>
        /// Called when a state is assigned.
        /// </summary>
        event Action<IWriteableAssignmentOperation> StateAssigned;

        /// <summary>
        /// Called when a state is unassigned.
        /// </summary>
        event Action<IWriteableAssignmentOperation> StateUnassigned;

        /// <summary>
        /// Called when a state is moved.
        /// </summary>
        event Action<IWriteableChangeNodeOperation> StateChanged;

        /// <summary>
        /// Called when a state is moved.
        /// </summary>
        event Action<IWriteableChangeBlockOperation> BlockStateChanged;

        /// <summary>
        /// Called when a state is moved.
        /// </summary>
        event Action<IWriteableMoveNodeOperation> StateMoved;

        /// <summary>
        /// Called when a block state is moved.
        /// </summary>
        event Action<IWriteableMoveBlockOperation> BlockStateMoved;

        /// <summary>
        /// Called when a block is split.
        /// </summary>
        event Action<IWriteableSplitBlockOperation> BlockSplit;

        /// <summary>
        /// Called when two blocks are merged.
        /// </summary>
        event Action<IWriteableMergeBlocksOperation> BlocksMerged;

        /// <summary>
        /// Called to refresh views.
        /// </summary>
        event Action<IWriteableGenericRefreshOperation> GenericRefresh;

        /// <summary>
        /// Inserts a new node in a list or block list.
        /// </summary>
        /// <param name="inner">The inner for the list or block list where the node is inserted.</param>
        /// <param name="insertedIndex">Index for the insertion operation.</param>
        /// <param name="nodeIndex">Index of the inserted node upon return.</param>
        void Insert(IWriteableCollectionInner inner, IWriteableInsertionCollectionNodeIndex insertedIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex);

        /// <summary>
        /// Checks whether a node can be removed from a list.
        /// </summary>
        /// <param name="inner">The inner where the node is.</param>
        /// <param name="nodeIndex">Index of the node that would be removed.</param>
        bool IsRemoveable(IWriteableCollectionInner inner, IWriteableBrowsingCollectionNodeIndex nodeIndex);

        /// <summary>
        /// Removes a node from a list or block list.
        /// </summary>
        /// <param name="inner">The inner for the list or block list from which the node is removed.</param>
        /// <param name="nodeIndex">Index for the removed node.</param>
        void Remove(IWriteableCollectionInner inner, IWriteableBrowsingCollectionNodeIndex nodeIndex);

        /// <summary>
        /// Replace an existing node with a new one.
        /// </summary>
        /// <param name="inner">The inner where the node is replaced.</param>
        /// <param name="insertedIndex">Index for the replace operation.</param>
        /// <param name="nodeIndex">Index of the replacing node upon return.</param>
        void Replace(IWriteableInner inner, IWriteableInsertionChildIndex insertedIndex, out IWriteableBrowsingChildIndex nodeIndex);

        /// <summary>
        /// Assign the optional node.
        /// </summary>
        /// <param name="nodeIndex">Index of the optional node.</param>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already assigned.</param>
        void Assign(IWriteableBrowsingOptionalNodeIndex nodeIndex, out bool isChanged);

        /// <summary>
        /// Unassign the optional node.
        /// </summary>
        /// <param name="nodeIndex">Index of the optional node.</param>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already not assigned.</param>
        void Unassign(IWriteableBrowsingOptionalNodeIndex nodeIndex, out bool isChanged);

        /// <summary>
        /// Changes the replication state of a block.
        /// </summary>
        /// <param name="inner">The inner where the blok is changed.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="replication">New replication value.</param>
        void ChangeReplication(IWriteableBlockListInner inner, int blockIndex, ReplicationStatus replication);

        /// <summary>
        /// Changes the value of an enum or boolean.
        /// If the value exceeds allowed values, it is rounded to fit.
        /// </summary>
        /// <param name="nodeIndex">Index of the state with the enum to change.</param>
        /// <param name="propertyName">Name of the property to change.</param>
        /// <param name="value">The new value.</param>
        void ChangeDiscreteValue(IWriteableIndex nodeIndex, string propertyName, int value);

        /// <summary>
        /// Checks whether a block can be split at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block would be split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        bool IsSplittable(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Splits a block in two at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block is split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        void SplitBlock(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Checks whether a block can be merged at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block would be split.</param>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        bool IsMergeable(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Merges two blocks at the given index.
        /// </summary>
        /// <param name="inner">The inner where blocks are merged.</param>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        void MergeBlocks(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Checks whether a node can be moved in a list.
        /// </summary>
        /// <param name="inner">The inner where the node is.</param>
        /// <param name="nodeIndex">Index of the node that would be moved.</param>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        bool IsMoveable(IWriteableCollectionInner inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction);

        /// <summary>
        /// Moves a node around in a list or block list. In a block list, the node stays in same block.
        /// </summary>
        /// <param name="inner">The inner for the list or block list in which the node is moved.</param>
        /// <param name="nodeIndex">Index for the moved node.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        void Move(IWriteableCollectionInner inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction);

        /// <summary>
        /// Checks whether a block can be moved in a block list.
        /// </summary>
        /// <param name="inner">The inner where the block is.</param>
        /// <param name="blockIndex">Index of the block that would be moved.</param>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        bool IsBlockMoveable(IWriteableBlockListInner inner, int blockIndex, int direction);

        /// <summary>
        /// Moves a block around in a block list.
        /// </summary>
        /// <param name="inner">The inner where the block is moved.</param>
        /// <param name="blockIndex">Index of the block to move.</param>
        /// <param name="direction">The change in position, relative to the current block position.</param>
        void MoveBlock(IWriteableBlockListInner inner, int blockIndex, int direction);

        /// <summary>
        /// Expands an existing node. In the node:
        /// * All optional children are assigned if they aren't
        /// * If the node is a feature call, with no arguments, an empty argument is inserted.
        /// </summary>
        /// <param name="expandedIndex">Index of the expanded node.</param>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already expanded.</param>
        void Expand(IWriteableNodeIndex expandedIndex, out bool isChanged);

        /// <summary>
        /// Reduces an existing node. Opposite of <see cref="Expand"/>.
        /// </summary>
        /// <param name="reducedIndex">Index of the reduced node.</param>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already reduced.</param>
        void Reduce(IWriteableNodeIndex reducedIndex, out bool isChanged);

        /// <summary>
        /// Reduces all expanded nodes, and clear all unassigned optional nodes.
        /// </summary>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already canonic.</param>
        void Canonicalize(out bool isChanged);

        /// <summary>
        /// Undo the last operation.
        /// </summary>
        void Undo();

        /// <summary>
        /// Redo the last operation undone.
        /// </summary>
        void Redo();
    }

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports operations to modify the tree.
    /// </summary>
    public class WriteableController : ReadOnlyController, IWriteableController
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

        #region Properties
        /// <summary>
        /// Index of the root node.
        /// </summary>
        public new IWriteableRootNodeIndex RootIndex { get { return (IWriteableRootNodeIndex)base.RootIndex; } }

        /// <summary>
        /// State of the root node.
        /// </summary>
        public new IWriteablePlaceholderNodeState RootState { get { return (IWriteablePlaceholderNodeState)base.RootState; } }

        /// <summary>
        /// State table.
        /// </summary>
        public new IWriteableIndexNodeStateReadOnlyDictionary StateTable { get { return (IWriteableIndexNodeStateReadOnlyDictionary)base.StateTable; } }

        /// <summary>
        /// List of operations that have been performed, and can be undone or redone.
        /// </summary>
        public IWriteableOperationGroupReadOnlyList OperationStack { get; }
        private IWriteableOperationGroupList _OperationStack;

        /// <summary>
        /// Index of the next operation that can be redone in <see cref="OperationStack"/>.
        /// </summary>
        public int RedoIndex { get; private set; }

        /// <summary>
        /// Checks if there is an operation that can be undone.
        /// </summary>
        public bool CanUndo { get { return RedoIndex > 0; } }

        /// <summary>
        /// Checks if there is an operation that can be redone.
        /// </summary>
        public bool CanRedo { get { return RedoIndex < OperationStack.Count; } }

        /// <summary>
        /// Called when a block state is inserted.
        /// </summary>
        public event Action<IWriteableInsertBlockOperation> BlockStateInserted
        {
            add { AddBlockStateInsertedDelegate(value); }
            remove { RemoveBlockStateInsertedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableInsertBlockOperation> BlockStateInsertedHandler;
        private protected virtual void AddBlockStateInsertedDelegate(Action<IWriteableInsertBlockOperation> handler) { BlockStateInsertedHandler += handler; }
        private protected virtual void RemoveBlockStateInsertedDelegate(Action<IWriteableInsertBlockOperation> handler) { BlockStateInsertedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        public event Action<IWriteableRemoveBlockOperation> BlockStateRemoved
        {
            add { AddBlockStateRemovedDelegate(value); }
            remove { RemoveBlockStateRemovedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableRemoveBlockOperation> BlockStateRemovedHandler;
        private protected virtual void AddBlockStateRemovedDelegate(Action<IWriteableRemoveBlockOperation> handler) { BlockStateRemovedHandler += handler; }
        private protected virtual void RemoveBlockStateRemovedDelegate(Action<IWriteableRemoveBlockOperation> handler) { BlockStateRemovedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a block view must be removed.
        /// </summary>
        public event Action<IWriteableRemoveBlockViewOperation> BlockViewRemoved
        {
            add { AddBlockViewRemovedDelegate(value); }
            remove { RemoveBlockViewRemovedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableRemoveBlockViewOperation> BlockViewRemovedHandler;
        private protected virtual void AddBlockViewRemovedDelegate(Action<IWriteableRemoveBlockViewOperation> handler) { BlockViewRemovedHandler += handler; }
        private protected virtual void RemoveBlockViewRemovedDelegate(Action<IWriteableRemoveBlockViewOperation> handler) { BlockViewRemovedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a state is inserted.
        /// </summary>
        public event Action<IWriteableInsertNodeOperation> StateInserted
        {
            add { AddStateInsertedDelegate(value); }
            remove { RemoveStateInsertedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableInsertNodeOperation> StateInsertedHandler;
        private protected virtual void AddStateInsertedDelegate(Action<IWriteableInsertNodeOperation> handler) { StateInsertedHandler += handler; }
        private protected virtual void RemoveStateInsertedDelegate(Action<IWriteableInsertNodeOperation> handler) { StateInsertedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        public event Action<IWriteableRemoveNodeOperation> StateRemoved
        {
            add { AddStateRemovedDelegate(value); }
            remove { RemoveStateRemovedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableRemoveNodeOperation> StateRemovedHandler;
        private protected virtual void AddStateRemovedDelegate(Action<IWriteableRemoveNodeOperation> handler) { StateRemovedHandler += handler; }
        private protected virtual void RemoveStateRemovedDelegate(Action<IWriteableRemoveNodeOperation> handler) { StateRemovedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a state is replaced.
        /// </summary>
        public event Action<IWriteableReplaceOperation> StateReplaced
        {
            add { AddStateReplacedDelegate(value); }
            remove { RemoveStateReplacedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableReplaceOperation> StateReplacedHandler;
        private protected virtual void AddStateReplacedDelegate(Action<IWriteableReplaceOperation> handler) { StateReplacedHandler += handler; }
        private protected virtual void RemoveStateReplacedDelegate(Action<IWriteableReplaceOperation> handler) { StateReplacedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a state is assigned.
        /// </summary>
        public event Action<IWriteableAssignmentOperation> StateAssigned
        {
            add { AddStateAssignedDelegate(value); }
            remove { RemoveStateAssignedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableAssignmentOperation> StateAssignedHandler;
        private protected virtual void AddStateAssignedDelegate(Action<IWriteableAssignmentOperation> handler) { StateAssignedHandler += handler; }
        private protected virtual void RemoveStateAssignedDelegate(Action<IWriteableAssignmentOperation> handler) { StateAssignedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a state is unassigned.
        /// </summary>
        public event Action<IWriteableAssignmentOperation> StateUnassigned
        {
            add { AddStateUnassignedDelegate(value); }
            remove { RemoveStateUnassignedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableAssignmentOperation> StateUnassignedHandler;
        private protected virtual void AddStateUnassignedDelegate(Action<IWriteableAssignmentOperation> handler) { StateUnassignedHandler += handler; }
        private protected virtual void RemoveStateUnassignedDelegate(Action<IWriteableAssignmentOperation> handler) { StateUnassignedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a state is changed.
        /// </summary>
        public event Action<IWriteableChangeNodeOperation> StateChanged
        {
            add { AddStateChangedDelegate(value); }
            remove { RemoveStateChangedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableChangeNodeOperation> StateChangedHandler;
        private protected virtual void AddStateChangedDelegate(Action<IWriteableChangeNodeOperation> handler) { StateChangedHandler += handler; }
        private protected virtual void RemoveStateChangedDelegate(Action<IWriteableChangeNodeOperation> handler) { StateChangedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a state is changed.
        /// </summary>
        public event Action<IWriteableChangeBlockOperation> BlockStateChanged
        {
            add { AddBlockStateChangedDelegate(value); }
            remove { RemoveBlockStateChangedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableChangeBlockOperation> BlockStateChangedHandler;
        private protected virtual void AddBlockStateChangedDelegate(Action<IWriteableChangeBlockOperation> handler) { BlockStateChangedHandler += handler; }
        private protected virtual void RemoveBlockStateChangedDelegate(Action<IWriteableChangeBlockOperation> handler) { BlockStateChangedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a state is moved.
        /// </summary>
        public event Action<IWriteableMoveNodeOperation> StateMoved
        {
            add { AddStateMovedDelegate(value); }
            remove { RemoveStateMovedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableMoveNodeOperation> StateMovedHandler;
        private protected virtual void AddStateMovedDelegate(Action<IWriteableMoveNodeOperation> handler) { StateMovedHandler += handler; }
        private protected virtual void RemoveStateMovedDelegate(Action<IWriteableMoveNodeOperation> handler) { StateMovedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a block state is moved.
        /// </summary>
        public event Action<IWriteableMoveBlockOperation> BlockStateMoved
        {
            add { AddBlockStateMovedDelegate(value); }
            remove { RemoveBlockStateMovedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableMoveBlockOperation> BlockStateMovedHandler;
        private protected virtual void AddBlockStateMovedDelegate(Action<IWriteableMoveBlockOperation> handler) { BlockStateMovedHandler += handler; }
        private protected virtual void RemoveBlockStateMovedDelegate(Action<IWriteableMoveBlockOperation> handler) { BlockStateMovedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a block is split.
        /// </summary>
        public event Action<IWriteableSplitBlockOperation> BlockSplit
        {
            add { AddBlockSplitDelegate(value); }
            remove { RemoveBlockSplitDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableSplitBlockOperation> BlockSplitHandler;
        private protected virtual void AddBlockSplitDelegate(Action<IWriteableSplitBlockOperation> handler) { BlockSplitHandler += handler; }
        private protected virtual void RemoveBlockSplitDelegate(Action<IWriteableSplitBlockOperation> handler) { BlockSplitHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when two blocks are merged.
        /// </summary>
        public event Action<IWriteableMergeBlocksOperation> BlocksMerged
        {
            add { AddBlocksMergedDelegate(value); }
            remove { RemoveBlocksMergedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableMergeBlocksOperation> BlocksMergedHandler;
        private protected virtual void AddBlocksMergedDelegate(Action<IWriteableMergeBlocksOperation> handler) { BlocksMergedHandler += handler; }
        private protected virtual void RemoveBlocksMergedDelegate(Action<IWriteableMergeBlocksOperation> handler) { BlocksMergedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called to refresh views.
        /// </summary>
        public event Action<IWriteableGenericRefreshOperation> GenericRefresh
        {
            add { AddGenericRefreshDelegate(value); }
            remove { RemoveGenericRefreshDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableGenericRefreshOperation> GenericRefreshHandler;
        private protected virtual void AddGenericRefreshDelegate(Action<IWriteableGenericRefreshOperation> handler) { GenericRefreshHandler += handler; }
        private protected virtual void RemoveGenericRefreshDelegate(Action<IWriteableGenericRefreshOperation> handler) { GenericRefreshHandler -= handler; }
#pragma warning restore 1591
        #endregion

        #region Client Interface
        /// <summary>
        /// Inserts a new node in a list or block list.
        /// </summary>
        /// <param name="inner">The inner for the list or block list where the node is inserted.</param>
        /// <param name="insertedIndex">Index for the insertion operation.</param>
        /// <param name="nodeIndex">Index of the inserted node upon return.</param>
        public virtual void Insert(IWriteableCollectionInner inner, IWriteableInsertionCollectionNodeIndex insertedIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(insertedIndex != null);
            IWriteableNodeState Owner = inner.Owner;
            IWriteableIndex ParentIndex = Owner.ParentIndex;
            Debug.Assert(Contains(ParentIndex));
            Debug.Assert(IndexToState(ParentIndex) == Owner);
            IWriteableInnerReadOnlyDictionary<string> InnerTable = Owner.InnerTable;
            Debug.Assert(InnerTable.ContainsKey(inner.PropertyName));
            Debug.Assert(InnerTable[inner.PropertyName] == inner);

            bool IsHandled = false;
            nodeIndex = null;

            if (inner is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner && insertedIndex is IWriteableInsertionNewBlockNodeIndex AsNewBlockIndex)
            {
                InsertNewBlock(AsBlockListInner, AsNewBlockIndex, out nodeIndex);
                IsHandled = true;
            }
            else if (inner is IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> AsCollectionInner && insertedIndex is IWriteableInsertionCollectionNodeIndex AsCollectionIndex)
            {
                InsertNewNode(AsCollectionInner, AsCollectionIndex, out nodeIndex);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);
        }

        /// <summary></summary>
        private protected virtual void InsertNewBlock(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> blockListInner, IWriteableInsertionNewBlockNodeIndex newBlockIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            IBlock NewBlock = NodeTreeHelperBlockList.CreateBlock(blockListInner.Owner.Node, blockListInner.PropertyName, ReplicationStatus.Normal, newBlockIndex.PatternNode, newBlockIndex.SourceNode);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoInsertNewBlock(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoInsertNewBlock(operation);
            IWriteableInsertBlockOperation Operation = CreateInsertBlockOperation(blockListInner.Owner.Node, blockListInner.PropertyName, newBlockIndex.BlockIndex, NewBlock, newBlockIndex.Node, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();

            nodeIndex = Operation.BrowsingIndex;
        }

        /// <summary></summary>
        private protected virtual void RedoInsertNewBlock(IWriteableOperation operation)
        {
            IWriteableInsertBlockOperation InsertBlockOperation = (IWriteableInsertBlockOperation)operation;
            ExecuteInsertNewBlock(InsertBlockOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteInsertNewBlock(IWriteableInsertBlockOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;

            Inner.InsertNewBlock(operation);

            IWriteableBrowsingExistingBlockNodeIndex BrowsingIndex = operation.BrowsingIndex;
            IWriteableBlockState BlockState = operation.BlockState;
            IWriteablePlaceholderNodeState ChildState = operation.ChildState;

            Debug.Assert(BlockState.StateList.Count == 1);
            Debug.Assert(BlockState.StateList[0] == ChildState);
            ((IWriteableBlockState<IWriteableInner<IWriteableBrowsingChildIndex>>)BlockState).InitBlockState();
            Stats.BlockCount++;

            IWriteableBrowsingPatternIndex PatternIndex = BlockState.PatternIndex;
            IWriteablePatternState PatternState = BlockState.PatternState;
            AddState(PatternIndex, PatternState);
            Stats.PlaceholderNodeCount++;

            IWriteableBrowsingSourceIndex SourceIndex = BlockState.SourceIndex;
            IWriteableSourceState SourceState = BlockState.SourceState;
            AddState(SourceIndex, SourceState);
            Stats.PlaceholderNodeCount++;

            AddState(BrowsingIndex, ChildState);
            Stats.PlaceholderNodeCount++;
            BuildStateTable(Inner, null, BrowsingIndex, ChildState);

            Debug.Assert(Contains(BrowsingIndex));

            NotifyBlockStateInserted(operation);
        }

        /// <summary></summary>
        private protected virtual void UndoInsertNewBlock(IWriteableOperation operation)
        {
            IWriteableInsertBlockOperation InsertBlockOperation = (IWriteableInsertBlockOperation)operation;
            IWriteableRemoveBlockOperation RemoveBlockOperation = InsertBlockOperation.ToRemoveBlockOperation();

            ExecuteRemoveBlock(RemoveBlockOperation);
        }

        /// <summary></summary>
        private protected virtual void InsertNewNode(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableInsertionCollectionNodeIndex insertedIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            int BlockIndex = -1;
            int Index = -1;
            INode Node = null;

            bool IsHandled = false;

            switch (insertedIndex)
            {
                case IWriteableInsertionListNodeIndex AsListNodeIndex:
                    Index = AsListNodeIndex.Index;
                    Node = AsListNodeIndex.Node;
                    IsHandled = true;
                    break;

                case IWriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex:
                    BlockIndex = AsExistingBlockNodeIndex.BlockIndex;
                    Index = AsExistingBlockNodeIndex.Index;
                    Node = AsExistingBlockNodeIndex.Node;
                    IsHandled = true;
                    break;
            }

            Debug.Assert(IsHandled);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoInsertNewNode(operation);
            IWriteableInsertNodeOperation Operation = CreateInsertNodeOperation(inner.Owner.Node, inner.PropertyName, BlockIndex, Index, Node, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();

            nodeIndex = Operation.BrowsingIndex;
        }

        /// <summary></summary>
        private protected virtual void RedoInsertNewNode(IWriteableOperation operation)
        {
            IWriteableInsertNodeOperation InsertNodeOperation = (IWriteableInsertNodeOperation)operation;
            ExecuteInsertNewNode(InsertNodeOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteInsertNewNode(IWriteableInsertNodeOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex>;

            Inner.Insert(operation);

            IWriteableBrowsingCollectionNodeIndex BrowsingIndex = operation.BrowsingIndex;
            IWriteablePlaceholderNodeState ChildState = operation.ChildState;

            AddState(BrowsingIndex, ChildState);
            Stats.PlaceholderNodeCount++;
            BuildStateTable(Inner, null, BrowsingIndex, ChildState);

            Debug.Assert(Contains(BrowsingIndex));

            NotifyStateInserted(operation);
        }

        /// <summary></summary>
        private protected virtual void UndoInsertNewNode(IWriteableOperation operation)
        {
            IWriteableInsertNodeOperation InsertNodeOperation = (IWriteableInsertNodeOperation)operation;
            IWriteableRemoveNodeOperation RemoveNodeOperation = InsertNodeOperation.ToRemoveNodeOperation();

            ExecuteRemoveNode(RemoveNodeOperation);
        }

        /// <summary>
        /// Checks whether a node can be removed from a list.
        /// </summary>
        /// <param name="inner">The inner where the node is.</param>
        /// <param name="nodeIndex">Index of the node that would be removed.</param>
        public bool IsRemoveable(IWriteableCollectionInner inner, IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            if (inner.Count > 1)
                return true;

            Debug.Assert(inner.Count == 1);
            Debug.Assert(inner.Owner != null);

            INode Node = inner.Owner.Node;
            string PropertyName = inner.PropertyName;
            Debug.Assert(Node != null);

            bool Result = true;

            Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(inner.Owner.Node.GetType());
            IReadOnlyDictionary<Type, string[]> NeverEmptyCollectionTable = NodeHelper.NeverEmptyCollectionTable;
            if (NeverEmptyCollectionTable.ContainsKey(InterfaceType))
            {
                foreach (string Item in NeverEmptyCollectionTable[InterfaceType])
                    if (Item == PropertyName)
                        Result = false;
            }

            return Result;
        }

        /// <summary>
        /// Removes a node from a list or block list.
        /// </summary>
        /// <param name="inner">The inner for the list or block list from which the node is removed.</param>
        /// <param name="nodeIndex">Index for the removed node.</param>
        public virtual void Remove(IWriteableCollectionInner inner, IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);
            IWriteableNodeState Owner = inner.Owner;
            IWriteableIndex ParentIndex = Owner.ParentIndex;
            Debug.Assert(Contains(ParentIndex));
            Debug.Assert(IndexToState(ParentIndex) == Owner);
            IWriteableInnerReadOnlyDictionary<string> InnerTable = Owner.InnerTable;
            Debug.Assert(InnerTable.ContainsKey(inner.PropertyName));
            Debug.Assert(InnerTable[inner.PropertyName] == inner);

            int BlockIndex = -1;
            int Index = -1;
            INode Node = null;
            bool IsHandled = false;

            switch (nodeIndex)
            {
                case IWriteableBrowsingListNodeIndex AsListNodeIndex:
                    Index = AsListNodeIndex.Index;
                    Node = AsListNodeIndex.Node;
                    IsHandled = true;
                    break;

                case IWriteableBrowsingExistingBlockNodeIndex AsExistingBlockNodeIndex:
                    BlockIndex = AsExistingBlockNodeIndex.BlockIndex;
                    Index = AsExistingBlockNodeIndex.Index;
                    Node = AsExistingBlockNodeIndex.Node;
                    IsHandled = true;
                    break;
            }

            Debug.Assert(IsHandled);

            IsHandled = false;

            if (inner is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner && nodeIndex is IWriteableBrowsingExistingBlockNodeIndex ExistingBlockIndex)
            {
                if (AsBlockListInner.BlockStateList[ExistingBlockIndex.BlockIndex].StateList.Count == 1)
                    RemoveBlock(AsBlockListInner, BlockIndex);
                else
                    RemoveNode(AsBlockListInner, BlockIndex, Index);

                IsHandled = true;
            }
            else if (inner is IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> AsCollectionInner)
            {
                RemoveNode(AsCollectionInner, BlockIndex, Index);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);
        }

        /// <summary></summary>
        private protected virtual void RemoveBlock(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> blockListInner, int blockIndex)
        {
            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRemoveBlock(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoRemoveBlock(operation);
            IWriteableRemoveBlockOperation Operation = CreateRemoveBlockOperation(blockListInner.Owner.Node, blockListInner.PropertyName, blockIndex, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        /// <summary></summary>
        private protected virtual void RedoRemoveBlock(IWriteableOperation operation)
        {
            IWriteableRemoveBlockOperation RemoveBlockOperation = (IWriteableRemoveBlockOperation)operation;
            ExecuteRemoveBlock(RemoveBlockOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteRemoveBlock(IWriteableRemoveBlockOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;

            Inner.RemoveWithBlock(operation);

            IWriteableBlockState RemovedBlockState = operation.BlockState;
            Debug.Assert(RemovedBlockState != null);

            IWriteableBrowsingPatternIndex PatternIndex = RemovedBlockState.PatternIndex;
            IWriteableBrowsingSourceIndex SourceIndex = RemovedBlockState.SourceIndex;

            Debug.Assert(PatternIndex != null);
            Debug.Assert(StateTable.ContainsKey(PatternIndex));
            Debug.Assert(SourceIndex != null);
            Debug.Assert(StateTable.ContainsKey(SourceIndex));

            Stats.BlockCount--;

            RemoveState(PatternIndex);
            Stats.PlaceholderNodeCount--;

            RemoveState(SourceIndex);
            Stats.PlaceholderNodeCount--;

            IWriteableNodeState RemovedState = operation.RemovedState;
            Debug.Assert(RemovedState != null);

            PruneState(RemovedState);
            Stats.PlaceholderNodeCount--;

            NotifyBlockStateRemoved(operation);
        }

        /// <summary></summary>
        private protected virtual void UndoRemoveBlock(IWriteableOperation operation)
        {
            IWriteableRemoveBlockOperation RemoveBlockOperation = (IWriteableRemoveBlockOperation)operation;
            IWriteableInsertBlockOperation InsertBlockOperation = RemoveBlockOperation.ToInsertBlockOperation();

            ExecuteInsertNewBlock(InsertBlockOperation);
        }

        /// <summary></summary>
        private protected virtual void RemoveNode(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, int blockIndex, int index)
        {
            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRemoveNode(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoRemoveNode(operation);
            IWriteableRemoveNodeOperation Operation = CreateRemoveNodeOperation(inner.Owner.Node, inner.PropertyName, blockIndex, index, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        /// <summary></summary>
        private protected virtual void RedoRemoveNode(IWriteableOperation operation)
        {
            IWriteableRemoveNodeOperation RemoveNodeOperation = (IWriteableRemoveNodeOperation)operation;
            ExecuteRemoveNode(RemoveNodeOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteRemoveNode(IWriteableRemoveNodeOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex>;

            Inner.Remove(operation);

            IWriteableNodeState RemovedState = operation.RemovedState;
            PruneState(RemovedState);
            Stats.PlaceholderNodeCount--;

            NotifyStateRemoved(operation);
        }

        /// <summary></summary>
        private protected virtual void UndoRemoveNode(IWriteableOperation operation)
        {
            IWriteableRemoveNodeOperation RemoveNodeOperation = (IWriteableRemoveNodeOperation)operation;
            IWriteableInsertNodeOperation InsertNodeOperation = RemoveNodeOperation.ToInsertNodeOperation();

            ExecuteInsertNewNode(InsertNodeOperation);
        }

        /// <summary>
        /// Replace an existing node with a new one.
        /// </summary>
        /// <param name="inner">The inner where the node is replaced.</param>
        /// <param name="replacementIndex">Index for the replace operation.</param>
        /// <param name="nodeIndex">Index of the replacing node upon return.</param>
        public void Replace(IWriteableInner inner, IWriteableInsertionChildIndex replacementIndex, out IWriteableBrowsingChildIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(replacementIndex != null);
            IWriteableNodeState Owner = inner.Owner;
            IWriteableIndex ParentIndex = Owner.ParentIndex;
            Debug.Assert(Contains(ParentIndex));
            Debug.Assert(IndexToState(ParentIndex) == Owner);
            IWriteableInnerReadOnlyDictionary<string> InnerTable = Owner.InnerTable;
            Debug.Assert(InnerTable.ContainsKey(inner.PropertyName));
            Debug.Assert(InnerTable[inner.PropertyName] == inner);

            int BlockIndex = -1;
            int Index = -1;
            INode NewNode = null;
            bool IsHandled = false;

            switch (replacementIndex)
            {
                case IWriteableInsertionPlaceholderNodeIndex AsPlaceholderNodeIndex:
                    NewNode = AsPlaceholderNodeIndex.Node;
                    IsHandled = true;
                    break;

                case IWriteableInsertionOptionalNodeIndex AsOptionalNodeIndex:
                    NewNode = AsOptionalNodeIndex.Node;
                    IsHandled = true;
                    break;

                case IWriteableInsertionOptionalClearIndex AsOptionalClearIndex:
                    IsHandled = true;
                    break;

                case IWriteableInsertionListNodeIndex AsListNodeIndex:
                    Index = AsListNodeIndex.Index;
                    NewNode = AsListNodeIndex.Node;
                    IsHandled = true;
                    break;

                case IWriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex:
                    BlockIndex = AsExistingBlockNodeIndex.BlockIndex;
                    Index = AsExistingBlockNodeIndex.Index;
                    NewNode = AsExistingBlockNodeIndex.Node;
                    IsHandled = true;
                    break;
            }

            Debug.Assert(IsHandled);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoReplace(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoReplace(operation);
            IWriteableReplaceOperation Operation = CreateReplaceOperation(inner.Owner.Node, inner.PropertyName, BlockIndex, Index, NewNode, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();

            nodeIndex = Operation.NewBrowsingIndex;
        }

        /// <summary></summary>
        private protected virtual void RedoReplace(IWriteableOperation operation)
        {
            IWriteableReplaceOperation ReplaceOperation = (IWriteableReplaceOperation)operation;
            ExecuteReplace(ReplaceOperation);
        }

        /// <summary></summary>
        private protected virtual void UndoReplace(IWriteableOperation operation)
        {
            IWriteableReplaceOperation ReplaceOperation = (IWriteableReplaceOperation)operation;
            ReplaceOperation = ReplaceOperation.ToInverseReplace();

            ExecuteReplace(ReplaceOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteReplace(IWriteableReplaceOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableInner<IWriteableBrowsingChildIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableInner<IWriteableBrowsingChildIndex>;

            ReplaceState(operation, Inner);
            Debug.Assert(Contains(operation.NewBrowsingIndex));

            NotifyStateReplaced(operation);
        }

        /// <summary></summary>
        private protected virtual void ReplaceState(IWriteableReplaceOperation operation, IWriteableInner<IWriteableBrowsingChildIndex> inner)
        {
            Debug.Assert(inner != null);
            IWriteableNodeState Owner = inner.Owner;
            IWriteableIndex ParentIndex = Owner.ParentIndex;
            Debug.Assert(Contains(ParentIndex));
            Debug.Assert(IndexToState(ParentIndex) == Owner);
            IWriteableInnerReadOnlyDictionary<string> InnerTable = Owner.InnerTable;
            Debug.Assert(InnerTable.ContainsKey(inner.PropertyName));
            Debug.Assert(InnerTable[inner.PropertyName] == inner);

            IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> AsOptionalInner = inner as IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex>;
            if (AsOptionalInner != null)
            {
                IWriteableNodeState OldState = AsOptionalInner.ChildState;
                PruneStateChildren(OldState);

                if (AsOptionalInner.IsAssigned)
                    Stats.AssignedOptionalNodeCount--;
            }

            inner.Replace(operation);

            IWriteableBrowsingChildIndex OldBrowsingIndex = operation.OldBrowsingIndex;
            IWriteableBrowsingChildIndex NewBrowsingIndex = operation.NewBrowsingIndex;
            IWriteableNodeState ChildState = operation.NewChildState;

            if (AsOptionalInner != null)
            {
                if (AsOptionalInner.IsAssigned)
                    Stats.AssignedOptionalNodeCount++;
            }
            else
            {
                Debug.Assert(Contains(OldBrowsingIndex));
                IWriteableNodeState OldState = StateTable[OldBrowsingIndex];

                PruneStateChildren(OldState);
            }

            RemoveState(OldBrowsingIndex);
            AddState(NewBrowsingIndex, ChildState);

            BuildStateTable(inner, null, NewBrowsingIndex, ChildState);
        }

        /// <summary>
        /// Assign the optional node.
        /// </summary>
        /// <param name="nodeIndex">Index of the optional node.</param>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already assigned.</param>
        public virtual void Assign(IWriteableBrowsingOptionalNodeIndex nodeIndex, out bool isChanged)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(StateTable.ContainsKey(nodeIndex));

            IWriteableOptionalNodeState State = StateTable[nodeIndex] as IWriteableOptionalNodeState;
            Debug.Assert(State != null);

            IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> Inner = State.ParentInner as IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex>;
            Debug.Assert(Inner != null);

            if (!Inner.IsAssigned)
            {
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoAssign(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoAssign(operation);
                IWriteableAssignmentOperation Operation = CreateAssignmentOperation(Inner.Owner.Node, Inner.PropertyName, HandlerRedo, HandlerUndo, isNested: false);

                Operation.Redo();
                SetLastOperation(Operation);
                CheckInvariant();

                isChanged = true;
            }
            else
                isChanged = false;
        }

        /// <summary></summary>
        private protected virtual void RedoAssign(IWriteableOperation operation)
        {
            IWriteableAssignmentOperation AssignmentOperation = (IWriteableAssignmentOperation)operation;
            ExecuteAssign(AssignmentOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteAssign(IWriteableAssignmentOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex>;

            Inner.Assign(operation);

            Stats.AssignedOptionalNodeCount++;

            NotifyStateAssigned(operation);
        }

        /// <summary></summary>
        private protected virtual void UndoAssign(IWriteableOperation operation)
        {
            IWriteableAssignmentOperation AssignmentOperation = (IWriteableAssignmentOperation)operation;
            AssignmentOperation = AssignmentOperation.ToInverseAssignment();

            ExecuteUnassign(AssignmentOperation);
        }

        /// <summary>
        /// Unassign the optional node.
        /// </summary>
        /// <param name="nodeIndex">Index of the optional node.</param>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already not assigned.</param>
        public virtual void Unassign(IWriteableBrowsingOptionalNodeIndex nodeIndex, out bool isChanged)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(StateTable.ContainsKey(nodeIndex));

            IWriteableOptionalNodeState State = StateTable[nodeIndex] as IWriteableOptionalNodeState;
            Debug.Assert(State != null);

            IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> Inner = State.ParentInner as IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex>;
            Debug.Assert(Inner != null);

            if (Inner.IsAssigned)
            {
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoUnassign(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoUnassign(operation);
                IWriteableAssignmentOperation Operation = CreateAssignmentOperation(Inner.Owner.Node, Inner.PropertyName, HandlerRedo, HandlerUndo, isNested: false);

                Operation.Redo();
                SetLastOperation(Operation);
                CheckInvariant();

                isChanged = true;
            }
            else
                isChanged = false;
        }

        /// <summary></summary>
        private protected virtual void RedoUnassign(IWriteableOperation operation)
        {
            IWriteableAssignmentOperation AssignmentOperation = (IWriteableAssignmentOperation)operation;
            ExecuteUnassign(AssignmentOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteUnassign(IWriteableAssignmentOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex>;

            Inner.Unassign(operation);

            Stats.AssignedOptionalNodeCount--;

            NotifyStateUnassigned(operation);
        }

        /// <summary></summary>
        private protected virtual void UndoUnassign(IWriteableOperation operation)
        {
            IWriteableAssignmentOperation AssignmentOperation = (IWriteableAssignmentOperation)operation;
            AssignmentOperation = AssignmentOperation.ToInverseAssignment();

            ExecuteAssign(AssignmentOperation);
        }

        /// <summary>
        /// Changes the replication state of a block.
        /// </summary>
        /// <param name="inner">The inner where the blok is changed.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="replication">New replication value.</param>
        public virtual void ChangeReplication(IWriteableBlockListInner inner, int blockIndex, ReplicationStatus replication)
        {
            Debug.Assert(inner != null);
            Debug.Assert(blockIndex >= 0 && blockIndex < inner.BlockStateList.Count);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoChangeReplication(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoChangeReplication(operation);
            IWriteableChangeBlockOperation Operation = CreateChangeBlockOperation(inner.Owner.Node, inner.PropertyName, blockIndex, replication, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        /// <summary></summary>
        private protected virtual void RedoChangeReplication(IWriteableOperation operation)
        {
            IWriteableChangeBlockOperation ChangeBlockOperation = (IWriteableChangeBlockOperation)operation;
            ExecuteChangeReplication(ChangeBlockOperation);
        }

        /// <summary></summary>
        private protected virtual void UndoChangeReplication(IWriteableOperation operation)
        {
            IWriteableChangeBlockOperation ChangeBlockOperation = (IWriteableChangeBlockOperation)operation;
            ChangeBlockOperation = ChangeBlockOperation.ToInverseChange();

            ExecuteChangeReplication(ChangeBlockOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteChangeReplication(IWriteableChangeBlockOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;

            Inner.ChangeReplication(operation);

            NotifyBlockStateChanged(operation);
        }

        /// <summary>
        /// Changes the value of an enum or boolean.
        /// If the value exceeds allowed values, it is rounded to fit.
        /// </summary>
        /// <param name="nodeIndex">Index of the state with the enum to change.</param>
        /// <param name="propertyName">Name of the property to change.</param>
        /// <param name="value">The new value.</param>
        public virtual void ChangeDiscreteValue(IWriteableIndex nodeIndex, string propertyName, int value)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(StateTable.ContainsKey(nodeIndex));
            Debug.Assert(value >= 0);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoChangeDiscreteValue(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoChangeDiscreteValue(operation);
            IWriteableNodeState State = StateTable[nodeIndex];
            IWriteableChangeNodeOperation Operation = CreateChangeNodeOperation(State.Node, propertyName, value, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        /// <summary></summary>
        private protected virtual void RedoChangeDiscreteValue(IWriteableOperation operation)
        {
            IWriteableChangeNodeOperation ChangeNodeOperation = (IWriteableChangeNodeOperation)operation;
            ExecuteChangeDiscreteValue(ChangeNodeOperation);
        }

        /// <summary></summary>
        private protected virtual void UndoChangeDiscreteValue(IWriteableOperation operation)
        {
            IWriteableChangeNodeOperation ChangeNodeOperation = (IWriteableChangeNodeOperation)operation;
            ChangeNodeOperation = ChangeNodeOperation.ToInverseChange();

            ExecuteChangeDiscreteValue(ChangeNodeOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteChangeDiscreteValue(IWriteableChangeNodeOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            int NewValue = operation.NewValue;

            IWriteableNodeState State = (IWriteableNodeState)GetState(ParentNode);
            Debug.Assert(State != null);
            Debug.Assert(State.ValuePropertyTypeTable.ContainsKey(PropertyName));
            Debug.Assert(State.ValuePropertyTypeTable[PropertyName] == Constants.ValuePropertyType.Boolean || State.ValuePropertyTypeTable[PropertyName] == Constants.ValuePropertyType.Enum);

            int OldValue = NodeTreeHelper.GetEnumValue(State.Node, PropertyName);

            NodeTreeHelper.GetEnumRange(State.Node.GetType(), PropertyName, out int Min, out int Max);
            Debug.Assert(NewValue >= Min && NewValue <= Max);

            NodeTreeHelper.SetEnumValue(State.Node, PropertyName, NewValue);

            operation.Update(State, OldValue);

            NotifyStateChanged(operation);
        }

        /// <summary>
        /// Checks whether a block can be split at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block would be split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        public virtual bool IsSplittable(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            return inner.IsSplittable(nodeIndex);
        }

        /// <summary>
        /// Splits a block in two at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block is split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        public virtual void SplitBlock(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);
            Debug.Assert(inner.IsSplittable(nodeIndex));

            IWriteableBlockState BlockState = inner.BlockStateList[nodeIndex.BlockIndex];
            ReplicationStatus Replication = BlockState.ChildBlock.Replication;
            IPattern NewPatternNode = NodeHelper.CreateSimplePattern(BlockState.ChildBlock.ReplicationPattern.Text);
            IIdentifier NewSourceNode = NodeHelper.CreateSimpleIdentifier(BlockState.ChildBlock.SourceIdentifier.Text);
            IBlock NewBlock = NodeTreeHelperBlockList.CreateBlock(inner.Owner.Node, inner.PropertyName, Replication, NewPatternNode, NewSourceNode);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoSplitBlock(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoSplitBlock(operation);
            IWriteableSplitBlockOperation Operation = CreateSplitBlockOperation(inner.Owner.Node, inner.PropertyName, nodeIndex.BlockIndex, nodeIndex.Index, NewBlock, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        /// <summary></summary>
        private protected virtual void RedoSplitBlock(IWriteableOperation operation)
        {
            IWriteableSplitBlockOperation SplitBlockOperation = (IWriteableSplitBlockOperation)operation;
            ExecuteSplitBlock(SplitBlockOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteSplitBlock(IWriteableSplitBlockOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;

            IWriteableBlockState OldBlockState = Inner.BlockStateList[operation.BlockIndex];
            Debug.Assert(operation.Index < OldBlockState.StateList.Count);

            int OldNodeCount = OldBlockState.StateList.Count;

            Inner.SplitBlock(operation);
            Stats.BlockCount++;

            IWriteableBlockState NewBlockState = operation.BlockState;

            Debug.Assert(OldBlockState.StateList.Count + NewBlockState.StateList.Count == OldNodeCount);
            Debug.Assert(NewBlockState.StateList.Count > 0);

            IReadOnlyBrowsingPatternIndex PatternIndex = NewBlockState.PatternIndex;
            IReadOnlyPatternState PatternState = NewBlockState.PatternState;
            AddState(PatternIndex, PatternState);
            Stats.PlaceholderNodeCount++;

            IReadOnlyBrowsingSourceIndex SourceIndex = NewBlockState.SourceIndex;
            IReadOnlySourceState SourceState = NewBlockState.SourceState;
            AddState(SourceIndex, SourceState);
            Stats.PlaceholderNodeCount++;

            NotifyBlockSplit(operation);
        }

        /// <summary></summary>
        private protected virtual void UndoSplitBlock(IWriteableOperation operation)
        {
            IWriteableSplitBlockOperation SplitBlockOperation = (IWriteableSplitBlockOperation)operation;
            IWriteableMergeBlocksOperation MergeBlocksOperation = SplitBlockOperation.ToMergeBlocksOperation();

            ExecuteMergeBlocks(MergeBlocksOperation);
        }

        /// <summary>
        /// Checks whether a block can be merged at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block would be split.</param>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        public virtual bool IsMergeable(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            return inner.IsMergeable(nodeIndex);
        }

        /// <summary>
        /// Merges two blocks at the given index.
        /// </summary>
        /// <param name="inner">The inner where blocks are merged.</param>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        public virtual void MergeBlocks(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);
            Debug.Assert(inner.IsMergeable(nodeIndex));

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoMergeBlocks(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoMergeBlocks(operation);
            IWriteableMergeBlocksOperation Operation = CreateMergeBlocksOperation(inner.Owner.Node, inner.PropertyName, nodeIndex.BlockIndex, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        /// <summary></summary>
        private protected virtual void RedoMergeBlocks(IWriteableOperation operation)
        {
            IWriteableMergeBlocksOperation MergeBlocksOperation = (IWriteableMergeBlocksOperation)operation;
            ExecuteMergeBlocks(MergeBlocksOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteMergeBlocks(IWriteableMergeBlocksOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;

            int BlockIndex = operation.BlockIndex;
            IWriteableBlockState FirstBlockState = Inner.BlockStateList[BlockIndex - 1];
            IWriteableBlockState SecondBlockState = Inner.BlockStateList[BlockIndex];

            IReadOnlyBrowsingSourceIndex SourceIndex = FirstBlockState.SourceIndex;
            RemoveState(SourceIndex);
            Stats.PlaceholderNodeCount--;

            IReadOnlyBrowsingPatternIndex PatternIndex = FirstBlockState.PatternIndex;
            RemoveState(PatternIndex);
            Stats.PlaceholderNodeCount--;

            int OldNodeCount = FirstBlockState.StateList.Count + SecondBlockState.StateList.Count;
            int FirstNodeIndex = FirstBlockState.StateList.Count;

            Inner.MergeBlocks(operation);
            Stats.BlockCount--;

            IWriteableBlockState BlockState = Inner.BlockStateList[BlockIndex - 1];

            Debug.Assert(BlockState.StateList.Count == OldNodeCount);
            Debug.Assert(FirstNodeIndex < BlockState.StateList.Count);

            NotifyBlocksMerged(operation);
        }

        /// <summary></summary>
        private protected virtual void UndoMergeBlocks(IWriteableOperation operation)
        {
            IWriteableMergeBlocksOperation MergeBlocksOperation = (IWriteableMergeBlocksOperation)operation;
            IWriteableSplitBlockOperation SplitBlockOperation = MergeBlocksOperation.ToSplitBlockOperation();

            ExecuteSplitBlock(SplitBlockOperation);
        }

        /// <summary>
        /// Checks whether a node can be moved in a list.
        /// </summary>
        /// <param name="inner">The inner where the node is.</param>
        /// <param name="nodeIndex">Index of the node that would be moved.</param>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        public virtual bool IsMoveable(IWriteableCollectionInner inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IWriteableNodeState State = StateTable[nodeIndex];
            Debug.Assert(State != null);

            bool Result = inner.IsMoveable(nodeIndex, direction);

            return Result;
        }

        /// <summary>
        /// Moves a node around in a list or block list. In a block list, the node stays in same block.
        /// </summary>
        /// <param name="inner">The inner for the list or block list in which the node is moved.</param>
        /// <param name="nodeIndex">Index for the moved node.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        public virtual void Move(IWriteableCollectionInner inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IWriteableNodeState State = StateTable[nodeIndex];
            Debug.Assert(State != null);

            int BlockIndex = -1;
            int Index = -1;
            bool IsHandled = false;

            switch (nodeIndex)
            {
                case IWriteableBrowsingListNodeIndex AsListNodeIndex:
                    Index = AsListNodeIndex.Index;
                    IsHandled = true;
                    break;

                case IWriteableBrowsingExistingBlockNodeIndex AsExistingBlockNodeIndex:
                    BlockIndex = AsExistingBlockNodeIndex.BlockIndex;
                    Index = AsExistingBlockNodeIndex.Index;
                    IsHandled = true;
                    break;
            }

            Debug.Assert(IsHandled);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoMove(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoMove(operation);
            IWriteableMoveNodeOperation Operation = CreateMoveNodeOperation(inner.Owner.Node, inner.PropertyName, BlockIndex, Index, direction, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        /// <summary></summary>
        private protected virtual void RedoMove(IWriteableOperation operation)
        {
            IWriteableMoveNodeOperation MoveNodeOperation = (IWriteableMoveNodeOperation)operation;
            ExecuteMove(MoveNodeOperation);
        }

        /// <summary></summary>
        private protected virtual void UndoMove(IWriteableOperation operation)
        {
            IWriteableMoveNodeOperation MoveNodeOperation = (IWriteableMoveNodeOperation)operation;
            MoveNodeOperation = MoveNodeOperation.ToInverseMove();

            ExecuteMove(MoveNodeOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteMove(IWriteableMoveNodeOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex>;

            Inner.Move(operation);
            NotifyStateMoved(operation);
        }

        /// <summary>
        /// Checks whether a block can be moved in a block list.
        /// </summary>
        /// <param name="inner">The inner where the block is.</param>
        /// <param name="blockIndex">Index of the block that would be moved.</param>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        public virtual bool IsBlockMoveable(IWriteableBlockListInner inner, int blockIndex, int direction)
        {
            Debug.Assert(inner != null);
            Debug.Assert(blockIndex >= 0 && blockIndex < inner.BlockStateList.Count);

            return blockIndex + direction >= 0 && blockIndex + direction < inner.BlockStateList.Count;
        }

        /// <summary>
        /// Moves a block around in a block list.
        /// </summary>
        /// <param name="inner">The inner where the block is moved.</param>
        /// <param name="blockIndex">Index of the block to move.</param>
        /// <param name="direction">The change in position, relative to the current block position.</param>
        public virtual void MoveBlock(IWriteableBlockListInner inner, int blockIndex, int direction)
        {
            Debug.Assert(inner != null);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoMoveBlock(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoMoveBlock(operation);
            IWriteableMoveBlockOperation Operation = CreateMoveBlockOperation(inner.Owner.Node, inner.PropertyName, blockIndex, direction, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        /// <summary></summary>
        private protected virtual void RedoMoveBlock(IWriteableOperation operation)
        {
            IWriteableMoveBlockOperation MoveBlockOperation = (IWriteableMoveBlockOperation)operation;
            ExecuteMoveBlock(MoveBlockOperation);
        }

        /// <summary></summary>
        private protected virtual void UndoMoveBlock(IWriteableOperation operation)
        {
            IWriteableMoveBlockOperation MoveBlockOperation = (IWriteableMoveBlockOperation)operation;
            MoveBlockOperation = MoveBlockOperation.ToInverseMoveBlock();

            ExecuteMoveBlock(MoveBlockOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteMoveBlock(IWriteableMoveBlockOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;

            IWriteableBlockState BlockState = Inner.BlockStateList[operation.BlockIndex];
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockState.StateList.Count > 0);

            IWriteableNodeState State = BlockState.StateList[0];
            Debug.Assert(State != null);

            IWriteableBrowsingExistingBlockNodeIndex NodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
            Debug.Assert(NodeIndex != null);

            Inner.MoveBlock(operation);

            NotifyBlockStateMoved(operation);
        }

        /// <summary>
        /// Expands an existing node. In the node:
        /// * All optional children are assigned if they aren't
        /// * If the node is a feature call, with no arguments, an empty argument is inserted.
        /// </summary>
        /// <param name="expandedIndex">Index of the expanded node.</param>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already expanded.</param>
        public virtual void Expand(IWriteableNodeIndex expandedIndex, out bool isChanged)
        {
            Debug.Assert(expandedIndex != null);
            Debug.Assert(StateTable.ContainsKey(expandedIndex));
            Debug.Assert(StateTable[expandedIndex] is IWriteablePlaceholderNodeState);

            IWriteablePlaceholderNodeState State = StateTable[expandedIndex] as IWriteablePlaceholderNodeState;
            Debug.Assert(State != null);

            IWriteableInnerReadOnlyDictionary<string> InnerTable = State.InnerTable;
            IWriteableOperationList OperationList = CreateOperationList();

            foreach (KeyValuePair<string, IWriteableInner> Entry in InnerTable)
            {
                if (Entry.Value is IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> AsOptionalInner)
                    ExpandOptional(AsOptionalInner, OperationList);
                else if (Entry.Value is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner)
                    ExpandBlockList(AsBlockListInner, OperationList);
            }

            if (OperationList.Count > 0)
            {
                IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, null);

                SetLastOperation(OperationGroup);
                CheckInvariant();

                isChanged = true;
            }
            else
                isChanged = false;
        }

        /// <summary>
        /// Expands the optional node.
        /// * If assigned, does nothing.
        /// * If it has an item, assign it.
        /// * Otherwise, assign the item to a default node.
        /// </summary>
        private protected virtual void ExpandOptional(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> optionalInner, IWriteableOperationList operationList)
        {
            if (optionalInner.IsAssigned)
                return;

            IWriteableBrowsingOptionalNodeIndex ParentIndex = optionalInner.ChildState.ParentIndex;
            if (ParentIndex.Optional.HasItem)
            {
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoAssign(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoAssign(operation);
                IWriteableAssignmentOperation Operation = CreateAssignmentOperation(optionalInner.Owner.Node, optionalInner.PropertyName, HandlerRedo, HandlerUndo, isNested: false);

                Operation.Redo();

                operationList.Add(Operation);
            }
            else
            {
                INode NewNode = NodeHelper.CreateDefaultFromInterface(optionalInner.InterfaceType);
                Debug.Assert(NewNode != null);

                IWriteableInsertionOptionalNodeIndex NewOptionalNodeIndex = CreateNewOptionalNodeIndex(optionalInner.Owner.Node, optionalInner.PropertyName, NewNode);

                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoReplace(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoReplace(operation);
                IWriteableReplaceOperation Operation = CreateReplaceOperation(optionalInner.Owner.Node, optionalInner.PropertyName, -1, -1, NewNode, HandlerRedo, HandlerUndo, isNested: false);

                Operation.Redo();

                operationList.Add(Operation);
            }
        }

        /// <summary>
        /// Expands the block list.
        /// * Only expand block list of arguments
        /// * Only expand if the list is empty. In that case, add a single default argument.
        /// </summary>
        private protected virtual void ExpandBlockList(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> blockListInner, IWriteableOperationList operationList)
        {
            if (!blockListInner.IsEmpty)
                return;

            if (!NodeHelper.IsCollectionWithExpand(blockListInner.Owner.Node, blockListInner.PropertyName))
                return;

            INode NewItem = NodeHelper.CreateDefaultFromInterface(blockListInner.InterfaceType);
            IPattern NewPattern = NodeHelper.CreateEmptyPattern();
            IIdentifier NewSource = NodeHelper.CreateEmptyIdentifier();
            IBlock NewBlock = NodeTreeHelperBlockList.CreateBlock(blockListInner.Owner.Node, blockListInner.PropertyName, ReplicationStatus.Normal, NewPattern, NewSource);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoExpandBlockList(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoExpandBlockList(operation);
            IWriteableExpandArgumentOperation Operation = CreateExpandArgumentOperation(blockListInner.Owner.Node, blockListInner.PropertyName, NewBlock, NewItem, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();

            operationList.Add(Operation);
        }

        /// <summary></summary>
        private protected virtual void RedoExpandBlockList(IWriteableOperation operation)
        {
            IWriteableExpandArgumentOperation ExpandArgumentOperation = (IWriteableExpandArgumentOperation)operation;
            ExecuteInsertNewBlock(ExpandArgumentOperation);
        }

        /// <summary></summary>
        private protected virtual void UndoExpandBlockList(IWriteableOperation operation)
        {
            IWriteableExpandArgumentOperation ExpandArgumentOperation = (IWriteableExpandArgumentOperation)operation;
            IWriteableRemoveBlockOperation RemoveBlockOperation = ExpandArgumentOperation.ToRemoveBlockOperation();

            ExecuteRemoveBlock(RemoveBlockOperation);
        }

        /// <summary>
        /// Reduces an existing node. Opposite of <see cref="Expand"/>.
        /// </summary>
        /// <param name="reducedIndex">Index of the reduced node.</param>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already reduced.</param>
        public virtual void Reduce(IWriteableNodeIndex reducedIndex, out bool isChanged)
        {
            Debug.Assert(reducedIndex != null);
            Debug.Assert(StateTable.ContainsKey(reducedIndex));
            Debug.Assert(StateTable[reducedIndex] is IWriteablePlaceholderNodeState);

            IWriteableOperationList OperationList = CreateOperationList();

            Reduce(reducedIndex, OperationList, isNested: false);

            if (OperationList.Count > 0)
            {
                IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, null);

                SetLastOperation(OperationGroup);
                CheckInvariant();

                isChanged = true;
            }
            else
                isChanged = false;
        }

        /// <summary></summary>
        private protected virtual void Reduce(IWriteableNodeIndex reducedIndex, IWriteableOperationList operationList, bool isNested)
        {
            IWriteablePlaceholderNodeState State = StateTable[reducedIndex] as IWriteablePlaceholderNodeState;
            Debug.Assert(State != null);

            IWriteableInnerReadOnlyDictionary<string> InnerTable = State.InnerTable;

            foreach (KeyValuePair<string, IWriteableInner> Entry in InnerTable)
            {
                if (Entry.Value is IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> AsOptionalInner)
                    ReduceOptional(AsOptionalInner, operationList, isNested);
                else if (Entry.Value is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner)
                    ReduceBlockList(AsBlockListInner, operationList, isNested);
            }
        }

        /// <summary>
        /// Reduces the optional node.
        /// </summary>
        private protected virtual void ReduceOptional(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> optionalInner, IWriteableOperationList operationList, bool isNested)
        {
            if (optionalInner.IsAssigned)
            {
                IWriteableBrowsingOptionalNodeIndex ParentIndex = optionalInner.ChildState.ParentIndex;

                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoUnassign(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoUnassign(operation);
                IWriteableAssignmentOperation Operation = CreateAssignmentOperation(optionalInner.Owner.Node, optionalInner.PropertyName, HandlerRedo, HandlerUndo, isNested);

                Operation.Redo();

                operationList.Add(Operation);
            }
        }

        /// <summary>
        /// Reduces the block list.
        /// </summary>
        private protected virtual void ReduceBlockList(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> blockListInner, IWriteableOperationList operationList, bool isNested)
        {
            if (!blockListInner.IsSingle)
                return;

            if (NodeHelper.IsCollectionNeverEmpty(blockListInner.Owner.Node, blockListInner.PropertyName))
                return;

            if (!NodeHelper.IsCollectionWithExpand(blockListInner.Owner.Node, blockListInner.PropertyName))
                return;

            Debug.Assert(blockListInner.BlockStateList.Count == 1);
            Debug.Assert(blockListInner.BlockStateList[0].StateList.Count == 1);
            IWriteableNodeState FirstState = blockListInner.BlockStateList[0].StateList[0];

            if (!NodeHelper.IsDefaultNode(FirstState.Node))
                return;

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRemoveBlock(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoRemoveBlock(operation);
            IWriteableRemoveBlockOperation Operation = CreateRemoveBlockOperation(blockListInner.Owner.Node, blockListInner.PropertyName, 0, HandlerRedo, HandlerUndo, isNested);

            Operation.Redo();

            operationList.Add(Operation);
        }

        /// <summary>
        /// Reduces all expanded nodes, and clear all unassigned optional nodes.
        /// </summary>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already canonic.</param>
        public virtual void Canonicalize(out bool isChanged)
        {
            IWriteableOperationList OperationList = CreateOperationList();
            Canonicalize(RootState, OperationList);

            if (OperationList.Count > 0)
            {
                DebugObjects.AddReference(OperationList);

                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRefresh(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                IWriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedo, HandlerUndo, isNested: false);

                RefreshOperation.Redo();

                IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);

                SetLastOperation(OperationGroup);
                CheckInvariant();

                isChanged = true;
            }
            else
                isChanged = false;
        }

        /// <summary></summary>
        private protected virtual void Canonicalize(IWriteableNodeState state, IWriteableOperationList operationList)
        {
            IWriteableNodeIndex NodeIndex = state.ParentIndex as IWriteableNodeIndex;
            Debug.Assert(NodeIndex != null);

            CanonicalizeChildren(state, operationList);

            Reduce(NodeIndex, operationList, isNested: state != RootState);
        }

        /// <summary></summary>
        private protected virtual void CanonicalizeChildren(IWriteableNodeState state, IWriteableOperationList operationList)
        {
            List<IWriteableNodeState> ChildStateList = new List<IWriteableNodeState>();
            foreach (KeyValuePair<string, IWriteableInner> Entry in state.InnerTable)
            {
                switch (Entry.Value)
                {
                    case IWriteablePlaceholderInner<IWriteableBrowsingPlaceholderNodeIndex> AsPlaceholderInner:
                        ChildStateList.Add(AsPlaceholderInner.ChildState);
                        break;

                    case IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> AsOptionalInner:
                        if (AsOptionalInner.IsAssigned)
                            CanonicalizeChildren(AsOptionalInner.ChildState, operationList);
                        break;

                    case IWriteableListInner<IWriteableBrowsingListNodeIndex> AsListlInner:
                        foreach (IWriteablePlaceholderNodeState ChildState in AsListlInner.StateList)
                            ChildStateList.Add(ChildState);
                        break;

                    case IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListlInner:
                        foreach (IWriteableBlockState BlockState in AsBlockListlInner.BlockStateList)
                            foreach (IWriteablePlaceholderNodeState ChildState in BlockState.StateList)
                                ChildStateList.Add(ChildState);
                        break;
                }
            }

            foreach (IWriteableNodeState ChildState in ChildStateList)
                Canonicalize(ChildState, operationList);
        }

        /// <summary></summary>
        private protected virtual void RedoRefresh(IWriteableOperation operation)
        {
            IWriteableGenericRefreshOperation GenericRefreshOperation = (IWriteableGenericRefreshOperation)operation;
            ExecuteRefresh(GenericRefreshOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteRefresh(IWriteableGenericRefreshOperation operation)
        {
            NotifyGenericRefresh(operation);
        }

        /// <summary>
        /// Undo the last operation.
        /// </summary>
        public virtual void Undo()
        {
            Debug.Assert(CanUndo);

            RedoIndex--;
            IWriteableOperationGroup OperationGroup = OperationStack[RedoIndex];
            OperationGroup.Undo();

            CheckInvariant();
        }

        /// <summary>
        /// Redo the last operation undone.
        /// </summary>
        public virtual void Redo()
        {
            Debug.Assert(CanRedo);

            IWriteableOperationGroup OperationGroup = OperationStack[RedoIndex];
            OperationGroup.Redo();
            RedoIndex++;

            CheckInvariant();
        }
        #endregion

        #region Descendant Interface
        /// <summary></summary>
        private protected override void CheckContextConsistency(IReadOnlyBrowseContext browseContext)
        {
            ((WriteableBrowseContext)browseContext).CheckConsistency();
        }

        /// <summary></summary>
        private protected virtual void PruneState(IWriteableNodeState state)
        {
            PruneStateChildren(state);
            RemoveState(state.ParentIndex);
        }

        /// <summary></summary>
        private protected virtual void PruneStateChildren(IWriteableNodeState state)
        {
            foreach (KeyValuePair<string, IWriteableInner> Entry in state.InnerTable)
            {
                bool IsHandled = false;

                if (Entry.Value is IWriteablePlaceholderInner<IWriteableBrowsingPlaceholderNodeIndex> AsPlaceholderInner)
                {
                    PrunePlaceholderInner(AsPlaceholderInner);
                    IsHandled = true;
                }
                else if (Entry.Value is IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> AsOptionalInner)
                {
                    PruneOptionalInner(AsOptionalInner);
                    IsHandled = true;
                }
                else if (Entry.Value is IWriteableListInner<IWriteableBrowsingListNodeIndex> AsListInner)
                {
                    PruneListInner(AsListInner);
                    IsHandled = true;
                }
                else if (Entry.Value is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner)
                {
                    PruneBlockListInner(AsBlockListInner);
                    IsHandled = true;
                }

                Debug.Assert(IsHandled);
            }
        }

        /// <summary></summary>
        private protected virtual void PrunePlaceholderInner(IWriteablePlaceholderInner<IWriteableBrowsingPlaceholderNodeIndex> inner)
        {
            PruneState(inner.ChildState);

            Stats.PlaceholderNodeCount--;
        }

        /// <summary></summary>
        private protected virtual void PruneOptionalInner(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> inner)
        {
            PruneState(inner.ChildState);

            Stats.OptionalNodeCount--;
            if (inner.IsAssigned)
                Stats.AssignedOptionalNodeCount--;
        }

        /// <summary></summary>
        private protected virtual void PruneListInner(IWriteableListInner<IWriteableBrowsingListNodeIndex> inner)
        {
            foreach (IWriteableNodeState State in inner.StateList)
            {
                PruneState(State);

                Stats.PlaceholderNodeCount--;
            }

            Stats.ListCount--;
        }

        /// <summary></summary>
        private protected virtual void PruneBlockListInner(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner)
        {
            for (int BlockIndex = inner.BlockStateList.Count; BlockIndex > 0; BlockIndex--)
            {
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRemoveBlockView(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                IWriteableRemoveBlockViewOperation Operation = CreateRemoveBlockViewOperation(inner.Owner.Node, inner.PropertyName, BlockIndex - 1, HandlerRedo, HandlerUndo, isNested: true);

                Operation.Redo();
            }

            Stats.BlockListCount--;
        }

        /// <summary></summary>
        private protected virtual void RedoRemoveBlockView(IWriteableOperation operation)
        {
            IWriteableRemoveBlockViewOperation RemoveBlockViewOperation = (IWriteableRemoveBlockViewOperation)operation;
            ExecuteRemoveBlockView(RemoveBlockViewOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteRemoveBlockView(IWriteableRemoveBlockViewOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;
            IWriteableBlockState RemovedBlockState = Inner.BlockStateList[operation.BlockIndex];

            for (int Index = 0; Index < RemovedBlockState.StateList.Count; Index++)
            {
                IWriteableNodeState State = RemovedBlockState.StateList[Index];

                PruneState(State);
                Stats.PlaceholderNodeCount--;
            }

            IWriteableBrowsingPatternIndex PatternIndex = RemovedBlockState.PatternIndex;
            IWriteableBrowsingSourceIndex SourceIndex = RemovedBlockState.SourceIndex;

            Debug.Assert(PatternIndex != null);
            Debug.Assert(StateTable.ContainsKey(PatternIndex));
            Debug.Assert(SourceIndex != null);
            Debug.Assert(StateTable.ContainsKey(SourceIndex));

            RemoveState(PatternIndex);
            Stats.PlaceholderNodeCount--;

            RemoveState(SourceIndex);
            Stats.PlaceholderNodeCount--;

            Inner.NotifyBlockStateRemoved(RemovedBlockState);

            Stats.BlockCount--;

            operation.Update(RemovedBlockState);

            NotifyBlockViewRemoved(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyBlockStateInserted(IWriteableInsertBlockOperation operation)
        {
            BlockStateInsertedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyBlockStateRemoved(IWriteableRemoveBlockOperation operation)
        {
            BlockStateRemovedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyBlockViewRemoved(IWriteableRemoveBlockViewOperation operation)
        {
            BlockViewRemovedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyStateInserted(IWriteableInsertNodeOperation operation)
        {
            StateInsertedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyStateRemoved(IWriteableRemoveNodeOperation operation)
        {
            StateRemovedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyStateReplaced(IWriteableReplaceOperation operation)
        {
            StateReplacedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyStateAssigned(IWriteableAssignmentOperation operation)
        {
            StateAssignedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyStateUnassigned(IWriteableAssignmentOperation operation)
        {
            StateUnassignedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyStateChanged(IWriteableChangeNodeOperation operation)
        {
            StateChangedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyBlockStateChanged(IWriteableChangeBlockOperation operation)
        {
            BlockStateChangedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyStateMoved(IWriteableMoveNodeOperation operation)
        {
            StateMovedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyBlockStateMoved(IWriteableMoveBlockOperation operation)
        {
            BlockStateMovedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyBlockSplit(IWriteableSplitBlockOperation operation)
        {
            BlockSplitHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyBlocksMerged(IWriteableMergeBlocksOperation operation)
        {
            BlocksMergedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyGenericRefresh(IWriteableGenericRefreshOperation operation)
        {
            GenericRefreshHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void SetLastOperation(IWriteableOperation operation)
        {
            IWriteableOperationList OperationList = CreateOperationList();
            OperationList.Add(operation);
            IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
            IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, null);

            SetLastOperation(OperationGroup);
        }

        /// <summary></summary>
        private protected virtual void SetLastOperation(IWriteableOperationGroup operationGroup)
        {
            Debug.Assert(RedoIndex >= 0 && RedoIndex <= _OperationStack.Count);

            while (_OperationStack.Count > RedoIndex)
                _OperationStack.RemoveAt(RedoIndex);

            Debug.Assert(RedoIndex == _OperationStack.Count);

            _OperationStack.Add(operationGroup);
            RedoIndex = _OperationStack.Count;

            Debug.Assert(RedoIndex == _OperationStack.Count);
        }
        #endregion

        #region Invariant
        /// <summary></summary>
        private protected override void CheckInvariant()
        {
            base.CheckInvariant();

            bool IsValid = IsNodeTreeValid(RootState.Node);
            Debug.Assert(IsValid);
        }

        /// <summary></summary>
        private protected virtual bool IsNodeTreeValid(INode node)
        {
            Type ChildNodeType;
            IList<string> PropertyNames = NodeTreeHelper.EnumChildNodeProperties(node);
            bool IsValid = true;

            foreach (string PropertyName in PropertyNames)
            {
                if (NodeTreeHelperChild.IsChildNodeProperty(node, PropertyName, out ChildNodeType))
                {
                    IsValid &= InvariantFailed(IsNodeTreeChildNodeValid(node, PropertyName));
                }
                else if (NodeTreeHelperOptional.IsOptionalChildNodeProperty(node, PropertyName, out ChildNodeType))
                {
                    IsValid &= InvariantFailed(IsNodeTreeOptionalNodeValid(node, PropertyName));
                }
                else if (NodeTreeHelperList.IsNodeListProperty(node, PropertyName, out ChildNodeType))
                {
                    IsValid &= InvariantFailed(IsNodeTreeListValid(node, PropertyName));
                }
                else if (NodeTreeHelperBlockList.IsBlockListProperty(node, PropertyName, out Type ChildInterfaceType, out ChildNodeType))
                {
                    IsValid &= InvariantFailed(IsNodeTreeBlockListValid(node, PropertyName));
                }
            }

            return IsValid;
        }

        /// <summary></summary>
        private protected virtual bool IsNodeTreeChildNodeValid(INode node, string propertyName)
        {
            NodeTreeHelperChild.GetChildNode(node, propertyName, out INode ChildNode);
            Debug.Assert(ChildNode != null);

            bool IsValid = InvariantFailed(IsNodeTreeValid(ChildNode));

            return IsValid;
        }

        /// <summary></summary>
        private protected virtual bool IsNodeTreeOptionalNodeValid(INode node, string propertyName)
        {
            bool IsValid = true;

            NodeTreeHelperOptional.GetChildNode(node, propertyName, out bool IsAssigned, out INode ChildNode);
            if (IsAssigned)
            {
                IsValid &= InvariantFailed(IsNodeTreeValid(ChildNode));
            }

            return IsValid;
        }

        /// <summary></summary>
        private protected virtual bool IsNodeTreeListValid(INode node, string propertyName)
        {
            NodeTreeHelperList.GetChildNodeList(node, propertyName, out IReadOnlyList<INode> ChildNodeList);
            Debug.Assert(ChildNodeList != null);

            bool IsValid = true;

            if (ChildNodeList.Count == 0)
            {
                IsValid &= InvariantFailed(IsEmptyListValid(node, propertyName));
            }

            for (int Index = 0; Index < ChildNodeList.Count; Index++)
            {
                INode ChildNode = ChildNodeList[Index];
                Debug.Assert(ChildNode != null);

                IsValid &= InvariantFailed(IsNodeTreeValid(ChildNode));
            }

            return IsValid;
        }

        /// <summary></summary>
        private protected virtual bool IsNodeTreeBlockListValid(INode node, string propertyName)
        {
            NodeTreeHelperBlockList.GetChildBlockList(node, propertyName, out IReadOnlyList<INodeTreeBlock> ChildBlockList);
            Debug.Assert(ChildBlockList != null);

            bool IsValid = true;

            if (ChildBlockList.Count == 0)
            {
                IsValid &= InvariantFailed(IsEmptyBlockListValid(node, propertyName));
            }

            for (int BlockIndex = 0; BlockIndex < ChildBlockList.Count; BlockIndex++)
            {
                INodeTreeBlock Block = ChildBlockList[BlockIndex];
                Debug.Assert(Block.NodeList.Count > 0);

                for (int Index = 0; Index < Block.NodeList.Count; Index++)
                {
                    INode ChildNode = Block.NodeList[Index];
                    Debug.Assert(ChildNode != null);

                    IsValid &= InvariantFailed(IsNodeTreeValid(ChildNode));
                }
            }

            return IsValid;
        }

        /// <summary></summary>
        private protected virtual bool IsEmptyListValid(INode node, string propertyName)
        {
            Type NodeType = node.GetType();
            Debug.Assert(NodeTreeHelperList.IsNodeListProperty(NodeType, propertyName, out Type ChildNodeType));

            bool IsValid = true;

            Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(NodeType);
            IReadOnlyDictionary<Type, string[]> NeverEmptyCollectionTable = NodeHelper.NeverEmptyCollectionTable;
            if (NeverEmptyCollectionTable.ContainsKey(InterfaceType))
            {
                foreach (string Item in NeverEmptyCollectionTable[InterfaceType])
                {
                    IsValid &= InvariantFailed(Item != propertyName);
                }
            }

            return IsValid;
        }

        /// <summary></summary>
        private protected virtual bool IsEmptyBlockListValid(INode node, string propertyName)
        {
            Type NodeType = node.GetType();
            Debug.Assert(NodeTreeHelperBlockList.IsBlockListProperty(NodeType, propertyName, out Type ChildInterfaceType, out Type ChildNodeType));

            bool IsValid = true;

            Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(NodeType);
            IReadOnlyDictionary<Type, string[]> NeverEmptyCollectionTable = NodeHelper.NeverEmptyCollectionTable;
            if (NeverEmptyCollectionTable.ContainsKey(InterfaceType))
            {
                foreach (string Item in NeverEmptyCollectionTable[InterfaceType])
                {
                    IsValid &= InvariantFailed(Item != propertyName);
                }
            }

            return IsValid;
        }

        /// <summary></summary>
        private protected bool InvariantFailed(bool condition)
        {
            Debug.Assert(condition, "Invariant");
            return condition;
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
        /// Creates a IxxxIndexNodeStateReadOnlyDictionary object.
        /// </summary>
        private protected override IReadOnlyIndexNodeStateReadOnlyDictionary CreateStateTableReadOnly(IReadOnlyIndexNodeStateDictionary stateTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableIndexNodeStateReadOnlyDictionary((IWriteableIndexNodeStateDictionary)stateTable);
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
        /// Creates a IxxxInnerReadOnlyDictionary{string} object.
        /// </summary>
        private protected override IReadOnlyInnerReadOnlyDictionary<string> CreateInnerTableReadOnly(IReadOnlyInnerDictionary<string> innerTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableInnerReadOnlyDictionary<string>((IWriteableInnerDictionary<string>)innerTable);
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
        private protected virtual IWriteableInsertionOptionalNodeIndex CreateNewOptionalNodeIndex(INode parentNode, string propertyName, INode node)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableInsertionOptionalNodeIndex(parentNode, propertyName, node);
        }

        /// <summary>
        /// Creates a IxxxInsertNodeOperation object.
        /// </summary>
        private protected virtual IWriteableInsertNodeOperation CreateInsertNodeOperation(INode parentNode, string propertyName, int blockIndex, int index, INode node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableInsertNodeOperation(parentNode, propertyName, blockIndex, index, node, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxInsertBlockOperation object.
        /// </summary>
        private protected virtual IWriteableInsertBlockOperation CreateInsertBlockOperation(INode parentNode, string propertyName, int blockIndex, IBlock block, INode node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableInsertBlockOperation(parentNode, propertyName, blockIndex, block, node, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveBlockOperation object.
        /// </summary>
        private protected virtual IWriteableRemoveBlockOperation CreateRemoveBlockOperation(INode parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableRemoveBlockOperation(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveBlockViewOperation object.
        /// </summary>
        private protected virtual IWriteableRemoveBlockViewOperation CreateRemoveBlockViewOperation(INode parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableRemoveBlockViewOperation(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveNodeOperation object.
        /// </summary>
        private protected virtual IWriteableRemoveNodeOperation CreateRemoveNodeOperation(INode parentNode, string propertyName, int blockIndex, int index, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableRemoveNodeOperation(parentNode, propertyName, blockIndex, index, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxReplaceOperation object.
        /// </summary>
        private protected virtual IWriteableReplaceOperation CreateReplaceOperation(INode parentNode, string propertyName, int blockIndex, int index, INode newNode, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableReplaceOperation(parentNode, propertyName, blockIndex, index, newNode, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxAssignmentOperation object.
        /// </summary>
        private protected virtual IWriteableAssignmentOperation CreateAssignmentOperation(INode parentNode, string propertyName, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableAssignmentOperation(parentNode, propertyName, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeNodeOperation object.
        /// </summary>
        private protected virtual IWriteableChangeNodeOperation CreateChangeNodeOperation(INode parentNode, string propertyName, int value, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableChangeNodeOperation(parentNode, propertyName, value, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeBlockOperation object.
        /// </summary>
        private protected virtual IWriteableChangeBlockOperation CreateChangeBlockOperation(INode parentNode, string propertyName, int blockIndex, ReplicationStatus replication, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableChangeBlockOperation(parentNode, propertyName, blockIndex, replication, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxSplitBlockOperation object.
        /// </summary>
        private protected virtual IWriteableSplitBlockOperation CreateSplitBlockOperation(INode parentNode, string propertyName, int blockIndex, int index, IBlock newBlock, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableSplitBlockOperation(parentNode, propertyName, blockIndex, index, newBlock, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMergeBlocksOperation object.
        /// </summary>
        private protected virtual IWriteableMergeBlocksOperation CreateMergeBlocksOperation(INode parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableMergeBlocksOperation(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMoveNodeOperation object.
        /// </summary>
        private protected virtual IWriteableMoveNodeOperation CreateMoveNodeOperation(INode parentNode, string propertyName, int blockIndex, int index, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableMoveNodeOperation(parentNode, propertyName, blockIndex, index, direction, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMoveBlockOperation object.
        /// </summary>
        private protected virtual IWriteableMoveBlockOperation CreateMoveBlockOperation(INode parentNode, string propertyName, int blockIndex, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableMoveBlockOperation(parentNode, propertyName, blockIndex, direction, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxExpandArgumentOperation object.
        /// </summary>
        private protected virtual IWriteableExpandArgumentOperation CreateExpandArgumentOperation(INode parentNode, string propertyName, IBlock block, INode node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
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
