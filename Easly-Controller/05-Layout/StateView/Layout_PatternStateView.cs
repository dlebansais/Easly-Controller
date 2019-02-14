namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Controller;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// View of a pattern state.
    /// </summary>
    public interface ILayoutPatternStateView : IFocusPatternStateView, ILayoutNodeStateView
    {
        /// <summary>
        /// The pattern state.
        /// </summary>
        new ILayoutPatternState State { get; }
    }

    /// <summary>
    /// View of a pattern state.
    /// </summary>
    internal class LayoutPatternStateView : FocusPatternStateView, ILayoutPatternStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutPatternStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The pattern state.</param>
        public LayoutPatternStateView(ILayoutControllerView controllerView, ILayoutPatternState state)
            : base(controllerView, state)
        {
            CellOrigin = ArrangeHelper.InvalidOrigin;
            CellSize = MeasureHelper.InvalidSize;
            CellPadding = Padding.Empty;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new ILayoutControllerView ControllerView { get { return (ILayoutControllerView)base.ControllerView; } }

        /// <summary>
        /// The pattern state.
        /// </summary>
        public new ILayoutPatternState State { get { return (ILayoutPatternState)base.State; } }
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
        public new ILayoutAssignableCellViewReadOnlyDictionary<string> CellViewTable { get { return (ILayoutAssignableCellViewReadOnlyDictionary<string>)base.CellViewTable; } }

        /// <summary>
        /// The cell view that is embedding this state view. Can be null.
        /// </summary>
        public new ILayoutContainerCellView ParentContainer { get { return (ILayoutContainerCellView)base.ParentContainer; } }

        /// <summary>
        /// Location of the state view.
        /// </summary>
        public Point CellOrigin { get; private set; }

        /// <summary>
        /// Size of cells in this state view.
        /// </summary>
        public Size CellSize { get; private set; }

        /// <summary>
        /// Padding inside the cell.
        /// </summary>
        public Padding CellPadding { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Measure all cells in this state view.
        /// </summary>
        /// <param name="collectionWithSeparator">A collection that can draw separators around the cell.</param>
        /// <param name="referenceContainer">The cell view in <paramref name="collectionWithSeparator"/> that contains this cell.</param>
        /// <param name="separatorLength">The length of the separator in <paramref name="collectionWithSeparator"/>.</param>
        public void MeasureCells(ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer, double separatorLength)
        {
            Debug.Assert(RootCellView != null);
            RootCellView.Measure(collectionWithSeparator, referenceContainer, separatorLength);

            CellSize = RootCellView.CellSize;

            Debug.Assert(MeasureHelper.IsValid(CellSize));
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

            Debug.Assert(ArrangeHelper.IsValid(CellOrigin));
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ILayoutPatternStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutPatternStateView AsPatternStateView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsPatternStateView))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxAssignableCellViewDictionary{string} object.
        /// </summary>
        private protected override IFrameAssignableCellViewDictionary<string> CreateCellViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutPatternStateView));
            return new LayoutAssignableCellViewDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        private protected override IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView, IFocusCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutPatternStateView));
            return new LayoutEmptyCellView((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView);
        }
        #endregion
    }
}
