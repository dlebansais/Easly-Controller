namespace EaslyController.Frame
{
    using EaslyController.Writeable;

    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    public interface IFrameSingleInner : IWriteableSingleInner, IFrameInner
    {
        /// <summary>
        /// State of the node.
        /// </summary>
        new IFrameNodeState ChildState { get; }
    }

    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    public interface IFrameSingleInner<out IIndex> : IWriteableSingleInner<IIndex>, IFrameInner<IIndex>
        where IIndex : IFrameBrowsingChildIndex
    {
        /// <summary>
        /// State of the node.
        /// </summary>
        new IFrameNodeState ChildState { get; }
    }

    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal abstract class FrameSingleInner<IIndex> : WriteableSingleInner<IIndex>, IFrameSingleInner<IIndex>, IFrameSingleInner
        where IIndex : IFrameBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameSingleInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public FrameSingleInner(IFrameNodeState owner, string propertyName)
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
        /// State of the node.
        /// </summary>
        public new IFrameNodeState ChildState { get { return (IFrameNodeState)base.ChildState; } }
        #endregion
    }
}
