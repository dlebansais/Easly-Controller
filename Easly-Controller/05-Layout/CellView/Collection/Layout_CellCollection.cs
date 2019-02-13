namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Controller;
    using EaslyController.Focus;

    /// <summary>
    /// Base interface for collection of cell views.
    /// </summary>
    public interface ILayoutCellViewCollection : IFocusCellViewCollection, ILayoutCellView, ILayoutAssignableCellView
    {
        /// <summary>
        /// The collection of child cells.
        /// </summary>
        new ILayoutCellViewList CellViewList { get; }
    }

    /// <summary>
    /// Base interface for collection of cell views.
    /// </summary>
    internal abstract class LayoutCellViewCollection : FocusCellViewCollection, ILayoutCellViewCollection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutCellViewCollection"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        /// <param name="cellViewList">The list of child cell views.</param>
        public LayoutCellViewCollection(ILayoutNodeStateView stateView, ILayoutCellViewCollection parentCellView, ILayoutCellViewList cellViewList)
            : base(stateView, parentCellView, cellViewList)
        {
            CellOrigin = ArrangeHelper.InvalidOrigin;
            CellSize = MeasureHelper.InvalidSize;
            CellPadding = Padding.Empty;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The collection of child cells.
        /// </summary>
        public new ILayoutCellViewList CellViewList { get { return (ILayoutCellViewList)base.CellViewList; } }

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

        /// <summary>
        /// Padding inside the cell.
        /// </summary>
        public Padding CellPadding { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Measures the cell.
        /// </summary>
        public abstract void Measure();

        /// <summary>
        /// Arranges the cell.
        /// </summary>
        public abstract void Arrange(Point origin);
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

            if (!comparer.IsSameType(other, out LayoutCellViewCollection AsCellViewCollection))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsCellViewCollection))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
