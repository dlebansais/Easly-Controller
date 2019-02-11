namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Interface for all inners.
    /// </summary>
    public interface ILayoutInner : IFocusInner
    {
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        new ILayoutNodeState Owner { get; }
    }

    /// <summary>
    /// Interface for all inners.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    public interface ILayoutInner<out IIndex> : IFocusInner<IIndex>
        where IIndex : ILayoutBrowsingChildIndex
    {
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        new ILayoutNodeState Owner { get; }
    }

    /// <summary>
    /// Interface for all inners.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal abstract class LayoutInner<IIndex> : FocusInner<IIndex>, ILayoutInner<IIndex>, ILayoutInner
        where IIndex : ILayoutBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public LayoutInner(ILayoutNodeState owner, string propertyName)
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
