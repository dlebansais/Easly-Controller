﻿namespace EaslyController.Layout
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
        /// Size of cells in this block state view.
        /// </summary>
        Size CellSize { get; }

        /// <summary>
        /// Measure all cells in this block state view.
        /// </summary>
        void MeasureCells();
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
            CellSize = MeasureHelper.InvalidSize;
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
        /// Size of cells in this block state view.
        /// </summary>
        public Size CellSize { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Measure all cells in this block state view.
        /// </summary>
        public void MeasureCells()
        {
            Debug.Assert(RootCellView != null);
            RootCellView.Measure();

            CellSize = RootCellView.CellSize;

            Debug.Assert(MeasureHelper.IsValid(CellSize));
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

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        private protected override IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutBlockStateView));
            return new LayoutEmptyCellView((ILayoutNodeStateView)stateView);
        }
        #endregion
    }
}
