﻿using BaseNode;
using BaseNodeHelper;
using Easly;
using EaslyController.ReadOnly;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.Writeable
{
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
        event Action<IWriteableBrowsingOptionalNodeIndex, IWriteableOptionalNodeState> StateAssigned;

        /// <summary>
        /// Called when a state is unassigned.
        /// </summary>
        event Action<IWriteableBrowsingOptionalNodeIndex, IWriteableOptionalNodeState> StateUnassigned;

        /// <summary>
        /// Called when a state is moved.
        /// </summary>
        event Action<IWriteableBrowsingChildIndex, IWriteableNodeState, int> StateMoved;

        /// <summary>
        /// Called when a block state is moved.
        /// </summary>
        event Action<IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>, int, int> BlockStateMoved;

        /// <summary>
        /// Called when a block is split.
        /// </summary>
        event Action<IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>, int, int> BlockSplit;

        /// <summary>
        /// Called when two blocks are merged.
        /// </summary>
        event Action<IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>, int> BlocksMerged;

        /// <summary>
        /// Called when an argument block is expanded.
        /// </summary>
        event Action<IWriteableExpandArgumentOperation> ArgumentExpanded;

        /// <summary>
        /// Inserts a new node in a list or block list.
        /// </summary>
        /// <param name="inner">The inner for the list or block list where the node is inserted.</param>
        /// <param name="insertedIndex">Index for the insertion operation.</param>
        /// <param name="nodeIndex">Index of the inserted node upon return.</param>
        void Insert(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableInsertionCollectionNodeIndex insertedIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex);

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
        protected Action<IWriteableInsertBlockOperation> BlockStateInsertedHandler;
        protected virtual void AddBlockStateInsertedDelegate(Action<IWriteableInsertBlockOperation> handler) { BlockStateInsertedHandler += handler; }
        protected virtual void RemoveBlockStateInsertedDelegate(Action<IWriteableInsertBlockOperation> handler) { BlockStateInsertedHandler -= handler; }

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        public event Action<IWriteableRemoveBlockOperation> BlockStateRemoved
        {
            add { AddBlockStateRemovedDelegate(value); }
            remove { RemoveBlockStateRemovedDelegate(value); }
        }
        protected Action<IWriteableRemoveBlockOperation> BlockStateRemovedHandler;
        protected virtual void AddBlockStateRemovedDelegate(Action<IWriteableRemoveBlockOperation> handler) { BlockStateRemovedHandler += handler; }
        protected virtual void RemoveBlockStateRemovedDelegate(Action<IWriteableRemoveBlockOperation> handler) { BlockStateRemovedHandler -= handler; }

        /// <summary>
        /// Called when a state is inserted.
        /// </summary>
        public event Action<IWriteableInsertNodeOperation> StateInserted
        {
            add { AddStateInsertedDelegate(value); }
            remove { RemoveStateInsertedDelegate(value); }
        }
        protected Action<IWriteableInsertNodeOperation> StateInsertedHandler;
        protected virtual void AddStateInsertedDelegate(Action<IWriteableInsertNodeOperation> handler) { StateInsertedHandler += handler; }
        protected virtual void RemoveStateInsertedDelegate(Action<IWriteableInsertNodeOperation> handler) { StateInsertedHandler -= handler; }

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        public event Action<IWriteableRemoveNodeOperation> StateRemoved
        {
            add { AddStateRemovedDelegate(value); }
            remove { RemoveStateRemovedDelegate(value); }
        }
        protected Action<IWriteableRemoveNodeOperation> StateRemovedHandler;
        protected virtual void AddStateRemovedDelegate(Action<IWriteableRemoveNodeOperation> handler) { StateRemovedHandler += handler; }
        protected virtual void RemoveStateRemovedDelegate(Action<IWriteableRemoveNodeOperation> handler) { StateRemovedHandler -= handler; }

        /// <summary>
        /// Called when a state is replaced.
        /// </summary>
        public event Action<IWriteableReplaceOperation> StateReplaced
        {
            add { AddStateReplacedDelegate(value); }
            remove { RemoveStateReplacedDelegate(value); }
        }
        protected Action<IWriteableReplaceOperation> StateReplacedHandler;
        protected virtual void AddStateReplacedDelegate(Action<IWriteableReplaceOperation> handler) { StateReplacedHandler += handler; }
        protected virtual void RemoveStateReplacedDelegate(Action<IWriteableReplaceOperation> handler) { StateReplacedHandler -= handler; }

        /// <summary>
        /// Called when a state is assigned.
        /// </summary>
        public event Action<IWriteableBrowsingOptionalNodeIndex, IWriteableOptionalNodeState> StateAssigned
        {
            add { AddStateAssignedDelegate(value); }
            remove { RemoveStateAssignedDelegate(value); }
        }
        protected Action<IWriteableBrowsingOptionalNodeIndex, IWriteableOptionalNodeState> StateAssignedHandler;
        protected virtual void AddStateAssignedDelegate(Action<IWriteableBrowsingOptionalNodeIndex, IWriteableOptionalNodeState> handler) { StateAssignedHandler += handler; }
        protected virtual void RemoveStateAssignedDelegate(Action<IWriteableBrowsingOptionalNodeIndex, IWriteableOptionalNodeState> handler) { StateAssignedHandler -= handler; }

        /// <summary>
        /// Called when a state is unassigned.
        /// </summary>
        public event Action<IWriteableBrowsingOptionalNodeIndex, IWriteableOptionalNodeState> StateUnassigned
        {
            add { AddStateUnassignedDelegate(value); }
            remove { RemoveStateUnassignedDelegate(value); }
        }
        protected Action<IWriteableBrowsingOptionalNodeIndex, IWriteableOptionalNodeState> StateUnassignedHandler;
        protected virtual void AddStateUnassignedDelegate(Action<IWriteableBrowsingOptionalNodeIndex, IWriteableOptionalNodeState> handler) { StateUnassignedHandler += handler; }
        protected virtual void RemoveStateUnassignedDelegate(Action<IWriteableBrowsingOptionalNodeIndex, IWriteableOptionalNodeState> handler) { StateUnassignedHandler -= handler; }

        /// <summary>
        /// Called when a state is moved.
        /// </summary>
        public event Action<IWriteableBrowsingChildIndex, IWriteableNodeState, int> StateMoved
        {
            add { AddStateMovedDelegate(value); }
            remove { RemoveStateMovedDelegate(value); }
        }
        protected Action<IWriteableBrowsingChildIndex, IWriteableNodeState, int> StateMovedHandler;
        protected virtual void AddStateMovedDelegate(Action<IWriteableBrowsingChildIndex, IWriteableNodeState, int> handler) { StateMovedHandler += handler; }
        protected virtual void RemoveStateMovedDelegate(Action<IWriteableBrowsingChildIndex, IWriteableNodeState, int> handler) { StateMovedHandler -= handler; }

        /// <summary>
        /// Called when a block state is moved.
        /// </summary>
        public event Action<IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>, int, int> BlockStateMoved
        {
            add { AddBlockStateMovedDelegate(value); }
            remove { RemoveBlockStateMovedDelegate(value); }
        }
        protected Action<IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>, int, int> BlockStateMovedHandler;
        protected virtual void AddBlockStateMovedDelegate(Action<IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>, int, int> handler) { BlockStateMovedHandler += handler; }
        protected virtual void RemoveBlockStateMovedDelegate(Action<IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>, int, int> handler) { BlockStateMovedHandler -= handler; }

        /// <summary>
        /// Called when a block is split.
        /// </summary>
        public event Action<IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>, int, int> BlockSplit
        {
            add { AddBlockSplitDelegate(value); }
            remove { RemoveBlockSplitDelegate(value); }
        }
        protected Action<IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>, int, int> BlockSplitHandler;
        protected virtual void AddBlockSplitDelegate(Action<IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>, int, int> handler) { BlockSplitHandler += handler; }
        protected virtual void RemoveBlockSplitDelegate(Action<IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>, int, int> handler) { BlockSplitHandler -= handler; }

        /// <summary>
        /// Called when two blocks are merged.
        /// </summary>
        public event Action<IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>, int> BlocksMerged
        {
            add { AddBlocksMergedDelegate(value); }
            remove { RemoveBlocksMergedDelegate(value); }
        }
        protected Action<IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>, int> BlocksMergedHandler;
        protected virtual void AddBlocksMergedDelegate(Action<IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>, int> handler) { BlocksMergedHandler += handler; }
        protected virtual void RemoveBlocksMergedDelegate(Action<IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>, int> handler) { BlocksMergedHandler -= handler; }

        /// <summary>
        /// Called when an argument block is expanded.
        /// </summary>
        public event Action<IWriteableExpandArgumentOperation> ArgumentExpanded
        {
            add { AddArgumentExpandedDelegate(value); }
            remove { RemoveArgumentExpandedDelegate(value); }
        }
        protected Action<IWriteableExpandArgumentOperation> ArgumentExpandedHandler;
        protected virtual void AddArgumentExpandedDelegate(Action<IWriteableExpandArgumentOperation> handler) { ArgumentExpandedHandler += handler; }
        protected virtual void RemoveArgumentExpandedDelegate(Action<IWriteableExpandArgumentOperation> handler) { ArgumentExpandedHandler -= handler; }

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
        }

        protected virtual void InsertNewBlock(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> blockListInner, IWriteableInsertionNewBlockNodeIndex newBlockIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            IWriteableInsertBlockOperation Operation = CreateInsertBlockOperation(blockListInner, newBlockIndex.BlockIndex);

            blockListInner.InsertNew(Operation, newBlockIndex);

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

        protected virtual void InsertNewNode(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableInsertionCollectionNodeIndex insertedIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            IWriteableInsertNodeOperation Operation = CreateInsertNodeOperation(inner, insertedIndex);

            inner.Insert(Operation, insertedIndex);

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
        }

        protected virtual void RemoveNodeFromBlock(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> blockListInner, IWriteableBrowsingExistingBlockNodeIndex blockIndex)
        {
            IWriteableRemoveBlockOperation BlockOperation = CreateRemoveBlockOperation(blockListInner, blockIndex);
            IWriteableRemoveNodeOperation NodeOperation = CreateRemoveNodeOperation(blockListInner, blockIndex);

            blockListInner.RemoveWithBlock(BlockOperation, NodeOperation, blockIndex);
            IWriteableBlockState RemovedBlockState = BlockOperation.BlockState;

            if (RemovedBlockState != null)
                RemoveLastBlock(BlockOperation, blockIndex);
            else
            {
                IWriteableNodeState OldState = StateTable[blockIndex];
                PruneState(OldState);
                Stats.PlaceholderNodeCount--;

                NotifyStateRemoved(NodeOperation);
            }
        }

        protected virtual void RemoveLastBlock(IWriteableRemoveBlockOperation blockOperation, IWriteableBrowsingExistingBlockNodeIndex blockIndex)
        {
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

            IWriteableNodeState RemovedChildState = StateTable[blockIndex];
            PruneState(RemovedChildState);
            Stats.PlaceholderNodeCount--;

            NotifyBlockStateRemoved(blockOperation);
        }

        protected virtual void RemoveNode(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            IWriteableRemoveNodeOperation NodeOperation = CreateRemoveNodeOperation(inner, nodeIndex);

            inner.Remove(NodeOperation, nodeIndex);

            IWriteableNodeState OldState = StateTable[nodeIndex];
            PruneState(OldState);
            Stats.PlaceholderNodeCount--;

            NotifyStateRemoved(NodeOperation);
        }

        /// <summary>
        /// Replace an existing node with a new one.
        /// </summary>
        /// <param name="inner">The inner where the node is replaced.</param>
        /// <param name="insertedIndex">Index for the replace operation.</param>
        /// <param name="nodeIndex">Index of the replacing node upon return.</param>
        public void Replace(IWriteableInner<IWriteableBrowsingChildIndex> inner, IWriteableInsertionChildIndex replacementIndex, out IWriteableBrowsingChildIndex nodeIndex)
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

            IWriteableReplaceOperation Operation = CreateReplaceOperation(inner, replacementIndex);
            inner.Replace(Operation, replacementIndex, out IWriteableBrowsingChildIndex OldBrowsingIndex, out IWriteableBrowsingChildIndex NewBrowsingIndex, out IWriteableNodeState ChildState);

            Debug.Assert(Contains(OldBrowsingIndex));
            IWriteableNodeState OldState = StateTable[OldBrowsingIndex];

            PruneState(OldState);
            AddState(NewBrowsingIndex, ChildState);

            BuildStateTable(inner, null, NewBrowsingIndex, ChildState);

            nodeIndex = NewBrowsingIndex;
            Debug.Assert(Contains(nodeIndex));

            NotifyStateReplaced(Operation);
        }

        protected virtual void PruneState(IWriteableNodeState state)
        {
            foreach (KeyValuePair<string, IWriteableInner<IWriteableBrowsingChildIndex>> Entry in state.InnerTable)
                if (Entry.Value is IWriteablePlaceholderInner<IWriteableBrowsingPlaceholderNodeIndex> AsPlaceholderInner)
                    PrunePlaceholderInner(AsPlaceholderInner);
                else if (Entry.Value is IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> AsOptionalInner)
                    PruneOptionalInner(AsOptionalInner);
                else if (Entry.Value is IWriteableListInner<IWriteableBrowsingListNodeIndex> AsListInner)
                    PruneListInner(AsListInner);
                else if (Entry.Value is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner)
                    PruneBlockListInner(AsBlockListInner);
                else
                    Debug.Assert(false);

            RemoveState(state.ParentIndex);
        }

        protected virtual void PrunePlaceholderInner(IWriteablePlaceholderInner<IWriteableBrowsingPlaceholderNodeIndex> inner)
        {
            PruneState(inner.ChildState);

            Stats.PlaceholderNodeCount--;
        }

        protected virtual void PruneOptionalInner(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> inner)
        {
            PruneState(inner.ChildState);

            Stats.OptionalNodeCount--;
            if (inner.IsAssigned)
                Stats.AssignedOptionalNodeCount--;
        }

        protected virtual void PruneListInner(IWriteableListInner<IWriteableBrowsingListNodeIndex> inner)
        {
            foreach (IWriteableNodeState State in inner.StateList)
            {
                PruneState(State);

                Stats.PlaceholderNodeCount--;
            }

            Stats.ListCount--;
        }

        protected virtual void PruneBlockListInner(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner)
        {
            while (inner.BlockStateList.Count > 0)
            {
                IWriteableBlockState BlockState = inner.BlockStateList[0];
                IWriteableRemoveBlockOperation BlockOperation = null;

                do
                {
                    Debug.Assert(BlockState.StateList.Count > 0);
                    IWriteableNodeState FirstNodeState = BlockState.StateList[0];
                    IWriteableBrowsingExistingBlockNodeIndex FirstNodeIndex = FirstNodeState.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
                    Debug.Assert(FirstNodeIndex != null);

                    BlockOperation = CreateRemoveBlockOperation(inner, FirstNodeIndex);

                    IWriteableRemoveNodeOperation NodeOperation = CreateRemoveNodeOperation(inner, FirstNodeIndex);

                    inner.RemoveWithBlock(BlockOperation, NodeOperation, FirstNodeIndex);
                    PruneState(FirstNodeState);

                    Stats.PlaceholderNodeCount--;
                }
                while (BlockOperation.BlockState == null);

                Debug.Assert(BlockOperation != null);
                IWriteableBlockState RemovedBlockState = BlockOperation.BlockState;
                Debug.Assert(RemovedBlockState != null);

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

                Stats.BlockCount--;

                NotifyBlockStateRemoved(BlockOperation);
            }

            Stats.BlockListCount--;
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
                Inner.Assign();
                Stats.AssignedOptionalNodeCount++;

                NotifyStateAssigned(nodeIndex, State);
            }
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
                Inner.Unassign();
                Stats.AssignedOptionalNodeCount--;

                NotifyStateUnassigned(nodeIndex, State);
            }
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

            int BlockIndex = nodeIndex.BlockIndex;
            int Index = nodeIndex.Index;
            int OldNodeCount = OldBlockState.StateList.Count;

            inner.SplitBlock(nodeIndex, out IWriteableBlockState NewBlockState);
            Stats.BlockCount++;

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

            NotifyBlockSplit(inner, BlockIndex, Index);
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

            IReadOnlyBrowsingSourceIndex SourceIndex = FirstBlockState.SourceIndex;
            RemoveState(SourceIndex);
            Stats.PlaceholderNodeCount--;

            IReadOnlyBrowsingPatternIndex PatternIndex = FirstBlockState.PatternIndex;
            RemoveState(PatternIndex);
            Stats.PlaceholderNodeCount--;

            int OldNodeCount = FirstBlockState.StateList.Count + SecondBlockState.StateList.Count;
            int FirstNodeIndex = FirstBlockState.StateList.Count;

            inner.MergeBlocks(nodeIndex);
            Stats.BlockCount--;

            IWriteableBlockState BlockState = inner.BlockStateList[BlockIndex - 1];

            Debug.Assert(BlockState.StateList.Count == OldNodeCount);
            Debug.Assert(FirstNodeIndex < BlockState.StateList.Count);
            Debug.Assert(BlockState.StateList[FirstNodeIndex].ParentIndex == nodeIndex);

            NotifyBlocksMerged(inner, BlockIndex);
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

            NotifyStateMoved(nodeIndex, State, direction);

            inner.Move(nodeIndex, direction);
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

            inner.MoveBlock(blockIndex, direction);

            NotifyBlockStateMoved(inner, blockIndex, direction);
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
                optionalInner.Assign();
                Stats.AssignedOptionalNodeCount++;

                NotifyStateAssigned(ParentIndex, optionalInner.ChildState);
                return;
            }

            INode NewNode = NodeHelper.CreateDefaultFromInterface(optionalInner.InterfaceType);
            Debug.Assert(NewNode != null);

            IWriteableInsertionOptionalNodeIndex NewOptionalNodeIndex = CreateNewOptionalNodeIndex(optionalInner.Owner.Node, optionalInner.PropertyName, NewNode);
            IWriteableReplaceOperation Operation = CreateReplaceOperation(optionalInner, NewOptionalNodeIndex);

            optionalInner.Replace(Operation, NewOptionalNodeIndex, out IWriteableBrowsingChildIndex OldBrowsingIndex, out IWriteableBrowsingChildIndex NewBrowsingIndex, out IWriteableNodeState ChildState);

            Debug.Assert(Contains(OldBrowsingIndex));
            IWriteableNodeState OldState = StateTable[OldBrowsingIndex];

            PruneState(OldState);
            AddState(NewBrowsingIndex, ChildState);
            Stats.AssignedOptionalNodeCount++;

            BuildStateTable(optionalInner, null, NewBrowsingIndex, ChildState);

            NotifyStateReplaced(Operation);
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

            IWriteableExpandArgumentOperation Operation = CreateExpandArgumentOperation(blockListInner, NewBlockNodeIndex.BlockIndex);

            blockListInner.InsertNew(Operation, NewBlockNodeIndex);

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

            IWriteablePlaceholderNodeState State = StateTable[reducedIndex] as IWriteablePlaceholderNodeState;
            Debug.Assert(State != null);

            IWriteableInnerReadOnlyDictionary<string> InnerTable = State.InnerTable;

            foreach (KeyValuePair<string, IWriteableInner<IWriteableBrowsingChildIndex>> Entry in InnerTable)
            {
                if (Entry.Value is IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> AsOptionalInner)
                    ReduceOptional(AsOptionalInner);

                else if (Entry.Value is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner)
                    ReduceBlockList(AsBlockListInner);
            }
        }

        /// <summary>
        /// Reduces the optional node.
        /// </summary>
        protected virtual void ReduceOptional(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> optionalInner)
        {
            if (optionalInner.IsAssigned)
            {
                optionalInner.Unassign();
                Stats.AssignedOptionalNodeCount--;

                IWriteableBrowsingOptionalNodeIndex ParentIndex = optionalInner.ChildState.ParentIndex;
                NotifyStateUnassigned(ParentIndex, optionalInner.ChildState);
            }
        }

        /// <summary>
        /// Reduces the block list.
        /// </summary>
        protected virtual void ReduceBlockList(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> blockListInner)
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
            PruneState(FirstState);
            Stats.PlaceholderNodeCount--;

            IWriteableRemoveBlockOperation BlockOperation = CreateRemoveBlockOperation(blockListInner, FirstNodeIndex);

            blockListInner.RemoveWithBlock(BlockOperation, null, FirstNodeIndex);

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
        }

        protected virtual void Canonicalize(IWriteableNodeState state)
        {
            IWriteableNodeIndex NodeIndex = state.ParentIndex as IWriteableNodeIndex;
            Debug.Assert(NodeIndex != null);

            CanonicalizeChildren(state);
            Reduce(NodeIndex);
        }

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
        protected virtual void NotifyBlockStateInserted(IWriteableInsertBlockOperation operation)
        {
            BlockStateInsertedHandler?.Invoke(operation);
        }

        protected virtual void NotifyBlockStateRemoved(IWriteableRemoveBlockOperation operation)
        {
            BlockStateRemovedHandler?.Invoke(operation);
        }

        protected virtual void NotifyStateInserted(IWriteableInsertNodeOperation operation)
        {
            StateInsertedHandler?.Invoke(operation);
        }

        protected virtual void NotifyStateRemoved(IWriteableRemoveNodeOperation operation)
        {
            StateRemovedHandler?.Invoke(operation);
        }

        protected virtual void NotifyStateReplaced(IWriteableReplaceOperation operation)
        {
            StateReplacedHandler?.Invoke(operation);
        }

        protected virtual void NotifyStateAssigned(IWriteableBrowsingOptionalNodeIndex nodeIndex, IWriteableOptionalNodeState state)
        {
            StateAssignedHandler?.Invoke(nodeIndex, state);
        }

        protected virtual void NotifyStateUnassigned(IWriteableBrowsingOptionalNodeIndex nodeIndex, IWriteableOptionalNodeState state)
        {
            StateUnassignedHandler?.Invoke(nodeIndex, state);
        }

        protected virtual void NotifyStateMoved(IWriteableBrowsingChildIndex nodeIndex, IWriteableNodeState state, int direction)
        {
            StateMovedHandler?.Invoke(nodeIndex, state, direction);
        }

        protected virtual void NotifyBlockStateMoved(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex, int direction)
        {
            BlockStateMovedHandler?.Invoke(inner, blockIndex, direction);
        }

        protected virtual void NotifyBlockSplit(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex, int index)
        {
            BlockSplitHandler?.Invoke(inner, blockIndex, index);
        }

        protected virtual void NotifyBlocksMerged(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex)
        {
            BlocksMergedHandler?.Invoke(inner, blockIndex);
        }

        protected virtual void NotifyArgumentExpanded(IWriteableExpandArgumentOperation operation)
        {
            ArgumentExpandedHandler?.Invoke(operation);
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
        protected virtual IWriteableInsertNodeOperation CreateInsertNodeOperation(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableInsertionCollectionNodeIndex insertionIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableInsertNodeOperation(inner, insertionIndex);
        }

        /// <summary>
        /// Creates a IxxxInsertBlockOperation object.
        /// </summary>
        protected virtual IWriteableInsertBlockOperation CreateInsertBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableInsertBlockOperation(inner, blockIndex);
        }

        /// <summary>
        /// Creates a IxxxRemoveBlockOperation object.
        /// </summary>
        protected virtual IWriteableRemoveBlockOperation CreateRemoveBlockOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex blockIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableRemoveBlockOperation(inner, blockIndex);
        }

        /// <summary>
        /// Creates a IxxxRemoveNodeOperation object.
        /// </summary>
        protected virtual IWriteableRemoveNodeOperation CreateRemoveNodeOperation(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableRemoveNodeOperation(inner, nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxReplaceOperation object.
        /// </summary>
        protected virtual IWriteableReplaceOperation CreateReplaceOperation(IWriteableInner<IWriteableBrowsingChildIndex> inner, IWriteableInsertionChildIndex replacementIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableReplaceOperation(inner, replacementIndex);
        }

        /// <summary>
        /// Creates a IxxxExpandArgumentOperation object.
        /// </summary>
        protected virtual IWriteableExpandArgumentOperation CreateExpandArgumentOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableExpandArgumentOperation(inner, blockIndex);
        }
        #endregion
    }
}
