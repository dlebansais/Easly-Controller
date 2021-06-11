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
        /// Floating size of the cell.
        /// </summary>
        Size CellSize { get; }

        /// <summary>
        /// Padding inside the cell.
        /// </summary>
        Padding CellPadding { get; }

        /// <summary>
        /// Actual size of the cell.
        /// </summary>
        Size ActualCellSize { get; }

        /// <summary>
        /// Rectangular region for the cell.
        /// </summary>
        Rect CellRect { get; }

        /// <summary>
        /// The collection that can add separators around this item.
        /// </summary>
        ILayoutCellViewCollection CollectionWithSeparator { get; }

        /// <summary>
        /// The reference when displaying separators.
        /// </summary>
        ILayoutCellView ReferenceContainer { get; }

        /// <summary>
        /// The separator measure.
        /// </summary>
        Measure SeparatorLength { get; }

        /// <summary>
        /// Measures the cell.
        /// </summary>
        /// <param name="collectionWithSeparator">A collection that can draw separators around the cell.</param>
        /// <param name="referenceContainer">The cell view in <paramref name="collectionWithSeparator"/> that contains this cell.</param>
        /// <param name="separatorLength">The length of the separator in <paramref name="collectionWithSeparator"/>.</param>
        void Measure(ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer, Measure separatorLength);

        /// <summary>
        /// Arranges the cell.
        /// </summary>
        /// <param name="origin">The cell location.</param>
        void Arrange(Point origin);

        /// <summary>
        /// Updates the actual size of the cell.
        /// </summary>
        void UpdateActualSize();

        /// <summary>
        /// Draws the cell.
        /// </summary>
        void Draw();

        /// <summary>
        /// Prints the cell.
        /// </summary>
        /// <param name="origin">The origin from where to start printing.</param>
        void Print(Point origin);
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
            CellOrigin = RegionHelper.InvalidOrigin;
            CellSize = RegionHelper.InvalidSize;
            ActualCellSize = RegionHelper.InvalidSize;
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
        /// Floating size of the cell.
        /// </summary>
        public Size CellSize { get; }

        /// <summary>
        /// Padding inside the cell.
        /// </summary>
        public Padding CellPadding { get; }

        /// <summary>
        /// Actual size of the cell.
        /// </summary>
        public Size ActualCellSize { get; private set; }

        /// <summary>
        /// Rectangular region for the cell.
        /// </summary>
        public Rect CellRect { get { return new Rect(CellOrigin, ActualCellSize); } }

        /// <summary>
        /// The collection that can add separators around this item.
        /// </summary>
        public ILayoutCellViewCollection CollectionWithSeparator { get; private set; }

        /// <summary>
        /// The reference when displaying separators.
        /// </summary>
        public ILayoutCellView ReferenceContainer { get; private set; }

        /// <summary>
        /// The separator measure.
        /// </summary>
        public Measure SeparatorLength { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Measures the cell.
        /// </summary>
        /// <param name="collectionWithSeparator">A collection that can draw separators around the cell.</param>
        /// <param name="referenceContainer">The cell view in <paramref name="collectionWithSeparator"/> that contains this cell.</param>
        /// <param name="separatorLength">The length of the separator in <paramref name="collectionWithSeparator"/>.</param>
        public abstract void Measure(ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer, Measure separatorLength);

        /// <summary>
        /// Arranges the cell.
        /// </summary>
        /// <param name="origin">The cell location.</param>
        public abstract void Arrange(Point origin);

        /// <summary>
        /// Updates the actual size of the cell.
        /// </summary>
        public abstract void UpdateActualSize();

        /// <summary>
        /// Draws the cell.
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// Prints the cell.
        /// </summary>
        /// <param name="origin">The origin from where to start printing.</param>
        public abstract void Print(Point origin);
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="LayoutCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public abstract override bool IsEqual(CompareEqual comparer, IEqualComparable other);
        #endregion
    }
}
