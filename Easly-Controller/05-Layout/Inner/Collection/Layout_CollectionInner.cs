namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    public interface ILayoutCollectionInner : IFocusCollectionInner, ILayoutInner
    {
    }

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal interface ILayoutCollectionInner<out IIndex> : IFocusCollectionInner<IIndex>, ILayoutInner<IIndex>
        where IIndex : ILayoutBrowsingCollectionNodeIndex
    {
    }

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index as interface.</typeparam>
    /// <typeparam name="TIndex">Type of the index as class.</typeparam>
    internal abstract class LayoutCollectionInner<IIndex, TIndex> : FocusCollectionInner<IIndex, TIndex>, ILayoutCollectionInner<IIndex>, ILayoutCollectionInner
        where IIndex : ILayoutBrowsingCollectionNodeIndex
        where TIndex : LayoutBrowsingCollectionNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutCollectionInner{IIndex, TIndex}"/> class.
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
