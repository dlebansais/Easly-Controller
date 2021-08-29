namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    public interface IReadOnlySingleInner : IReadOnlyInner
    {
    }

    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal interface IReadOnlySingleInner<out IIndex> : IReadOnlyInner<IIndex>
        where IIndex : IReadOnlyBrowsingChildIndex
    {
    }

    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal abstract class ReadOnlySingleInner<IIndex> : ReadOnlyInner<IIndex>, IReadOnlySingleInner<IIndex>, IReadOnlySingleInner, IReadOnlyInner
        where IIndex : IReadOnlyBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlySingleInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public ReadOnlySingleInner(IReadOnlyNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion
    }
}
