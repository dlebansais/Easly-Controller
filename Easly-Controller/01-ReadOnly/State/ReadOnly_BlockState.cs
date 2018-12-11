using BaseNode;
using BaseNodeHelper;
using System;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBlockState : IReadOnlyState
    {
        IReadOnlyBlockListInner ParentInner { get; }
        IBlock ChildBlock { get; }
        IReadOnlyPatternState PatternState { get; }
        IReadOnlySourceState SourceState { get; }
        IReadOnlyNodeStateReadOnlyList StateList { get; }
        void Init(Func<IReadOnlyBlockState, IReadOnlyPatternState> createPatternState, Func<IReadOnlyBlockState, IReadOnlySourceState> createSourceState, bool isEnumerating);
        void SetState(IReadOnlyNodeState state, bool isEnumerating);
    }

    public class ReadOnlyBlockState : ReadOnlyState, IReadOnlyBlockState
    {
        #region Init
        public ReadOnlyBlockState(IReadOnlyBlockListInner parentInner, IBlock childBlock, Func<IReadOnlyBlockState, IReadOnlyPatternState> createPatternState, Func<IReadOnlyBlockState, IReadOnlySourceState> createSourceState)
        {
            ParentInner = parentInner;
            ChildBlock = childBlock;
            InitStateList();
        }

        public void Init(Func<IReadOnlyBlockState, IReadOnlyPatternState> createPatternState, Func<IReadOnlyBlockState, IReadOnlySourceState> createSourceState, bool isEnumerating)
        {
            PatternState = createPatternState(this);
            SourceState = createSourceState(this);
            PatternInner = CreatePatternInner(ParentInner.Owner, PatternState);
            SourceInner = CreateSourceInner(ParentInner.Owner, SourceState);
        }
        #endregion

        #region Properties
        public IReadOnlyBlockListInner ParentInner { get; protected set; }
        public IBlock ChildBlock { get; private set; }
        public IReadOnlyPatternState PatternState { get; protected set; }
        public IReadOnlySourceState SourceState { get; protected set; }
        #endregion

        #region Client Interface
        public override IReadOnlyInner PropertyToInner(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IBlock.ReplicationPattern):
                    return PatternInner;

                case nameof(IBlock.SourceIdentifier):
                    return SourceInner;

                default:
                    throw new ArgumentOutOfRangeException(nameof(propertyName));
            }
        }

        private IReadOnlyPlaceholderInner PatternInner;
        private IReadOnlyPlaceholderInner SourceInner;
        #endregion

        #region StateList
        protected virtual void InitStateList()
        {
            _StateList = CreateStateList();
            StateList = CreateStateListReadOnly(_StateList);
        }

        public virtual void SetState(IReadOnlyNodeState state, bool isEnumerating)
        {
            InsertState(StateList.Count, state, isEnumerating);
        }

        protected virtual void InsertState(int index, IReadOnlyNodeState state, bool isEnumerating)
        {
            if (isEnumerating)
                Debug.Assert(_StateList[index] == state);
            else
                _StateList.Insert(index, state);
        }

        protected virtual void RemoveState(int index, IReadOnlyNodeState nodeState)
        {
            _StateList.RemoveAt(index);
        }

        protected virtual void MoveState(int index, int direction)
        {
            Debug.Assert(index >= 0 && index < _StateList.Count);
            Debug.Assert(index + direction >= 0 && index + direction < _StateList.Count);

            int index1 = (direction > 0) ? index : index + direction;
            int index2 = (direction > 0) ? index + direction : index;

            IReadOnlyNodeState NodeState1 = _StateList[index1];
            IReadOnlyNodeState NodeState2 = _StateList[index2];

            RemoveState(index2, NodeState2);
            RemoveState(index1, NodeState1);

            NodeTreeHelper.MoveNode(ChildBlock, index, direction);

            InsertState(index1, NodeState2, isEnumerating: false);
            InsertState(index2, NodeState1, isEnumerating: false);
        }

        public IReadOnlyNodeStateReadOnlyList StateList { get; protected set; }
        protected IReadOnlyNodeStateList _StateList { get; set; }
        #endregion

        #region Create Methods
        protected virtual IReadOnlyNodeStateList CreateStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyNodeStateList();
        }

        protected virtual IReadOnlyNodeStateReadOnlyList CreateStateListReadOnly(IReadOnlyNodeStateList stateList)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyNodeStateReadOnlyList(stateList);
        }

        protected virtual IReadOnlyPlaceholderInner CreatePatternInner(IReadOnlyNodeState owner, IReadOnlyPatternState patternState)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex, ReadOnlyBrowsingPlaceholderNodeIndex>(owner, nameof(IBlock.ReplicationPattern), patternState);
        }

        protected virtual IReadOnlyPlaceholderInner CreateSourceInner(IReadOnlyNodeState owner, IReadOnlySourceState sourceState)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex, ReadOnlyBrowsingPlaceholderNodeIndex>(owner, nameof(IBlock.SourceIdentifier), sourceState);
        }
        #endregion
    }
}
