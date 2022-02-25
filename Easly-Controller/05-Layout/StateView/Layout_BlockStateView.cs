namespace EaslyController.Layout
{
    using System.Diagnostics;
    using Contracts;
    using EaslyController.Controller;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using NotNullReflection;

    /// <summary>
    /// View of a block state.
    /// </summary>
    public class LayoutBlockStateView : FocusBlockStateView
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="LayoutBlockStateView"/> object.
        /// </summary>
        public static new LayoutBlockStateView Empty { get; } = new LayoutBlockStateView(LayoutControllerView.Empty, LayoutBlockState<ILayoutInner<ILayoutBrowsingChildIndex>>.Empty, LayoutTemplate.Empty);

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBlockStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="blockState">The block state.</param>
        /// <param name="template">The frame template.</param>
        protected LayoutBlockStateView(LayoutControllerView controllerView, ILayoutBlockState blockState, ILayoutTemplate template)
            : base(controllerView, blockState, template)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBlockStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="blockState">The block state.</param>
        public LayoutBlockStateView(LayoutControllerView controllerView, ILayoutBlockState blockState)
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
        public new LayoutControllerView ControllerView { get { return (LayoutControllerView)base.ControllerView; } }

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
        /// Compares two <see cref="LayoutBlockStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out LayoutBlockStateView AsBlockStateView))
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
        private protected override FrameAssignableCellViewDictionary<string> CreateCellViewTable()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutBlockStateView>());
            return new LayoutAssignableCellViewDictionary<string>();
        }
        #endregion
    }
}
