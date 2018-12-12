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

        public override IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(nodeIndex.PropertyName == PropertyName);
            Debug.Assert(ChildState == null);

            IReadOnlyNodeState State = CreateNodeState(nodeIndex);
            SetChildState(State);

            return State;
        }
        #endregion

        #region Properties
        public override Type ItemType { get { return NodeTreeHelper.ChildInterfaceType(Owner.Node, PropertyName); } }
        public override IReadOnlyNodeState ChildState { get { return _ChildState; } }
        private IReadOnlyNodeState _ChildState;
        #endregion

        #region Ancestor Interface
        protected virtual void SetChildState(IReadOnlyNodeState childState)
        {
            Debug.Assert(ChildState == null);

            _ChildState = childState;
        }
        #endregion

        #region Create Methods
        protected override IIndex CreateNodeIndex(IReadOnlyNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyPlaceholderInner<IIndex, TIndex>));
            return (TIndex)new ReadOnlyBrowsingPlaceholderNodeIndex(Owner.Node, state.Node, PropertyName);
        }

        protected virtual IReadOnlyNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyPlaceholderInner<IIndex, TIndex>));
            return new ReadOnlyNodeState(nodeIndex.Node);
        }
        #endregion
    }
}
