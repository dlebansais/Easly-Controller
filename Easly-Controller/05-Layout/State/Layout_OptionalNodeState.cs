namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;
    using NotNullReflection;

    /// <summary>
    /// State of an optional node.
    /// </summary>
    public interface ILayoutOptionalNodeState : IFocusOptionalNodeState, ILayoutNodeState, ILayoutCyclableNodeState
    {
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new ILayoutBrowsingOptionalNodeIndex ParentIndex { get; }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        new ILayoutOptionalInner ParentInner { get; }
    }

    /// <summary>
    /// State of an optional node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal interface ILayoutOptionalNodeState<out IInner> : IFocusOptionalNodeState<IInner>, ILayoutNodeState<IInner>
        where IInner : ILayoutInner<ILayoutBrowsingChildIndex>
    {
    }

    /// <summary>
    /// State of an optional node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal class LayoutOptionalNodeState<IInner> : FocusOptionalNodeState<IInner>, ILayoutOptionalNodeState<IInner>, ILayoutOptionalNodeState
        where IInner : ILayoutInner<ILayoutBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutOptionalNodeState{IInner}"/> class.
        /// </summary>
        /// <param name="parentIndex">The index used to create the state.</param>
        public LayoutOptionalNodeState(ILayoutBrowsingOptionalNodeIndex parentIndex)
            : base(parentIndex)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new ILayoutBrowsingOptionalNodeIndex ParentIndex { get { return (ILayoutBrowsingOptionalNodeIndex)base.ParentIndex; } }
        ILayoutIndex ILayoutNodeState.ParentIndex { get { return ParentIndex; } }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        public new ILayoutOptionalInner ParentInner { get { return (ILayoutOptionalInner)base.ParentInner; } }
        ILayoutInner ILayoutNodeState.ParentInner { get { return ParentInner; } }

        /// <summary>
        /// State of the parent.
        /// </summary>
        public new ILayoutNodeState ParentState { get { return (ILayoutNodeState)base.ParentState; } }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        public new LayoutInnerReadOnlyDictionary<string> InnerTable { get { return (LayoutInnerReadOnlyDictionary<string>)base.InnerTable; } }

        /// <summary>
        /// List of node indexes that can replace the current node. Can be null.
        /// Applies only to bodies and features.
        /// </summary>
        public new LayoutInsertionChildNodeIndexList CycleIndexList { get { return (LayoutInsertionChildNodeIndexList)base.CycleIndexList; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeStateList object.
        /// </summary>
        private protected override ReadOnlyNodeStateList CreateNodeStateList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutOptionalNodeState<IInner>>());
            return new LayoutNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxBrowsingPlaceholderNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingPlaceholderNodeIndex CreateChildNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, Node childNode)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutOptionalNodeState<IInner>>());
            return new LayoutBrowsingPlaceholderNodeIndex(node, childNode, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingOptionalNodeIndex CreateOptionalNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutOptionalNodeState<IInner>>());
            return new LayoutBrowsingOptionalNodeIndex(node, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingListNodeIndex CreateListNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, Node childNode, int index)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutOptionalNodeState<IInner>>());
            return new LayoutBrowsingListNodeIndex(node, childNode, propertyName, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, int blockIndex, Node childNode)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutOptionalNodeState<IInner>>());
            return new LayoutBrowsingNewBlockNodeIndex(node, childNode, propertyName, blockIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, int blockIndex, int index, Node childNode)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutOptionalNodeState<IInner>>());
            return new LayoutBrowsingExistingBlockNodeIndex(node, childNode, propertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingPlaceholderNodeIndex.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreatePlaceholderIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingPlaceholderNodeIndex childNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutOptionalNodeState<IInner>>());
            return new LayoutIndexCollection<ILayoutBrowsingPlaceholderNodeIndex>(propertyName, new List<ILayoutBrowsingPlaceholderNodeIndex>() { (ILayoutBrowsingPlaceholderNodeIndex)childNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingOptionalNodeIndex.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateOptionalIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingOptionalNodeIndex optionalNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutOptionalNodeState<IInner>>());
            return new LayoutIndexCollection<ILayoutBrowsingOptionalNodeIndex>(propertyName, new List<ILayoutBrowsingOptionalNodeIndex>() { (ILayoutBrowsingOptionalNodeIndex)optionalNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndexList object.
        /// </summary>
        private protected override ReadOnlyBrowsingListNodeIndexList CreateBrowsingListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutOptionalNodeState<IInner>>());
            return new LayoutBrowsingListNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingListNodeIndex objects.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateListIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, ReadOnlyBrowsingListNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutOptionalNodeState<IInner>>());
            return new LayoutIndexCollection<ILayoutBrowsingListNodeIndex>(propertyName, (LayoutBrowsingListNodeIndexList)nodeIndexList);
        }

        /// <summary>
        /// Creates a IxxxBrowsingBlockNodeIndexList object.
        /// </summary>
        private protected override ReadOnlyBrowsingBlockNodeIndexList CreateBrowsingBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutOptionalNodeState<IInner>>());
            return new LayoutBrowsingBlockNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingBlockNodeIndex objects.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateBlockIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, ReadOnlyBrowsingBlockNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutOptionalNodeState<IInner>>());
            return new LayoutIndexCollection<ILayoutBrowsingBlockNodeIndex>(propertyName, (LayoutBrowsingBlockNodeIndexList)nodeIndexList);
        }

        /// <summary>
        /// Creates a IxxxInsertionChildIndexNodeList object.
        /// </summary>
        private protected override FocusInsertionChildNodeIndexList CreateInsertionChildIndexList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutOptionalNodeState<IInner>>());
            return new LayoutInsertionChildNodeIndexList();
        }
        #endregion
    }
}
