namespace EaslyController.Focus
{
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <inheritdoc/>
    public interface IFocusPlaceholderInner : IFramePlaceholderInner, IFocusSingleInner
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        new IFocusPlaceholderNodeState ChildState { get; }
    }

    /// <inheritdoc/>
    internal interface IFocusPlaceholderInner<out IIndex> : IFramePlaceholderInner<IIndex>, IFocusSingleInner<IIndex>
        where IIndex : IFocusBrowsingPlaceholderNodeIndex
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        new IFocusPlaceholderNodeState ChildState { get; }
    }

    /// <inheritdoc/>
    internal class FocusPlaceholderInner<IIndex> : FramePlaceholderInner<IIndex>, IFocusPlaceholderInner<IIndex>, IFocusPlaceholderInner
        where IIndex : IFocusBrowsingPlaceholderNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusPlaceholderInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public FocusPlaceholderInner(IFocusNodeState owner, string propertyName)
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
        /// The state of the optional node.
        /// </summary>
        public new IFocusPlaceholderNodeState ChildState { get { return (IFocusPlaceholderNodeState)base.ChildState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyBrowsingPlaceholderNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusPlaceholderInner<IIndex>>());
            return new FocusPlaceholderNodeState<IFocusInner<IFocusBrowsingChildIndex>>((IFocusBrowsingPlaceholderNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingPlaceholderNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingPlaceholderNodeIndex CreateBrowsingNodeIndex(Node node)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusPlaceholderInner<IIndex>>());
            return new FocusBrowsingPlaceholderNodeIndex(Owner.Node, node, PropertyName);
        }
        #endregion
    }
}
