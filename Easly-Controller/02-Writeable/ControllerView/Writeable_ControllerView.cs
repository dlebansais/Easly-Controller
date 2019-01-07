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
            Controller.StateInserted += OnStateInserted;
            Controller.StateRemoved += OnStateRemoved;
            Controller.StateReplaced += OnStateReplaced;
            Controller.StateAssigned += OnStateAssigned;
            Controller.StateUnassigned += OnStateUnassigned;
            Controller.StateMoved += OnStateMoved;
            Controller.BlockStateMoved += OnBlockStateMoved;
            Controller.BlockSplit += OnBlockSplit;
            Controller.BlocksMerged+= OnBlocksMerged;
            Controller.ArgumentExpanded += OnArgumentExpanded;
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
        /// Handler called every time a state is inserted in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void OnStateInserted(IWriteableInsertNodeOperation operation)
        {
            Debug.Assert(operation != null);

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

            IWriteableNodeState ChildState = operation.ChildState;
            Debug.Assert(ChildState != null);
            Debug.Assert(StateViewTable.ContainsKey(ChildState));
        }

        /// <summary>
        /// Handler called every time a state is assigned in the controller.
        /// </summary>
        /// <param name="nodeIndex">Index of the assigned state.</param>
        /// <param name="state">The state assigned.</param>
        public virtual void OnStateAssigned(IWriteableBrowsingOptionalNodeIndex nodeIndex, IWriteableOptionalNodeState state)
        {
            Debug.Assert(state != null);
            Debug.Assert(StateViewTable.ContainsKey(state));
        }

        /// <summary>
        /// Handler called every time a state is unassigned in the controller.
        /// </summary>
        /// <param name="nodeIndex">Index of the unassigned state.</param>
        /// <param name="state">The state unassigned.</param>
        public virtual void OnStateUnassigned(IWriteableBrowsingOptionalNodeIndex nodeIndex, IWriteableOptionalNodeState state)
        {
            Debug.Assert(state != null);
            Debug.Assert(StateViewTable.ContainsKey(state));
        }

        /// <summary>
        /// Handler called every time a state is moved in the controller.
        /// </summary>
        /// <param name="nodeIndex">Index of the moved state.</param>
        /// <param name="state">The moved state.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        public virtual void OnStateMoved(IWriteableBrowsingChildIndex nodeIndex, IWriteableNodeState state, int direction)
        {
            Debug.Assert(state != null);
            Debug.Assert(StateViewTable.ContainsKey(state));
        }

        /// <summary>
        /// Handler called every time a block state is moved in the controller.
        /// </summary>
        /// <param name="inner">Inner where the block is split.</param>
        /// <param name="blockIndex">Index of the split block.</param>
        /// <param name="direction">The change in position, relative to the current block position.</param>
        public virtual void OnBlockStateMoved(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex, int direction)
        {
        }

        /// <summary>
        /// Handler called every time a block split in the controller.
        /// </summary>
        /// <param name="inner">Inner where the block is split.</param>
        /// <param name="blockIndex">Index of the split block.</param>
        /// <param name="index">Index of the last node to stay in the old block.</param>
        public virtual void OnBlockSplit(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex, int index)
        {
        }

        /// <summary>
        /// Handler called every time two blocks are merged.
        /// </summary>
        /// <param name="inner">Inner where the blocks are merged.</param>
        /// <param name="blockIndex">Index of the first merged block.</param>
        public virtual void OnBlocksMerged(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex)
        {
        }

        /// <summary>
        /// Handler called every time an argument block is expanded.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public virtual void OnArgumentExpanded(IWriteableExpandArgumentOperation operation)
        {
            Debug.Assert(operation != null);

            IWriteableBlockState BlockState = operation.BlockState;

            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));

            Debug.Assert(StateViewTable.ContainsKey(BlockState.PatternState));
            Debug.Assert(StateViewTable.ContainsKey(BlockState.SourceState));

            Debug.Assert(BlockState.StateList.Count == 1);
            Debug.Assert(StateViewTable.ContainsKey(BlockState.StateList[0]));
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
                return false;

            if (!base.IsEqual(comparer, AsControllerView))
                return false;

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
                BlockListInnerAttachedHandler = OnBlockListInnerCreated,
                BlockStateAttachedHandler = OnBlockStateCreated,
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
