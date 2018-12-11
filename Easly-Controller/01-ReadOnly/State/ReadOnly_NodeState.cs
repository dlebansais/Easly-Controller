using BaseNode;
using BaseNodeHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyNodeState : IReadOnlyState
    {
#if DEBUG
        int DebugIndex { get; }
#endif
        INode Node { get; }
        IReadOnlyNodeState ParentState { get; }
        IReadOnlyInner ParentInner { get; }
        IReadOnlyInnerReadOnlyDictionary<string> InnerTable { get; }
        IReadOnlyBrowsingChildNodeIndex IndexOf(string propertyName, IReadOnlyNodeState childState);
        bool FindFirstUnassignedOptional(out IReadOnlyOptionalInner inner);
        IReadOnlyNodeStateReadOnlyList GetAllChildren();

        void BrowseNodes(IReadOnlyBrowseNodeContext browseContext, bool isEnumerating);
        void Init(IReadOnlyInner parentInner, IReadOnlyNodeIndex nodeIndex, IReadOnlyInnerReadOnlyDictionary<string> innerTable, IReadOnlyBrowseNodeContext browseContext, bool isEnumerating);
    }

    public class ReadOnlyNodeState : ReadOnlyState, IReadOnlyNodeState
    {
#if DEBUG
        static int NextDebugIndex;
        static int GetNextDebugIndex()
        {
            return NextDebugIndex++;
        }
#endif
        #region Init
        public ReadOnlyNodeState(INode node)
        {
#if DEBUG
            DebugIndex = GetNextDebugIndex();
#endif

            Debug.Assert(node != null);

            Node = node;
            IsInitialized = false;

            CheckInvariant();
        }

        private bool IsInitialized;
        #endregion

        #region Properties
#if DEBUG
        public int DebugIndex { get; private set; }
#endif
        public INode Node { get; private set; }
        #endregion

        #region Client Interface
        public virtual IReadOnlyBrowsingChildNodeIndex IndexOf(string propertyName, IReadOnlyNodeState childState)
        {
            return InnerTable[propertyName].IndexOf(childState);
        }

        public override IReadOnlyInner PropertyToInner(string propertyName)
        {
            return InnerTable[propertyName];
        }

        public virtual bool FindFirstUnassignedOptional(out IReadOnlyOptionalInner inner)
        {
            if (ParentInner is IReadOnlyOptionalInner AsOptional)
                if (!AsOptional.IsAssigned)
                {
                    inner = AsOptional;
                    return true;
                }

            if (ParentState == null)
            {
                inner = null;
                return false;
            }

            return ParentState.FindFirstUnassignedOptional(out inner);
        }

        public IReadOnlyNodeStateReadOnlyList GetAllChildren()
        {
            IReadOnlyNodeStateList StateList = CreateNodeStateList();
            AddChildStates(StateList, this);

            return CreateNodeStateReadOnlyList(StateList);
        }

        private void AddChildStates(IReadOnlyNodeStateList stateList, IReadOnlyNodeState state)
        {
            stateList.Add(state);

            foreach (KeyValuePair<string, IReadOnlyInner> Entry in state.InnerTable)
                AddChildInner(stateList, Entry.Value);
        }

        private void AddChildInner(IReadOnlyNodeStateList stateList, IReadOnlyInner inner)
        {
            switch (inner)
            {
                case IReadOnlySingleInner AsSingle:
                    AddChildStates(stateList, AsSingle.ChildState);
                    break;

                case IReadOnlyListInner AsList:
                    foreach (IReadOnlyNodeState ChildState in AsList.StateList)
                        AddChildStates(stateList, ChildState);
                    break;

                case IReadOnlyBlockListInner AsBlockList:
                    foreach (IReadOnlyBlockState Block in AsBlockList.BlockStateList)
                    {
                        stateList.Add(Block.PatternState);
                        stateList.Add(Block.SourceState);

                        foreach (IReadOnlyNodeState ChildState in Block.StateList)
                            AddChildStates(stateList, ChildState);
                    }
                    break;

                default:
                    throw new InvalidCastException(nameof(inner));
            }
        }

        public virtual void BrowseNodes(IReadOnlyBrowseNodeContext browseNodeContext, bool isEnumerating)
        {
            IList<string> PropertyNames = NodeTreeHelper.EnumChildNodeProperties(Node);
            bool IsAssigned;
            INode ChildNode;
            Type ChildInterfaceType, ChildNodeType;
            IReadOnlyList<INode> ChildNodeList;
            IReadOnlyList<INodeTreeBlock> ChildBlockList;

            foreach (string PropertyName in PropertyNames)
            {
                if (NodeTreeHelper.IsChildNodeProperty(Node, PropertyName))
                {
                    NodeTreeHelper.GetChildNode(Node, PropertyName, out IsAssigned, out ChildNode);
                    IReadOnlyBrowsingPlaceholderNodeIndex ChildNodeIndex = CreateChildNodeIndex(browseNodeContext, PropertyName, ChildNode);

                    IReadOnlyIndexCollection IndexCollection = CreatePlaceholderIndexCollection(browseNodeContext, PropertyName, ChildNodeIndex);
                    browseNodeContext.AddCollection(IndexCollection);
                }

                else if (NodeTreeHelper.IsOptionalChildNodeProperty(Node, PropertyName))
                {
                    NodeTreeHelper.GetChildNode(Node, PropertyName, out IsAssigned, out ChildNode);
                    IReadOnlyBrowsingOptionalNodeIndex OptionalNodeIndex = CreateOptionalNodeIndex(browseNodeContext, PropertyName, ChildNode);

                    IReadOnlyIndexCollection IndexCollection = CreateOptionalIndexCollection(browseNodeContext, PropertyName, OptionalNodeIndex);
                    browseNodeContext.AddCollection(IndexCollection);
                }

                else if (NodeTreeHelper.IsChildNodeList(Node, PropertyName, out ChildNodeType))
                {
                    NodeTreeHelper.GetChildNodeList(Node, PropertyName, out ChildNodeList);

                    IReadOnlyIndexCollection IndexCollection = BrowseNodeList(browseNodeContext, PropertyName, ChildNodeList, isEnumerating);
                    browseNodeContext.AddCollection(IndexCollection);
                }

                else if (NodeTreeHelper.IsChildBlockList(Node, PropertyName, out ChildInterfaceType, out ChildNodeType))
                {
                    NodeTreeHelper.GetChildBlockList(Node, PropertyName, out ChildBlockList);

                    IReadOnlyIndexCollection IndexCollection = BrowseNodeBlockList(browseNodeContext, PropertyName, ChildBlockList, isEnumerating);
                    browseNodeContext.AddCollection(IndexCollection);
                }
            }
        }

        protected virtual IReadOnlyIndexCollection BrowseNodeList(IReadOnlyBrowseNodeContext browseNodeContext, string propertyName, IReadOnlyList<INode> childNodeList, bool isEnumerating)
        {
            IReadOnlyBrowsingListNodeIndexList NodeIndexList = CreateBrowsingListNodeIndexList();

            for (int Index = 0; Index < childNodeList.Count; Index++)
            {
                INode ChildNode = childNodeList[Index];

                IReadOnlyBrowsingListNodeIndex NewNodeIndex = CreateListNodeIndex(browseNodeContext, propertyName, ChildNode, Index);
                NodeIndexList.Add(NewNodeIndex);
            }

            return CreateListIndexCollection(browseNodeContext, propertyName, NodeIndexList);
        }

        protected virtual IReadOnlyIndexCollection BrowseNodeBlockList(IReadOnlyBrowseNodeContext browseNodeContext, string propertyName, IReadOnlyList<INodeTreeBlock> childBlockList, bool isEnumerating)
        {
            IReadOnlyBrowsingBlockNodeIndexList NodeIndexList = CreateBrowsingBlockNodeIndexList();

            for (int BlockIndex = 0; BlockIndex < childBlockList.Count; BlockIndex++)
            {
                INodeTreeBlock ChildBlock = childBlockList[BlockIndex];
                BrowseBlock(browseNodeContext, propertyName, BlockIndex, ChildBlock, NodeIndexList, isEnumerating);
            }

            return CreateBlockIndexCollection(browseNodeContext, propertyName, NodeIndexList);
        }

        protected virtual void BrowseBlock(IReadOnlyBrowseNodeContext browseNodeContext, string propertyName, int blockIndex, INodeTreeBlock childBlock, IReadOnlyBrowsingBlockNodeIndexList nodeIndexList, bool isEnumerating)
        {
            for (int Index = 0; Index < childBlock.NodeList.Count; Index++)
            {
                INode ChildNode = childBlock.NodeList[Index];

                IReadOnlyBrowsingBlockNodeIndex NewNodeIndex;
                if (Index == 0)
                    NewNodeIndex = CreateNewBlockNodeIndex(browseNodeContext, propertyName, childBlock, ChildNode, blockIndex);
                else
                    NewNodeIndex = CreateExistingBlockNodeIndex(browseNodeContext, propertyName, ChildNode, blockIndex, Index);

                nodeIndexList.Add(NewNodeIndex);
            }
        }

        public virtual void Init(IReadOnlyInner parentInner, IReadOnlyNodeIndex nodeIndex, IReadOnlyInnerReadOnlyDictionary<string> innerTable, IReadOnlyBrowseNodeContext browseContext, bool isEnumerating)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(innerTable != null);

            if (isEnumerating)
            {
                InvariantAssert(IsInitialized);

                Debug.Assert(ParentInner == parentInner);
                Debug.Assert((ParentInner == null && ParentState == null) || (ParentInner.Owner == ParentState));
                Debug.Assert(InnerTable == innerTable);
            }
            else
            {
                InvariantAssert(!IsInitialized);

                InitParentInner(parentInner);
                InitParentState((parentInner != null) ? ParentInner.Owner : null);
                InitInnerTable(innerTable);

                IsInitialized = true;
            }

            CheckInvariant();
        }
        #endregion

        #region Parent State
        protected void InitParentState(IReadOnlyNodeState state)
        {
            Debug.Assert(ParentState == null);

            ParentState = state;
        }

        public IReadOnlyNodeState ParentState { get; private set; }
        #endregion

        #region Parent Inner
        protected void InitParentInner(IReadOnlyInner inner)
        {
            Debug.Assert(ParentInner == null);

            ParentInner = inner;
        }

        public IReadOnlyInner ParentInner { get; private set; }
        #endregion

        #region Inner Table
        protected virtual void InitInnerTable(IReadOnlyInnerReadOnlyDictionary<string> innerTable)
        {
            Debug.Assert(InnerTable == null);

            InnerTable = innerTable;
        }

        public IReadOnlyInnerReadOnlyDictionary<string> InnerTable { get; private set; }
        #endregion

        #region Invariant
        protected virtual void CheckInvariant()
        {
            if (IsInitialized)
            {
                InvariantAssert(Node != null);
                InvariantAssert(InnerTable != null);

                foreach (KeyValuePair<string, IReadOnlyInner> Entry in InnerTable)
                {
                    IReadOnlyInner Inner = Entry.Value;

                    InvariantAssert((Inner is IReadOnlyBlockListInner) || (Inner is IReadOnlyListInner) || (Inner is IReadOnlyOptionalInner) || (Inner is IReadOnlyPlaceholderInner));
                    InvariantAssert(Inner.Owner == this);
                }
            }
        }

        protected void InvariantAssert(bool Condition)
        {
            if (!Condition)
            {
            }

            Debug.Assert(Condition);
        }
        #endregion

        #region Create Methods
        protected virtual IReadOnlyInnerReadOnlyDictionary<string> CreateInnerTableReadOnly(IReadOnlyInnerDictionary<string> innerTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyInnerReadOnlyDictionary<string>(innerTable);
        }

        protected virtual IReadOnlyNodeStateList CreateNodeStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyNodeStateList();
        }

        protected virtual IReadOnlyNodeStateReadOnlyList CreateNodeStateReadOnlyList(IReadOnlyNodeStateList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyNodeStateReadOnlyList(list);
        }

        protected virtual IReadOnlyBrowsingPlaceholderNodeIndex CreateChildNodeIndex(IReadOnlyBrowseNodeContext browseNodeContext, string propertyName, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyBrowsingPlaceholderNodeIndex(Node, childNode, propertyName);
        }

        protected virtual IReadOnlyBrowsingOptionalNodeIndex CreateOptionalNodeIndex(IReadOnlyBrowseNodeContext browseNodeContext, string propertyName, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyBrowsingOptionalNodeIndex(Node, childNode, propertyName);
        }

        protected virtual IReadOnlyBrowsingListNodeIndex CreateListNodeIndex(IReadOnlyBrowseNodeContext browseNodeContext, string propertyName, INode childNode, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyBrowsingListNodeIndex(Node, childNode, propertyName, index);
        }

        protected virtual IReadOnlyBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(IReadOnlyBrowseNodeContext browseNodeContext, string propertyName, INodeTreeBlock childBlock, INode childNode, int blockIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyBrowsingNewBlockNodeIndex(Node, childNode, propertyName, blockIndex, childBlock.ReplicationPattern, childBlock.SourceIdentifier);
        }

        protected virtual IReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockNodeIndex(IReadOnlyBrowseNodeContext browseNodeContext, string propertyName, INode childNode, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyBrowsingExistingBlockNodeIndex(Node, childNode, propertyName, blockIndex, index);
        }

        protected virtual IReadOnlyIndexCollection CreatePlaceholderIndexCollection(IReadOnlyBrowseNodeContext browseNodeContext, string propertyName, IReadOnlyBrowsingPlaceholderNodeIndex childNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyIndexCollection<IReadOnlyBrowsingPlaceholderNodeIndex>(propertyName, new List<IReadOnlyBrowsingPlaceholderNodeIndex>() { childNodeIndex });
        }

        protected virtual IReadOnlyIndexCollection CreateOptionalIndexCollection(IReadOnlyBrowseNodeContext browseNodeContext, string propertyName, IReadOnlyBrowsingOptionalNodeIndex optionalNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyIndexCollection<IReadOnlyBrowsingOptionalNodeIndex>(propertyName, new List<IReadOnlyBrowsingOptionalNodeIndex>() { optionalNodeIndex });
        }

        protected virtual IReadOnlyBrowsingListNodeIndexList CreateBrowsingListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyBrowsingListNodeIndexList();
        }
        
        protected virtual IReadOnlyIndexCollection CreateListIndexCollection(IReadOnlyBrowseNodeContext browseNodeContext, string propertyName, IReadOnlyBrowsingListNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyIndexCollection<IReadOnlyBrowsingListNodeIndex>(propertyName, (IReadOnlyList<IReadOnlyBrowsingListNodeIndex>)nodeIndexList);
        }

        protected virtual IReadOnlyBrowsingBlockNodeIndexList CreateBrowsingBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyBrowsingBlockNodeIndexList();
        }

        protected virtual IReadOnlyIndexCollection CreateBlockIndexCollection(IReadOnlyBrowseNodeContext browseNodeContext, string propertyName, IReadOnlyBrowsingBlockNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyIndexCollection<IReadOnlyBrowsingBlockNodeIndex>(propertyName, (IReadOnlyList<IReadOnlyBrowsingBlockNodeIndex>)nodeIndexList);
        }
        #endregion
    }
}
