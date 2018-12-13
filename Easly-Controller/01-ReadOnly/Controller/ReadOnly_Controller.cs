using BaseNode;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyController
    {
        IReadOnlyPlaceholderNodeState RootState { get; }
        IReadOnlyRootNodeIndex RootIndex { get; }
        Stats Stats { get; }
        bool Contains(IReadOnlyIndex index);
        IReadOnlyNodeState ToState(IReadOnlyIndex index);
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
        public IReadOnlyPlaceholderNodeState RootState { get; private set; }
        public IReadOnlyRootNodeIndex RootIndex { get; private set; }
        public Stats Stats { get; private set; }
        protected IReadOnlyIndexNodeStateReadOnlyDictionary StateTable { get; private set; }
        private IReadOnlyIndexNodeStateDictionary _StateTable;
        #endregion

        #region Client Interface
        public virtual bool Contains(IReadOnlyIndex index)
        {
            Debug.Assert(index != null);

            return StateTable.ContainsKey(index);
        }

        public virtual IReadOnlyNodeState ToState(IReadOnlyIndex index)
        {
            Debug.Assert(index != null);
            Debug.Assert(Contains(index));

            return StateTable[index];
        }
        #endregion

        #region Implementation
        protected virtual void SetRoot(IReadOnlyRootNodeIndex rootIndex)
        {
            Debug.Assert(rootIndex != null);

            IReadOnlyPlaceholderNodeState State = CreateRootNodeState(rootIndex);
            RootIndex = rootIndex;
            RootState = State;

            AddState(RootIndex, State);
            Stats.NodeCount++;
            BuildStateTable(null, null, rootIndex, State);

            IsInitialized = true;
            CheckInvariant();
        }

        protected virtual void AddState(IReadOnlyIndex index, IReadOnlyNodeState state)
        {
            Debug.Assert(state != null);
            Debug.Assert(!StateTable.ContainsKey(index));

            _StateTable.Add(index, state);
        }

        protected virtual void RemoveState(IReadOnlyIndex index)
        {
            Debug.Assert(index != null);
            Debug.Assert(StateTable.ContainsKey(index));

            _StateTable.Remove(index);
        }

        protected virtual void BuildStateTable(IReadOnlyInner parentInner, IReadOnlyBrowseContext parentBrowseContext, IReadOnlyIndex nodeIndex, IReadOnlyNodeState state)
        {
            Debug.Assert((parentInner == null && parentBrowseContext == null) || (parentInner != null && parentBrowseContext != null));
            Debug.Assert(nodeIndex != null);
            Debug.Assert(state != null);

            IReadOnlyBrowseContext BrowseContext = CreateBrowseContext(parentBrowseContext, state);
            BrowseStateChildren(BrowseContext);

            IReadOnlyInnerReadOnlyDictionary<string> InnerTable = BuildInnerTable(BrowseContext);
            InitState(BrowseContext, parentInner, nodeIndex, InnerTable);

            IReadOnlyIndexNodeStateDictionary ChildrenStateTable = BuildChildrenStateTable(BrowseContext);
            BuildChildrenStates(BrowseContext, ChildrenStateTable);
        }

        protected virtual void BrowseStateChildren(IReadOnlyBrowseContext browseContext)
        {
            Debug.Assert(browseContext != null);
            Debug.Assert(browseContext.IndexCollectionList.Count == 0);

            IReadOnlyNodeState State = browseContext.State;
            State.BrowseChildren(browseContext);

            Stats.ListCount += browseContext.ListCount;
            Stats.BlockListCount += browseContext.BlockListCount;
        }

        protected virtual IReadOnlyInnerReadOnlyDictionary<string> BuildInnerTable(IReadOnlyBrowseContext browseContext)
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

        protected virtual void InitState(IReadOnlyBrowseContext browseContext, IReadOnlyInner parentInner, IReadOnlyIndex index, IReadOnlyInnerReadOnlyDictionary<string> innerTable)
        {
            IReadOnlyNodeState State = browseContext.State;
            State.Init(browseContext, parentInner, index, innerTable, browseContext.ValuePropertyTypeTable);
        }

        protected virtual IReadOnlyIndexNodeStateDictionary BuildChildrenStateTable(IReadOnlyBrowseContext browseContext)
        {
            IReadOnlyNodeState State = browseContext.State;
            IReadOnlyDictionary<string, IReadOnlyInner> InnerTable = State.InnerTable;
            IReadOnlyIndexCollectionReadOnlyList IndexCollectionList = browseContext.IndexCollectionList;

            IReadOnlyIndexNodeStateDictionary ChildStateTable = CreateChildStateTable();

            foreach (IReadOnlyIndexCollection<IReadOnlyBrowsingChildIndex> NodeIndexCollection in IndexCollectionList)
            {
                IReadOnlyList<IReadOnlyBrowsingChildIndex> NodeIndexList = NodeIndexCollection.NodeIndexList;
                string PropertyName = NodeIndexCollection.PropertyName;

                Debug.Assert(InnerTable.ContainsKey(PropertyName));
                IReadOnlyInner<IReadOnlyBrowsingChildIndex> Inner = (IReadOnlyInner<IReadOnlyBrowsingChildIndex>)InnerTable[PropertyName];

                for (int i = 0; i < NodeIndexList.Count; i++)
                {
                    IReadOnlyBrowsingChildIndex ChildIndex = NodeIndexList[i];

                    if ((Inner is IReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex> AsBlockListInner) && (ChildIndex is IReadOnlyBrowsingNewBlockNodeIndex AsNewBlockIndex))
                    {
                        IReadOnlyBlockState BlockState = AsBlockListInner.InitNewBlock(AsNewBlockIndex);
                        BlockState.InitBlockState(browseContext);

                        IReadOnlyBrowsingPatternIndex PatternIndex = BlockState.PatternIndex;
                        IReadOnlyPatternState PatternState = BlockState.PatternState;
                        AddState(PatternIndex , PatternState);
                        Stats.NodeCount++;
                        Stats.PlaceholderNodeCount++;

                        IReadOnlyBrowsingSourceIndex SourceIndex = BlockState.SourceIndex;
                        IReadOnlySourceState SourceState = BlockState.SourceState;
                        AddState(SourceIndex, SourceState);
                        Stats.NodeCount++;
                        Stats.PlaceholderNodeCount++;
                    }

                    IReadOnlyNodeState ChildState = BuildChildState(Inner, ChildIndex);
                    ChildStateTable.Add(ChildIndex, ChildState);
                }
            }

            return ChildStateTable;
        }

        protected virtual IReadOnlyNodeState BuildChildState(IReadOnlyInner<IReadOnlyBrowsingChildIndex> inner, IReadOnlyBrowsingChildIndex nodeIndex)
        {
            IReadOnlyNodeState ChildState = inner.InitChildState(nodeIndex);
            AddState(nodeIndex, ChildState);
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

        protected virtual void BuildChildrenStates(IReadOnlyBrowseContext browseContext, IReadOnlyIndexNodeStateDictionary childrenStateTable)
        {
            IReadOnlyNodeState State = browseContext.State;
            IReadOnlyInnerReadOnlyDictionary<string> InnerTable = State.InnerTable;
            IReadOnlyIndexCollectionReadOnlyList IndexCollectionList = browseContext.IndexCollectionList;

            foreach (IReadOnlyIndexCollection<IReadOnlyBrowsingChildIndex> NodeIndexCollection in IndexCollectionList)
            {
                IReadOnlyList<IReadOnlyBrowsingChildIndex> NodeIndexList = NodeIndexCollection.NodeIndexList;
                string PropertyName = NodeIndexCollection.PropertyName;
                IReadOnlyInner Inner = InnerTable[PropertyName];

                for (int i = 0; i < NodeIndexList.Count; i++)
                {
                    IReadOnlyBrowsingChildIndex ChildNodeIndex = NodeIndexList[i];
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
        protected virtual IReadOnlyIndexNodeStateDictionary CreateStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyIndexNodeStateDictionary();
        }

        protected virtual IReadOnlyIndexNodeStateReadOnlyDictionary CreateStateTableReadOnly(IReadOnlyIndexNodeStateDictionary stateTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyIndexNodeStateReadOnlyDictionary(stateTable);
        }

        protected virtual IReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyInnerDictionary<string>();
        }

        protected virtual IReadOnlyInnerReadOnlyDictionary<string> CreateInnerTableReadOnly(IReadOnlyInnerDictionary<string> innerTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyInnerReadOnlyDictionary<string>(innerTable);
        }

        protected virtual IReadOnlyIndexNodeStateDictionary CreateChildStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyIndexNodeStateDictionary();
        }

        protected virtual IReadOnlyBrowseContext CreateBrowseContext(IReadOnlyBrowseContext parentBrowseContext, IReadOnlyNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyBrowseContext(state);
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

        protected virtual IReadOnlyPlaceholderNodeState CreateRootNodeState(IReadOnlyRootNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyPlaceholderNodeState(nodeIndex);
        }
        #endregion
    }
}
