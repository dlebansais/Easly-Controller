using BaseNode;
using BaseNodeHelper;
using EaslyController.ReadOnly;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.Writeable
{
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
        /// Called when a state is created.
        /// </summary>
        new event Action<IWriteableNodeState> NodeStateCreated;

        /// <summary>
        /// Called when a state is initialized.
        /// </summary>
        new event Action<IWriteableNodeState> NodeStateInitialized;

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        new event Action<IWriteableNodeState> NodeStateRemoved;

        /// <summary>
        /// Called when a block list inner is created
        /// </summary>
        new event Action<IWriteableBlockListInner> BlockListInnerCreated;

        /// <summary>
        /// Called when a block state is inserted.
        /// </summary>
        event Action<IWriteableInsertBlockOperation> BlockStateInserted;

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        event Action<IWriteableRemoveBlockOperation> BlockStateRemoved;

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
        /// Called when an argument block is expanded.
        /// </summary>
        event Action<IWriteableExpandArgumentOperation> ArgumentExpanded;

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
        void Insert(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableInsertionCollectionNodeIndex insertedIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex);

        /// <summary>
        /// Checks whether a node can be removed from a list.
        /// </summary>
        /// <param name="inner">The inner where the node is.</param>
        /// <param name="nodeIndex">Index of the node that would be removed.</param>
        bool IsRemoveable(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex);

        /// <summary>
        /// Removes a node from a list or block list.
        /// </summary>
        /// <param name="inner">The inner for the list or block list from which the node is removed.</param>
        /// <param name="nodeIndex">Index for the removed node.</param>
        void Remove(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex);

        /// <summary>
        /// Replace an existing node with a new one.
        /// </summary>
        /// <param name="inner">The inner where the node is replaced.</param>
        /// <param name="insertedIndex">Index for the replace operation.</param>
        /// <param name="nodeIndex">Index of the replacing node upon return.</param>
        void Replace(IWriteableInner<IWriteableBrowsingChildIndex> inner, IWriteableInsertionChildIndex insertedIndex, out IWriteableBrowsingChildIndex nodeIndex);

        /// <summary>
        /// Assign the optional node.
        /// </summary>
        /// <param name="nodeIndex">Index of the optional node.</param>
        void Assign(IWriteableBrowsingOptionalNodeIndex nodeIndex);

        /// <summary>
        /// Unassign the optional node.
        /// </summary>
        /// <param name="nodeIndex">Index of the optional node.</param>
        void Unassign(IWriteableBrowsingOptionalNodeIndex nodeIndex);

        /// <summary>
        /// Changes the replication state of a block.
        /// </summary>
        /// <param name="inner">The inner where the blok is changed.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="replication">New replication value.</param>
        void ChangeReplication(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex, ReplicationStatus replication);

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
        bool IsSplittable(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Splits a block in two at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block is split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        void SplitBlock(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Checks whether a block can be merged at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block would be split.</param>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        bool IsMergeable(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Merges two blocks at the given index.
        /// </summary>
        /// <param name="inner">The inner where blocks are merged.</param>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        void MergeBlocks(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Checks whether a node can be moved in a list.
        /// </summary>
        /// <param name="inner">The inner where the node is.</param>
        /// <param name="nodeIndex">Index of the node that would be moved.</param>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        bool IsMoveable(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction);

        /// <summary>
        /// Moves a node around in a list or block list. In a block list, the node stays in same block.
        /// </summary>
        /// <param name="inner">The inner for the list or block list in which the node is moved.</param>
        /// <param name="nodeIndex">Index for the moved node.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        void Move(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction);

        /// <summary>
        /// Moves a block around in a block list.
        /// </summary>
        /// <param name="inner">The inner where the block is moved.</param>
        /// <param name="blockIndex">Index of the block to move.</param>
        /// <param name="direction">The change in position, relative to the current block position.</param>
        void MoveBlock(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex, int direction);

        /// <summary>
        /// Expands an existing node. In the node:
        /// * All optional children are assigned if they aren't
        /// * If the node is a feature call, with no arguments, an empty argument is inserted.
        /// </summary>
        /// <param name="expandedIndex">Index of the expanded node.</param>
        void Expand(IWriteableNodeIndex expandedIndex);

        /// <summary>
        /// Reduces an existing node. Opposite of <see cref="Expand"/>.
        /// </summary>
        /// <param name="reducedIndex">Index of the reduced node.</param>
        void Reduce(IWriteableNodeIndex reducedIndex);

        /// <summary>
        /// Reduces all expanded nodes, and clear all unassigned optional nodes.
        /// </summary>
        void Canonicalize();
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
        /// Initializes a new instance of a <see cref="WriteableController"/> object.
        /// </summary>
        protected WriteableController()
        {
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
        /// Called when a state is created.
        /// </summary>
        public new event Action<IWriteableNodeState> NodeStateCreated
        {
            add { AddNodeStateCreatedDelegate((Action<IReadOnlyNodeState>)value); }
            remove { RemoveNodeStateCreatedDelegate((Action<IReadOnlyNodeState>)value); }
        }

        /// <summary>
        /// Called when a state is initialized.
        /// </summary>
        public new event Action<IWriteableNodeState> NodeStateInitialized
        {
            add { AddNodeStateInitializedDelegate((Action<IReadOnlyNodeState>)value); }
            remove { RemoveNodeStateInitializedDelegate((Action<IReadOnlyNodeState>)value); }
        }

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        public new event Action<IWriteableNodeState> NodeStateRemoved
        {
            add { AddNodeStateRemovedDelegate((Action<IReadOnlyNodeState>)value); }
            remove { RemoveNodeStateRemovedDelegate((Action<IReadOnlyNodeState>)value); }
        }

        /// <summary>
        /// Called when a block list inner is created
        /// </summary>
        public new event Action<IWriteableBlockListInner> BlockListInnerCreated
        {
            add { AddBlockListInnerCreatedDelegate((Action<IReadOnlyBlockListInner>)value); }
            remove { RemoveBlockListInnerCreatedDelegate((Action<IReadOnlyBlockListInner>)value); }
        }

        /// <summary>
        /// Called when a block state is inserted.
        /// </summary>
        public event Action<IWriteableInsertBlockOperation> BlockStateInserted
        {
            add { AddBlockStateInsertedDelegate(value); }
            remove { RemoveBlockStateInsertedDelegate(value); }
        }
#pragma warning disable 1591
        protected Action<IWriteableInsertBlockOperation> BlockStateInsertedHandler;
        protected virtual void AddBlockStateInsertedDelegate(Action<IWriteableInsertBlockOperation> handler) { BlockStateInsertedHandler += handler; }
        protected virtual void RemoveBlockStateInsertedDelegate(Action<IWriteableInsertBlockOperation> handler) { BlockStateInsertedHandler -= handler; }
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
        protected Action<IWriteableRemoveBlockOperation> BlockStateRemovedHandler;
        protected virtual void AddBlockStateRemovedDelegate(Action<IWriteableRemoveBlockOperation> handler) { BlockStateRemovedHandler += handler; }
        protected virtual void RemoveBlockStateRemovedDelegate(Action<IWriteableRemoveBlockOperation> handler) { BlockStateRemovedHandler -= handler; }
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
        protected Action<IWriteableInsertNodeOperation> StateInsertedHandler;
        protected virtual void AddStateInsertedDelegate(Action<IWriteableInsertNodeOperation> handler) { StateInsertedHandler += handler; }
        protected virtual void RemoveStateInsertedDelegate(Action<IWriteableInsertNodeOperation> handler) { StateInsertedHandler -= handler; }
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
        protected Action<IWriteableRemoveNodeOperation> StateRemovedHandler;
        protected virtual void AddStateRemovedDelegate(Action<IWriteableRemoveNodeOperation> handler) { StateRemovedHandler += handler; }
        protected virtual void RemoveStateRemovedDelegate(Action<IWriteableRemoveNodeOperation> handler) { StateRemovedHandler -= handler; }
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
        protected Action<IWriteableReplaceOperation> StateReplacedHandler;
        protected virtual void AddStateReplacedDelegate(Action<IWriteableReplaceOperation> handler) { StateReplacedHandler += handler; }
        protected virtual void RemoveStateReplacedDelegate(Action<IWriteableReplaceOperation> handler) { StateReplacedHandler -= handler; }
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
        protected Action<IWriteableAssignmentOperation> StateAssignedHandler;
        protected virtual void AddStateAssignedDelegate(Action<IWriteableAssignmentOperation> handler) { StateAssignedHandler += handler; }
        protected virtual void RemoveStateAssignedDelegate(Action<IWriteableAssignmentOperation> handler) { StateAssignedHandler -= handler; }
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
        protected Action<IWriteableAssignmentOperation> StateUnassignedHandler;
        protected virtual void AddStateUnassignedDelegate(Action<IWriteableAssignmentOperation> handler) { StateUnassignedHandler += handler; }
        protected virtual void RemoveStateUnassignedDelegate(Action<IWriteableAssignmentOperation> handler) { StateUnassignedHandler -= handler; }
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
        protected Action<IWriteableChangeNodeOperation> StateChangedHandler;
        protected virtual void AddStateChangedDelegate(Action<IWriteableChangeNodeOperation> handler) { StateChangedHandler += handler; }
        protected virtual void RemoveStateChangedDelegate(Action<IWriteableChangeNodeOperation> handler) { StateChangedHandler -= handler; }
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
        protected Action<IWriteableMoveNodeOperation> StateMovedHandler;
        protected virtual void AddStateMovedDelegate(Action<IWriteableMoveNodeOperation> handler) { StateMovedHandler += handler; }
        protected virtual void RemoveStateMovedDelegate(Action<IWriteableMoveNodeOperation> handler) { StateMovedHandler -= handler; }
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
        protected Action<IWriteableMoveBlockOperation> BlockStateMovedHandler;
        protected virtual void AddBlockStateMovedDelegate(Action<IWriteableMoveBlockOperation> handler) { BlockStateMovedHandler += handler; }
        protected virtual void RemoveBlockStateMovedDelegate(Action<IWriteableMoveBlockOperation> handler) { BlockStateMovedHandler -= handler; }
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
        protected Action<IWriteableSplitBlockOperation> BlockSplitHandler;
        protected virtual void AddBlockSplitDelegate(Action<IWriteableSplitBlockOperation> handler) { BlockSplitHandler += handler; }
        protected virtual void RemoveBlockSplitDelegate(Action<IWriteableSplitBlockOperation> handler) { BlockSplitHandler -= handler; }
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
        protected Action<IWriteableMergeBlocksOperation> BlocksMergedHandler;
        protected virtual void AddBlocksMergedDelegate(Action<IWriteableMergeBlocksOperation> handler) { BlocksMergedHandler += handler; }
        protected virtual void RemoveBlocksMergedDelegate(Action<IWriteableMergeBlocksOperation> handler) { BlocksMergedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when an argument block is expanded.
        /// </summary>
        public event Action<IWriteableExpandArgumentOperation> ArgumentExpanded
        {
            add { AddArgumentExpandedDelegate(value); }
            remove { RemoveArgumentExpandedDelegate(value); }
        }
#pragma warning disable 1591
        protected Action<IWriteableExpandArgumentOperation> ArgumentExpandedHandler;
        protected virtual void AddArgumentExpandedDelegate(Action<IWriteableExpandArgumentOperation> handler) { ArgumentExpandedHandler += handler; }
        protected virtual void RemoveArgumentExpandedDelegate(Action<IWriteableExpandArgumentOperation> handler) { ArgumentExpandedHandler -= handler; }
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
        protected Action<IWriteableGenericRefreshOperation> GenericRefreshHandler;
        protected virtual void AddGenericRefreshDelegate(Action<IWriteableGenericRefreshOperation> handler) { GenericRefreshHandler += handler; }
        protected virtual void RemoveGenericRefreshDelegate(Action<IWriteableGenericRefreshOperation> handler) { GenericRefreshHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// State table.
        /// </summary>
        protected new IWriteableIndexNodeStateReadOnlyDictionary StateTable { get { return (IWriteableIndexNodeStateReadOnlyDictionary)base.StateTable; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Inserts a new node in a list or block list.
        /// </summary>
        /// <param name="inner">The inner for the list or block list where the node is inserted.</param>
        /// <param name="insertedIndex">Index for the insertion operation.</param>
        /// <param name="nodeIndex">Index of the inserted node upon return.</param>
        public virtual void Insert(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableInsertionCollectionNodeIndex insertedIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex)
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

            if ((inner is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner) && (insertedIndex is IWriteableInsertionNewBlockNodeIndex AsNewBlockIndex))
                InsertNewBlock(AsBlockListInner, AsNewBlockIndex, out nodeIndex);
            else
                InsertNewNode(inner, insertedIndex, out nodeIndex);

            CheckInvariant();
        }

        /// <summary></summary>
        protected virtual void InsertNewBlock(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> blockListInner, IWriteableInsertionNewBlockNodeIndex newBlockIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            IWriteableInsertBlockOperation Operation = CreateInsertBlockOperation(blockListInner, newBlockIndex, isNested: false);

            blockListInner.InsertNew(Operation);

            IWriteableBrowsingExistingBlockNodeIndex BrowsingIndex = Operation.BrowsingIndex;
            IWriteableBlockState BlockState = Operation.BlockState;
            IWriteablePlaceholderNodeState ChildState = Operation.ChildState;
            nodeIndex = BrowsingIndex;

            Debug.Assert(BlockState.StateList.Count == 1);
            Debug.Assert(BlockState.StateList[0] == ChildState);
            BlockState.InitBlockState();
            Stats.BlockCount++;

            IWriteableBrowsingPatternIndex PatternIndex = BlockState.PatternIndex;
            IWriteablePatternState PatternState = BlockState.PatternState;
            AddState(PatternIndex, PatternState);
            Stats.PlaceholderNodeCount++;

            IWriteableBrowsingSourceIndex SourceIndex = BlockState.SourceIndex;
            IWriteableSourceState SourceState = BlockState.SourceState;
            AddState(SourceIndex, SourceState);
            Stats.PlaceholderNodeCount++;

            AddState(nodeIndex, ChildState);
            Stats.PlaceholderNodeCount++;
            BuildStateTable(blockListInner, null, nodeIndex, ChildState);

            Debug.Assert(Contains(nodeIndex));

            NotifyBlockStateInserted(Operation);
        }

        /// <summary></summary>
        protected virtual void InsertNewNode(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableInsertionCollectionNodeIndex insertedIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            IWriteableInsertNodeOperation Operation = CreateInsertNodeOperation(inner, insertedIndex, isNested: false);

            inner.Insert(Operation);

            IWriteableBrowsingCollectionNodeIndex BrowsingIndex = Operation.BrowsingIndex;
            IWriteablePlaceholderNodeState ChildState = Operation.ChildState;
            nodeIndex = BrowsingIndex;
            
            AddState(nodeIndex, ChildState);
            Stats.PlaceholderNodeCount++;
            BuildStateTable(inner, null, nodeIndex, ChildState);

            Debug.Assert(Contains(nodeIndex));

            NotifyStateInserted(Operation);
        }
        /// <summary>
        /// Checks whether a node can be removed from a list.
        /// </summary>
        /// <param name="inner">The inner where the node is.</param>
        /// <param name="nodeIndex">Index of the node that would be removed.</param>
        public bool IsRemoveable(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex)
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

            Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(inner.Owner.Node.GetType());
            IReadOnlyDictionary<Type, string[]> NeverEmptyCollectionTable = NodeHelper.NeverEmptyCollectionTable;
            if (NeverEmptyCollectionTable.ContainsKey(InterfaceType))
            {
                foreach (string Item in NeverEmptyCollectionTable[InterfaceType])
                    if (Item == PropertyName)
                        return false;
            }

            return true;
        }

        /// <summary>
        /// Removes a node from a list or block list.
        /// </summary>
        /// <param name="inner">The inner for the list or block list from which the node is removed.</param>
        /// <param name="nodeIndex">Index for the removed node.</param>
        public virtual void Remove(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex)
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

            if ((inner is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner) && (nodeIndex is IWriteableBrowsingExistingBlockNodeIndex AsBlockIndex))
                RemoveNodeFromBlock(AsBlockListInner, AsBlockIndex);
            else
                RemoveNode(inner, nodeIndex);

            CheckInvariant();
        }

        /// <summary></summary>
        protected virtual void RemoveNodeFromBlock(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> blockListInner, IWriteableBrowsingExistingBlockNodeIndex blockIndex)
        {
            IWriteableRemoveBlockOperation BlockOperation = CreateRemoveBlockOperation(blockListInner, blockIndex, isNested: false);
            IWriteableRemoveNodeOperation NodeOperation = CreateRemoveNodeOperation(blockListInner, blockIndex, isNested: false);

            blockListInner.RemoveWithBlock(BlockOperation, NodeOperation);

            IWriteableBlockState RemovedBlockState = BlockOperation.BlockState;

            if (RemovedBlockState != null)
                RemoveLastBlock(BlockOperation);
            else
            {
                IWriteableNodeState OldState = StateTable[blockIndex];
                PruneState(OldState, true);
                Stats.PlaceholderNodeCount--;

                NotifyStateRemoved(NodeOperation);
            }
        }

        /// <summary></summary>
        protected virtual void RemoveLastBlock(IWriteableRemoveBlockOperation blockOperation)
        {
            IWriteableBrowsingExistingBlockNodeIndex BlockIndex = blockOperation.BlockIndex;
            IWriteableBlockState RemovedBlockState = blockOperation.BlockState;
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

            IWriteableNodeState RemovedChildState = StateTable[BlockIndex];
            PruneState(RemovedChildState, true);
            Stats.PlaceholderNodeCount--;

            NotifyBlockStateRemoved(blockOperation);
        }

        /// <summary></summary>
        protected virtual void RemoveNode(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            IWriteableRemoveNodeOperation NodeOperation = CreateRemoveNodeOperation(inner, nodeIndex, isNested: false);

            inner.Remove(NodeOperation);

            IWriteableNodeState OldState = StateTable[nodeIndex];
            PruneState(OldState, true);
            Stats.PlaceholderNodeCount--;

            NotifyStateRemoved(NodeOperation);
        }

        /// <summary>
        /// Replace an existing node with a new one.
        /// </summary>
        /// <param name="inner">The inner where the node is replaced.</param>
        /// <param name="replacementIndex">Index for the replace operation.</param>
        /// <param name="nodeIndex">Index of the replacing node upon return.</param>
        public void Replace(IWriteableInner<IWriteableBrowsingChildIndex> inner, IWriteableInsertionChildIndex replacementIndex, out IWriteableBrowsingChildIndex nodeIndex)
        {
            ReplaceState(inner, replacementIndex, true, out nodeIndex, out IWriteableReplaceOperation Operation);

            NotifyStateReplaced(Operation);

            CheckInvariant();
        }

        /// <summary></summary>
        protected virtual void ReplaceState(IWriteableInner<IWriteableBrowsingChildIndex> inner, IWriteableInsertionChildIndex replacementIndex, bool cleanupBlockList, out IWriteableBrowsingChildIndex nodeIndex, out IWriteableReplaceOperation operation)
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

            if (inner is IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> AsOptionalInner)
            {
                IWriteableNodeState OldState = AsOptionalInner.ChildState;
                PruneStateChildren(OldState, cleanupBlockList);
            }

            operation = CreateReplaceOperation(inner, replacementIndex, isNested: false);
            inner.Replace(operation);

            IWriteableBrowsingChildIndex OldBrowsingIndex = operation.OldBrowsingIndex;
            IWriteableBrowsingChildIndex NewBrowsingIndex = operation.NewBrowsingIndex;
            IWriteableNodeState ChildState = operation.ChildState;

            if (!(inner is IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex>))
            {
                Debug.Assert(Contains(OldBrowsingIndex));
                IWriteableNodeState OldState = StateTable[OldBrowsingIndex];

                PruneStateChildren(OldState, cleanupBlockList);
            }

            RemoveState(OldBrowsingIndex);
            AddState(NewBrowsingIndex, ChildState);

            BuildStateTable(inner, null, NewBrowsingIndex, ChildState);

            nodeIndex = NewBrowsingIndex;
            Debug.Assert(Contains(nodeIndex));
        }

        /// <summary>
        /// Assign the optional node.
        /// </summary>
        /// <param name="nodeIndex">Index of the optional node.</param>
        public virtual void Assign(IWriteableBrowsingOptionalNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(StateTable.ContainsKey(nodeIndex));

            IWriteableOptionalNodeState State = StateTable[nodeIndex] as IWriteableOptionalNodeState;
            Debug.Assert(State != null);

            IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> Inner = State.ParentInner as IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex>;
            Debug.Assert(Inner != null);

            if (!Inner.IsAssigned)
            {
                IWriteableAssignmentOperation Operation = CreateAssignmentOperation(Inner, nodeIndex, isNested: false);
                Inner.Assign(Operation);

                Stats.AssignedOptionalNodeCount++;

                NotifyStateAssigned(Operation);
            }

            CheckInvariant();
        }

        /// <summary>
        /// Unassign the optional node.
        /// </summary>
        /// <param name="nodeIndex">Index of the optional node.</param>
        public virtual void Unassign(IWriteableBrowsingOptionalNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(StateTable.ContainsKey(nodeIndex));

            IWriteableOptionalNodeState State = StateTable[nodeIndex] as IWriteableOptionalNodeState;
            Debug.Assert(State != null);

            IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> Inner = State.ParentInner as IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex>;
            Debug.Assert(Inner != null);

            if (Inner.IsAssigned)
            {
                IWriteableAssignmentOperation Operation = CreateAssignmentOperation(Inner, nodeIndex, isNested: false);
                Inner.Unassign(Operation);

                Stats.AssignedOptionalNodeCount--;

                NotifyStateUnassigned(Operation);
            }

            CheckInvariant();
        }

        /// <summary>
        /// Changes the replication state of a block.
        /// </summary>
        /// <param name="inner">The inner where the blok is changed.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="replication">New replication value.</param>
        public virtual void ChangeReplication(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex, ReplicationStatus replication)
        {
            Debug.Assert(inner != null);
            Debug.Assert(blockIndex >= 0 && blockIndex < inner.BlockStateList.Count);

            inner.ChangeReplication(blockIndex, replication);

            CheckInvariant();
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

            IWriteableChangeNodeOperation Operation = CreateChangeNodeOperation(nodeIndex, isNested: false);

            IWriteablePlaceholderNodeState State = StateTable[nodeIndex] as IWriteablePlaceholderNodeState;
            Debug.Assert(State != null);
            Debug.Assert(State.ValuePropertyTypeTable.ContainsKey(propertyName));
            Debug.Assert(State.ValuePropertyTypeTable[propertyName] == Constants.ValuePropertyType.Boolean || State.ValuePropertyTypeTable[propertyName] == Constants.ValuePropertyType.Enum);

            Debug.Assert(value >= 0);

            NodeTreeHelper.GetEnumRange(State.Node.GetType(), propertyName, out int Min, out int Max);
            value = (value - Min) % (Max - Min + 1) + Min;
            NodeTreeHelper.SetEnumValue(State.Node, propertyName, value);

            Operation.Update(State);

            NotifyStateChanged(Operation);

            CheckInvariant();
        }

        /// <summary>
        /// Checks whether a block can be split at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block would be split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        public virtual bool IsSplittable(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
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
        public virtual void SplitBlock(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);
            Debug.Assert(inner.IsSplittable(nodeIndex));

            IWriteableBlockState OldBlockState = inner.BlockStateList[nodeIndex.BlockIndex];
            Debug.Assert(nodeIndex.Index < OldBlockState.StateList.Count);

            int OldNodeCount = OldBlockState.StateList.Count;

            IWriteableSplitBlockOperation Operation = CreateSplitBlockOperation(inner, nodeIndex, isNested: false);
            inner.SplitBlock(Operation);
            Stats.BlockCount++;

            IWriteableBlockState NewBlockState = Operation.BlockState;

            Debug.Assert(OldBlockState.StateList.Count + NewBlockState.StateList.Count == OldNodeCount);
            Debug.Assert(NewBlockState.StateList.Count > 0);
            Debug.Assert(OldBlockState.StateList[0].ParentIndex == nodeIndex);

            IReadOnlyBrowsingPatternIndex PatternIndex = NewBlockState.PatternIndex;
            IReadOnlyPatternState PatternState = NewBlockState.PatternState;
            AddState(PatternIndex, PatternState);
            Stats.PlaceholderNodeCount++;

            IReadOnlyBrowsingSourceIndex SourceIndex = NewBlockState.SourceIndex;
            IReadOnlySourceState SourceState = NewBlockState.SourceState;
            AddState(SourceIndex, SourceState);
            Stats.PlaceholderNodeCount++;

            NotifyBlockSplit(Operation);

            CheckInvariant();
        }

        /// <summary>
        /// Checks whether a block can be merged at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block would be split.</param>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        public virtual bool IsMergeable(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
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
        public virtual void MergeBlocks(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);
            Debug.Assert(inner.IsMergeable(nodeIndex));

            int BlockIndex = nodeIndex.BlockIndex;
            IWriteableBlockState FirstBlockState = inner.BlockStateList[BlockIndex - 1];
            IWriteableBlockState SecondBlockState = inner.BlockStateList[BlockIndex];

            IWriteableMergeBlocksOperation Operation = CreateMergeBlocksOperation(inner, nodeIndex, isNested: false);

            IReadOnlyBrowsingSourceIndex SourceIndex = FirstBlockState.SourceIndex;
            RemoveState(SourceIndex);
            Stats.PlaceholderNodeCount--;

            IReadOnlyBrowsingPatternIndex PatternIndex = FirstBlockState.PatternIndex;
            RemoveState(PatternIndex);
            Stats.PlaceholderNodeCount--;

            int OldNodeCount = FirstBlockState.StateList.Count + SecondBlockState.StateList.Count;
            int FirstNodeIndex = FirstBlockState.StateList.Count;

            inner.MergeBlocks(Operation);
            Stats.BlockCount--;

            IWriteableBlockState BlockState = inner.BlockStateList[BlockIndex - 1];

            Debug.Assert(BlockState.StateList.Count == OldNodeCount);
            Debug.Assert(FirstNodeIndex < BlockState.StateList.Count);
            Debug.Assert(BlockState.StateList[FirstNodeIndex].ParentIndex == nodeIndex);

            NotifyBlocksMerged(Operation);

            CheckInvariant();
        }

        /// <summary>
        /// Checks whether a node can be moved in a list.
        /// </summary>
        /// <param name="inner">The inner where the node is.</param>
        /// <param name="nodeIndex">Index of the node that would be moved.</param>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        public virtual bool IsMoveable(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction)
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
        public virtual void Move(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IWriteableNodeState State = StateTable[nodeIndex];
            Debug.Assert(State != null);

            IWriteableMoveNodeOperation Operation = CreateMoveNodeOperation(inner, nodeIndex, direction, isNested: false);

            inner.Move(Operation);

            NotifyStateMoved(Operation);

            CheckInvariant();
        }

        /// <summary>
        /// Moves a block around in a block list.
        /// </summary>
        /// <param name="inner">The inner where the block is moved.</param>
        /// <param name="blockIndex">Index of the block to move.</param>
        /// <param name="direction">The change in position, relative to the current block position.</param>
        public virtual void MoveBlock(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex, int direction)
        {
            Debug.Assert(inner != null);

            IWriteableBlockState BlockState = inner.BlockStateList[blockIndex];
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockState.StateList.Count > 0);

            IWriteableNodeState State = BlockState.StateList[0];
            Debug.Assert(State != null);

            IWriteableBrowsingExistingBlockNodeIndex NodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
            Debug.Assert(NodeIndex != null);

            IWriteableMoveBlockOperation Operation = CreateMoveBlockOperation(inner, blockIndex, direction, isNested: false);
            
            inner.MoveBlock(Operation);

            NotifyBlockStateMoved(Operation);

            CheckInvariant();
        }

        /// <summary>
        /// Expands an existing node. In the node:
        /// * All optional children are assigned if they aren't
        /// * If the node is a feature call, with no arguments, an empty argument is inserted.
        /// </summary>
        /// <param name="expandedIndex">Index of the expanded node.</param>
        public virtual void Expand(IWriteableNodeIndex expandedIndex)
        {
            Debug.Assert(expandedIndex != null);
            Debug.Assert(StateTable.ContainsKey(expandedIndex));
            Debug.Assert(StateTable[expandedIndex] is IWriteablePlaceholderNodeState);

            IWriteablePlaceholderNodeState State = StateTable[expandedIndex] as IWriteablePlaceholderNodeState;
            Debug.Assert(State != null);

            IWriteableInnerReadOnlyDictionary<string> InnerTable = State.InnerTable;

            foreach (KeyValuePair<string, IWriteableInner<IWriteableBrowsingChildIndex>> Entry in InnerTable)
            {
                if (Entry.Value is IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> AsOptionalInner)
                    ExpandOptional(AsOptionalInner);

                else if (Entry.Value is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner)
                    ExpandBlockList(AsBlockListInner);
            }

            CheckInvariant();
        }

        /// <summary>
        /// Expands the optional node.
        /// * If assigned, does nothing.
        /// * If it has an item, assign it.
        /// * Otherwise, assign the item to a default node.
        /// </summary>
        protected virtual void ExpandOptional(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> optionalInner)
        {
            if (optionalInner.IsAssigned)
                return;

            IWriteableBrowsingOptionalNodeIndex ParentIndex = optionalInner.ChildState.ParentIndex;
            if (ParentIndex.Optional.HasItem)
            {
                IWriteableAssignmentOperation Operation = CreateAssignmentOperation(optionalInner, ParentIndex, isNested: false);
                optionalInner.Assign(Operation);

                Stats.AssignedOptionalNodeCount++;

                NotifyStateAssigned(Operation);
            }
            else
            {
                INode NewNode = NodeHelper.CreateDefaultFromInterface(optionalInner.InterfaceType);
                Debug.Assert(NewNode != null);

                IWriteableInsertionOptionalNodeIndex NewOptionalNodeIndex = CreateNewOptionalNodeIndex(optionalInner.Owner.Node, optionalInner.PropertyName, NewNode);
                IWriteableReplaceOperation Operation = CreateReplaceOperation(optionalInner, NewOptionalNodeIndex, isNested: false);

                optionalInner.Replace(Operation);

                IWriteableBrowsingChildIndex OldBrowsingIndex = Operation.OldBrowsingIndex;
                Debug.Assert(Contains(OldBrowsingIndex));
                IWriteableNodeState OldState = StateTable[OldBrowsingIndex];
                PruneState(OldState, true);

                IWriteableBrowsingChildIndex NewBrowsingIndex = Operation.NewBrowsingIndex;
                IWriteableNodeState ChildState = Operation.ChildState;
                AddState(NewBrowsingIndex, ChildState);
                Stats.AssignedOptionalNodeCount++;

                BuildStateTable(optionalInner, null, NewBrowsingIndex, ChildState);

                NotifyStateReplaced(Operation);
            }
        }

        /// <summary>
        /// Expands the block list.
        /// * Only expand block list of arguments
        /// * Only expand if the list is empty. In that case, add a single default argument.
        /// </summary>
        protected virtual void ExpandBlockList(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> blockListInner)
        {
            if (!(blockListInner.InterfaceType == typeof(IArgument)))
                return;

            if (!blockListInner.IsEmpty)
                return;

            IArgument NewArgument = NodeHelper.CreateDefaultArgument();
            IPattern NewPattern = NodeHelper.CreateEmptyPattern();
            IIdentifier NewSource = NodeHelper.CreateEmptyIdentifier();
            IWriteableInsertionNewBlockNodeIndex NewBlockNodeIndex = CreateNewBlockNodeIndex(blockListInner.Owner.Node, blockListInner.PropertyName, NewArgument, 0, NewPattern, NewSource);

            IWriteableExpandArgumentOperation Operation = CreateExpandArgumentOperation(blockListInner, NewBlockNodeIndex, isNested: false);

            blockListInner.InsertNew(Operation);

            IWriteableBrowsingExistingBlockNodeIndex ArgumentNodeIndex = Operation.BrowsingIndex;
            IWriteableBlockState BlockState = Operation.BlockState;
            IWriteablePlaceholderNodeState ArgumentChildState = Operation.ChildState;

            Debug.Assert(BlockState.StateList.Count == 1);
            Debug.Assert(BlockState.StateList[0] == ArgumentChildState);
            BlockState.InitBlockState();
            Stats.BlockCount++;

            IReadOnlyBrowsingPatternIndex PatternIndex = BlockState.PatternIndex;
            IReadOnlyPatternState PatternState = BlockState.PatternState;
            AddState(PatternIndex, PatternState);
            Stats.PlaceholderNodeCount++;

            IReadOnlyBrowsingSourceIndex SourceIndex = BlockState.SourceIndex;
            IReadOnlySourceState SourceState = BlockState.SourceState;
            AddState(SourceIndex, SourceState);
            Stats.PlaceholderNodeCount++;

            AddState(ArgumentNodeIndex, ArgumentChildState);
            Stats.PlaceholderNodeCount++;
            BuildStateTable(blockListInner, null, ArgumentNodeIndex, ArgumentChildState);

            NotifyArgumentExpanded(Operation);
        }

        /// <summary>
        /// Reduces an existing node. Opposite of <see cref="Expand"/>.
        /// </summary>
        /// <param name="reducedIndex">Index of the reduced node.</param>
        public virtual void Reduce(IWriteableNodeIndex reducedIndex)
        {
            Debug.Assert(reducedIndex != null);
            Debug.Assert(StateTable.ContainsKey(reducedIndex));
            Debug.Assert(StateTable[reducedIndex] is IWriteablePlaceholderNodeState);

            Reduce(reducedIndex, isNested: false);

            CheckInvariant();
        }

        /// <summary></summary>
        protected virtual void Reduce(IWriteableNodeIndex reducedIndex, bool isNested)
        {
            IWriteablePlaceholderNodeState State = StateTable[reducedIndex] as IWriteablePlaceholderNodeState;
            Debug.Assert(State != null);

            IWriteableInnerReadOnlyDictionary<string> InnerTable = State.InnerTable;

            foreach (KeyValuePair<string, IWriteableInner<IWriteableBrowsingChildIndex>> Entry in InnerTable)
            {
                if (Entry.Value is IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> AsOptionalInner)
                    ReduceOptional(AsOptionalInner, isNested);

                else if (Entry.Value is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner)
                    ReduceBlockList(AsBlockListInner, isNested);
            }
        }

        /// <summary>
        /// Reduces the optional node.
        /// </summary>
        protected virtual void ReduceOptional(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> optionalInner, bool isNested)
        {
            if (optionalInner.IsAssigned)
            {
                IWriteableBrowsingOptionalNodeIndex ParentIndex = optionalInner.ChildState.ParentIndex;
                IWriteableAssignmentOperation Operation = CreateAssignmentOperation(optionalInner, ParentIndex, isNested);
                optionalInner.Unassign(Operation);

                Stats.AssignedOptionalNodeCount--;

                NotifyStateUnassigned(Operation);
            }
        }

        /// <summary>
        /// Reduces the block list.
        /// </summary>
        protected virtual void ReduceBlockList(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> blockListInner, bool isNested)
        {
            if (!(blockListInner.InterfaceType == typeof(IArgument)))
                return;

            if (!blockListInner.IsSingle)
                return;

            Debug.Assert(blockListInner.BlockStateList.Count == 1);
            Debug.Assert(blockListInner.BlockStateList[0].StateList.Count == 1);
            IWriteableNodeState FirstState = blockListInner.BlockStateList[0].StateList[0];

            if (!NodeHelper.IsDefaultArgument(FirstState.Node))
                return;

            IWriteableBrowsingExistingBlockNodeIndex FirstNodeIndex = FirstState.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
            Debug.Assert(FirstNodeIndex != null);

            Debug.Assert(FirstState == StateTable[FirstNodeIndex]);
            PruneState(FirstState, true);
            Stats.PlaceholderNodeCount--;

            IWriteableRemoveBlockOperation BlockOperation = CreateRemoveBlockOperation(blockListInner, FirstNodeIndex, isNested);
            blockListInner.RemoveWithBlock(BlockOperation, null);

            IWriteableBlockState RemovedBlockState = BlockOperation.BlockState;
            Debug.Assert(RemovedBlockState != null);

            Stats.BlockCount--;

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

            NotifyBlockStateRemoved(BlockOperation);
        }

        /// <summary>
        /// Reduces all expanded nodes, and clear all unassigned optional nodes.
        /// </summary>
        public virtual void Canonicalize()
        {
            Canonicalize(RootState);

            IWriteableGenericRefreshOperation Operation = CreateGenericRefreshOperation(RootState, isNested: false);
            NotifyGenericRefresh(Operation);

            CheckInvariant();
        }

        /// <summary></summary>
        protected virtual void Canonicalize(IWriteableNodeState state)
        {
            IWriteableNodeIndex NodeIndex = state.ParentIndex as IWriteableNodeIndex;
            Debug.Assert(NodeIndex != null);

            CanonicalizeChildren(state);
            Reduce(NodeIndex, isNested: state != RootState);
        }

        /// <summary></summary>
        protected virtual void CanonicalizeChildren(IWriteableNodeState state)
        {
            List<IWriteableNodeState> ChildStateList = new List<IWriteableNodeState>();
            foreach (KeyValuePair<string, IWriteableInner<IWriteableBrowsingChildIndex>> Entry in state.InnerTable)
            {
                switch (Entry.Value)
                {
                    case IWriteablePlaceholderInner<IWriteableBrowsingPlaceholderNodeIndex> AsPlaceholderInner:
                        ChildStateList.Add(AsPlaceholderInner.ChildState);
                        break;

                    case IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> AsOptionalInner:
                        if (AsOptionalInner.IsAssigned)
                            CanonicalizeChildren(AsOptionalInner.ChildState);
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
                Canonicalize(ChildState);
        }
        #endregion

        #region Descendant Interface
        /// <summary></summary>
        protected virtual void PruneState(IWriteableNodeState state, bool cleanupBlockList)
        {
            PruneStateChildren(state, cleanupBlockList);
            RemoveState(state.ParentIndex);
        }

        /// <summary></summary>
        protected virtual void PruneStateChildren(IWriteableNodeState state, bool cleanupBlockList)
        {
            foreach (KeyValuePair<string, IWriteableInner<IWriteableBrowsingChildIndex>> Entry in state.InnerTable)
                if (Entry.Value is IWriteablePlaceholderInner<IWriteableBrowsingPlaceholderNodeIndex> AsPlaceholderInner)
                    PrunePlaceholderInner(AsPlaceholderInner, cleanupBlockList);
                else if (Entry.Value is IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> AsOptionalInner)
                    PruneOptionalInner(AsOptionalInner, cleanupBlockList);
                else if (Entry.Value is IWriteableListInner<IWriteableBrowsingListNodeIndex> AsListInner)
                    PruneListInner(AsListInner, cleanupBlockList);
                else if (Entry.Value is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner)
                    PruneBlockListInner(AsBlockListInner, cleanupBlockList);
                else
                    Debug.Assert(false);
        }

        /// <summary></summary>
        protected virtual void PrunePlaceholderInner(IWriteablePlaceholderInner<IWriteableBrowsingPlaceholderNodeIndex> inner, bool cleanupBlockList)
        {
            PruneState(inner.ChildState, cleanupBlockList);

            Stats.PlaceholderNodeCount--;
        }

        /// <summary></summary>
        protected virtual void PruneOptionalInner(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> inner, bool cleanupBlockList)
        {
            PruneState(inner.ChildState, cleanupBlockList);

            Stats.OptionalNodeCount--;
            if (inner.IsAssigned)
                Stats.AssignedOptionalNodeCount--;
        }

        /// <summary></summary>
        protected virtual void PruneListInner(IWriteableListInner<IWriteableBrowsingListNodeIndex> inner, bool cleanupBlockList)
        {
            foreach (IWriteableNodeState State in inner.StateList)
            {
                PruneState(State, cleanupBlockList);

                Stats.PlaceholderNodeCount--;
            }

            Stats.ListCount--;
        }

        /// <summary></summary>
        protected virtual void PruneBlockListInner(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, bool cleanupBlockList)
        {
            IWriteableBrowsingExistingBlockNodeIndex FirstNodeIndex = null;

            for (int BlockIndex = 0; BlockIndex < inner.BlockStateList.Count; BlockIndex++)
            {
                IWriteableBlockState RemovedBlockState = inner.BlockStateList[BlockIndex];

                for (int Index = 0; Index < RemovedBlockState.StateList.Count; Index++)
                {
                    IWriteableNodeState State = RemovedBlockState.StateList[Index];

                    if (FirstNodeIndex == null)
                        FirstNodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;

                    PruneState(State, cleanupBlockList);
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

                inner.NotifyBlockStateRemoved(RemovedBlockState);

                Debug.Assert(FirstNodeIndex != null);
                IWriteableRemoveBlockOperation BlockOperation = CreateRemoveBlockOperation(inner, FirstNodeIndex, isNested: true);
                BlockOperation.Update(RemovedBlockState);

                NotifyBlockStateRemoved(BlockOperation);

                Stats.BlockCount--;
            }

            if (cleanupBlockList)
            {
                IBlockList BlockList = NodeTreeHelperBlockList.GetBlockList(inner.Owner.Node, inner.PropertyName);
                BlockList.NodeBlockList.Clear();
            }

            Stats.BlockListCount--;

        }

        /// <summary></summary>
        protected virtual void NotifyBlockStateInserted(IWriteableInsertBlockOperation operation)
        {
            BlockStateInsertedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        protected virtual void NotifyBlockStateRemoved(IWriteableRemoveBlockOperation operation)
        {
            BlockStateRemovedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        protected virtual void NotifyStateInserted(IWriteableInsertNodeOperation operation)
        {
            StateInsertedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        protected virtual void NotifyStateRemoved(IWriteableRemoveNodeOperation operation)
        {
            StateRemovedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        protected virtual void NotifyStateReplaced(IWriteableReplaceOperation operation)
        {
            StateReplacedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        protected virtual void NotifyStateAssigned(IWriteableAssignmentOperation operation)
        {
            StateAssignedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        protected virtual void NotifyStateUnassigned(IWriteableAssignmentOperation operation)
        {
            StateUnassignedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        protected virtual void NotifyStateChanged(IWriteableChangeNodeOperation operation)
        {
            StateChangedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        protected virtual void NotifyStateMoved(IWriteableMoveNodeOperation operation)
        {
            StateMovedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        protected virtual void NotifyBlockStateMoved(IWriteableMoveBlockOperation operation)
        {
            BlockStateMovedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        protected virtual void NotifyBlockSplit(IWriteableSplitBlockOperation operation)
        {
            BlockSplitHandler?.Invoke(operation);
        }

        /// <summary></summary>
        protected virtual void NotifyBlocksMerged(IWriteableMergeBlocksOperation operation)
        {
            BlocksMergedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        protected virtual void NotifyArgumentExpanded(IWriteableExpandArgumentOperation operation)
        {
            ArgumentExpandedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        protected virtual void NotifyGenericRefresh(IWriteableGenericRefreshOperation operation)
        {
            GenericRefreshHandler?.Invoke(operation);
        }
        #endregion

        #region Invariant
        /// <summary></summary>
        protected override void CheckInvariant()
        {
            base.CheckInvariant();

            bool IsValid = IsNodeTreeValid(RootState.Node);
            Debug.Assert(IsValid);
        }

        /// <summary></summary>
        protected virtual bool IsNodeTreeValid(INode node)
        {
            Type ChildNodeType;
            IList<string> PropertyNames = NodeTreeHelper.EnumChildNodeProperties(node);

            foreach (string PropertyName in PropertyNames)
            {
                if (NodeTreeHelperChild.IsChildNodeProperty(node, PropertyName, out ChildNodeType))
                {
                    NodeTreeHelperChild.GetChildNode(node, PropertyName, out INode ChildNode);
                    if (!IsNodeTreeValid(ChildNode))
                        return false;
                }

                else if (NodeTreeHelperOptional.IsOptionalChildNodeProperty(node, PropertyName, out ChildNodeType))
                {
                    NodeTreeHelperOptional.GetChildNode(node, PropertyName, out bool IsAssigned, out INode ChildNode);
                    if (IsAssigned)
                        if (!IsNodeTreeValid(ChildNode))
                            return false;
                }

                else if (NodeTreeHelperList.IsNodeListProperty(node, PropertyName, out ChildNodeType))
                {
                    NodeTreeHelperList.GetChildNodeList(node, PropertyName, out IReadOnlyList<INode> ChildNodeList);

                    if (ChildNodeList.Count == 0)
                        if (!IsEmptyListValid(node, PropertyName))
                            return false;

                    for (int Index = 0; Index < ChildNodeList.Count; Index++)
                    {
                        INode ChildNode = ChildNodeList[Index];
                        if (!IsNodeTreeValid(ChildNode))
                            return false;
                    }
                }

                else if (NodeTreeHelperBlockList.IsBlockListProperty(node, PropertyName, out Type ChildInterfaceType, out ChildNodeType))
                {
                    NodeTreeHelperBlockList.GetChildBlockList(node, PropertyName, out IReadOnlyList<INodeTreeBlock> ChildBlockList);

                    if (ChildBlockList.Count == 0)
                        if (!IsEmptyBlockListValid(node, PropertyName))
                            return false;

                    for (int BlockIndex = 0; BlockIndex < ChildBlockList.Count; BlockIndex++)
                    {
                        INodeTreeBlock Block = ChildBlockList[BlockIndex];
                        Debug.Assert(Block.NodeList.Count > 0);

                        for (int Index = 0; Index < Block.NodeList.Count; Index++)
                        {
                            INode ChildNode = Block.NodeList[Index];
                            if (!IsNodeTreeValid(ChildNode))
                                return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary></summary>
        protected virtual bool IsEmptyListValid(INode node, string propertyName)
        {
            Type NodeType = node.GetType();
            Debug.Assert(NodeTreeHelperList.IsNodeListProperty(NodeType, propertyName, out Type ChildNodeType));

            Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(NodeType);
            IReadOnlyDictionary<Type, string[]> NeverEmptyCollectionTable = NodeHelper.NeverEmptyCollectionTable;
            if (NeverEmptyCollectionTable.ContainsKey(InterfaceType))
            {
                foreach (string Item in NeverEmptyCollectionTable[InterfaceType])
                    if (Item == propertyName)
                        return false;
            }

            return true;
        }

        /// <summary></summary>
        protected virtual bool IsEmptyBlockListValid(INode node, string propertyName)
        {
            Type NodeType = node.GetType();
            Debug.Assert(NodeTreeHelperBlockList.IsBlockListProperty(NodeType, propertyName, out Type ChildInterfaceType, out Type ChildNodeType));

            Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(NodeType);
            IReadOnlyDictionary<Type, string[]> NeverEmptyCollectionTable = NodeHelper.NeverEmptyCollectionTable;
            if (NeverEmptyCollectionTable.ContainsKey(InterfaceType))
            {
                foreach (string Item in NeverEmptyCollectionTable[InterfaceType])
                    if (Item == propertyName)
                        return false;
            }

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        protected override IReadOnlyIndexNodeStateDictionary CreateStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableIndexNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxIndexNodeStateReadOnlyDictionary object.
        /// </summary>
        protected override IReadOnlyIndexNodeStateReadOnlyDictionary CreateStateTableReadOnly(IReadOnlyIndexNodeStateDictionary stateTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableIndexNodeStateReadOnlyDictionary((IWriteableIndexNodeStateDictionary)stateTable);
        }

        /// <summary>
        /// Creates a IxxxInnerDictionary{string} object.
        /// </summary>
        protected override IReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableInnerDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxInnerReadOnlyDictionary{string} object.
        /// </summary>
        protected override IReadOnlyInnerReadOnlyDictionary<string> CreateInnerTableReadOnly(IReadOnlyInnerDictionary<string> innerTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableInnerReadOnlyDictionary<string>((IWriteableInnerDictionary<string>)innerTable);
        }

        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        protected override IReadOnlyIndexNodeStateDictionary CreateChildStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableIndexNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxxBrowseContext object.
        /// </summary>
        protected override IReadOnlyBrowseContext CreateBrowseContext(IReadOnlyBrowseContext parentBrowseContext, IReadOnlyNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableBrowseContext((IWriteableNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        protected override IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreatePlaceholderInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingPlaceholderNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteablePlaceholderInner<IWriteableBrowsingPlaceholderNodeIndex, WriteableBrowsingPlaceholderNodeIndex>((IWriteableNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxOptionalInner{IxxxBrowsingOptionalNodeIndex} object.
        /// </summary>
        protected override IReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex> CreateOptionalInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingOptionalNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex, WriteableBrowsingOptionalNodeIndex>((IWriteableNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxListInner{IxxxBrowsingListNodeIndex} object.
        /// </summary>
        protected override IReadOnlyListInner<IReadOnlyBrowsingListNodeIndex> CreateListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingListNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableListInner<IWriteableBrowsingListNodeIndex, WriteableBrowsingListNodeIndex>((IWriteableNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxBlockListInner{IxxxBrowsingBlockNodeIndex} object.
        /// </summary>
        protected override IReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex> CreateBlockListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingBlockNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableBlockListInner<IWriteableBrowsingBlockNodeIndex, WriteableBrowsingBlockNodeIndex>((IWriteableNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeState CreateRootNodeState(IReadOnlyRootNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteablePlaceholderNodeState((IWriteableRootNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxInsertionNewBlockNodeIndex object.
        /// </summary>
        protected virtual IWriteableInsertionNewBlockNodeIndex CreateNewBlockNodeIndex(INode parentNode, string propertyName, INode node, int blockIndex, IPattern patternNode, IIdentifier sourceNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableInsertionNewBlockNodeIndex(parentNode, propertyName, node, 0, patternNode, sourceNode);
        }

        /// <summary>
        /// Creates a IxxxWriteableInsertionOptionalNodeIndex object.
        /// </summary>
        protected virtual IWriteableInsertionOptionalNodeIndex CreateNewOptionalNodeIndex(INode parentNode, string propertyName, INode node)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableInsertionOptionalNodeIndex(parentNode, propertyName, node);
        }

        /// <summary>
        /// Creates a IxxxInsertNodeOperation object.
        /// </summary>
        protected virtual IWriteableInsertNodeOperation CreateInsertNodeOperation(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableInsertionCollectionNodeIndex insertionIndex, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableInsertNodeOperation(inner, insertionIndex, isNested);
        }

        /// <summary>
        /// Creates a IxxxInsertBlockOperation object.
        /// </summary>
        protected virtual IWriteableInsertBlockOperation CreateInsertBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableInsertionNewBlockNodeIndex blockIndex, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableInsertBlockOperation(inner, blockIndex, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveBlockOperation object.
        /// </summary>
        protected virtual IWriteableRemoveBlockOperation CreateRemoveBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex blockIndex, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableRemoveBlockOperation(inner, blockIndex, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveNodeOperation object.
        /// </summary>
        protected virtual IWriteableRemoveNodeOperation CreateRemoveNodeOperation(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableRemoveNodeOperation(inner, nodeIndex, isNested);
        }

        /// <summary>
        /// Creates a IxxxReplaceOperation object.
        /// </summary>
        protected virtual IWriteableReplaceOperation CreateReplaceOperation(IWriteableInner<IWriteableBrowsingChildIndex> inner, IWriteableInsertionChildIndex replacementIndex, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableReplaceOperation(inner, replacementIndex, isNested);
        }

        /// <summary>
        /// Creates a IxxxAssignmentOperation object.
        /// </summary>
        protected virtual IWriteableAssignmentOperation CreateAssignmentOperation(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> inner, IWriteableBrowsingOptionalNodeIndex nodeIndex, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableAssignmentOperation(inner, nodeIndex, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeNodeOperation object.
        /// </summary>
        protected virtual IWriteableChangeNodeOperation CreateChangeNodeOperation(IWriteableIndex nodeIndex, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableChangeNodeOperation(nodeIndex, isNested);
        }

        /// <summary>
        /// Creates a IxxxSplitBlockOperation object.
        /// </summary>
        protected virtual IWriteableSplitBlockOperation CreateSplitBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableSplitBlockOperation(inner, nodeIndex, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMergeBlocksOperation object.
        /// </summary>
        protected virtual IWriteableMergeBlocksOperation CreateMergeBlocksOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableMergeBlocksOperation(inner, nodeIndex, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMoveNodeOperation object.
        /// </summary>
        protected virtual IWriteableMoveNodeOperation CreateMoveNodeOperation(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableMoveNodeOperation(inner, nodeIndex, direction, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMoveBlockOperation object.
        /// </summary>
        protected virtual IWriteableMoveBlockOperation CreateMoveBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex, int direction, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableMoveBlockOperation(inner, blockIndex, direction, isNested);
        }

        /// <summary>
        /// Creates a IxxxExpandArgumentOperation object.
        /// </summary>
        protected virtual IWriteableExpandArgumentOperation CreateExpandArgumentOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableInsertionNewBlockNodeIndex blockIndex, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableExpandArgumentOperation(inner, blockIndex, isNested);
        }

        /// <summary>
        /// Creates a IxxxGenericRefreshOperation object.
        /// </summary>
        protected virtual IWriteableGenericRefreshOperation CreateGenericRefreshOperation(IWriteableNodeState refreshState, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableGenericRefreshOperation(refreshState, isNested);
        }
        #endregion
    }
}
