namespace EaslyController.Layout
{
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <inheritdoc/>
    public interface ILayoutPlaceholderInner : IFocusPlaceholderInner, ILayoutSingleInner
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        new ILayoutPlaceholderNodeState ChildState { get; }
    }

    /// <inheritdoc/>
    internal interface ILayoutPlaceholderInner<out IIndex> : IFocusPlaceholderInner<IIndex>, ILayoutSingleInner<IIndex>
        where IIndex : ILayoutBrowsingPlaceholderNodeIndex
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        new ILayoutPlaceholderNodeState ChildState { get; }
    }

    /// <inheritdoc/>
    internal class LayoutPlaceholderInner<IIndex> : FocusPlaceholderInner<IIndex>, ILayoutPlaceholderInner<IIndex>, ILayoutPlaceholderInner
        where IIndex : ILayoutBrowsingPlaceholderNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutPlaceholderInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public LayoutPlaceholderInner(ILayoutNodeState owner, string propertyName)
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
        /// The state of the optional node.
        /// </summary>
        public new ILayoutPlaceholderNodeState ChildState { get { return (ILayoutPlaceholderNodeState)base.ChildState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyBrowsingPlaceholderNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutPlaceholderInner<IIndex>>());
            return new LayoutPlaceholderNodeState<ILayoutInner<ILayoutBrowsingChildIndex>>((ILayoutBrowsingPlaceholderNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingPlaceholderNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingPlaceholderNodeIndex CreateBrowsingNodeIndex(Node node)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutPlaceholderInner<IIndex>>());
            return new LayoutBrowsingPlaceholderNodeIndex(Owner.Node, node, PropertyName);
        }
        #endregion
    }
}
