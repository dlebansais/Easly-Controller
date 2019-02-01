namespace EaslyController.Frame
{
    using EaslyController.Writeable;

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    public interface IFrameCollectionInner : IWriteableCollectionInner, IFrameInner
    {
        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new IFramePlaceholderNodeState FirstNodeState { get; }
    }

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal interface IFrameCollectionInner<out IIndex> : IWriteableCollectionInner<IIndex>, IFrameInner<IIndex>
        where IIndex : IFrameBrowsingCollectionNodeIndex
    {
        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new IFramePlaceholderNodeState FirstNodeState { get; }
    }

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index as interface.</typeparam>
    /// <typeparam name="TIndex">Type of the index as class.</typeparam>
    internal abstract class FrameCollectionInner<IIndex, TIndex> : WriteableCollectionInner<IIndex, TIndex>, IFrameCollectionInner<IIndex>, IFrameCollectionInner
        where IIndex : IFrameBrowsingCollectionNodeIndex
        where TIndex : FrameBrowsingCollectionNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameCollectionInner{IIndex, TIndex}"/> class.
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

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        public new IFramePlaceholderNodeState FirstNodeState { get { return (IFramePlaceholderNodeState)base.FirstNodeState; } }
        #endregion
    }
}
