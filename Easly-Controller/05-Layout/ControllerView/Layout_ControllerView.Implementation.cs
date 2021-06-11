namespace EaslyController.Layout
{
    using System;
    using System.Diagnostics;
    using EaslyController.Constants;
    using EaslyController.Controller;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public partial class LayoutControllerView : FocusControllerView, ILayoutControllerView, ILayoutInternalControllerView
    {
        /// <summary>
        /// Handler called every time a block state is inserted in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockStateInserted(IWriteableInsertBlockOperation operation)
        {
            base.OnBlockStateInserted(operation);

            ILayoutBlockState BlockState = ((ILayoutInsertBlockOperation)operation).BlockState;

            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));

            Debug.Assert(StateViewTable.ContainsKey(BlockState.PatternState));
            Debug.Assert(StateViewTable.ContainsKey(BlockState.SourceState));

            Debug.Assert(BlockState.StateList.Count == 1);

            ILayoutPlaceholderNodeState ChildState = ((ILayoutInsertBlockOperation)operation).ChildState;
            Debug.Assert(ChildState == BlockState.StateList[0]);
            Debug.Assert(ChildState.ParentIndex == ((ILayoutInsertBlockOperation)operation).BrowsingIndex);
            Debug.Assert(StateViewTable.ContainsKey(ChildState));
        }

        /// <summary>
        /// Handler called every time a block state is removed from the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockStateRemoved(IWriteableRemoveBlockOperation operation)
        {
            base.OnBlockStateRemoved(operation);

            ILayoutBlockState BlockState = ((ILayoutRemoveBlockOperation)operation).BlockState;

            Debug.Assert(BlockState != null);
            Debug.Assert(!BlockStateViewTable.ContainsKey(BlockState));

            Debug.Assert(!StateViewTable.ContainsKey(BlockState.PatternState));
            Debug.Assert(!StateViewTable.ContainsKey(BlockState.SourceState));

            ILayoutNodeState RemovedState = ((ILayoutRemoveBlockOperation)operation).RemovedState;
            Debug.Assert(!StateViewTable.ContainsKey(RemovedState));

            Debug.Assert(BlockState.StateList.Count == 0);
        }

        /// <summary>
        /// Handler called every time a block view must be removed from the controller view.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockViewRemoved(IWriteableRemoveBlockViewOperation operation)
        {
            base.OnBlockViewRemoved(operation);

            ILayoutBlockState BlockState = ((ILayoutRemoveBlockViewOperation)operation).BlockState;

            Debug.Assert(BlockState != null);
            Debug.Assert(!BlockStateViewTable.ContainsKey(BlockState));

            Debug.Assert(!StateViewTable.ContainsKey(BlockState.PatternState));
            Debug.Assert(!StateViewTable.ContainsKey(BlockState.SourceState));

            foreach (ILayoutNodeState State in BlockState.StateList)
                Debug.Assert(!StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a state is inserted in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateInserted(IWriteableInsertNodeOperation operation)
        {
            base.OnStateInserted(operation);

            ILayoutNodeState ChildState = ((ILayoutInsertNodeOperation)operation).ChildState;
            Debug.Assert(ChildState != null);
            Debug.Assert(StateViewTable.ContainsKey(ChildState));

            ILayoutBrowsingCollectionNodeIndex BrowsingIndex = ((ILayoutInsertNodeOperation)operation).BrowsingIndex;
            Debug.Assert(ChildState.ParentIndex == BrowsingIndex);
        }

        /// <summary>
        /// Handler called every time a state is removed from the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateRemoved(IWriteableRemoveNodeOperation operation)
        {
            base.OnStateRemoved(operation);

            ILayoutPlaceholderNodeState RemovedState = ((ILayoutRemoveNodeOperation)operation).RemovedState;
            Debug.Assert(RemovedState != null);
            Debug.Assert(!StateViewTable.ContainsKey(RemovedState));
        }

        /// <summary>
        /// Handler called every time a state is inserted in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateReplaced(IWriteableReplaceOperation operation)
        {
            base.OnStateReplaced(operation);

            ILayoutNodeState NewChildState = ((ILayoutReplaceOperation)operation).NewChildState;
            Debug.Assert(NewChildState != null);
            Debug.Assert(StateViewTable.ContainsKey(NewChildState));

            ILayoutBrowsingChildIndex OldBrowsingIndex = ((ILayoutReplaceOperation)operation).OldBrowsingIndex;
            Debug.Assert(OldBrowsingIndex != null);
            Debug.Assert(NewChildState.ParentIndex != OldBrowsingIndex);

            ILayoutBrowsingChildIndex NewBrowsingIndex = ((ILayoutReplaceOperation)operation).NewBrowsingIndex;
            Debug.Assert(NewBrowsingIndex != null);
            Debug.Assert(NewChildState.ParentIndex == NewBrowsingIndex);
        }

        /// <summary>
        /// Handler called every time a state is assigned in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateAssigned(IWriteableAssignmentOperation operation)
        {
            base.OnStateAssigned(operation);

            ILayoutOptionalNodeState State = ((ILayoutAssignmentOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a state is unassigned in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateUnassigned(IWriteableAssignmentOperation operation)
        {
            base.OnStateUnassigned(operation);

            ILayoutOptionalNodeState State = ((ILayoutAssignmentOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a discrete value is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnDiscreteValueChanged(IWriteableChangeDiscreteValueOperation operation)
        {
            base.OnDiscreteValueChanged(operation);

            ILayoutNodeState State = ((ILayoutChangeDiscreteValueOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a text is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnTextChanged(IWriteableChangeTextOperation operation)
        {
            base.OnTextChanged(operation);

            ILayoutNodeState State = ((ILayoutChangeTextOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a comment is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnCommentChanged(IWriteableChangeCommentOperation operation)
        {
            base.OnCommentChanged(operation);

            ILayoutNodeState State = ((ILayoutChangeCommentOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a block state is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockStateChanged(IWriteableChangeBlockOperation operation)
        {
            base.OnBlockStateChanged(operation);

            ILayoutBlockState BlockState = ((ILayoutChangeBlockOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called every time a state is moved in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateMoved(IWriteableMoveNodeOperation operation)
        {
            base.OnStateMoved(operation);

            ILayoutPlaceholderNodeState State = ((ILayoutMoveNodeOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a block state is moved in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockStateMoved(IWriteableMoveBlockOperation operation)
        {
            base.OnBlockStateMoved(operation);

            ILayoutBlockState BlockState = ((ILayoutMoveBlockOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called every time a block split in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockSplit(IWriteableSplitBlockOperation operation)
        {
            base.OnBlockSplit(operation);

            ILayoutBlockState BlockState = ((ILayoutSplitBlockOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called every time two blocks are merged.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlocksMerged(IWriteableMergeBlocksOperation operation)
        {
            base.OnBlocksMerged(operation);

            ILayoutBlockState BlockState = ((ILayoutMergeBlocksOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(!BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called to refresh views.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnGenericRefresh(IWriteableGenericRefreshOperation operation)
        {
            base.OnGenericRefresh(operation);

            ILayoutNodeState RefreshState = ((ILayoutGenericRefreshOperation)operation).RefreshState;
            Debug.Assert(RefreshState != null);
            Debug.Assert(StateViewTable.ContainsKey(RefreshState));
        }

        private protected override IFrameCellViewTreeContext InitializedCellViewTreeContext(IFrameNodeStateView stateView)
        {
            ILayoutCellViewTreeContext Context = base.InitializedCellViewTreeContext(stateView) as ILayoutCellViewTreeContext;
            Debug.Assert(Context.ControllerView == this);
            Debug.Assert(Context.BlockStateView == null);

            return Context;
        }

        private protected override void ValidateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameBlockCellView blockCellView)
        {
            Debug.Assert(((ILayoutBlockCellView)blockCellView).StateView == (ILayoutNodeStateView)stateView);
            Debug.Assert(((ILayoutBlockCellView)blockCellView).ParentCellView == (ILayoutCellViewCollection)parentCellView);
        }

        private protected override void ValidateContainerCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView, IFrameContainerCellView containerCellView)
        {
            Debug.Assert(((ILayoutContainerCellView)containerCellView).StateView == (ILayoutNodeStateView)stateView);
            Debug.Assert(((ILayoutContainerCellView)containerCellView).ParentCellView == (ILayoutCellViewCollection)parentCellView);
            Debug.Assert(((ILayoutContainerCellView)containerCellView).ChildStateView == (ILayoutNodeStateView)childStateView);
        }

        private protected override void Refresh(IFrameNodeState state)
        {
            base.Refresh(state);

            IsInvalidated = true;
        }

        private protected virtual void MeasureCells()
        {
            ILayoutPlaceholderNodeState RootState = Controller.RootState;
            ILayoutNodeStateView RootStateView = StateViewTable[RootState];
            RootStateView.MeasureCells(null, null, Measure.Floating);

            InternalViewSize = RootStateView.CellSize;

            Debug.Assert(RegionHelper.IsFixed(InternalViewSize));
        }

        private protected virtual void ArrangeCells()
        {
            ILayoutPlaceholderNodeState RootState = Controller.RootState;
            ILayoutNodeStateView RootStateView = StateViewTable[RootState];

            Point ViewOrigin;

            if (ShowLineNumber)
            {
                Measure Width = MeasureLineNumberWidth();
                ViewOrigin = new Point(Width, Measure.Zero);
            }
            else
                ViewOrigin = Point.Origin;

            RootStateView.ArrangeCells(ViewOrigin);

            Debug.Assert(Point.IsEqual(RootStateView.CellOrigin, ViewOrigin));
        }

        private protected virtual Measure MeasureLineNumberWidth()
        {
            string LongestLineText = $"{LastLineNumber} ";
            Size LineSize = MeasureContext.MeasureText(LongestLineText, TextStyles.LineNumber, Measure.Floating);

            return LineSize.Width;
        }

        private protected virtual void DisplayLineNumber(Action<string, Point> handler)
        {
            bool[] DrawnLines = new bool[LastLineNumber + 1];
            int MaxLength = LastLineNumber.ToString().Length;

            EnumerateVisibleCellViews((IFrameVisibleCellView cellView) => DisplayCellViewLineNumber(cellView, handler, DrawnLines, MaxLength), out IFrameVisibleCellView lastCellView, reversed: false);
        }

        private protected virtual bool DisplayCellViewLineNumber(IFrameVisibleCellView cellView, Action<string, Point> handler, bool[] drawnLines, int maxLength)
        {
            int LineNumber = cellView.LineNumber;
            Debug.Assert(LineNumber < drawnLines.Length);

            if (!drawnLines[LineNumber])
            {
                drawnLines[LineNumber] = true;

                Point LineOrigin = new Point(Measure.Zero, ((ILayoutVisibleCellView)cellView).CellOrigin.Y);
                string LineText = LineNumber.ToString();
                while (LineText.Length < maxLength)
                    LineText = " " + LineText;

                handler(LineText, LineOrigin);
            }

            return false;
        }

        private protected override void ChangeFocus(int direction, int oldIndex, int newIndex, bool resetAnchor, out bool isRefreshed)
        {
            ILayoutFocus OldFocus = (ILayoutFocus)FocusChain[oldIndex];
            Debug.Assert(OldFocus == Focus);

            base.ChangeFocus(direction, oldIndex, newIndex, resetAnchor, out isRefreshed);

            ILayoutFocus NewFocus = Focus;
            Debug.Assert(NewFocus != OldFocus);

            if (OldFocus is ILayoutCommentFocus || NewFocus is ILayoutCommentFocus)
                Invalidate();
        }
    }
}
