﻿using EaslyController.ReadOnly;
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
        int GlobalDebugIndex { get; set; }

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
        public int GlobalDebugIndex { get; set; }

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
                IFrameBlockStateView BlockStateView = Entry.Value;
                Debug.Assert(BlockStateView.RootCellView != null);
                Debug.Assert(BlockStateView.EmbeddingCellView != null);
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
        /// <param name="nodeIndex">Index of the inserted block state.</param>
        /// <param name="blockState">The block state inserted.</param>
        public override void OnBlockStateInserted(IWriteableBrowsingExistingBlockNodeIndex nodeIndex, IWriteableBlockState blockState)
        {
            base.OnBlockStateInserted(nodeIndex, blockState);

            IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> ParentInner = blockState.ParentInner as IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>;
            IFrameNodeState OwnerState = ParentInner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            IFrameBlockStateView BlockStateView = BlockStateViewTable[(IFrameBlockState)blockState];
            BuildBlockCellView(OwnerStateView, BlockStateView);

            IFrameCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = ParentInner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));
            IFrameMutableCellViewCollection EmbeddingCellView = CellViewTable[PropertyName] as IFrameMutableCellViewCollection;
            Debug.Assert(EmbeddingCellView != null);

            int BlockIndex = nodeIndex.BlockIndex;
            EmbeddingCellView.Insert(BlockIndex, BlockStateView.RootCellView);
        }

        /// <summary>
        /// Handler called every time a state is inserted in the controller.
        /// </summary>
        /// <param name="inner">Inner in which the state is inserted.</param>
        /// <param name="nodeIndex">Index of the inserted state.</param>
        /// <param name="state">The state inserted.</param>
        public override void OnStateInserted(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, IWriteableNodeState state, bool isBlockInserted)
        {
            base.OnStateInserted(inner, nodeIndex, state, isBlockInserted);

            IFrameNodeState InsertedState = state as IFrameNodeState;
            Debug.Assert(InsertedState != null);

            BuildCellViewRecursive(InsertedState);

            if ((inner is IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> AsBlockListInner) && (nodeIndex is IFrameBrowsingExistingBlockNodeIndex AsBlockListIndex))
                OnBlockListStateInserted(AsBlockListInner, AsBlockListIndex, isBlockInserted, InsertedState);

            else if ((inner is IFrameListInner<IFrameBrowsingListNodeIndex> AsListInner) && (nodeIndex is IFrameBrowsingListNodeIndex AsListIndex))
                OnListStateInserted(AsListInner, AsListIndex, InsertedState);

            else
                throw new ArgumentOutOfRangeException(nameof(inner));
        }

        protected virtual void OnBlockListStateInserted(IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> inner, IFrameBrowsingExistingBlockNodeIndex nodeIndex, bool isBlockInserted, IFrameNodeState insertedState)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            if (!isBlockInserted)
            {
                IFrameNodeState OwnerState = (IFrameNodeState)inner.Owner;
                IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

                int BlockIndex = nodeIndex.BlockIndex;
                IFrameBlockState BlockState = inner.BlockStateList[BlockIndex];
                IFrameBlockStateView BlockStateView = BlockStateViewTable[BlockState];
                IFrameMutableCellViewCollection EmbeddingCellView = BlockStateView.EmbeddingCellView;
                Debug.Assert(EmbeddingCellView != null);

                IFrameNodeStateView InsertedStateView = StateViewTable[insertedState];
                IFrameContainerCellView InsertedCellView = CreateFrameCellView(OwnerStateView, EmbeddingCellView, InsertedStateView);

                int Index = nodeIndex.Index;
                EmbeddingCellView.Insert(Index, InsertedCellView);
            }
        }

        protected virtual void OnListStateInserted(IFrameListInner<IFrameBrowsingListNodeIndex> inner, IFrameBrowsingListNodeIndex nodeIndex, IFrameNodeState insertedState)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IFrameNodeState OwnerState = inner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            IFrameCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = inner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));
            IFrameMutableCellViewCollection EmbeddingCellView = CellViewTable[PropertyName] as IFrameMutableCellViewCollection;
            Debug.Assert(EmbeddingCellView != null);

            IFrameNodeStateView InsertedStateView = StateViewTable[insertedState];
            IFrameContainerCellView InsertedCellView = CreateFrameCellView(OwnerStateView, EmbeddingCellView, InsertedStateView);

            int Index = nodeIndex.Index;
            EmbeddingCellView.Insert(Index, InsertedCellView);
        }

        /// <summary>
        /// Handler called every time a state is inserted in the controller.
        /// </summary>
        /// <param name="inner">Inner in which the state is inserted.</param>
        /// <param name="nodeIndex">Index of the inserted state.</param>
        /// <param name="state">The state inserted.</param>
        public override void OnStateReplaced(IWriteableInner<IWriteableBrowsingChildIndex> inner, IWriteableBrowsingChildIndex nodeIndex, IWriteableNodeState state)
        {
            base.OnStateReplaced(inner, nodeIndex, state);

            IFrameNodeState ReplacedState = state as IFrameNodeState;
            Debug.Assert(ReplacedState != null);

            BuildCellViewRecursive(ReplacedState);

            if ((inner is IFramePlaceholderInner<IFrameBrowsingPlaceholderNodeIndex> AsPlaceholderInner) && (nodeIndex is IFrameBrowsingPlaceholderNodeIndex AsPlaceholderIndex))
                OnPlaceholderStateReplaced(AsPlaceholderInner, AsPlaceholderIndex, ReplacedState);

            else if ((inner is IFrameOptionalInner<IFrameBrowsingOptionalNodeIndex> AsOptionalInner) && (nodeIndex is IFrameBrowsingOptionalNodeIndex AsOptionalIndex))
                OnOptionalStateReplaced(AsOptionalInner, AsOptionalIndex, ReplacedState);

            else if ((inner is IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> AsBlockListInner) && (nodeIndex is IFrameBrowsingExistingBlockNodeIndex AsBlockListIndex))
                OnBlockListStateReplaced(AsBlockListInner, AsBlockListIndex, ReplacedState);

            else if ((inner is IFrameListInner<IFrameBrowsingListNodeIndex> AsListInner) && (nodeIndex is IFrameBrowsingListNodeIndex AsListIndex))
                OnListStateReplaced(AsListInner, AsListIndex, ReplacedState);

            else
                throw new ArgumentOutOfRangeException(nameof(inner));
        }

        protected virtual void OnPlaceholderStateReplaced(IFramePlaceholderInner<IFrameBrowsingPlaceholderNodeIndex> inner, IFrameBrowsingPlaceholderNodeIndex nodeIndex, IFrameNodeState replacedState)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IFrameNodeState OwnerState = (IFrameNodeState)inner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            Debug.Assert(OwnerStateView != null);

            IFrameCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = inner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));
            IFrameContainerCellView PreviousCellView = CellViewTable[PropertyName] as IFrameContainerCellView;
            Debug.Assert(PreviousCellView != null);
            IFrameMutableCellViewCollection EmbeddingCellView = PreviousCellView.ParentCellView;

            IFrameNodeStateView ReplacedStateView = StateViewTable[replacedState];
            IFrameContainerCellView ReplacedCellView = CreateFrameCellView(OwnerStateView, EmbeddingCellView, ReplacedStateView);

            EmbeddingCellView.Replace(PreviousCellView, ReplacedCellView);
            OwnerStateView.ReplaceCellView(PropertyName, ReplacedCellView);
        }

        protected virtual void OnOptionalStateReplaced(IFrameOptionalInner<IFrameBrowsingOptionalNodeIndex> inner, IFrameBrowsingOptionalNodeIndex nodeIndex, IFrameNodeState replacedState)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IFrameNodeState OwnerState = (IFrameNodeState)inner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            Debug.Assert(OwnerStateView != null);

            IFrameCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = inner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));
            IFrameContainerCellView PreviousCellView = CellViewTable[PropertyName] as IFrameContainerCellView;
            Debug.Assert(PreviousCellView != null);
            IFrameMutableCellViewCollection EmbeddingCellView = PreviousCellView.ParentCellView;

            IFrameNodeStateView ReplacedStateView = StateViewTable[replacedState];
            IFrameContainerCellView ReplacedCellView = CreateFrameCellView(OwnerStateView, EmbeddingCellView, ReplacedStateView);

            EmbeddingCellView.Replace(PreviousCellView, ReplacedCellView);
            OwnerStateView.ReplaceCellView(PropertyName, ReplacedCellView);
        }

        protected virtual void OnBlockListStateReplaced(IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> inner, IFrameBrowsingExistingBlockNodeIndex nodeIndex, IFrameNodeState replacedState)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IFrameNodeState OwnerState = (IFrameNodeState)inner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            int BlockIndex = nodeIndex.BlockIndex;
            IFrameBlockState BlockState = inner.BlockStateList[BlockIndex];
            IFrameBlockStateView BlockStateView = BlockStateViewTable[BlockState];
            IFrameMutableCellViewCollection EmbeddingCellView = BlockStateView.EmbeddingCellView;
            Debug.Assert(EmbeddingCellView != null);

            IFrameNodeStateView ReplacedStateView = StateViewTable[replacedState];
            IFrameContainerCellView ReplacedCellView = CreateFrameCellView(OwnerStateView, EmbeddingCellView, ReplacedStateView);

            int Index = nodeIndex.Index;
            EmbeddingCellView.Replace(Index, ReplacedCellView);
        }

        protected virtual void OnListStateReplaced(IFrameListInner<IFrameBrowsingListNodeIndex> inner, IFrameBrowsingListNodeIndex nodeIndex, IFrameNodeState replacedState)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IFrameNodeState OwnerState = (IFrameNodeState)inner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            IFrameCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = inner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));
            IFrameMutableCellViewCollection EmbeddingCellView = CellViewTable[PropertyName] as IFrameMutableCellViewCollection;
            Debug.Assert(EmbeddingCellView != null);

            IFrameNodeStateView ReplacedStateView = StateViewTable[replacedState];
            IFrameContainerCellView ReplacedCellView = CreateFrameCellView(OwnerStateView, EmbeddingCellView, ReplacedStateView);

            int Index = nodeIndex.Index;
            EmbeddingCellView.Replace(Index, ReplacedCellView);
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
            return new FramePlaceholderNodeStateView(this, (IFramePlaceholderNodeState)state, TemplateSet);
        }

        /// <summary>
        /// Creates a IxxxOptionalNodeStateView object.
        /// </summary>
        protected override IReadOnlyOptionalNodeStateView CreateOptionalNodeStateView(IReadOnlyOptionalNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameOptionalNodeStateView(this, (IFrameOptionalNodeState)state, TemplateSet);
        }

        /// <summary>
        /// Creates a IxxxPatternStateView object.
        /// </summary>
        protected override IReadOnlyPatternStateView CreatePatternStateView(IReadOnlyPatternState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FramePatternStateView(this, (IFramePatternState)state, TemplateSet);
        }

        /// <summary>
        /// Creates a IxxxSourceStateView object.
        /// </summary>
        protected override IReadOnlySourceStateView CreateSourceStateView(IReadOnlySourceState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameSourceStateView(this, (IFrameSourceState)state, TemplateSet);
        }

        /// <summary>
        /// Creates a IxxxBlockStateView object.
        /// </summary>
        protected override IReadOnlyBlockStateView CreateBlockStateView(IReadOnlyBlockState blockState)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameBlockStateView(this, (IFrameBlockState)blockState, TemplateSet);
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        protected virtual IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameMutableCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameContainerCellView(stateView, parentCellView, childStateView);
        }
        #endregion
    }
}
