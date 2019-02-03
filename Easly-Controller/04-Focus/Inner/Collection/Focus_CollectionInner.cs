namespace EaslyController.Focus
{
    using EaslyController.Frame;

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    public interface IFocusCollectionInner : IFrameCollectionInner, IFocusInner
    {
    }

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal interface IFocusCollectionInner<out IIndex> : IFrameCollectionInner<IIndex>, IFocusInner<IIndex>
        where IIndex : IFocusBrowsingCollectionNodeIndex
    {
    }

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index as interface.</typeparam>
    /// <typeparam name="TIndex">Type of the index as class.</typeparam>
    internal abstract class FocusCollectionInner<IIndex, TIndex> : FrameCollectionInner<IIndex, TIndex>, IFocusCollectionInner<IIndex>, IFocusCollectionInner
        where IIndex : IFocusBrowsingCollectionNodeIndex
        where TIndex : FocusBrowsingCollectionNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusCollectionInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public FocusCollectionInner(IFocusNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        public new IFocusNodeState Owner { get { return (IFocusNodeState)base.Owner; } }
        #endregion
    }
}
