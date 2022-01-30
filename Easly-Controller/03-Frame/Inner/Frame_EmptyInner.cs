namespace EaslyController.Frame
{
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public interface IFrameEmptyInner : IWriteableEmptyInner, IFrameSingleInner
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        new IFrameEmptyNodeState ChildState { get; }
    }

    /// <inheritdoc/>
    internal interface IFrameEmptyInner<out IIndex> : IWriteableEmptyInner<IIndex>, IFrameSingleInner<IIndex>
        where IIndex : IFrameBrowsingChildIndex
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        new IFrameEmptyNodeState ChildState { get; }
    }

    /// <inheritdoc/>
    internal class FrameEmptyInner<IIndex> : WriteableEmptyInner<IIndex>, IFrameEmptyInner<IIndex>, IFrameEmptyInner
        where IIndex : IFrameBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameEmptyInner{IIndex}"/> class.
        /// </summary>
        public FrameEmptyInner()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        public new IFrameNodeState Owner { get { return (IFrameNodeState)base.Owner; } }

        /// <summary>
        /// The state of the optional node.
        /// </summary>
        public new IFrameEmptyNodeState ChildState { get { return (IFrameEmptyNodeState)base.ChildState; } }
        #endregion
    }
}
