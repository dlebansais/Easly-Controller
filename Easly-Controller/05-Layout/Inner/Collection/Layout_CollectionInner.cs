namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <inheritdoc/>
    public interface ILayoutCollectionInner : IFocusCollectionInner, ILayoutInner
    {
    }

    /// <inheritdoc/>
    internal interface ILayoutCollectionInner<out IIndex> : IFocusCollectionInner<IIndex>, ILayoutInner<IIndex>
        where IIndex : ILayoutBrowsingCollectionNodeIndex
    {
    }

    /// <inheritdoc/>
    internal abstract class LayoutCollectionInner<IIndex> : FocusCollectionInner<IIndex>, ILayoutCollectionInner<IIndex>, ILayoutCollectionInner
        where IIndex : ILayoutBrowsingCollectionNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutCollectionInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public LayoutCollectionInner(ILayoutNodeState owner, string propertyName)
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
