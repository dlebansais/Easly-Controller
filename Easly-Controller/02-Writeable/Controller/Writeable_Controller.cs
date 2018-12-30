using BaseNode;
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
        /// Removes a node from a list of block list.
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
        /// <param name="inner">The inner where the node is assigned.</param>
        void Assign(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> inner);

        /// <summary>
        /// Unassign the optional node.
        /// </summary>
        /// <param name="inner">The inner where the node is unassigned.</param>
        void Unassign(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> inner);

        /// <summary>
        /// Changes the replication state of a block.
        /// </summary>
        /// <param name="inner">The inner where the blok is changed.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="replication">New replication value.</param>
        void ChangeReplication(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex, ReplicationStatus replication);

        /// <summary>
        /// Splits a block in two at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block is split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        void SplitBlock(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Merges two blocks at the given index.
        /// </summary>
        /// <param name="inner">The inner where blocks are merged.</param>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        void MergeBlocks(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex);
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
        protected new IWriteableIndexNodeStateReadOnlyDictionary StateTable { get { return (IWriteableIndexNodeStateReadOnlyDictionary)base.StateTable; } }
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
            BuildStateTable(inner, null, nodeIndex, ChildState);

            Debug.Assert(Contains(nodeIndex));
        }

        /// <summary>
        /// Removes a node from a list of block list.
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

            foreach (IWriteableNodeState State in ToRemove)
            {
                RemoveState(State.ParentIndex);
                PruneStateTable(State);
            }
        }

        /// <summary>
        /// Assign the optional node.
        /// </summary>
        /// <param name="inner">The inner where the node is assigned.</param>
        public virtual void Assign(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> inner)
        {
            Debug.Assert(inner != null);

            inner.Assign();
        }

        /// <summary>
        /// Unassign the optional node.
        /// </summary>
        /// <param name="inner">The inner where the node is unassigned.</param>
        public virtual void Unassign(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> inner)
        {
            Debug.Assert(inner != null);

            inner.Unassign();
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
        /// Splits a block in two at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block is split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        public virtual void SplitBlock(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);
            Debug.Assert(nodeIndex.BlockIndex >= 0 && nodeIndex.BlockIndex < inner.BlockStateList.Count);
            Debug.Assert(nodeIndex.Index > 0);

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
        /// Merges two blocks at the given index.
        /// </summary>
        /// <param name="inner">The inner where blocks are merged.</param>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        public virtual void MergeBlocks(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);
            Debug.Assert(nodeIndex.BlockIndex > 0 && nodeIndex.BlockIndex < inner.BlockStateList.Count);
            Debug.Assert(nodeIndex.Index == 0);

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
        #endregion
    }
}
