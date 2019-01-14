using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
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
        /// Called when a block list inner is created
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
    public class ReadOnlyController : IReadOnlyController
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
        /// Initializes a new instance of a <see cref="ReadOnlyController"/> object.
        /// </summary>
        protected ReadOnlyController()
        {
            IsInitialized = false;
            _StateTable = CreateStateTable();
            StateTable = CreateStateTableReadOnly(_StateTable);
            Stats = new Stats();
        }

        protected bool IsInitialized { get; private set; }
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
        protected Action<IReadOnlyNodeState> NodeStateCreatedHandler;
        protected virtual void AddNodeStateCreatedDelegate(Action<IReadOnlyNodeState> handler) { NodeStateCreatedHandler += handler; }
        protected virtual void RemoveNodeStateCreatedDelegate(Action<IReadOnlyNodeState> handler) { NodeStateCreatedHandler -= handler; }
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
        protected Action<IReadOnlyNodeState> NodeStateInitializedHandler;
        protected virtual void AddNodeStateInitializedDelegate(Action<IReadOnlyNodeState> handler) { NodeStateInitializedHandler += handler; }
        protected virtual void RemoveNodeStateInitializedDelegate(Action<IReadOnlyNodeState> handler) { NodeStateInitializedHandler -= handler; }
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
        protected Action<IReadOnlyNodeState> NodeStateRemovedHandler;
        protected virtual void AddNodeStateRemovedDelegate(Action<IReadOnlyNodeState> handler) { NodeStateRemovedHandler += handler; }
        protected virtual void RemoveNodeStateRemovedDelegate(Action<IReadOnlyNodeState> handler) { NodeStateRemovedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a block list inner is created
        /// </summary>
        public event Action<IReadOnlyBlockListInner> BlockListInnerCreated
        {
            add { AddBlockListInnerCreatedDelegate(value); }
            remove { RemoveBlockListInnerCreatedDelegate(value); }
        }
#pragma warning disable 1591
        protected Action<IReadOnlyBlockListInner> BlockListInnerCreatedHandler;
        protected virtual void AddBlockListInnerCreatedDelegate(Action<IReadOnlyBlockListInner> handler) { BlockListInnerCreatedHandler += handler; }
        protected virtual void RemoveBlockListInnerCreatedDelegate(Action<IReadOnlyBlockListInner> handler) { BlockListInnerCreatedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// State table.
        /// </summary>
        protected IReadOnlyIndexNodeStateReadOnlyDictionary StateTable { get; }
        private IReadOnlyIndexNodeStateDictionary _StateTable;
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
            RootState.Attach(view, callbackSet);
        }

        /// <summary>
        /// Detach a controller view.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to no longer call when enumerating existing states.</param>
        public virtual void Detach(IReadOnlyControllerView view, IReadOnlyAttachCallbackSet callbackSet)
        {
            RootState.Detach(view, callbackSet);
        }
        #endregion

        #region Descendant Interface
        protected virtual void NotifyNodeStateCreated(IReadOnlyNodeState state)
        {
            NodeStateCreatedHandler?.Invoke(state);
        }

        protected virtual void NotifyNodeStateInitialized(IReadOnlyNodeState state)
        {
            NodeStateInitializedHandler?.Invoke(state);
        }

        protected virtual void NotifyNodeStateRemoved(IReadOnlyNodeState state)
        {
            NodeStateRemovedHandler?.Invoke(state);
        }

        protected virtual void NotifyBlockListInnerCreated(IReadOnlyBlockListInner inner)
        {
            BlockListInnerCreatedHandler?.Invoke(inner);
        }
        #endregion

        #region Implementation
        protected virtual void SetRoot(IReadOnlyRootNodeIndex rootIndex)
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

        protected virtual void SetInitialized()
        {
            Debug.Assert(!IsInitialized); // Must be called during initialization

            IsInitialized = true;
            CheckInvariant();
        }

        protected virtual void AddState(IReadOnlyIndex index, IReadOnlyNodeState state)
        {
            Debug.Assert(state != null);
            Debug.Assert(!StateTable.ContainsKey(index));

            _StateTable.Add(index, state);
            Stats.NodeCount++;

            NotifyNodeStateCreated(StateTable[index]);

            Debug.Assert(Stats.NodeCount == StateTable.Count);
        }

        protected virtual void RemoveState(IReadOnlyIndex index)
        {
            Debug.Assert(index != null);
            Debug.Assert(StateTable.ContainsKey(index));

            NotifyNodeStateRemoved(StateTable[index]);

            Stats.NodeCount--;
            _StateTable.Remove(index);

            Debug.Assert(Stats.NodeCount == StateTable.Count);
        }

        protected virtual void BuildStateTable(IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner, IReadOnlyBrowseContext parentBrowseContext, IReadOnlyIndex nodeIndex, IReadOnlyNodeState state)
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

        protected virtual void BrowseStateChildren(IReadOnlyBrowseContext browseContext, IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner)
        {
            Debug.Assert(browseContext != null);
            Debug.Assert(browseContext.IndexCollectionList.Count == 0);

            IReadOnlyNodeState State = browseContext.State;
            State.BrowseChildren(browseContext, parentInner);
        }

        protected virtual IReadOnlyInnerReadOnlyDictionary<string> BuildInnerTable(IReadOnlyBrowseContext browseContext)
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

                IReadOnlyInner<IReadOnlyBrowsingChildIndex> Inner = BuildInner(State, NodeIndexCollection);
                InnerTable.Add(PropertyName, Inner);
            }

            return CreateInnerTableReadOnly(InnerTable);
        }

        protected virtual IReadOnlyInner<IReadOnlyBrowsingChildIndex> BuildInner(IReadOnlyNodeState parentState, IReadOnlyIndexCollection nodeIndexCollection)
        {
            Debug.Assert(parentState != null);
            Debug.Assert(nodeIndexCollection != null);

            switch (nodeIndexCollection)
            {
                case IReadOnlyIndexCollection<IReadOnlyBrowsingPlaceholderNodeIndex> AsPlaceholderNodeIndexCollection:
                    return CreatePlaceholderInner(parentState, AsPlaceholderNodeIndexCollection);

                case IReadOnlyIndexCollection<IReadOnlyBrowsingOptionalNodeIndex> AsOptionalNodeIndexCollection:
                    return CreateOptionalInner(parentState, AsOptionalNodeIndexCollection);

                case IReadOnlyIndexCollection<IReadOnlyBrowsingListNodeIndex> AsListNodeIndexCollection:
                    Stats.ListCount++;
                    return CreateListInner(parentState, AsListNodeIndexCollection);

                case IReadOnlyIndexCollection<IReadOnlyBrowsingBlockNodeIndex> AsBlockNodeIndexCollection:
                    Stats.BlockListCount++;
                    IReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex> Inner = CreateBlockListInner(parentState, AsBlockNodeIndexCollection);
                    NotifyBlockListInnerCreated(Inner as IReadOnlyBlockListInner);
                    return Inner;

                default:
                    throw new ArgumentOutOfRangeException(nameof(nodeIndexCollection));
            }
        }

        protected virtual void InitState(IReadOnlyBrowseContext browseContext, IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner, IReadOnlyIndex nodeIndex, IReadOnlyInnerReadOnlyDictionary<string> innerTable)
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

            State.Init(parentInner, innerTable, browseContext.ValuePropertyTypeTable);

            NotifyNodeStateInitialized(State);
        }

        protected virtual IReadOnlyIndexNodeStateDictionary BuildChildrenStateTable(IReadOnlyBrowseContext browseContext)
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
                    if ((Inner is IReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex> AsBlockListInner) && (ChildIndex is IReadOnlyBrowsingNewBlockNodeIndex AsNewBlockIndex))
                    {
                        IReadOnlyBlockState BlockState = AsBlockListInner.InitNewBlock(AsNewBlockIndex);
                        BlockState.InitBlockState();
                        Stats.BlockCount++;

                        IReadOnlyBrowsingPatternIndex PatternIndex = BlockState.PatternIndex;
                        IReadOnlyPatternState PatternState = BlockState.PatternState;
                        AddState(PatternIndex , PatternState);
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

        protected virtual IReadOnlyNodeState BuildChildState(IReadOnlyInner<IReadOnlyBrowsingChildIndex> inner, IReadOnlyBrowsingChildIndex nodeIndex)
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

        protected virtual void BuildChildrenStates(IReadOnlyBrowseContext browseContext, IReadOnlyIndexNodeStateDictionary childrenStateTable)
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
                IReadOnlyInner<IReadOnlyBrowsingChildIndex> Inner = InnerTable[PropertyName];

                for (int i = 0; i < NodeIndexList.Count; i++)
                {
                    IReadOnlyBrowsingChildIndex ChildNodeIndex = NodeIndexList[i];
                    IReadOnlyNodeState ChildState = childrenStateTable[ChildNodeIndex];

                    BuildStateTable(Inner, browseContext, ChildState.ParentIndex, ChildState);
                }
            }
        }
        #endregion

        #region Invariant
        protected virtual void CheckInvariant()
        {
            InvariantAssert(IsInitialized);
        }

        protected void InvariantAssert(bool condition)
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

            if (!(other is ReadOnlyController AsController))
                return false;

            if (!comparer.VerifyEqual(RootIndex, AsController.RootIndex))
                return false;

            if (!comparer.VerifyEqual(RootState, AsController.RootState))
                return false;

            if (!comparer.VerifyEqual(Stats, AsController.Stats))
                return false;

            if (StateTable.Count != AsController.StateTable.Count)
                return false;

            List<IReadOnlyIndex> OtherTable = new List<IReadOnlyIndex>();
            foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> Entry in AsController.StateTable)
                OtherTable.Add(Entry.Key);

            CompareEqual MatchComparer = CompareEqual.New();
            Dictionary<IReadOnlyIndex, IReadOnlyIndex> MatchTable = new Dictionary<IReadOnlyIndex, IReadOnlyIndex>();
            foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyNodeState> Entry in StateTable)
            {
                MatchComparer.Reset();

                bool Found = false;
                for (int i = 0; i < OtherTable.Count; i++)
                    if (MatchComparer.VerifyEqual(Entry.Key, OtherTable[i]))
                    {
                        MatchTable.Add(Entry.Key, OtherTable[i]);
                        OtherTable.RemoveAt(i);
                        Found = true;
                        break;
                    }

                if (!Found)
                    return false;
            }

            foreach (KeyValuePair<IReadOnlyIndex, IReadOnlyIndex> Entry in MatchTable)
            {
                IReadOnlyNodeState State = StateTable[Entry.Key];
                IReadOnlyNodeState OtherState = AsController.StateTable[Entry.Value];

                if (!comparer.VerifyEqual(State, OtherState))
                    return false;
            }

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        protected virtual IReadOnlyIndexNodeStateDictionary CreateStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyIndexNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxIndexNodeStateReadOnlyDictionary object.
        /// </summary>
        protected virtual IReadOnlyIndexNodeStateReadOnlyDictionary CreateStateTableReadOnly(IReadOnlyIndexNodeStateDictionary stateTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyIndexNodeStateReadOnlyDictionary(stateTable);
        }

        /// <summary>
        /// Creates a IxxxInnerDictionary{string} object.
        /// </summary>
        protected virtual IReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyInnerDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxInnerReadOnlyDictionary{string} object.
        /// </summary>
        protected virtual IReadOnlyInnerReadOnlyDictionary<string> CreateInnerTableReadOnly(IReadOnlyInnerDictionary<string> innerTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyInnerReadOnlyDictionary<string>(innerTable);
        }

        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        protected virtual IReadOnlyIndexNodeStateDictionary CreateChildStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyIndexNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxxBrowseContext object.
        /// </summary>
        protected virtual IReadOnlyBrowseContext CreateBrowseContext(IReadOnlyBrowseContext parentBrowseContext, IReadOnlyNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyBrowseContext(state);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        protected virtual IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreatePlaceholderInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingPlaceholderNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex, ReadOnlyBrowsingPlaceholderNodeIndex>(owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxOptionalInner{IxxxBrowsingOptionalNodeIndex} object.
        /// </summary>
        protected virtual IReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex> CreateOptionalInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingOptionalNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex, ReadOnlyBrowsingOptionalNodeIndex>(owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxListInner{IxxxBrowsingListNodeIndex} object.
        /// </summary>
        protected virtual IReadOnlyListInner<IReadOnlyBrowsingListNodeIndex> CreateListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingListNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyListInner<IReadOnlyBrowsingListNodeIndex, ReadOnlyBrowsingListNodeIndex>(owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxBlockListInner{IxxxBrowsingBlockNodeIndex} object.
        /// </summary>
        protected virtual IReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex> CreateBlockListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingBlockNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex, ReadOnlyBrowsingBlockNodeIndex>(owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        protected virtual IReadOnlyPlaceholderNodeState CreateRootNodeState(IReadOnlyRootNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyPlaceholderNodeState(nodeIndex);
        }
        #endregion
    }
}
