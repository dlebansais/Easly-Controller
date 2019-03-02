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
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        /// <param name="cellViewList">The list of child cell views.</param>
        /// <param name="frame">Frame providing the horizontal separator to insert between cells. Can be null.</param>
        public LayoutLine(ILayoutNodeStateView stateView, ILayoutCellViewCollection parentCellView, ILayoutCellViewList cellViewList, ILayoutFrame frame)
            : base(stateView, parentCellView, cellViewList, frame)
        {
            Debug.Assert(frame is ILayoutFrameWithHorizontalSeparator);

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
        /// The collection of cell views containing this view. Null for the root of the cell tree.
        /// </summary>
        public new ILayoutCellViewCollection ParentCellView { get { return (ILayoutCellViewCollection)base.ParentCellView; } }

        /// <summary>
        /// The collection of child cells.
        /// </summary>
        public new ILayoutCellViewList CellViewList { get { return (ILayoutCellViewList)base.CellViewList; } }

        /// <summary>
        /// The frame that was used to create this cell. Can be null.
        /// </summary>
        public new ILayoutFrame Frame { get { return (ILayoutFrame)base.Frame; } }

        /// <summary>
        /// Location of the cell.
        /// </summary>
        public Point CellOrigin { get; private set; }

        /// <summary>
        /// Floating size of the cell.
        /// </summary>
        public Size CellSize { get; private set; }

        /// <summary>
        /// Actual size of the cell.
        /// </summary>
        public Size ActualCellSize { get; private set; }

        /// <summary>
        /// Rectangular region for the cell.
        /// </summary>
        public Rect CellRect { get { return new Rect(CellOrigin, CellSize); } }

        /// <summary>
        /// Padding inside the cell.
        /// </summary>
        public Padding CellPadding { get; private set; }

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
        public virtual void Measure(ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer, double separatorLength)
        {
            CollectionWithSeparator = collectionWithSeparator;
            ReferenceContainer = referenceContainer;
            SeparatorLength = separatorLength;

            Debug.Assert(StateView != null);
            Debug.Assert(StateView.ControllerView != null);

            ILayoutDrawContext DrawContext = StateView.ControllerView.DrawContext;
            Debug.Assert(DrawContext != null);

            ILayoutFrameWithHorizontalSeparator AsFrameWithHorizontalSeparator = Frame as ILayoutFrameWithHorizontalSeparator;
            Debug.Assert(AsFrameWithHorizontalSeparator != null);

            bool OverrideReferenceContainer = false;

            // Ensures arguments of Arrange() are valid. This only happens for the root cell view, where there is no separator.
            if (collectionWithSeparator == null && referenceContainer == null)
            {
                collectionWithSeparator = this;
                OverrideReferenceContainer = true;
                separatorLength = 0;
            }

            double Width = 0;
            double Height = double.NaN;
            Size AccumulatedSize = Size.Empty;

            for (int i = 0; i < CellViewList.Count; i++)
            {
                ILayoutCellView CellView = CellViewList[i];

                if (i > 0)
                {
                    // Starting with the second cell, we use the separator of our frame.
                    if (i == 1)
                    {
                        collectionWithSeparator = this;
                        OverrideReferenceContainer = true;
                        separatorLength = DrawContext.GetHorizontalSeparatorWidth(AsFrameWithHorizontalSeparator.Separator);
                    }

                    Width += separatorLength;
                }

                if (OverrideReferenceContainer)
                    referenceContainer = CellView;

                Debug.Assert(collectionWithSeparator != this || CellViewList.IndexOf(referenceContainer) == i);
                CellView.Measure(collectionWithSeparator, referenceContainer, separatorLength);

                Size NestedCellSize = CellView.CellSize;
                Debug.Assert(RegionHelper.IsValid(NestedCellSize));

                bool IsFixed = RegionHelper.IsFixed(NestedCellSize);
                bool IsStretched = RegionHelper.IsStretchedVertically(NestedCellSize);
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

            if (Width != 0)
                AccumulatedSize = new Size(Width, Height);

            CellSize = AccumulatedSize;
            ActualCellSize = RegionHelper.InvalidSize;

            Debug.Assert(RegionHelper.IsValid(CellSize));
        }

        /// <summary>
        /// Arranges the cell.
        /// </summary>
        /// <param name="origin">The cell location.</param>
        public virtual void Arrange(Point origin)
        {
            CellOrigin = origin;

            Debug.Assert(StateView != null);
            Debug.Assert(StateView.ControllerView != null);

            ILayoutDrawContext DrawContext = StateView.ControllerView.DrawContext;
            Debug.Assert(DrawContext != null);

            double OriginX = origin.X;
            double OriginY = origin.Y;

            for (int i = 0; i < CellViewList.Count; i++)
            {
                ILayoutCellView CellView = CellViewList[i];

                // The separator length has been calculated in Measure().
                if (i > 0)
                    OriginX += CellView.SeparatorLength;

                Point ColumnOrigin = new Point(OriginX, OriginY);
                CellView.Arrange(ColumnOrigin);

                Size NestedCellSize = CellView.CellSize;
                Debug.Assert(!double.IsNaN(NestedCellSize.Width));
                OriginX += NestedCellSize.Width;
            }

            Point FinalOrigin = new Point(OriginX, OriginY);
            Point ExpectedOrigin = CellOrigin.Moved(CellSize.Width, 0);
            bool IsEqual = Point.IsEqual(FinalOrigin, ExpectedOrigin);
            Debug.Assert(IsEqual);
            Debug.Assert(Size.IsEqual(CellRect.Size, CellSize));
        }

        /// <summary>
        /// Updates the actual size of the cell.
        /// </summary>
        public virtual void UpdateActualSize()
        {
            ActualCellSize = CellSize;
            if (ParentCellView != null)
                ActualCellSize = ParentCellView.GetMeasuredSize(CellSize);

            foreach (ILayoutCellView CellView in CellViewList)
            {
                CellView.UpdateActualSize();
                Debug.Assert(RegionHelper.IsFixed(CellView.ActualCellSize));
            }
        }

        /// <summary>
        /// Draws the cell.
        /// </summary>
        public virtual void Draw()
        {
            Debug.Assert(RegionHelper.IsFixed(ActualCellSize));

            foreach (ILayoutCellView CellView in CellViewList)
                CellView.Draw();
        }

        /// <summary>
        /// Draws container or separator before an element of a collection.
        /// </summary>
        /// <param name="drawContext">The context used to draw the cell.</param>
        /// <param name="cellView">The cell to draw.</param>
        /// <param name="origin">The location where to start drawing.</param>
        /// <param name="size">The drawing size, padding included.</param>
        /// <param name="padding">The padding to use when drawing.</param>
        public virtual void DrawBeforeItem(ILayoutDrawContext drawContext, ILayoutCellView cellView, Point origin, Size size, Padding padding)
        {
            int Index = CellViewList.IndexOf(cellView);
            Debug.Assert(Index >= 0 && Index < CellViewList.Count);

            if (Index > 0)
                drawContext.DrawHorizontalSeparator(((ILayoutFrameWithHorizontalSeparator)Frame).Separator, cellView.CellOrigin, cellView.CellSize.Height);
        }

        /// <summary>
        /// Returns the measured size of streched cells in the collection.
        /// </summary>
        /// <param name="size">The cell size.</param>
        public virtual Size GetMeasuredSize(Size size)
        {
            Debug.Assert(!double.IsNaN(size.Width));

            double Height = size.Height;
            if (double.IsNaN(Height))
                Height = CellSize.Height;

            if (!double.IsNaN(Height))
            {
                Size MeasuredSize = new Size(size.Width, Height);
                return MeasuredSize;
            }
            else
                return ParentCellView.GetMeasuredSize(size);
        }

        /// <summary>
        /// Draws container or separator after an element of a collection.
        /// </summary>
        /// <param name="drawContext">The context used to draw the cell.</param>
        /// <param name="cellView">The cell to draw.</param>
        /// <param name="origin">The location where to start drawing.</param>
        /// <param name="size">The drawing size, padding included.</param>
        /// <param name="padding">The padding to use when drawing.</param>
        public virtual void DrawAfterItem(ILayoutDrawContext drawContext, ILayoutCellView cellView, Point origin, Size size, Padding padding)
        {
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
