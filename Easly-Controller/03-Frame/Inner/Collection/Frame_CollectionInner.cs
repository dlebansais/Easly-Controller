namespace EaslyController.Frame
{
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public interface IFrameCollectionInner : IWriteableCollectionInner, IFrameInner
    {
    }

    /// <inheritdoc/>
    internal interface IFrameCollectionInner<out IIndex> : IWriteableCollectionInner<IIndex>, IFrameInner<IIndex>
        where IIndex : IFrameBrowsingCollectionNodeIndex
    {
    }

    /// <inheritdoc/>
    internal abstract class FrameCollectionInner<IIndex> : WriteableCollectionInner<IIndex>, IFrameCollectionInner<IIndex>, IFrameCollectionInner
        where IIndex : IFrameBrowsingCollectionNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameCollectionInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public FrameCollectionInner(IFrameNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        public new IFrameNodeState Owner { get { return (IFrameNodeState)base.Owner; } }
        #endregion
    }
}
