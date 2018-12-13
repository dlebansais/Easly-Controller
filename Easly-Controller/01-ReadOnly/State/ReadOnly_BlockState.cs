using BaseNode;
using BaseNodeHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBlockState : IReadOnlyState
    {
        IReadOnlyBlockListInner ParentInner { get; }
        IBlock ChildBlock { get; }
        ReplicationStatus Replication { get; }
        IReadOnlyPatternState PatternState { get; }
        IReadOnlySourceState SourceState { get; }
        IReadOnlyNodeStateReadOnlyList StateList { get; }
        void InitBlockState(IReadOnlyBrowseNodeContext browseContext);
        void InitNodeState(IReadOnlyNodeState state);
        void CloneBlock(INode parentNode, int blockIndex);
    }

    public class ReadOnlyBlockState : ReadOnlyState, IReadOnlyBlockState
    {
        #region Init
        public ReadOnlyBlockState(IReadOnlyBlockListInner parentInner, IBlock childBlock)
        {
            ParentInner = parentInner;
            ChildBlock = childBlock;

            InitStateList();
            PatternInner = CreatePatternInner(ParentInner.Owner);
            SourceInner = CreateSourceInner(ParentInner.Owner);
        }

        public virtual void InitBlockState(IReadOnlyBrowseNodeContext browseContext)
        {
            IReadOnlyBrowsingPatternIndex PatternIndex = CreateExistingPatternIndex();
            PatternState = CreatePatternState(PatternIndex);
            IReadOnlyInnerReadOnlyDictionary<string> PatternEmptyInnerTable = CreateInnerTableReadOnly(CreateInnerTable());
            Dictionary<string, ValuePropertyType> PatternValuePropertyTypeTable = new Dictionary<string, ValuePropertyType>();
            PatternValuePropertyTypeTable.Add(nameof(IPattern.Text), ValuePropertyType.String);
            PatternState.Init(browseContext, PatternInner, PatternIndex, PatternEmptyInnerTable, PatternValuePropertyTypeTable);

            IReadOnlyBrowsingSourceIndex SourceIndex = CreateExistingSourceIndex();
            SourceState = CreateSourceState(SourceIndex);
            IReadOnlyInnerReadOnlyDictionary<string> SourceEmptyInnerTable = CreateInnerTableReadOnly(CreateInnerTable());
            Dictionary<string, ValuePropertyType> SourceValuePropertyTypeTable = new Dictionary<string, ValuePropertyType>();
            SourceValuePropertyTypeTable.Add(nameof(IIdentifier.Text), ValuePropertyType.String);
            SourceState.Init(browseContext, SourceInner, SourceIndex, SourceEmptyInnerTable, SourceValuePropertyTypeTable);
        }

        public virtual void InitNodeState(IReadOnlyNodeState state)
        {
            InsertState(StateList.Count, state);
        }
        #endregion

        #region Properties
        public IReadOnlyBlockListInner ParentInner { get; private set; }
        public IBlock ChildBlock { get; private set; }
        public ReplicationStatus Replication { get { return ChildBlock.Replication; } }
        public IReadOnlyPatternState PatternState { get; private set; }
        public IReadOnlySourceState SourceState { get; private set; }
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

        public virtual void CloneBlock(INode parentNode, int blockIndex)
        {
            IPattern PatternClone = ClonePattern();
            Debug.Assert(PatternClone != null);

            IIdentifier SourceClone = CloneSource();
            Debug.Assert(SourceClone != null);

            NodeTreeHelper.InsertIntoBlockList(parentNode, ParentInner.PropertyName, blockIndex, Replication, PatternClone, SourceClone, out IBlock NewBlock);
            NodeTreeHelper.CopyDocumentation(ChildBlock, NewBlock);

            CloneChildren(parentNode, NewBlock);
        }

        protected virtual IPattern ClonePattern()
        {
            return PatternState.CloneNode();
        }

        protected virtual IIdentifier CloneSource()
        {
            return SourceState.CloneNode();
        }

        protected virtual void CloneChildren(INode parentNode, IBlock parentBlock)
        {
            for (int i = 0; i < StateList.Count; i++)
            {
                IReadOnlyNodeState ChildState = StateList[i];

                INode ChildNodeClone = ChildState.CloneNode();
                Debug.Assert(ChildNodeClone != null);

                NodeTreeHelper.InsertIntoBlock(parentBlock, i, ChildNodeClone);
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

        protected virtual void InsertState(int index, IReadOnlyNodeState state)
        {
            Debug.Assert(index >= 0 && index <= _StateList.Count);
            Debug.Assert(state != null);

            _StateList.Insert(index, state);
        }

        protected virtual void RemoveState(int index, IReadOnlyNodeState nodeState)
        {
            Debug.Assert(index >= 0 && index < _StateList.Count);

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

            InsertState(index1, NodeState2);
            InsertState(index2, NodeState1);
        }

        public IReadOnlyNodeStateReadOnlyList StateList { get; protected set; }
        private IReadOnlyNodeStateList _StateList { get; set; }
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

        protected virtual IReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyController));
            return new ReadOnlyInnerDictionary<string>();
        }

        protected virtual IReadOnlyInnerReadOnlyDictionary<string> CreateInnerTableReadOnly(IReadOnlyInnerDictionary<string> innerTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyInnerReadOnlyDictionary<string>(innerTable);
        }

        protected virtual IReadOnlyPlaceholderInner CreatePatternInner(IReadOnlyNodeState owner)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex, ReadOnlyBrowsingPlaceholderNodeIndex>(owner, nameof(IBlock.ReplicationPattern));
        }

        protected virtual IReadOnlyPlaceholderInner CreateSourceInner(IReadOnlyNodeState owner)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex, ReadOnlyBrowsingPlaceholderNodeIndex>(owner, nameof(IBlock.SourceIdentifier));
        }

        protected virtual IReadOnlyBrowsingPatternIndex CreateExistingPatternIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyBrowsingPatternIndex(ChildBlock);
        }

        protected virtual IReadOnlyBrowsingSourceIndex CreateExistingSourceIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyBrowsingSourceIndex(ChildBlock);
        }

        protected virtual IReadOnlyPatternState CreatePatternState(IReadOnlyBrowsingPatternIndex patternIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyPatternState(this, patternIndex.Node);
        }

        protected virtual IReadOnlySourceState CreateSourceState(IReadOnlyBrowsingSourceIndex sourceIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlySourceState(this, sourceIndex.Node);
        }
        #endregion
    }
}
