using BaseNode;
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
        new event Action<IWriteableNodeState> StateCreated;

        /// <summary>
        /// Called when a state is initialized.
        /// </summary>
        new event Action<IWriteableNodeState> StateInitialized;

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        new event Action<IWriteableNodeState> StateRemoved;

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
        public new event Action<IWriteableNodeState> StateCreated
        {
            add { AddStateCreatedDelegate((Action<IReadOnlyNodeState>)value); }
            remove { RemoveStateCreatedDelegate((Action<IReadOnlyNodeState>)value); }
        }

        /// <summary>
        /// Called when a state is initialized.
        /// </summary>
        public new event Action<IWriteableNodeState> StateInitialized
        {
            add { AddStateInitializedDelegate((Action<IReadOnlyNodeState>)value); }
            remove { RemoveStateInitializedDelegate((Action<IReadOnlyNodeState>)value); }
        }

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        public new event Action<IWriteableNodeState> StateRemoved
        {
            add { AddStateRemovedDelegate((Action<IReadOnlyNodeState>)value); }
            remove { RemoveStateRemovedDelegate((Action<IReadOnlyNodeState>)value); }
        }

        /// <summary>
        /// State table.
        /// </summary>
        public new IWriteableIndexNodeStateReadOnlyDictionary StateTable { get { return (IWriteableIndexNodeStateReadOnlyDictionary)base.StateTable; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Inserts a new node in a list or block list.
        /// </summary>
        /// <param name="inner">The inner for the list or block list where the node is inserted.</param>
        /// <param name="insertedIndex">Index for the insertion operation.</param>
        /// <param name="nodeIndex">Index of the inserted node upon return.</param>
        public void Insert(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableInsertionCollectionNodeIndex insertedIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex)
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

            IWriteablePlaceholderNodeState ChildState;

            if ((inner is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner) && (insertedIndex is IWriteableInsertionNewBlockNodeIndex AsNewBlockIndex))
            {
                AsBlockListInner.InsertNew(AsNewBlockIndex, out nodeIndex, out IWriteableBlockState BlockState, out ChildState);
                Debug.Assert(BlockState.StateList.Count == 1);
                Debug.Assert(BlockState.StateList[0] == ChildState);
                BlockState.InitBlockState();

                IReadOnlyBrowsingPatternIndex PatternIndex = BlockState.PatternIndex;
                IReadOnlyPatternState PatternState = BlockState.PatternState;
                AddState(PatternIndex, PatternState);
                Stats.PlaceholderNodeCount++;

                IReadOnlyBrowsingSourceIndex SourceIndex = BlockState.SourceIndex;
                IReadOnlySourceState SourceState = BlockState.SourceState;
                AddState(SourceIndex, SourceState);
                Stats.PlaceholderNodeCount++;
            }
            else
                inner.Insert(insertedIndex, out nodeIndex, out ChildState);

            AddState(nodeIndex, ChildState);
            Stats.PlaceholderNodeCount++;
            BuildStateTable(inner, null, nodeIndex, ChildState);

            Debug.Assert(Contains(nodeIndex));
        }

        /// <summary>
        /// Removes a node from a list or block list.
        /// </summary>
        /// <param name="inner">The inner for the list or block list from which the node is removed.</param>
        /// <param name="nodeIndex">Index for the removed node.</param>
        public void Remove(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex)
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

            IWriteableNodeState OldState;

            if ((inner is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner) && (nodeIndex is IWriteableBrowsingBlockNodeIndex AsBlockIndex))
            {
                AsBlockListInner.RemoveWithBlock(AsBlockIndex, out bool IsBlockRemoved, out IWriteableBrowsingPatternIndex PatternIndex, out IWriteableBrowsingSourceIndex SourceIndex);

                if (IsBlockRemoved)
                {
                    Debug.Assert(PatternIndex != null);
                    Debug.Assert(StateTable.ContainsKey(PatternIndex));
                    Debug.Assert(SourceIndex != null);
                    Debug.Assert(StateTable.ContainsKey(SourceIndex));

                    RemoveState(PatternIndex);
                    Stats.PlaceholderNodeCount--;

                    RemoveState(SourceIndex);
                    Stats.PlaceholderNodeCount--;
                }
            }
            else
                inner.Remove(nodeIndex);

            OldState = StateTable[nodeIndex];
            RemoveState(nodeIndex);
            Stats.PlaceholderNodeCount--;

            PruneStateTable(OldState);
        }

        /// <summary>
        /// Replace an existing node with a new one.
        /// </summary>
        /// <param name="inner">The inner where the node is replaced.</param>
        /// <param name="insertedIndex">Index for the replace operation.</param>
        /// <param name="nodeIndex">Index of the replacing node upon return.</param>
        public void Replace(IWriteableInner<IWriteableBrowsingChildIndex> inner, IWriteableInsertionChildIndex insertedIndex, out IWriteableBrowsingChildIndex nodeIndex)
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

            inner.Replace(insertedIndex, out IWriteableBrowsingChildIndex OldBrowsingIndex, out IWriteableBrowsingChildIndex NewBrowsingIndex, out IWriteableNodeState ChildState);

            Debug.Assert(Contains(OldBrowsingIndex));
            IWriteableNodeState OldState = StateTable[OldBrowsingIndex];

            RemoveState(OldBrowsingIndex);
            PruneStateTable(OldState);
            AddState(NewBrowsingIndex, ChildState);

            BuildStateTable(inner, null, NewBrowsingIndex, ChildState);

            nodeIndex = NewBrowsingIndex;
            Debug.Assert(Contains(nodeIndex));
        }

        protected virtual void PruneStateTable(IWriteableNodeState parentState)
        {
            List<IWriteableNodeState> ToRemove = new List<IWriteableNodeState>();

            foreach (KeyValuePair<IWriteableIndex, IWriteableNodeState> Entry in StateTable)
            {
                IWriteableNodeState State = Entry.Value;
                if (State.ParentState == parentState)
                    ToRemove.Add(State);
            }

            foreach (KeyValuePair<string, IWriteableInner<IWriteableBrowsingChildIndex>> Entry in parentState.InnerTable)
                if (Entry.Value is IWriteablePlaceholderInner AsPlaceholderInner)
                    Stats.PlaceholderNodeCount--;
                else if (Entry.Value is IWriteableOptionalInner AsOptionalInner)
                {
                    Stats.OptionalNodeCount--;
                    if (AsOptionalInner.IsAssigned)
                        Stats.AssignedOptionalNodeCount--;
                }
                else if (Entry.Value is IWriteableListInner AsListInner)
                {
                    Stats.PlaceholderNodeCount -= AsListInner.StateList.Count;
                    Stats.ListCount--;
                }
                else if (Entry.Value is IWriteableBlockListInner AsBlockListInner)
                {
                    for (int i = 0; i < AsBlockListInner.BlockStateList.Count; i++)
                        Stats.PlaceholderNodeCount -= (2 + AsBlockListInner.BlockStateList[i].StateList.Count);
                    Stats.BlockListCount--;
                }
                else
                    Debug.Assert(false);

            foreach (IWriteableNodeState State in ToRemove)
            {
                RemoveState(State.ParentIndex);
                PruneStateTable(State);
            }
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

            int OldNodeCount = OldBlockState.StateList.Count;

            inner.SplitBlock(nodeIndex, out IWriteableBlockState NewBlockState);

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

            IWriteableBlockState FirstBlockState = inner.BlockStateList[nodeIndex.BlockIndex - 1];
            IWriteableBlockState SecondBlockState = inner.BlockStateList[nodeIndex.BlockIndex];

            IReadOnlyBrowsingSourceIndex SourceIndex = FirstBlockState.SourceIndex;
            RemoveState(SourceIndex);
            Stats.PlaceholderNodeCount--;

            IReadOnlyBrowsingPatternIndex PatternIndex = FirstBlockState.PatternIndex;
            RemoveState(PatternIndex);
            Stats.PlaceholderNodeCount--;

            int OldNodeCount = FirstBlockState.StateList.Count + SecondBlockState.StateList.Count;
            int FirstNodeIndex = FirstBlockState.StateList.Count;

            inner.MergeBlocks(nodeIndex);

            IWriteableBlockState BlockState = inner.BlockStateList[nodeIndex.BlockIndex];

            Debug.Assert(BlockState.StateList.Count == OldNodeCount);
            Debug.Assert(FirstNodeIndex < BlockState.StateList.Count);
            Debug.Assert(BlockState.StateList[FirstNodeIndex].ParentIndex == nodeIndex);
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

            inner.MoveBlock(blockIndex, direction);
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

            if (optionalInner.ChildState.ParentIndex.Optional.HasItem)
            {
                optionalInner.Assign();
                Stats.AssignedOptionalNodeCount++;
                return;
            }

            INode NewNode = NodeHelper.CreateDefaultFromInterface(optionalInner.InterfaceType);
            Debug.Assert(NewNode != null);

            IWriteableInsertionOptionalNodeIndex NewOptionalNodeIndex = CreateNewOptionalNodeIndex(optionalInner.Owner.Node, optionalInner.PropertyName, NewNode);
            optionalInner.Replace(NewOptionalNodeIndex, out IWriteableBrowsingChildIndex OldBrowsingIndex, out IWriteableBrowsingChildIndex NewBrowsingIndex, out IWriteableNodeState ChildState);

            Debug.Assert(Contains(OldBrowsingIndex));
            IWriteableNodeState OldState = StateTable[OldBrowsingIndex];

            RemoveState(OldBrowsingIndex); // No need to call PruneStateTable
            AddState(NewBrowsingIndex, ChildState);
            Stats.AssignedOptionalNodeCount++;

            BuildStateTable(optionalInner, null, NewBrowsingIndex, ChildState);
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

            blockListInner.InsertNew(NewBlockNodeIndex, out IWriteableBrowsingCollectionNodeIndex ArgumentNodeIndex, out IWriteableBlockState BlockState, out IWriteablePlaceholderNodeState ArgumentChildState);
            Debug.Assert(BlockState.StateList.Count == 1);
            Debug.Assert(BlockState.StateList[0] == ArgumentChildState);
            BlockState.InitBlockState();

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
            IOptionalReference Optional = optionalInner.ChildState.ParentIndex.Optional;

            if (NodeHelper.IsOptionalAssignedToDefault(Optional))
            {
                Optional.Unassign();
                Stats.AssignedOptionalNodeCount--;
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

            IWriteableBrowsingBlockNodeIndex FirstNodeIndex = FirstState.ParentIndex as IWriteableBrowsingBlockNodeIndex;
            Debug.Assert(FirstNodeIndex != null);

            blockListInner.RemoveWithBlock(FirstNodeIndex, out bool IsBlockRemoved, out IWriteableBrowsingPatternIndex PatternIndex, out IWriteableBrowsingSourceIndex SourceIndex);
            Debug.Assert(IsBlockRemoved);

            Debug.Assert(PatternIndex != null);
            Debug.Assert(StateTable.ContainsKey(PatternIndex));
            Debug.Assert(SourceIndex != null);
            Debug.Assert(StateTable.ContainsKey(SourceIndex));

            RemoveState(PatternIndex);
            Stats.PlaceholderNodeCount--;

            RemoveState(SourceIndex);
            Stats.PlaceholderNodeCount--;

            Debug.Assert(FirstState == StateTable[FirstNodeIndex]);
            RemoveState(FirstNodeIndex);
            Stats.PlaceholderNodeCount--;

            PruneStateTable(FirstState);
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

            Reduce(NodeIndex);
            CanonicalizeChildren(state);
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
            return new WriteableBrowseContext(state);
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
        #endregion
    }
}
