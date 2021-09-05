namespace EaslyController.Focus
{
    using System.Diagnostics;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public interface IFocusOptionalInner : IFrameOptionalInner, IFocusSingleInner
    {
        /// <summary>
        /// The state of the optional node.
        /// </summary>
        new IFocusOptionalNodeState ChildState { get; }
    }

    /// <inheritdoc/>
    internal interface IFocusOptionalInner<out IIndex> : IFrameOptionalInner<IIndex>, IFocusSingleInner<IIndex>
        where IIndex : IFocusBrowsingOptionalNodeIndex
    {
        /// <summary>
        /// The state of the optional node.
        /// </summary>
        new IFocusOptionalNodeState ChildState { get; }
    }

    /// <inheritdoc/>
    internal class FocusOptionalInner<IIndex> : FrameOptionalInner<IIndex>, IFocusOptionalInner<IIndex>, IFocusOptionalInner
        where IIndex : IFocusBrowsingOptionalNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusOptionalInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public FocusOptionalInner(IFocusNodeState owner, string propertyName)
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
        public new IFocusOptionalNodeState ChildState { get { return (IFocusOptionalNodeState)base.ChildState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxOptionalNodeState object.
        /// </summary>
        private protected override IReadOnlyOptionalNodeState CreateNodeState(IReadOnlyBrowsingOptionalNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusOptionalInner<IIndex>));
            return new FocusOptionalNodeState<IFocusInner<IFocusBrowsingChildIndex>>((IFocusBrowsingOptionalNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingOptionalNodeIndex CreateBrowsingNodeIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusOptionalInner<IIndex>));
            return new FocusBrowsingOptionalNodeIndex(Owner.Node, PropertyName);
        }
        #endregion
    }
}
