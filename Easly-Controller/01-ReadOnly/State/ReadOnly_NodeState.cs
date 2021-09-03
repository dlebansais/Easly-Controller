namespace EaslyController.ReadOnly
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.Constants;

    /// <summary>
    /// Base interface for the state of a node.
    /// </summary>
    public interface IReadOnlyNodeState : IEqualComparable
    {
        /// <summary>
        /// The node.
        /// </summary>
        Node Node { get; }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        IReadOnlyIndex ParentIndex { get; }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        IReadOnlyInner ParentInner { get; }

        /// <summary>
        /// State of the parent.
        /// </summary>
        IReadOnlyNodeState ParentState { get; }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        ReadOnlyInnerReadOnlyDictionary<string> InnerTable { get; }

        /// <summary>
        /// Table of children that are not nodes.
        /// </summary>
        IReadOnlyDictionary<string, ValuePropertyType> ValuePropertyTypeTable { get; }

        /// <summary>
        /// The comment associated to this state. Null if none.
        /// </summary>
        string Comment { get; }

        /// <summary>
        /// Gets the inner corresponding to a property.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        IReadOnlyInner PropertyToInner(string propertyName);

        /// <summary>
        /// Gets the value corresponding to a value property.
        /// The value type can be obtained from <see cref="ValuePropertyTypeTable"/>.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <param name="value">Value of the property upon return.</param>
        /// <param name="minValue">Min value of the property upon return. Only applies to enum and booleans.</param>
        /// <param name="maxValue">Max value of the property upon return. Only applies to enum and booleans.</param>
        void PropertyToValue(string propertyName, out object value, out int minValue, out int maxValue);

        /// <summary>
        /// Returns a list of states for all child nodes.
        /// </summary>
        ReadOnlyNodeStateReadOnlyList GetAllChildren();

        /// <summary>
        /// Returns a clone of the node of this state.
        /// </summary>
        /// <returns>The cloned node.</returns>
        Node CloneNode();
    }

    /// <summary>
    /// Base interface for the state of a node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal interface IReadOnlyNodeState<out IInner> : IEqualComparable
        where IInner : IReadOnlyInner<IReadOnlyBrowsingChildIndex>
    {
        /// <summary>
        /// Find children in the node tree.
        /// </summary>
        /// <param name="browseContext">The context used to browse the node tree.</param>
        /// <param name="parentInner">The inner containing this state as a child.</param>
        void BrowseChildren(ReadOnlyBrowseContext browseContext, IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner);

        /// <summary>
        /// Initializes a newly created state.
        /// </summary>
        /// <param name="parentInner">Inner containing this state.</param>
        /// <param name="innerTable">Table for all inners in this state.</param>
        /// <param name="valuePropertyTable">Table of children that are not nodes.</param>
        void Init(IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner, ReadOnlyInnerReadOnlyDictionary<string> innerTable, IReadOnlyDictionary<string, ValuePropertyType> valuePropertyTable);

        /// <summary>
        /// Attach a view to the state.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to call when enumerating existing states.</param>
        void Attach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet);

        /// <summary>
        /// Detach a view from the state.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to no longer call when enumerating existing states.</param>
        void Detach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet);
    }

    /// <summary>
    /// Base class for the state of a node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal abstract class ReadOnlyNodeState<IInner> : IReadOnlyNodeState<IInner>, IReadOnlyNodeState, IEqualComparable
        where IInner : IReadOnlyInner<IReadOnlyBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyNodeState{IInner}"/> class.
        /// </summary>
        /// <param name="parentIndex">The index used to create the state.</param>
        public ReadOnlyNodeState(IReadOnlyIndex parentIndex)
        {
            Debug.Assert(parentIndex != null);

            ParentIndex = parentIndex;
            ValuePropertyTypeTable = null;
            IsInitialized = false;
        }

        /// <summary>
        /// Initializes a newly created state.
        /// </summary>
        /// <param name="parentInner">Inner containing this state.</param>
        /// <param name="innerTable">Table for all inners in this state.</param>
        /// <param name="valuePropertyTable">Table of children that are not nodes.</param>
        public virtual void Init(IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner, ReadOnlyInnerReadOnlyDictionary<string> innerTable, IReadOnlyDictionary<string, ValuePropertyType> valuePropertyTable)
        {
            Debug.Assert(innerTable != null);
            Debug.Assert(valuePropertyTable != null);
            Debug.Assert(!IsInitialized);
            Debug.Assert(ParentInner == null);
            Debug.Assert(InnerTable == null);
            Debug.Assert(ValuePropertyTypeTable == null);

            InitParentInner(parentInner);
            if (parentInner != null) // The root node doesn't have a parent inner.
                InitParentState(ParentInner.Owner);
            InitInnerTable(innerTable);

            Dictionary<string, ValuePropertyType> Table = new Dictionary<string, ValuePropertyType>();
            foreach (KeyValuePair<string, ValuePropertyType> Entry in valuePropertyTable)
                Table.Add(Entry.Key, Entry.Value);

            ValuePropertyTypeTable = Table;
            IsInitialized = true;

            CheckInvariant();
        }

        private protected virtual void InitParentState(IReadOnlyNodeState parentState)
        {
            Debug.Assert(parentState != null);
            Debug.Assert(ParentState == null);

            ParentState = parentState;
        }

        private protected virtual void InitParentInner(IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner)
        {
            Debug.Assert(ParentInner == null);

            ParentInner = (IReadOnlyInner)parentInner;
        }

        private protected virtual void InitInnerTable(ReadOnlyInnerReadOnlyDictionary<string> innerTable)
        {
            Debug.Assert(innerTable != null);
            Debug.Assert(InnerTable == null);

            InnerTable = innerTable;
        }

        /// <summary>
        /// Find children in the node tree.
        /// </summary>
        /// <param name="browseContext">The context used to browse the node tree.</param>
        /// <param name="parentInner">The inner containing this state as a child.</param>
        public virtual void BrowseChildren(ReadOnlyBrowseContext browseContext, IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner)
        {
            BrowseChildrenOfNode(browseContext, Node);
        }

        private protected virtual void BrowseChildrenOfNode(ReadOnlyBrowseContext browseNodeContext, Node node)
        {
            IList<string> PropertyNames = NodeTreeHelper.EnumChildNodeProperties(node);

            foreach (string PropertyName in PropertyNames)
            {
                Node ChildNode;
                Type ChildInterfaceType, ChildNodeType;
                IReadOnlyList<Node> ChildNodeList;
                IReadOnlyList<NodeTreeBlock> ChildBlockList;
                bool IsHandled = false;

                if (NodeTreeHelperChild.IsChildNodeProperty(node, PropertyName, out ChildNodeType))
                {
                    NodeTreeHelperChild.GetChildNode(node, PropertyName, out ChildNode);
                    Debug.Assert(ChildNode != null);
                    ReadOnlyBrowsingPlaceholderNodeIndex ChildNodeIndex = CreateChildNodeIndex(browseNodeContext, node, PropertyName, ChildNode);

                    // Create a collection containing one index for this child node.
                    IReadOnlyIndexCollection IndexCollection = CreatePlaceholderIndexCollection(browseNodeContext, PropertyName, ChildNodeIndex);
                    browseNodeContext.AddIndexCollection(IndexCollection);

                    IsHandled = true;
                }
                else if (NodeTreeHelperOptional.IsOptionalChildNodeProperty(node, PropertyName, out ChildNodeType))
                {
                    ReadOnlyBrowsingOptionalNodeIndex OptionalNodeIndex = CreateOptionalNodeIndex(browseNodeContext, node, PropertyName);

                    // Create a collection containing one index for this optional node.
                    IReadOnlyIndexCollection IndexCollection = CreateOptionalIndexCollection(browseNodeContext, PropertyName, OptionalNodeIndex);
                    browseNodeContext.AddIndexCollection(IndexCollection);

                    IsHandled = true;
                }
                else if (NodeTreeHelperList.IsNodeListProperty(node, PropertyName, out ChildNodeType))
                {
                    NodeTreeHelperList.GetChildNodeList(node, PropertyName, out ChildNodeList);
                    Debug.Assert(ChildNodeList != null);

                    // Create a collection containing indexes for each children.
                    IReadOnlyIndexCollection IndexCollection = BrowseNodeList(browseNodeContext, node, PropertyName, ChildNodeList);
                    browseNodeContext.AddIndexCollection(IndexCollection);

                    IsHandled = true;
                }
                else if (NodeTreeHelperBlockList.IsBlockListProperty(node, PropertyName, out ChildInterfaceType, out ChildNodeType))
                {
                    NodeTreeHelperBlockList.GetChildBlockList(node, PropertyName, out ChildBlockList);
                    Debug.Assert(ChildBlockList != null);

                    // Create a collection containing indexes for each child blocks and their children.
                    IReadOnlyIndexCollection IndexCollection = BrowseNodeBlockList(browseNodeContext, node, PropertyName, ChildBlockList);
                    browseNodeContext.AddIndexCollection(IndexCollection);

                    IsHandled = true;
                }
                else if (NodeTreeHelper.IsBooleanProperty(node, PropertyName))
                {
                    browseNodeContext.AddValueProperty(PropertyName, ValuePropertyType.Boolean);
                    IsHandled = true;
                }
                else if (NodeTreeHelper.IsEnumProperty(node, PropertyName))
                {
                    browseNodeContext.AddValueProperty(PropertyName, ValuePropertyType.Enum);
                    IsHandled = true;
                }
                else if (NodeTreeHelper.IsStringProperty(node, PropertyName))
                {
                    browseNodeContext.AddValueProperty(PropertyName, ValuePropertyType.String);
                    IsHandled = true;
                }
                else if (NodeTreeHelper.IsGuidProperty(node, PropertyName))
                {
                    browseNodeContext.AddValueProperty(PropertyName, ValuePropertyType.Guid);
                    IsHandled = true;
                }
                else if (NodeTreeHelper.IsDocumentProperty(node, PropertyName))
                {
                    IsHandled = true; // Ignore the doc node.
                }

                Debug.Assert(IsHandled);
            }
        }

        private protected virtual IReadOnlyIndexCollection BrowseNodeList(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, IReadOnlyList<Node> childNodeList)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));

            ReadOnlyBrowsingListNodeIndexList NodeIndexList = CreateBrowsingListNodeIndexList();

            for (int Index = 0; Index < childNodeList.Count; Index++)
            {
                Node ChildNode = childNodeList[Index];
                Debug.Assert(ChildNode != null);

                ReadOnlyBrowsingListNodeIndex NewNodeIndex = CreateListNodeIndex(browseNodeContext, node, propertyName, ChildNode, Index);
                NodeIndexList.Add(NewNodeIndex);
            }

            return CreateListIndexCollection(browseNodeContext, propertyName, NodeIndexList);
        }

        private protected virtual IReadOnlyIndexCollection BrowseNodeBlockList(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, IReadOnlyList<NodeTreeBlock> childBlockList)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));

            ReadOnlyBrowsingBlockNodeIndexList NodeIndexList = CreateBrowsingBlockNodeIndexList();

            for (int BlockIndex = 0; BlockIndex < childBlockList.Count; BlockIndex++)
            {
                NodeTreeBlock ChildBlock = childBlockList[BlockIndex];
                BrowseBlock(browseNodeContext, node, propertyName, BlockIndex, ChildBlock, NodeIndexList);
            }

            return CreateBlockIndexCollection(browseNodeContext, propertyName, NodeIndexList);
        }

        private protected virtual void BrowseBlock(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, int blockIndex, NodeTreeBlock childBlock, ReadOnlyBrowsingBlockNodeIndexList nodeIndexList)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));

            for (int Index = 0; Index < childBlock.NodeList.Count; Index++)
            {
                Node ChildNode = childBlock.NodeList[Index];

                ReadOnlyBrowsingBlockNodeIndex NewNodeIndex;
                if (Index == 0) // For the first node, we use a IxxxBrowsingNewBlockNodeIndex, otherwise a IxxxBrowsingExistingBlockNodeIndex.
                    NewNodeIndex = CreateNewBlockNodeIndex(browseNodeContext, node, propertyName, blockIndex, ChildNode);
                else
                    NewNodeIndex = CreateExistingBlockNodeIndex(browseNodeContext, node, propertyName, blockIndex, Index, ChildNode);

                nodeIndexList.Add(NewNodeIndex);
            }
        }

        private protected bool IsInitialized { get; private set; }
        #endregion

        #region Properties
        /// <summary>
        /// The node.
        /// </summary>
        public abstract Node Node { get; }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public IReadOnlyIndex ParentIndex { get; }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        public IReadOnlyInner ParentInner { get; private set; }

        /// <summary>
        /// State of the parent.
        /// </summary>
        public IReadOnlyNodeState ParentState { get; private set; }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        public ReadOnlyInnerReadOnlyDictionary<string> InnerTable { get; private set; }

        /// <summary>
        /// Table of children that are not nodes.
        /// </summary>
        public IReadOnlyDictionary<string, ValuePropertyType> ValuePropertyTypeTable { get; private set; }

        /// <summary>
        /// The comment associated to this state. Null if none.
        /// </summary>
        public abstract string Comment { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Gets the inner corresponding to a property in the node.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        public virtual IReadOnlyInner PropertyToInner(string propertyName)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(InnerTable.ContainsKey(propertyName));

            return (IReadOnlyInner)InnerTable[propertyName];
        }

        /// <summary>
        /// Gets the value corresponding to a value property.
        /// The value type can be obtained from <see cref="ValuePropertyTypeTable"/>.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <param name="value">Value of the property upon return.</param>
        /// <param name="minValue">Min value of the property upon return. Only applies to enum and booleans.</param>
        /// <param name="maxValue">Max value of the property upon return. Only applies to enum and booleans.</param>
        public virtual void PropertyToValue(string propertyName, out object value, out int minValue, out int maxValue)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(ValuePropertyTypeTable.ContainsKey(propertyName));

            value = null;
            minValue = -1;
            maxValue = -1;
            bool IsHandled = false;

            switch (ValuePropertyTypeTable[propertyName])
            {
                case ValuePropertyType.Boolean:
                case ValuePropertyType.Enum:
                    value = NodeTreeHelper.GetEnumValue(Node, propertyName);
                    NodeTreeHelper.GetEnumRange(Node.GetType(), propertyName, out minValue, out maxValue);
                    IsHandled = true;
                    break;

                case ValuePropertyType.String:
                    value = NodeTreeHelper.GetString(Node, propertyName);
                    IsHandled = true;
                    break;

                case ValuePropertyType.Guid:
                    value = NodeTreeHelper.GetGuid(Node, propertyName);
                    IsHandled = true;
                    break;
            }

            Debug.Assert(IsHandled);
        }

        /// <summary>
        /// Returns a list of states for all child nodes.
        /// </summary>
        public ReadOnlyNodeStateReadOnlyList GetAllChildren()
        {
            ReadOnlyNodeStateList StateList = CreateNodeStateList();
            AddChildStates(StateList, this);

            DebugObjects.AddReference(StateList);

            return StateList.ToReadOnly();
        }

        private void AddChildStates(ReadOnlyNodeStateList stateList, IReadOnlyNodeState state)
        {
            stateList.Add(state);

            foreach (KeyValuePair<string, IReadOnlyInner> Entry in state.InnerTable)
                AddChildInner(stateList, Entry.Value);
        }

        private void AddChildInner(ReadOnlyNodeStateList stateList, IReadOnlyInner inner)
        {
            bool IsHandled = false;

            switch (inner)
            {
                case IReadOnlyPlaceholderInner AsPlaceholderInner:
                    AddChildStates(stateList, AsPlaceholderInner.ChildState);
                    IsHandled = true;
                    break;

                case IReadOnlyOptionalInner AsOptionalInner:
                    if (AsOptionalInner.IsAssigned)
                        AddChildStates(stateList, AsOptionalInner.ChildState);
                    IsHandled = true;
                    break;

                case IReadOnlyListInner AsListInner:
                    foreach (IReadOnlyNodeState ChildState in AsListInner.StateList)
                        AddChildStates(stateList, ChildState);
                    IsHandled = true;
                    break;

                case IReadOnlyBlockListInner AsBlockListInner:
                    foreach (IReadOnlyBlockState Block in AsBlockListInner.BlockStateList)
                    {
                        stateList.Add(Block.PatternState);
                        stateList.Add(Block.SourceState);

                        foreach (IReadOnlyNodeState ChildState in Block.StateList)
                            AddChildStates(stateList, ChildState);
                    }
                    IsHandled = true;
                    break;
            }

            Debug.Assert(IsHandled);
        }

        /// <summary>
        /// Attach a view to the state.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to call when enumerating existing states.</param>
        public virtual void Attach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet)
        {
            callbackSet.OnNodeStateAttached(this);

            foreach (KeyValuePair<string, IReadOnlyInner> Entry in InnerTable)
            {
                IReadOnlyInner Inner = Entry.Value;
                ((IReadOnlyInner<IReadOnlyBrowsingChildIndex>)Inner).Attach(view, callbackSet);
            }
        }

        /// <summary>
        /// Detach a view to the state.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to no longer call when enumerating existing states.</param>
        public virtual void Detach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet)
        {
            foreach (KeyValuePair<string, IReadOnlyInner> Entry in InnerTable)
            {
                IReadOnlyInner Inner = Entry.Value;
                ((IReadOnlyInner<IReadOnlyBrowsingChildIndex>)Inner).Detach(view, callbackSet);
            }

            callbackSet.OnNodeStateDetached(this);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ReadOnlyNodeState{IInner}"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyNodeState<IInner> AsNodeState))
                return comparer.Failed();

            if (!comparer.VerifyEqual((IEqualComparable)ParentIndex, (IEqualComparable)AsNodeState.ParentIndex))
                return comparer.Failed();

            if (!comparer.IsTrue((ParentInner == null && AsNodeState.ParentInner == null) || (ParentInner != null && AsNodeState.ParentInner != null)) || (ParentInner != null && !comparer.VerifyEqual(ParentInner, AsNodeState.ParentInner)))
                return comparer.Failed();

            if (!comparer.IsTrue((ParentState == null && AsNodeState.ParentState == null) || (ParentState != null && AsNodeState.ParentState != null)) || (ParentState != null && !comparer.VerifyEqual(ParentState, AsNodeState.ParentState)))
                return comparer.Failed();

            return true;
        }

        private protected virtual bool IsChildrenEqual(CompareEqual comparer, IReadOnlyNodeState nodeState)
        {
            if (!comparer.VerifyEqual(InnerTable, nodeState.InnerTable))
                return comparer.Failed();

            if (!comparer.IsSameCount(ValuePropertyTypeTable.Count, nodeState.ValuePropertyTypeTable.Count))
                return comparer.Failed();

            foreach (KeyValuePair<string, ValuePropertyType> Entry in ValuePropertyTypeTable)
            {
                if (!comparer.IsTrue(nodeState.ValuePropertyTypeTable.ContainsKey(Entry.Key)))
                    return comparer.Failed();

                if (!comparer.IsTrue(nodeState.ValuePropertyTypeTable[Entry.Key] == Entry.Value))
                    return comparer.Failed();
            }

            return true;
        }

        /// <summary>
        /// Returns a clone of the node of this state.
        /// </summary>
        /// <returns>The cloned node.</returns>
        public virtual Node CloneNode()
        {
            // Create a clone, initially empty and full of null references.
            Node NewNode = NodeHelper.CreateEmptyNode(Node.GetType());

            // Clone and assign reference to all nodes, optional or not, list and block lists.
            foreach (KeyValuePair<string, IReadOnlyInner> Entry in InnerTable)
            {
                string PropertyName = Entry.Key;
                IReadOnlyInner Inner = Entry.Value;
                ((IReadOnlyInner<IReadOnlyBrowsingChildIndex>)Inner).CloneChildren(NewNode);
            }

            // Copy other properties.
            foreach (KeyValuePair<string, ValuePropertyType> Entry in ValuePropertyTypeTable)
            {
                string PropertyName = Entry.Key;
                ValuePropertyType Type = Entry.Value;
                bool IsHandled = false;

                switch (Type)
                {
                    case ValuePropertyType.Boolean:
                        NodeTreeHelper.CopyBooleanProperty(Node, NewNode, Entry.Key);
                        IsHandled = true;
                        break;

                    case ValuePropertyType.Enum:
                        NodeTreeHelper.CopyEnumProperty(Node, NewNode, Entry.Key);
                        IsHandled = true;
                        break;

                    case ValuePropertyType.String:
                        NodeTreeHelper.CopyStringProperty(Node, NewNode, Entry.Key);
                        IsHandled = true;
                        break;

                    case ValuePropertyType.Guid:
                        NodeTreeHelper.CopyGuidProperty(Node, NewNode, Entry.Key);
                        IsHandled = true;
                        break;
                }

                Debug.Assert(IsHandled);
            }

            // Also copy comments.
            NodeTreeHelper.CopyDocumentation(Node, NewNode, cloneCommentGuid: true);

            return NewNode;
        }
        #endregion

        #region Invariant
        private protected virtual void CheckInvariant()
        {
            InvariantAssert(IsInitialized);
            InvariantAssert(Node != null);
            InvariantAssert(InnerTable != null);

            foreach (KeyValuePair<string, IReadOnlyInner> Entry in InnerTable)
            {
                IReadOnlyInner Inner = Entry.Value;

                InvariantAssert((Inner is IReadOnlyBlockListInner) || (Inner is IReadOnlyListInner) || (Inner is IReadOnlyOptionalInner) || (Inner is IReadOnlyPlaceholderInner));
                InvariantAssert(Inner.Owner == this);
            }
        }

        private protected void InvariantAssert(bool condition)
        {
            Debug.Assert(condition);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeStateList object.
        /// </summary>
        private protected virtual ReadOnlyNodeStateList CreateNodeStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState<IInner>));
            return new ReadOnlyNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxBrowsingPlaceholderNodeIndex object.
        /// </summary>
        private protected virtual ReadOnlyBrowsingPlaceholderNodeIndex CreateChildNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, Node childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState<IInner>));
            return new ReadOnlyBrowsingPlaceholderNodeIndex(node, childNode, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        private protected virtual ReadOnlyBrowsingOptionalNodeIndex CreateOptionalNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState<IInner>));
            return new ReadOnlyBrowsingOptionalNodeIndex(node, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        private protected virtual ReadOnlyBrowsingListNodeIndex CreateListNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, Node childNode, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState<IInner>));
            return new ReadOnlyBrowsingListNodeIndex(node, childNode, propertyName, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        private protected virtual ReadOnlyBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, int blockIndex, Node childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState<IInner>));
            return new ReadOnlyBrowsingNewBlockNodeIndex(node, childNode, propertyName, blockIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected virtual ReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, int blockIndex, int index, Node childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState<IInner>));
            return new ReadOnlyBrowsingExistingBlockNodeIndex(node, childNode, propertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingPlaceholderNodeIndex.
        /// </summary>
        private protected virtual IReadOnlyIndexCollection CreatePlaceholderIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, ReadOnlyBrowsingPlaceholderNodeIndex childNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState<IInner>));
            return new ReadOnlyIndexCollection<ReadOnlyBrowsingPlaceholderNodeIndex>(propertyName, new List<ReadOnlyBrowsingPlaceholderNodeIndex>() { childNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingOptionalNodeIndex.
        /// </summary>
        private protected virtual IReadOnlyIndexCollection CreateOptionalIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, ReadOnlyBrowsingOptionalNodeIndex optionalNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState<IInner>));
            return new ReadOnlyIndexCollection<ReadOnlyBrowsingOptionalNodeIndex>(propertyName, new List<ReadOnlyBrowsingOptionalNodeIndex>() { optionalNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndexList object.
        /// </summary>
        private protected virtual ReadOnlyBrowsingListNodeIndexList CreateBrowsingListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState<IInner>));
            return new ReadOnlyBrowsingListNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingListNodeIndex objects.
        /// </summary>
        private protected virtual IReadOnlyIndexCollection CreateListIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, ReadOnlyBrowsingListNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState<IInner>));
            return new ReadOnlyIndexCollection<IReadOnlyBrowsingListNodeIndex>(propertyName, nodeIndexList);
        }

        /// <summary>
        /// Creates a IxxxBrowsingBlockNodeIndexList object.
        /// </summary>
        private protected virtual ReadOnlyBrowsingBlockNodeIndexList CreateBrowsingBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState<IInner>));
            return new ReadOnlyBrowsingBlockNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingBlockNodeIndex objects.
        /// </summary>
        private protected virtual IReadOnlyIndexCollection CreateBlockIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, ReadOnlyBrowsingBlockNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState<IInner>));
            return new ReadOnlyIndexCollection<IReadOnlyBrowsingBlockNodeIndex>(propertyName, nodeIndexList);
        }
        #endregion
    }
}
