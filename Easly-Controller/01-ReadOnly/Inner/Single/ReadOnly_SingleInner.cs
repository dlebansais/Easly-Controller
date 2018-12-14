namespace EaslyController.ReadOnly
{
    public interface IReadOnlySingleInner : IReadOnlyInner
    {
        IReadOnlyNodeState ChildState { get; }
    }

    public interface IReadOnlySingleInner<out IIndex> : IReadOnlyInner<IIndex>
        where IIndex : IReadOnlyBrowsingChildIndex
    {
        IReadOnlyNodeState ChildState { get; }
    }

    public abstract class ReadOnlySingleInner<IIndex> : ReadOnlyInner<IIndex>, IReadOnlySingleInner<IIndex>, IReadOnlySingleInner
        where IIndex : IReadOnlyBrowsingChildIndex
    {
        #region Init
        public ReadOnlySingleInner(IReadOnlyNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        public abstract IReadOnlyNodeState ChildState { get; }
        #endregion
    }
}
