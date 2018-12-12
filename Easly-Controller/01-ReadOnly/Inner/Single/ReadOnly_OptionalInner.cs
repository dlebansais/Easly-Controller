using BaseNodeHelper;
using System;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyOptionalInner : IReadOnlySingleInner
    {
        bool IsAssigned { get; }
    }

    public interface IReadOnlyOptionalInner<out IIndex> : IReadOnlySingleInner<IIndex>
        where IIndex : IReadOnlyBrowsingOptionalNodeIndex
    {
        bool IsAssigned { get; }
    }

    public class ReadOnlyOptionalInner<IIndex, TIndex> : ReadOnlySingleInner<IIndex, TIndex>, IReadOnlyOptionalInner<IIndex>, IReadOnlyOptionalInner
        where IIndex : IReadOnlyBrowsingOptionalNodeIndex
        where TIndex : ReadOnlyBrowsingOptionalNodeIndex, IIndex
    {
        #region Init
        public ReadOnlyOptionalInner(IReadOnlyNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
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
        public override Type ItemType { get { return NodeTreeHelper.OptionalChildInterfaceType(Owner.Node, PropertyName); } }
        public bool IsAssigned { get { return NodeTreeHelper.IsChildNodeAssigned(Owner.Node, PropertyName); } }
        bool IReadOnlyOptionalInner.IsAssigned { get { return IsAssigned; } }
        #endregion

        #region Child State
        protected virtual void SetChildState(IReadOnlyNodeState childState)
        {
            Debug.Assert(ChildState == null);

            _ChildState = childState;
        }

        public override IReadOnlyNodeState ChildState { get { return _ChildState; } }
        private IReadOnlyNodeState _ChildState;
        #endregion

        #region Create Methods
        protected override IIndex CreateNodeIndex(IReadOnlyNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyOptionalInner<IIndex, TIndex>));
            return (TIndex)new ReadOnlyBrowsingOptionalNodeIndex(Owner.Node, state.Node, PropertyName);
        }

        protected virtual IReadOnlyNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyOptionalInner<IIndex, TIndex>));
            return new ReadOnlyNodeState(nodeIndex.Node);
        }
        #endregion
    }
}
