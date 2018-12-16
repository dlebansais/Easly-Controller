using System;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyControllerView
    {
        IReadOnlyController Controller { get; }
        IReadOnlyStateViewDictionary StateViewTable { get; }
    }

    public class ReadOnlyControllerView : IReadOnlyControllerView
    {
        #region Init
        public static IReadOnlyControllerView Create(IReadOnlyController controller)
        {
            ReadOnlyControllerView View = new ReadOnlyControllerView(controller);
            View.Init();
            return View;
        }

        protected ReadOnlyControllerView(IReadOnlyController controller)
        {
            Controller = controller;

            StateViewTable = CreateStateViewTable();
        }

        protected virtual void Init()
        {
            IReadOnlyAttachCallbackSet CallbackSet = CreateCallbackSet();
            Controller.Attach(this, CallbackSet);

            Debug.Assert(StateViewTable.Count == Controller.Stats.NodeCount);

            Controller.StateCreated += OnNodeStateCreated;
            Controller.StateInitialized += OnNodeStateInitialized;
        }
        #endregion

        #region Properties
        public IReadOnlyController Controller { get; }
        public IReadOnlyStateViewDictionary StateViewTable { get; }
        #endregion

        #region Client Interface
        public virtual void OnNodeStateCreated(IReadOnlyNodeState state)
        {
            Debug.Assert(state != null);
            Debug.Assert(!StateViewTable.ContainsKey(state));

            IReadOnlyNodeStateView StateView;

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

                default:
                    throw new ArgumentOutOfRangeException(nameof(state));
            }

            StateViewTable.Add(state, StateView);
        }

        public virtual void OnNodeStateInitialized(IReadOnlyNodeState state)
        {
            Debug.Assert(state != null);
            Debug.Assert(StateViewTable.ContainsKey(state));
        }

        public virtual void OnBlockListInnerCreated(IReadOnlyBlockListInner inner)
        {
            inner.BlockStateCreated += OnBlockStateCreated;
            inner.BlockStateRemoved += OnBlockStateRemoved;
        }

        public virtual void OnBlockStateCreated(IReadOnlyBlockState blockState)
        {
            Debug.Assert(blockState != null);
        }

        public virtual void OnBlockStateRemoved(IReadOnlyBlockState blockState)
        {
            Debug.Assert(blockState != null);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeStateViewDictionary object.
        /// </summary>
        protected virtual IReadOnlyStateViewDictionary CreateStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyControllerView));
            return new ReadOnlyStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxAttachCallbackSet object.
        /// </summary>
        protected virtual IReadOnlyAttachCallbackSet CreateCallbackSet()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyControllerView));
            return new ReadOnlyAttachCallbackSet()
            {
                NodeStateAttachedHandler = OnNodeStateCreated,
                BlockListInnerAttachedHandler = OnBlockListInnerCreated,
                BlockStateAttachedHandler = OnBlockStateCreated,
            };
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateView object.
        /// </summary>
        protected virtual IReadOnlyPlaceholderNodeStateView CreatePlaceholderNodeStateView(IReadOnlyPlaceholderNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyControllerView));
            return new ReadOnlyPlaceholderNodeStateView(state);
        }

        /// <summary>
        /// Creates a IxxxOptionalNodeStateView object.
        /// </summary>
        protected virtual IReadOnlyOptionalNodeStateView CreateOptionalNodeStateView(IReadOnlyOptionalNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyControllerView));
            return new ReadOnlyOptionalNodeStateView(state);
        }

        /// <summary>
        /// Creates a IxxxPatternStateView object.
        /// </summary>
        protected virtual IReadOnlyPatternStateView CreatePatternStateView(IReadOnlyPatternState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyControllerView));
            return new ReadOnlyPatternStateView(state);
        }

        /// <summary>
        /// Creates a IxxxSourceStateView object.
        /// </summary>
        protected virtual IReadOnlySourceStateView CreateSourceStateView(IReadOnlySourceState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyControllerView));
            return new ReadOnlySourceStateView(state);
        }
        #endregion
    }
}
