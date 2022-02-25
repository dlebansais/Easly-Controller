namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using BaseNode;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <summary>
    /// State of an child node.
    /// </summary>
    public interface IFramePlaceholderNodeState : IWriteablePlaceholderNodeState, IFrameNodeState
    {
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IFrameNodeIndex ParentIndex { get; }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        new IFrameInner ParentInner { get; }
    }

    /// <summary>
    /// State of an child node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal interface IFramePlaceholderNodeState<out IInner> : IWriteablePlaceholderNodeState<IInner>, IFrameNodeState<IInner>
        where IInner : IFrameInner<IFrameBrowsingChildIndex>
    {
    }

    /// <summary>
    /// State of an child node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal class FramePlaceholderNodeState<IInner> : WriteablePlaceholderNodeState<IInner>, IFramePlaceholderNodeState<IInner>, IFramePlaceholderNodeState
        where IInner : IFrameInner<IFrameBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FramePlaceholderNodeState{IInner}"/> object.
        /// </summary>
        public static new FramePlaceholderNodeState<IInner> Empty { get; } = new FramePlaceholderNodeState<IInner>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FramePlaceholderNodeState{IInner}"/> class.
        /// </summary>
        protected FramePlaceholderNodeState()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FramePlaceholderNodeState{IInner}"/> class.
        /// </summary>
        /// <param name="parentIndex">The index used to create the state.</param>
        public FramePlaceholderNodeState(IFrameNodeIndex parentIndex)
            : base(parentIndex)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new IFrameNodeIndex ParentIndex { get { return (IFrameNodeIndex)base.ParentIndex; } }
        IFrameIndex IFrameNodeState.ParentIndex { get { return ParentIndex; } }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        public new IFrameInner ParentInner { get { return (IFrameInner)base.ParentInner; } }

        /// <summary>
        /// State of the parent.
        /// </summary>
        public new IFrameNodeState ParentState { get { return (IFrameNodeState)base.ParentState; } }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        public new FrameInnerReadOnlyDictionary<string> InnerTable { get { return (FrameInnerReadOnlyDictionary<string>)base.InnerTable; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeStateList object.
        /// </summary>
        private protected override ReadOnlyNodeStateList CreateNodeStateList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FramePlaceholderNodeState<IInner>>());
            return new FrameNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxBrowsingPlaceholderNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingPlaceholderNodeIndex CreateChildNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, Node childNode)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FramePlaceholderNodeState<IInner>>());
            return new FrameBrowsingPlaceholderNodeIndex(node, childNode, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingOptionalNodeIndex CreateOptionalNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FramePlaceholderNodeState<IInner>>());
            return new FrameBrowsingOptionalNodeIndex(node, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingListNodeIndex CreateListNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, Node childNode, int index)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FramePlaceholderNodeState<IInner>>());
            return new FrameBrowsingListNodeIndex(node, childNode, propertyName, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, int blockIndex, Node childNode)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FramePlaceholderNodeState<IInner>>());
            return new FrameBrowsingNewBlockNodeIndex(node, childNode, propertyName, blockIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, int blockIndex, int index, Node childNode)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FramePlaceholderNodeState<IInner>>());
            return new FrameBrowsingExistingBlockNodeIndex(node, childNode, propertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingPlaceholderNodeIndex.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreatePlaceholderIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingPlaceholderNodeIndex childNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FramePlaceholderNodeState<IInner>>());
            return new FrameIndexCollection<IFrameBrowsingPlaceholderNodeIndex>(propertyName, new List<IFrameBrowsingPlaceholderNodeIndex>() { (IFrameBrowsingPlaceholderNodeIndex)childNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingOptionalNodeIndex.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateOptionalIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingOptionalNodeIndex optionalNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FramePlaceholderNodeState<IInner>>());
            return new FrameIndexCollection<IFrameBrowsingOptionalNodeIndex>(propertyName, new List<IFrameBrowsingOptionalNodeIndex>() { (IFrameBrowsingOptionalNodeIndex)optionalNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndexList object.
        /// </summary>
        private protected override ReadOnlyBrowsingListNodeIndexList CreateBrowsingListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FramePlaceholderNodeState<IInner>>());
            return new FrameBrowsingListNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingListNodeIndex objects.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateListIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, ReadOnlyBrowsingListNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FramePlaceholderNodeState<IInner>>());
            return new FrameIndexCollection<IFrameBrowsingListNodeIndex>(propertyName, (FrameBrowsingListNodeIndexList)nodeIndexList);
        }

        /// <summary>
        /// Creates a IxxxBrowsingBlockNodeIndexList object.
        /// </summary>
        private protected override ReadOnlyBrowsingBlockNodeIndexList CreateBrowsingBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FramePlaceholderNodeState<IInner>>());
            return new FrameBrowsingBlockNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingBlockNodeIndex objects.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateBlockIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, ReadOnlyBrowsingBlockNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FramePlaceholderNodeState<IInner>>());
            return new FrameIndexCollection<IFrameBrowsingBlockNodeIndex>(propertyName, (FrameBrowsingBlockNodeIndexList)nodeIndexList);
        }
        #endregion
    }
}
