namespace EaslyController.Layout
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Constants;
    using EaslyController.Controller;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;

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
        /// State view of the root state.
        /// </summary>
        new ILayoutNodeStateView RootStateView { get; }

        /// <summary>
        /// Template set describing the node tree.
        /// </summary>
        new ILayoutTemplateSet TemplateSet { get; }

        /// <summary>
        /// Cell view with the focus.
        /// </summary>
        new ILayoutFocus Focus { get; }

        /// <summary>
        /// The current selection.
        /// </summary>
        new ILayoutSelection Selection { get; }

        /// <summary>
        /// The measure context.
        /// </summary>
        ILayoutMeasureContext MeasureContext { get; }

        /// <summary>
        /// The draw context.
        /// </summary>
        ILayoutDrawContext DrawContext { get; }

        /// <summary>
        /// The print context.
        /// </summary>
        ILayoutPrintContext PrintContext { get; }

        /// <summary>
        /// Size of view.
        /// </summary>
        Size ViewSize { get; }

        /// <summary>
        /// Current text style if the focus is on a string property. Default otherwise.
        /// </summary>
        TextStyles FocusedTextStyle { get; }

        /// <summary>
        /// Displayed caret mode.
        /// </summary>
        CaretModes ActualCaretMode { get; }

        /// <summary>
        /// Indicates if the caret is shown or hidden.
        /// </summary>
        bool IsCaretShown { get; }

        /// <summary>
        /// Indicates if there are cells that must be measured and arranged.
        /// </summary>
        bool IsInvalidated { get; }

        /// <summary>
        /// Shows a comment sign over comments in <see cref="CommentDisplayModes.OnFocus"/> mode.
        /// </summary>
        bool ShowUnfocusedComments { get; }

        /// <summary>
        /// Shows block geometry around blocks.
        /// </summary>
        bool ShowBlockGeometry { get; }

        /// <summary>
        /// Shows line numbers.
        /// </summary>
        bool ShowLineNumber { get; }

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
        /// Updates all floating sizes and positions.
        /// </summary>
        void UpdateLayout();

        /// <summary>
        /// Draws all visible cells in the view using <see cref="DrawContext"/>.
        /// <param name="stateView">The view to draw.</param>
        /// </summary>
        void Draw(ILayoutNodeStateView stateView);

        /// <summary>
        /// Prints all visible cells in a view using <see cref="PrintContext"/>.
        /// </summary>
        /// <param name="stateView">The view to print.</param>
        /// <param name="origin">The origin from where to start printing.</param>
        void Print(ILayoutNodeStateView stateView, Point origin);

        /// <summary>
        /// Prints the selection.
        /// </summary>
        void PrintSelection();

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
        /// <param name="resetAnchor">If true, resets the selected text anchor.</param>
        /// <param name="isMoved">True if the focus has changed.</param>
        void MoveFocusVertically(double distance, bool resetAnchor, out bool isMoved);

        /// <summary>
        /// Moves the focus to the beginning or end of a line.
        /// The starting point is the center of the area covered by the current focus.
        /// </summary>
        /// <param name="direction">-1 for the beginning of the line, +1 for the end.</param>
        /// <param name="resetAnchor">If true, resets the selected text anchor.</param>
        /// <param name="isMoved">True if the focus has changed.</param>
        void MoveFocusHorizontally(int direction, bool resetAnchor, out bool isMoved);

        /// <summary>
        /// Sets <see cref="ShowUnfocusedComments"/>.
        /// </summary>
        /// <param name="show">True to show, false to hide.</param>
        void SetShowUnfocusedComments(bool show);

        /// <summary>
        /// Sets <see cref="ShowBlockGeometry"/>.
        /// </summary>
        /// <param name="show">True to show, false to hide.</param>
        void SetShowBlockGeometry(bool show);

        /// <summary>
        /// Sets <see cref="ShowLineNumber"/>.
        /// </summary>
        /// <param name="show">True to show, false to hide.</param>
        void SetShowLineNumber(bool show);

        /// <summary>
        /// Gets the visible cell view corresponding to a location.
        /// </summary>
        /// <param name="x">X-coordinate of the location where to look.</param>
        /// <param name="y">Y-coordinate of the location where to look.</param>
        /// <param name="cellView">The cell view upon return. Null if not found.</param>
        /// <returns>True if found; otherwise, false.</returns>
        bool CellViewFromPoint(double x, double y, out ILayoutVisibleCellView cellView);

        /// <summary>
        /// Sets the focus to the visible cell view corresponding to a location.
        /// </summary>
        /// <param name="x">X-coordinate of the location where to set the focus.</param>
        /// <param name="y">Y-coordinate of the location where to set the focus.</param>
        /// <param name="resetAnchor">If true, resets the selected text anchor.</param>
        /// <param name="isMoved">True if the focus was moved.</param>
        void SetFocusToPoint(double x, double y, bool resetAnchor, out bool isMoved);
    }

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public partial class LayoutControllerView : FocusControllerView, ILayoutControllerView, ILayoutInternalControllerView
    {
        #region Debugging
        /// <summary>
        /// Compares two <see cref="LayoutControllerView"/> objects.
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
            return new LayoutCellViewTreeContext(this, (ILayoutNodeStateView)stateView, (ILayoutNodeStateView)ForcedCommentStateView);
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
        private protected override IFocusInsertionNewBlockNodeIndex CreateNewBlockNodeIndex(Node parentNode, string propertyName, Node node, int blockIndex, Pattern patternNode, Identifier sourceNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutInsertionNewBlockNodeIndex(parentNode, propertyName, node, 0, patternNode, sourceNode);
        }

        /// <summary>
        /// Creates a IxxxInsertionExistingBlockNodeIndex object.
        /// </summary>
        private protected override IFocusInsertionExistingBlockNodeIndex CreateExistingBlockNodeIndex(Node parentNode, string propertyName, Node node, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutInsertionExistingBlockNodeIndex(parentNode, propertyName, node, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxInsertionListNodeIndex object.
        /// </summary>
        private protected override IFocusInsertionListNodeIndex CreateListNodeIndex(Node parentNode, string propertyName, Node node, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutInsertionListNodeIndex(parentNode, propertyName, node, index);
        }

        /// <summary>
        /// Creates a IxxxEmptySelection object.
        /// </summary>
        private protected override IFocusEmptySelection CreateEmptySelection()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutEmptySelection(RootStateView);
        }

        /// <summary>
        /// Creates a IxxxDiscreteContentSelection object.
        /// </summary>
        private protected override IFocusDiscreteContentSelection CreateDiscreteContentSelection(IFocusNodeStateView stateView, string propertyName)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutDiscreteContentSelection((ILayoutNodeStateView)stateView, propertyName);
        }

        /// <summary>
        /// Creates a IxxxStringContentSelection object.
        /// </summary>
        private protected override IFocusStringContentSelection CreateStringContentSelection(IFocusNodeStateView stateView, string propertyName, int start, int end)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutStringContentSelection((ILayoutNodeStateView)stateView, propertyName, start, end);
        }

        /// <summary>
        /// Creates a IxxxCommentSelection object.
        /// </summary>
        private protected override IFocusCommentSelection CreateCommentSelection(IFocusNodeStateView stateView, int start, int end)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutCommentSelection((ILayoutNodeStateView)stateView, start, end);
        }

        /// <summary>
        /// Creates a IxxxNodeSelection object.
        /// </summary>
        private protected override IFocusNodeSelection CreateNodeSelection(IFocusNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutNodeSelection((ILayoutNodeStateView)stateView);
        }

        /// <summary>
        /// Creates a IxxxListNodeSelection object.
        /// </summary>
        private protected override IFocusNodeListSelection CreateNodeListSelection(IFocusNodeStateView stateView, string propertyName, int startIndex, int endIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutNodeListSelection((ILayoutNodeStateView)stateView, propertyName, startIndex, endIndex);
        }

        /// <summary>
        /// Creates a IxxxBlockListNodeSelection object.
        /// </summary>
        private protected override IFocusBlockNodeListSelection CreateBlockNodeListSelection(IFocusNodeStateView stateView, string propertyName, int blockIndex, int startIndex, int endIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutBlockNodeListSelection((ILayoutNodeStateView)stateView, propertyName, blockIndex, startIndex, endIndex);
        }

        /// <summary>
        /// Creates a IxxxBlockSelection object.
        /// </summary>
        private protected override IFocusBlockListSelection CreateBlockListSelection(IFocusNodeStateView stateView, string propertyName, int startIndex, int endIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutControllerView));
            return new LayoutBlockListSelection((ILayoutNodeStateView)stateView, propertyName, startIndex, endIndex);
        }
        #endregion
    }
}
