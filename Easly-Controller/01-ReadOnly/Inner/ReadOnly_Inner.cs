using BaseNode;
using System;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyInner
    {
        IReadOnlyNodeState Owner { get; }
        string PropertyName { get; }
        Type InterfaceType { get; }
        IReadOnlyBrowsingChildIndex IndexOf(IReadOnlyNodeState childState);
        void CloneChildren(INode parentNode);
    }

    public interface IReadOnlyInner<out IIndex>
        where IIndex : IReadOnlyBrowsingChildIndex
    {
        IReadOnlyNodeState Owner { get; }
        string PropertyName { get; }
        Type InterfaceType { get; }
        IIndex IndexOf(IReadOnlyNodeState childState);
        IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildIndex nodeIndex);
    }

    public abstract class ReadOnlyInner<IIndex> : IReadOnlyInner<IIndex>, IReadOnlyInner
        where IIndex : IReadOnlyBrowsingChildIndex
    {
        #region Init
        public ReadOnlyInner(IReadOnlyNodeState owner, string propertyName)
        {
            Owner = owner;
            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        public IReadOnlyNodeState Owner { get; private set; }
        public string PropertyName { get; private set; }
        public abstract Type InterfaceType { get; }
        #endregion

        #region Client Interface
        public abstract IIndex IndexOf(IReadOnlyNodeState childState);
        IReadOnlyBrowsingChildIndex IReadOnlyInner.IndexOf(IReadOnlyNodeState childState) { return IndexOf(childState); }
        public abstract IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildIndex nodeIndex);
        public abstract void CloneChildren(INode parentNode);
        #endregion
    }
}
