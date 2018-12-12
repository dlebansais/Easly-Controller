namespace EaslyController.ReadOnly
{
    public interface IReadOnlyCollectionInner : IReadOnlyInner
    {
        int Count { get; }
        IReadOnlyNodeState FirstNodeState { get; }
    }

    public interface IReadOnlyCollectionInner<out IIndex> : IReadOnlyInner<IIndex>
        where IIndex : IReadOnlyBrowsingChildNodeIndex
    {
        int Count { get; }
        IReadOnlyNodeState FirstNodeState { get; }
    }

    public abstract class ReadOnlyCollectionInner<IIndex, TIndex> : ReadOnlyInner<IIndex, TIndex>, IReadOnlyCollectionInner<IIndex>, IReadOnlyCollectionInner
        where IIndex : IReadOnlyBrowsingChildNodeIndex
        where TIndex : ReadOnlyBrowsingChildNodeIndex, IIndex
    {
        #region Init
        public ReadOnlyCollectionInner(IReadOnlyNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        public abstract int Count { get; }
        int IReadOnlyCollectionInner.Count { get { return Count; } }
        public abstract IReadOnlyNodeState FirstNodeState { get; }
        IReadOnlyNodeState IReadOnlyCollectionInner.FirstNodeState { get { return FirstNodeState; } }
        #endregion
    }
}
