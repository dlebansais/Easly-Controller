namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Controller;
    using EaslyController.Focus;

    /// <summary>
    /// Atomic cell view of a component in a node.
    /// </summary>
    public interface ILayoutCellView : IFocusCellView
    {
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        new ILayoutNodeStateView StateView { get; }

        /// <summary>
        /// The collection of cell views containing this view. Null for the root of the cell tree.
        /// </summary>
        new ILayoutCellViewCollection ParentCellView { get; }

        /// <summary>
        /// Location of the cell.
        /// </summary>
        Point CellOrigin { get; }

        /// <summary>
        /// Size of the cell.
        /// </summary>
        Size CellSize { get; }

        /// <summary>
        /// Padding inside the cell.
        /// </summary>
        Padding CellPadding { get; }

        /// <summary>
        /// Measures the cell.
        /// </summary>
        void Measure();

        /// <summary>
        /// Arranges the cell.
        /// </summary>
        void Arrange(Point origin, ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer);
    }

    /// <summary>
    /// Atomic cell view of a component in a node.
    /// </summary>
    internal abstract class LayoutCellView : FocusCellView, ILayoutCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        public LayoutCellView(ILayoutNodeStateView stateView, ILayoutCellViewCollection parentCellView)
            : base(stateView, parentCellView)
        {
            CellOrigin = ArrangeHelper.InvalidOrigin;
            CellSize = MeasureHelper.InvalidSize;
            CellPadding = Padding.Empty;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public new ILayoutNodeStateView StateView { get { return (ILayoutNodeStateView)base.StateView; } }

        /// <summary>
        /// The collection of cell views containing this view.
        /// </summary>
        public new ILayoutCellViewCollection ParentCellView { get { return (ILayoutCellViewCollection)base.ParentCellView; } }

        /// <summary>
        /// Location of the cell.
        /// </summary>
        public Point CellOrigin { get; }

        /// <summary>
        /// Size of the cell.
        /// </summary>
        public Size CellSize { get; }

        /// <summary>
        /// Padding inside the cell.
        /// </summary>
        public Padding CellPadding { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Measures the cell.
        /// </summary>
        public abstract void Measure();

        /// <summary>
        /// Arranges the cell.
        /// </summary>
        public abstract void Arrange(Point origin, ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer);
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

            if (!comparer.IsSameType(other, out LayoutCellView AsCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
