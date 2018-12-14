using BaseNode;
using BaseNodeHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyNodeState : IReadOnlyState
    {
        void Init(IReadOnlyBrowseContext browseContext, IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner, IReadOnlyInnerReadOnlyDictionary<string> innerTable, IReadOnlyDictionary<string, ValuePropertyType> valuePropertyTable);
        INode Node { get; }
        IReadOnlyIndex ParentIndex { get; }
        IReadOnlyInner<IReadOnlyBrowsingChildIndex> ParentInner { get; }
        IReadOnlyNodeState ParentState { get; }
        IReadOnlyInnerReadOnlyDictionary<string> InnerTable { get; }
        IReadOnlyDictionary<string, ValuePropertyType> ValuePropertyTypeTable { get; }
        bool FindFirstUnassignedOptional(out IReadOnlyOptionalInner inner);
        IReadOnlyNodeStateReadOnlyList GetAllChildren();
        INode CloneNode();
        void BrowseChildren(IReadOnlyBrowseContext browseContext);
    }

    public abstract class ReadOnlyNodeState : IReadOnlyNodeState
    {
        #region Init
        public ReadOnlyNodeState(IReadOnlyIndex parentIndex)
        {
            Debug.Assert(parentIndex != null);

            ParentIndex = parentIndex;
            ValuePropertyTypeTable = null;
            IsInitialized = false;
        }

        public virtual void Init(IReadOnlyBrowseContext browseContext, IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner, IReadOnlyInnerReadOnlyDictionary<string> innerTable, IReadOnlyDictionary<string, ValuePropertyType> valuePropertyTable)
        {
            Debug.Assert(browseContext != null);
            Debug.Assert(innerTable != null);
            Debug.Assert(valuePropertyTable != null);
            Debug.Assert(!IsInitialized);
            Debug.Assert(ParentInner == null);
            Debug.Assert(InnerTable == null);
            Debug.Assert(ValuePropertyTypeTable == null);

            InitParentInner(parentInner);
            if (parentInner != null)
                InitParentState(ParentInner.Owner);
            InitInnerTable(innerTable);

            Dictionary<string, ValuePropertyType> Table = new Dictionary<string, ValuePropertyType>();
            foreach (KeyValuePair<string, ValuePropertyType> Entry in valuePropertyTable)
                Table.Add(Entry.Key, Entry.Value);

            ValuePropertyTypeTable = Table;
            IsInitialized = true;

            CheckInvariant();
        }

        protected void InitParentState(IReadOnlyNodeState parentState)
        {
            Debug.Assert(parentState != null);
            Debug.Assert(ParentState == null);

            ParentState = parentState;
        }

        protected void InitParentInner(IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner)
        {
            Debug.Assert(ParentInner == null);

            ParentInner = parentInner;
        }

        protected virtual void InitInnerTable(IReadOnlyInnerReadOnlyDictionary<string> innerTable)
        {
            Debug.Assert(innerTable != null);
            Debug.Assert(InnerTable == null);

            InnerTable = innerTable;
        }

        protected bool IsInitialized { get; private set; }
        #endregion

        #region Properties
        public abstract INode Node { get; }
        public IReadOnlyIndex ParentIndex { get; }
        public IReadOnlyNodeState ParentState { get; private set; }
        public IReadOnlyInner<IReadOnlyBrowsingChildIndex> ParentInner { get; private set; }
        public IReadOnlyInnerReadOnlyDictionary<string> InnerTable { get; private set; }
        public IReadOnlyDictionary<string, ValuePropertyType> ValuePropertyTypeTable { get; private set; }
        #endregion

        #region Client Interface
        public virtual IReadOnlyInner<IReadOnlyBrowsingChildIndex> PropertyToInner(string propertyName)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(InnerTable.ContainsKey(propertyName));

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

            foreach (KeyValuePair<string, IReadOnlyInner<IReadOnlyBrowsingChildIndex>> Entry in state.InnerTable)
                AddChildInner(stateList, Entry.Value);
        }

        private void AddChildInner(IReadOnlyNodeStateList stateList, IReadOnlyInner<IReadOnlyBrowsingChildIndex> inner)
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

        public virtual void BrowseChildren(IReadOnlyBrowseContext browseNodeContext)
        {
            IList<string> PropertyNames = NodeTreeHelper.EnumChildNodeProperties(Node);
            bool IsAssigned;
            INode ChildNode;
            Type ChildInterfaceType, ChildNodeType;
            IReadOnlyList<INode> ChildNodeList;
            IReadOnlyList<INodeTreeBlock> ChildBlockList;

            foreach (string PropertyName in PropertyNames)
            {
                if (NodeTreeHelper.IsChildNodeProperty(Node, PropertyName, out ChildNodeType))
                {
                    NodeTreeHelper.GetChildNode(Node, PropertyName, out IsAssigned, out ChildNode);
                    IReadOnlyBrowsingPlaceholderNodeIndex ChildNodeIndex = CreateChildNodeIndex(browseNodeContext, PropertyName, ChildNode);

                    IReadOnlyIndexCollection IndexCollection = CreatePlaceholderIndexCollection(browseNodeContext, PropertyName, ChildNodeIndex);
                    browseNodeContext.AddIndexCollection(IndexCollection);
                }

                else if (NodeTreeHelper.IsOptionalChildNodeProperty(Node, PropertyName, out ChildNodeType))
                {
                    IReadOnlyBrowsingOptionalNodeIndex OptionalNodeIndex = CreateOptionalNodeIndex(browseNodeContext, PropertyName);

                    IReadOnlyIndexCollection IndexCollection = CreateOptionalIndexCollection(browseNodeContext, PropertyName, OptionalNodeIndex);
                    browseNodeContext.AddIndexCollection(IndexCollection);
                }

                else if (NodeTreeHelper.IsChildNodeList(Node, PropertyName, out ChildNodeType))
                {
                    browseNodeContext.IncrementListCount();
                    NodeTreeHelper.GetChildNodeList(Node, PropertyName, out ChildNodeList);

                    IReadOnlyIndexCollection IndexCollection = BrowseNodeList(browseNodeContext, PropertyName, ChildNodeList);
                    browseNodeContext.AddIndexCollection(IndexCollection);
                }

                else if (NodeTreeHelper.IsChildBlockList(Node, PropertyName, out ChildInterfaceType, out ChildNodeType))
                {
                    browseNodeContext.IncrementBlockListCount();
                    NodeTreeHelper.GetChildBlockList(Node, PropertyName, out ChildBlockList);

                    IReadOnlyIndexCollection IndexCollection = BrowseNodeBlockList(browseNodeContext, PropertyName, ChildBlockList);
                    browseNodeContext.AddIndexCollection(IndexCollection);
                }

                else if (NodeTreeHelper.IsBooleanProperty(Node, PropertyName))
                    browseNodeContext.AddValueProperty(PropertyName, ValuePropertyType.Boolean);

                else if (NodeTreeHelper.IsEnumProperty(Node, PropertyName))
                    browseNodeContext.AddValueProperty(PropertyName, ValuePropertyType.Enum);

                else if (NodeTreeHelper.IsStringProperty(Node, PropertyName))
                    browseNodeContext.AddValueProperty(PropertyName, ValuePropertyType.String);

                else if (NodeTreeHelper.IsGuidProperty(Node, PropertyName))
                    browseNodeContext.AddValueProperty(PropertyName, ValuePropertyType.Guid);

                else if (NodeTreeHelper.IsDocumentProperty(Node, PropertyName))
                { }

                else
                    throw new ArgumentOutOfRangeException(nameof(PropertyName));
            }
        }

        protected virtual IReadOnlyIndexCollection BrowseNodeList(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyList<INode> childNodeList)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));

            IReadOnlyBrowsingListNodeIndexList NodeIndexList = CreateBrowsingListNodeIndexList();

            for (int Index = 0; Index < childNodeList.Count; Index++)
            {
                INode ChildNode = childNodeList[Index];

                IReadOnlyBrowsingListNodeIndex NewNodeIndex = CreateListNodeIndex(browseNodeContext, propertyName, ChildNode, Index);
                NodeIndexList.Add(NewNodeIndex);
            }

            return CreateListIndexCollection(browseNodeContext, propertyName, NodeIndexList);
        }

        protected virtual IReadOnlyIndexCollection BrowseNodeBlockList(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyList<INodeTreeBlock> childBlockList)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));

            IReadOnlyBrowsingBlockNodeIndexList NodeIndexList = CreateBrowsingBlockNodeIndexList();

            for (int BlockIndex = 0; BlockIndex < childBlockList.Count; BlockIndex++)
            {
                INodeTreeBlock ChildBlock = childBlockList[BlockIndex];
                BrowseBlock(browseNodeContext, propertyName, BlockIndex, ChildBlock, NodeIndexList);
            }

            return CreateBlockIndexCollection(browseNodeContext, propertyName, NodeIndexList);
        }

        protected virtual void BrowseBlock(IReadOnlyBrowseContext browseNodeContext, string propertyName, int blockIndex, INodeTreeBlock childBlock, IReadOnlyBrowsingBlockNodeIndexList nodeIndexList)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));

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
        #endregion

        #region Debugging
        public INode CloneNode()
        {
            INode NewNode = NodeHelper.CreateEmptyNode(Node.GetType());

            foreach (KeyValuePair<string, IReadOnlyInner<IReadOnlyBrowsingChildIndex>> Entry in InnerTable)
            {
                string PropertyName = Entry.Key;
                IReadOnlyInner<IReadOnlyBrowsingChildIndex> Inner = Entry.Value;
                Inner.CloneChildren(NewNode);
            }

            foreach (KeyValuePair<string, ValuePropertyType> Entry in ValuePropertyTypeTable)
            {
                string PropertyName = Entry.Key;
                ValuePropertyType Type = Entry.Value;

                switch (Type)
                {
                    case ValuePropertyType.Boolean:
                        NodeTreeHelper.CopyBooleanProperty(Node, NewNode, Entry.Key);
                        break;
                    case ValuePropertyType.Enum:
                        NodeTreeHelper.CopyEnumProperty(Node, NewNode, Entry.Key);
                        break;
                    case ValuePropertyType.String:
                        NodeTreeHelper.CopyStringProperty(Node, NewNode, Entry.Key);
                        break;
                    case ValuePropertyType.Guid:
                        NodeTreeHelper.CopyGuidProperty(Node, NewNode, Entry.Key);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Type));
                }
            }

            NodeTreeHelper.CopyDocumentation(Node, NewNode);

            return NewNode;
        }

        protected virtual void ClonePlaceholderChildNode(INode parentNode, string propertyName)
        {
            Debug.Assert(parentNode != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(InnerTable.ContainsKey(propertyName));
            Debug.Assert(InnerTable[propertyName] is IReadOnlyPlaceholderInner);

            IReadOnlyPlaceholderInner Inner = (IReadOnlyPlaceholderInner)InnerTable[propertyName];
            INode ChildNodeClone = Inner.ChildState.CloneNode();

            NodeTreeHelper.ReplaceChildNode(parentNode, propertyName, ChildNodeClone);
        }

        protected virtual void CloneOptionalChildNode(INode parentNode, string propertyName)
        {
            Debug.Assert(parentNode != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(InnerTable.ContainsKey(propertyName));
            Debug.Assert(InnerTable[propertyName] is IReadOnlyOptionalInner);

            IReadOnlyOptionalInner Inner = (IReadOnlyOptionalInner)InnerTable[propertyName];
            if (Inner.IsAssigned)
            {
                INode ChildNodeClone = Inner.ChildState.CloneNode();
                NodeTreeHelper.SetOptionalChildNode(parentNode, propertyName, ChildNodeClone);
            }
        }
        #endregion

        #region Invariant
        protected virtual void CheckInvariant()
        {
            InvariantAssert(IsInitialized);
            InvariantAssert(Node != null);
            InvariantAssert(InnerTable != null);

            foreach (KeyValuePair<string, IReadOnlyInner<IReadOnlyBrowsingChildIndex>> Entry in InnerTable)
            {
                IReadOnlyInner<IReadOnlyBrowsingChildIndex> Inner = Entry.Value;

                InvariantAssert((Inner is IReadOnlyBlockListInner) || (Inner is IReadOnlyListInner) || (Inner is IReadOnlyOptionalInner) || (Inner is IReadOnlyPlaceholderInner));
                InvariantAssert(Inner.Owner == this);
            }
        }

        protected void InvariantAssert(bool Condition)
        {
            Debug.Assert(Condition);
        }
        #endregion

        #region Create Methods
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

        protected virtual IReadOnlyBrowsingPlaceholderNodeIndex CreateChildNodeIndex(IReadOnlyBrowseContext browseNodeContext, string propertyName, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyBrowsingPlaceholderNodeIndex(Node, childNode, propertyName);
        }

        protected virtual IReadOnlyBrowsingOptionalNodeIndex CreateOptionalNodeIndex(IReadOnlyBrowseContext browseNodeContext, string propertyName)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyBrowsingOptionalNodeIndex(Node, propertyName);
        }

        protected virtual IReadOnlyBrowsingListNodeIndex CreateListNodeIndex(IReadOnlyBrowseContext browseNodeContext, string propertyName, INode childNode, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyBrowsingListNodeIndex(Node, childNode, propertyName, index);
        }

        protected virtual IReadOnlyBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(IReadOnlyBrowseContext browseNodeContext, string propertyName, INodeTreeBlock childBlock, INode childNode, int blockIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyBrowsingNewBlockNodeIndex(Node, childNode, propertyName, blockIndex, childBlock.ReplicationPattern, childBlock.SourceIdentifier);
        }

        protected virtual IReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockNodeIndex(IReadOnlyBrowseContext browseNodeContext, string propertyName, INode childNode, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyBrowsingExistingBlockNodeIndex(Node, childNode, propertyName, blockIndex, index);
        }

        protected virtual IReadOnlyIndexCollection CreatePlaceholderIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingPlaceholderNodeIndex childNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyIndexCollection<IReadOnlyBrowsingPlaceholderNodeIndex>(propertyName, new List<IReadOnlyBrowsingPlaceholderNodeIndex>() { childNodeIndex });
        }

        protected virtual IReadOnlyIndexCollection CreateOptionalIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingOptionalNodeIndex optionalNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyIndexCollection<IReadOnlyBrowsingOptionalNodeIndex>(propertyName, new List<IReadOnlyBrowsingOptionalNodeIndex>() { optionalNodeIndex });
        }

        protected virtual IReadOnlyBrowsingListNodeIndexList CreateBrowsingListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyBrowsingListNodeIndexList();
        }
        
        protected virtual IReadOnlyIndexCollection CreateListIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingListNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyIndexCollection<IReadOnlyBrowsingListNodeIndex>(propertyName, (IReadOnlyList<IReadOnlyBrowsingListNodeIndex>)nodeIndexList);
        }

        protected virtual IReadOnlyBrowsingBlockNodeIndexList CreateBrowsingBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyBrowsingBlockNodeIndexList();
        }

        protected virtual IReadOnlyIndexCollection CreateBlockIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingBlockNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyIndexCollection<IReadOnlyBrowsingBlockNodeIndex>(propertyName, (IReadOnlyList<IReadOnlyBrowsingBlockNodeIndex>)nodeIndexList);
        }
        #endregion
    }
}
