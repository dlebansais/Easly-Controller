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
        IReadOnlyBrowsingPatternIndex PatternIndex { get; }
        IReadOnlyPatternState PatternState { get; }
        IReadOnlyBrowsingSourceIndex SourceIndex { get; }
        IReadOnlySourceState SourceState { get; }
        IReadOnlyPlaceholderNodeStateReadOnlyList StateList { get; }
        void InitBlockState(IReadOnlyBrowseContext browseContext);
        void InitNodeState(IReadOnlyPlaceholderNodeState state);
        void CloneBlock(INode parentNode, int blockIndex);
    }

    public class ReadOnlyBlockState : IReadOnlyBlockState
    {
        #region Init
        public ReadOnlyBlockState(IReadOnlyBlockListInner parentInner, IBlock childBlock)
        {
            Debug.Assert(parentInner != null);
            Debug.Assert(childBlock != null);

            ParentInner = parentInner;
            ChildBlock = childBlock;

            _StateList = CreateStateList();
            StateList = CreateStateListReadOnly(_StateList);
            PatternInner = CreatePatternInner(ParentInner.Owner);
            SourceInner = CreateSourceInner(ParentInner.Owner);
        }

        public virtual void InitBlockState(IReadOnlyBrowseContext browseContext)
        {
            Debug.Assert(browseContext != null);

            PatternIndex = CreateExistingPatternIndex();
            PatternState = CreatePatternState(PatternIndex);
            IReadOnlyInnerReadOnlyDictionary<string> PatternEmptyInnerTable = CreateInnerTableReadOnly(CreateInnerTable());
            Dictionary<string, ValuePropertyType> PatternValuePropertyTypeTable = new Dictionary<string, ValuePropertyType>();
            PatternValuePropertyTypeTable.Add(nameof(IPattern.Text), ValuePropertyType.String);
            PatternState.Init(browseContext, PatternInner, PatternEmptyInnerTable, PatternValuePropertyTypeTable);

            SourceIndex = CreateExistingSourceIndex();
            SourceState = CreateSourceState(SourceIndex);
            IReadOnlyInnerReadOnlyDictionary<string> SourceEmptyInnerTable = CreateInnerTableReadOnly(CreateInnerTable());
            Dictionary<string, ValuePropertyType> SourceValuePropertyTypeTable = new Dictionary<string, ValuePropertyType>();
            SourceValuePropertyTypeTable.Add(nameof(IIdentifier.Text), ValuePropertyType.String);
            SourceState.Init(browseContext, SourceInner, SourceEmptyInnerTable, SourceValuePropertyTypeTable);
        }

        public virtual void InitNodeState(IReadOnlyPlaceholderNodeState state)
        {
            Debug.Assert(state != null);

            InsertState(StateList.Count, state);
        }
        #endregion

        #region Properties
        public IReadOnlyBlockListInner ParentInner { get; }
        public IBlock ChildBlock { get; }
        public ReplicationStatus Replication { get { return ChildBlock.Replication; } }
        public IReadOnlyBrowsingPatternIndex PatternIndex { get; private set; }
        public IReadOnlyPatternState PatternState { get; private set; }
        public IReadOnlyBrowsingSourceIndex SourceIndex { get; private set; }
        public IReadOnlySourceState SourceState { get; private set; }
        #endregion

        #region Client Interface
        public virtual IReadOnlyInner<IReadOnlyBrowsingChildIndex> PropertyToInner(string propertyName)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));

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
            Debug.Assert(parentNode != null);

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
                IReadOnlyPlaceholderNodeState ChildState = StateList[i];

                INode ChildNodeClone = ChildState.CloneNode();
                Debug.Assert(ChildNodeClone != null);

                NodeTreeHelper.InsertIntoBlock(parentBlock, i, ChildNodeClone);
            }
        }

        private IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> PatternInner;
        private IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> SourceInner;
        #endregion

        #region StateList
        protected virtual void InsertState(int index, IReadOnlyPlaceholderNodeState state)
        {
            Debug.Assert(index >= 0 && index <= _StateList.Count);
            Debug.Assert(state != null);

            _StateList.Insert(index, state);
        }

        protected virtual void RemoveState(int index, IReadOnlyPlaceholderNodeState nodeState)
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

            IReadOnlyPlaceholderNodeState NodeState1 = _StateList[index1];
            IReadOnlyPlaceholderNodeState NodeState2 = _StateList[index2];

            RemoveState(index2, NodeState2);
            RemoveState(index1, NodeState1);

            NodeTreeHelper.MoveNode(ChildBlock, index, direction);

            InsertState(index1, NodeState2);
            InsertState(index2, NodeState1);
        }

        public IReadOnlyPlaceholderNodeStateReadOnlyList StateList { get; }
        private IReadOnlyPlaceholderNodeStateList _StateList;
        #endregion

        #region Create Methods
        protected virtual IReadOnlyPlaceholderNodeStateList CreateStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyPlaceholderNodeStateList();
        }

        protected virtual IReadOnlyPlaceholderNodeStateReadOnlyList CreateStateListReadOnly(IReadOnlyPlaceholderNodeStateList stateList)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyPlaceholderNodeStateReadOnlyList(stateList);
        }

        protected virtual IReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyInnerDictionary<string>();
        }

        protected virtual IReadOnlyInnerReadOnlyDictionary<string> CreateInnerTableReadOnly(IReadOnlyInnerDictionary<string> innerTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyInnerReadOnlyDictionary<string>(innerTable);
        }

        protected virtual IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreatePatternInner(IReadOnlyNodeState owner)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex, ReadOnlyBrowsingPlaceholderNodeIndex>(owner, nameof(IBlock.ReplicationPattern));
        }

        protected virtual IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreateSourceInner(IReadOnlyNodeState owner)
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
            return new ReadOnlyPatternState(this, patternIndex);
        }

        protected virtual IReadOnlySourceState CreateSourceState(IReadOnlyBrowsingSourceIndex sourceIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlySourceState(this, sourceIndex);
        }
        #endregion
    }
}
