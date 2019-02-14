namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Controller;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// View of a source state.
    /// </summary>
    public interface ILayoutSourceStateView : IFocusSourceStateView, ILayoutNodeStateView
    {
        /// <summary>
        /// The pattern state.
        /// </summary>
        new ILayoutSourceState State { get; }
    }

    /// <summary>
    /// View of a source state.
    /// </summary>
    internal class LayoutSourceStateView : FocusSourceStateView, ILayoutSourceStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutSourceStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The source state.</param>
        public LayoutSourceStateView(ILayoutControllerView controllerView, ILayoutSourceState state)
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
        public new ILayoutSourceState State { get { return (ILayoutSourceState)base.State; } }
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
        public void MeasureCells()
        {
            Debug.Assert(RootCellView != null);
            RootCellView.Measure();

            CellSize = RootCellView.CellSize;

            Debug.Assert(MeasureHelper.IsValid(CellSize));
        }

        /// <summary>
        /// Arranges cells in this state view.
        /// </summary>
        public virtual void ArrangeCells(Point origin, ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer)
        {
            Debug.Assert(RootCellView != null);
            RootCellView.Arrange(origin, collectionWithSeparator, referenceContainer);

            CellOrigin = RootCellView.CellOrigin;

            Debug.Assert(ArrangeHelper.IsValid(CellOrigin));
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ILayoutSourceStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutSourceStateView AsSourceStateView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsSourceStateView))
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
            ControllerTools.AssertNoOverride(this, typeof(LayoutSourceStateView));
            return new LayoutAssignableCellViewDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        private protected override IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView, IFocusCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutSourceStateView));
            return new LayoutEmptyCellView((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView);
        }
        #endregion
    }
}
