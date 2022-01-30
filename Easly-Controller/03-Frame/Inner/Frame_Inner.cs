namespace EaslyController.Frame
{
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public interface IFrameInner : IWriteableInner
    {
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        new IFrameNodeState Owner { get; }
    }

    /// <inheritdoc/>
    public interface IFrameInner<out IIndex> : IWriteableInner<IIndex>
        where IIndex : IFrameBrowsingChildIndex
    {
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        new IFrameNodeState Owner { get; }
    }

    /// <inheritdoc/>
    internal abstract class FrameInner<IIndex> : WriteableInner<IIndex>, IFrameInner<IIndex>, IFrameInner
        where IIndex : IFrameBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FrameInner{IIndex}"/> object.
        /// </summary>
        public static new IFrameInner Empty { get; } = new FrameEmptyInner<IIndex>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameInner{IIndex}"/> class.
        /// </summary>
        protected FrameInner()
            : this(FrameNodeState<IFrameInner<IFrameBrowsingChildIndex>>.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        protected FrameInner(IFrameNodeState owner)
            : base(owner)
        {
        }

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
