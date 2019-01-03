using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public interface IFrameControllerView : IWriteableControllerView
    {
        /// <summary>
        /// The controller.
        /// </summary>
        new IFrameController Controller { get; }

        /// <summary>
        /// Table of views of each state in the controller.
        /// </summary>
        new IFrameStateViewDictionary StateViewTable { get; }

        /// <summary>
        /// Table of views of each block state in the controller.
        /// </summary>
        new IFrameBlockStateViewDictionary BlockStateViewTable { get; }

        /// <summary>
        /// Template set describing the node tree.
        /// </summary>
        IFrameTemplateSet TemplateSet { get; }
    }

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public class FrameControllerView : WriteableControllerView, IFrameControllerView
    {
        #region Init
        /// <summary>
        /// Creates and initializes a new instance of a <see cref="FrameControllerView"/> object.
        /// </summary>
        /// <param name="controller">The controller on which the view is attached.</param>
        /// <param name="templateSet">The template set used to describe the view.</param>
        public static IFrameControllerView Create(IFrameController controller, IFrameTemplateSet templateSet)
        {
            FrameControllerView View = new FrameControllerView(controller, templateSet);
            View.Init();
            return View;
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="FrameControllerView"/> object.
        /// </summary>
        /// <param name="controller">The controller on which the view is attached.</param>
        /// <param name="templateSet">The template set used to describe the view.</param>
        protected FrameControllerView(IFrameController controller, IFrameTemplateSet templateSet)
            : base(controller)
        {
            Debug.Assert(templateSet != null);

            TemplateSet = templateSet;
        }

        /// <summary>
        /// Initializes the view by attaching it to the controller.
        /// </summary>
        protected override void Init()
        {
            base.Init();

            foreach (KeyValuePair<IFrameNodeState, IFrameNodeStateView> Entry in StateViewTable)
            {
                IFrameNodeStateView StateView = Entry.Value;
                BuildCellView(StateView);
            }

            foreach (KeyValuePair<IFrameBlockState, IFrameBlockStateView> Entry in BlockStateViewTable)
            {
                IFrameBlockState BlockState = Entry.Key;
                IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> ParentInner = BlockState.ParentInner as IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>;
                IFrameNodeState Owner = ParentInner.Owner;
                IFrameNodeStateView StateView = StateViewTable[Owner];

                IFrameBlockStateView BlockStateView = Entry.Value;
                BuildBlockCellView(StateView, BlockStateView);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller.
        /// </summary>
        public new IFrameController Controller { get { return (IFrameController)base.Controller; } }

        /// <summary>
        /// Table of views of each state in the controller.
        /// </summary>
        public new IFrameStateViewDictionary StateViewTable { get { return (IFrameStateViewDictionary)base.StateViewTable; } }

        /// <summary>
        /// Table of views of each block state in the controller.
        /// </summary>
        public new IFrameBlockStateViewDictionary BlockStateViewTable { get { return (IFrameBlockStateViewDictionary)base.BlockStateViewTable; } }

        /// <summary>
        /// Template set describing the node tree.
        /// </summary>
        public IFrameTemplateSet TemplateSet { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Handler called every time a block state is inserted in the controller.
        /// </summary>
        /// <param name="blockState">The block state inserted.</param>
        public override void OnBlockStateInserted(IWriteableBrowsingCollectionNodeIndex nodeIndex, IWriteableBlockState blockState)
        {
            base.OnBlockStateInserted(nodeIndex, blockState);

            IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> ParentInner = blockState.ParentInner as IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>;
            IFrameNodeState Owner = ParentInner.Owner;
            IFrameNodeStateView StateView = StateViewTable[Owner];

            IFrameBlockStateView BlockStateView = BlockStateViewTable[(IFrameBlockState)blockState];
            BuildBlockCellView(StateView, BlockStateView);
        }

        /// <summary>
        /// Handler called every time a state is inserted in the controller.
        /// </summary>
        /// <param name="state">The state inserted.</param>
        public override void OnStateInserted(IWriteableBrowsingCollectionNodeIndex nodeIndex, IWriteableNodeState state)
        {
            base.OnStateInserted(nodeIndex, state);

            IFrameNodeStateView StateView = StateViewTable[(IFrameNodeState)state];
            BuildCellView(StateView);
        }
        #endregion

        #region Implementation
        protected virtual void BuildCellView(IFrameNodeStateView stateView)
        {
            stateView.BuildRootCellView(this);
        }

        protected virtual void ClearCellView(IFrameNodeStateView stateView)
        {
        }

        protected virtual void BuildBlockCellView(IFrameNodeStateView stateView, IFrameBlockStateView blockStateView)
        {
            blockStateView.BuildRootCellView(this, stateView);
        }

        protected virtual void ClearBlockCellView(IFrameNodeStateView stateView, IFrameBlockStateView blockStateView)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameControllerView"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameControllerView AsFrame))
                return false;

            if (!base.IsEqual(comparer, AsFrame))
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
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxBlockStateViewDictionary object.
        /// </summary>
        protected override IReadOnlyBlockStateViewDictionary CreateBlockStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameBlockStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxAttachCallbackSet object.
        /// </summary>
        protected override IReadOnlyAttachCallbackSet CreateCallbackSet()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameAttachCallbackSet()
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
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FramePlaceholderNodeStateView((IFramePlaceholderNodeState)state, TemplateSet);
        }

        /// <summary>
        /// Creates a IxxxOptionalNodeStateView object.
        /// </summary>
        protected override IReadOnlyOptionalNodeStateView CreateOptionalNodeStateView(IReadOnlyOptionalNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameOptionalNodeStateView((IFrameOptionalNodeState)state, TemplateSet);
        }

        /// <summary>
        /// Creates a IxxxPatternStateView object.
        /// </summary>
        protected override IReadOnlyPatternStateView CreatePatternStateView(IReadOnlyPatternState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FramePatternStateView((IFramePatternState)state, TemplateSet);
        }

        /// <summary>
        /// Creates a IxxxSourceStateView object.
        /// </summary>
        protected override IReadOnlySourceStateView CreateSourceStateView(IReadOnlySourceState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameSourceStateView((IFrameSourceState)state, TemplateSet);
        }

        /// <summary>
        /// Creates a IxxxBlockStateView object.
        /// </summary>
        protected override IReadOnlyBlockStateView CreateBlockStateView(IReadOnlyBlockState blockState)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameBlockStateView((IFrameBlockState)blockState, TemplateSet);
        }
        #endregion
    }
}
