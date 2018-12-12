using BaseNodeHelper;
using System;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyListInner : IReadOnlyCollectionInner
    {
        IReadOnlyNodeStateReadOnlyList StateList { get; }
    }

    public interface IReadOnlyListInner<out IIndex> : IReadOnlyCollectionInner<IIndex>
        where IIndex : IReadOnlyBrowsingListNodeIndex
    {
        IReadOnlyNodeStateReadOnlyList StateList { get; }
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

        public override IReadOnlyNodeState InitChildState(IReadOnlyBrowsingChildNodeIndex nodeIndex)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(nodeIndex.PropertyName == PropertyName);

            int Index = ((IIndex)nodeIndex).Index;
            Debug.Assert(Index == StateList.Count);

            IReadOnlyNodeState State = CreateNodeState(nodeIndex);
            InsertInStateList(Index, State);

            return State;
        }
        #endregion

        #region Properties
        public override Type ItemType { get { return NodeTreeHelper.ListInterfaceType(Owner.Node, PropertyName); } }

        public override int Count
        {
            get { return StateList.Count; }
        }

        public override IReadOnlyNodeState FirstNodeState
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
            GetIndexOf(childState, out int Index);
            Debug.Assert(Index >= 0 && Index < Count);

            return CreateNodeIndex(childState, PropertyName, Index);
        }

        private void GetIndexOf(IReadOnlyNodeState childState, out int stateIndex)
        {
            for (int Index = 0; Index < StateList.Count; Index++)
                if (StateList[Index] == childState)
                {
                    stateIndex = Index;
                    return;
                }

            stateIndex = -1;
        }
        #endregion

        #region StateList
        protected virtual void InitStateList()
        {
            _StateList = CreateStateList();
            StateList = CreateStateListReadOnly(_StateList);
        }

        protected virtual void InsertInStateList(int index, IReadOnlyNodeState nodeState)
        {
            Debug.Assert(index >= 0 && index <= _StateList.Count);
            Debug.Assert(nodeState != null);

            _StateList.Insert(index, nodeState);
        }

        protected virtual void RemoveFromStateList(int index, IReadOnlyNodeState nodeState)
        {
            Debug.Assert(index >= 0 && index < _StateList.Count);

            _StateList.RemoveAt(index);
        }

        public IReadOnlyNodeStateReadOnlyList StateList { get; protected set; }
        private IReadOnlyNodeStateList _StateList;
        #endregion

        #region Create Methods
        protected virtual IReadOnlyNodeStateList CreateStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyListInner<IIndex, TIndex>));
            return new ReadOnlyNodeStateList();
        }

        protected virtual IReadOnlyNodeStateReadOnlyList CreateStateListReadOnly(IReadOnlyNodeStateList stateList)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyListInner<IIndex, TIndex>));
            return new ReadOnlyNodeStateReadOnlyList(stateList);
        }

        protected virtual IReadOnlyNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyListInner<IIndex, TIndex>));
            return new ReadOnlyNodeState(nodeIndex.Node);
        }

        protected virtual IIndex CreateNodeIndex(IReadOnlyNodeState state, string propertyName, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyListInner<IIndex, TIndex>));
            return (TIndex)new ReadOnlyBrowsingListNodeIndex(Owner.Node, state.Node, propertyName, index);
        }
        #endregion
    }
}
