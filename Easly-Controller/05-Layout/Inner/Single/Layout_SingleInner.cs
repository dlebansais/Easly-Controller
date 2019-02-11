namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    public interface ILayoutSingleInner : IFocusSingleInner, ILayoutInner
    {
    }

    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal interface ILayoutSingleInner<out IIndex> : IFocusSingleInner<IIndex>, ILayoutInner<IIndex>
        where IIndex : ILayoutBrowsingChildIndex
    {
    }

    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal abstract class LayoutSingleInner<IIndex> : FocusSingleInner<IIndex>, ILayoutSingleInner<IIndex>, ILayoutSingleInner
        where IIndex : ILayoutBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutSingleInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public LayoutSingleInner(ILayoutNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        public new ILayoutNodeState Owner { get { return (ILayoutNodeState)base.Owner; } }
        #endregion
    }
}
