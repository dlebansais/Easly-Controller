namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;

    /// <summary>
    /// State of an child node.
    /// </summary>
    public interface ILayoutPlaceholderNodeState : IFocusPlaceholderNodeState, ILayoutNodeState, ILayoutCyclableNodeState
    {
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new ILayoutNodeIndex ParentIndex { get; }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        new ILayoutInner ParentInner { get; }
    }

    /// <summary>
    /// State of an child node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal interface ILayoutPlaceholderNodeState<out IInner> : IFocusPlaceholderNodeState<IInner>, ILayoutNodeState<IInner>
        where IInner : ILayoutInner<ILayoutBrowsingChildIndex>
    {
    }

    /// <summary>
    /// State of an child node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal class LayoutPlaceholderNodeState<IInner> : FocusPlaceholderNodeState<IInner>, ILayoutPlaceholderNodeState<IInner>, ILayoutPlaceholderNodeState
        where IInner : ILayoutInner<ILayoutBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutPlaceholderNodeState{IInner}"/> class.
        /// </summary>
        /// <param name="parentIndex">The index used to create the state.</param>
        public LayoutPlaceholderNodeState(ILayoutNodeIndex parentIndex)
            : base(parentIndex)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new ILayoutNodeIndex ParentIndex { get { return (ILayoutNodeIndex)base.ParentIndex; } }
        ILayoutIndex ILayoutNodeState.ParentIndex { get { return ParentIndex; } }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        public new ILayoutInner ParentInner { get { return (ILayoutInner)base.ParentInner; } }

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
            ControllerTools.AssertNoOverride(this, typeof(LayoutPlaceholderNodeState<IInner>));
            return new LayoutNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxBrowsingPlaceholderNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingPlaceholderNodeIndex CreateChildNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, Node childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutPlaceholderNodeState<IInner>));
            return new LayoutBrowsingPlaceholderNodeIndex(node, childNode, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingOptionalNodeIndex CreateOptionalNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutPlaceholderNodeState<IInner>));
            return new LayoutBrowsingOptionalNodeIndex(node, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingListNodeIndex CreateListNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, Node childNode, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutPlaceholderNodeState<IInner>));
            return new LayoutBrowsingListNodeIndex(node, childNode, propertyName, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, int blockIndex, Node childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutPlaceholderNodeState<IInner>));
            return new LayoutBrowsingNewBlockNodeIndex(node, childNode, propertyName, blockIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, int blockIndex, int index, Node childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutPlaceholderNodeState<IInner>));
            return new LayoutBrowsingExistingBlockNodeIndex(node, childNode, propertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingPlaceholderNodeIndex.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreatePlaceholderIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingPlaceholderNodeIndex childNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutPlaceholderNodeState<IInner>));
            return new LayoutIndexCollection<ILayoutBrowsingPlaceholderNodeIndex>(propertyName, new List<ILayoutBrowsingPlaceholderNodeIndex>() { (ILayoutBrowsingPlaceholderNodeIndex)childNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingOptionalNodeIndex.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateOptionalIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingOptionalNodeIndex optionalNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutPlaceholderNodeState<IInner>));
            return new LayoutIndexCollection<ILayoutBrowsingOptionalNodeIndex>(propertyName, new List<ILayoutBrowsingOptionalNodeIndex>() { (ILayoutBrowsingOptionalNodeIndex)optionalNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndexList object.
        /// </summary>
        private protected override ReadOnlyBrowsingListNodeIndexList CreateBrowsingListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutPlaceholderNodeState<IInner>));
            return new LayoutBrowsingListNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingListNodeIndex objects.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateListIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, ReadOnlyBrowsingListNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutPlaceholderNodeState<IInner>));
            return new LayoutIndexCollection<ILayoutBrowsingListNodeIndex>(propertyName, (LayoutBrowsingListNodeIndexList)nodeIndexList);
        }

        /// <summary>
        /// Creates a IxxxBrowsingBlockNodeIndexList object.
        /// </summary>
        private protected override ReadOnlyBrowsingBlockNodeIndexList CreateBrowsingBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutPlaceholderNodeState<IInner>));
            return new LayoutBrowsingBlockNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingBlockNodeIndex objects.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateBlockIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, ReadOnlyBrowsingBlockNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutPlaceholderNodeState<IInner>));
            return new LayoutIndexCollection<ILayoutBrowsingBlockNodeIndex>(propertyName, (LayoutBrowsingBlockNodeIndexList)nodeIndexList);
        }

        /// <summary>
        /// Creates a IxxxInsertionChildNodeIndexList object.
        /// </summary>
        private protected override FocusInsertionChildNodeIndexList CreateInsertionChildIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutPlaceholderNodeState<IInner>));
            return new LayoutInsertionChildNodeIndexList();
        }
        #endregion
    }
}
