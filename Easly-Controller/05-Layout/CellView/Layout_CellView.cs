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
        /// The collection that can add separators around this item.
        /// </summary>
        ILayoutCellViewCollection CollectionWithSeparator { get; }

        /// <summary>
        /// The reference when displaying separators.
        /// </summary>
        ILayoutCellView ReferenceContainer { get; }

        /// <summary>
        /// The length of the separator.
        /// </summary>
        double SeparatorLength { get; }

        /// <summary>
        /// Measures the cell.
        /// </summary>
        /// <param name="collectionWithSeparator">A collection that can draw separators around the cell.</param>
        /// <param name="referenceContainer">The cell view in <paramref name="collectionWithSeparator"/> that contains this cell.</param>
        /// <param name="separatorLength">The length of the separator in <paramref name="collectionWithSeparator"/>.</param>
        void Measure(ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer, double separatorLength);

        /// <summary>
        /// Arranges the cell.
        /// </summary>
        /// <param name="collectionWithSeparator">A collection that can draw separators around the cell.</param>
        /// <param name="referenceContainer">The cell view in <paramref name="collectionWithSeparator"/> that contains this cell.</param>
        /// <param name="separatorLength">The length of the separator in <paramref name="collectionWithSeparator"/>.</param>
        /// <param name="origin">The cell location.</param>
        void Arrange(ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer, double separatorLength, Point origin);
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

        /// <summary>
        /// The collection that can add separators around this item.
        /// </summary>
        public ILayoutCellViewCollection CollectionWithSeparator { get; private set; }

        /// <summary>
        /// The reference when displaying separators.
        /// </summary>
        public ILayoutCellView ReferenceContainer { get; private set; }

        /// <summary>
        /// The length of the separator.
        /// </summary>
        public double SeparatorLength { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Measures the cell.
        /// </summary>
        /// <param name="collectionWithSeparator">A collection that can draw separators around the cell.</param>
        /// <param name="referenceContainer">The cell view in <paramref name="collectionWithSeparator"/> that contains this cell.</param>
        /// <param name="separatorLength">The length of the separator in <paramref name="collectionWithSeparator"/>.</param>
        public abstract void Measure(ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer, double separatorLength);

        /// <summary>
        /// Arranges the cell.
        /// </summary>
        /// <param name="collectionWithSeparator">A collection that can draw separators around the cell.</param>
        /// <param name="referenceContainer">The cell view in <paramref name="collectionWithSeparator"/> that contains this cell.</param>
        /// <param name="separatorLength">The length of the separator in <paramref name="collectionWithSeparator"/>.</param>
        /// <param name="origin">The cell location.</param>
        public abstract void Arrange(ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer, double separatorLength, Point origin);
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
