namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using BaseNode;
    using BaseNodeHelper;
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
        new IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> ParentInner { get; }
    }

    /// <summary>
    /// State of an optional node.
    /// </summary>
    public class WriteableOptionalNodeState : ReadOnlyOptionalNodeState, IWriteableOptionalNodeState
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableOptionalNodeState"/> class.
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
        public new IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> ParentInner { get { return (IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex>)base.ParentInner; } }
        IWriteableInner<IWriteableBrowsingChildIndex> IWriteableNodeState.ParentInner { get { return ParentInner; } }

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
        protected override IReadOnlyNodeStateList CreateNodeStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState));
            return new WriteableNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxNodeStateReadOnlyList object.
        /// </summary>
        protected override IReadOnlyNodeStateReadOnlyList CreateNodeStateReadOnlyList(IReadOnlyNodeStateList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState));
            return new WriteableNodeStateReadOnlyList((IWriteableNodeStateList)list);
        }

        /// <summary>
        /// Creates a IxxxBrowsingPlaceholderNodeIndex object.
        /// </summary>
        protected override IReadOnlyBrowsingPlaceholderNodeIndex CreateChildNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState));
            return new WriteableBrowsingPlaceholderNodeIndex(node, childNode, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        protected override IReadOnlyBrowsingOptionalNodeIndex CreateOptionalNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState));
            return new WriteableBrowsingOptionalNodeIndex(node, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        protected override IReadOnlyBrowsingListNodeIndex CreateListNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, INode childNode, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState));
            return new WriteableBrowsingListNodeIndex(node, childNode, propertyName, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        protected override IReadOnlyBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, INodeTreeBlock childBlock, int blockIndex, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState));
            return new WriteableBrowsingNewBlockNodeIndex(node, childNode, propertyName, blockIndex, childBlock.ReplicationPattern, childBlock.SourceIdentifier);
        }

        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        protected override IReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, int blockIndex, int index, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState));
            return new WriteableBrowsingExistingBlockNodeIndex(node, childNode, propertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingPlaceholderNodeIndex.
        /// </summary>
        protected override IReadOnlyIndexCollection CreatePlaceholderIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingPlaceholderNodeIndex childNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState));
            return new WriteableIndexCollection<IWriteableBrowsingPlaceholderNodeIndex>(propertyName, new List<IWriteableBrowsingPlaceholderNodeIndex>() { (IWriteableBrowsingPlaceholderNodeIndex)childNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingOptionalNodeIndex.
        /// </summary>
        protected override IReadOnlyIndexCollection CreateOptionalIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingOptionalNodeIndex optionalNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState));
            return new WriteableIndexCollection<IWriteableBrowsingOptionalNodeIndex>(propertyName, new List<IWriteableBrowsingOptionalNodeIndex>() { (IWriteableBrowsingOptionalNodeIndex)optionalNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndexList object.
        /// </summary>
        protected override IReadOnlyBrowsingListNodeIndexList CreateBrowsingListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState));
            return new WriteableBrowsingListNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingListNodeIndex objects.
        /// </summary>
        protected override IReadOnlyIndexCollection CreateListIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingListNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState));
            return new WriteableIndexCollection<IWriteableBrowsingListNodeIndex>(propertyName, (IWriteableBrowsingListNodeIndexList)nodeIndexList);
        }

        /// <summary>
        /// Creates a IxxxBrowsingBlockNodeIndexList object.
        /// </summary>
        protected override IReadOnlyBrowsingBlockNodeIndexList CreateBrowsingBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState));
            return new WriteableBrowsingBlockNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingBlockNodeIndex objects.
        /// </summary>
        protected override IReadOnlyIndexCollection CreateBlockIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingBlockNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState));
            return new WriteableIndexCollection<IWriteableBrowsingBlockNodeIndex>(propertyName, (IWriteableBrowsingBlockNodeIndexList)nodeIndexList);
        }
        #endregion
    }
}
