using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    public interface IWriteableCollectionInner : IReadOnlyCollectionInner, IWriteableInner
    {
    }

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    public interface IWriteableCollectionInner<out IIndex> : IReadOnlyCollectionInner<IIndex>, IWriteableInner<IIndex>
        where IIndex : IWriteableBrowsingCollectionNodeIndex
    {
    }

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    public abstract class WriteableCollectionInner<IIndex, TIndex> : ReadOnlyCollectionInner<IIndex, TIndex>, IWriteableCollectionInner<IIndex>, IWriteableCollectionInner
        where IIndex : IWriteableBrowsingCollectionNodeIndex
        where TIndex : WriteableBrowsingCollectionNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableCollectionInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public WriteableCollectionInner(IWriteableNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion
    }
}
