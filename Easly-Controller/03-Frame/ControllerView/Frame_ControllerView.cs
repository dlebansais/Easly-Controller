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
        /// Handler called every time a block state is removed from the controller.
        /// </summary>
        /// <param name="nodeIndex">Index of the removed block state.</param>
        /// <param name="blockState">The block state removed.</param>
        public override void OnBlockStateRemoved(IWriteableBrowsingExistingBlockNodeIndex nodeIndex, IWriteableBlockState blockState)
        {
            base.OnBlockStateRemoved(nodeIndex, blockState);

            IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> ParentInner = blockState.ParentInner as IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>;
            IFrameNodeState OwnerState = ParentInner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            IFrameCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = ParentInner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));
            IFrameMutableCellViewCollection EmbeddingCellView = CellViewTable[PropertyName] as IFrameMutableCellViewCollection;
            Debug.Assert(EmbeddingCellView != null);

            int BlockIndex = nodeIndex.BlockIndex;
            EmbeddingCellView.Remove(BlockIndex);
        }

        /// <summary>
        /// Handler called every time a state is inserted in the controller.
        /// </summary>
        /// <param name="inner">Inner in which the state is inserted.</param>
        /// <param name="nodeIndex">Index of the inserted state.</param>
        /// <param name="state">The state inserted.</param>
        public override void OnStateInserted(IWriteableBrowsingCollectionNodeIndex nodeIndex, IWriteableNodeState state, bool isBlockInserted)
        {
            base.OnStateInserted(nodeIndex, state, isBlockInserted);

            IFrameNodeState InsertedState = state as IFrameNodeState;
            Debug.Assert(InsertedState != null);

            BuildCellViewRecursive(InsertedState);

            IFrameInner<IFrameBrowsingChildIndex> ParentInner = InsertedState.ParentInner;
            Debug.Assert(ParentInner != null);

            if ((ParentInner is IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> AsBlockListInner) && (nodeIndex is IFrameBrowsingExistingBlockNodeIndex AsBlockListIndex))
                OnBlockListStateInserted(AsBlockListInner, AsBlockListIndex, isBlockInserted, InsertedState);

            else if ((ParentInner is IFrameListInner<IFrameBrowsingListNodeIndex> AsListInner) && (nodeIndex is IFrameBrowsingListNodeIndex AsListIndex))
                OnListStateInserted(AsListInner, AsListIndex, InsertedState);

            else
                throw new ArgumentOutOfRangeException(nameof(state));
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
        /// Handler called every time a state is removed in the controller.
        /// </summary>
        /// <param name="state">The state removed.</param>
        public override void OnNodeStateRemoved(IReadOnlyNodeState state)
        {

            base.OnNodeStateRemoved(state);
        }

        /// <summary>
        /// Handler called every time a state is removed from the controller.
        /// </summary>
        /// <param name="nodeIndex">Index of the removed state.</param>
        /// <param name="state">The state removed.</param>
        public override void OnStateRemoved(IWriteableBrowsingCollectionNodeIndex nodeIndex, IWriteableNodeState state)
        {
            base.OnStateRemoved(nodeIndex, state);

            IFrameNodeState RemovedState = state as IFrameNodeState;
            Debug.Assert(RemovedState != null);

            IFrameInner<IFrameBrowsingChildIndex> ParentInner = RemovedState.ParentInner;
            Debug.Assert(ParentInner != null);

            if ((ParentInner is IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> AsBlockListInner) && (nodeIndex is IFrameBrowsingExistingBlockNodeIndex AsBlockListIndex))
                OnBlockListStateRemoved(AsBlockListInner, AsBlockListIndex, RemovedState);

            else if ((ParentInner is IFrameListInner<IFrameBrowsingListNodeIndex> AsListInner) && (nodeIndex is IFrameBrowsingListNodeIndex AsListIndex))
                OnListStateRemoved(AsListInner, AsListIndex, RemovedState);

            else
                throw new ArgumentOutOfRangeException(nameof(state));
        }

        protected virtual void OnBlockListStateRemoved(IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> inner, IFrameBrowsingExistingBlockNodeIndex nodeIndex, IFrameNodeState removedState)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IFrameNodeState OwnerState = inner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            int BlockIndex = nodeIndex.BlockIndex;
            IFrameBlockState BlockState = inner.BlockStateList[BlockIndex];
            IFrameBlockStateView BlockStateView = BlockStateViewTable[BlockState];
            IFrameMutableCellViewCollection EmbeddingCellView = BlockStateView.EmbeddingCellView;
            Debug.Assert(EmbeddingCellView != null);

            int Index = nodeIndex.Index;
            EmbeddingCellView.Remove(Index);
        }

        protected virtual void OnListStateRemoved(IFrameListInner<IFrameBrowsingListNodeIndex> inner, IFrameBrowsingListNodeIndex nodeIndex, IFrameNodeState removedState)
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

            int Index = nodeIndex.Index;
            EmbeddingCellView.Remove(Index);
        }

        /// <summary>
        /// Handler called every time a state is inserted in the controller.
        /// </summary>
        /// <param name="nodeIndex">Index of the inserted state.</param>
        /// <param name="state">The state inserted.</param>
        public override void OnStateReplaced(IWriteableBrowsingChildIndex nodeIndex, IWriteableNodeState state)
        {
            base.OnStateReplaced(nodeIndex, state);

            IFrameNodeState ReplacedState = state as IFrameNodeState;
            Debug.Assert(ReplacedState != null);

            BuildCellViewRecursive(ReplacedState);

            IFrameInner<IFrameBrowsingChildIndex> ParentInner = ReplacedState.ParentInner;
            Debug.Assert(ParentInner != null);

            if ((ParentInner is IFramePlaceholderInner<IFrameBrowsingPlaceholderNodeIndex> AsPlaceholderInner) && (nodeIndex is IFrameBrowsingPlaceholderNodeIndex AsPlaceholderIndex))
                OnPlaceholderStateReplaced(AsPlaceholderInner, AsPlaceholderIndex, ReplacedState);

            else if ((ParentInner is IFrameOptionalInner<IFrameBrowsingOptionalNodeIndex> AsOptionalInner) && (nodeIndex is IFrameBrowsingOptionalNodeIndex AsOptionalIndex))
                OnOptionalStateReplaced(AsOptionalInner, AsOptionalIndex, ReplacedState);

            else if ((ParentInner is IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> AsBlockListInner) && (nodeIndex is IFrameBrowsingExistingBlockNodeIndex AsBlockListIndex))
                OnBlockListStateReplaced(AsBlockListInner, AsBlockListIndex, ReplacedState);

            else if ((ParentInner is IFrameListInner<IFrameBrowsingListNodeIndex> AsListInner) && (nodeIndex is IFrameBrowsingListNodeIndex AsListIndex))
                OnListStateReplaced(AsListInner, AsListIndex, ReplacedState);

            else
                throw new ArgumentOutOfRangeException(nameof(state));
        }

        protected virtual void OnPlaceholderStateReplaced(IFramePlaceholderInner<IFrameBrowsingPlaceholderNodeIndex> inner, IFrameBrowsingPlaceholderNodeIndex nodeIndex, IFrameNodeState replacedState)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IFrameNodeState OwnerState = inner.Owner;
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

            IFrameNodeState OwnerState = inner.Owner;
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

            IFrameNodeState OwnerState = inner.Owner;
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

            IFrameNodeState OwnerState = inner.Owner;
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

        /// <summary>
        /// Handler called every time a state is assigned in the controller.
        /// </summary>
        /// <param name="nodeIndex">Index of the assigned state.</param>
        /// <param name="state">The state assigned.</param>
        public override void OnStateAssigned(IWriteableBrowsingOptionalNodeIndex nodeIndex, IWriteableOptionalNodeState state)
        {
            base.OnStateAssigned(nodeIndex, state);

            IFrameNodeState AssignedState = state as IFrameNodeState;
            Debug.Assert(AssignedState != null);

            BuildCellViewRecursive(AssignedState);

            OnOptionalStateReplaced(state.ParentInner as IFrameOptionalInner<IFrameBrowsingOptionalNodeIndex>, nodeIndex as IFrameBrowsingOptionalNodeIndex, state as IFrameNodeState);
        }

        /// <summary>
        /// Handler called every time a state is unassigned in the controller.
        /// </summary>
        /// <param name="nodeIndex">Index of the unassigned state.</param>
        /// <param name="state">The state unassigned.</param>
        public override void OnStateUnassigned(IWriteableBrowsingOptionalNodeIndex nodeIndex, IWriteableOptionalNodeState state)
        {
            base.OnStateUnassigned(nodeIndex, state);

            IFrameNodeState UnassignedState = state as IFrameNodeState;
            Debug.Assert(UnassignedState != null);

            BuildCellViewRecursive(UnassignedState);

            OnOptionalStateReplaced(state.ParentInner as IFrameOptionalInner<IFrameBrowsingOptionalNodeIndex>, nodeIndex as IFrameBrowsingOptionalNodeIndex, state as IFrameNodeState);
        }

        /// <summary>
        /// Handler called every time a state is moved in the controller.
        /// </summary>
        /// <param name="nodeIndex">Index of the moved state.</param>
        /// <param name="state">The moved state.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        public override void OnStateMoved(IWriteableBrowsingChildIndex nodeIndex, IWriteableNodeState state, int direction)
        {
            base.OnStateMoved(nodeIndex, state, direction);

            IFrameNodeState MovedState = state as IFrameNodeState;
            Debug.Assert(MovedState != null);

            IFrameInner<IFrameBrowsingChildIndex> ParentInner = MovedState.ParentInner;
            Debug.Assert(ParentInner != null);

            if ((ParentInner is IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> AsBlockListInner) && (nodeIndex is IFrameBrowsingExistingBlockNodeIndex AsBlockListIndex))
                OnBlockListStateMoved(AsBlockListInner, AsBlockListIndex, direction);

            else if ((ParentInner is IFrameListInner<IFrameBrowsingListNodeIndex> AsListInner) && (nodeIndex is IFrameBrowsingListNodeIndex AsListIndex))
                OnListStateMoved(AsListInner, AsListIndex, direction);

            else
                throw new ArgumentOutOfRangeException(nameof(state));
        }

        protected virtual void OnBlockListStateMoved(IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> inner, IFrameBrowsingExistingBlockNodeIndex nodeIndex, int direction)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IFrameNodeState OwnerState = inner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            int BlockIndex = nodeIndex.BlockIndex;
            IFrameBlockState BlockState = inner.BlockStateList[BlockIndex];
            IFrameBlockStateView BlockStateView = BlockStateViewTable[BlockState];
            IFrameMutableCellViewCollection EmbeddingCellView = BlockStateView.EmbeddingCellView;
            Debug.Assert(EmbeddingCellView != null);

            int Index = nodeIndex.Index;
            EmbeddingCellView.Move(Index, direction);
        }

        protected virtual void OnListStateMoved(IFrameListInner<IFrameBrowsingListNodeIndex> inner, IFrameBrowsingListNodeIndex nodeIndex, int direction)
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

            int Index = nodeIndex.Index;
            EmbeddingCellView.Move(Index, direction);
        }

        /// <summary>
        /// Handler called every time a block state is moved in the controller.
        /// </summary>
        /// <param name="inner">Inner where the block is moved.</param>
        /// <param name="blockIndex">Index of the moved block.</param>
        /// <param name="direction">The change in position, relative to the current block position.</param>
        public override void OnBlockStateMoved(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex, int direction)
        {
            base.OnBlockStateMoved(inner, blockIndex, direction);

            IFrameNodeState OwnerState = inner.Owner as IFrameNodeState;
            Debug.Assert(OwnerState != null);
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            IFrameCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = inner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));
            IFrameMutableCellViewCollection EmbeddingCellView = CellViewTable[PropertyName] as IFrameMutableCellViewCollection;
            Debug.Assert(EmbeddingCellView != null);

            EmbeddingCellView.Move(blockIndex, direction);
        }

        /// <summary>
        /// Handler called every time a block split in the controller.
        /// </summary>
        /// <param name="inner">Inner where the block is split.</param>
        /// <param name="blockIndex">Index of the split block.</param>
        /// <param name="Index">Index of the last node to stay in the old block.</param>
        public override void OnBlockSplit(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex, int index)
        {
            base.OnBlockSplit(inner, blockIndex, index);

            IFrameNodeState OwnerState = inner.Owner as IFrameNodeState;
            Debug.Assert(OwnerState != null);
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            IFrameBlockState FirstBlockState = inner.BlockStateList[blockIndex] as IFrameBlockState;
            Debug.Assert(FirstBlockState != null);

            IFrameBlockStateView FirstBlockStateView = BlockStateViewTable[FirstBlockState];
            BuildBlockCellView(OwnerStateView, FirstBlockStateView);

            IFrameCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = inner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));
            IFrameMutableCellViewCollection FirstEmbeddingCellView = CellViewTable[PropertyName] as IFrameMutableCellViewCollection;
            Debug.Assert(FirstEmbeddingCellView != null);

            FirstEmbeddingCellView.Insert(blockIndex, FirstBlockStateView.RootCellView);

            IFrameBlockState SecondBlockState = inner.BlockStateList[blockIndex + 1] as IFrameBlockState;
            Debug.Assert(SecondBlockState != null);

            IFrameBlockStateView SecondBlockStateView = BlockStateViewTable[SecondBlockState];
            IFrameMutableCellViewCollection SecondEmbeddingCellView = SecondBlockStateView.EmbeddingCellView;
            Debug.Assert(SecondEmbeddingCellView != null);

            for (int i = 0; i < index; i++)
                SecondEmbeddingCellView.Remove(0);
        }

        /// <summary>
        /// Handler called every time two blocks are merged.
        /// </summary>
        /// <param name="inner">Inner where the blocks are merged.</param>
        /// <param name="blockIndex">Index of the first merged block.</param>
        public override void OnBlocksMerged(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, int blockIndex)
        {
            IFrameNodeState OwnerState = inner.Owner as IFrameNodeState;
            Debug.Assert(OwnerState != null);
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            IFrameCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = inner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));
            IFrameMutableCellViewCollection BlockEmbeddingCellView = CellViewTable[PropertyName] as IFrameMutableCellViewCollection;
            Debug.Assert(BlockEmbeddingCellView != null);

            if (blockIndex < 0)
            {
//                PrintCellViewTree(BlockEmbeddingCellView);
                return;
            }

            base.OnBlocksMerged(inner, blockIndex);

            IFrameBlockState FirstBlockState = inner.BlockStateList[blockIndex - 1] as IFrameBlockState;
            Debug.Assert(FirstBlockState != null);
            IFrameBlockStateView FirstBlockStateView = BlockStateViewTable[FirstBlockState];

            ClearBlockCellView(OwnerStateView, FirstBlockStateView);
            BuildBlockCellView(OwnerStateView, FirstBlockStateView);

            BlockEmbeddingCellView.Replace(blockIndex - 1, FirstBlockStateView.RootCellView);
            BlockEmbeddingCellView.Remove(blockIndex);

            //PrintCellViewTree(BlockEmbeddingCellView);
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
            blockStateView.ClearRootCellView(this, stateView);
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

        protected virtual void PrintCellViewTree(IFrameCellView cellView)
        {
            string Tree = cellView.PrintTree(0);

            Debug.WriteLine("Cell View Tree:");
            Debug.WriteLine(Tree);
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
            return new FramePlaceholderNodeStateView(this, (IFramePlaceholderNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxOptionalNodeStateView object.
        /// </summary>
        protected override IReadOnlyOptionalNodeStateView CreateOptionalNodeStateView(IReadOnlyOptionalNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameOptionalNodeStateView(this, (IFrameOptionalNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxPatternStateView object.
        /// </summary>
        protected override IReadOnlyPatternStateView CreatePatternStateView(IReadOnlyPatternState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FramePatternStateView(this, (IFramePatternState)state);
        }

        /// <summary>
        /// Creates a IxxxSourceStateView object.
        /// </summary>
        protected override IReadOnlySourceStateView CreateSourceStateView(IReadOnlySourceState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameSourceStateView(this, (IFrameSourceState)state);
        }

        /// <summary>
        /// Creates a IxxxBlockStateView object.
        /// </summary>
        protected override IReadOnlyBlockStateView CreateBlockStateView(IReadOnlyBlockState blockState)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameBlockStateView(this, (IFrameBlockState)blockState);
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
