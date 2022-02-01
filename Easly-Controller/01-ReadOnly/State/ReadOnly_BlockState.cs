namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.Constants;

    /// <summary>
    /// State of a block in a block list.
    /// </summary>
    public interface IReadOnlyBlockState : IEqualComparable
    {
        /// <summary>
        /// The parent inner containing this state.
        /// </summary>
        IReadOnlyBlockListInner ParentInner { get; }

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
        ReadOnlyPlaceholderNodeStateReadOnlyList StateList { get; }

        /// <summary>
        /// The comment associated to this block state. Null if none.
        /// </summary>
        string Comment { get; }

        /// <summary>
        /// Gets the inner corresponding to a property.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        IReadOnlyInner<IReadOnlyBrowsingChildIndex> PropertyToInner(string propertyName);
    }

    /// <summary>
    /// State of a block in a block list.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the block state.</typeparam>
    internal interface IReadOnlyBlockState<out IInner> : IEqualComparable
        where IInner : IReadOnlyInner<IReadOnlyBrowsingChildIndex>
    {
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
        /// Attach a view to the block state.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to call when enumerating existing states.</param>
        void Attach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet);

        /// <summary>
        /// Detach a view from the block state.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to no longer call when enumerating existing states.</param>
        void Detach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet);

        /// <summary>
        /// Creates a clone of the block and assigns it in the provided parent.
        /// </summary>
        /// <param name="parentNode">The node that will contains a reference to the cloned block upon return.</param>
        /// <param name="blockIndex">Position where to insert the block in <paramref name="parentNode"/>.</param>
        void CloneBlock(Node parentNode, int blockIndex);
    }

    /// <inheritdoc/>
    internal class ReadOnlyBlockState<IInner> : IReadOnlyBlockState<IInner>, IReadOnlyBlockState, IEqualComparable
        where IInner : IReadOnlyInner<IReadOnlyBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="ReadOnlyBlockState{IInner}"/> object.
        /// </summary>
        public static ReadOnlyBlockState<IInner> Empty { get; } = new ReadOnlyBlockState<IInner>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBlockState{IInner}"/> class.
        /// </summary>
        private ReadOnlyBlockState()
            : this(ReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex>.Empty, ReadOnlyBrowsingNewBlockNodeIndex.Empty, (IBlock)Block<Node>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBlockState{IInner}"/> class.
        /// </summary>
        /// <param name="parentInner">Inner containing the block state.</param>
        /// <param name="newBlockIndex">Index that was used to create the block state.</param>
        /// <param name="childBlock">The block.</param>
        public ReadOnlyBlockState(IReadOnlyBlockListInner parentInner, IReadOnlyBrowsingNewBlockNodeIndex newBlockIndex, IBlock childBlock)
        {
            Debug.Assert(parentInner != null);
            Debug.Assert(newBlockIndex != null);
            Debug.Assert(childBlock != null);

            ParentInner = parentInner;
            ChildBlock = childBlock;

            _StateList = CreateStateList();
            StateList = _StateList.ToReadOnly();
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
            ReadOnlyInnerReadOnlyDictionary<string> PatternEmptyInnerTable = CreateInnerTable().ToReadOnly();
            Dictionary<string, ValuePropertyType> PatternValuePropertyTypeTable = new Dictionary<string, ValuePropertyType>();
            PatternValuePropertyTypeTable.Add(nameof(Pattern.Text), ValuePropertyType.String);
            ((IReadOnlyPatternState<IInner>)PatternState).Init(PatternInner, PatternEmptyInnerTable, PatternValuePropertyTypeTable);
            Debug.Assert(PatternState.ToString() != null); // For code coverage.

            SourceIndex = CreateExistingSourceIndex();
            SourceState = CreateSourceState(SourceIndex);
            ReadOnlyInnerReadOnlyDictionary<string> SourceEmptyInnerTable = CreateInnerTable().ToReadOnly();
            Dictionary<string, ValuePropertyType> SourceValuePropertyTypeTable = new Dictionary<string, ValuePropertyType>();
            SourceValuePropertyTypeTable.Add(nameof(Identifier.Text), ValuePropertyType.String);
            ((IReadOnlySourceState<IInner>)SourceState).Init(SourceInner, SourceEmptyInnerTable, SourceValuePropertyTypeTable);
            Debug.Assert(SourceState.ToString() != null); // For code coverage.
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
        public ReadOnlyPlaceholderNodeStateReadOnlyList StateList { get; }
        private ReadOnlyPlaceholderNodeStateList _StateList;

        /// <summary>
        /// The comment associated to this state. Null if none.
        /// </summary>
        public string Comment { get { return ChildBlock.Documentation.Comment; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Gets the inner corresponding to a property in a node.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        public virtual IReadOnlyInner<IReadOnlyBrowsingChildIndex> PropertyToInner(string propertyName)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));

            IReadOnlyInner<IReadOnlyBrowsingChildIndex> Result = null;

            switch (propertyName)
            {
                case nameof(IBlock.ReplicationPattern):
                    Result = PatternInner;
                    break;

                case nameof(IBlock.SourceIdentifier):
                    Result = SourceInner;
                    break;
            }

            Debug.Assert(Result != null);
            return Result;
        }

        /// <summary>
        /// Creates a clone of the block and assigns it in the provided parent.
        /// </summary>
        /// <param name="parentNode">The node that will contains a reference to the cloned block upon return.</param>
        /// <param name="blockIndex">Position where to insert the block in <paramref name="parentNode"/>.</param>
        public virtual void CloneBlock(Node parentNode, int blockIndex)
        {
            Debug.Assert(parentNode != null);

            Pattern PatternClone = ClonePattern();
            Debug.Assert(PatternClone != null);

            Identifier SourceClone = CloneSource();
            Debug.Assert(SourceClone != null);

            IBlock NewBlock = NodeTreeHelperBlockList.CreateBlock(parentNode, ParentInner.PropertyName, ChildBlock.Replication, PatternClone, SourceClone);
            NodeTreeHelperBlockList.InsertIntoBlockList(parentNode, ParentInner.PropertyName, blockIndex, NewBlock);
            NodeTreeHelper.CopyDocumentation(ChildBlock, NewBlock, cloneCommentGuid: true);

            // Clone children recursively.
            CloneChildren(parentNode, NewBlock);
        }

        private protected virtual Pattern ClonePattern()
        {
            return PatternState.CloneNode();
        }

        private protected virtual Identifier CloneSource()
        {
            return SourceState.CloneNode();
        }

        private protected virtual void CloneChildren(Node parentNode, IBlock parentBlock)
        {
            for (int i = 0; i < StateList.Count; i++)
            {
                IReadOnlyPlaceholderNodeState ChildState = StateList[i];

                Node ChildNodeClone = ChildState.CloneNode();
                Debug.Assert(ChildNodeClone != null);

                NodeTreeHelperBlockList.InsertIntoBlock(parentBlock, i, ChildNodeClone);
            }
        }

        /// <summary>
        /// Attach a view to the block state.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to call when enumerating existing states.</param>
        public virtual void Attach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet)
        {
            callbackSet.OnBlockStateAttached(this);

            ((IReadOnlyPatternState<IInner>)PatternState).Attach(view, callbackSet);
            ((IReadOnlySourceState<IInner>)SourceState).Attach(view, callbackSet);

            foreach (IReadOnlyNodeState ChildState in StateList)
                ((IReadOnlyNodeState<IInner>)ChildState).Attach(view, callbackSet);
        }

        /// <summary>
        /// Detach a view from the block state.
        /// </summary>
        /// <param name="view">The attaching view.</param>
        /// <param name="callbackSet">The set of callbacks to no longer call when enumerating existing states.</param>
        public virtual void Detach(ReadOnlyControllerView view, ReadOnlyAttachCallbackSet callbackSet)
        {
            foreach (IReadOnlyNodeState ChildState in StateList)
                ((IReadOnlyNodeState<IInner>)ChildState).Detach(view, callbackSet);

            ((IReadOnlySourceState<IInner>)SourceState).Detach(view, callbackSet);
            ((IReadOnlyPatternState<IInner>)PatternState).Detach(view, callbackSet);

            callbackSet.OnBlockStateDetached(this);
        }

        private IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> PatternInner;
        private IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> SourceInner;
        #endregion

        #region Implementation
        private protected virtual void InsertState(int index, IReadOnlyPlaceholderNodeState state)
        {
            Debug.Assert(index >= 0 && index <= _StateList.Count);
            Debug.Assert(state != null);
            Debug.Assert(state.ParentIndex is IReadOnlyBrowsingExistingBlockNodeIndex);

            _StateList.Insert(index, state);
        }

        private protected virtual void RemoveState(int index)
        {
            Debug.Assert(index >= 0 && index < _StateList.Count);

            _StateList.RemoveAt(index);
        }

        private protected virtual void MoveState(int index, int direction)
        {
            Debug.Assert(index >= 0 && index < _StateList.Count);
            Debug.Assert(index + direction >= 0 && index + direction < _StateList.Count);

            IReadOnlyPlaceholderNodeState NodeState = _StateList[index];
            _StateList.RemoveAt(index);
            _StateList.Insert(index + direction, NodeState);
        }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyBlockState<IInner> AsBlockState))
                return comparer.Failed();

            if (!comparer.VerifyEqual(ParentInner, AsBlockState.ParentInner))
                return comparer.Failed();

            if (!comparer.IsSameReference(ChildBlock, AsBlockState.ChildBlock))
                return comparer.Failed();

            if (!comparer.VerifyEqual((IEqualComparable)PatternIndex, (IEqualComparable)AsBlockState.PatternIndex))
                return comparer.Failed();

            if (!comparer.VerifyEqual(PatternState, AsBlockState.PatternState))
                return comparer.Failed();

            if (!comparer.VerifyEqual((IEqualComparable)SourceIndex, (IEqualComparable)AsBlockState.SourceIndex))
                return comparer.Failed();

            if (!comparer.VerifyEqual(SourceState, AsBlockState.SourceState))
                return comparer.Failed();

            if (!comparer.VerifyEqual(StateList, AsBlockState.StateList))
                return comparer.Failed();

            return true;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"Block, {StateList.Count} state(s) of {ParentInner.InterfaceType.Name}";
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateList object.
        /// </summary>
        private protected virtual ReadOnlyPlaceholderNodeStateList CreateStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState<IInner>));
            return new ReadOnlyPlaceholderNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxInnerDictionary{string} object.
        /// </summary>
        private protected virtual ReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState<IInner>));
            return new ReadOnlyInnerDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        private protected virtual IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreatePatternInner(IReadOnlyNodeState owner)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState<IInner>));
            return new ReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex>(owner, nameof(IBlock.ReplicationPattern));
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        private protected virtual IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreateSourceInner(IReadOnlyNodeState owner)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState<IInner>));
            return new ReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex>(owner, nameof(IBlock.SourceIdentifier));
        }

        /// <summary>
        /// Creates a IxxxBrowsingPatternIndex object.
        /// </summary>
        private protected virtual IReadOnlyBrowsingPatternIndex CreateExistingPatternIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState<IInner>));
            return new ReadOnlyBrowsingPatternIndex(ChildBlock);
        }

        /// <summary>
        /// Creates a IxxxBrowsingSourceIndex object.
        /// </summary>
        private protected virtual IReadOnlyBrowsingSourceIndex CreateExistingSourceIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState<IInner>));
            return new ReadOnlyBrowsingSourceIndex(ChildBlock);
        }

        /// <summary>
        /// Creates a IxxxPatternState object.
        /// </summary>
        private protected virtual IReadOnlyPatternState CreatePatternState(IReadOnlyBrowsingPatternIndex patternIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState<IInner>));
            return new ReadOnlyPatternState<IInner>(this, patternIndex);
        }

        /// <summary>
        /// Creates a IxxxSourceState object.
        /// </summary>
        private protected virtual IReadOnlySourceState CreateSourceState(IReadOnlyBrowsingSourceIndex sourceIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBlockState<IInner>));
            return new ReadOnlySourceState<IInner>(this, sourceIndex);
        }
        #endregion
    }
}
