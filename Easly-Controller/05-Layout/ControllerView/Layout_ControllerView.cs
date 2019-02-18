namespace EaslyController.Layout
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.Constants;
    using EaslyController.Controller;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public interface ILayoutControllerView : IFocusControllerView
    {
        /// <summary>
        /// The controller.
        /// </summary>
        new ILayoutController Controller { get; }

        /// <summary>
        /// Table of views of each state in the controller.
        /// </summary>
        new ILayoutStateViewDictionary StateViewTable { get; }

        /// <summary>
        /// Table of views of each block state in the controller.
        /// </summary>
        new ILayoutBlockStateViewDictionary BlockStateViewTable { get; }

        /// <summary>
        /// Template set describing the node tree.
        /// </summary>
        new ILayoutTemplateSet TemplateSet { get; }

        /// <summary>
        /// Cell view with the focus.
        /// </summary>
        new ILayoutFocus Focus { get; }

        /// <summary>
        /// The draw context.
        /// </summary>
        ILayoutDrawContext DrawContext { get; }

        /// <summary>
        /// Size of view.
        /// </summary>
        Size ViewSize { get; }

        /// <summary>
        /// Current text style if the focus is on a string property. Default otherwise.
        /// </summary>
        TextStyles FocusedTextStyle { get; }

        /// <summary>
        /// Indicates if the caret is shown or hidden.
        /// </summary>
        bool IsCaretShown { get; }

        /// <summary>
        /// Indicates if there are cells that must be measured and arranged.
        /// </summary>
        bool IsInvalidated { get; }

        /// <summary>
        /// Invalidates the entire view.
        /// </summary>
        void Invalidate();

        /// <summary>
        /// Invalidates the specified region.
        /// </summary>
        void Invalidate(Rect region);

        /// <summary>
        /// Measure and arrange cells in the view.
        /// </summary>
        void MeasureAndArrange();

        /// <summary>
        /// Draws all visible cells in the view using <see cref="DrawContext"/>.
        /// </summary>
        void Draw();

        /// <summary>
        /// Shows or hides the caret.
        /// </summary>
        /// <param name="show">Shows the caret if true. Otherwise, hides it.</param>
        /// <param name="draw">Draws the caret according to <paramref name="show"/> if true. Otherwise, just save the setting.</param>
        void ShowCaret(bool show, bool draw);

        /// <summary>
        /// Moves the focus up or down.
        /// The starting point is the center of the area covered by the current focus.
        /// </summary>
        /// <param name="distance">The distance to cross.</param>
        /// <param name="isMoved">True if the focus has changed.</param>
        void MoveFocusVertically(double distance, out bool isMoved);
    }

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public class LayoutControllerView : FocusControllerView, ILayoutControllerView
    {
        #region Init
        /// <summary>
        /// Creates and initializes a new instance of a <see cref="LayoutControllerView"/> object.
        /// </summary>
        /// <param name="controller">The controller on which the view is attached.</param>
        /// <param name="templateSet">The template set used to describe the view.</param>
        /// <param name="drawContext">The draw context used to measure, arrange and draw the view.</param>
        public static ILayoutControllerView Create(ILayoutController controller, ILayoutTemplateSet templateSet, ILayoutDrawContext drawContext)
        {
            LayoutControllerView View = new LayoutControllerView(controller, templateSet, drawContext);
            View.Init();
            return View;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutControllerView"/> class.
        /// </summary>
        /// <param name="controller">The controller on which the view is attached.</param>
        /// <param name="templateSet">The template set used to describe the view.</param>
        /// <param name="drawContext">The draw context used to measure, arrange and draw the view.</param>
        private protected LayoutControllerView(ILayoutController controller, ILayoutTemplateSet templateSet, ILayoutDrawContext drawContext)
            : base(controller, templateSet)
        {
            DrawContext = drawContext;
            InternalViewSize = RegionHelper.InvalidSize;
            IsInvalidated = true;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller.
        /// </summary>
        public new ILayoutController Controller { get { return (ILayoutController)base.Controller; } }

        /// <summary>
        /// Table of views of each state in the controller.
        /// </summary>
        public new ILayoutStateViewDictionary StateViewTable { get { return (ILayoutStateViewDictionary)base.StateViewTable; } }

        /// <summary>
        /// Table of views of each block state in the controller.
        /// </summary>
        public new ILayoutBlockStateViewDictionary BlockStateViewTable { get { return (ILayoutBlockStateViewDictionary)base.BlockStateViewTable; } }

        /// <summary>
        /// Template set describing the node tree.
        /// </summary>
        public new ILayoutTemplateSet TemplateSet { get { return (ILayoutTemplateSet)base.TemplateSet; } }

        /// <summary>
        /// Cell view with the focus.
        /// </summary>
        public new ILayoutFocus Focus { get { return (ILayoutFocus)base.Focus; } }

        /// <summary>
        /// The draw context.
        /// </summary>
        public ILayoutDrawContext DrawContext { get; private set; }

        /// <summary>
        /// Size of view.
        /// </summary>
        public Size ViewSize
        {
            get
            {
                if (IsInvalidated)
                    MeasureAndArrange();

                return InternalViewSize;
            }
        }
        private Size InternalViewSize;

        /// <summary>
        /// Current text style if the focus is on a string property. Default otherwise.
        /// </summary>
        public TextStyles FocusedTextStyle
        {
            get
            {
                TextStyles Result = TextStyles.Default;
                bool IsHandled = false;

                if (Focus is ILayoutTextCellFocus AsText)
                {
                    switch (AsText.CellView.Frame)
                    {
                        case ILayoutCharacterFrame AsCharacterFrame:
                            Result = TextStyles.Character;
                            IsHandled = true;
                            break;

                        case ILayoutNumberFrame AsNumberFrame:
                            Result = TextStyles.Number;
                            IsHandled = true;
                            break;

                        case ILayoutTextValueFrame AsTextValueFrame:
                            Result = AsTextValueFrame.TextStyle;
                            IsHandled = true;
                            break;
                    }
                }
                else
                    IsHandled = true;

                Debug.Assert(IsHandled);
                return Result;
            }
        }

        /// <summary>
        /// Indicates if the caret is shown or hidden.
        /// </summary>
        public bool IsCaretShown { get; private set; }

        /// <summary>
        /// Indicates if there are cells that must be measured and arranged.
        /// </summary>
        public bool IsInvalidated { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Invalidates the entire view.
        /// </summary>
        public virtual void Invalidate()
        {
            IsInvalidated = true;
        }

        /// <summary>
        /// Invalidates the specified region.
        /// </summary>
        public virtual void Invalidate(Rect region)
        {
            IsInvalidated = true;
        }

        /// <summary>
        /// Measure and arrange cells in the view.
        /// </summary>
        public virtual void MeasureAndArrange()
        {
            IsInvalidated = false;

            MeasureCells();
            ArrangeCells();
        }

        /// <summary>
        /// Draws all visible cells in the view using <see cref="DrawContext"/>.
        /// </summary>
        public virtual void Draw()
        {
            if (IsInvalidated)
                MeasureAndArrange();

            if (IsCaretShown)
                DrawContext.HideCaret();

            ILayoutVisibleCellViewList CellList = new LayoutVisibleCellViewList();
            EnumerateVisibleCellViews(CellList);

            IDocument LastDocumentation = null;

            foreach (ILayoutVisibleCellView CellView in CellList)
            {
                bool IsFocused = Focus is ILayoutCellFocus AsCellFocus && CellView == AsCellFocus.CellView;
                CellView.Draw(IsFocused, out Size MeasuredSize);

                if (!IsFocused)
                {
                    IDocument Documentation = CellView.StateView.State.Node.Documentation;
                    if (Documentation.Comment.Length > 0 && LastDocumentation != Documentation)
                    {
                        Debug.Assert(RegionHelper.IsFixed(MeasuredSize));

                        LastDocumentation = Documentation;
                        DrawContext.DrawCommentIcon(new Rect(CellView.CellOrigin, MeasuredSize));
                    }
                }
            }

            if (IsCaretShown && IsCaretOnText(out ILayoutTextCellFocus TextCellFocus))
                DrawTextCaret(TextCellFocus);
        }

        /// <summary>
        /// Shows or hides the caret.
        /// </summary>
        /// <param name="show">Shows the caret if true. Otherwise, hides it.</param>
        /// <param name="draw">Draws the caret according to <paramref name="show"/> if true. Otherwise, just save the setting.</param>
        public virtual void ShowCaret(bool show, bool draw)
        {
            if (IsCaretShown == show)
                return;

            IsCaretShown = show;

            if (draw)
                if (IsCaretShown)
                {
                    if (IsCaretOnText(out ILayoutTextCellFocus TextCellFocus))
                        DrawTextCaret(TextCellFocus);
                    else if (Focus is ILayoutCellFocus AsCellFocus)
                        AsCellFocus.CellView.Draw(true, out Size MeasuredSize);
                }
                else
                    DrawContext.HideCaret();
        }

        /// <summary></summary>
        private protected bool IsCaretOnText(out ILayoutTextCellFocus textCellFocus)
        {
            textCellFocus = null;

            if (Focus is ILayoutTextCellFocus AsTextCellFocus)
            {
                bool IsHandled = false;

                switch (AsTextCellFocus.CellView.Frame)
                {
                    case ILayoutCharacterFrame AsCharacterFrame:
                        IsHandled = true; // The focus was displayed directly with the character.
                        break;

                    case ILayoutNumberFrame AsNumberFrame:
                    case ILayoutTextValueFrame AsTextValueFrame:
                        textCellFocus = AsTextCellFocus;
                        IsHandled = true;
                        break;
                }

                Debug.Assert(IsHandled);
            }

            return textCellFocus != null;
        }

        /// <summary></summary>
        private protected void DrawTextCaret(ILayoutTextCellFocus textCellFocus)
        {
            ILayoutTextFocusableCellView CellView = textCellFocus.CellView;

            INode Node = CellView.StateView.State.Node;
            string PropertyName = CellView.PropertyName;
            string Text = NodeTreeHelper.GetString(Node, PropertyName);

            Point CellOrigin = CellView.CellOrigin;
            Padding CellPadding = CellView.CellPadding;

            Point OriginWithPadding = CellOrigin.Moved(CellPadding.Left, 0);
            DrawContext.ShowCaret(OriginWithPadding, FocusedText, FocusedTextStyle, CaretMode, CaretPosition);
        }

        /// <summary>
        /// Moves the focus up or down.
        /// The starting point is the center of the area covered by the current focus.
        /// </summary>
        /// <param name="distance">The distance to cross.</param>
        /// <param name="isMoved">True if the focus has changed.</param>
        public virtual void MoveFocusVertically(double distance, out bool isMoved)
        {
            Point Origin = GetMoveFocusDestinationLocation(distance);
            double BestVerticalDistance = 0;
            double BestHorizontalDistance = 0;
            int BestCellIndex = -1;
            int FocusIndex = FocusChain.IndexOf(Focus);

            for (int i = 0; i < FocusChain.Count; i++)
            {
                int CellIndex = distance < 0 ? i : FocusChain.Count - i - 1;
                ILayoutFocus Focus = (ILayoutFocus)FocusChain[CellIndex];

                if (Focus is ILayoutCellFocus AsCellFocus)
                {
                    ILayoutFocusableCellView CellView = AsCellFocus.CellView;

                    // If we check against the current focus, it might be the closest cell and then we don't move!
                    if (CellIndex == FocusIndex)
                        continue;

                    // Don't consider cells that are in the wrong direction;
                    if ((distance < 0 && CellView.CellOrigin.Y >= Origin.Y) || (distance > 0 && CellView.CellOrigin.Y + CellView.CellSize.Height <= Origin.Y))
                        continue;

                    if (CellView.CellRect.IsPointInRect(Origin))
                    {
                        BestCellIndex = CellIndex;
                        break;
                    }
                    else
                    {
                        Point Center = CellView.CellRect.Center;

                        double VerticalDistance = Math.Abs(Center.Y - Origin.Y);
                        double HorizontalDistance = Math.Abs(Center.X - Origin.X);

                        if (BestCellIndex < 0 || BestVerticalDistance > VerticalDistance || (RegionHelper.IsZero(BestVerticalDistance - VerticalDistance) && BestHorizontalDistance > HorizontalDistance))
                        {
                            BestVerticalDistance = VerticalDistance;
                            BestHorizontalDistance = HorizontalDistance;
                            BestCellIndex = CellIndex;
                        }
                    }
                }
            }

            if (BestCellIndex >= 0 && BestCellIndex != FocusIndex)
            {
                Debug.Assert(BestCellIndex >= MinFocusMove + FocusIndex && BestCellIndex <= MaxFocusMove + FocusIndex);

                MoveFocus(BestCellIndex - FocusIndex, out isMoved);
            }
            else
                isMoved = false;
        }

        /// <summary></summary>
        private protected Point GetMoveFocusDestinationLocation(double distance)
        {
            Point Origin = RegionHelper.InvalidOrigin;

            if (Focus is ILayoutTextCellFocus AsTextCellFocus)
            {
                bool IsHandled = false;
                ILayoutTextFocusableCellView CellView = AsTextCellFocus.CellView;
                double Position;

                switch (CaretMode)
                {
                    case CaretModes.Insertion:
                        Debug.Assert(CaretPosition >= 0 && CaretPosition <= MaxCaretPosition);
                        Position = (CellView.CellSize.Width * CaretPosition) / MaxCaretPosition;
                        Origin = CellView.CellOrigin.Moved(Position, CellView.CellSize.Height / 2);
                        IsHandled = true;
                        break;

                    case CaretModes.Override:
                        Debug.Assert(CaretPosition >= 0 && CaretPosition < MaxCaretPosition);
                        Position = (CellView.CellSize.Width * ((CaretPosition * 2) + 1)) / (MaxCaretPosition * 2);
                        Origin = CellView.CellOrigin.Moved(Position, CellView.CellSize.Height / 2);
                        IsHandled = true;
                        break;
                }

                Debug.Assert(IsHandled);
            }
            else if (Focus is ILayoutCellFocus AsCellFocus)
            {
                ILayoutFocusableCellView CellView = AsCellFocus.CellView;
                Origin = CellView.CellOrigin.Moved(CellView.CellSize.Width / 2, CellView.CellSize.Height / 2);
            }

            Debug.Assert(RegionHelper.IsValid(Origin));

            Origin = Origin.Moved(0, distance);

            return Origin;
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
        /// Handler called every time a state is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateChanged(IWriteableChangeNodeOperation operation)
        {
            base.OnStateChanged(operation);

            ILayoutNodeState State = ((ILayoutChangeNodeOperation)operation).State;
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

        /// <summary></summary>
        private protected override IFrameCellViewTreeContext InitializedCellViewTreeContext(IFrameNodeStateView stateView)
        {
            ILayoutCellViewTreeContext Context = base.InitializedCellViewTreeContext(stateView) as ILayoutCellViewTreeContext;
            Debug.Assert(Context.ControllerView == this);
            Debug.Assert(Context.BlockStateView == null);

            return Context;
        }

        /// <summary></summary>
        private protected override void ValidateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameBlockCellView blockCellView)
        {
            Debug.Assert(((ILayoutBlockCellView)blockCellView).StateView == (ILayoutNodeStateView)stateView);
            Debug.Assert(((ILayoutBlockCellView)blockCellView).ParentCellView == (ILayoutCellViewCollection)parentCellView);
        }

        /// <summary></summary>
        private protected override void ValidateContainerCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView, IFrameContainerCellView containerCellView)
        {
            Debug.Assert(((ILayoutContainerCellView)containerCellView).StateView == (ILayoutNodeStateView)stateView);
            Debug.Assert(((ILayoutContainerCellView)containerCellView).ParentCellView == (ILayoutCellViewCollection)parentCellView);
            Debug.Assert(((ILayoutContainerCellView)containerCellView).ChildStateView == (ILayoutNodeStateView)childStateView);
        }

        /// <summary></summary>
        private protected override void Refresh(IFrameNodeState state)
        {
            base.Refresh(state);

            IsInvalidated = true;
        }

        /// <summary></summary>
        private protected virtual void MeasureCells()
        {
            ILayoutPlaceholderNodeState RootState = Controller.RootState;
            ILayoutNodeStateView RootStateView = StateViewTable[RootState];
            RootStateView.MeasureCells(null, null, double.NaN);

            InternalViewSize = RootStateView.CellSize;

            Debug.Assert(RegionHelper.IsFixed(InternalViewSize));
        }

        /// <summary></summary>
        private protected virtual void ArrangeCells()
        {
            ILayoutPlaceholderNodeState RootState = Controller.RootState;
            ILayoutNodeStateView RootStateView = StateViewTable[RootState];

            RootStateView.ArrangeCells(Point.Origin);

            Point ViewOrigin = RootStateView.CellOrigin;
            Debug.Assert(ViewOrigin.IsOrigin);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ILayoutControllerView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutControllerView AsControllerView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsControllerView))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxStateViewDictionary object.
        /// </summary>
        private protected override IReadOnlyStateViewDictionary CreateStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxBlockStateViewDictionary object.
        /// </summary>
        private protected override IReadOnlyBlockStateViewDictionary CreateBlockStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutBlockStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxAttachCallbackSet object.
        /// </summary>
        private protected override IReadOnlyAttachCallbackSet CreateCallbackSet()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutAttachCallbackSet()
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
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutPlaceholderNodeStateView(this, (ILayoutPlaceholderNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxOptionalNodeStateView object.
        /// </summary>
        private protected override IReadOnlyOptionalNodeStateView CreateOptionalNodeStateView(IReadOnlyOptionalNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutOptionalNodeStateView(this, (ILayoutOptionalNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxPatternStateView object.
        /// </summary>
        private protected override IReadOnlyPatternStateView CreatePatternStateView(IReadOnlyPatternState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutPatternStateView(this, (ILayoutPatternState)state);
        }

        /// <summary>
        /// Creates a IxxxSourceStateView object.
        /// </summary>
        private protected override IReadOnlySourceStateView CreateSourceStateView(IReadOnlySourceState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutSourceStateView(this, (ILayoutSourceState)state);
        }

        /// <summary>
        /// Creates a IxxxBlockStateView object.
        /// </summary>
        private protected override IReadOnlyBlockStateView CreateBlockStateView(IReadOnlyBlockState blockState)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutBlockStateView(this, (ILayoutBlockState)blockState);
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        private protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView, IFrameFrame frame)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutContainerCellView((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView, (ILayoutNodeStateView)childStateView, (ILayoutFrame)frame);
        }

        /// <summary>
        /// Creates a IxxxBlockCellView object.
        /// </summary>
        private protected override IFrameBlockCellView CreateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameBlockStateView blockStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutBlockCellView((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView, (ILayoutBlockStateView)blockStateView);
        }

        /// <summary>
        /// Creates a IxxxCellViewTreeContext object.
        /// </summary>
        private protected override IFrameCellViewTreeContext CreateCellViewTreeContext(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutCellViewTreeContext(this, (ILayoutNodeStateView)stateView);
        }

        /// <summary>
        /// Creates a IxxxFocusList object.
        /// </summary>
        private protected override IFocusFocusList CreateFocusChain()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutFocusList();
        }

        /// <summary>
        /// Creates a IxxxInsertionNewBlockNodeIndex object.
        /// </summary>
        private protected override IFocusInsertionNewBlockNodeIndex CreateNewBlockNodeIndex(INode parentNode, string propertyName, INode node, int blockIndex, IPattern patternNode, IIdentifier sourceNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutInsertionNewBlockNodeIndex(parentNode, propertyName, node, 0, patternNode, sourceNode);
        }

        /// <summary>
        /// Creates a IxxxInsertionExistingBlockNodeIndex object.
        /// </summary>
        private protected override IFocusInsertionExistingBlockNodeIndex CreateExistingBlockNodeIndex(INode parentNode, string propertyName, INode node, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutInsertionExistingBlockNodeIndex(parentNode, propertyName, node, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxInsertionListNodeIndex object.
        /// </summary>
        private protected override IFocusInsertionListNodeIndex CreateListNodeIndex(INode parentNode, string propertyName, INode node, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutInsertionListNodeIndex(parentNode, propertyName, node, index);
        }
        #endregion
    }
}
