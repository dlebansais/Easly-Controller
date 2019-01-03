using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System;
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
        public override void OnStateInserted(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, IWriteableNodeState state)
        {
            base.OnStateInserted(inner, nodeIndex, state);

            IFrameNodeState InsertedState = state as IFrameNodeState;
            Debug.Assert(InsertedState != null);

            BuildCellViewRecursive(InsertedState);

            IFrameNodeStateView InsertedStateView = StateViewTable[InsertedState];
            string PropertyName = inner.PropertyName;
            IFrameCellView InsertedRootCellView = InsertedStateView.RootCellView;
            IFrameNodeState OwnerState = (IFrameNodeState)inner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];
            IFrameCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));
            IFrameMutableCellViewCollection EmbeddingCellView = CellViewTable[PropertyName] as IFrameMutableCellViewCollection;
            Debug.Assert(EmbeddingCellView != null);

            if ((inner is IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> AsBlockListInner) && (nodeIndex is IFrameBrowsingExistingBlockNodeIndex AsBlockListIndex))
            {
                int BlockIndex = AsBlockListIndex.BlockIndex;
                IFrameBlockState BlockState = AsBlockListInner.BlockStateList[BlockIndex];
                IFrameBlockStateView BlockStateView = BlockStateViewTable[BlockState];
                //...
            }
            else if ((inner is IFrameListInner<IFrameBrowsingListNodeIndex> AsListInner) && (nodeIndex is IFrameBrowsingListNodeIndex AsListIndex))
            {
                int Index = AsListIndex.Index;
                EmbeddingCellView.Insert(InsertedRootCellView);
            }
            else
                throw new ArgumentOutOfRangeException(nameof(inner));
        }

        protected virtual void BuildCellViewRecursive(IFrameNodeState state)
        {
            IFrameNodeStateView StateView = StateViewTable[state];
            BuildCellView(StateView);

            IFrameInnerReadOnlyDictionary<string> InnerTable = state.InnerTable;
            foreach (KeyValuePair<string, IFrameInner<IFrameBrowsingChildIndex>> Entry in InnerTable)
            {
                IFrameInner<IFrameBrowsingChildIndex> Inner = Entry.Value;
                switch (Inner)
                {
                    case IFramePlaceholderInner<IFrameBrowsingPlaceholderNodeIndex> AsPlaceholderInner:
                        BuildCellViewRecursive(AsPlaceholderInner.ChildState);
                        break;

                    case IFrameOptionalInner<IFrameBrowsingOptionalNodeIndex> AsOptionalInner:
                        BuildCellViewRecursive(AsOptionalInner.ChildState);
                        break;

                    case IFrameListInner<IFrameBrowsingListNodeIndex> AsListInner:
                        foreach (IFrameNodeState ChildState in AsListInner.StateList)
                            BuildCellViewRecursive(ChildState);
                        break;

                    case IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> AsBlockListInner:
                        foreach (IFrameBlockState BlockState in AsBlockListInner.BlockStateList)
                        {
                            BuildCellViewRecursive(BlockState.PatternState);
                            BuildCellViewRecursive(BlockState.SourceState);

                            foreach (IFrameNodeState ChildState in BlockState.StateList)
                                BuildCellViewRecursive(ChildState);
                        }
                        break;
                }
            }
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

            if (!(other is IFrameControllerView AsControllerView))
                return false;

            if (!base.IsEqual(comparer, AsControllerView))
                return false;

            if (TemplateSet != AsControllerView.TemplateSet)
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
