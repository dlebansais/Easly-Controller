namespace EaslyController.Focus
{
    using EaslyController.Frame;

    /// <inheritdoc/>
    public interface IFocusCollectionInner : IFrameCollectionInner, IFocusInner
    {
    }

    /// <inheritdoc/>
    internal interface IFocusCollectionInner<out IIndex> : IFrameCollectionInner<IIndex>, IFocusInner<IIndex>
        where IIndex : IFocusBrowsingCollectionNodeIndex
    {
    }

    /// <inheritdoc/>
    internal abstract class FocusCollectionInner<IIndex> : FrameCollectionInner<IIndex>, IFocusCollectionInner<IIndex>, IFocusCollectionInner
        where IIndex : IFocusBrowsingCollectionNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusCollectionInner{IIndex}"/> class.
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
