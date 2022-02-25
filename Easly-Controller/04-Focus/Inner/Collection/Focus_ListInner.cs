namespace EaslyController.Focus
{
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <inheritdoc/>
    public interface IFocusListInner : IFrameListInner, IFocusCollectionInner
    {
        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        new FocusPlaceholderNodeStateReadOnlyList StateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new IFocusPlaceholderNodeState FirstNodeState { get; }
    }

    /// <inheritdoc/>
    internal interface IFocusListInner<out IIndex> : IFrameListInner<IIndex>, IFocusCollectionInner<IIndex>
        where IIndex : IFocusBrowsingListNodeIndex
    {
        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        new FocusPlaceholderNodeStateReadOnlyList StateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new IFocusPlaceholderNodeState FirstNodeState { get; }
    }

    /// <inheritdoc/>
    internal class FocusListInner<IIndex> : FrameListInner<IIndex>, IFocusListInner<IIndex>, IFocusListInner
        where IIndex : IFocusBrowsingListNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusListInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public FocusListInner(IFocusNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        public new IFocusNodeState Owner { get { return (IFocusNodeState)base.Owner; } }

        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        public new FocusPlaceholderNodeStateReadOnlyList StateList { get { return (FocusPlaceholderNodeStateReadOnlyList)base.StateList; } }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        public new IFocusPlaceholderNodeState FirstNodeState { get { return (IFocusPlaceholderNodeState)base.FirstNodeState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateList object.
        /// </summary>
        private protected override ReadOnlyPlaceholderNodeStateList CreateStateList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusListInner<IIndex>>());
            return new FocusPlaceholderNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusListInner<IIndex>>());
            return new FocusPlaceholderNodeState<IFocusInner<IFocusBrowsingChildIndex>>((IFocusNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndexList.
        /// </summary>
        private protected override ReadOnlyBrowsingListNodeIndexList CreateListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusListInner<IIndex>>());
            return new FocusBrowsingListNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingListNodeIndex CreateBrowsingNodeIndex(Node node, int index)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusListInner<IIndex>>());
            return new FocusBrowsingListNodeIndex(Owner.Node, node, PropertyName, index);
        }
        #endregion
    }
}
