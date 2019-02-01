namespace EaslyController.Focus
{
    using EaslyController.Frame;

    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    public interface IFocusSingleInner : IFrameSingleInner, IFocusInner
    {
        /// <summary>
        /// State of the node.
        /// </summary>
        new IFocusNodeState ChildState { get; }
    }

    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal interface IFocusSingleInner<out IIndex> : IFrameSingleInner<IIndex>, IFocusInner<IIndex>
        where IIndex : IFocusBrowsingChildIndex
    {
        /// <summary>
        /// State of the node.
        /// </summary>
        new IFocusNodeState ChildState { get; }
    }

    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal abstract class FocusSingleInner<IIndex> : FrameSingleInner<IIndex>, IFocusSingleInner<IIndex>, IFocusSingleInner
        where IIndex : IFocusBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusSingleInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public FocusSingleInner(IFocusNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        public new IFocusNodeState Owner { get { return (IFocusNodeState)base.Owner; } }

        /// <summary>
        /// State of the node.
        /// </summary>
        public new IFocusNodeState ChildState { get { return (IFocusNodeState)base.ChildState; } }
        #endregion
    }
}
