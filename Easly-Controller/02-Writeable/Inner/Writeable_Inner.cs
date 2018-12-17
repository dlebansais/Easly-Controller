using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Interface for all inners.
    /// </summary>
    public interface IWriteableInner : IReadOnlyInner
    {
    }

    /// <summary>
    /// Interface for all inners.
    /// </summary>
    public interface IWriteableInner<out IIndex> : IReadOnlyInner<IIndex>
        where IIndex : IWriteableBrowsingChildIndex
    {
    }

    /// <summary>
    /// Interface for all inners.
    /// </summary>
    public abstract class WriteableInner<IIndex> : ReadOnlyInner<IIndex>, IWriteableInner<IIndex>, IWriteableInner
        where IIndex : IWriteableBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public WriteableInner(IWriteableNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion
    }
}
