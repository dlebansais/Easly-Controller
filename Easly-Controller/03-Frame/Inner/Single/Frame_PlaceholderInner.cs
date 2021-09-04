namespace EaslyController.Frame
{
    using BaseNode;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public interface IFramePlaceholderInner : IWriteablePlaceholderInner, IFrameSingleInner
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        new IFramePlaceholderNodeState ChildState { get; }
    }

    /// <inheritdoc/>
    internal interface IFramePlaceholderInner<out IIndex> : IWriteablePlaceholderInner<IIndex>, IFrameSingleInner<IIndex>
        where IIndex : IFrameBrowsingPlaceholderNodeIndex
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        new IFramePlaceholderNodeState ChildState { get; }
    }

    /// <inheritdoc/>
    internal class FramePlaceholderInner<IIndex> : WriteablePlaceholderInner<IIndex>, IFramePlaceholderInner<IIndex>, IFramePlaceholderInner
        where IIndex : IFrameBrowsingPlaceholderNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FramePlaceholderInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public FramePlaceholderInner(IFrameNodeState owner, string propertyName)
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
        /// The state of the optional node.
        /// </summary>
        public new IFramePlaceholderNodeState ChildState { get { return (IFramePlaceholderNodeState)base.ChildState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyBrowsingPlaceholderNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePlaceholderInner<IIndex>));
            return new FramePlaceholderNodeState<IFrameInner<IFrameBrowsingChildIndex>>((IFrameBrowsingPlaceholderNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingPlaceholderNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingPlaceholderNodeIndex CreateBrowsingNodeIndex(Node node)
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePlaceholderInner<IIndex>));
            return new FrameBrowsingPlaceholderNodeIndex(Owner.Node, node, PropertyName);
        }
        #endregion
    }
}
