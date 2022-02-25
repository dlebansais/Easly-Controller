namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using BaseNode;
    using EaslyController.ReadOnly;
    using NotNullReflection;

    /// <summary>
    /// State of an child node.
    /// </summary>
    public interface IWriteableEmptyNodeState : IReadOnlyEmptyNodeState, IWriteableNodeState
    {
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IWriteableNodeIndex ParentIndex { get; }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        new IWriteableInner ParentInner { get; }
    }

    /// <summary>
    /// State of an child node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal interface IWriteableEmptyNodeState<out IInner> : IReadOnlyEmptyNodeState<IInner>, IWriteableNodeState<IInner>
        where IInner : IWriteableInner<IWriteableBrowsingChildIndex>
    {
    }

    /// <summary>
    /// State of an child node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal class WriteableEmptyNodeState<IInner> : ReadOnlyEmptyNodeState<IInner>, IWriteableEmptyNodeState<IInner>, IWriteableEmptyNodeState
        where IInner : IWriteableInner<IWriteableBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableEmptyNodeState{IInner}"/> class.
        /// </summary>
        public WriteableEmptyNodeState()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new IWriteableNodeIndex ParentIndex { get { return (IWriteableNodeIndex)base.ParentIndex; } }
        IWriteableIndex IWriteableNodeState.ParentIndex { get { return ParentIndex; } }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        public new IWriteableInner ParentInner { get { return (IWriteableInner)base.ParentInner; } }

        /// <summary>
        /// State of the parent.
        /// </summary>
        public new IWriteableNodeState ParentState { get { return (IWriteableNodeState)base.ParentState; } }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        public new WriteableInnerReadOnlyDictionary<string> InnerTable { get { return (WriteableInnerReadOnlyDictionary<string>)base.InnerTable; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeStateList object.
        /// </summary>
        private protected override ReadOnlyNodeStateList CreateNodeStateList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableEmptyNodeState<IInner>>());
            return new WriteableNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxBrowsingEmptyNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingPlaceholderNodeIndex CreateChildNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, Node childNode)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableEmptyNodeState<IInner>>());
            return new WriteableBrowsingPlaceholderNodeIndex(node, childNode, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingOptionalNodeIndex CreateOptionalNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableEmptyNodeState<IInner>>());
            return new WriteableBrowsingOptionalNodeIndex(node, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingListNodeIndex CreateListNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, Node childNode, int index)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableEmptyNodeState<IInner>>());
            return new WriteableBrowsingListNodeIndex(node, childNode, propertyName, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, int blockIndex, Node childNode)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableEmptyNodeState<IInner>>());
            return new WriteableBrowsingNewBlockNodeIndex(node, childNode, propertyName, blockIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, int blockIndex, int index, Node childNode)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableEmptyNodeState<IInner>>());
            return new WriteableBrowsingExistingBlockNodeIndex(node, childNode, propertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingEmptyNodeIndex.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreatePlaceholderIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingPlaceholderNodeIndex childNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableEmptyNodeState<IInner>>());
            return new WriteableIndexCollection<IWriteableBrowsingPlaceholderNodeIndex>(propertyName, new List<IWriteableBrowsingPlaceholderNodeIndex>() { (IWriteableBrowsingPlaceholderNodeIndex)childNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingOptionalNodeIndex.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateOptionalIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingOptionalNodeIndex optionalNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableEmptyNodeState<IInner>>());
            return new WriteableIndexCollection<IWriteableBrowsingOptionalNodeIndex>(propertyName, new List<IWriteableBrowsingOptionalNodeIndex>() { (IWriteableBrowsingOptionalNodeIndex)optionalNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndexList object.
        /// </summary>
        private protected override ReadOnlyBrowsingListNodeIndexList CreateBrowsingListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableEmptyNodeState<IInner>>());
            return new WriteableBrowsingListNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingListNodeIndex objects.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateListIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, ReadOnlyBrowsingListNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableEmptyNodeState<IInner>>());
            return new WriteableIndexCollection<IWriteableBrowsingListNodeIndex>(propertyName, (WriteableBrowsingListNodeIndexList)nodeIndexList);
        }

        /// <summary>
        /// Creates a IxxxBrowsingBlockNodeIndexList object.
        /// </summary>
        private protected override ReadOnlyBrowsingBlockNodeIndexList CreateBrowsingBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableEmptyNodeState<IInner>>());
            return new WriteableBrowsingBlockNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingBlockNodeIndex objects.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateBlockIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, ReadOnlyBrowsingBlockNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableEmptyNodeState<IInner>>());
            return new WriteableIndexCollection<IWriteableBrowsingBlockNodeIndex>(propertyName, (WriteableBrowsingBlockNodeIndexList)nodeIndexList);
        }
        #endregion
    }
}
