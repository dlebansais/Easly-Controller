namespace EaslyController.Layout
{
    using EaslyController.Focus;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <inheritdoc/>
    public interface ILayoutOptionalInner : IFocusOptionalInner, ILayoutSingleInner
    {
        /// <summary>
        /// The state of the optional node.
        /// </summary>
        new ILayoutOptionalNodeState ChildState { get; }
    }

    /// <inheritdoc/>
    internal interface ILayoutOptionalInner<out IIndex> : IFocusOptionalInner<IIndex>, ILayoutSingleInner<IIndex>
        where IIndex : ILayoutBrowsingOptionalNodeIndex
    {
        /// <summary>
        /// The state of the optional node.
        /// </summary>
        new ILayoutOptionalNodeState ChildState { get; }
    }

    /// <inheritdoc/>
    internal class LayoutOptionalInner<IIndex> : FocusOptionalInner<IIndex>, ILayoutOptionalInner<IIndex>, ILayoutOptionalInner
        where IIndex : ILayoutBrowsingOptionalNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutOptionalInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public LayoutOptionalInner(ILayoutNodeState owner, string propertyName)
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
        public new ILayoutOptionalNodeState ChildState { get { return (ILayoutOptionalNodeState)base.ChildState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxOptionalNodeState object.
        /// </summary>
        private protected override IReadOnlyOptionalNodeState CreateNodeState(IReadOnlyBrowsingOptionalNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutOptionalInner<IIndex>>());
            return new LayoutOptionalNodeState<ILayoutInner<ILayoutBrowsingChildIndex>>((ILayoutBrowsingOptionalNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingOptionalNodeIndex CreateBrowsingNodeIndex()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutOptionalInner<IIndex>>());
            return new LayoutBrowsingOptionalNodeIndex(Owner.Node, PropertyName);
        }
        #endregion
    }
}
