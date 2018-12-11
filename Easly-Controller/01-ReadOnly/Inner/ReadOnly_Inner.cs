﻿using System;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyInner
    {
        IReadOnlyNodeState Owner { get; }
        string PropertyName { get; }
        Type ItemType { get; }
        IReadOnlyBrowsingChildNodeIndex IndexOf(IReadOnlyNodeState childState);
    }

    public interface IReadOnlyInner<out IIndex>
        where IIndex : IReadOnlyBrowsingChildNodeIndex
    {
        IReadOnlyNodeState Owner { get; }
        string PropertyName { get; }
        Type ItemType { get; }
        IIndex IndexOf(IReadOnlyNodeState childState);
        void Set(IReadOnlyBrowsingChildNodeIndex nodeIndex, IReadOnlyNodeState childState, bool isEnumerating);
    }

    public abstract class ReadOnlyInner<IIndex, TIndex> : IReadOnlyInner<IIndex>, IReadOnlyInner
        where IIndex : IReadOnlyBrowsingChildNodeIndex
        where TIndex : ReadOnlyBrowsingChildNodeIndex, IIndex
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
        IReadOnlyNodeState IReadOnlyInner.Owner { get { return Owner; } }
        public string PropertyName { get; private set; }
        string IReadOnlyInner.PropertyName { get { return PropertyName; } }
        public abstract Type ItemType { get; }
        Type IReadOnlyInner.ItemType { get { return ItemType; } }
        #endregion

        #region Client Interface
        public abstract IIndex IndexOf(IReadOnlyNodeState childState);
        IReadOnlyBrowsingChildNodeIndex IReadOnlyInner.IndexOf(IReadOnlyNodeState childState) { return IndexOf(childState); }
        public abstract void Set(IReadOnlyBrowsingChildNodeIndex nodeIndex, IReadOnlyNodeState childState, bool isEnumerating);
        #endregion
    }
}
