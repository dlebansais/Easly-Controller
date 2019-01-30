namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.ReadOnly;

    /// <summary>
    /// State of a source identifier node.
    /// </summary>
    public interface IWriteableSourceState : IReadOnlySourceState, IWriteablePlaceholderNodeState
    {
        /// <summary>
        /// The parent block state.
        /// </summary>
        new IWriteableBlockState ParentBlockState { get; }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IWriteableBrowsingSourceIndex ParentIndex { get; }
    }

    /// <summary>
    /// State of a source identifier node.
    /// </summary>
    internal class WriteableSourceState : ReadOnlySourceState, IWriteableSourceState
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableSourceState"/> class.
        /// </summary>
        /// <param name="parentBlockState">The parent block state.</param>
        /// <param name="index">The index used to create the state.</param>
        public WriteableSourceState(IWriteableBlockState parentBlockState, IWriteableBrowsingSourceIndex index)
            : base(parentBlockState, index)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The parent block state.
        /// </summary>
        public new IWriteableBlockState ParentBlockState { get { return (IWriteableBlockState)base.ParentBlockState; } }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new IWriteableBrowsingSourceIndex ParentIndex { get { return (IWriteableBrowsingSourceIndex)base.ParentIndex; } }
        IWriteableIndex IWriteableNodeState.ParentIndex { get { return ParentIndex; } }
        IWriteableNodeIndex IWriteablePlaceholderNodeState.ParentIndex { get { return ParentIndex; } }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        public new IWriteableInner<IWriteableBrowsingChildIndex> ParentInner { get { return (IWriteableInner<IWriteableBrowsingChildIndex>)base.ParentInner; } }

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
            ControllerTools.AssertNoOverride(this, typeof(WriteableSourceState));
            return new WriteableNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxNodeStateReadOnlyList object.
        /// </summary>
        private protected override IReadOnlyNodeStateReadOnlyList CreateNodeStateReadOnlyList(IReadOnlyNodeStateList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableSourceState));
            return new WriteableNodeStateReadOnlyList((IWriteableNodeStateList)list);
        }

        /// <summary>
        /// Creates a IxxxBrowsingPlaceholderNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingPlaceholderNodeIndex CreateChildNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableSourceState));
            return new WriteableBrowsingPlaceholderNodeIndex(node, childNode, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingOptionalNodeIndex CreateOptionalNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableSourceState));
            return new WriteableBrowsingOptionalNodeIndex(node, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingListNodeIndex CreateListNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, INode childNode, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableSourceState));
            return new WriteableBrowsingListNodeIndex(node, childNode, propertyName, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, INodeTreeBlock childBlock, int blockIndex, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableSourceState));
            return new WriteableBrowsingNewBlockNodeIndex(node, childNode, propertyName, blockIndex, childBlock.ReplicationPattern, childBlock.SourceIdentifier);
        }

        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, int blockIndex, int index, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableSourceState));
            return new WriteableBrowsingExistingBlockNodeIndex(node, childNode, propertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingPlaceholderNodeIndex.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreatePlaceholderIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingPlaceholderNodeIndex childNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableSourceState));
            return new WriteableIndexCollection<IWriteableBrowsingPlaceholderNodeIndex>(propertyName, new List<IWriteableBrowsingPlaceholderNodeIndex>() { (IWriteableBrowsingPlaceholderNodeIndex)childNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingOptionalNodeIndex.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateOptionalIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingOptionalNodeIndex optionalNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableSourceState));
            return new WriteableIndexCollection<IWriteableBrowsingOptionalNodeIndex>(propertyName, new List<IWriteableBrowsingOptionalNodeIndex>() { (IWriteableBrowsingOptionalNodeIndex)optionalNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndexList object.
        /// </summary>
        private protected override IReadOnlyBrowsingListNodeIndexList CreateBrowsingListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableSourceState));
            return new WriteableBrowsingListNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingListNodeIndex objects.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateListIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingListNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableSourceState));
            return new WriteableIndexCollection<IWriteableBrowsingListNodeIndex>(propertyName, (IWriteableBrowsingListNodeIndexList)nodeIndexList);
        }

        /// <summary>
        /// Creates a IxxxBrowsingBlockNodeIndexList object.
        /// </summary>
        private protected override IReadOnlyBrowsingBlockNodeIndexList CreateBrowsingBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableSourceState));
            return new WriteableBrowsingBlockNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingBlockNodeIndex objects.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateBlockIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingBlockNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableSourceState));
            return new WriteableIndexCollection<IWriteableBrowsingBlockNodeIndex>(propertyName, (IWriteableBrowsingBlockNodeIndexList)nodeIndexList);
        }
        #endregion
    }
}
