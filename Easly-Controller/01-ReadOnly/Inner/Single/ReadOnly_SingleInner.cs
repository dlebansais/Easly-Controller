namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    public interface IReadOnlySingleInner : IReadOnlyInner
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        IReadOnlyNodeState ChildState { get; }
    }

    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    public interface IReadOnlySingleInner<out IIndex> : IReadOnlyInner<IIndex>
        where IIndex : IReadOnlyBrowsingChildIndex
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        IReadOnlyNodeState ChildState { get; }
    }

    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    public abstract class ReadOnlySingleInner<IIndex> : ReadOnlyInner<IIndex>, IReadOnlySingleInner<IIndex>, IReadOnlySingleInner
        where IIndex : IReadOnlyBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlySingleInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public ReadOnlySingleInner(IReadOnlyNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// State of the node.
        /// </summary>
        public abstract IReadOnlyNodeState ChildState { get; }
        #endregion
    }
}
