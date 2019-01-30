namespace EaslyController.Frame
{
    using EaslyController.Writeable;

    /// <summary>
    /// Interface for all inners.
    /// </summary>
    public interface IFrameInner : IWriteableInner
    {
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        new IFrameNodeState Owner { get; }
    }

    /// <summary>
    /// Interface for all inners.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    public interface IFrameInner<out IIndex> : IWriteableInner<IIndex>
        where IIndex : IFrameBrowsingChildIndex
    {
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        new IFrameNodeState Owner { get; }
    }

    /// <summary>
    /// Interface for all inners.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal abstract class FrameInner<IIndex> : WriteableInner<IIndex>, IFrameInner<IIndex>, IFrameInner
        where IIndex : IFrameBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public FrameInner(IFrameNodeState owner, string propertyName)
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
