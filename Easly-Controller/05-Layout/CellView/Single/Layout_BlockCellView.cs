namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Controller;
    using EaslyController.Focus;

    /// <summary>
    /// A cell view for a block state.
    /// </summary>
    public interface ILayoutBlockCellView : IFocusBlockCellView, ILayoutCellView
    {
        /// <summary>
        /// The collection of cell views containing this view.
        /// </summary>
        new ILayoutCellViewCollection ParentCellView { get; }

        /// <summary>
        /// The block state view of the state associated to this cell.
        /// </summary>
        new ILayoutBlockStateView BlockStateView { get; }
    }

    /// <summary>
    /// A leaf of the cell view tree for a child state.
    /// </summary>
    internal class LayoutBlockCellView : FocusBlockCellView, ILayoutBlockCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBlockCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view.</param>
        /// <param name="blockStateView">The block state view of the state associated to this cell.</param>
        public LayoutBlockCellView(ILayoutNodeStateView stateView, ILayoutCellViewCollection parentCellView, ILayoutBlockStateView blockStateView)
            : base(stateView, parentCellView, blockStateView)
        {
            CellOrigin = ArrangeHelper.InvalidOrigin;
            CellSize = MeasureHelper.InvalidSize;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The collection of cell views containing this view.
        /// </summary>
        public new ILayoutCellViewCollection ParentCellView { get { return (ILayoutCellViewCollection)base.ParentCellView; } }

        /// <summary>
        /// The block state view of the state associated to this cell.
        /// </summary>
        public new ILayoutBlockStateView BlockStateView { get { return (ILayoutBlockStateView)base.BlockStateView; } }

        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public new ILayoutNodeStateView StateView { get { return (ILayoutNodeStateView)base.StateView; } }

        /// <summary>
        /// Location of the cell.
        /// </summary>
        public Point CellOrigin { get; private set; }

        /// <summary>
        /// Size of the cell.
        /// </summary>
        public Size CellSize { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Measures the cell.
        /// </summary>
        public virtual void Measure()
        {
            Debug.Assert(BlockStateView != null);
            BlockStateView.MeasureCells();

            CellSize = BlockStateView.CellSize;

            Debug.Assert(MeasureHelper.IsValid(CellSize));
        }

        /// <summary>
        /// Arranges the cell.
        /// </summary>
        public virtual void Arrange(Point origin)
        {
            Debug.Assert(BlockStateView != null);
            BlockStateView.ArrangeCells(origin);

            CellOrigin = BlockStateView.CellOrigin;

            Debug.Assert(ArrangeHelper.IsValid(CellOrigin));
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ILayoutCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutBlockCellView AsBlockCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBlockCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
