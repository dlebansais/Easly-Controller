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
    public partial class LayoutControllerView : FocusControllerView, ILayoutInternalControllerView
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
        private protected override void OnBlockStateRemoved(WriteableRemoveBlockOperation operation)
        {
            base.OnBlockStateRemoved(operation);

            ILayoutBlockState BlockState = ((LayoutRemoveBlockOperation)operation).BlockState;

            Debug.Assert(BlockState != null);
            Debug.Assert(!BlockStateViewTable.ContainsKey(BlockState));

            Debug.Assert(!StateViewTable.ContainsKey(BlockState.PatternState));
            Debug.Assert(!StateViewTable.ContainsKey(BlockState.SourceState));

            ILayoutNodeState RemovedState = ((LayoutRemoveBlockOperation)operation).RemovedState;
            Debug.Assert(!StateViewTable.ContainsKey(RemovedState));

            Debug.Assert(BlockState.StateList.Count == 0);
        }

        /// <summary>
        /// Handler called every time a block view must be removed from the controller view.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockViewRemoved(WriteableRemoveBlockViewOperation operation)
        {
            base.OnBlockViewRemoved(operation);

            ILayoutBlockState BlockState = ((LayoutRemoveBlockViewOperation)operation).BlockState;

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
        private protected override void OnStateInserted(WriteableInsertNodeOperation operation)
        {
            base.OnStateInserted(operation);

            ILayoutNodeState ChildState = ((LayoutInsertNodeOperation)operation).ChildState;
            Debug.Assert(ChildState != null);
            Debug.Assert(StateViewTable.ContainsKey(ChildState));

            ILayoutBrowsingCollectionNodeIndex BrowsingIndex = ((LayoutInsertNodeOperation)operation).BrowsingIndex;
            Debug.Assert(ChildState.ParentIndex == BrowsingIndex);
        }

        /// <summary>
        /// Handler called every time a state is removed from the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateRemoved(WriteableRemoveNodeOperation operation)
        {
            base.OnStateRemoved(operation);

            ILayoutPlaceholderNodeState RemovedState = ((LayoutRemoveNodeOperation)operation).RemovedState;
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
        private protected override void OnStateAssigned(WriteableAssignmentOperation operation)
        {
            base.OnStateAssigned(operation);

            ILayoutOptionalNodeState State = ((LayoutAssignmentOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a state is unassigned in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateUnassigned(WriteableAssignmentOperation operation)
        {
            base.OnStateUnassigned(operation);

            ILayoutOptionalNodeState State = ((LayoutAssignmentOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a discrete value is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnDiscreteValueChanged(WriteableChangeDiscreteValueOperation operation)
        {
            base.OnDiscreteValueChanged(operation);

            ILayoutNodeState State = ((LayoutChangeDiscreteValueOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a text is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnTextChanged(WriteableChangeTextOperation operation)
        {
            base.OnTextChanged(operation);

            ILayoutNodeState State = ((LayoutChangeTextOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a comment is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnCommentChanged(WriteableChangeCommentOperation operation)
        {
            base.OnCommentChanged(operation);

            ILayoutNodeState State = ((LayoutChangeCommentOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a block state is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockStateChanged(WriteableChangeBlockOperation operation)
        {
            base.OnBlockStateChanged(operation);

            ILayoutBlockState BlockState = ((LayoutChangeBlockOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called every time a state is moved in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateMoved(WriteableMoveNodeOperation operation)
        {
            base.OnStateMoved(operation);

            ILayoutPlaceholderNodeState State = ((LayoutMoveNodeOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a block state is moved in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockStateMoved(WriteableMoveBlockOperation operation)
        {
            base.OnBlockStateMoved(operation);

            ILayoutBlockState BlockState = ((LayoutMoveBlockOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called every time a block split in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockSplit(WriteableSplitBlockOperation operation)
        {
            base.OnBlockSplit(operation);

            ILayoutBlockState BlockState = ((LayoutSplitBlockOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called every time two blocks are merged.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlocksMerged(WriteableMergeBlocksOperation operation)
        {
            base.OnBlocksMerged(operation);

            ILayoutBlockState BlockState = ((LayoutMergeBlocksOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(!BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called to refresh views.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnGenericRefresh(WriteableGenericRefreshOperation operation)
        {
            base.OnGenericRefresh(operation);

            ILayoutNodeState RefreshState = ((LayoutGenericRefreshOperation)operation).RefreshState;
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
            ILayoutNodeStateView RootStateView = (ILayoutNodeStateView)StateViewTable[RootState];
            RootStateView.MeasureCells(null, null, Measure.Floating);

            InternalViewSize = RootStateView.CellSize;

            Debug.Assert(RegionHelper.IsFixed(InternalViewSize));
        }

        private protected virtual void ArrangeCells()
        {
            ILayoutPlaceholderNodeState RootState = Controller.RootState;
            ILayoutNodeStateView RootStateView = (ILayoutNodeStateView)StateViewTable[RootState];

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
