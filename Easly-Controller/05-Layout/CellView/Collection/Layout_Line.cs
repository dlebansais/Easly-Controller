namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Controller;
    using EaslyController.Focus;

    /// <summary>
    /// A collection of cell views organized in a column.
    /// </summary>
    public interface ILayoutLine : IFocusLine, ILayoutCellViewCollection
    {
    }

    /// <summary>
    /// A collection of cell views organized in a column.
    /// </summary>
    internal class LayoutLine : FocusLine, ILayoutLine
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutLine"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="cellViewList">The list of child cell views.</param>
        public LayoutLine(ILayoutNodeStateView stateView, ILayoutCellViewList cellViewList)
            : base(stateView, cellViewList)
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
        public virtual void Measure()
        {
            double Width = 0;
            double Height = double.NaN;

            foreach (ILayoutCellView CellView in CellViewList)
            {
                CellView.Measure();

                Size NestedCellSize = CellView.CellSize;
                Debug.Assert(MeasureHelper.IsValid(NestedCellSize));

                bool IsFixed = MeasureHelper.IsFixed(NestedCellSize);
                bool IsStretched = MeasureHelper.IsStretchedVertically(NestedCellSize);
                Debug.Assert(IsFixed || IsStretched);

                Debug.Assert(!double.IsNaN(NestedCellSize.Width));
                Width += NestedCellSize.Width;

                if (IsFixed)
                {
                    Debug.Assert(!double.IsNaN(NestedCellSize.Height));
                    if (double.IsNaN(Height) || Height < NestedCellSize.Height)
                        Height = NestedCellSize.Height;
                }
            }

            if (Width == 0)
                CellSize = Size.Empty;
            else
                CellSize = new Size(Width, Height);

            Debug.Assert(MeasureHelper.IsValid(CellSize));
        }

        /// <summary>
        /// Arranges the cell.
        /// </summary>
        public virtual void Arrange(Point origin)
        {
            CellOrigin = origin;

            Point ColumnOrigin = origin;

            foreach (ILayoutCellView CellView in CellViewList)
            {
                CellView.Arrange(ColumnOrigin);

                Size NestedCellSize = CellView.CellSize;
                Debug.Assert(!double.IsNaN(NestedCellSize.Width));
                ColumnOrigin.X += NestedCellSize.Width;
            }

            Point FinalOrigin = new Point(ColumnOrigin.X, ColumnOrigin.Y);
            Point ExpectedOrigin = new Point(CellOrigin.X + CellSize.Width, CellOrigin.Y);
            bool IsEqual = Point.IsEqual(FinalOrigin, ExpectedOrigin);
            Debug.Assert(IsEqual);
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

            if (!comparer.IsSameType(other, out LayoutLine AsLine))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsLine))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
