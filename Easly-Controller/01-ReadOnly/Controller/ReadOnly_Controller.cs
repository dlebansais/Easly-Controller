using BaseNode;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyController
    {
        IReadOnlyNodeState RootState { get; }
        IReadOnlyRootNodeIndex RootIndex { get; }
        Stats Stats { get; }
        bool ContainsNode(INode node);
        IReadOnlyNodeState NodeToState(INode node);
    }

    public class ReadOnlyController : IReadOnlyController
    {
        #region Init
        public static IReadOnlyController Create(IReadOnlyRootNodeIndex nodeIndex)
        {
            ReadOnlyController Controller = new ReadOnlyController();
            Controller.SetRoot(nodeIndex);
            return Controller;
        }

        protected ReadOnlyController()
        {
            IsInitialized = false;
            _StateTable = CreateStateTable();
            StateTable = CreateStateTableReadOnly(_StateTable);
            Stats = new Stats();
        }

        private bool IsInitialized;
        #endregion

        #region Properties
        public IReadOnlyNodeState RootState { get; private set; }
        public IReadOnlyRootNodeIndex RootIndex { get; private set; }
        public Stats Stats { get; private set; }
        protected IReadOnlyNodeStateReadOnlyDictionary<INode> StateTable { get; private set; }
        private IReadOnlyNodeStateDictionary<INode> _StateTable;
        #endregion

        #region Client Interface
        public virtual bool ContainsNode(INode node)
        {
            Debug.Assert(node != null);

            return StateTable.ContainsKey(node);
        }

        public virtual IReadOnlyNodeState NodeToState(INode node)
        {
            Debug.Assert(node != null);
            Debug.Assert(ContainsNode(node));

            return StateTable[node];
        }
        #endregion

        #region Implementation
        protected virtual void SetRoot(IReadOnlyRootNodeIndex rootIndex)
        {
            Debug.Assert(rootIndex != null);

            IReadOnlyNodeState State = CreateRootNodeState(rootIndex);
            RootIndex = rootIndex;
            RootState = State;

            AddState(State);
            Stats.NodeCount++;
            BuildStateTable(null, null, rootIndex, State);

            IsInitialized = true;
            CheckInvariant();
        }

        protected virtual void AddState(IReadOnlyNodeState state)
        {
            Debug.Assert(state != null);
            Debug.Assert(!StateTable.ContainsKey(state.Node));

            _StateTable.Add(state.Node, state);
        }

        protected virtual void RemoveState(IReadOnlyNodeState state)
        {
            Debug.Assert(state != null);
            Debug.Assert(StateTable.ContainsKey(state.Node));

            _StateTable.Remove(state.Node);
        }

        protected virtual void BuildStateTable(IReadOnlyInner parentInner, IReadOnlyBrowseNodeContext parentBrowseContext, IReadOnlyNodeIndex nodeIndex, IReadOnlyNodeState state)
        {
            Debug.Assert((parentInner == null && parentBrowseContext == null) || (parentInner != null && parentBrowseContext != null));
            Debug.Assert(nodeIndex != null);
            Debug.Assert(state != null);
            Debug.Assert(nodeIndex.Node == state.Node);

            IReadOnlyBrowseNodeContext BrowseContext = CreateBrowseContext(parentBrowseContext, state);
            BrowseStateChildren(BrowseContext);

            IReadOnlyInnerReadOnlyDictionary<string> InnerTable = BuildInnerTable(BrowseContext);
            InitState(BrowseContext, parentInner, nodeIndex, InnerTable);

            IReadOnlyNodeIndexNodeStateDictionary ChildrenStateTable = BuildChildrenStateTable(BrowseContext);
            BuildChildrenStates(BrowseContext, ChildrenStateTable);
        }

        protected virtual void BrowseStateChildren(IReadOnlyBrowseNodeContext browseContext)
        {
            Debug.Assert(browseContext != null);
            Debug.Assert(browseContext.IndexCollectionList.Count == 0);

            IReadOnlyNodeState State = browseContext.State;
            State.BrowseChildren(browseContext);

            Stats.ListCount += browseContext.ListCount;
            Stats.BlockListCount += browseContext.BlockListCount;
        }

        protected virtual IReadOnlyInnerReadOnlyDictionary<string> BuildInnerTable(IReadOnlyBrowseNodeContext browseContext)
        {
            Debug.Assert(browseContext != null);

            IReadOnlyNodeState State = browseContext.State;
            Debug.Assert(browseContext.State.InnerTable == null);

            IReadOnlyIndexCollectionReadOnlyList IndexCollectionList = browseContext.IndexCollectionList;
            IReadOnlyInnerDictionary<string> InnerTable = CreateInnerTable();

            foreach (IReadOnlyIndexCollection NodeIndexCollection in IndexCollectionList)
            {
                string PropertyName = NodeIndexCollection.PropertyName;
                Debug.Assert(!InnerTable.ContainsKey(PropertyName));

                IReadOnlyInner Inner = BuildInner(State, NodeIndexCollection);
                InnerTable.Add(PropertyName, Inner);
            }

            return CreateInnerTableReadOnly(InnerTable);
        }

        protected virtual IReadOnlyInner BuildInner(IReadOnlyNodeState parentState, IReadOnlyIndexCollection nodeIndexCollection)
        {
            switch (nodeIndexCollection)
            {
                case IReadOnlyIndexCollection<IReadOnlyBrowsingPlaceholderNodeIndex> AsPlaceholderNodeIndexCollection:
                    return CreatePlaceholderInner(parentState, AsPlaceholderNodeIndexCollection);

                case IReadOnlyIndexCollection<IReadOnlyBrowsingOptionalNodeIndex> AsOptionalNodeIndexCollection:
                    return CreateOptionalInner(parentState, AsOptionalNodeIndexCollection);

                case IReadOnlyIndexCollection<IReadOnlyBrowsingListNodeIndex> AsListNodeIndexCollection:
                    return CreateListInner(parentState, AsListNodeIndexCollection);

                case IReadOnlyIndexCollection<IReadOnlyBrowsingBlockNodeIndex> AsBlockNodeIndexCollection:
                    return CreateBlockListInner(parentState, AsBlockNodeIndexCollection);

                default:
                    throw new ArgumentOutOfRangeException(nameof(nodeIndexCollection));
            }
        }

        protected virtual void InitState(IReadOnlyBrowseNodeContext browseContext, IReadOnlyInner parentInner, IReadOnlyNodeIndex nodeIndex, IReadOnlyInnerReadOnlyDictionary<string> innerTable)
        {
            IReadOnlyNodeState State = browseContext.State;
            Debug.Assert(nodeIndex.Node == State.Node);

            State.Init(browseContext, parentInner, nodeIndex, innerTable);
        }

        protected virtual IReadOnlyNodeIndexNodeStateDictionary BuildChildrenStateTable(IReadOnlyBrowseNodeContext browseContext)
        {
            IReadOnlyNodeState State = browseContext.State;
            IReadOnlyDictionary<string, IReadOnlyInner> InnerTable = State.InnerTable;
            IReadOnlyIndexCollectionReadOnlyList IndexCollectionList = browseContext.IndexCollectionList;

            IReadOnlyNodeIndexNodeStateDictionary ChildStateTable = CreateChildStateTable();

            foreach (IReadOnlyIndexCollection<IReadOnlyBrowsingChildNodeIndex> NodeIndexCollection in IndexCollectionList)
            {
                IReadOnlyList<IReadOnlyBrowsingChildNodeIndex> NodeIndexList = NodeIndexCollection.NodeIndexList;
                string PropertyName = NodeIndexCollection.PropertyName;

                Debug.Assert(InnerTable.ContainsKey(PropertyName));
                IReadOnlyInner<IReadOnlyBrowsingChildNodeIndex> Inner = (IReadOnlyInner<IReadOnlyBrowsingChildNodeIndex>)InnerTable[PropertyName];

                for (int i = 0; i < NodeIndexList.Count; i++)
                {
                    IReadOnlyBrowsingChildNodeIndex ChildNodeIndex = NodeIndexList[i];

                    if ((Inner is IReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex> AsBlockListInner) && (ChildNodeIndex is IReadOnlyBrowsingNewBlockNodeIndex AsNewBlockIndex))
                    {
                        IReadOnlyBlockState BlockState = AsBlockListInner.InitNewBlock(AsNewBlockIndex);
                        BlockState.InitBlockState();

                        IReadOnlyPatternState PatternState = BlockState.PatternState;
                        AddState(PatternState);
                        Stats.NodeCount++;
                        Stats.PlaceholderNodeCount++;

                        IReadOnlySourceState SourceState = BlockState.SourceState;
                        AddState(SourceState);
                        Stats.NodeCount++;
                        Stats.PlaceholderNodeCount++;
                    }

                    IReadOnlyNodeState ChildState = BuildChildState(Inner, ChildNodeIndex);
                    ChildStateTable.Add(ChildNodeIndex, ChildState);
                }
            }

            return ChildStateTable;
        }

        protected virtual IReadOnlyNodeState BuildChildState(IReadOnlyInner<IReadOnlyBrowsingChildNodeIndex> inner, IReadOnlyBrowsingChildNodeIndex nodeIndex)
        {
            IReadOnlyNodeState ChildState = inner.InitChildState(nodeIndex);
            AddState(ChildState);
            Stats.NodeCount++;

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

        protected virtual void BuildChildrenStates(IReadOnlyBrowseNodeContext browseContext, IReadOnlyNodeIndexNodeStateDictionary childrenStateTable)
        {
            IReadOnlyNodeState State = browseContext.State;
            IReadOnlyInnerReadOnlyDictionary<string> InnerTable = State.InnerTable;
            IReadOnlyIndexCollectionReadOnlyList IndexCollectionList = browseContext.IndexCollectionList;

            foreach (IReadOnlyIndexCollection<IReadOnlyBrowsingChildNodeIndex> NodeIndexCollection in IndexCollectionList)
            {
                IReadOnlyList<IReadOnlyBrowsingChildNodeIndex> NodeIndexList = NodeIndexCollection.NodeIndexList;
                string PropertyName = NodeIndexCollection.PropertyName;
                IReadOnlyInner Inner = InnerTable[PropertyName];

                for (int i = 0; i < NodeIndexList.Count; i++)
                {
                    IReadOnlyBrowsingChildNodeIndex ChildNodeIndex = NodeIndexList[i];
                    IReadOnlyNodeState ChildState = childrenStateTable[ChildNodeIndex];

                    BuildStateTable(Inner, browseContext, ChildNodeIndex, ChildState);
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

        #region Create Methods
        protected virtual IReadOnlyNodeStateDictionary<INode> CreateStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyNodeStateDictionary<INode>();
        }

        protected virtual IReadOnlyNodeStateReadOnlyDictionary<INode> CreateStateTableReadOnly(IReadOnlyNodeStateDictionary<INode> stateTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyNodeStateReadOnlyDictionary<INode>(stateTable);
        }

        protected virtual IReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyInnerDictionary<string>();
        }

        protected virtual IReadOnlyInnerReadOnlyDictionary<string> CreateInnerTableReadOnly(IReadOnlyInnerDictionary<string> innerTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyInnerReadOnlyDictionary<string>(innerTable);
        }

        protected virtual IReadOnlyNodeIndexNodeStateDictionary CreateChildStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyNodeIndexNodeStateDictionary();
        }

        protected virtual IReadOnlyBrowseNodeContext CreateBrowseContext(IReadOnlyBrowseNodeContext parentBrowseContext, IReadOnlyNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyBrowseNodeContext(state);
        }

        protected virtual IReadOnlyPlaceholderInner CreatePlaceholderInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingPlaceholderNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex, ReadOnlyBrowsingPlaceholderNodeIndex>(owner, nodeIndexCollection.PropertyName);
        }

        protected virtual IReadOnlyOptionalInner CreateOptionalInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingOptionalNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex, ReadOnlyBrowsingOptionalNodeIndex>(owner, nodeIndexCollection.PropertyName);
        }

        protected virtual IReadOnlyListInner CreateListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingListNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyListInner<IReadOnlyBrowsingListNodeIndex, ReadOnlyBrowsingListNodeIndex>(owner, nodeIndexCollection.PropertyName);
        }

        protected virtual IReadOnlyBlockListInner CreateBlockListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingBlockNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex, ReadOnlyBrowsingBlockNodeIndex>(owner, nodeIndexCollection.PropertyName);
        }

        protected virtual IReadOnlyNodeState CreateRootNodeState(IReadOnlyRootNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyNodeState(nodeIndex.Node);
        }
        #endregion
    }
}
