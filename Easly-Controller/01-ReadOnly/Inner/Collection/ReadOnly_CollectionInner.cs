namespace EaslyController.ReadOnly
{
    public interface IReadOnlyCollectionInner : IReadOnlyInner
    {
        int Count { get; }
        IReadOnlyNodeState FirstNodeState();
    }

    public interface IReadOnlyCollectionInner<out IIndex> : IReadOnlyInner<IIndex>
        where IIndex : IReadOnlyBrowsingChildNodeIndex
    {
        int Count { get; }
        IReadOnlyNodeState FirstNodeState();
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

        #region Client Interface
        public abstract int Count { get; }
        int IReadOnlyCollectionInner.Count { get { return Count; } }
        public abstract IReadOnlyNodeState FirstNodeState();
        IReadOnlyNodeState IReadOnlyCollectionInner.FirstNodeState() { return FirstNodeState(); }
        #endregion
    }
}
