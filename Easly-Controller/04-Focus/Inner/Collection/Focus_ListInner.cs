namespace EaslyController.Focus
{
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    public interface IFocusListInner : IFrameListInner, IFocusCollectionInner
    {
        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        new IFocusPlaceholderNodeStateReadOnlyList StateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new IFocusPlaceholderNodeState FirstNodeState { get; }
    }

    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal interface IFocusListInner<out IIndex> : IFrameListInner<IIndex>, IFocusCollectionInner<IIndex>
        where IIndex : IFocusBrowsingListNodeIndex
    {
        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        new IFocusPlaceholderNodeStateReadOnlyList StateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new IFocusPlaceholderNodeState FirstNodeState { get; }
    }

    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index as interface.</typeparam>
    /// <typeparam name="TIndex">Type of the index as class.</typeparam>
    internal class FocusListInner<IIndex, TIndex> : FrameListInner<IIndex, TIndex>, IFocusListInner<IIndex>, IFocusListInner
        where IIndex : IFocusBrowsingListNodeIndex
        where TIndex : FocusBrowsingListNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusListInner{IIndex, TIndex}"/> class.
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
        public new IFocusPlaceholderNodeStateReadOnlyList StateList { get { return (IFocusPlaceholderNodeStateReadOnlyList)base.StateList; } }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        public new IFocusPlaceholderNodeState FirstNodeState { get { return (IFocusPlaceholderNodeState)base.FirstNodeState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateList object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeStateList CreateStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusListInner<IIndex, TIndex>));
            return new FocusPlaceholderNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusListInner<IIndex, TIndex>));
            return new FocusPlaceholderNodeState<IFocusInner<IFocusBrowsingChildIndex>>((IFocusNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndexList.
        /// </summary>
        private protected override IReadOnlyBrowsingListNodeIndexList CreateListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusListInner<IIndex, TIndex>));
            return new FocusBrowsingListNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingListNodeIndex CreateBrowsingNodeIndex(INode node, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusListInner<IIndex, TIndex>));
            return new FocusBrowsingListNodeIndex(Owner.Node, node, PropertyName, index);
        }
        #endregion
    }
}
