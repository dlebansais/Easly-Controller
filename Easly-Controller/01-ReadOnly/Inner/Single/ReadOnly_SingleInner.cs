using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlySingleInner : IReadOnlyInner
    {
        IReadOnlyNodeState ChildState { get; }
    }

    public interface IReadOnlySingleInner<out IIndex> : IReadOnlyInner<IIndex>
        where IIndex : IReadOnlyBrowsingChildNodeIndex
    {
        IReadOnlyNodeState ChildState { get; }
    }

    public abstract class ReadOnlySingleInner<IIndex, TIndex> : ReadOnlyInner<IIndex, TIndex>, IReadOnlySingleInner<IIndex>, IReadOnlySingleInner
        where IIndex : IReadOnlyBrowsingChildNodeIndex
        where TIndex : ReadOnlyBrowsingChildNodeIndex, IIndex
    {
        #region Init
        public ReadOnlySingleInner(IReadOnlyNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        public abstract IReadOnlyNodeState ChildState { get; }
        IReadOnlyNodeState IReadOnlySingleInner.ChildState { get { return ChildState; } }
        #endregion

        #region Client Interface
        public override IIndex IndexOf(IReadOnlyNodeState childState)
        {
            Debug.Assert(ChildState == childState);

            return CreateNodeIndex(childState, PropertyName);
        }
        #endregion

        #region Create Methods
        protected abstract IIndex CreateNodeIndex(IReadOnlyNodeState state, string propertyName);
        #endregion
    }
}
