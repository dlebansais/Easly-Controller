namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Constants;
    using EaslyController.Controller;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// View of an optional node state.
    /// </summary>
    public interface ILayoutOptionalNodeStateView : IFocusOptionalNodeStateView, ILayoutNodeStateView, ILayoutReplaceableStateView
    {
        /// <summary>
        /// The optional node state.
        /// </summary>
        new ILayoutOptionalNodeState State { get; }
    }

    /// <summary>
    /// View of an optional node state.
    /// </summary>
    internal class LayoutOptionalNodeStateView : FocusOptionalNodeStateView, ILayoutOptionalNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutOptionalNodeStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The optional node state.</param>
        public LayoutOptionalNodeStateView(LayoutControllerView controllerView, ILayoutOptionalNodeState state)
            : base(controllerView, state)
        {
            CellOrigin = RegionHelper.InvalidOrigin;
            CellSize = RegionHelper.InvalidSize;
            ActualCellSize = RegionHelper.InvalidSize;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new LayoutControllerView ControllerView { get { return (LayoutControllerView)base.ControllerView; } }

        /// <summary>
        /// The optional node state.
        /// </summary>
        public new ILayoutOptionalNodeState State { get { return (ILayoutOptionalNodeState)base.State; } }
        ILayoutNodeState ILayoutNodeStateView.State { get { return State; } }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        public new ILayoutTemplate Template { get { return (ILayoutTemplate)base.Template; } }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        public new ILayoutCellView RootCellView { get { return (ILayoutCellView)base.RootCellView; } }

        /// <summary>
        /// Table of cell views that are mutable lists of cells.
        /// </summary>
        public new LayoutAssignableCellViewReadOnlyDictionary<string> CellViewTable { get { return (LayoutAssignableCellViewReadOnlyDictionary<string>)base.CellViewTable; } }

        /// <summary>
        /// The cell view that is embedding this state view. Can be null.
        /// </summary>
        public new ILayoutContainerCellView ParentContainer { get { return (ILayoutContainerCellView)base.ParentContainer; } }

        /// <summary>
        /// Location of the state view.
        /// </summary>
        public Point CellOrigin { get; private set; }

        /// <summary>
        /// Floating size of cells in this state view.
        /// </summary>
        public Size CellSize { get; private set; }

        /// <summary>
        /// Actual size of cells in this state view.
        /// </summary>
        public Size ActualCellSize { get; private set; }

        /// <summary>
        /// Rectangular region for cells in this state view.
        /// </summary>
        public Rect CellRect { get { return new Rect(CellOrigin, ActualCellSize); } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Measure all cells in this state view.
        /// </summary>
        /// <param name="collectionWithSeparator">A collection that can draw separators around the cell.</param>
        /// <param name="referenceContainer">The cell view in <paramref name="collectionWithSeparator"/> that contains this cell.</param>
        /// <param name="separatorLength">The length of the separator in <paramref name="collectionWithSeparator"/>.</param>
        public void MeasureCells(ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer, Measure separatorLength)
        {
            Debug.Assert(RootCellView != null);
            RootCellView.Measure(collectionWithSeparator, referenceContainer, separatorLength);

            CellSize = RootCellView.CellSize;
            ActualCellSize = RegionHelper.InvalidSize;

            Debug.Assert(RegionHelper.IsValid(CellSize));
        }

        /// <summary>
        /// Arranges cells in this state view.
        /// </summary>
        /// <param name="origin">The cell location.</param>
        public virtual void ArrangeCells(Point origin)
        {
            Debug.Assert(RootCellView != null);
            RootCellView.Arrange(origin);

            CellOrigin = RootCellView.CellOrigin;

            Debug.Assert(RegionHelper.IsValid(CellOrigin));
        }

        /// <summary>
        /// Updates the actual size of cells in this state view.
        /// </summary>
        public virtual void UpdateActualCellsSize()
        {
            Debug.Assert(RootCellView != null);
            RootCellView.UpdateActualSize();

            Debug.Assert(RegionHelper.IsValid(RootCellView.ActualCellSize));
            ActualCellSize = RootCellView.ActualCellSize;
        }

        /// <summary>
        /// Draws cells in the state view.
        /// </summary>
        public virtual void DrawCells()
        {
            Debug.Assert(RegionHelper.IsValid(ActualCellSize));
            Debug.Assert(RootCellView != null);

            DrawSelection();
            RootCellView.Draw();
        }

        /// <summary></summary>
        protected virtual void DrawSelection()
        {
            if (ControllerView.Selection is ILayoutNodeSelection AsNodeSelection && AsNodeSelection.StateView == this)
            {
                ILayoutDrawContext DrawContext = ControllerView.DrawContext;
                Debug.Assert(DrawContext != null);

                DrawContext.DrawSelectionRectangle(CellRect, SelectionStyles.Node);
            }
        }

        /// <summary>
        /// Prints cells in the state view.
        /// </summary>
        /// <param name="origin">The origin from where to start printing.</param>
        public virtual void PrintCells(Point origin)
        {
            Debug.Assert(RegionHelper.IsValid(ActualCellSize));
            Debug.Assert(RootCellView != null);

            RootCellView.Print(origin);
        }
        #endregion

        #region Implementation
        private protected override void ValidateEmptyCellView(IFrameCellViewTreeContext context, IFrameEmptyCellView emptyCellView)
        {
            Debug.Assert(((ILayoutEmptyCellView)emptyCellView).StateView == ((ILayoutCellViewTreeContext)context).StateView);
            ILayoutCellViewCollection ParentCellView = ((ILayoutEmptyCellView)emptyCellView).ParentCellView;
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="LayoutOptionalNodeStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutOptionalNodeStateView AsOptionalNodeStateView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsOptionalNodeStateView))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxAssignableCellViewDictionary{string} object.
        /// </summary>
        private protected override FrameAssignableCellViewDictionary<string> CreateCellViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutOptionalNodeStateView));
            return new LayoutAssignableCellViewDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        private protected override IFrameEmptyCellView CreateEmptyCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutOptionalNodeStateView));
            return new LayoutEmptyCellView((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView);
        }
        #endregion
    }
}
