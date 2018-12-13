namespace EaslyController.ReadOnly
{
    public interface IReadOnlyCollectionInner : IReadOnlyInner
    {
        int Count { get; }
        IReadOnlyPlaceholderNodeState FirstNodeState { get; }
    }

    public interface IReadOnlyCollectionInner<out IIndex> : IReadOnlyInner<IIndex>
        where IIndex : IReadOnlyBrowsingCollectionNodeIndex
    {
        int Count { get; }
        IReadOnlyPlaceholderNodeState FirstNodeState { get; }
    }

    public abstract class ReadOnlyCollectionInner<IIndex, TIndex> : ReadOnlyInner<IIndex>, IReadOnlyCollectionInner<IIndex>, IReadOnlyCollectionInner
        where IIndex : IReadOnlyBrowsingCollectionNodeIndex
        where TIndex : ReadOnlyBrowsingCollectionNodeIndex, IIndex
    {
        #region Init
        public ReadOnlyCollectionInner(IReadOnlyNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        public abstract int Count { get; }
        public abstract IReadOnlyPlaceholderNodeState FirstNodeState { get; }
        #endregion
    }
}
