﻿namespace EaslyController.ReadOnly
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports read-only access only.
    /// </summary>
    public interface IReadOnlyController : IEqualComparable
    {
        /// <summary>
        /// Index of the root node.
        /// </summary>
        IReadOnlyRootNodeIndex RootIndex { get; }

        /// <summary>
        /// State of the root node.
        /// </summary>
        IReadOnlyPlaceholderNodeState RootState { get; }

        /// <summary>
        /// State table.
        /// </summary>
        IReadOnlyIndexNodeStateReadOnlyDictionary StateTable { get; }

        /// <summary>
        /// Stats for debugging and test purpose.
        /// </summary>
        Stats Stats { get; }

        /// <summary>
        /// Called when a state is created.
        /// </summary>
        event Action<IReadOnlyNodeState> NodeStateCreated;

        /// <summary>
        /// Called when a state is initialized.
        /// </summary>
        event Action<IReadOnlyNodeState> NodeStateInitialized;

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        event Action<IReadOnlyNodeState> NodeStateRemoved;

        /// <summary>
        /// Called when a block list inner is created.
        /// </summary>
        event Action<IReadOnlyBlockListInner> BlockListInnerCreated;

        /// <summary>
        /// Checks if an index corresponds to a state.
        /// </summary>
        /// <param name="index">The index to check</param>
        bool Contains(IReadOnlyIndex index);

        /// <summary>
        /// Gets the state corresponding to a given index.
        /// </summary>
        /// <param name="index">The index.</param>
        IReadOnlyNodeState IndexToState(IReadOnlyIndex index);

        /// <summary>
        /// Returns the assigned state of an optional node.
        /// </summary>
        /// <param name="index">Index of the node.</param>
        bool IsAssigned(IReadOnlyBrowsingOptionalNodeIndex index);

        /// <summary>
        /// Returns the value of an enum or boolean.
        /// </summary>
        /// <param name="index">Index of the node.</param>
        /// <param name="propertyName">Name of the property to read.</param>
        /// <param name="minValue">Minimum valid value for this property upon return.</param>
        /// <param name="maxValue">Maximum valid value for this property upon return.</param>
        int GetDiscreteValue(IReadOnlyIndex index, string propertyName, out int minValue, out int maxValue);

        /// <summary>
        /// Returns the value of a string.
        /// </summary>
        /// <param name="index">Index of the node.</param>
        /// <param name="propertyName">Name of the property to read.</param>
        string GetStringValue(IReadOnlyIndex index, string propertyName);

        /// <summary>
        /// Returns the value of a guid.
        /// </summary>
        /// <param name="index">Index of the node.</param>
        /// <param name="propertyName">Name of the property to read.</param>
        Guid GetGuidValue(IReadOnlyIndex index, string propertyName);

        /// <summary>
        /// Checks if a state is the child of another. This method returns true if <paramref name="parentState"/> and <paramref name="state"/> are the same.
        /// </summary>
        /// <param name="parentState">The parent state.</param>
        /// <param name="state">The state to check.</param>
        /// <param name="firstIndex">The first index in the chain leading from the parent to the child. Null if they are the same.</param>
        /// <returns>True if <paramref name="parentState"/> is <paramref name="state"/> or a parent; Otherwise, false.</returns>
        bool IsChildState(IReadOnlyNodeState parentState, IReadOnlyNodeState state, out IReadOnlyIndex firstIndex);
    }

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
        void Attach(IReadOnlyControllerView view, IReadOnlyAttachCallbackSet callbackSet);

        /// <summary>
        /// Detach a controller view.
        /// </summary>
        /// <param name="view">The detaching view.</param>
        /// <param name="callbackSet">The set of callbacks to no longer call when enumerating existing states.</param>
        void Detach(IReadOnlyControllerView view, IReadOnlyAttachCallbackSet callbackSet);
    }

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports read-only access only.
    /// </summary>
    public class ReadOnlyController : IReadOnlyControllerInternal, IReadOnlyController
    {
        #region Init
        /// <summary>
        /// Creates and initializes a new instance of a <see cref="ReadOnlyController"/> object.
        /// </summary>
        /// <param name="nodeIndex">Index of the root of the node tree.</param>
        public static IReadOnlyController Create(IReadOnlyRootNodeIndex nodeIndex)
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
        public event Action<IReadOnlyNodeState> NodeStateCreated
        {
            add { AddNodeStateCreatedDelegate(value); }
            remove { RemoveNodeStateCreatedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IReadOnlyNodeState> NodeStateCreatedHandler;
        private protected virtual void AddNodeStateCreatedDelegate(Action<IReadOnlyNodeState> handler) { NodeStateCreatedHandler += handler; }
        private protected virtual void RemoveNodeStateCreatedDelegate(Action<IReadOnlyNodeState> handler) { NodeStateCreatedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a state is initialized.
        /// </summary>
        public event Action<IReadOnlyNodeState> NodeStateInitialized
        {
            add { AddNodeStateInitializedDelegate(value); }
            remove { RemoveNodeStateInitializedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IReadOnlyNodeState> NodeStateInitializedHandler;
        private protected virtual void AddNodeStateInitializedDelegate(Action<IReadOnlyNodeState> handler) { NodeStateInitializedHandler += handler; }
        private protected virtual void RemoveNodeStateInitializedDelegate(Action<IReadOnlyNodeState> handler) { NodeStateInitializedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        public event Action<IReadOnlyNodeState> NodeStateRemoved
        {
            add { AddNodeStateRemovedDelegate(value); }
            remove { RemoveNodeStateRemovedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IReadOnlyNodeState> NodeStateRemovedHandler;
        private protected virtual void AddNodeStateRemovedDelegate(Action<IReadOnlyNodeState> handler) { NodeStateRemovedHandler += handler; }
        private protected virtual void RemoveNodeStateRemovedDelegate(Action<IReadOnlyNodeState> handler) { NodeStateRemovedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a block list inner is created.
        /// </summary>
        public event Action<IReadOnlyBlockListInner> BlockListInnerCreated
        {
            add { AddBlockListInnerCreatedDelegate(value); }
            remove { RemoveBlockListInnerCreatedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IReadOnlyBlockListInner> BlockListInnerCreatedHandler;
        private protected virtual void AddBlockListInnerCreatedDelegate(Action<IReadOnlyBlockListInner> handler) { BlockListInnerCreatedHandler += handler; }
        private protected virtual void RemoveBlockListInnerCreatedDelegate(Action<IReadOnlyBlockListInner> handler) { BlockListInnerCreatedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// State table.
        /// </summary>
        public IReadOnlyIndexNodeStateReadOnlyDictionary StateTable { get; }
        private IReadOnlyIndexNodeStateDictionary _StateTable;
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
            Debug.Assert(index != null);

            return StateTable.ContainsKey(index);
        }

        /// <summary>
        /// Gets the state corresponding to a given index.
        /// </summary>
        /// <param name="index">The index.</param>
        public virtual IReadOnlyNodeState IndexToState(IReadOnlyIndex index)
        {
            Debug.Assert(index != null);
            Debug.Assert(Contains(index));

            return StateTable[index];
        }

        /// <summary>
        /// Attach a controller view.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to call when enumerating existing states.</param>
        public virtual void Attach(IReadOnlyControllerView view, IReadOnlyAttachCallbackSet callbackSet)
        {
            ((IReadOnlyPlaceholderNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)RootState).Attach(view, callbackSet);
        }

        /// <summary>
        /// Detach a controller view.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to no longer call when enumerating existing states.</param>
        public virtual void Detach(IReadOnlyControllerView view, IReadOnlyAttachCallbackSet callbackSet)
        {
            ((IReadOnlyPlaceholderNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)RootState).Detach(view, callbackSet);
        }

        /// <summary>
        /// Returns the assigned state of an optional node.
        /// </summary>
        /// <param name="index">Index of the node.</param>
        public virtual bool IsAssigned(IReadOnlyBrowsingOptionalNodeIndex index)
        {
            Debug.Assert(index != null);
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
            Debug.Assert(index != null);
            Debug.Assert(Contains(index));

            IReadOnlyNodeState State = StateTable[index];
            Debug.Assert(State != null);
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
            Debug.Assert(index != null);
            Debug.Assert(Contains(index));

            IReadOnlyNodeState State = StateTable[index];
            Debug.Assert(State != null);
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
            Debug.Assert(index != null);
            Debug.Assert(Contains(index));

            IReadOnlyNodeState State = StateTable[index];
            Debug.Assert(State != null);
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
            Debug.Assert(rootIndex != null);
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
            Debug.Assert(state != null);
            Debug.Assert(!StateTable.ContainsKey(index));

            _StateTable.Add(index, state);
            Stats.NodeCount++;

            NotifyNodeStateCreated(StateTable[index]);

            Debug.Assert(Stats.NodeCount == StateTable.Count);
        }

        private protected virtual void RemoveState(IReadOnlyIndex index)
        {
            Debug.Assert(index != null);
            Debug.Assert(StateTable.ContainsKey(index));

            NotifyNodeStateRemoved(StateTable[index]);

            Stats.NodeCount--;
            _StateTable.Remove(index);

            Debug.Assert(Stats.NodeCount == StateTable.Count);
        }

        private protected virtual void BuildStateTable(IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner, IReadOnlyBrowseContext parentBrowseContext, IReadOnlyIndex nodeIndex, IReadOnlyNodeState state)
        {
            Debug.Assert((parentBrowseContext == null) || (parentBrowseContext != null && parentInner != null));
            Debug.Assert(nodeIndex != null);
            Debug.Assert(state != null);
            Debug.Assert(Contains(nodeIndex));
            Debug.Assert(IndexToState(nodeIndex) == state);

            // Browse the uninitialized state for children
            IReadOnlyBrowseContext BrowseContext = CreateBrowseContext(parentBrowseContext, state);
            BrowseStateChildren(BrowseContext, parentInner);

            // Build inners for each child
            IReadOnlyInnerReadOnlyDictionary<string> InnerTable = BuildInnerTable(BrowseContext);

            // Initialize the state
            InitState(BrowseContext, parentInner, nodeIndex, InnerTable);

            // Build uninitialized states for each child
            IReadOnlyIndexNodeStateDictionary ChildrenStateTable = BuildChildrenStateTable(BrowseContext);

            // Continue to build the table for each of them
            BuildChildrenStates(BrowseContext, ChildrenStateTable);
        }

        private protected virtual void BrowseStateChildren(IReadOnlyBrowseContext browseContext, IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner)
        {
            Debug.Assert(browseContext != null);
            Debug.Assert(browseContext.IndexCollectionList.Count == 0);
            Debug.Assert(browseContext.ToString() != null); // For code coverage.

            IReadOnlyNodeState State = browseContext.State;
            ((IReadOnlyNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)State).BrowseChildren(browseContext, parentInner);

            CheckContextConsistency(browseContext);
        }

        private protected virtual void CheckContextConsistency(IReadOnlyBrowseContext browseContext)
        {
            ((ReadOnlyBrowseContext)browseContext).CheckConsistency();
        }

        private protected virtual IReadOnlyInnerReadOnlyDictionary<string> BuildInnerTable(IReadOnlyBrowseContext browseContext)
        {
            Debug.Assert(browseContext != null);

            IReadOnlyNodeState State = browseContext.State;
            Debug.Assert(State.InnerTable == null);

            IReadOnlyIndexCollectionReadOnlyList IndexCollectionList = browseContext.IndexCollectionList;
            IReadOnlyInnerDictionary<string> InnerTable = CreateInnerTable();

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
            Debug.Assert(parentState != null);
            Debug.Assert(nodeIndexCollection != null);

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

        private protected virtual void InitState(IReadOnlyBrowseContext browseContext, IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner, IReadOnlyIndex nodeIndex, IReadOnlyInnerReadOnlyDictionary<string> innerTable)
        {
            Debug.Assert(browseContext != null);
            Debug.Assert(nodeIndex != null);
            Debug.Assert(Contains(nodeIndex));
            Debug.Assert(innerTable != null);
            Debug.Assert(parentInner != null || nodeIndex == RootIndex);

            IReadOnlyNodeState State = browseContext.State;
            Debug.Assert(IndexToState(nodeIndex) == State);
            Debug.Assert(State.ParentInner == null);
            Debug.Assert(State.ParentIndex == nodeIndex);
            Debug.Assert(State.ParentState == null);
            Debug.Assert(State.InnerTable == null);
            Debug.Assert(State.ValuePropertyTypeTable == null || State.ValuePropertyTypeTable.Count == 0);

            ((IReadOnlyNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>)State).Init(parentInner, innerTable, browseContext.ValuePropertyTypeTable);
            Debug.Assert(State.ToString() != null); // For code coverage.

            NotifyNodeStateInitialized(State);
        }

        private protected virtual IReadOnlyIndexNodeStateDictionary BuildChildrenStateTable(IReadOnlyBrowseContext browseContext)
        {
            Debug.Assert(browseContext != null);

            IReadOnlyNodeState State = browseContext.State;
            IReadOnlyInnerReadOnlyDictionary<string> InnerTable = State.InnerTable;
            IReadOnlyIndexCollectionReadOnlyList IndexCollectionList = browseContext.IndexCollectionList;

            IReadOnlyIndexNodeStateDictionary ChildStateTable = CreateChildStateTable();

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
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

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

        private protected virtual void BuildChildrenStates(IReadOnlyBrowseContext browseContext, IReadOnlyIndexNodeStateDictionary childrenStateTable)
        {
            Debug.Assert(browseContext != null);
            Debug.Assert(childrenStateTable != null);

            IReadOnlyNodeState State = browseContext.State;
            IReadOnlyInnerReadOnlyDictionary<string> InnerTable = State.InnerTable;
            IReadOnlyIndexCollectionReadOnlyList IndexCollectionList = browseContext.IndexCollectionList;

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

        private protected virtual IReadOnlyNodeState GetState(INode node)
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

        private protected virtual IReadOnlyInner<IReadOnlyBrowsingChildIndex> GetInner(INode parentNode, string propertyName)
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
        /// <summary>
        /// Compares two <see cref="IReadOnlyController"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyController AsController))
                return comparer.Failed();

            if (!comparer.VerifyEqual(RootIndex, AsController.RootIndex))
                return comparer.Failed();

            if (!comparer.VerifyEqual(RootState, AsController.RootState))
                return comparer.Failed();

            if (!comparer.VerifyEqual(Stats, AsController.Stats))
                return comparer.Failed();

            if (!comparer.IsSameCount(StateTable.Count, AsController.StateTable.Count))
                return comparer.Failed();

            List<IReadOnlyIndex> OtherTable = new List<IReadOnlyIndex>();
            foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> Entry in AsController.StateTable)
                OtherTable.Add(Entry.Key);

            CompareEqual MatchComparer = CompareEqual.New(true);
            IDictionary<IReadOnlyIndex, IReadOnlyIndex> MatchTable = new Dictionary<IReadOnlyIndex, IReadOnlyIndex>();
            foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> Entry in StateTable)
            {
                MatchComparer.Reset();

                bool Found = false;
                for (int i = 0; i < OtherTable.Count; i++)
                    if (MatchComparer.VerifyEqual((IEqualComparable)Entry.Key, (IEqualComparable)OtherTable[i]))
                    {
                        MatchTable.Add(Entry.Key, OtherTable[i]);
                        OtherTable.RemoveAt(i);
                        Found = true;
                        break;
                    }

                if (!comparer.IsTrue(Found))
                    return comparer.Failed();
            }

            foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyIndex> Entry in MatchTable)
            {
                IReadOnlyNodeState State = StateTable[Entry.Key];
                IReadOnlyNodeState OtherState = AsController.StateTable[Entry.Value];

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
        private protected virtual IReadOnlyIndexNodeStateDictionary CreateStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyIndexNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxInnerDictionary{string} object.
        /// </summary>
        private protected virtual IReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyInnerDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        private protected virtual IReadOnlyIndexNodeStateDictionary CreateChildStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyIndexNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxxBrowseContext object.
        /// </summary>
        private protected virtual IReadOnlyBrowseContext CreateBrowseContext(IReadOnlyBrowseContext parentBrowseContext, IReadOnlyNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyBrowseContext(state);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        private protected virtual IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreatePlaceholderInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingPlaceholderNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex, ReadOnlyBrowsingPlaceholderNodeIndex>(owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxOptionalInner{IxxxBrowsingOptionalNodeIndex} object.
        /// </summary>
        private protected virtual IReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex> CreateOptionalInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingOptionalNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex, ReadOnlyBrowsingOptionalNodeIndex>(owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxListInner{IxxxBrowsingListNodeIndex} object.
        /// </summary>
        private protected virtual IReadOnlyListInner<IReadOnlyBrowsingListNodeIndex> CreateListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingListNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyListInner<IReadOnlyBrowsingListNodeIndex, ReadOnlyBrowsingListNodeIndex>(owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxBlockListInner{IxxxBrowsingBlockNodeIndex} object.
        /// </summary>
        private protected virtual IReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex> CreateBlockListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingBlockNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex, ReadOnlyBrowsingBlockNodeIndex>(owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected virtual IReadOnlyPlaceholderNodeState CreateRootNodeState(IReadOnlyRootNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyPlaceholderNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>(nodeIndex);
        }
        #endregion
    }
}
