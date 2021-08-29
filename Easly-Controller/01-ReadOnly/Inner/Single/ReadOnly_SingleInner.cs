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
    /// <typeparam name="TIndex">Type of the index.</typeparam>
    internal abstract class ReadOnlySingleInner<TIndex> : ReadOnlyInner<TIndex>, IReadOnlySingleInner, IReadOnlyInner
        where TIndex : IReadOnlyBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlySingleInner{TIndex}"/> class.
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
