using BaseNodeHelper;
using System;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyPlaceholderInner : IReadOnlySingleInner
    {
    }

    public interface IReadOnlyPlaceholderInner<out IIndex> : IReadOnlySingleInner<IIndex>
        where IIndex : IReadOnlyBrowsingPlaceholderNodeIndex
    {
    }

    public class ReadOnlyPlaceholderInner<IIndex, TIndex> : ReadOnlySingleInner<IIndex, TIndex>, IReadOnlyPlaceholderInner<IIndex>, IReadOnlyPlaceholderInner
        where IIndex : IReadOnlyBrowsingPlaceholderNodeIndex
        where TIndex : ReadOnlyBrowsingPlaceholderNodeIndex, IIndex
    {
        #region Init
        public ReadOnlyPlaceholderInner(IReadOnlyNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
            _ChildState = null;
        }

        public ReadOnlyPlaceholderInner(IReadOnlyNodeState owner, string propertyName, IReadOnlyNodeState childState)
            : base(owner, propertyName)
        {
            _ChildState = childState;
        }
        #endregion

        #region Build Table Interface
        public override void Set(IReadOnlyBrowsingChildNodeIndex nodeIndex, IReadOnlyNodeState childState, bool isEnumerating)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(nodeIndex.PropertyName == PropertyName);
            Debug.Assert(childState != null);

            SetChildState(childState, isEnumerating);
        }
        #endregion

        #region Properties
        public override Type ItemType { get { return NodeTreeHelper.ChildInterfaceType(Owner.Node, PropertyName); } }
        public override IReadOnlyNodeState ChildState { get { return _ChildState; } }
        private IReadOnlyNodeState _ChildState;
        #endregion

        #region Ancestor Interface
        protected virtual void SetChildState(IReadOnlyNodeState childState, bool isEnumerating)
        {
            if (isEnumerating)
                Debug.Assert(_ChildState == childState);
            else
                _ChildState = childState;
        }
        #endregion

        #region Create Methods
        protected override IIndex CreateNodeIndex(IReadOnlyNodeState state, string propertyName)
        {
            return (TIndex)new ReadOnlyBrowsingPlaceholderNodeIndex(Owner.Node, state.Node, propertyName);
        }
        #endregion
    }
}
