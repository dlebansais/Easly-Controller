namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using BaseNode;
    using EaslyController.ReadOnly;

    /// <summary>
    /// State of an optional node.
    /// </summary>
    public interface IWriteableOptionalNodeState : IReadOnlyOptionalNodeState, IWriteableNodeState
    {
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IWriteableBrowsingOptionalNodeIndex ParentIndex { get; }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        new IWriteableOptionalInner ParentInner { get; }
    }

    /// <summary>
    /// State of an optional node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal interface IWriteableOptionalNodeState<out IInner> : IReadOnlyOptionalNodeState<IInner>, IWriteableNodeState<IInner>
        where IInner : IWriteableInner<IWriteableBrowsingChildIndex>
    {
    }

    /// <summary>
    /// State of an optional node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal class WriteableOptionalNodeState<IInner> : ReadOnlyOptionalNodeState<IInner>, IWriteableOptionalNodeState<IInner>, IWriteableOptionalNodeState
        where IInner : IWriteableInner<IWriteableBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableOptionalNodeState{IInner}"/> class.
        /// </summary>
        /// <param name="parentIndex">The index used to create the state.</param>
        public WriteableOptionalNodeState(IWriteableBrowsingOptionalNodeIndex parentIndex)
            : base(parentIndex)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new IWriteableBrowsingOptionalNodeIndex ParentIndex { get { return (IWriteableBrowsingOptionalNodeIndex)base.ParentIndex; } }
        IWriteableIndex IWriteableNodeState.ParentIndex { get { return ParentIndex; } }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        public new IWriteableOptionalInner ParentInner { get { return (IWriteableOptionalInner)base.ParentInner; } }
        IWriteableInner IWriteableNodeState.ParentInner { get { return ParentInner; } }

        /// <summary>
        /// State of the parent.
        /// </summary>
        public new IWriteableNodeState ParentState { get { return (IWriteableNodeState)base.ParentState; } }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        public new IWriteableInnerReadOnlyDictionary<string> InnerTable { get { return (IWriteableInnerReadOnlyDictionary<string>)base.InnerTable; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeStateList object.
        /// </summary>
        private protected override IReadOnlyNodeStateList CreateNodeStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxBrowsingPlaceholderNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingPlaceholderNodeIndex CreateChildNodeIndex(IReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, Node childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableBrowsingPlaceholderNodeIndex(node, childNode, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingOptionalNodeIndex CreateOptionalNodeIndex(IReadOnlyBrowseContext browseNodeContext, Node node, string propertyName)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableBrowsingOptionalNodeIndex(node, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingListNodeIndex CreateListNodeIndex(IReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, Node childNode, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableBrowsingListNodeIndex(node, childNode, propertyName, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(IReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, int blockIndex, Node childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableBrowsingNewBlockNodeIndex(node, childNode, propertyName, blockIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockNodeIndex(IReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, int blockIndex, int index, Node childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableBrowsingExistingBlockNodeIndex(node, childNode, propertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingPlaceholderNodeIndex.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreatePlaceholderIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingPlaceholderNodeIndex childNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableIndexCollection<IWriteableBrowsingPlaceholderNodeIndex>(propertyName, new List<IWriteableBrowsingPlaceholderNodeIndex>() { (IWriteableBrowsingPlaceholderNodeIndex)childNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingOptionalNodeIndex.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateOptionalIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingOptionalNodeIndex optionalNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableIndexCollection<IWriteableBrowsingOptionalNodeIndex>(propertyName, new List<IWriteableBrowsingOptionalNodeIndex>() { (IWriteableBrowsingOptionalNodeIndex)optionalNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndexList object.
        /// </summary>
        private protected override IReadOnlyBrowsingListNodeIndexList CreateBrowsingListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableBrowsingListNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingListNodeIndex objects.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateListIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingListNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableIndexCollection<IWriteableBrowsingListNodeIndex>(propertyName, (IWriteableBrowsingListNodeIndexList)nodeIndexList);
        }

        /// <summary>
        /// Creates a IxxxBrowsingBlockNodeIndexList object.
        /// </summary>
        private protected override IReadOnlyBrowsingBlockNodeIndexList CreateBrowsingBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableBrowsingBlockNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingBlockNodeIndex objects.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateBlockIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingBlockNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableIndexCollection<IWriteableBrowsingBlockNodeIndex>(propertyName, (IWriteableBrowsingBlockNodeIndexList)nodeIndexList);
        }
        #endregion
    }
}
