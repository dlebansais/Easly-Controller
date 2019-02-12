namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Controller;
    using EaslyController.Focus;

    /// <summary>
    /// A collection of cell views organized in a column.
    /// </summary>
    public interface ILayoutColumn : IFocusColumn, ILayoutCellViewCollection
    {
    }

    /// <summary>
    /// A collection of cell views organized in a column.
    /// </summary>
    internal class LayoutColumn : FocusColumn, ILayoutColumn
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutColumn"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="cellViewList">The list of child cell views.</param>
        public LayoutColumn(ILayoutNodeStateView stateView, ILayoutCellViewList cellViewList)
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
            double Width = double.NaN;
            double Height = 0;

            foreach (ILayoutCellView CellView in CellViewList)
            {
                CellView.Measure();

                Size NestedCellSize = CellView.CellSize;
                Debug.Assert(MeasureHelper.IsValid(NestedCellSize));

                bool IsFixed = MeasureHelper.IsFixed(NestedCellSize);
                bool IsStretched = MeasureHelper.IsStretchedHorizontally(NestedCellSize);
                Debug.Assert(IsFixed || IsStretched);

                Debug.Assert(!double.IsNaN(NestedCellSize.Height));
                Height += NestedCellSize.Height;

                if (IsFixed)
                {
                    Debug.Assert(!double.IsNaN(NestedCellSize.Width));
                    if (double.IsNaN(Width) || Width < NestedCellSize.Width)
                        Width = NestedCellSize.Width;
                }
            }

            if (Height == 0)
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

            Point LineOrigin = origin;

            foreach (ILayoutCellView CellView in CellViewList)
            {
                CellView.Arrange(LineOrigin);

                Size NestedCellSize = CellView.CellSize;
                Debug.Assert(!double.IsNaN(NestedCellSize.Height));
                LineOrigin.Y += NestedCellSize.Height;
            }

            Debug.Assert(LineOrigin.Y == CellOrigin.Y + CellSize.Height);
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

            if (!comparer.IsSameType(other, out LayoutColumn AsColumn))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsColumn))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
