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
        #endregion

        #region Properties
        public override Type ItemType { get { return NodeTreeHelper.ListInterfaceType(Owner.Node, PropertyName); } }

        public override int Count
        {
            get { return StateList.Count; }
        }
        #endregion

        #region Build Table Interface
        public override void Set(IReadOnlyBrowsingChildNodeIndex nodeIndex, IReadOnlyNodeState childState, bool isEnumerating)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(nodeIndex.PropertyName == PropertyName);
            Debug.Assert(childState != null);

            int Index = ((IIndex)nodeIndex).Index;
            Debug.Assert(isEnumerating ? (Index < StateList.Count) : (Index == StateList.Count));

            InsertInStateList(Index, childState, isEnumerating);
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

        public override IReadOnlyNodeState FirstNodeState()
        {
            Debug.Assert(Count > 0);

            return StateList[0];
        }
        #endregion

        #region StateList
        protected virtual void InitStateList()
        {
            _StateList = CreateStateList();
            StateList = CreateStateListReadOnly(_StateList);
        }

        protected virtual void InsertInStateList(int index, IReadOnlyNodeState nodeState, bool isEnumerating)
        {
            if (isEnumerating)
                Debug.Assert(_StateList[index] == nodeState);
            else
                _StateList.Insert(index, nodeState);
        }

        protected virtual void RemoveFromStateList(int index, IReadOnlyNodeState nodeState)
        {
            _StateList.RemoveAt(index);
        }

        public IReadOnlyNodeStateReadOnlyList StateList { get; protected set; }
        protected IReadOnlyNodeStateList _StateList;
        #endregion

        #region Create Methods
        protected virtual IReadOnlyNodeStateList CreateStateList()
        {
            return new ReadOnlyNodeStateList();
        }

        protected virtual IReadOnlyNodeStateReadOnlyList CreateStateListReadOnly(IReadOnlyNodeStateList stateList)
        {
            return new ReadOnlyNodeStateReadOnlyList(stateList);
        }

        protected virtual IIndex CreateNodeIndex(IReadOnlyNodeState state, string propertyName, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyListInner<IIndex, TIndex>));
            return (TIndex)new ReadOnlyBrowsingListNodeIndex(Owner.Node, state.Node, propertyName, index);
        }
        #endregion
    }
}
