﻿namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Guid = System.Guid;
    using BaseNode;
    using BaseNodeHelper;
    using NotNullReflection;

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports read-only access only.
    /// </summary>
    internal interface IReadOnlyControllerInternal : IEqualComparable
    {
        /// <summary>
        /// Attach a controller view.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to call when enumerating existing states.</param>
        void Attach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet);

        /// <summary>
        /// Detach a controller view.
        /// </summary>
        /// <param name="view">The detaching view.</param>
        /// <param name="callbackSet">The set of callbacks to no longer call when enumerating existing states.</param>
        void Detach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet);
    }

    /// <inheritdoc/>
    public class ReadOnlyController : IReadOnlyControllerInternal, IEqualComparable
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="ReadOnlyController"/> object.
        /// </summary>
        public static ReadOnlyController Empty { get; } = new ReadOnlyController();

        /// <summary>
        /// Creates and initializes a new instance of a <see cref="ReadOnlyController"/> object.
        /// </summary>
        /// <param name="nodeIndex">Index of the root of the node tree.</param>
        public static ReadOnlyController Create(IReadOnlyRootNodeIndex nodeIndex)
        {
            ReadOnlyController Controller = new ReadOnlyController();
            Controller.SetRoot(nodeIndex);
            Controller.SetInitialized();

            return Controller;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyController"/> class.
        /// </summary>
        private protected ReadOnlyController()
        {
            IsInitialized = false;
            _StateTable = CreateStateTable();
            StateTable = _StateTable.ToReadOnly();
            Stats = new Stats();
        }

        private protected bool IsInitialized { get; private set; }
        #endregion

        #region Properties
        /// <summary>
        /// Index of the root node.
        /// </summary>
        public IReadOnlyRootNodeIndex RootIndex { get; private set; }

        /// <summary>
        /// State of the root node.
        /// </summary>
        public IReadOnlyPlaceholderNodeState RootState { get; private set; }

        /// <summary>
        /// Stats for debugging and test purpose.
        /// </summary>
        public Stats Stats { get; }

        /// <summary>
        /// Called when a state is created.
        /// </summary>
        public event System.Action<IReadOnlyNodeState> NodeStateCreated
        {
            add { AddNodeStateCreatedDelegate(value); }
            remove { RemoveNodeStateCreatedDelegate(value); }
        }
#pragma warning disable 1591
        private System.Action<IReadOnlyNodeState> NodeStateCreatedHandler;
        private protected virtual void AddNodeStateCreatedDelegate(System.Action<IReadOnlyNodeState> handler) { NodeStateCreatedHandler += handler; }
        private protected virtual void RemoveNodeStateCreatedDelegate(System.Action<IReadOnlyNodeState> handler) { NodeStateCreatedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a state is initialized.
        /// </summary>
        public event System.Action<IReadOnlyNodeState> NodeStateInitialized
        {
            add { AddNodeStateInitializedDelegate(value); }
            remove { RemoveNodeStateInitializedDelegate(value); }
        }
#pragma warning disable 1591
        private System.Action<IReadOnlyNodeState> NodeStateInitializedHandler;
        private protected virtual void AddNodeStateInitializedDelegate(System.Action<IReadOnlyNodeState> handler) { NodeStateInitializedHandler += handler; }
        private protected virtual void RemoveNodeStateInitializedDelegate(System.Action<IReadOnlyNodeState> handler) { NodeStateInitializedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        public event System.Action<IReadOnlyNodeState> NodeStateRemoved
        {
            add { AddNodeStateRemovedDelegate(value); }
            remove { RemoveNodeStateRemovedDelegate(value); }
        }
#pragma warning disable 1591
        private System.Action<IReadOnlyNodeState> NodeStateRemovedHandler;
        private protected virtual void AddNodeStateRemovedDelegate(System.Action<IReadOnlyNodeState> handler) { NodeStateRemovedHandler += handler; }
        private protected virtual void RemoveNodeStateRemovedDelegate(System.Action<IReadOnlyNodeState> handler) { NodeStateRemovedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a block list inner is created.
        /// </summary>
        public event System.Action<IReadOnlyBlockListInner> BlockListInnerCreated
        {
            add { AddBlockListInnerCreatedDelegate(value); }
            remove { RemoveBlockListInnerCreatedDelegate(value); }
        }
#pragma warning disable 1591
        private System.Action<IReadOnlyBlockListInner> BlockListInnerCreatedHandler;
        private protected virtual void AddBlockListInnerCreatedDelegate(System.Action<IReadOnlyBlockListInner> handler) { BlockListInnerCreatedHandler += handler; }
        private protected virtual void RemoveBlockListInnerCreatedDelegate(System.Action<IReadOnlyBlockListInner> handler) { BlockListInnerCreatedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// State table.
        /// </summary>
        public ReadOnlyNodeStateReadOnlyDictionary StateTable { get; }
        private ReadOnlyNodeStateDictionary _StateTable;
#pragma warning disable 1591
        [Conditional("DEBUG")]
        public void DebugGetStateTable() { DebugObjects.AddReference(_StateTable); }
#pragma warning restore 1591
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks if an index corresponds to a state.
        /// </summary>
        /// <param name="index">The index to check</param>
        public virtual bool Contains(IReadOnlyIndex index)
        {
            return StateTable.ContainsKey(index);
        }

        /// <summary>
        /// Gets the state corresponding to a given index.
        /// </summary>
        /// <param name="index">The index.</param>
        public virtual IReadOnlyNodeState IndexToState(IReadOnlyIndex index)
        {
            Debug.Assert(Contains(index));

            return StateTable[index];
        }

        /// <inheritdoc/>
        public virtual void Attach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet)
        {
            ((IReadOnlyPlaceholderNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)RootState).Attach(view, callbackSet);
        }

        /// <inheritdoc/>
        public virtual void Detach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet)
        {
            ((IReadOnlyPlaceholderNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)RootState).Detach(view, callbackSet);
        }

        /// <summary>
        /// Returns the assigned state of an optional node.
        /// </summary>
        /// <param name="index">Index of the node.</param>
        public virtual bool IsAssigned(IReadOnlyBrowsingOptionalNodeIndex index)
        {
            Debug.Assert(Contains(index));

            return index.Optional.IsAssigned;
        }

        /// <summary>
        /// Returns the value of an enum or boolean.
        /// </summary>
        /// <param name="index">Index of the node.</param>
        /// <param name="propertyName">Name of the property to read.</param>
        /// <param name="minValue">Minimum valid value for this property upon return.</param>
        /// <param name="maxValue">Maximum valid value for this property upon return.</param>
        public virtual int GetDiscreteValue(IReadOnlyIndex index, string propertyName, out int minValue, out int maxValue)
        {
            Debug.Assert(Contains(index));

            IReadOnlyNodeState State = StateTable[index];
            Debug.Assert(State.ValuePropertyTypeTable.ContainsKey(propertyName));
            Debug.Assert(State.ValuePropertyTypeTable[propertyName] == Constants.ValuePropertyType.Boolean || State.ValuePropertyTypeTable[propertyName] == Constants.ValuePropertyType.Enum);

            return NodeTreeHelper.GetEnumValueAndRange(State.Node, propertyName, out minValue, out maxValue);
        }

        /// <summary>
        /// Returns the value of a string.
        /// </summary>
        /// <param name="index">Index of the node.</param>
        /// <param name="propertyName">Name of the property to read.</param>
        public virtual string GetStringValue(IReadOnlyIndex index, string propertyName)
        {
            Debug.Assert(Contains(index));

            IReadOnlyNodeState State = StateTable[index];
            Debug.Assert(State.ValuePropertyTypeTable.ContainsKey(propertyName));
            Debug.Assert(State.ValuePropertyTypeTable[propertyName] == Constants.ValuePropertyType.String);

            string Value = NodeTreeHelper.GetString(State.Node, propertyName);

            return Value;
        }

        /// <summary>
        /// Returns the value of a guid.
        /// </summary>
        /// <param name="index">Index of the node.</param>
        /// <param name="propertyName">Name of the property to read.</param>
        public virtual Guid GetGuidValue(IReadOnlyIndex index, string propertyName)
        {
            Debug.Assert(Contains(index));

            IReadOnlyNodeState State = StateTable[index];
            Debug.Assert(State.ValuePropertyTypeTable.ContainsKey(propertyName));
            Debug.Assert(State.ValuePropertyTypeTable[propertyName] == Constants.ValuePropertyType.Guid);

            Guid Value = NodeTreeHelper.GetGuid(State.Node, propertyName);

            return Value;
        }

        /// <summary>
        /// Checks if a state is the child of another. This method returns true if <paramref name="parentState"/> and <paramref name="state"/> are the same.
        /// </summary>
        /// <param name="parentState">The parent state.</param>
        /// <param name="state">The state to check.</param>
        /// <param name="firstIndex">The first index in the chain leading from the parent to the child. Null if they are the same.</param>
        /// <returns>True if <paramref name="parentState"/> is <paramref name="state"/> or a parent; Otherwise, false.</returns>
        public virtual bool IsChildState(IReadOnlyNodeState parentState, IReadOnlyNodeState state, out IReadOnlyIndex firstIndex)
        {
            firstIndex = null;

            while (state != null)
            {
                if (state == parentState)
                    return true;

                if (state is IReadOnlyPatternState AsPatternState)
                {
                    IReadOnlyBlockState BlockState = AsPatternState.ParentBlockState;
                    Debug.Assert(BlockState.StateList.Count > 0);
                    IReadOnlyPlaceholderNodeState FirstState = BlockState.StateList[0];
                    firstIndex = FirstState.ParentIndex;
                }
                else if (state is IReadOnlySourceState AsSourceState)
                {
                    IReadOnlyBlockState BlockState = AsSourceState.ParentBlockState;
                    Debug.Assert(BlockState.StateList.Count > 0);
                    IReadOnlyPlaceholderNodeState FirstState = BlockState.StateList[0];
                    firstIndex = FirstState.ParentIndex;
                }
                else
                    firstIndex = state.ParentIndex;

                state = state.ParentState;
            }

            return false;
        }
        #endregion

        #region Descendant Interface
        private protected virtual void NotifyNodeStateCreated(IReadOnlyNodeState state)
        {
            NodeStateCreatedHandler?.Invoke(state);
        }

        private protected virtual void NotifyNodeStateInitialized(IReadOnlyNodeState state)
        {
            NodeStateInitializedHandler?.Invoke(state);
        }

        private protected virtual void NotifyNodeStateRemoved(IReadOnlyNodeState state)
        {
            NodeStateRemovedHandler?.Invoke(state);
        }

        private protected virtual void NotifyBlockListInnerCreated(IReadOnlyBlockListInner inner)
        {
            BlockListInnerCreatedHandler?.Invoke(inner);
        }
        #endregion

        #region Implementation
        private protected virtual void SetRoot(IReadOnlyRootNodeIndex rootIndex)
        {
            Debug.Assert(!IsInitialized); // Must be called during initialization

            IReadOnlyPlaceholderNodeState State = CreateRootNodeState(rootIndex);
            RootIndex = rootIndex;
            RootState = State;
            AddState(RootIndex, State);

            // Recursively build all states, starting from the root.
            BuildStateTable(null, null, rootIndex, State);
        }

        private protected virtual void SetInitialized()
        {
            Debug.Assert(!IsInitialized); // Must be called during initialization

            IsInitialized = true;
            CheckInvariant();

            DebugGetStateTable();
        }

        private protected virtual void AddState(IReadOnlyIndex index, IReadOnlyNodeState state)
        {
            Debug.Assert(!StateTable.ContainsKey(index));

            _StateTable.Add(index, state);
            Stats.NodeCount++;

            NotifyNodeStateCreated(StateTable[index]);

            Debug.Assert(Stats.NodeCount == StateTable.Count);
        }

        private protected virtual void RemoveState(IReadOnlyIndex index)
        {
            Debug.Assert(StateTable.ContainsKey(index));

            NotifyNodeStateRemoved(StateTable[index]);

            Stats.NodeCount--;
            _StateTable.Remove(index);

            Debug.Assert(Stats.NodeCount == StateTable.Count);
        }

        private protected virtual void BuildStateTable(IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner, ReadOnlyBrowseContext parentBrowseContext, IReadOnlyIndex nodeIndex, IReadOnlyNodeState state)
        {
            Debug.Assert((parentBrowseContext == null) || (parentBrowseContext != null && parentInner != null));
            Debug.Assert(Contains(nodeIndex));
            Debug.Assert(IndexToState(nodeIndex) == state);

            // Browse the uninitialized state for children
            ReadOnlyBrowseContext BrowseContext = CreateBrowseContext(parentBrowseContext, state);
            BrowseStateChildren(BrowseContext, parentInner);

            // Build inners for each child
            ReadOnlyInnerReadOnlyDictionary<string> InnerTable = BuildInnerTable(BrowseContext);

            // Initialize the state
            InitState(BrowseContext, parentInner, nodeIndex, InnerTable);

            // Build uninitialized states for each child
            ReadOnlyNodeStateDictionary ChildrenStateTable = BuildChildrenStateTable(BrowseContext);

            // Continue to build the table for each of them
            BuildChildrenStates(BrowseContext, ChildrenStateTable);
        }

        private protected virtual void BrowseStateChildren(ReadOnlyBrowseContext browseContext, IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner)
        {
            Debug.Assert(browseContext.IndexCollectionList.Count == 0);

            IReadOnlyNodeState State = browseContext.State;
            ((IReadOnlyNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)State).BrowseChildren(browseContext, parentInner);

            CheckContextConsistency(browseContext);
        }

        private protected virtual void CheckContextConsistency(ReadOnlyBrowseContext browseContext)
        {
            ((ReadOnlyBrowseContext)browseContext).CheckConsistency();
        }

        private protected virtual ReadOnlyInnerReadOnlyDictionary<string> BuildInnerTable(ReadOnlyBrowseContext browseContext)
        {
            IReadOnlyNodeState State = browseContext.State;
            Debug.Assert(State.InnerTable == null);

            ReadOnlyIndexCollectionReadOnlyList IndexCollectionList = browseContext.IndexCollectionList;
            ReadOnlyInnerDictionary<string> InnerTable = CreateInnerTable();

            foreach (IReadOnlyIndexCollection NodeIndexCollection in IndexCollectionList)
            {
                string PropertyName = NodeIndexCollection.PropertyName;
                Debug.Assert(!InnerTable.ContainsKey(PropertyName));

                IReadOnlyInner Inner = BuildInner(State, NodeIndexCollection);
                InnerTable.Add(PropertyName, Inner);
            }

            if (InnerTable.Count > 0)
                DebugObjects.AddReference(InnerTable);

            return InnerTable.ToReadOnly();
        }

        private protected virtual IReadOnlyInner BuildInner(IReadOnlyNodeState parentState, IReadOnlyIndexCollection nodeIndexCollection)
        {
            IReadOnlyInner Result = null;

            switch (nodeIndexCollection)
            {
                case IReadOnlyIndexCollection<IReadOnlyBrowsingPlaceholderNodeIndex> AsPlaceholderNodeIndexCollection:
                    Result = (IReadOnlyPlaceholderInner)CreatePlaceholderInner(parentState, AsPlaceholderNodeIndexCollection);
                    break;

                case IReadOnlyIndexCollection<IReadOnlyBrowsingOptionalNodeIndex> AsOptionalNodeIndexCollection:
                    Result = (IReadOnlyOptionalInner)CreateOptionalInner(parentState, AsOptionalNodeIndexCollection);
                    break;

                case IReadOnlyIndexCollection<IReadOnlyBrowsingListNodeIndex> AsListNodeIndexCollection:
                    Stats.ListCount++;
                    Result = (IReadOnlyListInner)CreateListInner(parentState, AsListNodeIndexCollection);
                    break;

                case IReadOnlyIndexCollection<IReadOnlyBrowsingBlockNodeIndex> AsBlockNodeIndexCollection:
                    Stats.BlockListCount++;
                    IReadOnlyBlockListInner Inner = (IReadOnlyBlockListInner)CreateBlockListInner(parentState, AsBlockNodeIndexCollection);
                    NotifyBlockListInnerCreated(Inner as IReadOnlyBlockListInner);
                    Result = Inner;
                    break;
            }

            Debug.Assert(Result != null);
            return Result;
        }

        private protected virtual void InitState(ReadOnlyBrowseContext browseContext, IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner, IReadOnlyIndex nodeIndex, ReadOnlyInnerReadOnlyDictionary<string> innerTable)
        {
            Debug.Assert(Contains(nodeIndex));
            Debug.Assert(parentInner != null || nodeIndex == RootIndex);

            IReadOnlyNodeState State = browseContext.State;
            Debug.Assert(IndexToState(nodeIndex) == State);
            Debug.Assert(State.ParentInner == null);
            Debug.Assert(State.ParentIndex == nodeIndex);
            Debug.Assert(State.ParentState == null);
            Debug.Assert(State.InnerTable == null);
            Debug.Assert(State.IsEmptyValuePropertyTypeTable || State.ValuePropertyTypeTable.Count == 0);

            ((IReadOnlyNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)State).Init(parentInner, innerTable, browseContext.ValuePropertyTypeTable);

            NotifyNodeStateInitialized(State);
        }

        private protected virtual ReadOnlyNodeStateDictionary BuildChildrenStateTable(ReadOnlyBrowseContext browseContext)
        {
            IReadOnlyNodeState State = browseContext.State;
            ReadOnlyInnerReadOnlyDictionary<string> InnerTable = State.InnerTable;
            ReadOnlyIndexCollectionReadOnlyList IndexCollectionList = browseContext.IndexCollectionList;

            ReadOnlyNodeStateDictionary ChildStateTable = CreateChildStateTable();

            foreach (IReadOnlyIndexCollection<IReadOnlyBrowsingChildIndex> NodeIndexCollection in IndexCollectionList)
            {
                // List of indexes for this property (one for placeholder and optional node, several for lists and block lists)
                IReadOnlyList<IReadOnlyBrowsingChildIndex> NodeIndexList = NodeIndexCollection.NodeIndexList;
                string PropertyName = NodeIndexCollection.PropertyName;

                Debug.Assert(InnerTable.ContainsKey(PropertyName));
                IReadOnlyInner<IReadOnlyBrowsingChildIndex> Inner = (IReadOnlyInner<IReadOnlyBrowsingChildIndex>)InnerTable[PropertyName];

                for (int i = 0; i < NodeIndexList.Count; i++)
                {
                    IReadOnlyBrowsingChildIndex ChildIndex = NodeIndexList[i];

                    // If the inner is that of a block list, and the index is for the first node in the block, add block-specific states
                    if (Inner is IReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex> AsBlockListInner && ChildIndex is IReadOnlyBrowsingNewBlockNodeIndex AsNewBlockIndex)
                    {
                        IReadOnlyBlockState BlockState = AsBlockListInner.InitNewBlock(AsNewBlockIndex);
                        ((IReadOnlyBlockState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)BlockState).InitBlockState();
                        Stats.BlockCount++;

                        IReadOnlyBrowsingPatternIndex PatternIndex = BlockState.PatternIndex;
                        IReadOnlyPatternState PatternState = BlockState.PatternState;
                        AddState(PatternIndex, PatternState);
                        Stats.PlaceholderNodeCount++;

                        IReadOnlyBrowsingSourceIndex SourceIndex = BlockState.SourceIndex;
                        IReadOnlySourceState SourceState = BlockState.SourceState;
                        AddState(SourceIndex, SourceState);
                        Stats.PlaceholderNodeCount++;

                        ChildIndex = AsNewBlockIndex.ToExistingBlockIndex();
                    }

                    IReadOnlyNodeState ChildState = BuildChildState(Inner, ChildIndex);
                    ChildStateTable.Add(NodeIndexList[i], ChildState);
                }
            }

            return ChildStateTable;
        }

        private protected virtual IReadOnlyNodeState BuildChildState(IReadOnlyInner<IReadOnlyBrowsingChildIndex> inner, IReadOnlyBrowsingChildIndex nodeIndex)
        {
            IReadOnlyNodeState ChildState = inner.InitChildState(nodeIndex);
            AddState(nodeIndex, ChildState);

            // For debugging: count nodes according to their type
            if (inner is IReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex> AsOptionalInner)
            {
                Stats.OptionalNodeCount++;
                if (AsOptionalInner.IsAssigned)
                    Stats.AssignedOptionalNodeCount++;
            }
            else
                Stats.PlaceholderNodeCount++;

            return ChildState;
        }

        private protected virtual void BuildChildrenStates(ReadOnlyBrowseContext browseContext, ReadOnlyNodeStateDictionary childrenStateTable)
        {
            IReadOnlyNodeState State = browseContext.State;
            ReadOnlyInnerReadOnlyDictionary<string> InnerTable = State.InnerTable;
            ReadOnlyIndexCollectionReadOnlyList IndexCollectionList = browseContext.IndexCollectionList;

            // Build states for children in the order they have been reported when browsing the parent state
            foreach (IReadOnlyIndexCollection<IReadOnlyBrowsingChildIndex> NodeIndexCollection in IndexCollectionList)
            {
                IReadOnlyList<IReadOnlyBrowsingChildIndex> NodeIndexList = NodeIndexCollection.NodeIndexList;
                string PropertyName = NodeIndexCollection.PropertyName;
                IReadOnlyInner<IReadOnlyBrowsingChildIndex> Inner = (IReadOnlyInner<IReadOnlyBrowsingChildIndex>)InnerTable[PropertyName];

                for (int i = 0; i < NodeIndexList.Count; i++)
                {
                    IReadOnlyBrowsingChildIndex ChildNodeIndex = NodeIndexList[i];
                    IReadOnlyNodeState ChildState = childrenStateTable[ChildNodeIndex];

                    BuildStateTable(Inner, browseContext, ChildState.ParentIndex, ChildState);
                }
            }
        }

        private protected virtual IReadOnlyNodeState GetState(Node node)
        {
            IReadOnlyNodeState Result = null;

            foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> Entry in StateTable)
            {
                IReadOnlyNodeState State = Entry.Value;

                if (State.Node == node)
                {
                    Result = State;
                    break;
                }
            }

            Debug.Assert(Result != null);
            return Result;
        }

        private protected virtual IReadOnlyInner<IReadOnlyBrowsingChildIndex> GetInner(Node parentNode, string propertyName)
        {
            IReadOnlyInner<IReadOnlyBrowsingChildIndex> Result = null;

            foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> Entry in StateTable)
            {
                IReadOnlyNodeState State = Entry.Value;

                if (State.Node == parentNode)
                {
                    Result = State.PropertyToInner(propertyName) as IReadOnlyInner<IReadOnlyBrowsingChildIndex>;
                    break;
                }
            }

            Debug.Assert(Result != null);
            return Result;
        }
        #endregion

        #region Invariant
        private protected virtual void CheckInvariant()
        {
            InvariantAssert(IsInitialized);
        }

        private protected void InvariantAssert(bool condition)
        {
            Debug.Assert(condition);
        }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out ReadOnlyController AsController))
                return comparer.Failed();

            if (!IsEqualFields(comparer, AsController))
                return false;

            List<IReadOnlyIndex> OtherTable = new List<IReadOnlyIndex>();
            foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> Entry in AsController.StateTable)
                OtherTable.Add(Entry.Key);

            if (!IsEqualTable(comparer, OtherTable, out IDictionary<IReadOnlyIndex, IReadOnlyIndex> MatchTable))
                return false;

            if (!IsEqualMatchTable(comparer, AsController, MatchTable))
                return false;

            return true;
        }

        private bool IsEqualFields(CompareEqual comparer, ReadOnlyController other)
        {
            if (!comparer.VerifyEqual(RootIndex, other.RootIndex))
                return comparer.Failed();

            if (!comparer.VerifyEqual(RootState, other.RootState))
                return comparer.Failed();

            if (!comparer.VerifyEqual(Stats, other.Stats))
                return comparer.Failed();

            if (!comparer.IsSameCount(StateTable.Count, other.StateTable.Count))
                return comparer.Failed();

            return true;
        }

        private bool IsEqualTable(CompareEqual comparer, List<IReadOnlyIndex> otherTable, out IDictionary<IReadOnlyIndex, IReadOnlyIndex> matchTable)
        {
            matchTable = new Dictionary<IReadOnlyIndex, IReadOnlyIndex>();

            CompareEqual MatchComparer = CompareEqual.New(true);
            foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> Entry in StateTable)
            {
                MatchComparer.Reset();

                bool Found = false;
                for (int i = 0; i < otherTable.Count; i++)
                    if (MatchComparer.VerifyEqual((IEqualComparable)Entry.Key, (IEqualComparable)otherTable[i]))
                    {
                        matchTable.Add(Entry.Key, otherTable[i]);
                        otherTable.RemoveAt(i);
                        Found = true;
                        break;
                    }

                if (!comparer.IsTrue(Found))
                    return comparer.Failed();
            }

            return true;
        }

        private bool IsEqualMatchTable(CompareEqual comparer, ReadOnlyController other, IDictionary<IReadOnlyIndex, IReadOnlyIndex> matchTable)
        {
            foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyIndex> Entry in matchTable)
            {
                IReadOnlyNodeState State = StateTable[Entry.Key];
                IReadOnlyNodeState OtherState = other.StateTable[Entry.Value];

                if (!comparer.VerifyEqual(State, OtherState))
                    return comparer.Failed();
            }

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        private protected virtual ReadOnlyNodeStateDictionary CreateStateTable()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<ReadOnlyController>());
            return new ReadOnlyNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxInnerDictionary{string} object.
        /// </summary>
        private protected virtual ReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<ReadOnlyController>());
            return new ReadOnlyInnerDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        private protected virtual ReadOnlyNodeStateDictionary CreateChildStateTable()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<ReadOnlyController>());
            return new ReadOnlyNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxxBrowseContext object.
        /// </summary>
        private protected virtual ReadOnlyBrowseContext CreateBrowseContext(ReadOnlyBrowseContext parentBrowseContext, IReadOnlyNodeState state)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<ReadOnlyController>());
            return new ReadOnlyBrowseContext(state);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        private protected virtual IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreatePlaceholderInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingPlaceholderNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<ReadOnlyController>());
            return new ReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex>(owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxOptionalInner{IxxxBrowsingOptionalNodeIndex} object.
        /// </summary>
        private protected virtual IReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex> CreateOptionalInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingOptionalNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<ReadOnlyController>());
            return new ReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex>(owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxListInner{IxxxBrowsingListNodeIndex} object.
        /// </summary>
        private protected virtual IReadOnlyListInner<IReadOnlyBrowsingListNodeIndex> CreateListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingListNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<ReadOnlyController>());
            return new ReadOnlyListInner<IReadOnlyBrowsingListNodeIndex>(owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxBlockListInner{IxxxBrowsingBlockNodeIndex} object.
        /// </summary>
        private protected virtual IReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex> CreateBlockListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingBlockNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<ReadOnlyController>());
            return new ReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex>(owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected virtual IReadOnlyPlaceholderNodeState CreateRootNodeState(IReadOnlyRootNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<ReadOnlyController>());
            return new ReadOnlyPlaceholderNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>(nodeIndex);
        }
        #endregion
    }
}
