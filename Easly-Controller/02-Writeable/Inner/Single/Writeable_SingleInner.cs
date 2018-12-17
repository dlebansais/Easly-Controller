using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    public interface IWriteableSingleInner : IReadOnlySingleInner, IWriteableInner
    {
    }

    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    public interface IWriteableSingleInner<out IIndex> : IReadOnlySingleInner<IIndex>, IWriteableInner<IIndex>
        where IIndex : IWriteableBrowsingChildIndex
    {
    }

    /// <summary>
    /// Base inner for a single node inner.
    /// </summary>
    public abstract class WriteableSingleInner<IIndex> : ReadOnlySingleInner<IIndex>, IWriteableSingleInner<IIndex>, IWriteableSingleInner
        where IIndex : IWriteableBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableSingleInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public WriteableSingleInner(IWriteableNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion
    }
}
