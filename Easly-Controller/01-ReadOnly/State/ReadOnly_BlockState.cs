using BaseNode;
using BaseNodeHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// State of a block in a block list.
    /// </summary>
    public interface IReadOnlyBlockState
    {
        /// <summary>
        /// The parent inner containing this state.
        /// </summary>
        IReadOnlyBlockListInner ParentInner { get; }

        /// <summary>
        /// Index that was used to create the block state.
        /// </summary>
        IReadOnlyBrowsingNewBlockNodeIndex ParentIndex { get; }

        /// <summary>
        /// The block.
        /// </summary>
        IBlock ChildBlock { get; }

        /// <summary>
        /// Index that was used to create the pattern state for this block.
        /// </summary>
        IReadOnlyBrowsingPatternIndex PatternIndex { get; }

        /// <summary>
        /// The pattern state for this block.
        /// </summary>
        IReadOnlyPatternState PatternState { get; }

        /// <summary>
        /// Index that was used to create the source state for this block.
        /// </summary>
        IReadOnlyBrowsingSourceIndex SourceIndex { get; }

        /// <summary>
        /// The source state for this block.
        /// </summary>
        IReadOnlySourceState SourceState { get; }

        /// <summary>
        /// States for nodes in the block.
        /// </summary>
        IReadOnlyPlaceholderNodeStateReadOnlyList StateList { get; }

        /// <summary>
        /// Initializes the block state.
        /// </summary>
        void InitBlockState();

        /// <summary>
        /// Initializes the state for a node of the block.
        /// </summary>
        /// <param name="state">The state to initialize.</param>
        void InitNodeState(IReadOnlyPlaceholderNodeState state);

        /// <summary>
        /// Gets the inner corresponding to a property.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        IReadOnlyInner<IReadOnlyBrowsingChildIndex> PropertyToInner(string propertyName);

        /// <summary>
        /// Creates a clone of the block and assigns it in the provided parent.
        /// </summary>
        /// <param name="parentNode">The node that will contains a reference to the cloned block upon return.</param>
        void CloneBlock(INode parentNode);

        /// <summary>
        /// Attach a view to the block state.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to call when enumerating existing states.</param>
        void Attach(IReadOnlyControllerView view, IReadOnlyAttachCallbackSet callbackSet);
    }

    /// <summary>
    /// State of a block in a block list.
    /// </summary>
    public class ReadOnlyBlockState : IReadOnlyBlockState
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBlockState"/> class.
        /// </summary>
        /// <param name="parentInner">Inner containing the block state.</param>
        /// <param name="parentIndex">Index that was used to create the block state.</param>
        /// <param name="childBlock">The block.</param>
        public ReadOnlyBlockState(IReadOnlyBlockListInner parentInner, IReadOnlyBrowsingNewBlockNodeIndex parentIndex, IBlock childBlock)
        {
            Debug.Assert(parentInner != null);
            Debug.Assert(parentIndex != null);
            Debug.Assert(childBlock != null);

            ParentInner = parentInner;
            ParentIndex = parentIndex;
            ChildBlock = childBlock;

            _StateList = CreateStateList();
            StateList = CreateStateListReadOnly(_StateList);
            PatternInner = CreatePatternInner(ParentInner.Owner);
            SourceInner = CreateSourceInner(ParentInner.Owner);
        }

        /// <summary>
        /// Initializes the block state.
        /// </summary>
        public virtual void InitBlockState()
        {
            PatternIndex = CreateExistingPatternIndex();
            PatternState = CreatePatternState(PatternIndex);
            IReadOnlyInnerReadOnlyDictionary<string> PatternEmptyInnerTable = CreateInnerTableReadOnly(CreateInnerTable());
            Dictionary<string, ValuePropertyType> PatternValuePropertyTypeTable = new Dictionary<string, ValuePropertyType>();
            PatternValuePropertyTypeTable.Add(nameof(IPattern.Text), ValuePropertyType.String);
            PatternState.Init(PatternInner, PatternEmptyInnerTable, PatternValuePropertyTypeTable);

            SourceIndex = CreateExistingSourceIndex();
            SourceState = CreateSourceState(SourceIndex);
            IReadOnlyInnerReadOnlyDictionary<string> SourceEmptyInnerTable = CreateInnerTableReadOnly(CreateInnerTable());
            Dictionary<string, ValuePropertyType> SourceValuePropertyTypeTable = new Dictionary<string, ValuePropertyType>();
            SourceValuePropertyTypeTable.Add(nameof(IIdentifier.Text), ValuePropertyType.String);
            SourceState.Init(SourceInner, SourceEmptyInnerTable, SourceValuePropertyTypeTable);
        }

        /// <summary>
        /// Initializes the state for a node of the block.
        /// </summary>
        /// <param name="state">The state to initialize.</param>
        public virtual void InitNodeState(IReadOnlyPlaceholderNodeState state)
        {
            Debug.Assert(state != null);

            InsertState(StateList.Count, state);
        }
        #endregion

        #region Properties
        /// <summary>
        /// The parent inner containing this state.
        /// </summary>
        public IReadOnlyBlockListInner ParentInner { get; }

        /// <summary>
        /// Index that was used to create the block state.
        /// </summary>
        public IReadOnlyBrowsingNewBlockNodeIndex ParentIndex { get; }

        /// <summary>
        /// The block.
        /// </summary>
        public IBlock ChildBlock { get; }

        /// <summary>
        /// Index that was used to create the pattern state for this block.
        /// </summary>
        public IReadOnlyBrowsingPatternIndex PatternIndex { get; private set; }

        /// <summary>
        /// The pattern state for this block.
        /// </summary>
        public IReadOnlyPatternState PatternState { get; private set; }

        /// <summary>
        /// Index that was used to create the source state for this block.
        /// </summary>
        public IReadOnlyBrowsingSourceIndex SourceIndex { get; private set; }

        /// <summary>
        /// The source state for this block.
        /// </summary>
        public IReadOnlySourceState SourceState { get; private set; }

        /// <summary>
        /// States for nodes in the block.
        /// </summary>
        public IReadOnlyPlaceholderNodeStateReadOnlyList StateList { get; }
        private IReadOnlyPlaceholderNodeStateList _StateList;
        #endregion

        #region Client Interface
        /// <summary>
        /// Gets the inner corresponding to a property in a node.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
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

        /// <summary>
        /// Creates a clone of the block and assigns it in the provided parent.
        /// </summary>
        /// <param name="parentNode">The node that will contains a reference to the cloned block upon return.</param>
        public virtual void CloneBlock(INode parentNode)
        {
            Debug.Assert(parentNode != null);

            IPattern PatternClone = ClonePattern();
            Debug.Assert(PatternClone != null);

            IIdentifier SourceClone = CloneSource();
            Debug.Assert(SourceClone != null);

            NodeTreeHelper.InsertIntoBlockList(parentNode, ParentInner.PropertyName, ParentIndex.BlockIndex, ChildBlock.Replication, PatternClone, SourceClone, out IBlock NewBlock);
            NodeTreeHelper.CopyDocumentation(ChildBlock, NewBlock);

            // Clone children recursively.
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

        /// <summary>
        /// Attach a view to the block state.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to call when enumerating existing states.</param>
        public virtual void Attach(IReadOnlyControllerView view, IReadOnlyAttachCallbackSet callbackSet)
        {
            callbackSet.OnBlockStateAttached(this);

            PatternState.Attach(view, callbackSet);
            SourceState.Attach(view, callbackSet);

            foreach (IReadOnlyNodeState ChildState in StateList)
                ChildState.Attach(view, callbackSet);
        }

        private IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> PatternInner;
        private IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> SourceInner;
        #endregion

        #region Implementation
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
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateList object.
        /// </summary>
        protected virtual IReadOnlyPlaceholderNodeStateList CreateStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyPlaceholderNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateReadOnlyList object.
        /// </summary>
        protected virtual IReadOnlyPlaceholderNodeStateReadOnlyList CreateStateListReadOnly(IReadOnlyPlaceholderNodeStateList stateList)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyPlaceholderNodeStateReadOnlyList(stateList);
        }

        /// <summary>
        /// Creates a IxxxInnerDictionary{string} object.
        /// </summary>
        protected virtual IReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyInnerDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxInnerReadOnlyDictionary{string} object.
        /// </summary>
        protected virtual IReadOnlyInnerReadOnlyDictionary<string> CreateInnerTableReadOnly(IReadOnlyInnerDictionary<string> innerTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyInnerReadOnlyDictionary<string>(innerTable);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        protected virtual IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreatePatternInner(IReadOnlyNodeState owner)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex, ReadOnlyBrowsingPlaceholderNodeIndex>(owner, nameof(IBlock.ReplicationPattern));
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        protected virtual IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreateSourceInner(IReadOnlyNodeState owner)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex, ReadOnlyBrowsingPlaceholderNodeIndex>(owner, nameof(IBlock.SourceIdentifier));
        }

        /// <summary>
        /// Creates a IxxxBrowsingPatternIndex object.
        /// </summary>
        protected virtual IReadOnlyBrowsingPatternIndex CreateExistingPatternIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyBrowsingPatternIndex(ChildBlock);
        }

        /// <summary>
        /// Creates a IxxxBrowsingSourceIndex object.
        /// </summary>
        protected virtual IReadOnlyBrowsingSourceIndex CreateExistingSourceIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyBrowsingSourceIndex(ChildBlock);
        }

        /// <summary>
        /// Creates a IxxxPatternState object.
        /// </summary>
        protected virtual IReadOnlyPatternState CreatePatternState(IReadOnlyBrowsingPatternIndex patternIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlyPatternState(this, patternIndex);
        }

        /// <summary>
        /// Creates a IxxxSourceState object.
        /// </summary>
        protected virtual IReadOnlySourceState CreateSourceState(IReadOnlyBrowsingSourceIndex sourceIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState));
            return new ReadOnlySourceState(this, sourceIndex);
        }
        #endregion
    }
}
