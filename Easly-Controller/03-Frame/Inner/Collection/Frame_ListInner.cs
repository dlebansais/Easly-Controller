namespace EaslyController.Frame
{
    using BaseNode;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    public interface IFrameListInner : IWriteableListInner, IFrameCollectionInner
    {
        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        new IFramePlaceholderNodeStateReadOnlyList StateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new IFramePlaceholderNodeState FirstNodeState { get; }
    }

    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal interface IFrameListInner<out IIndex> : IWriteableListInner<IIndex>, IFrameCollectionInner<IIndex>
        where IIndex : IFrameBrowsingListNodeIndex
    {
        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        new IFramePlaceholderNodeStateReadOnlyList StateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new IFramePlaceholderNodeState FirstNodeState { get; }
    }

    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index as interface.</typeparam>
    /// <typeparam name="TIndex">Type of the index as class.</typeparam>
    internal class FrameListInner<IIndex, TIndex> : WriteableListInner<IIndex, TIndex>, IFrameListInner<IIndex>, IFrameListInner
        where IIndex : IFrameBrowsingListNodeIndex
        where TIndex : FrameBrowsingListNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameListInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public FrameListInner(IFrameNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        public new IFrameNodeState Owner { get { return (IFrameNodeState)base.Owner; } }

        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        public new IFramePlaceholderNodeStateReadOnlyList StateList { get { return (IFramePlaceholderNodeStateReadOnlyList)base.StateList; } }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        public new IFramePlaceholderNodeState FirstNodeState { get { return (IFramePlaceholderNodeState)base.FirstNodeState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateList object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeStateList CreateStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameListInner<IIndex, TIndex>));
            return new FramePlaceholderNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameListInner<IIndex, TIndex>));
            return new FramePlaceholderNodeState<IFrameInner<IFrameBrowsingChildIndex>>((IFrameNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndexList.
        /// </summary>
        private protected override IReadOnlyBrowsingListNodeIndexList CreateListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameListInner<IIndex, TIndex>));
            return new FrameBrowsingListNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingListNodeIndex CreateBrowsingNodeIndex(Node node, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameListInner<IIndex, TIndex>));
            return new FrameBrowsingListNodeIndex(Owner.Node, node, PropertyName, index);
        }
        #endregion
    }
}
