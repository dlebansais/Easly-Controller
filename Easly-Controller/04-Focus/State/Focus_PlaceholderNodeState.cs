namespace EaslyController.Focus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using Easly;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;

    /// <summary>
    /// State of an child node.
    /// </summary>
    public interface IFocusPlaceholderNodeState : IFramePlaceholderNodeState, IFocusNodeState, IFocusCyclableNodeState
    {
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IFocusNodeIndex ParentIndex { get; }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        new IFocusInner ParentInner { get; }
    }

    /// <summary>
    /// State of an child node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal interface IFocusPlaceholderNodeState<out IInner> : IFramePlaceholderNodeState<IInner>, IFocusNodeState<IInner>
        where IInner : IFocusInner<IFocusBrowsingChildIndex>
    {
    }

    /// <summary>
    /// State of an child node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal class FocusPlaceholderNodeState<IInner> : FramePlaceholderNodeState<IInner>, IFocusPlaceholderNodeState<IInner>, IFocusPlaceholderNodeState
        where IInner : IFocusInner<IFocusBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusPlaceholderNodeState{IInner}"/> class.
        /// </summary>
        /// <param name="parentIndex">The index used to create the state.</param>
        public FocusPlaceholderNodeState(IFocusNodeIndex parentIndex)
            : base(parentIndex)
        {
            CycleIndexList = null;
            CycleCurrentPosition = -1;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new IFocusNodeIndex ParentIndex { get { return (IFocusNodeIndex)base.ParentIndex; } }
        IFocusIndex IFocusNodeState.ParentIndex { get { return ParentIndex; } }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        public new IFocusInner ParentInner { get { return (IFocusInner)base.ParentInner; } }

        /// <summary>
        /// State of the parent.
        /// </summary>
        public new IFocusNodeState ParentState { get { return (IFocusNodeState)base.ParentState; } }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        public new IFocusInnerReadOnlyDictionary<string> InnerTable { get { return (IFocusInnerReadOnlyDictionary<string>)base.InnerTable; } }

        /// <summary>
        /// List of node indexes that can replace the current node. Can be null.
        /// Applies only to bodies and features.
        /// </summary>
        public IFocusInsertionChildNodeIndexList CycleIndexList { get; private set; }

        /// <summary>
        /// Position of the current node in <see cref="CycleIndexList"/>.
        /// </summary>
        public int CycleCurrentPosition { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Initializes the cycle index list if not already initialized.
        /// </summary>
        public virtual void InitializeCycleIndexList()
        {
            // If it's the first time we're cycling through this node, initialize it.
            if (CycleIndexList == null)
            {
                IFocusInsertionChildNodeIndex ThisIndex = ((IFocusBrowsingInsertableIndex)ParentIndex).ToInsertionIndex(ParentState.Node, Node) as IFocusInsertionChildNodeIndex;
                Debug.Assert(ThisIndex != null);

                CycleIndexList = CreateInsertionChildIndexList();
                CycleIndexList.Add(ThisIndex);
            }
        }

        /// <summary>
        /// Updates the position of the node in the cycle.
        /// </summary>
        public virtual void UpdateCyclePosition()
        {
            for (int i = 0; i < CycleIndexList.Count; i++)
                if (CycleIndexList[i].Node == Node)
                {
                    CycleCurrentPosition = i;
                    break;
                }

            Debug.Assert(CycleCurrentPosition >= 0 && CycleCurrentPosition < CycleIndexList.Count);
        }

        /// <summary>
        /// Restores the cycle index list from which this state was created.
        /// </summary>
        /// <param name="cycleIndexList">The list to restore.</param>
        public virtual void RestoreCycleIndexList(IFocusInsertionChildNodeIndexList cycleIndexList)
        {
            Debug.Assert(cycleIndexList != null && cycleIndexList.Count >= 2);
            Debug.Assert(CycleIndexList == null);

            CycleIndexList = cycleIndexList;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeStateList object.
        /// </summary>
        private protected override IReadOnlyNodeStateList CreateNodeStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPlaceholderNodeState<IInner>));
            return new FocusNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxBrowsingPlaceholderNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingPlaceholderNodeIndex CreateChildNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPlaceholderNodeState<IInner>));
            return new FocusBrowsingPlaceholderNodeIndex(node, childNode, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingOptionalNodeIndex CreateOptionalNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPlaceholderNodeState<IInner>));
            return new FocusBrowsingOptionalNodeIndex(node, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingListNodeIndex CreateListNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, INode childNode, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPlaceholderNodeState<IInner>));
            return new FocusBrowsingListNodeIndex(node, childNode, propertyName, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, int blockIndex, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPlaceholderNodeState<IInner>));
            return new FocusBrowsingNewBlockNodeIndex(node, childNode, propertyName, blockIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, int blockIndex, int index, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPlaceholderNodeState<IInner>));
            return new FocusBrowsingExistingBlockNodeIndex(node, childNode, propertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingPlaceholderNodeIndex.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreatePlaceholderIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingPlaceholderNodeIndex childNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPlaceholderNodeState<IInner>));
            return new FocusIndexCollection<IFocusBrowsingPlaceholderNodeIndex>(propertyName, new List<IFocusBrowsingPlaceholderNodeIndex>() { (IFocusBrowsingPlaceholderNodeIndex)childNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingOptionalNodeIndex.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateOptionalIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingOptionalNodeIndex optionalNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPlaceholderNodeState<IInner>));
            return new FocusIndexCollection<IFocusBrowsingOptionalNodeIndex>(propertyName, new List<IFocusBrowsingOptionalNodeIndex>() { (IFocusBrowsingOptionalNodeIndex)optionalNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndexList object.
        /// </summary>
        private protected override IReadOnlyBrowsingListNodeIndexList CreateBrowsingListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPlaceholderNodeState<IInner>));
            return new FocusBrowsingListNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingListNodeIndex objects.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateListIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingListNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPlaceholderNodeState<IInner>));
            return new FocusIndexCollection<IFocusBrowsingListNodeIndex>(propertyName, (IFocusBrowsingListNodeIndexList)nodeIndexList);
        }

        /// <summary>
        /// Creates a IxxxBrowsingBlockNodeIndexList object.
        /// </summary>
        private protected override IReadOnlyBrowsingBlockNodeIndexList CreateBrowsingBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPlaceholderNodeState<IInner>));
            return new FocusBrowsingBlockNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingBlockNodeIndex objects.
        /// </summary>
        private protected override IReadOnlyIndexCollection CreateBlockIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingBlockNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPlaceholderNodeState<IInner>));
            return new FocusIndexCollection<IFocusBrowsingBlockNodeIndex>(propertyName, (IFocusBrowsingBlockNodeIndexList)nodeIndexList);
        }

        /// <summary>
        /// Creates a IxxxInsertionChildNodeIndexList object.
        /// </summary>
        private protected virtual IFocusInsertionChildNodeIndexList CreateInsertionChildIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPlaceholderNodeState<IInner>));
            return new FocusInsertionChildNodeIndexList();
        }
        #endregion
    }
}
