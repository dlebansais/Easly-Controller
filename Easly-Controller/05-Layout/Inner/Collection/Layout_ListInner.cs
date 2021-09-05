namespace EaslyController.Layout
{
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public interface ILayoutListInner : IFocusListInner, ILayoutCollectionInner
    {
        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        new LayoutPlaceholderNodeStateReadOnlyList StateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new ILayoutPlaceholderNodeState FirstNodeState { get; }
    }

    /// <inheritdoc/>
    internal interface ILayoutListInner<out IIndex> : IFocusListInner<IIndex>, ILayoutCollectionInner<IIndex>
        where IIndex : ILayoutBrowsingListNodeIndex
    {
        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        new LayoutPlaceholderNodeStateReadOnlyList StateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new ILayoutPlaceholderNodeState FirstNodeState { get; }
    }

    /// <inheritdoc/>
    internal class LayoutListInner<IIndex> : FocusListInner<IIndex>, ILayoutListInner<IIndex>, ILayoutListInner
        where IIndex : ILayoutBrowsingListNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutListInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public LayoutListInner(ILayoutNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        public new ILayoutNodeState Owner { get { return (ILayoutNodeState)base.Owner; } }

        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        public new LayoutPlaceholderNodeStateReadOnlyList StateList { get { return (LayoutPlaceholderNodeStateReadOnlyList)base.StateList; } }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        public new ILayoutPlaceholderNodeState FirstNodeState { get { return (ILayoutPlaceholderNodeState)base.FirstNodeState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateList object.
        /// </summary>
        private protected override ReadOnlyPlaceholderNodeStateList CreateStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutListInner<IIndex>));
            return new LayoutPlaceholderNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutListInner<IIndex>));
            return new LayoutPlaceholderNodeState<ILayoutInner<ILayoutBrowsingChildIndex>>((ILayoutNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndexList.
        /// </summary>
        private protected override ReadOnlyBrowsingListNodeIndexList CreateListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutListInner<IIndex>));
            return new LayoutBrowsingListNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingListNodeIndex CreateBrowsingNodeIndex(Node node, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutListInner<IIndex>));
            return new LayoutBrowsingListNodeIndex(Owner.Node, node, PropertyName, index);
        }
        #endregion
    }
}
