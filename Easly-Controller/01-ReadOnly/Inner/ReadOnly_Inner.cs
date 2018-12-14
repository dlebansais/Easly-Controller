using BaseNode;
using System;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyInner
    {
        IReadOnlyNodeState Owner { get; }
        string PropertyName { get; }
        Type InterfaceType { get; }
        void CloneChildren(INode parentNode);
    }

    public interface IReadOnlyInner<out IIndex>
        where IIndex : IReadOnlyBrowsingChildIndex
    {
        IReadOnlyNodeState Owner { get; }
        string PropertyName { get; }
        Type InterfaceType { get; }
        void CloneChildren(INode parentNode);
        IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildIndex nodeIndex);
    }

    public abstract class ReadOnlyInner<IIndex> : IReadOnlyInner<IIndex>, IReadOnlyInner
        where IIndex : IReadOnlyBrowsingChildIndex
    {
        #region Init
        public ReadOnlyInner(IReadOnlyNodeState owner, string propertyName)
        {
            Debug.Assert(owner != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));

            Owner = owner;
            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        public IReadOnlyNodeState Owner { get; }
        public string PropertyName { get; }
        public abstract Type InterfaceType { get; }
        #endregion

        #region Client Interface
        public abstract IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildIndex nodeIndex);
        public abstract void CloneChildren(INode parentNode);
        #endregion
    }
}
