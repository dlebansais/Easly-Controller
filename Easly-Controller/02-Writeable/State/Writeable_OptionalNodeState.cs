﻿namespace EaslyController.Writeable
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
        new WriteableBrowsingOptionalNodeIndex ParentIndex { get; }

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
        public WriteableOptionalNodeState(WriteableBrowsingOptionalNodeIndex parentIndex)
            : base(parentIndex)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new WriteableBrowsingOptionalNodeIndex ParentIndex { get { return (WriteableBrowsingOptionalNodeIndex)base.ParentIndex; } }
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
        public new WriteableInnerReadOnlyDictionary<string> InnerTable { get { return (WriteableInnerReadOnlyDictionary<string>)base.InnerTable; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeStateList object.
        /// </summary>
        private protected override ReadOnlyNodeStateList CreateNodeStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxBrowsingPlaceholderNodeIndex object.
        /// </summary>
        private protected override ReadOnlyBrowsingPlaceholderNodeIndex CreateChildNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, Node childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableBrowsingPlaceholderNodeIndex(node, childNode, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        private protected override ReadOnlyBrowsingOptionalNodeIndex CreateOptionalNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableBrowsingOptionalNodeIndex(node, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        private protected override ReadOnlyBrowsingListNodeIndex CreateListNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, Node childNode, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableBrowsingListNodeIndex(node, childNode, propertyName, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        private protected override ReadOnlyBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, int blockIndex, Node childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableBrowsingNewBlockNodeIndex(node, childNode, propertyName, blockIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected override ReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockNodeIndex(ReadOnlyBrowseContext browseNodeContext, Node node, string propertyName, int blockIndex, int index, Node childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableBrowsingExistingBlockNodeIndex(node, childNode, propertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingPlaceholderNodeIndex.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreatePlaceholderIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, ReadOnlyBrowsingPlaceholderNodeIndex childNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableIndexCollection<WriteableBrowsingPlaceholderNodeIndex>(propertyName, new List<WriteableBrowsingPlaceholderNodeIndex>() { (WriteableBrowsingPlaceholderNodeIndex)childNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingOptionalNodeIndex.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateOptionalIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, ReadOnlyBrowsingOptionalNodeIndex optionalNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableIndexCollection<WriteableBrowsingOptionalNodeIndex>(propertyName, new List<WriteableBrowsingOptionalNodeIndex>() { (WriteableBrowsingOptionalNodeIndex)optionalNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndexList object.
        /// </summary>
        private protected override ReadOnlyBrowsingListNodeIndexList CreateBrowsingListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableBrowsingListNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingListNodeIndex objects.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateListIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, ReadOnlyBrowsingListNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableIndexCollection<IWriteableBrowsingListNodeIndex>(propertyName, (WriteableBrowsingListNodeIndexList)nodeIndexList);
        }

        /// <summary>
        /// Creates a IxxxBrowsingBlockNodeIndexList object.
        /// </summary>
        private protected override ReadOnlyBrowsingBlockNodeIndexList CreateBrowsingBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableBrowsingBlockNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingBlockNodeIndex objects.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateBlockIndexCollection(ReadOnlyBrowseContext browseNodeContext, string propertyName, ReadOnlyBrowsingBlockNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableOptionalNodeState<IInner>));
            return new WriteableIndexCollection<IWriteableBrowsingBlockNodeIndex>(propertyName, (WriteableBrowsingBlockNodeIndexList)nodeIndexList);
        }
        #endregion
    }
}
