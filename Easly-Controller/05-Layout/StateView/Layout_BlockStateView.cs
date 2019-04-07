namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Controller;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// View of a block state.
    /// </summary>
    public interface ILayoutBlockStateView : IFocusBlockStateView
    {
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        new ILayoutControllerView ControllerView { get; }

        /// <summary>
        /// The block state.
        /// </summary>
        new ILayoutBlockState BlockState { get; }

        /// <summary>
        /// The template used to display the block state.
        /// </summary>
        new ILayoutTemplate Template { get; }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        new ILayoutCellView RootCellView { get; }

        /// <summary>
        /// List of cell views for each child node.
        /// </summary>
        new ILayoutCellViewCollection EmbeddingCellView { get; }

        /// <summary>
        /// Location of the block state view.
        /// </summary>
        Point CellOrigin { get; }

        /// <summary>
        /// Floating size of cells in this block state view.
        /// </summary>
        Size CellSize { get; }

        /// <summary>
        /// Actual size of cells in this block state view.
        /// </summary>
        Size ActualCellSize { get; }

        /// <summary>
        /// Measure all cells in this block state view.
        /// </summary>
        /// <param name="collectionWithSeparator">A collection that can draw separators around the cell.</param>
        /// <param name="referenceContainer">The cell view in <paramref name="collectionWithSeparator"/> that contains this cell.</param>
        /// <param name="separatorLength">The length of the separator in <paramref name="collectionWithSeparator"/>.</param>
        void MeasureCells(ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer, Measure separatorLength);

        /// <summary>
        /// Arranges cells in this block state view.
        /// </summary>
        /// <param name="origin">The cell location.</param>
        void ArrangeCells(Point origin);

        /// <summary>
        /// Updates the actual size of cells in this block state view.
        /// </summary>
        void UpdateActualCellsSize();

        /// <summary>
        /// Draws cells in this block state view.
        /// </summary>
        void DrawCells();

        /// <summary>
        /// Prints cells in this block state view.
        /// </summary>
        /// <param name="origin">The origin from where to start printing.</param>
        void PrintCells(Point origin);
    }

    /// <summary>
    /// View of a block state.
    /// </summary>
    internal class LayoutBlockStateView : FocusBlockStateView, ILayoutBlockStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBlockStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="blockState">The block state.</param>
        public LayoutBlockStateView(ILayoutControllerView controllerView, ILayoutBlockState blockState)
            : base(controllerView, blockState)
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
        public new ILayoutControllerView ControllerView { get { return (ILayoutControllerView)base.ControllerView; } }

        /// <summary>
        /// The block state.
        /// </summary>
        public new ILayoutBlockState BlockState { get { return (ILayoutBlockState)base.BlockState; } }

        /// <summary>
        /// The template used to display the block state.
        /// </summary>
        public new ILayoutTemplate Template { get { return (ILayoutTemplate)base.Template; } }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        public new ILayoutCellView RootCellView { get { return (ILayoutCellView)base.RootCellView; } }

        /// <summary>
        /// List of cell views for each child node.
        /// </summary>
        public new ILayoutCellViewCollection EmbeddingCellView { get { return (ILayoutCellViewCollection)base.EmbeddingCellView; } }

        /// <summary>
        /// Location of the block state view.
        /// </summary>
        public Point CellOrigin { get; private set; }

        /// <summary>
        /// Floating size of cells in this block state view.
        /// </summary>
        public Size CellSize { get; private set; }

        /// <summary>
        /// Actual size of cells in this block state view.
        /// </summary>
        public Size ActualCellSize { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Measure all cells in this block state view.
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
        /// Arranges cells in this block state view.
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
        /// Updates the actual size of cells in this block state view.
        /// </summary>
        public virtual void UpdateActualCellsSize()
        {
            Debug.Assert(RootCellView != null);
            RootCellView.UpdateActualSize();

            Debug.Assert(RegionHelper.IsValid(RootCellView.ActualCellSize));
            ActualCellSize = RootCellView.ActualCellSize;
        }

        /// <summary>
        /// Draws cells in this block state view.
        /// </summary>
        public virtual void DrawCells()
        {
            Debug.Assert(RegionHelper.IsValid(ActualCellSize));
            Debug.Assert(RootCellView != null);

            RootCellView.Draw();
        }

        /// <summary>
        /// Prints cells in this block state view.
        /// </summary>
        /// <param name="origin">The origin from where to start printing.</param>
        public virtual void PrintCells(Point origin)
        {
            Debug.Assert(RegionHelper.IsValid(ActualCellSize));
            Debug.Assert(RootCellView != null);

            RootCellView.Print(origin);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ILayoutBlockStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutBlockStateView AsBlockStateView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBlockStateView))
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
            ControllerTools.AssertNoOverride(this, typeof(LayoutBlockStateView));
            return new LayoutAssignableCellViewDictionary<string>();
        }
        #endregion
    }
}
