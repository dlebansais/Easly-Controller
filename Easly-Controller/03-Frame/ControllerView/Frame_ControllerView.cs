using BaseNode;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System;
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

        /// <summary>
        /// Last line number in the cell tree.
        /// </summary>
        int LastLineNumber { get; }

        /// <summary>
        /// Last column number in the cell tree.
        /// </summary>
        int LastColumnNumber { get; }

        /// <summary>
        /// Enumerate all visible cell views.
        /// </summary>
        /// <param name="list"></param>
        void EnumerateVisibleCellViews(IFrameVisibleCellViewList list);
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

            IFrameNodeState RootState = Controller.RootState;
            IFrameNodeStateView RootStateView = StateViewTable[RootState];

            Debug.Assert(RootStateView.RootCellView == null);
            BuildCellView(RootStateView);

            Refresh(Controller.RootState);
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

        /// <summary>
        /// Last line number in the cell tree.
        /// </summary>
        public int LastLineNumber { get; private set; }

        /// <summary>
        /// Last column number in the cell tree.
        /// </summary>
        public int LastColumnNumber { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Handler called every time a block state is inserted in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public override void OnBlockStateInserted(IWriteableInsertBlockOperation operation)
        {
            base.OnBlockStateInserted(operation);

            IFrameInsertBlockOperation InsertBlockOperation = (IFrameInsertBlockOperation)operation;
            IFrameBlockListInner <IFrameBrowsingBlockNodeIndex> ParentInner = (IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)InsertBlockOperation.BlockState.ParentInner;
            IFrameNodeState OwnerState = ParentInner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            IFrameAssignableCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = ParentInner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));
            IFrameCellViewCollection EmbeddingCellView = CellViewTable[PropertyName] as IFrameCellViewCollection;
            Debug.Assert(EmbeddingCellView != null);

            // Build all cell views for the inserted block.
            IFrameBlockStateView BlockStateView = BlockStateViewTable[((IFrameInsertBlockOperation)operation).BlockState];
            IFrameBlockCellView RootCellView = BuildBlockCellView(OwnerStateView, EmbeddingCellView, BlockStateView);

            // Insert the root cell view in the collection embedding other blocks.
            int BlockIndex = operation.BrowsingIndex.BlockIndex;
            EmbeddingCellView.Insert(BlockIndex, RootCellView);

            Refresh(OwnerState);
        }

        /// <summary>
        /// Handler called every time a block state is removed from the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public override void OnBlockStateRemoved(IWriteableRemoveBlockOperation operation)
        {
            base.OnBlockStateRemoved(operation);

            IFrameRemoveBlockOperation RemoveBlockOperation = (IFrameRemoveBlockOperation)operation;
            IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> ParentInner = (IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)RemoveBlockOperation.BlockState.ParentInner;
            IFrameNodeState OwnerState = ParentInner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            IFrameAssignableCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = ParentInner.PropertyName;

            if (CellViewTable != null)
            {
                Debug.Assert(CellViewTable.ContainsKey(PropertyName));
                IFrameCellViewCollection EmbeddingCellView = CellViewTable[PropertyName] as IFrameCellViewCollection;
                Debug.Assert(EmbeddingCellView != null);

                int BlockIndex = operation.BlockIndex;
                EmbeddingCellView.Remove(BlockIndex);
            }
            else
            {
                Debug.Assert(OwnerStateView.RootCellView == null);
                Debug.Assert(operation.IsNested);
            }

            if (!operation.IsNested)
                Refresh(OwnerState);
        }

        /// <summary>
        /// Handler called every time a block view must be removed from the controller view.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public override void OnBlockViewRemoved(IWriteableRemoveBlockViewOperation operation)
        {
            base.OnBlockViewRemoved(operation);

            IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> ParentInner = ((IFrameRemoveBlockViewOperation)operation).Inner;
            IFrameNodeState OwnerState = ParentInner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            IFrameAssignableCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = ParentInner.PropertyName;

            if (CellViewTable != null)
            {
                Debug.Assert(CellViewTable.ContainsKey(PropertyName));
                IFrameCellViewCollection EmbeddingCellView = CellViewTable[PropertyName] as IFrameCellViewCollection;
                Debug.Assert(EmbeddingCellView != null);

                int BlockIndex = operation.BlockIndex;
                EmbeddingCellView.Remove(BlockIndex);
            }
            else
            {
                Debug.Assert(OwnerStateView.RootCellView == null);
                Debug.Assert(operation.IsNested);
            }

            if (!operation.IsNested)
                Refresh(OwnerState);
        }

        /// <summary>
        /// Handler called every time a state is inserted in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public override void OnStateInserted(IWriteableInsertNodeOperation operation)
        {
            base.OnStateInserted(operation);

            IFrameBrowsingCollectionNodeIndex nodeIndex = ((IFrameInsertNodeOperation)operation).BrowsingIndex;

            IFrameNodeState InsertedState = ((IFrameInsertNodeOperation)operation).ChildState;
            Debug.Assert(InsertedState != null);

            // Build all cell views for the inserted node.
            IFrameNodeStateView InsertedStateView = StateViewTable[InsertedState];
            BuildCellView(InsertedStateView);

            IFrameInner<IFrameBrowsingChildIndex> ParentInner = InsertedState.ParentInner;
            Debug.Assert(ParentInner != null);

            if ((ParentInner is IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> AsBlockListInner) && (nodeIndex is IFrameBrowsingExistingBlockNodeIndex AsBlockListIndex))
                OnBlockListStateInserted(AsBlockListInner, AsBlockListIndex, InsertedState);
            else if ((ParentInner is IFrameListInner<IFrameBrowsingListNodeIndex> AsListInner) && (nodeIndex is IFrameBrowsingListNodeIndex AsListIndex))
                OnListStateInserted(AsListInner, AsListIndex, InsertedState);
            else
                throw new ArgumentOutOfRangeException(nameof(operation));

            Refresh(InsertedState);
        }

        /// <summary></summary>
        protected virtual void OnBlockListStateInserted(IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> inner, IFrameBrowsingExistingBlockNodeIndex nodeIndex, IFrameNodeState insertedState)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IFrameNodeState OwnerState = (IFrameNodeState)inner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            int BlockIndex = nodeIndex.BlockIndex;
            IFrameBlockState BlockState = inner.BlockStateList[BlockIndex];
            IFrameBlockStateView BlockStateView = BlockStateViewTable[BlockState];
            IFrameCellViewCollection EmbeddingCellView = BlockStateView.EmbeddingCellView;
            Debug.Assert(EmbeddingCellView != null);

            IFrameNodeStateView InsertedStateView = StateViewTable[insertedState];
            Debug.Assert(InsertedStateView.RootCellView != null);
            IFrameContainerCellView InsertedCellView = CreateFrameCellView(OwnerStateView, EmbeddingCellView, InsertedStateView);

            int Index = nodeIndex.Index;
            EmbeddingCellView.Insert(Index, InsertedCellView);
        }

        /// <summary></summary>
        protected virtual void OnListStateInserted(IFrameListInner<IFrameBrowsingListNodeIndex> inner, IFrameBrowsingListNodeIndex nodeIndex, IFrameNodeState insertedState)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IFrameNodeState OwnerState = inner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            IFrameAssignableCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = inner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));
            IFrameCellViewCollection EmbeddingCellView = CellViewTable[PropertyName] as IFrameCellViewCollection;
            Debug.Assert(EmbeddingCellView != null);

            IFrameNodeStateView InsertedStateView = StateViewTable[insertedState];
            Debug.Assert(InsertedStateView.RootCellView != null);
            IFrameContainerCellView InsertedCellView = CreateFrameCellView(OwnerStateView, EmbeddingCellView, InsertedStateView);

            int Index = nodeIndex.Index;
            EmbeddingCellView.Insert(Index, InsertedCellView);
        }

        /// <summary>
        /// Handler called every time a state is removed from the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public override void OnStateRemoved(IWriteableRemoveNodeOperation operation)
        {
            base.OnStateRemoved(operation);

            IFrameRemoveNodeOperation RemoveNodeOperation = (IFrameRemoveNodeOperation)operation;
            IFramePlaceholderNodeState RemovedState = RemoveNodeOperation.RemovedState;
            IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex> Inner = (IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex>)RemovedState.ParentInner;
            Debug.Assert(Inner != null);

            IFrameNodeState OwnerState = Inner.Owner;

            if (Inner is IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> AsBlockListInner)
                OnBlockListStateRemoved(AsBlockListInner, RemoveNodeOperation.BlockIndex, RemoveNodeOperation.Index, RemovedState);
            else if (Inner is IFrameListInner<IFrameBrowsingListNodeIndex> AsListInner)
                OnListStateRemoved(AsListInner, RemoveNodeOperation.Index, RemovedState);
            else
                throw new ArgumentOutOfRangeException(nameof(operation));

            Refresh(OwnerState);
        }

        /// <summary></summary>
        protected virtual void OnBlockListStateRemoved(IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> inner, int blockIndex, int index, IFrameNodeState removedState)
        {
            Debug.Assert(inner != null);

            IFrameNodeState OwnerState = inner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            IFrameBlockState BlockState = inner.BlockStateList[blockIndex];
            IFrameBlockStateView BlockStateView = BlockStateViewTable[BlockState];
            IFrameCellViewCollection EmbeddingCellView = BlockStateView.EmbeddingCellView;
            Debug.Assert(EmbeddingCellView != null);

            EmbeddingCellView.Remove(index);
        }

        /// <summary></summary>
        protected virtual void OnListStateRemoved(IFrameListInner<IFrameBrowsingListNodeIndex> inner, int index, IFrameNodeState removedState)
        {
            Debug.Assert(inner != null);

            IFrameNodeState OwnerState = inner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            IFrameAssignableCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = inner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));
            IFrameCellViewCollection EmbeddingCellView = CellViewTable[PropertyName] as IFrameCellViewCollection;
            Debug.Assert(EmbeddingCellView != null);

            EmbeddingCellView.Remove(index);
        }

        /// <summary>
        /// Handler called every time a state is inserted in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public override void OnStateReplaced(IWriteableReplaceOperation operation)
        {
            base.OnStateReplaced(operation);

            IFrameInner<IFrameBrowsingChildIndex> Inner = ((IFrameReplaceOperation)operation).Inner;
            Debug.Assert(Inner != null);
            IFrameBrowsingChildIndex NewBrowsingIndex = ((IFrameReplaceOperation)operation).NewBrowsingIndex;
            Debug.Assert(NewBrowsingIndex != null);
            IFrameNodeState ReplacedState = ((IFrameReplaceOperation)operation).NewChildState;
            Debug.Assert(ReplacedState != null);

            IFrameNodeStateView ReplacedStateView = StateViewTable[ReplacedState];
            ClearCellView(ReplacedStateView);
            BuildCellView(ReplacedStateView);

            IFrameInner<IFrameBrowsingChildIndex> ParentInner = ReplacedState.ParentInner;
            Debug.Assert(ParentInner != null);

            if ((Inner is IFramePlaceholderInner<IFrameBrowsingPlaceholderNodeIndex> AsPlaceholderInner) && (NewBrowsingIndex is IFrameBrowsingPlaceholderNodeIndex AsPlaceholderIndex))
                OnPlaceholderStateReplaced(AsPlaceholderInner, AsPlaceholderIndex, ReplacedState);
            else if ((Inner is IFrameOptionalInner<IFrameBrowsingOptionalNodeIndex> AsOptionalInner) && (NewBrowsingIndex is IFrameBrowsingOptionalNodeIndex AsOptionalIndex))
                OnOptionalStateReplaced(AsOptionalInner, AsOptionalIndex, ReplacedState);
            else if ((Inner is IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> AsBlockListInner) && (NewBrowsingIndex is IFrameBrowsingExistingBlockNodeIndex AsBlockListIndex))
                OnBlockListStateReplaced(AsBlockListInner, AsBlockListIndex, ReplacedState);
            else if ((Inner is IFrameListInner<IFrameBrowsingListNodeIndex> AsListInner) && (NewBrowsingIndex is IFrameBrowsingListNodeIndex AsListIndex))
                OnListStateReplaced(AsListInner, AsListIndex, ReplacedState);
            else
                throw new ArgumentOutOfRangeException(nameof(operation));

            Refresh(ReplacedState);
        }

        /// <summary></summary>
        protected virtual void OnPlaceholderStateReplaced(IFramePlaceholderInner<IFrameBrowsingPlaceholderNodeIndex> inner, IFrameBrowsingPlaceholderNodeIndex nodeIndex, IFrameNodeState replacedState)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IFrameNodeState OwnerState = inner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            Debug.Assert(OwnerStateView != null);

            IFrameAssignableCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = inner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));
            IFrameContainerCellView PreviousCellView = CellViewTable[PropertyName] as IFrameContainerCellView;
            Debug.Assert(PreviousCellView != null);
            IFrameCellViewCollection EmbeddingCellView = PreviousCellView.ParentCellView;

            IFrameNodeStateView ReplacedStateView = StateViewTable[replacedState];
            Debug.Assert(ReplacedStateView.RootCellView != null);
            IFrameContainerCellView ReplacedCellView = CreateFrameCellView(OwnerStateView, EmbeddingCellView, ReplacedStateView);

            Debug.Assert(PreviousCellView.IsAssignedToTable);
            ReplacedCellView.AssignToCellViewTable();
            EmbeddingCellView.Replace(PreviousCellView, ReplacedCellView);
            OwnerStateView.ReplaceCellView(PropertyName, ReplacedCellView);
        }

        /// <summary></summary>
        protected virtual void OnOptionalStateReplaced(IFrameOptionalInner<IFrameBrowsingOptionalNodeIndex> inner, IFrameBrowsingOptionalNodeIndex nodeIndex, IFrameNodeState replacedState)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IFrameNodeState OwnerState = inner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            Debug.Assert(OwnerStateView != null);

            IFrameAssignableCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = inner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));
            IFrameContainerCellView PreviousCellView = CellViewTable[PropertyName] as IFrameContainerCellView;
            Debug.Assert(PreviousCellView != null);
            IFrameCellViewCollection EmbeddingCellView = PreviousCellView.ParentCellView;

            IFrameNodeStateView ReplacedStateView = StateViewTable[replacedState];
            Debug.Assert(ReplacedStateView.RootCellView != null);
            IFrameContainerCellView ReplacedCellView = CreateFrameCellView(OwnerStateView, EmbeddingCellView, ReplacedStateView);

            Debug.Assert(PreviousCellView.IsAssignedToTable);
            ReplacedCellView.AssignToCellViewTable();
            EmbeddingCellView.Replace(PreviousCellView, ReplacedCellView);
            OwnerStateView.ReplaceCellView(PropertyName, ReplacedCellView);
        }

        /// <summary></summary>
        protected virtual void OnBlockListStateReplaced(IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> inner, IFrameBrowsingExistingBlockNodeIndex nodeIndex, IFrameNodeState replacedState)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IFrameNodeState OwnerState = inner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            int BlockIndex = nodeIndex.BlockIndex;
            IFrameBlockState BlockState = inner.BlockStateList[BlockIndex];
            IFrameBlockStateView BlockStateView = BlockStateViewTable[BlockState];
            IFrameCellViewCollection EmbeddingCellView = BlockStateView.EmbeddingCellView;
            Debug.Assert(EmbeddingCellView != null);

            IFrameNodeStateView ReplacedStateView = StateViewTable[replacedState];
            Debug.Assert(ReplacedStateView.RootCellView != null);
            IFrameContainerCellView ReplacedCellView = CreateFrameCellView(OwnerStateView, EmbeddingCellView, ReplacedStateView);

            int Index = nodeIndex.Index;
            EmbeddingCellView.Replace(Index, ReplacedCellView);
        }

        /// <summary></summary>
        protected virtual void OnListStateReplaced(IFrameListInner<IFrameBrowsingListNodeIndex> inner, IFrameBrowsingListNodeIndex nodeIndex, IFrameNodeState replacedState)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IFrameNodeState OwnerState = inner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            IFrameAssignableCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = inner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));
            IFrameCellViewCollection EmbeddingCellView = CellViewTable[PropertyName] as IFrameCellViewCollection;
            Debug.Assert(EmbeddingCellView != null);

            IFrameNodeStateView ReplacedStateView = StateViewTable[replacedState];
            Debug.Assert(ReplacedStateView.RootCellView != null);
            IFrameContainerCellView ReplacedCellView = CreateFrameCellView(OwnerStateView, EmbeddingCellView, ReplacedStateView);

            int Index = nodeIndex.Index;
            EmbeddingCellView.Replace(Index, ReplacedCellView);
        }

        /// <summary>
        /// Handler called every time a state is assigned in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public override void OnStateAssigned(IWriteableAssignmentOperation operation)
        {
            base.OnStateAssigned(operation);

            IFrameOptionalNodeState AssignedState = ((IFrameAssignmentOperation)operation).State;

            Debug.Assert(AssignedState != null);

            IFrameNodeStateView AssignedStateView = StateViewTable[AssignedState];
            ClearCellView(AssignedStateView);
            BuildCellView(AssignedStateView);

            Refresh(AssignedState);
        }

        /// <summary>
        /// Handler called every time a state is unassigned in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public override void OnStateUnassigned(IWriteableAssignmentOperation operation)
        {
            base.OnStateUnassigned(operation);

            IFrameOptionalNodeState UnassignedState = ((IFrameAssignmentOperation)operation).State;
            Debug.Assert(UnassignedState != null);

            IFrameNodeStateView UnassignedStateView = StateViewTable[UnassignedState];
            ClearCellView(UnassignedStateView);
            BuildCellView(UnassignedStateView);

            if (!operation.IsNested)
                Refresh(UnassignedState);
        }

        /// <summary>
        /// Handler called every time a state is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public override void OnStateChanged(IWriteableChangeNodeOperation operation)
        {
            base.OnStateChanged(operation);

            IFrameNodeState ChangedState = ((IFrameChangeNodeOperation)operation).State;
            Debug.Assert(ChangedState != null);

            IFrameNodeStateView ChangedStateView = StateViewTable[ChangedState];
            ClearCellView(ChangedStateView);
            BuildCellView(ChangedStateView);

            if (!operation.IsNested)
                Refresh(ChangedStateView.State);
        }

        /// <summary>
        /// Handler called every time a block state is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public override void OnBlockStateChanged(IWriteableChangeBlockOperation operation)
        {
            base.OnBlockStateChanged(operation);

            //TODO: only refresh the local state. Right now for some reason it doesn't work.
            Refresh(Controller.RootState);
        }

        /// <summary>
        /// Handler called every time a state is moved in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public override void OnStateMoved(IWriteableMoveNodeOperation operation)
        {
            base.OnStateMoved(operation);

            IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex> Inner = ((IFrameMoveNodeOperation)operation).Inner;
            IFrameBrowsingCollectionNodeIndex NodeIndex = ((IFrameMoveNodeOperation)operation).NodeIndex;
            int MoveIndex = ((IFrameMoveNodeOperation)operation).Index;
            int Direction = ((IFrameMoveNodeOperation)operation).Direction;
            IFramePlaceholderNodeState MovedState = ((IFrameMoveNodeOperation)operation).State;
            Debug.Assert(MovedState != null);
            IFrameNodeState OwnerState = Inner.Owner;

            if ((Inner is IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> AsBlockListInner) && (NodeIndex is IFrameBrowsingExistingBlockNodeIndex AsBlockListIndex))
                OnBlockListStateMoved(AsBlockListInner, AsBlockListIndex, MoveIndex, Direction);
            else if ((Inner is IFrameListInner<IFrameBrowsingListNodeIndex> AsListInner) && (NodeIndex is IFrameBrowsingListNodeIndex AsListIndex))
                OnListStateMoved(AsListInner, AsListIndex, MoveIndex, Direction);
            else
                throw new ArgumentOutOfRangeException(nameof(operation));

            Refresh(OwnerState);
        }

        /// <summary></summary>
        protected virtual void OnBlockListStateMoved(IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> inner, IFrameBrowsingExistingBlockNodeIndex nodeIndex, int index, int direction)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IFrameNodeState OwnerState = inner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            int BlockIndex = nodeIndex.BlockIndex;
            IFrameBlockState BlockState = inner.BlockStateList[BlockIndex];
            IFrameBlockStateView BlockStateView = BlockStateViewTable[BlockState];
            IFrameCellViewCollection EmbeddingCellView = BlockStateView.EmbeddingCellView;
            Debug.Assert(EmbeddingCellView != null);

            EmbeddingCellView.Move(index, direction);
        }

        /// <summary></summary>
        protected virtual void OnListStateMoved(IFrameListInner<IFrameBrowsingListNodeIndex> inner, IFrameBrowsingListNodeIndex nodeIndex, int index, int direction)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IFrameNodeState OwnerState = inner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            IFrameAssignableCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = inner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));
            IFrameCellViewCollection EmbeddingCellView = CellViewTable[PropertyName] as IFrameCellViewCollection;
            Debug.Assert(EmbeddingCellView != null);

            EmbeddingCellView.Move(index, direction);
        }

        /// <summary>
        /// Handler called every time a block state is moved in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public override void OnBlockStateMoved(IWriteableMoveBlockOperation operation)
        {
            base.OnBlockStateMoved(operation);

            IFrameMoveBlockOperation MoveBlockOperation = (IFrameMoveBlockOperation)operation;
            IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> Inner = MoveBlockOperation.BlockState.ParentInner as IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>;
            Debug.Assert(Inner != null);
            IFrameNodeState OwnerState = Inner.Owner;
            Debug.Assert(OwnerState != null);
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            IFrameAssignableCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = Inner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));
            IFrameCellViewCollection EmbeddingCellView = CellViewTable[PropertyName] as IFrameCellViewCollection;
            Debug.Assert(EmbeddingCellView != null);

            EmbeddingCellView.Move(operation.BlockIndex, operation.Direction);

            Refresh(OwnerState);
        }

        /// <summary>
        /// Handler called every time a block split in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public override void OnBlockSplit(IWriteableSplitBlockOperation operation)
        {
            base.OnBlockSplit(operation);

            IFrameSplitBlockOperation SplitBlockOperation = (IFrameSplitBlockOperation)operation;
            IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> Inner = SplitBlockOperation.BlockState.ParentInner as IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>;
            Debug.Assert(Inner != null);
            IFrameNodeState OwnerState = Inner.Owner;
            Debug.Assert(OwnerState != null);
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            int BlockIndex = ((IFrameSplitBlockOperation)operation).BlockIndex;
            int Index = ((IFrameSplitBlockOperation)operation).Index;

            IFrameBlockState FirstBlockState = Inner.BlockStateList[BlockIndex] as IFrameBlockState;
            Debug.Assert(FirstBlockState != null);

            IFrameAssignableCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = Inner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));
            IFrameCellViewCollection FirstEmbeddingCellView = CellViewTable[PropertyName] as IFrameCellViewCollection;
            Debug.Assert(FirstEmbeddingCellView != null);

            foreach (IFrameNodeState ChildState in FirstBlockState.StateList)
            {
                IFrameNodeStateView ChildStateView = StateViewTable[ChildState];
                ClearCellView(ChildStateView);
            }

            IFrameBlockStateView FirstBlockStateView = BlockStateViewTable[FirstBlockState];
            IFrameCellView RootCellView = BuildBlockCellView(OwnerStateView, FirstEmbeddingCellView, FirstBlockStateView);

            FirstEmbeddingCellView.Insert(BlockIndex, RootCellView);

            IFrameBlockState SecondBlockState = Inner.BlockStateList[BlockIndex + 1] as IFrameBlockState;
            Debug.Assert(SecondBlockState != null);

            IFrameBlockStateView SecondBlockStateView = BlockStateViewTable[SecondBlockState];
            IFrameCellViewCollection SecondEmbeddingCellView = SecondBlockStateView.EmbeddingCellView;
            Debug.Assert(SecondEmbeddingCellView != null);

            for (int i = 0; i < Index; i++)
                SecondEmbeddingCellView.Remove(0);

            Refresh(OwnerState);
        }

        /// <summary>
        /// Handler called every time two blocks are merged.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public override void OnBlocksMerged(IWriteableMergeBlocksOperation operation)
        {
            IFrameMergeBlocksOperation MergeBlocksOperation = (IFrameMergeBlocksOperation)operation;
            IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> Inner = MergeBlocksOperation.BlockState.ParentInner as IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>;

            Debug.Assert(Inner != null);
            IFrameNodeState OwnerState = Inner.Owner;
            Debug.Assert(OwnerState != null);
            int BlockIndex = ((IFrameMergeBlocksOperation)operation).BlockIndex;

            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            IFrameAssignableCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = Inner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));
            IFrameCellViewCollection BlockEmbeddingCellView = CellViewTable[PropertyName] as IFrameCellViewCollection;
            Debug.Assert(BlockEmbeddingCellView != null);

            base.OnBlocksMerged(operation);

            IFrameBlockState FirstBlockState = Inner.BlockStateList[BlockIndex - 1] as IFrameBlockState;
            Debug.Assert(FirstBlockState != null);

            foreach (IFrameNodeState ChildState in FirstBlockState.StateList)
            {
                IFrameNodeStateView ChildStateView = StateViewTable[ChildState];
                ClearCellView(ChildStateView);
            }

            IFrameNodeStateView PatternStateView = StateViewTable[FirstBlockState.PatternState];
            ClearCellView(PatternStateView);
            IFrameNodeStateView SourceStateView = StateViewTable[FirstBlockState.SourceState];
            ClearCellView(SourceStateView);

            IFrameBlockStateView FirstBlockStateView = BlockStateViewTable[FirstBlockState];
            ClearBlockCellView(OwnerStateView, FirstBlockStateView);
            IFrameCellView RootCellView = BuildBlockCellView(OwnerStateView, BlockEmbeddingCellView, FirstBlockStateView);

            BlockEmbeddingCellView.Replace(BlockIndex - 1, RootCellView);
            BlockEmbeddingCellView.Remove(BlockIndex);

            Refresh(OwnerState);
        }

        /// <summary>
        /// Handler called every time an argument block is expanded.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public override void OnArgumentExpanded(IWriteableExpandArgumentOperation operation)
        {
            base.OnArgumentExpanded(operation);

            IFrameInsertBlockOperation InsertBlockOperation = (IFrameInsertBlockOperation)operation;
            IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> ParentInner = (IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)InsertBlockOperation.BlockState.ParentInner;
            IFrameNodeState OwnerState = ParentInner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            IFrameAssignableCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = ParentInner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));
            IFrameCellViewCollection EmbeddingCellView = CellViewTable[PropertyName] as IFrameCellViewCollection;
            Debug.Assert(EmbeddingCellView != null);

            IFrameBlockStateView BlockStateView = BlockStateViewTable[((IFrameInsertBlockOperation)operation).BlockState];
            ClearBlockCellView(OwnerStateView, BlockStateView);
            IFrameCellView RootCellView = BuildBlockCellView(OwnerStateView, EmbeddingCellView, BlockStateView);

            int BlockIndex = operation.BrowsingIndex.BlockIndex;
            EmbeddingCellView.Insert(BlockIndex, RootCellView);

            //OnStateInserted(null, ArgumentNodeIndex, ArgumentChildState, true);

            Refresh(OwnerState);
        }

        /// <summary>
        /// Handler called to refresh views.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        public override void OnGenericRefresh(IWriteableGenericRefreshOperation operation)
        {
            base.OnGenericRefresh(operation);

            Refresh((IFrameNodeState)operation.RefreshState);
        }

        /// <summary>
        /// Enumerate all visible cell views.
        /// </summary>
        /// <param name="list">The list of visible cell views upon return.</param>
        public void EnumerateVisibleCellViews(IFrameVisibleCellViewList list)
        {
            Debug.Assert(list != null);
            Debug.Assert(list.Count == 0);

            IFrameNodeState RootState = Controller.RootState;
            IFrameNodeStateView RootStateView = StateViewTable[RootState];

            RootStateView.EnumerateVisibleCellViews(list);
        }
        #endregion

        #region Implementation
        /// <summary></summary>
        protected virtual IFrameCellView BuildCellView(IFrameNodeStateView stateView)
        {
            IFrameCellViewTreeContext Context = InitializedCellViewTreeContext(stateView);
            stateView.BuildRootCellView(Context);
            return stateView.RootCellView;
        }

        /// <summary></summary>
        protected virtual IFrameCellViewTreeContext InitializedCellViewTreeContext(IFrameNodeStateView stateView)
        {
            return CreateCellViewTreeContext(stateView);
        }

        /// <summary></summary>
        protected virtual void ClearCellView(IFrameNodeStateView stateView)
        {
            stateView.ClearRootCellView();
        }

        /// <summary></summary>
        protected virtual IFrameBlockCellView BuildBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameBlockStateView blockStateView)
        {
            IFrameCellViewTreeContext Context = InitializedCellViewTreeContext(stateView);
            Context.SetBlockStateView(blockStateView);
            blockStateView.BuildRootCellView(Context);

            IFrameBlockCellView BlockCellView = CreateBlockCellView(stateView, parentCellView, blockStateView);
            return BlockCellView;
        }

        /// <summary></summary>
        protected virtual void ClearBlockCellView(IFrameNodeStateView stateView, IFrameBlockStateView blockStateView)
        {
            blockStateView.ClearRootCellView(stateView);
        }

        /// <summary></summary>
        protected virtual void Refresh(IFrameNodeState state)
        {
            Debug.Assert(StateViewTable.ContainsKey(state));

            if (state.ParentState != null)
            {
                Debug.Assert(StateViewTable.ContainsKey(state.ParentState));
                IFrameNodeStateView ParentStateView = StateViewTable[state.ParentState];

                ClearCellView(ParentStateView);
                BuildCellView(ParentStateView);
            }
            else
            {
                IFrameNodeStateView StateView = StateViewTable[state];

                ClearCellView(StateView);
                BuildCellView(StateView);
            }

            UpdateLineNumbers();
        }

        /// <summary></summary>
        protected virtual void UpdateLineNumbers()
        {
            IFrameNodeState RootState = Controller.RootState;
            IFrameNodeStateView RootStateView = StateViewTable[RootState];

            int LineNumber = 1;
            int ColumnNumber = 1;
            int MaxLineNumber = 1;
            int MaxColumnNumber = 1;

            Debug.Assert(RootStateView.IsCellViewTreeValid());
            RootStateView.UpdateLineNumbers(ref LineNumber, ref MaxLineNumber, ref ColumnNumber, ref MaxColumnNumber);

            LastLineNumber = MaxLineNumber;
            LastColumnNumber = MaxColumnNumber;
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameControllerView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameControllerView AsControllerView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsControllerView))
                return comparer.Failed();

            if (TemplateSet != AsControllerView.TemplateSet)
                return comparer.Failed();

            if (LastLineNumber != AsControllerView.LastLineNumber)
                return comparer.Failed();

            if (LastColumnNumber != AsControllerView.LastColumnNumber)
                return comparer.Failed();

            return true;
        }

        /// <summary></summary>
        protected virtual void PrintCellViewTree(bool isVerbose)
        {
            IFrameNodeState RootState = Controller.RootState;
            IFrameNodeStateView RootStateView = StateViewTable[RootState];
            PrintCellViewTree(RootStateView.RootCellView, isVerbose);
        }

        /// <summary></summary>
        protected virtual void PrintCellViewTree(IFrameCellView cellView, bool isVerbose)
        {
            string Tree = cellView.PrintTree(0, isVerbose);

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
        protected virtual IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameContainerCellView(stateView, parentCellView, childStateView);
        }

        /// <summary>
        /// Creates a IxxxBlockCellView object.
        /// </summary>
        protected virtual IFrameBlockCellView CreateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameBlockStateView blockStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameBlockCellView(stateView, parentCellView, blockStateView);
        }

        /// <summary>
        /// Creates a IxxxCellViewTreeContext object.
        /// </summary>
        protected virtual IFrameCellViewTreeContext CreateCellViewTreeContext(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameCellViewTreeContext(this, stateView);
        }
        #endregion
    }
}
