using BaseNode;
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

    public class ReadOnlyPlaceholderInner<IIndex, TIndex> : ReadOnlySingleInner<IIndex>, IReadOnlyPlaceholderInner<IIndex>, IReadOnlyPlaceholderInner
        where IIndex : IReadOnlyBrowsingPlaceholderNodeIndex
        where TIndex : ReadOnlyBrowsingPlaceholderNodeIndex, IIndex
    {
        #region Init
        public ReadOnlyPlaceholderInner(IReadOnlyNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
            _ChildState = null;
        }

        public override IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildIndex nodeIndex)
        {
            Debug.Assert(nodeIndex is IReadOnlyBrowsingPlaceholderNodeIndex);
            return InitChildState((IReadOnlyBrowsingPlaceholderNodeIndex)nodeIndex);
        }

        protected virtual IReadOnlyPlaceholderNodeState InitChildState(IReadOnlyBrowsingPlaceholderNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(nodeIndex.PropertyName == PropertyName);
            Debug.Assert(ChildState == null);

            IReadOnlyPlaceholderNodeState State = CreateNodeState(nodeIndex);
            SetChildState(State);

            return State;
        }
        #endregion

        #region Properties
        public override Type InterfaceType { get { return NodeTreeHelper.ChildInterfaceType(Owner.Node, PropertyName); } }
        public override IReadOnlyNodeState ChildState { get { return _ChildState; } }
        private IReadOnlyPlaceholderNodeState _ChildState;
        #endregion

        #region Implementation
        protected virtual void SetChildState(IReadOnlyPlaceholderNodeState childState)
        {
            Debug.Assert(ChildState == null);

            _ChildState = childState;
        }
        #endregion

        #region Client Interface
        public override void CloneChildren(INode parentNode)
        {
            Debug.Assert(parentNode != null);

            INode ChildNodeClone = ChildState.CloneNode();
            Debug.Assert(ChildNodeClone != null);

            NodeTreeHelper.ReplaceChildNode(parentNode, PropertyName, ChildNodeClone);
        }
        #endregion

        #region Create Methods
        protected override IIndex CreateNodeIndex(IReadOnlyNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyPlaceholderInner<IIndex, TIndex>));
            return (TIndex)new ReadOnlyBrowsingPlaceholderNodeIndex(Owner.Node, state.Node, PropertyName);
        }

        protected virtual IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyBrowsingPlaceholderNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyPlaceholderInner<IIndex, TIndex>));
            return new ReadOnlyPlaceholderNodeState(nodeIndex);
        }
        #endregion
    }
}
