using EaslyController.ReadOnly;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public interface IWriteableControllerView : IReadOnlyControllerView
    {
        /// <summary>
        /// The controller.
        /// </summary>
        new IWriteableController Controller { get; }

        /// <summary>
        /// Table of views of each state in the controller.
        /// </summary>
        new IWriteableStateViewDictionary StateViewTable { get; }

        /// <summary>
        /// Table of views of each block state in the controller.
        /// </summary>
        new IWriteableBlockStateViewDictionary BlockStateViewTable { get; }
    }

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public class WriteableControllerView : ReadOnlyControllerView, IWriteableControllerView
    {
        #region Init
        /// <summary>
        /// Creates and initializes a new instance of a <see cref="WriteableControllerView"/> object.
        /// </summary>
        /// <param name="controller">The controller on which the view is attached.</param>
        public static IWriteableControllerView Create(IWriteableController controller)
        {
            WriteableControllerView View = new WriteableControllerView(controller);
            View.Init();
            return View;
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="WriteableControllerView"/> object.
        /// </summary>
        /// <param name="controller">The controller on which the view is attached.</param>
        protected WriteableControllerView(IWriteableController controller)
            : base(controller)
        {
        }

        /// <summary>
        /// Initializes the view by attaching it to the controller.
        /// </summary>
        protected override void Init()
        {
            base.Init();

            Controller.BlockStateInserted += OnBlockStateInserted;
            Controller.BlockStateRemoved += OnBlockStateRemoved;
            Controller.BlockViewRemoved += OnBlockViewRemoved;
            Controller.StateInserted += OnStateInserted;
            Controller.StateRemoved += OnStateRemoved;
            Controller.StateReplaced += OnStateReplaced;
            Controller.StateAssigned += OnStateAssigned;
            Controller.StateUnassigned += OnStateUnassigned;
            Controller.StateChanged += OnStateChanged;
            Controller.BlockStateChanged += OnBlockStateChanged;
            Controller.StateMoved += OnStateMoved;
            Controller.BlockStateMoved += OnBlockStateMoved;
            Controller.BlockSplit += OnBlockSplit;
            Controller.BlocksMerged+= OnBlocksMerged;
            Controller.ArgumentExpanded += OnArgumentExpanded;
            Controller.GenericRefresh += OnGenericRefresh;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller.
        /// </summary>
        public new IWriteableController Controller { get { return (IWriteableController)base.Controller; } }

        /// <summary>
        /// Table of views of each state in the controller.
        /// </summary>
        public new IWriteableStateViewDictionary StateViewTable { get { return (IWriteableStateViewDictionary)base.StateViewTable; } }

        /// <summary>
        /// Table of views of each block state in the controller.
        /// </summary>
        public new IWriteableBlockStateViewDictionary BlockStateViewTable { get { return (IWriteableBlockStateViewDictionary)base.BlockStateViewTable; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Handler called every time a block state is inserted in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void OnBlockStateInserted(IWriteableInsertBlockOperation operation)
        {
            Debug.Assert(operation != null);
            Debug.Assert(!operation.IsNested);

            IWriteableBlockState BlockState = operation.BlockState;

            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));

            Debug.Assert(StateViewTable.ContainsKey(BlockState.PatternState));
            Debug.Assert(StateViewTable.ContainsKey(BlockState.SourceState));

            foreach (IWriteableNodeState State in BlockState.StateList)
                Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a block state is removed from the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void OnBlockStateRemoved(IWriteableRemoveBlockOperation operation)
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
        /// Handler called every time a block view must be removed from the controller view.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void OnBlockViewRemoved(IWriteableRemoveBlockViewOperation operation)
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
        public virtual void OnStateInserted(IWriteableInsertNodeOperation operation)
        {
            Debug.Assert(operation != null);
            Debug.Assert(!operation.IsNested);

            IWriteableNodeState ChildState = operation.ChildState;
            Debug.Assert(ChildState != null);
            Debug.Assert(StateViewTable.ContainsKey(ChildState));
        }

        /// <summary>
        /// Handler called every time a state is removed from the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void OnStateRemoved(IWriteableRemoveNodeOperation operation)
        {
            Debug.Assert(operation != null);
            Debug.Assert(!operation.IsNested);

            IWriteablePlaceholderNodeState RemovedState = operation.ChildState;
            Debug.Assert(RemovedState != null);
            Debug.Assert(!StateViewTable.ContainsKey(RemovedState));
        }

        /// <summary>
        /// Handler called every time a state is replaced in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void OnStateReplaced(IWriteableReplaceOperation operation)
        {
            Debug.Assert(operation != null);
            Debug.Assert(!operation.IsNested);

            IWriteableNodeState ChildState = operation.NewChildState;
            Debug.Assert(ChildState != null);
            Debug.Assert(StateViewTable.ContainsKey(ChildState));
        }

        /// <summary>
        /// Handler called every time a state is assigned in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void OnStateAssigned(IWriteableAssignmentOperation operation)
        {
            Debug.Assert(operation != null);
            Debug.Assert(!operation.IsNested);

            IWriteableOptionalNodeState State = operation.State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a state is unassigned in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void OnStateUnassigned(IWriteableAssignmentOperation operation)
        {
            Debug.Assert(operation != null);

            IWriteableOptionalNodeState State = operation.State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a state is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void OnStateChanged(IWriteableChangeNodeOperation operation)
        {
            Debug.Assert(operation != null);

            IWriteablePlaceholderNodeState State = operation.State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a block state is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void OnBlockStateChanged(IWriteableChangeBlockOperation operation)
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
        public virtual void OnStateMoved(IWriteableMoveNodeOperation operation)
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
        public virtual void OnBlockStateMoved(IWriteableMoveBlockOperation operation)
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
        public virtual void OnBlockSplit(IWriteableSplitBlockOperation operation)
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
        public virtual void OnBlocksMerged(IWriteableMergeBlocksOperation operation)
        {
            Debug.Assert(operation != null);
            Debug.Assert(!operation.IsNested);
        }

        /// <summary>
        /// Handler called every time an argument block is expanded.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void OnArgumentExpanded(IWriteableExpandArgumentOperation operation)
        {
            Debug.Assert(operation != null);
            Debug.Assert(!operation.IsNested);

            IWriteableBlockState BlockState = operation.BlockState;

            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));

            Debug.Assert(StateViewTable.ContainsKey(BlockState.PatternState));
            Debug.Assert(StateViewTable.ContainsKey(BlockState.SourceState));

            Debug.Assert(BlockState.StateList.Count == 1);
            Debug.Assert(StateViewTable.ContainsKey(BlockState.StateList[0]));
        }

        /// <summary>
        /// Handler called to refresh views.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void OnGenericRefresh(IWriteableGenericRefreshOperation operation)
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
        /// Compares two <see cref="IWriteableControllerView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IWriteableControllerView AsControllerView))
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
        protected override IReadOnlyStateViewDictionary CreateStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableControllerView));
            return new WriteableStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxBlockStateViewDictionary object.
        /// </summary>
        protected override IReadOnlyBlockStateViewDictionary CreateBlockStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableControllerView));
            return new WriteableBlockStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxAttachCallbackSet object.
        /// </summary>
        protected override IReadOnlyAttachCallbackSet CreateCallbackSet()
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
        protected override IReadOnlyPlaceholderNodeStateView CreatePlaceholderNodeStateView(IReadOnlyPlaceholderNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableControllerView));
            return new WriteablePlaceholderNodeStateView(this, (IWriteablePlaceholderNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxOptionalNodeStateView object.
        /// </summary>
        protected override IReadOnlyOptionalNodeStateView CreateOptionalNodeStateView(IReadOnlyOptionalNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableControllerView));
            return new WriteableOptionalNodeStateView(this, (IWriteableOptionalNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxPatternStateView object.
        /// </summary>
        protected override IReadOnlyPatternStateView CreatePatternStateView(IReadOnlyPatternState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableControllerView));
            return new WriteablePatternStateView(this, (IWriteablePatternState)state);
        }

        /// <summary>
        /// Creates a IxxxSourceStateView object.
        /// </summary>
        protected override IReadOnlySourceStateView CreateSourceStateView(IReadOnlySourceState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableControllerView));
            return new WriteableSourceStateView(this, (IWriteableSourceState)state);
        }

        /// <summary>
        /// Creates a IxxxBlockStateView object.
        /// </summary>
        protected override IReadOnlyBlockStateView CreateBlockStateView(IReadOnlyBlockState blockState)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableControllerView));
            return new WriteableBlockStateView(this, (IWriteableBlockState)blockState);
        }
        #endregion
    }
}
