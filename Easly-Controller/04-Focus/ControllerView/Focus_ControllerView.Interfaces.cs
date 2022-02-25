namespace EaslyController.Focus
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using Contracts;
    using EaslyController.Constants;
    using EaslyController.Frame;
    using NotNullReflection;

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public partial class FocusControllerView : FrameControllerView, IFocusInternalControllerView
    {
        /// <summary>
        /// Checks if the template associated to the <paramref name="propertyName"/> property of the <paramref name="stateView"/> state is complex.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the template to check.</param>
        public virtual bool IsTemplateComplex(IFocusNodeStateView stateView, string propertyName)
        {
            IFocusNodeState State = stateView.State;
            Debug.Assert(State.InnerTable.ContainsKey(propertyName));

            IFocusPlaceholderInner ParentInner = State.InnerTable[propertyName] as IFocusPlaceholderInner;
            Debug.Assert(ParentInner != null);

            NodeTreeHelperChild.GetChildNode(stateView.State.Node, propertyName, out Node ChildNode);
            Debug.Assert(ChildNode != null);

            Type NodeType = Type.FromGetType(ChildNode);

            //Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(NodeType);
            //Debug.Assert(TemplateSet.NodeTemplateTable.ContainsKey(InterfaceType));
            Debug.Assert(TemplateSet.NodeTemplateTable.ContainsKey(NodeType));

            IFocusNodeTemplate ParentTemplate = TemplateSet.NodeTemplateTable[NodeType] as IFocusNodeTemplate;
            Debug.Assert(ParentTemplate != null);

            return ParentTemplate.IsComplex;
        }

        /// <summary>
        /// Checks if the collection associated to the <paramref name="propertyName"/> property of the <paramref name="stateView"/> state has more than <paramref name="count"/> item.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the collection to check.</param>
        /// <param name="count">The number of items.</param>
        public virtual bool CollectionHasItems(IFocusNodeStateView stateView, string propertyName, int count)
        {
            IFocusNodeState State = stateView.State;
            Debug.Assert(State.InnerTable.ContainsKey(propertyName));

            bool IsHandled = false;
            bool HasItems = false;

            switch (State.InnerTable[propertyName])
            {
                case IFocusListInner AsListInner:
                    HasItems = AsListInner.Count > count;
                    IsHandled = true;
                    break;

                case IFocusBlockListInner AsBlockListInner:
                    HasItems = AsBlockListInner.Count > count;
                    IsHandled = true;
                    break;
            }

            Debug.Assert(IsHandled);

            return HasItems;
        }

        /// <summary>
        /// Checks if the optional node associated to <paramref name="propertyName"/> is assigned.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the node to check.</param>
        public virtual bool IsOptionalNodeAssigned(IFocusNodeStateView stateView, string propertyName)
        {
            IFocusNodeState State = stateView.State;
            Debug.Assert(State.InnerTable.ContainsKey(propertyName));

            IFocusOptionalInner OptionalInner = State.InnerTable[propertyName] as IFocusOptionalInner;
            Debug.Assert(OptionalInner != null);

            return OptionalInner.IsAssigned;
        }

        /// <summary>
        /// Checks if the enum or boolean associated to the <paramref name="propertyName"/> property of the <paramref name="stateView"/> state has value <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the template to check.</param>
        /// <param name="defaultValue">Expected default value.</param>
        public virtual bool DiscreteHasDefaultValue(IFocusNodeStateView stateView, string propertyName, int defaultValue)
        {
            IFocusNodeState State = stateView.State;
            Debug.Assert(State.ValuePropertyTypeTable.ContainsKey(propertyName));

            bool IsHandled = false;
            bool Result = false;

            switch (State.ValuePropertyTypeTable[propertyName])
            {
                case ValuePropertyType.Boolean:
                case ValuePropertyType.Enum:
                    Result = NodeTreeHelper.GetEnumValue(State.Node, propertyName) == defaultValue;
                    IsHandled = true;
                    break;
            }

            Debug.Assert(IsHandled);

            return Result;
        }

        /// <summary>
        /// Checks if the <paramref name="stateView"/> state is the first in a collection in the parent.
        /// </summary>
        /// <param name="stateView">The state view.</param>
        public virtual bool IsFirstItem(IFocusNodeStateView stateView)
        {
            Contract.RequireNotNull(stateView, out IFocusNodeStateView StateView);

            IFocusNodeState State = StateView.State;
            Debug.Assert(State != null);

            IFocusInner ParentInner = State.ParentInner;

            IFocusPlaceholderNodeState PlaceholderNodeState;
            FocusPlaceholderNodeStateReadOnlyList StateList;
            int Index;
            bool Result;

            switch (ParentInner)
            {
                case IFocusListInner AsListInner:
                    PlaceholderNodeState = State as IFocusPlaceholderNodeState;
                    Debug.Assert(PlaceholderNodeState != null);

                    StateList = AsListInner.StateList;
                    Index = StateList.IndexOf(PlaceholderNodeState);
                    Debug.Assert(Index >= 0 && Index < StateList.Count);
                    Result = Index == 0;
                    break;

                case IFocusBlockListInner AsBlockListInner:
                    PlaceholderNodeState = State as IFocusPlaceholderNodeState;
                    Debug.Assert(PlaceholderNodeState != null);

                    Result = false;
                    for (int BlockIndex = 0; BlockIndex < AsBlockListInner.BlockStateList.Count; BlockIndex++)
                    {
                        StateList = (FocusPlaceholderNodeStateReadOnlyList)AsBlockListInner.BlockStateList[BlockIndex].StateList;
                        Index = StateList.IndexOf(PlaceholderNodeState);
                        if (Index >= 0)
                        {
                            Debug.Assert(Index < StateList.Count);
                            Result = BlockIndex == 0 && Index == 0;
                        }
                    }
                    break;

                default:
                    Result = true;
                    break;
            }

            return Result;
        }

        /// <summary>
        /// Checks if the <paramref name="blockStateView"/> block state belongs to a replicated block.
        /// </summary>
        /// <param name="blockStateView">The block state view.</param>
        public virtual bool IsInReplicatedBlock(FocusBlockStateView blockStateView)
        {
            IFocusBlockState BlockState = blockStateView.BlockState;
            Debug.Assert(BlockState != null);

            return BlockState.ChildBlock.Replication == BaseNode.ReplicationStatus.Replicated;
        }

        /// <summary>
        /// Checks if the string associated to the <paramref name="propertyName"/> property of the <paramref name="stateView"/> state matches the pattern in <paramref name="textPattern"/>.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the template to check.</param>
        /// <param name="textPattern">Expected text.</param>
        public virtual bool StringMatchTextPattern(IFocusNodeStateView stateView, string propertyName, string textPattern)
        {
            IFocusNodeState State = stateView.State;
            Debug.Assert(State.InnerTable.ContainsKey(propertyName));

            bool IsHandled = false;
            bool Result = false;

            switch (State.InnerTable[propertyName])
            {
                case IFocusPlaceholderInner AsPlaceholderInner:
                    Debug.Assert(AsPlaceholderInner.InterfaceType.IsTypeof<Identifier>());
                    IFocusPlaceholderNodeState ChildState = AsPlaceholderInner.ChildState as IFocusPlaceholderNodeState;
                    Debug.Assert(ChildState != null);
                    Result = NodeTreeHelper.GetString(ChildState.Node, nameof(Identifier.Text)) == textPattern;
                    IsHandled = true;
                    break;
            }

            Debug.Assert(IsHandled);
            return Result;
        }
    }
}
