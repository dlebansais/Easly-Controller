namespace EaslyController.Frame
{
    using System;
    using System.Diagnostics;
    using EaslyController.Constants;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

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
        /// State view of the root state.
        /// </summary>
        new IFrameNodeStateView RootStateView { get; }

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
        /// The display mode for comments.
        /// </summary>
        CommentDisplayModes CommentDisplayMode { get; }

        /// <summary>
        /// Enumerate all visible cell views. Enumeration is interrupted if <paramref name="handler"/> returns true.
        /// </summary>
        /// <param name="handler">A handler to execute for each cell view.</param>
        /// <param name="cellView">The cell view for which <paramref name="handler"/> returned true. Null if none.</param>
        /// <param name="reversed">If true, search in reverse order.</param>
        /// <returns>The last value returned by <paramref name="handler"/>.</returns>
        bool EnumerateVisibleCellViews(Func<IFrameVisibleCellView, bool> handler, out IFrameVisibleCellView cellView, bool reversed);

        /// <summary>
        /// Prints the cell view tree.
        /// </summary>
        /// <param name="isVerbose">Prints all information.</param>
        void PrintCellViewTree(bool isVerbose);

        /// <summary>
        /// Sets the comment display mode.
        /// </summary>
        /// <param name="mode">The new mode.</param>
        void SetCommentDisplayMode(CommentDisplayModes mode);
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
        /// Initializes a new instance of the <see cref="FrameControllerView"/> class.
        /// </summary>
        /// <param name="controller">The controller on which the view is attached.</param>
        /// <param name="templateSet">The template set used to describe the view.</param>
        private protected FrameControllerView(IFrameController controller, IFrameTemplateSet templateSet)
            : base(controller)
        {
            Debug.Assert(templateSet != null);

            TemplateSet = templateSet;
        }

        /// <summary>
        /// Initializes the view by attaching it to the controller.
        /// </summary>
        private protected override void Init()
        {
            base.Init();

            CommentDisplayMode = CommentDisplayModes.Tooltip;

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
        /// State view of the root state.
        /// </summary>
        public new IFrameNodeStateView RootStateView { get { return (IFrameNodeStateView)base.RootStateView; } }

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

        /// <summary>
        /// Gets and sets the display mode for comments.
        /// All modes are quivalent to <see cref="CommentDisplayModes.None"/> except <see cref="CommentDisplayModes.All"/>.
        /// </summary>
        public CommentDisplayModes CommentDisplayMode { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Enumerate all visible cell views. Enumeration is interrupted if <paramref name="handler"/> returns true.
        /// </summary>
        /// <param name="handler">A handler to execute for each cell view.</param>
        /// <param name="cellView">The cell view for which <paramref name="handler"/> returned true. Null if none.</param>
        /// <param name="reversed">If true, search in reverse order.</param>
        /// <returns>The last value returned by <paramref name="handler"/>.</returns>
        public bool EnumerateVisibleCellViews(Func<IFrameVisibleCellView, bool> handler, out IFrameVisibleCellView cellView, bool reversed)
        {
            Debug.Assert(handler != null);

            IFrameNodeState RootState = Controller.RootState;
            IFrameNodeStateView RootStateView = StateViewTable[RootState];

            return RootStateView.EnumerateVisibleCellViews(handler, out cellView, reversed);
        }

        /// <summary>
        /// Prints the cell view tree.
        /// </summary>
        /// <param name="isVerbose">Prints all information.</param>
        public virtual void PrintCellViewTree(bool isVerbose)
        {
            IFrameNodeState RootState = Controller.RootState;
            IFrameNodeStateView RootStateView = StateViewTable[RootState];
            PrintCellViewTree(RootStateView.RootCellView, isVerbose);
        }

        /// <summary>
        /// Sets the comment display mode.
        /// </summary>
        /// <param name="mode">The new mode.</param>
        public virtual void SetCommentDisplayMode(CommentDisplayModes mode)
        {
            if (CommentDisplayMode != mode)
            {
                CommentDisplayMode = mode;
                Refresh(Controller.RootState);
            }
        }
        #endregion

        #region Implementation
        /// <summary>
        /// Handler called every time a block state is inserted in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockStateInserted(IWriteableInsertBlockOperation operation)
        {
            base.OnBlockStateInserted(operation);

            IFrameBlockState BlockState = ((IFrameInsertBlockOperation)operation).BlockState;

            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));

            Debug.Assert(StateViewTable.ContainsKey(BlockState.PatternState));
            Debug.Assert(StateViewTable.ContainsKey(BlockState.SourceState));

            Debug.Assert(BlockState.StateList.Count == 1);

            IFramePlaceholderNodeState ChildState = ((IFrameInsertBlockOperation)operation).ChildState;
            Debug.Assert(ChildState == BlockState.StateList[0]);
            Debug.Assert(ChildState.ParentIndex == ((IFrameInsertBlockOperation)operation).BrowsingIndex);
            Debug.Assert(StateViewTable.ContainsKey(ChildState));

            IFrameInsertBlockOperation InsertBlockOperation = (IFrameInsertBlockOperation)operation;
            IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> ParentInner = (IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)InsertBlockOperation.BlockState.ParentInner;
            IFrameNodeState OwnerState = ParentInner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            IFrameAssignableCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = ParentInner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));
            IFrameCellViewCollection EmbeddingCellView = CellViewTable[PropertyName] as IFrameCellViewCollection;

            // If the embedding cell view is available (it may not be the case for the first inserted element).
            if (EmbeddingCellView != null)
            {
                // Build all cell views for the inserted block.
                IFrameBlockStateView BlockStateView = BlockStateViewTable[((IFrameInsertBlockOperation)operation).BlockState];
                IFrameBlockCellView RootCellView = BuildBlockCellView(OwnerStateView, EmbeddingCellView, BlockStateView);

                // Insert the root cell view in the collection embedding other blocks.
                int BlockIndex = operation.BrowsingIndex.BlockIndex;
                EmbeddingCellView.Insert(BlockIndex, RootCellView);
            }

            Refresh(OwnerState);
        }

        /// <summary>
        /// Handler called every time a block state is removed from the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockStateRemoved(IWriteableRemoveBlockOperation operation)
        {
            base.OnBlockStateRemoved(operation);

            IFrameBlockState BlockState = ((IFrameRemoveBlockOperation)operation).BlockState;

            Debug.Assert(BlockState != null);
            Debug.Assert(!BlockStateViewTable.ContainsKey(BlockState));

            Debug.Assert(!StateViewTable.ContainsKey(BlockState.PatternState));
            Debug.Assert(!StateViewTable.ContainsKey(BlockState.SourceState));

            IFrameNodeState RemovedState = ((IFrameRemoveBlockOperation)operation).RemovedState;
            Debug.Assert(!StateViewTable.ContainsKey(RemovedState));

            Debug.Assert(BlockState.StateList.Count == 0);

            IFrameRemoveBlockOperation RemoveBlockOperation = (IFrameRemoveBlockOperation)operation;
            IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> ParentInner = (IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>)RemoveBlockOperation.BlockState.ParentInner;
            IFrameNodeState OwnerState = ParentInner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            IFrameAssignableCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = ParentInner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));

            IFrameCellViewCollection EmbeddingCellView = CellViewTable[PropertyName] as IFrameCellViewCollection;
            Debug.Assert(EmbeddingCellView != null);

            int BlockIndex = operation.BlockIndex;
            EmbeddingCellView.Remove(BlockIndex);

            if (!operation.IsNested)
                Refresh(OwnerState);
        }

        /// <summary>
        /// Handler called every time a block view must be removed from the controller view.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockViewRemoved(IWriteableRemoveBlockViewOperation operation)
        {
            base.OnBlockViewRemoved(operation);

            IFrameBlockState BlockState = ((IFrameRemoveBlockViewOperation)operation).BlockState;

            Debug.Assert(BlockState != null);
            Debug.Assert(!BlockStateViewTable.ContainsKey(BlockState));

            Debug.Assert(!StateViewTable.ContainsKey(BlockState.PatternState));
            Debug.Assert(!StateViewTable.ContainsKey(BlockState.SourceState));

            foreach (IFrameNodeState State in BlockState.StateList)
                Debug.Assert(!StateViewTable.ContainsKey(State));

            IFrameRemoveBlockViewOperation RemoveBlockViewOperation = (IFrameRemoveBlockViewOperation)operation;
            IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> ParentInner = RemoveBlockViewOperation.BlockState.ParentInner as IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>;
            Debug.Assert(ParentInner != null);
            IFrameNodeState OwnerState = ParentInner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            IFrameAssignableCellViewReadOnlyDictionary<string> CellViewTable = OwnerStateView.CellViewTable;
            string PropertyName = ParentInner.PropertyName;

            Debug.Assert(CellViewTable != null);
            Debug.Assert(CellViewTable.ContainsKey(PropertyName));

            IFrameCellViewCollection EmbeddingCellView = CellViewTable[PropertyName] as IFrameCellViewCollection;
            Debug.Assert(EmbeddingCellView != null);

            int BlockIndex = operation.BlockIndex;
            EmbeddingCellView.Remove(BlockIndex);

            Debug.Assert(operation.IsNested);
        }

        /// <summary>
        /// Handler called every time a state is inserted in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateInserted(IWriteableInsertNodeOperation operation)
        {
            base.OnStateInserted(operation);

            IFrameNodeState ChildState = ((IFrameInsertNodeOperation)operation).ChildState;
            Debug.Assert(ChildState != null);
            Debug.Assert(StateViewTable.ContainsKey(ChildState));

            IFrameBrowsingCollectionNodeIndex BrowsingIndex = ((IFrameInsertNodeOperation)operation).BrowsingIndex;
            Debug.Assert(ChildState.ParentIndex == BrowsingIndex);

            IFrameNodeState InsertedState = ((IFrameInsertNodeOperation)operation).ChildState;
            Debug.Assert(InsertedState != null);

            // Build all cell views for the inserted node.
            IFrameNodeStateView InsertedStateView = StateViewTable[InsertedState];
            BuildCellView(InsertedStateView);

            IFrameInner ParentInner = InsertedState.ParentInner;
            Debug.Assert(ParentInner != null);

            bool IsHandled = false;

            if (ParentInner is IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> AsBlockListInner && BrowsingIndex is IFrameBrowsingExistingBlockNodeIndex AsBlockListIndex)
            {
                OnBlockListStateInserted(AsBlockListInner, AsBlockListIndex, InsertedState);
                IsHandled = true;
            }
            else if (ParentInner is IFrameListInner<IFrameBrowsingListNodeIndex> AsListInner && BrowsingIndex is IFrameBrowsingListNodeIndex AsListIndex)
            {
                OnListStateInserted(AsListInner, AsListIndex, InsertedState);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);

            Refresh(InsertedState);
        }

        /// <summary></summary>
        private protected virtual void OnBlockListStateInserted(IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> inner, IFrameBrowsingExistingBlockNodeIndex nodeIndex, IFrameNodeState insertedState)
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

            IFrameFrame AssociatedFrame = GetAssociatedFrame(inner);
            Debug.Assert(AssociatedFrame != null);

            IFrameNodeStateView InsertedStateView = StateViewTable[insertedState];
            Debug.Assert(InsertedStateView.RootCellView != null);
            IFrameContainerCellView InsertedCellView = CreateFrameCellView(OwnerStateView, EmbeddingCellView, InsertedStateView, AssociatedFrame);
            ValidateContainerCellView(OwnerStateView, EmbeddingCellView, InsertedStateView, InsertedCellView);

            int Index = nodeIndex.Index;
            EmbeddingCellView.Insert(Index, InsertedCellView);

            foreach (IFrameCellView CellView in EmbeddingCellView.CellViewList)
            {
                IFrameContainerCellView AsContainerCellView = CellView as IFrameContainerCellView;
                Debug.Assert(AsContainerCellView != null);
                Debug.Assert(AsContainerCellView.Frame == AssociatedFrame);
            }
        }

        /// <summary></summary>
        private protected virtual void OnListStateInserted(IFrameListInner<IFrameBrowsingListNodeIndex> inner, IFrameBrowsingListNodeIndex nodeIndex, IFrameNodeState insertedState)
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

            IFrameFrame AssociatedFrame = GetAssociatedFrame(inner);
            Debug.Assert(AssociatedFrame != null);

            IFrameNodeStateView InsertedStateView = StateViewTable[insertedState];
            Debug.Assert(InsertedStateView.RootCellView != null);
            IFrameContainerCellView InsertedCellView = CreateFrameCellView(OwnerStateView, EmbeddingCellView, InsertedStateView, AssociatedFrame);
            ValidateContainerCellView(OwnerStateView, EmbeddingCellView, InsertedStateView, InsertedCellView);

            int Index = nodeIndex.Index;
            EmbeddingCellView.Insert(Index, InsertedCellView);

            foreach (IFrameCellView CellView in EmbeddingCellView.CellViewList)
            {
                IFrameContainerCellView AsContainerCellView = CellView as IFrameContainerCellView;
                Debug.Assert(AsContainerCellView != null);
                Debug.Assert(AsContainerCellView.Frame == AssociatedFrame);
            }
        }

        /// <summary>
        /// Handler called every time a state is removed from the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateRemoved(IWriteableRemoveNodeOperation operation)
        {
            base.OnStateRemoved(operation);

            IFramePlaceholderNodeState RemovedState = ((IFrameRemoveNodeOperation)operation).RemovedState;
            Debug.Assert(RemovedState != null);
            Debug.Assert(!StateViewTable.ContainsKey(RemovedState));

            IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex> Inner = (IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex>)RemovedState.ParentInner;
            Debug.Assert(Inner != null);

            IFrameNodeState OwnerState = Inner.Owner;
            bool IsHandled = false;

            if (Inner is IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> AsBlockListInner)
            {
                OnBlockListStateRemoved(AsBlockListInner, ((IFrameRemoveNodeOperation)operation).BlockIndex, ((IFrameRemoveNodeOperation)operation).Index, RemovedState);
                IsHandled = true;
            }
            else if (Inner is IFrameListInner<IFrameBrowsingListNodeIndex> AsListInner)
            {
                OnListStateRemoved(AsListInner, ((IFrameRemoveNodeOperation)operation).Index, RemovedState);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);

            Refresh(OwnerState);
        }

        /// <summary></summary>
        private protected virtual void OnBlockListStateRemoved(IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> inner, int blockIndex, int index, IFrameNodeState removedState)
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
        private protected virtual void OnListStateRemoved(IFrameListInner<IFrameBrowsingListNodeIndex> inner, int index, IFrameNodeState removedState)
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
        private protected override void OnStateReplaced(IWriteableReplaceOperation operation)
        {
            base.OnStateReplaced(operation);

            IFrameNodeState NewChildState = ((IFrameReplaceOperation)operation).NewChildState;
            Debug.Assert(NewChildState != null);
            Debug.Assert(StateViewTable.ContainsKey(NewChildState));

            IFrameBrowsingChildIndex OldBrowsingIndex = ((IFrameReplaceOperation)operation).OldBrowsingIndex;
            Debug.Assert(OldBrowsingIndex != null);
            Debug.Assert(NewChildState.ParentIndex != OldBrowsingIndex);

            IFrameBrowsingChildIndex NewBrowsingIndex = ((IFrameReplaceOperation)operation).NewBrowsingIndex;
            Debug.Assert(NewBrowsingIndex != null);
            Debug.Assert(NewChildState.ParentIndex == NewBrowsingIndex);

            IFrameInner Inner = NewChildState.ParentInner;
            Debug.Assert(Inner != null);
            Debug.Assert(NewBrowsingIndex != null);
            IFrameNodeState ReplacedState = ((IFrameReplaceOperation)operation).NewChildState;
            Debug.Assert(ReplacedState != null);

            IFrameNodeStateView ReplacedStateView = StateViewTable[ReplacedState];
            ClearCellView(ReplacedStateView);
            BuildCellView(ReplacedStateView);

            IFrameInner ParentInner = ReplacedState.ParentInner;
            Debug.Assert(ParentInner != null);

            bool IsHandled = false;

            if (Inner is IFramePlaceholderInner<IFrameBrowsingPlaceholderNodeIndex> AsPlaceholderInner && NewBrowsingIndex is IFrameBrowsingPlaceholderNodeIndex AsPlaceholderIndex)
            {
                OnPlaceholderStateReplaced(AsPlaceholderInner, AsPlaceholderIndex, ReplacedState);
                IsHandled = true;
            }
            else if (Inner is IFrameOptionalInner<IFrameBrowsingOptionalNodeIndex> AsOptionalInner && NewBrowsingIndex is IFrameBrowsingOptionalNodeIndex AsOptionalIndex)
            {
                OnOptionalStateReplaced(AsOptionalInner, AsOptionalIndex, ReplacedState);
                IsHandled = true;
            }
            else if (Inner is IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> AsBlockListInner && NewBrowsingIndex is IFrameBrowsingExistingBlockNodeIndex AsBlockListIndex)
            {
                OnBlockListStateReplaced(AsBlockListInner, AsBlockListIndex, ReplacedState);
                IsHandled = true;
            }
            else if (Inner is IFrameListInner<IFrameBrowsingListNodeIndex> AsListInner && NewBrowsingIndex is IFrameBrowsingListNodeIndex AsListIndex)
            {
                OnListStateReplaced(AsListInner, AsListIndex, ReplacedState);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);

            Refresh(ReplacedState);
        }

        /// <summary></summary>
        private protected virtual void OnPlaceholderStateReplaced(IFramePlaceholderInner<IFrameBrowsingPlaceholderNodeIndex> inner, IFrameBrowsingPlaceholderNodeIndex nodeIndex, IFrameNodeState replacedState)
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

            IFrameFrame AssociatedFrame = GetAssociatedFrame(inner);
            Debug.Assert(AssociatedFrame != null);
            Debug.Assert(AssociatedFrame == PreviousCellView.Frame);

            IFrameNodeStateView ReplacedStateView = StateViewTable[replacedState];
            Debug.Assert(ReplacedStateView.RootCellView != null);
            IFrameContainerCellView ReplacedCellView = CreateFrameCellView(OwnerStateView, EmbeddingCellView, ReplacedStateView, AssociatedFrame);
            ValidateContainerCellView(OwnerStateView, EmbeddingCellView, ReplacedStateView, ReplacedCellView);

            Debug.Assert(PreviousCellView.IsAssignedToTable);
            ReplacedCellView.AssignToCellViewTable();
            EmbeddingCellView.Replace(PreviousCellView, ReplacedCellView);
            ((IFrameReplaceableStateView)OwnerStateView).ReplaceCellView(PropertyName, ReplacedCellView);
        }

        /// <summary></summary>
        private protected virtual void OnOptionalStateReplaced(IFrameOptionalInner<IFrameBrowsingOptionalNodeIndex> inner, IFrameBrowsingOptionalNodeIndex nodeIndex, IFrameNodeState replacedState)
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

            IFrameFrame AssociatedFrame = GetAssociatedFrame(inner);
            Debug.Assert(AssociatedFrame != null);
            Debug.Assert(AssociatedFrame == PreviousCellView.Frame);

            IFrameNodeStateView ReplacedStateView = StateViewTable[replacedState];
            Debug.Assert(ReplacedStateView.RootCellView != null);
            IFrameContainerCellView ReplacedCellView = CreateFrameCellView(OwnerStateView, EmbeddingCellView, ReplacedStateView, AssociatedFrame);
            ValidateContainerCellView(OwnerStateView, EmbeddingCellView, ReplacedStateView, ReplacedCellView);

            Debug.Assert(PreviousCellView.IsAssignedToTable);
            ReplacedCellView.AssignToCellViewTable();
            EmbeddingCellView.Replace(PreviousCellView, ReplacedCellView);
            ((IFrameReplaceableStateView)OwnerStateView).ReplaceCellView(PropertyName, ReplacedCellView);
        }

        /// <summary></summary>
        private protected virtual void OnBlockListStateReplaced(IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> inner, IFrameBrowsingExistingBlockNodeIndex nodeIndex, IFrameNodeState replacedState)
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

            IFrameFrame AssociatedFrame = GetAssociatedFrame(inner);
            Debug.Assert(AssociatedFrame != null);

            IFrameNodeStateView ReplacedStateView = StateViewTable[replacedState];
            Debug.Assert(ReplacedStateView.RootCellView != null);
            IFrameContainerCellView ReplacedCellView = CreateFrameCellView(OwnerStateView, EmbeddingCellView, ReplacedStateView, AssociatedFrame);
            ValidateContainerCellView(OwnerStateView, EmbeddingCellView, ReplacedStateView, ReplacedCellView);

            int Index = nodeIndex.Index;

            IFrameContainerCellView PreviousCellView = EmbeddingCellView.CellViewList[Index] as IFrameContainerCellView;
            Debug.Assert(PreviousCellView != null);
            Debug.Assert(AssociatedFrame == PreviousCellView.Frame);

            EmbeddingCellView.Replace(Index, ReplacedCellView);
        }

        /// <summary></summary>
        private protected virtual void OnListStateReplaced(IFrameListInner<IFrameBrowsingListNodeIndex> inner, IFrameBrowsingListNodeIndex nodeIndex, IFrameNodeState replacedState)
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

            IFrameFrame AssociatedFrame = GetAssociatedFrame(inner);
            Debug.Assert(AssociatedFrame != null);

            IFrameNodeStateView ReplacedStateView = StateViewTable[replacedState];
            Debug.Assert(ReplacedStateView.RootCellView != null);
            IFrameContainerCellView ReplacedCellView = CreateFrameCellView(OwnerStateView, EmbeddingCellView, ReplacedStateView, AssociatedFrame);
            ValidateContainerCellView(OwnerStateView, EmbeddingCellView, ReplacedStateView, ReplacedCellView);

            int Index = nodeIndex.Index;

            IFrameContainerCellView PreviousCellView = EmbeddingCellView.CellViewList[Index] as IFrameContainerCellView;
            Debug.Assert(PreviousCellView != null);
            Debug.Assert(AssociatedFrame == PreviousCellView.Frame);

            EmbeddingCellView.Replace(Index, ReplacedCellView);
        }

        /// <summary>
        /// Handler called every time a state is assigned in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateAssigned(IWriteableAssignmentOperation operation)
        {
            base.OnStateAssigned(operation);

            IFrameOptionalNodeState State = ((IFrameAssignmentOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));

            IFrameNodeStateView AssignedStateView = StateViewTable[State];
            ClearCellView(AssignedStateView);
            BuildCellView(AssignedStateView);

            Refresh(State);
        }

        /// <summary>
        /// Handler called every time a state is unassigned in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateUnassigned(IWriteableAssignmentOperation operation)
        {
            base.OnStateUnassigned(operation);

            IFrameOptionalNodeState State = ((IFrameAssignmentOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));

            IFrameNodeStateView UnassignedStateView = StateViewTable[State];
            ClearCellView(UnassignedStateView);
            BuildCellView(UnassignedStateView);

            if (!operation.IsNested)
                Refresh(State);
        }

        /// <summary>
        /// Handler called every time a discrete value is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnDiscreteValueChanged(IWriteableChangeDiscreteValueOperation operation)
        {
            base.OnDiscreteValueChanged(operation);

            IFrameNodeState State = ((IFrameChangeDiscreteValueOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));

            IFrameNodeStateView ChangedStateView = StateViewTable[State];
            ClearCellView(ChangedStateView);
            BuildCellView(ChangedStateView);

            if (!operation.IsNested)
                Refresh(State);
        }

        /// <summary>
        /// Handler called every time a text is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnTextChanged(IWriteableChangeTextOperation operation)
        {
            base.OnTextChanged(operation);

            IFrameNodeState State = ((IFrameChangeTextOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));

            IFrameNodeStateView ChangedStateView = StateViewTable[State];
            ClearCellView(ChangedStateView);
            BuildCellView(ChangedStateView);

            if (!operation.IsNested)
                Refresh(State);
        }

        /// <summary>
        /// Handler called every time a comment is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnCommentChanged(IWriteableChangeCommentOperation operation)
        {
            base.OnCommentChanged(operation);

            IFrameNodeState State = ((IFrameChangeCommentOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));

            IFrameNodeStateView ChangedStateView = StateViewTable[State];
            ClearCellView(ChangedStateView);
            BuildCellView(ChangedStateView);

            if (!operation.IsNested)
                Refresh(State);
        }

        /// <summary>
        /// Handler called every time a block state is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockStateChanged(IWriteableChangeBlockOperation operation)
        {
            base.OnBlockStateChanged(operation);

            IFrameBlockState BlockState = ((IFrameChangeBlockOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));

            // TODO: only refresh the local state. Right now for some reason it doesn't work.
            Refresh(Controller.RootState);
        }

        /// <summary>
        /// Handler called every time a state is moved in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateMoved(IWriteableMoveNodeOperation operation)
        {
            base.OnStateMoved(operation);

            IFramePlaceholderNodeState State = ((IFrameMoveNodeOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));

            IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex> Inner = State.ParentInner as IFrameCollectionInner<IFrameBrowsingCollectionNodeIndex>;
            int BlockIndex = ((IFrameMoveNodeOperation)operation).BlockIndex;
            int MoveIndex = ((IFrameMoveNodeOperation)operation).Index;
            int Direction = ((IFrameMoveNodeOperation)operation).Direction;
            Debug.Assert(State != null);
            IFrameNodeState OwnerState = Inner.Owner;

            bool IsHandled = false;

            if (Inner is IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> AsBlockListInner)
            {
                OnBlockListStateMoved(AsBlockListInner, BlockIndex, MoveIndex, Direction);
                IsHandled = true;
            }
            else if (Inner is IFrameListInner<IFrameBrowsingListNodeIndex> AsListInner)
            {
                OnListStateMoved(AsListInner, MoveIndex, Direction);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);

            Refresh(OwnerState);
        }

        /// <summary></summary>
        private protected virtual void OnBlockListStateMoved(IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> inner, int blockIndex, int index, int direction)
        {
            Debug.Assert(inner != null);

            IFrameNodeState OwnerState = inner.Owner;
            IFrameNodeStateView OwnerStateView = StateViewTable[OwnerState];

            IFrameBlockState BlockState = inner.BlockStateList[blockIndex];
            IFrameBlockStateView BlockStateView = BlockStateViewTable[BlockState];
            IFrameCellViewCollection EmbeddingCellView = BlockStateView.EmbeddingCellView;
            Debug.Assert(EmbeddingCellView != null);

            EmbeddingCellView.Move(index, direction);
        }

        /// <summary></summary>
        private protected virtual void OnListStateMoved(IFrameListInner<IFrameBrowsingListNodeIndex> inner, int index, int direction)
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

            EmbeddingCellView.Move(index, direction);
        }

        /// <summary>
        /// Handler called every time a block state is moved in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockStateMoved(IWriteableMoveBlockOperation operation)
        {
            base.OnBlockStateMoved(operation);

            IFrameBlockState BlockState = ((IFrameMoveBlockOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));

            IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> Inner = BlockState.ParentInner as IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>;
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

            EmbeddingCellView.Move(((IFrameMoveBlockOperation)operation).BlockIndex, ((IFrameMoveBlockOperation)operation).Direction);

            Refresh(OwnerState);
        }

        /// <summary>
        /// Handler called every time a block split in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockSplit(IWriteableSplitBlockOperation operation)
        {
            base.OnBlockSplit(operation);

            IFrameBlockState BlockState = ((IFrameSplitBlockOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));

            IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> Inner = BlockState.ParentInner as IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>;
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
        private protected override void OnBlocksMerged(IWriteableMergeBlocksOperation operation)
        {
            base.OnBlocksMerged(operation);

            IFrameBlockState BlockState = ((IFrameMergeBlocksOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(!BlockStateViewTable.ContainsKey(BlockState));

            IFrameBlockListInner<IFrameBrowsingBlockNodeIndex> Inner = BlockState.ParentInner as IFrameBlockListInner<IFrameBrowsingBlockNodeIndex>;

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
        /// Handler called to refresh views.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnGenericRefresh(IWriteableGenericRefreshOperation operation)
        {
            base.OnGenericRefresh(operation);

            IFrameNodeState RefreshState = ((IFrameGenericRefreshOperation)operation).RefreshState;
            Debug.Assert(RefreshState != null);
            Debug.Assert(StateViewTable.ContainsKey(RefreshState));

            Refresh((IFrameNodeState)operation.RefreshState);
        }

        /// <summary></summary>
        private protected virtual IFrameCellView BuildCellView(IFrameNodeStateView stateView)
        {
            IFrameCellViewTreeContext Context = InitializedCellViewTreeContext(stateView);
            stateView.BuildRootCellView(Context);
            stateView.SetContainerCellView(null);
            CloseCellViewTreeContext(Context);

            return stateView.RootCellView;
        }

        /// <summary></summary>
        private protected virtual IFrameCellViewTreeContext InitializedCellViewTreeContext(IFrameNodeStateView stateView)
        {
            return CreateCellViewTreeContext(stateView);
        }

        /// <summary></summary>
        private protected virtual void CloseCellViewTreeContext(IFrameCellViewTreeContext context)
        {
        }

        /// <summary></summary>
        private protected virtual void ClearCellView(IFrameNodeStateView stateView)
        {
            stateView.ClearRootCellView();
        }

        /// <summary></summary>
        private protected virtual IFrameBlockCellView BuildBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameBlockStateView blockStateView)
        {
            IFrameCellViewTreeContext Context = InitializedCellViewTreeContext(stateView);
            Context.SetBlockStateView(blockStateView);
            blockStateView.BuildRootCellView(Context);

            IFrameBlockCellView BlockCellView = CreateBlockCellView(stateView, parentCellView, blockStateView);
            ValidateBlockCellView(stateView, parentCellView, BlockCellView);

            return BlockCellView;
        }

        /// <summary></summary>
        private protected virtual void ValidateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameBlockCellView blockCellView)
        {
            Debug.Assert(blockCellView.StateView == stateView);
            Debug.Assert(blockCellView.ParentCellView == parentCellView);
        }

        /// <summary></summary>
        private protected virtual void ClearBlockCellView(IFrameNodeStateView stateView, IFrameBlockStateView blockStateView)
        {
            blockStateView.ClearRootCellView(stateView);
        }

        /// <summary></summary>
        private protected virtual void Refresh(IFrameNodeState state)
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
        private protected virtual void UpdateLineNumbers()
        {
            IFrameNodeState RootState = Controller.RootState;
            IFrameNodeStateView RootStateView = StateViewTable[RootState];

            int LineNumber = 1;
            int ColumnNumber = 1;
            int MaxLineNumber = 1;
            int MaxColumnNumber = 1;

            Debug.Assert(RootStateView.IsCellViewTreeValid());
            Debug.Assert(RootStateView.HasVisibleCellView);

            RootStateView.UpdateLineNumbers(ref LineNumber, ref MaxLineNumber, ref ColumnNumber, ref MaxColumnNumber);

            LastLineNumber = MaxLineNumber;
            LastColumnNumber = MaxColumnNumber;
        }

        /// <summary></summary>
        private protected virtual IFrameFrame GetAssociatedFrame(IFrameInner<IFrameBrowsingChildIndex> inner)
        {
            IFrameFrame AssociatedFrame = TemplateSet.InnerToFrame(inner);

            return AssociatedFrame;
        }

        /// <summary></summary>
        private protected virtual void ValidateContainerCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView, IFrameContainerCellView containerCellView)
        {
            Debug.Assert(containerCellView.StateView == stateView);
            Debug.Assert(containerCellView.ParentCellView == parentCellView);
            Debug.Assert(containerCellView.ChildStateView == childStateView);
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

            if (!comparer.IsSameType(other, out FrameControllerView AsControllerView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsControllerView))
                return comparer.Failed();

            if (!comparer.IsSameReference(TemplateSet, AsControllerView.TemplateSet))
                return comparer.Failed();

            if (!comparer.IsSameInteger(LastLineNumber, AsControllerView.LastLineNumber))
                return comparer.Failed();

            if (!comparer.IsSameInteger(LastColumnNumber, AsControllerView.LastColumnNumber))
                return comparer.Failed();

            return true;
        }

        /// <summary></summary>
        private protected virtual void PrintCellViewTree(IFrameCellView cellView, bool isVerbose)
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
        private protected override IReadOnlyStateViewDictionary CreateStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxBlockStateViewDictionary object.
        /// </summary>
        private protected override IReadOnlyBlockStateViewDictionary CreateBlockStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameBlockStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxAttachCallbackSet object.
        /// </summary>
        private protected override IReadOnlyAttachCallbackSet CreateCallbackSet()
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
        private protected override IReadOnlyPlaceholderNodeStateView CreatePlaceholderNodeStateView(IReadOnlyPlaceholderNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FramePlaceholderNodeStateView(this, (IFramePlaceholderNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxOptionalNodeStateView object.
        /// </summary>
        private protected override IReadOnlyOptionalNodeStateView CreateOptionalNodeStateView(IReadOnlyOptionalNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameOptionalNodeStateView(this, (IFrameOptionalNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxPatternStateView object.
        /// </summary>
        private protected override IReadOnlyPatternStateView CreatePatternStateView(IReadOnlyPatternState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FramePatternStateView(this, (IFramePatternState)state);
        }

        /// <summary>
        /// Creates a IxxxSourceStateView object.
        /// </summary>
        private protected override IReadOnlySourceStateView CreateSourceStateView(IReadOnlySourceState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameSourceStateView(this, (IFrameSourceState)state);
        }

        /// <summary>
        /// Creates a IxxxBlockStateView object.
        /// </summary>
        private protected override IReadOnlyBlockStateView CreateBlockStateView(IReadOnlyBlockState blockState)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameBlockStateView(this, (IFrameBlockState)blockState);
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        private protected virtual IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView, IFrameFrame frame)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameContainerCellView(stateView, parentCellView, childStateView, frame);
        }

        /// <summary>
        /// Creates a IxxxBlockCellView object.
        /// </summary>
        private protected virtual IFrameBlockCellView CreateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameBlockStateView blockStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameBlockCellView(stateView, parentCellView, blockStateView);
        }

        /// <summary>
        /// Creates a IxxxCellViewTreeContext object.
        /// </summary>
        private protected virtual IFrameCellViewTreeContext CreateCellViewTreeContext(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameControllerView));
            return new FrameCellViewTreeContext(this, stateView);
        }
        #endregion
    }
}
