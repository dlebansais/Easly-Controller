namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using EaslyController.ReadOnly;

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public class WriteableControllerView : ReadOnlyControllerView
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="WriteableControllerView"/> object.
        /// </summary>
        public static new WriteableControllerView Empty { get; } = new WriteableControllerView();

        /// <summary>
        /// Creates and initializes a new instance of a <see cref="WriteableControllerView"/> object.
        /// </summary>
        /// <param name="controller">The controller on which the view is attached.</param>
        public static WriteableControllerView Create(WriteableController controller)
        {
            WriteableControllerView View = new WriteableControllerView(controller);
            View.Init();
            return View;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableControllerView"/> class.
        /// </summary>
        protected WriteableControllerView()
            : this(WriteableController.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableControllerView"/> class.
        /// </summary>
        /// <param name="controller">The controller on which the view is attached.</param>
        private protected WriteableControllerView(WriteableController controller)
            : base(controller)
        {
        }

        /// <summary>
        /// Add events to handlers.
        /// </summary>
        private protected override void InitAddEvents()
        {
            base.InitAddEvents();

            Controller.BlockStateInserted += OnBlockStateInserted;
            Controller.BlockStateRemoved += OnBlockStateRemoved;
            Controller.BlockViewRemoved += OnBlockViewRemoved;
            Controller.StateInserted += OnStateInserted;
            Controller.StateRemoved += OnStateRemoved;
            Controller.StateReplaced += OnStateReplaced;
            Controller.StateAssigned += OnStateAssigned;
            Controller.StateUnassigned += OnStateUnassigned;
            Controller.DiscreteValueChanged += OnDiscreteValueChanged;
            Controller.TextChanged += OnTextChanged;
            Controller.CommentChanged += OnCommentChanged;
            Controller.BlockStateChanged += OnBlockStateChanged;
            Controller.StateMoved += OnStateMoved;
            Controller.BlockStateMoved += OnBlockStateMoved;
            Controller.BlockSplit += OnBlockSplit;
            Controller.BlocksMerged += OnBlocksMerged;
            Controller.GenericRefresh += OnGenericRefresh;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller.
        /// </summary>
        public new WriteableController Controller { get { return (WriteableController)base.Controller; } }

        /// <summary>
        /// Table of views of each state in the controller.
        /// </summary>
        public new WriteableNodeStateViewDictionary StateViewTable { get { return (WriteableNodeStateViewDictionary)base.StateViewTable; } }

        /// <summary>
        /// Table of views of each block state in the controller.
        /// </summary>
        public new WriteableBlockStateViewDictionary BlockStateViewTable { get { return (WriteableBlockStateViewDictionary)base.BlockStateViewTable; } }

        /// <summary>
        /// State view of the root state.
        /// </summary>
        public new IWriteableNodeStateView RootStateView { get { return (IWriteableNodeStateView)base.RootStateView; } }
        #endregion

        #region Implementation
        /// <summary>
        /// Handler called every time a block state is inserted in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected virtual void OnBlockStateInserted(IWriteableInsertBlockOperation operation)
        {
            Debug.Assert(operation != null);

            IWriteableBlockState BlockState = operation.BlockState;

            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));

            Debug.Assert(StateViewTable.ContainsKey(BlockState.PatternState));
            Debug.Assert(StateViewTable.ContainsKey(BlockState.SourceState));

            Debug.Assert(BlockState.StateList.Count == 1);

            IWriteablePlaceholderNodeState ChildState = operation.ChildState;
            Debug.Assert(ChildState == BlockState.StateList[0]);
            Debug.Assert(ChildState.ParentIndex == operation.BrowsingIndex);
            Debug.Assert(StateViewTable.ContainsKey(ChildState));
        }

        /// <summary>
        /// Handler called every time a block state is removed from the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected virtual void OnBlockStateRemoved(IWriteableRemoveBlockOperation operation)
        {
            Debug.Assert(operation != null);

            IWriteableBlockState BlockState = operation.BlockState;

            Debug.Assert(BlockState != null);
            Debug.Assert(!BlockStateViewTable.ContainsKey(BlockState));

            Debug.Assert(!StateViewTable.ContainsKey(BlockState.PatternState));
            Debug.Assert(!StateViewTable.ContainsKey(BlockState.SourceState));

            IWriteableNodeState RemovedState = operation.RemovedState;
            Debug.Assert(!StateViewTable.ContainsKey(RemovedState));

            Debug.Assert(BlockState.StateList.Count == 0);
        }

        /// <summary>
        /// Handler called every time a block view must be removed from the controller view.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected virtual void OnBlockViewRemoved(WriteableRemoveBlockViewOperation operation)
        {
            Debug.Assert(operation != null);

            IWriteableBlockState BlockState = operation.BlockState;

            Debug.Assert(BlockState != null);
            Debug.Assert(!BlockStateViewTable.ContainsKey(BlockState));

            Debug.Assert(!StateViewTable.ContainsKey(BlockState.PatternState));
            Debug.Assert(!StateViewTable.ContainsKey(BlockState.SourceState));

            foreach (IWriteableNodeState State in BlockState.StateList)
                Debug.Assert(!StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a state is inserted in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected virtual void OnStateInserted(WriteableInsertNodeOperation operation)
        {
            Debug.Assert(operation != null);

            IWriteableNodeState ChildState = operation.ChildState;
            Debug.Assert(ChildState != null);
            Debug.Assert(StateViewTable.ContainsKey(ChildState));

            IWriteableBrowsingCollectionNodeIndex BrowsingIndex = operation.BrowsingIndex;
            Debug.Assert(ChildState.ParentIndex == BrowsingIndex);
        }

        /// <summary>
        /// Handler called every time a state is removed from the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected virtual void OnStateRemoved(WriteableRemoveNodeOperation operation)
        {
            Debug.Assert(operation != null);

            IWriteablePlaceholderNodeState RemovedState = operation.RemovedState;
            Debug.Assert(RemovedState != null);
            Debug.Assert(!StateViewTable.ContainsKey(RemovedState));
        }

        /// <summary>
        /// Handler called every time a state is replaced in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected virtual void OnStateReplaced(IWriteableReplaceOperation operation)
        {
            Debug.Assert(operation != null);

            IWriteableNodeState NewChildState = operation.NewChildState;
            Debug.Assert(NewChildState != null);
            Debug.Assert(StateViewTable.ContainsKey(NewChildState));

            IWriteableBrowsingChildIndex OldBrowsingIndex = operation.OldBrowsingIndex;
            Debug.Assert(OldBrowsingIndex != null);
            Debug.Assert(NewChildState.ParentIndex != OldBrowsingIndex);

            IWriteableBrowsingChildIndex NewBrowsingIndex = operation.NewBrowsingIndex;
            Debug.Assert(NewBrowsingIndex != null);
            Debug.Assert(NewChildState.ParentIndex == NewBrowsingIndex);
        }

        /// <summary>
        /// Handler called every time a state is assigned in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected virtual void OnStateAssigned(WriteableAssignmentOperation operation)
        {
            Debug.Assert(operation != null);

            IWriteableOptionalNodeState State = operation.State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a state is unassigned in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected virtual void OnStateUnassigned(WriteableAssignmentOperation operation)
        {
            Debug.Assert(operation != null);

            IWriteableOptionalNodeState State = operation.State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a discrete value is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected virtual void OnDiscreteValueChanged(WriteableChangeDiscreteValueOperation operation)
        {
            Debug.Assert(operation != null);

            IWriteableNodeState State = operation.State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a string is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected virtual void OnTextChanged(WriteableChangeTextOperation operation)
        {
            Debug.Assert(operation != null);

            IWriteableNodeState State = operation.State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a comment is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected virtual void OnCommentChanged(WriteableChangeCommentOperation operation)
        {
            Debug.Assert(operation != null);

            IWriteableNodeState State = operation.State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a block state is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected virtual void OnBlockStateChanged(WriteableChangeBlockOperation operation)
        {
            Debug.Assert(operation != null);

            IWriteableBlockState BlockState = operation.BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called every time a state is moved in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected virtual void OnStateMoved(WriteableMoveNodeOperation operation)
        {
            Debug.Assert(operation != null);
            Debug.Assert(!operation.IsNested);

            IWriteablePlaceholderNodeState State = operation.State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a block state is moved in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected virtual void OnBlockStateMoved(WriteableMoveBlockOperation operation)
        {
            Debug.Assert(operation != null);
            Debug.Assert(!operation.IsNested);

            IWriteableBlockState BlockState = operation.BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called every time a block split in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected virtual void OnBlockSplit(WriteableSplitBlockOperation operation)
        {
            Debug.Assert(operation != null);
            Debug.Assert(!operation.IsNested);

            IWriteableBlockState BlockState = operation.BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called every time two blocks are merged.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected virtual void OnBlocksMerged(WriteableMergeBlocksOperation operation)
        {
            Debug.Assert(operation != null);
            Debug.Assert(!operation.IsNested);

            IWriteableBlockState BlockState = operation.BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(!BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called to refresh views.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected virtual void OnGenericRefresh(WriteableGenericRefreshOperation operation)
        {
            Debug.Assert(operation != null);
            Debug.Assert(!operation.IsNested);

            IWriteableNodeState RefreshState = operation.RefreshState;

            Debug.Assert(RefreshState != null);
            Debug.Assert(StateViewTable.ContainsKey(RefreshState));
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="WriteableControllerView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out WriteableControllerView AsControllerView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsControllerView))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxStateViewDictionary object.
        /// </summary>
        private protected override ReadOnlyNodeStateViewDictionary CreateStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableControllerView));
            return new WriteableNodeStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxBlockStateViewDictionary object.
        /// </summary>
        private protected override ReadOnlyBlockStateViewDictionary CreateBlockStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableControllerView));
            return new WriteableBlockStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxAttachCallbackSet object.
        /// </summary>
        private protected override ReadOnlyAttachCallbackSet CreateCallbackSet()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableControllerView));
            return new WriteableAttachCallbackSet()
            {
                NodeStateAttachedHandler = OnNodeStateCreated,
                NodeStateDetachedHandler = OnNodeStateRemoved,
                BlockListInnerAttachedHandler = OnBlockListInnerCreated,
                BlockListInnerDetachedHandler = OnBlockListInnerRemoved,
                BlockStateAttachedHandler = OnBlockStateCreated,
                BlockStateDetachedHandler = OnBlockStateRemoved,
            };
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateView object.
        /// </summary>
        private protected override ReadOnlyPlaceholderNodeStateView CreatePlaceholderNodeStateView(IReadOnlyPlaceholderNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableControllerView));
            return new WriteablePlaceholderNodeStateView(this, (IWriteablePlaceholderNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxOptionalNodeStateView object.
        /// </summary>
        private protected override ReadOnlyOptionalNodeStateView CreateOptionalNodeStateView(IReadOnlyOptionalNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableControllerView));
            return new WriteableOptionalNodeStateView(this, (IWriteableOptionalNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxPatternStateView object.
        /// </summary>
        private protected override ReadOnlyPatternStateView CreatePatternStateView(IReadOnlyPatternState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableControllerView));
            return new WriteablePatternStateView(this, (IWriteablePatternState)state);
        }

        /// <summary>
        /// Creates a IxxxSourceStateView object.
        /// </summary>
        private protected override ReadOnlySourceStateView CreateSourceStateView(IReadOnlySourceState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableControllerView));
            return new WriteableSourceStateView(this, (IWriteableSourceState)state);
        }

        /// <summary>
        /// Creates a IxxxBlockStateView object.
        /// </summary>
        private protected override ReadOnlyBlockStateView CreateBlockStateView(IReadOnlyBlockState blockState)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableControllerView));
            return new WriteableBlockStateView(this, (IWriteableBlockState)blockState);
        }
        #endregion

        #region Implementation of IDisposable
        private protected override void DisposeNow()
        {
            Controller.BlockStateInserted -= OnBlockStateInserted;
            Controller.BlockStateRemoved -= OnBlockStateRemoved;
            Controller.BlockViewRemoved -= OnBlockViewRemoved;
            Controller.StateInserted -= OnStateInserted;
            Controller.StateRemoved -= OnStateRemoved;
            Controller.StateReplaced -= OnStateReplaced;
            Controller.StateAssigned -= OnStateAssigned;
            Controller.StateUnassigned -= OnStateUnassigned;
            Controller.DiscreteValueChanged -= OnDiscreteValueChanged;
            Controller.TextChanged -= OnTextChanged;
            Controller.CommentChanged -= OnCommentChanged;
            Controller.BlockStateChanged -= OnBlockStateChanged;
            Controller.StateMoved -= OnStateMoved;
            Controller.BlockStateMoved -= OnBlockStateMoved;
            Controller.BlockSplit -= OnBlockSplit;
            Controller.BlocksMerged -= OnBlocksMerged;
            Controller.GenericRefresh -= OnGenericRefresh;

            base.DisposeNow();
        }
        #endregion
    }
}
