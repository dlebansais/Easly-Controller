namespace EaslyController.Focus
{
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Inner for a child node.
    /// </summary>
    public interface IFocusPlaceholderInner : IFramePlaceholderInner, IFocusSingleInner
    {
    }

    /// <summary>
    /// Inner for a child node.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    public interface IFocusPlaceholderInner<out IIndex> : IFramePlaceholderInner<IIndex>, IFocusSingleInner<IIndex>
        where IIndex : IFocusBrowsingPlaceholderNodeIndex
    {
    }

    /// <summary>
    /// Inner for a child node.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index as interface.</typeparam>
    /// <typeparam name="TIndex">Type of the index as class.</typeparam>
    internal class FocusPlaceholderInner<IIndex, TIndex> : FramePlaceholderInner<IIndex, TIndex>, IFocusPlaceholderInner<IIndex>, IFocusPlaceholderInner
        where IIndex : IFocusBrowsingPlaceholderNodeIndex
        where TIndex : FocusBrowsingPlaceholderNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusPlaceholderInner{IIndex, TIndex}"/> class.
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
        public new IFocusNodeState ChildState { get { return (IFocusNodeState)base.ChildState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyBrowsingPlaceholderNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPlaceholderInner<IIndex, TIndex>));
            return new FocusPlaceholderNodeState((IFocusBrowsingPlaceholderNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingPlaceholderNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingPlaceholderNodeIndex CreateBrowsingNodeIndex(INode node)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPlaceholderInner<IIndex, TIndex>));
            return new FocusBrowsingPlaceholderNodeIndex(Owner.Node, node, PropertyName);
        }
        #endregion
    }
}
