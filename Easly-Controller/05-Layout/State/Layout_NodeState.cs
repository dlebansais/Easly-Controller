namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Base interface for the state of a node.
    /// </summary>
    public interface ILayoutNodeState : IFocusNodeState
    {
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new ILayoutIndex ParentIndex { get; }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        new ILayoutInner ParentInner { get; }

        /// <summary>
        /// State of the parent.
        /// </summary>
        new ILayoutNodeState ParentState { get; }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        new ILayoutInnerReadOnlyDictionary<string> InnerTable { get; }
    }

    /// <summary>
    /// Base interface for the state of a node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal interface ILayoutNodeState<out IInner> : IFocusNodeState<IInner>
        where IInner : ILayoutInner<ILayoutBrowsingChildIndex>
    {
    }

    /// <summary>
    /// Base class for the state of a node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal abstract class LayoutNodeState<IInner> : FocusNodeState<IInner>, ILayoutNodeState<IInner>, ILayoutNodeState
        where IInner : ILayoutInner<ILayoutBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutNodeState{IInner}"/> class.
        /// </summary>
        /// <param name="parentIndex">The index used to create the state.</param>
        public LayoutNodeState(ILayoutIndex parentIndex)
            : base(parentIndex)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new ILayoutIndex ParentIndex { get { return (ILayoutIndex)base.ParentIndex; } }

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
        public new ILayoutInnerReadOnlyDictionary<string> InnerTable { get { return (ILayoutInnerReadOnlyDictionary<string>)base.InnerTable; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeStateList object.
        /// </summary>
        private protected override IReadOnlyNodeStateList CreateNodeStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutNodeState<IInner>));
            return new LayoutNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxBrowsingPlaceholderNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingPlaceholderNodeIndex CreateChildNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutNodeState<IInner>));
            return new LayoutBrowsingPlaceholderNodeIndex(node, childNode, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingOptionalNodeIndex CreateOptionalNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutNodeState<IInner>));
            return new LayoutBrowsingOptionalNodeIndex(node, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingListNodeIndex CreateListNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, INode childNode, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutNodeState<IInner>));
            return new LayoutBrowsingListNodeIndex(node, childNode, propertyName, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, int blockIndex, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutNodeState<IInner>));
            return new LayoutBrowsingNewBlockNodeIndex(node, childNode, propertyName, blockIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, int blockIndex, int index, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutNodeState<IInner>));
            return new LayoutBrowsingExistingBlockNodeIndex(node, childNode, propertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingPlaceholderNodeIndex.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreatePlaceholderIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingPlaceholderNodeIndex childNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutNodeState<IInner>));
            return new LayoutIndexCollection<ILayoutBrowsingPlaceholderNodeIndex>(propertyName, new List<ILayoutBrowsingPlaceholderNodeIndex>() { (ILayoutBrowsingPlaceholderNodeIndex)childNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingOptionalNodeIndex.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateOptionalIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingOptionalNodeIndex optionalNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutNodeState<IInner>));
            return new LayoutIndexCollection<ILayoutBrowsingOptionalNodeIndex>(propertyName, new List<ILayoutBrowsingOptionalNodeIndex>() { (ILayoutBrowsingOptionalNodeIndex)optionalNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndexList object.
        /// </summary>
        private protected override IReadOnlyBrowsingListNodeIndexList CreateBrowsingListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutNodeState<IInner>));
            return new LayoutBrowsingListNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingListNodeIndex objects.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateListIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingListNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutNodeState<IInner>));
            return new LayoutIndexCollection<ILayoutBrowsingListNodeIndex>(propertyName, (ILayoutBrowsingListNodeIndexList)nodeIndexList);
        }

        /// <summary>
        /// Creates a IxxxBrowsingBlockNodeIndexList object.
        /// </summary>
        private protected override IReadOnlyBrowsingBlockNodeIndexList CreateBrowsingBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutNodeState<IInner>));
            return new LayoutBrowsingBlockNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingBlockNodeIndex objects.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateBlockIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingBlockNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutNodeState<IInner>));
            return new LayoutIndexCollection<ILayoutBrowsingBlockNodeIndex>(propertyName, (ILayoutBrowsingBlockNodeIndexList)nodeIndexList);
        }
        #endregion
    }
}
