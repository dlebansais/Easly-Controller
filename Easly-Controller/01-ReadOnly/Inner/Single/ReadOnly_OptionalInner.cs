using BaseNode;
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

    public class ReadOnlyOptionalInner<IIndex, TIndex> : ReadOnlySingleInner<IIndex>, IReadOnlyOptionalInner<IIndex>, IReadOnlyOptionalInner
        where IIndex : IReadOnlyBrowsingOptionalNodeIndex
        where TIndex : ReadOnlyBrowsingOptionalNodeIndex, IIndex
    {
        #region Init
        public ReadOnlyOptionalInner(IReadOnlyNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }

        public override IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildIndex nodeIndex)
        {
            Debug.Assert(nodeIndex is IReadOnlyBrowsingOptionalNodeIndex);
            return InitChildState((IReadOnlyBrowsingOptionalNodeIndex)nodeIndex);
        }

        protected virtual IReadOnlyOptionalNodeState InitChildState(IReadOnlyBrowsingOptionalNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(nodeIndex.PropertyName == PropertyName);
            Debug.Assert(ChildState == null);

            IReadOnlyOptionalNodeState State = CreateNodeState(nodeIndex);
            SetChildState(State);

            return State;
        }
        #endregion

        #region Properties
        public override Type InterfaceType { get { return NodeTreeHelper.OptionalChildInterfaceType(Owner.Node, PropertyName); } }
        public bool IsAssigned { get { return NodeTreeHelper.IsChildNodeAssigned(Owner.Node, PropertyName); } }
        bool IReadOnlyOptionalInner.IsAssigned { get { return IsAssigned; } }
        #endregion

        #region Client Interface
        public override void CloneChildren(INode parentNode)
        {
            Debug.Assert(parentNode != null);

            INode ChildNodeClone = ChildState.CloneNode();
            Debug.Assert(ChildNodeClone != null);

            NodeHelper.InitializeOptionalChildNode(parentNode, PropertyName, ChildNodeClone);
            if (IsAssigned)
                NodeTreeHelper.AssignChildNode(parentNode, PropertyName);
        }
        #endregion

        #region Implementation
        protected virtual void SetChildState(IReadOnlyOptionalNodeState childState)
        {
            Debug.Assert(ChildState == null);

            _ChildState = childState;
        }

        public override IReadOnlyNodeState ChildState { get { return _ChildState; } }
        private IReadOnlyOptionalNodeState _ChildState;
        #endregion

        #region Create Methods
        protected override IIndex CreateNodeIndex(IReadOnlyNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyOptionalInner<IIndex, TIndex>));
            return (TIndex)new ReadOnlyBrowsingOptionalNodeIndex(Owner.Node, PropertyName);
        }

        protected virtual IReadOnlyOptionalNodeState CreateNodeState(IReadOnlyBrowsingOptionalNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyOptionalInner<IIndex, TIndex>));
            return new ReadOnlyOptionalNodeState(nodeIndex);
        }
        #endregion
    }
}
