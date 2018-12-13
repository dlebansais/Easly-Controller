using BaseNode;
using BaseNodeHelper;
using System;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyListInner : IReadOnlyCollectionInner
    {
        IReadOnlyPlaceholderNodeStateReadOnlyList StateList { get; }
    }

    public interface IReadOnlyListInner<out IIndex> : IReadOnlyCollectionInner<IIndex>
        where IIndex : IReadOnlyBrowsingListNodeIndex
    {
        IReadOnlyPlaceholderNodeStateReadOnlyList StateList { get; }
    }

    public class ReadOnlyListInner<IIndex, TIndex> : ReadOnlyCollectionInner<IIndex, TIndex>, IReadOnlyListInner<IIndex>, IReadOnlyListInner
        where IIndex : IReadOnlyBrowsingListNodeIndex
        where TIndex : ReadOnlyBrowsingListNodeIndex, IIndex
    {
        #region Init
        public ReadOnlyListInner(IReadOnlyNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
            InitStateList();
        }

        public override IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildIndex nodeIndex)
        {
            Debug.Assert(nodeIndex is IReadOnlyBrowsingListNodeIndex);
            return InitChildState((IReadOnlyBrowsingListNodeIndex)nodeIndex);
        }

        protected virtual IReadOnlyNodeState InitChildState(IReadOnlyBrowsingListNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(nodeIndex.PropertyName == PropertyName);

            int Index = ((IIndex)nodeIndex).Index;
            Debug.Assert(Index == StateList.Count);

            IReadOnlyPlaceholderNodeState State = CreateNodeState(nodeIndex);
            InsertInStateList(Index, State);

            return State;
        }
        #endregion

        #region Properties
        public override Type InterfaceType { get { return NodeTreeHelper.ListInterfaceType(Owner.Node, PropertyName); } }

        public override int Count
        {
            get { return StateList.Count; }
        }

        public override IReadOnlyPlaceholderNodeState FirstNodeState
        {
            get
            {
                Debug.Assert(Count > 0);

                return StateList[0];
            }
        }
        #endregion

        #region Client Interface
        public override IIndex IndexOf(IReadOnlyNodeState childState)
        {
            Debug.Assert(childState is IReadOnlyPlaceholderNodeState);
            return IndexOf((IReadOnlyPlaceholderNodeState)childState);
        }

        protected virtual IIndex IndexOf(IReadOnlyPlaceholderNodeState childState)
        {
            GetIndexOf(childState, out int Index);
            Debug.Assert(Index >= 0 && Index < Count);

            return CreateNodeIndex(childState, PropertyName, Index);
        }

        private void GetIndexOf(IReadOnlyPlaceholderNodeState childState, out int stateIndex)
        {
            for (int Index = 0; Index < StateList.Count; Index++)
                if (StateList[Index] == childState)
                {
                    stateIndex = Index;
                    return;
                }

            stateIndex = -1;
        }

        public override void CloneChildren(INode parentNode)
        {
            Debug.Assert(parentNode != null);

            NodeHelper.InitializeEmptyNodeList(parentNode, PropertyName, InterfaceType);

            for (int i = 0; i < StateList.Count; i++)
            {
                IReadOnlyPlaceholderNodeState ChildState = StateList[i];

                INode ChildNodeClone = ChildState.CloneNode();
                Debug.Assert(ChildNodeClone != null);

                NodeTreeHelper.InsertIntoList(parentNode, PropertyName, i, ChildNodeClone);
            }
        }
        #endregion

        #region Implementation
        protected virtual void InitStateList()
        {
            _StateList = CreateStateList();
            StateList = CreateStateListReadOnly(_StateList);
        }

        protected virtual void InsertInStateList(int index, IReadOnlyPlaceholderNodeState nodeState)
        {
            Debug.Assert(index >= 0 && index <= _StateList.Count);
            Debug.Assert(nodeState != null);

            _StateList.Insert(index, nodeState);
        }

        protected virtual void RemoveFromStateList(int index, IReadOnlyPlaceholderNodeState nodeState)
        {
            Debug.Assert(index >= 0 && index < _StateList.Count);

            _StateList.RemoveAt(index);
        }

        public IReadOnlyPlaceholderNodeStateReadOnlyList StateList { get; protected set; }
        private IReadOnlyPlaceholderNodeStateList _StateList;
        #endregion

        #region Create Methods
        protected virtual IReadOnlyPlaceholderNodeStateList CreateStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyListInner<IIndex, TIndex>));
            return new ReadOnlyPlaceholderNodeStateList();
        }

        protected virtual IReadOnlyPlaceholderNodeStateReadOnlyList CreateStateListReadOnly(IReadOnlyPlaceholderNodeStateList stateList)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyListInner<IIndex, TIndex>));
            return new ReadOnlyPlaceholderNodeStateReadOnlyList(stateList);
        }

        protected virtual IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyListInner<IIndex, TIndex>));
            return new ReadOnlyPlaceholderNodeState(nodeIndex);
        }

        protected virtual IIndex CreateNodeIndex(IReadOnlyPlaceholderNodeState state, string propertyName, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyListInner<IIndex, TIndex>));
            return (TIndex)new ReadOnlyBrowsingListNodeIndex(Owner.Node, state.Node, propertyName, index);
        }
        #endregion
    }
}
