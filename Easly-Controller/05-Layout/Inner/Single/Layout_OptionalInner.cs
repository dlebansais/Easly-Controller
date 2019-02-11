namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    public interface ILayoutOptionalInner : IFocusOptionalInner, ILayoutSingleInner
    {
        /// <summary>
        /// The state of the optional node.
        /// </summary>
        new ILayoutOptionalNodeState ChildState { get; }
    }

    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal interface ILayoutOptionalInner<out IIndex> : IFocusOptionalInner<IIndex>, ILayoutSingleInner<IIndex>
        where IIndex : ILayoutBrowsingOptionalNodeIndex
    {
        /// <summary>
        /// The state of the optional node.
        /// </summary>
        new ILayoutOptionalNodeState ChildState { get; }
    }

    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index as interface.</typeparam>
    /// <typeparam name="TIndex">Type of the index as class.</typeparam>
    internal class LayoutOptionalInner<IIndex, TIndex> : FocusOptionalInner<IIndex, TIndex>, ILayoutOptionalInner<IIndex>, ILayoutOptionalInner
        where IIndex : ILayoutBrowsingOptionalNodeIndex
        where TIndex : LayoutBrowsingOptionalNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutOptionalInner{IIndex, TIndex}"/> class.
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
            ControllerTools.AssertNoOverride(this, typeof(LayoutOptionalInner<IIndex, TIndex>));
            return new LayoutOptionalNodeState<ILayoutInner<ILayoutBrowsingChildIndex>>((ILayoutBrowsingOptionalNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingOptionalNodeIndex CreateBrowsingNodeIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutOptionalInner<IIndex, TIndex>));
            return new LayoutBrowsingOptionalNodeIndex(Owner.Node, PropertyName);
        }
        #endregion
    }
}
