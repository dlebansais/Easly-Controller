namespace EaslyController.ReadOnly
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public class ReadOnlyControllerView : IEqualComparable, IDisposable
    {
        #region Init
        /// <summary>
        /// Creates and initializes a new instance of a <see cref="ReadOnlyControllerView"/> object.
        /// </summary>
        /// <param name="controller">The controller on which the view is attached.</param>
        public static ReadOnlyControllerView Create(ReadOnlyController controller)
        {
            ReadOnlyControllerView View = new ReadOnlyControllerView(controller);
            View.Init();
            return View;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyControllerView"/> class.
        /// </summary>
        /// <param name="controller">The controller on which the view is attached.</param>
        private protected ReadOnlyControllerView(ReadOnlyController controller)
        {
            Controller = controller;

            StateViewTable = CreateStateViewTable();
            BlockStateViewTable = CreateBlockStateViewTable();
        }

        /// <summary>
        /// Initializes the view by attaching it to the controller.
        /// </summary>
        private protected virtual void Init()
        {
            ReadOnlyAttachCallbackSet CallbackSet = CreateCallbackSet();
            ((IReadOnlyControllerInternal)Controller).Attach(this, CallbackSet);

            Debug.Assert(StateViewTable.Count == Controller.Stats.NodeCount);
            Debug.Assert(BlockStateViewTable.Count == Controller.Stats.BlockCount);

            InitAddEvents();
        }

        /// <summary>
        /// Add events to handlers.
        /// </summary>
        private protected virtual void InitAddEvents()
        {
            Controller.NodeStateCreated += OnNodeStateCreated;
            Controller.NodeStateInitialized += OnNodeStateInitialized;
            Controller.NodeStateRemoved += OnNodeStateRemoved;
            Controller.BlockListInnerCreated += OnBlockListInnerCreated;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller.
        /// </summary>
        public ReadOnlyController Controller { get; }

        /// <summary>
        /// Table of views of each state in the controller.
        /// </summary>
        public ReadOnlyStateViewDictionary StateViewTable { get; }

        /// <summary>
        /// Table of views of each block state in the controller.
        /// </summary>
        public ReadOnlyBlockStateViewDictionary BlockStateViewTable { get; }

        /// <summary>
        /// State view of the root state.
        /// </summary>
        public ReadOnlyNodeStateView RootStateView { get { return StateViewTable[Controller.RootState]; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Handler called every time a state is created in the controller.
        /// </summary>
        /// <param name="state">The state created.</param>
        public virtual void OnNodeStateCreated(IReadOnlyNodeState state)
        {
            Debug.Assert(state != null);
            Debug.Assert(!StateViewTable.ContainsKey(state));

            ReadOnlyNodeStateView StateView = null;

            switch (state)
            {
                case IReadOnlyPatternState AsPatternState:
                    StateView = CreatePatternStateView(AsPatternState);
                    break;

                case IReadOnlySourceState AsSourceState:
                    StateView = CreateSourceStateView(AsSourceState);
                    break;

                case IReadOnlyPlaceholderNodeState AsPlaceholderState:
                    StateView = CreatePlaceholderNodeStateView(AsPlaceholderState);
                    break;

                case IReadOnlyOptionalNodeState AsOptionalState:
                    StateView = CreateOptionalNodeStateView(AsOptionalState);
                    break;
            }

            Debug.Assert(StateView != null);

            StateViewTable.Add(state, StateView);
        }

        /// <summary>
        /// Handler called every time a state is initialized in the controller.
        /// </summary>
        /// <param name="state">The state initialized.</param>
        public virtual void OnNodeStateInitialized(IReadOnlyNodeState state)
        {
            Debug.Assert(state != null);
            Debug.Assert(StateViewTable.ContainsKey(state));

            IReadOnlyNodeStateView StateView = StateViewTable[state];
            Debug.Assert(StateView.ToString() != null); // For code coverage.
        }

        /// <summary>
        /// Handler called every time a state is removed in the controller.
        /// </summary>
        /// <param name="state">The state removed.</param>
        public virtual void OnNodeStateRemoved(IReadOnlyNodeState state)
        {
            Debug.Assert(state != null);
            Debug.Assert(StateViewTable.ContainsKey(state));

            StateViewTable.Remove(state);
        }

        /// <summary>
        /// Handler called every time a block list inner is created in the controller.
        /// </summary>
        /// <param name="inner">The block list inner created.</param>
        public virtual void OnBlockListInnerCreated(IReadOnlyBlockListInner inner)
        {
            inner.BlockStateCreated += OnBlockStateCreated;
            inner.BlockStateRemoved += OnBlockStateRemoved;
        }

        /// <summary>
        /// Handler called every time a block list inner is removed in the controller.
        /// </summary>
        /// <param name="inner">The block list inner removed.</param>
        public virtual void OnBlockListInnerRemoved(IReadOnlyBlockListInner inner)
        {
            inner.BlockStateCreated -= OnBlockStateCreated;
            inner.BlockStateRemoved -= OnBlockStateRemoved;
        }

        /// <summary>
        /// Handler called every time a block state is created in the controller.
        /// </summary>
        /// <param name="blockState">The block state created.</param>
        public virtual void OnBlockStateCreated(IReadOnlyBlockState blockState)
        {
            Debug.Assert(blockState != null);
            Debug.Assert(!BlockStateViewTable.ContainsKey(blockState));

            ReadOnlyBlockStateView BlockStateView = CreateBlockStateView(blockState);
            Debug.Assert(BlockStateView.ToString() != null); // For code coverage.
            BlockStateViewTable.Add(blockState, BlockStateView);
        }

        /// <summary>
        /// Handler called every time a block state is removed in the controller.
        /// </summary>
        /// <param name="blockState">The block state removed.</param>
        public virtual void OnBlockStateRemoved(IReadOnlyBlockState blockState)
        {
            Debug.Assert(blockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(blockState));

            BlockStateViewTable.Remove(blockState);
        }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyControllerView AsControllerView))
                return comparer.Failed();

            if (!comparer.IsSameReference(Controller, AsControllerView.Controller))
                return comparer.Failed();

            if (!comparer.VerifyEqual(StateViewTable, AsControllerView.StateViewTable))
                return comparer.Failed();

            if (!comparer.VerifyEqual(BlockStateViewTable, AsControllerView.BlockStateViewTable))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxStateViewDictionary object.
        /// </summary>
        private protected virtual ReadOnlyStateViewDictionary CreateStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyControllerView));
            return new ReadOnlyStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxBlockStateViewDictionary object.
        /// </summary>
        private protected virtual ReadOnlyBlockStateViewDictionary CreateBlockStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyControllerView));
            return new ReadOnlyBlockStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxAttachCallbackSet object.
        /// </summary>
        private protected virtual ReadOnlyAttachCallbackSet CreateCallbackSet()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyControllerView));
            return new ReadOnlyAttachCallbackSet()
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
        private protected virtual ReadOnlyPlaceholderNodeStateView CreatePlaceholderNodeStateView(IReadOnlyPlaceholderNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyControllerView));
            return new ReadOnlyPlaceholderNodeStateView(this, state);
        }

        /// <summary>
        /// Creates a IxxxOptionalNodeStateView object.
        /// </summary>
        private protected virtual ReadOnlyOptionalNodeStateView CreateOptionalNodeStateView(IReadOnlyOptionalNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyControllerView));
            return new ReadOnlyOptionalNodeStateView(this, state);
        }

        /// <summary>
        /// Creates a IxxxPatternStateView object.
        /// </summary>
        private protected virtual ReadOnlyPatternStateView CreatePatternStateView(IReadOnlyPatternState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyControllerView));
            return new ReadOnlyPatternStateView(this, state);
        }

        /// <summary>
        /// Creates a IxxxSourceStateView object.
        /// </summary>
        private protected virtual ReadOnlySourceStateView CreateSourceStateView(IReadOnlySourceState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyControllerView));
            return new ReadOnlySourceStateView(this, state);
        }

        /// <summary>
        /// Creates a IxxxBlockStateView object.
        /// </summary>
        private protected virtual ReadOnlyBlockStateView CreateBlockStateView(IReadOnlyBlockState blockState)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyControllerView));
            return new ReadOnlyBlockStateView(this, blockState);
        }
        #endregion

        #region Implementation of IDisposable
        private protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
                DisposeNow();
        }

        private protected virtual void DisposeNow()
        {
            Controller.NodeStateCreated -= OnNodeStateCreated;
            Controller.NodeStateInitialized -= OnNodeStateInitialized;
            Controller.NodeStateRemoved -= OnNodeStateRemoved;
            Controller.BlockListInnerCreated -= OnBlockListInnerCreated;

            ReadOnlyAttachCallbackSet CallbackSet = CreateCallbackSet();
            ((IReadOnlyControllerInternal)Controller).Detach(this, CallbackSet);

            Debug.Assert(StateViewTable.Count == 0);
            Debug.Assert(BlockStateViewTable.Count == 0);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
