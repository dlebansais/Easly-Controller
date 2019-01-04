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
            Controller.StateInserted += OnStateInserted;
            Controller.StateReplaced += OnStateReplaced;
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
        /// <param name="nodeIndex">Index of the inserted block state.</param>
        /// <param name="blockState">The block state inserted.</param>
        public virtual void OnBlockStateInserted(IWriteableBrowsingExistingBlockNodeIndex nodeIndex, IWriteableBlockState blockState)
        {
            Debug.Assert(blockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(blockState));

            Debug.Assert(StateViewTable.ContainsKey(blockState.PatternState));
            Debug.Assert(StateViewTable.ContainsKey(blockState.SourceState));

            foreach (IWriteableNodeState State in blockState.StateList)
                Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a state is inserted in the controller.
        /// </summary>
        /// <param name="inner">Inner in which the state is inserted.</param>
        /// <param name="nodeIndex">Index of the inserted state.</param>
        /// <param name="state">The state inserted.</param>
        public virtual void OnStateInserted(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, IWriteableNodeState state, bool isBlockInserted)
        {
            Debug.Assert(state != null);
            Debug.Assert(StateViewTable.ContainsKey(state));
        }

        /// <summary>
        /// Handler called every time a state is inserted in the controller.
        /// </summary>
        /// <param name="inner">Inner in which the state is inserted.</param>
        /// <param name="nodeIndex">Index of the inserted state.</param>
        /// <param name="state">The state inserted.</param>
        public virtual void OnStateReplaced(IWriteableInner<IWriteableBrowsingChildIndex> inner, IWriteableBrowsingChildIndex nodeIndex, IWriteableNodeState state)
        {
            Debug.Assert(state != null);
            Debug.Assert(StateViewTable.ContainsKey(state));
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IWriteableControllerView"/> objects.
        /// </summary>
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
