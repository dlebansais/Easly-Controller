namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// State of a source identifier node.
    /// </summary>
    public interface IFrameSourceState : IWriteableSourceState, IFramePlaceholderNodeState
    {
        /// <summary>
        /// The parent block state.
        /// </summary>
        new IFrameBlockState ParentBlockState { get; }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IFrameBrowsingSourceIndex ParentIndex { get; }
    }

    /// <summary>
    /// State of a source identifier node.
    /// </summary>
    internal class FrameSourceState : WriteableSourceState, IFrameSourceState
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameSourceState"/> class.
        /// </summary>
        /// <param name="parentBlockState">The parent block state.</param>
        /// <param name="index">The index used to create the state.</param>
        public FrameSourceState(IFrameBlockState parentBlockState, IFrameBrowsingSourceIndex index)
            : base(parentBlockState, index)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The parent block state.
        /// </summary>
        public new IFrameBlockState ParentBlockState { get { return (IFrameBlockState)base.ParentBlockState; } }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new IFrameBrowsingSourceIndex ParentIndex { get { return (IFrameBrowsingSourceIndex)base.ParentIndex; } }
        IFrameIndex IFrameNodeState.ParentIndex { get { return ParentIndex; } }
        IFrameNodeIndex IFramePlaceholderNodeState.ParentIndex { get { return ParentIndex; } }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        public new IFrameInner<IFrameBrowsingChildIndex> ParentInner { get { return (IFrameInner<IFrameBrowsingChildIndex>)base.ParentInner; } }

        /// <summary>
        /// State of the parent.
        /// </summary>
        public new IFrameNodeState ParentState { get { return (IFrameNodeState)base.ParentState; } }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        public new IFrameInnerReadOnlyDictionary<string> InnerTable { get { return (IFrameInnerReadOnlyDictionary<string>)base.InnerTable; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeStateList object.
        /// </summary>
        private protected override IReadOnlyNodeStateList CreateNodeStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameSourceState));
            return new FrameNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxNodeStateReadOnlyList object.
        /// </summary>
        private protected override IReadOnlyNodeStateReadOnlyList CreateNodeStateReadOnlyList(IReadOnlyNodeStateList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameSourceState));
            return new FrameNodeStateReadOnlyList((IFrameNodeStateList)list);
        }

        /// <summary>
        /// Creates a IxxxBrowsingPlaceholderNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingPlaceholderNodeIndex CreateChildNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameSourceState));
            return new FrameBrowsingPlaceholderNodeIndex(node, childNode, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingOptionalNodeIndex CreateOptionalNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameSourceState));
            return new FrameBrowsingOptionalNodeIndex(node, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingListNodeIndex CreateListNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, INode childNode, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameSourceState));
            return new FrameBrowsingListNodeIndex(node, childNode, propertyName, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, int blockIndex, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameSourceState));
            return new FrameBrowsingNewBlockNodeIndex(node, childNode, propertyName, blockIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, int blockIndex, int index, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameSourceState));
            return new FrameBrowsingExistingBlockNodeIndex(node, childNode, propertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingPlaceholderNodeIndex.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreatePlaceholderIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingPlaceholderNodeIndex childNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameSourceState));
            return new FrameIndexCollection<IFrameBrowsingPlaceholderNodeIndex>(propertyName, new List<IFrameBrowsingPlaceholderNodeIndex>() { (IFrameBrowsingPlaceholderNodeIndex)childNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingOptionalNodeIndex.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateOptionalIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingOptionalNodeIndex optionalNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameSourceState));
            return new FrameIndexCollection<IFrameBrowsingOptionalNodeIndex>(propertyName, new List<IFrameBrowsingOptionalNodeIndex>() { (IFrameBrowsingOptionalNodeIndex)optionalNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndexList object.
        /// </summary>
        private protected override IReadOnlyBrowsingListNodeIndexList CreateBrowsingListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameSourceState));
            return new FrameBrowsingListNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingListNodeIndex objects.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateListIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingListNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameSourceState));
            return new FrameIndexCollection<IFrameBrowsingListNodeIndex>(propertyName, (IFrameBrowsingListNodeIndexList)nodeIndexList);
        }

        /// <summary>
        /// Creates a IxxxBrowsingBlockNodeIndexList object.
        /// </summary>
        private protected override IReadOnlyBrowsingBlockNodeIndexList CreateBrowsingBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameSourceState));
            return new FrameBrowsingBlockNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingBlockNodeIndex objects.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateBlockIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingBlockNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameSourceState));
            return new FrameIndexCollection<IFrameBrowsingBlockNodeIndex>(propertyName, (IFrameBrowsingBlockNodeIndexList)nodeIndexList);
        }
        #endregion
    }
}
