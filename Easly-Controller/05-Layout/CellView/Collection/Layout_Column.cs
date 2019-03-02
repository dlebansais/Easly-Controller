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
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        /// <param name="cellViewList">The list of child cell views.</param>
        /// <param name="frame">The frame that was used to create this cell. Can be null.</param>
        public LayoutColumn(ILayoutNodeStateView stateView, ILayoutCellViewCollection parentCellView, ILayoutCellViewList cellViewList, ILayoutFrame frame)
            : base(stateView, parentCellView, cellViewList, frame)
        {
            Debug.Assert(frame is ILayoutFrameWithVerticalSeparator);

            CellOrigin = RegionHelper.InvalidOrigin;
            CellSize = RegionHelper.InvalidSize;
            ActualCellSize = RegionHelper.InvalidSize;
            CellPadding = Padding.Empty;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The collection of child cells.
        /// </summary>
        public new ILayoutCellViewList CellViewList { get { return (ILayoutCellViewList)base.CellViewList; } }

        /// <summary>
        /// The collection of cell views containing this view. Null for the root of the cell tree.
        /// </summary>
        public new ILayoutCellViewCollection ParentCellView { get { return (ILayoutCellViewCollection)base.ParentCellView; } }

        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public new ILayoutNodeStateView StateView { get { return (ILayoutNodeStateView)base.StateView; } }

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
        public Rect CellRect { get { return new Rect(CellOrigin, ActualCellSize); } }

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

            double LeftPadding = (Frame is ILayoutVerticalTabulatedFrame AsTabulatedFrame && AsTabulatedFrame.HasTabulationMargin) ? DrawContext.TabulationWidth : 0;

            ILayoutFrameWithVerticalSeparator AsFrameWithVerticalSeparator = Frame as ILayoutFrameWithVerticalSeparator;
            Debug.Assert(AsFrameWithVerticalSeparator != null);

            bool OverrideReferenceContainer = false;

            // Ensures arguments of Arrange() are valid. This only happens for the root cell view, where there is no separator.
            if (collectionWithSeparator == null && referenceContainer == null)
            {
                collectionWithSeparator = this;
                OverrideReferenceContainer = true;
                separatorLength = 0;
            }

            double Width = double.NaN;
            double Height = 0;
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
                        separatorLength = DrawContext.GetVerticalSeparatorHeight(AsFrameWithVerticalSeparator.Separator);
                    }

                    Height += separatorLength;
                }

                if (OverrideReferenceContainer)
                    referenceContainer = CellView;

                CellView.Measure(collectionWithSeparator, referenceContainer, separatorLength);

                Size NestedCellSize = CellView.CellSize;
                Debug.Assert(RegionHelper.IsValid(NestedCellSize));

                bool IsFixed = RegionHelper.IsFixed(NestedCellSize);
                bool IsStretched = RegionHelper.IsStretchedHorizontally(NestedCellSize);
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

            if (Height != 0)
                AccumulatedSize = new Size(Width + LeftPadding, Height);

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

            double LeftPadding = (Frame is ILayoutVerticalTabulatedFrame AsTabulatedFrame && AsTabulatedFrame.HasTabulationMargin) ? DrawContext.TabulationWidth : 0;

            double OriginX = origin.X + LeftPadding;
            double OriginY = origin.Y;

            for (int i = 0; i < CellViewList.Count; i++)
            {
                ILayoutCellView CellView = CellViewList[i];

                // The separator length has been calculated in Measure().
                if (i > 0)
                    OriginY += CellView.SeparatorLength;

                Point LineOrigin = new Point(OriginX, OriginY);
                CellView.Arrange(LineOrigin);

                Size NestedCellSize = CellView.CellSize;
                Debug.Assert(!double.IsNaN(NestedCellSize.Height));
                OriginY += NestedCellSize.Height;
            }

            Point FinalOrigin = new Point(OriginX, OriginY);
            Point ExpectedOrigin = CellOrigin.Moved(LeftPadding, CellSize.Height);
            bool IsEqual = Point.IsEqual(FinalOrigin, ExpectedOrigin);
            Debug.Assert(IsEqual);
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

            Debug.Assert(Size.IsEqual(CellRect.Size, ActualCellSize));
        }

        /// <summary>
        /// Draws the cell.
        /// </summary>
        public virtual void Draw()
        {
            Debug.Assert(RegionHelper.IsFixed(ActualCellSize));

            DrawSelection();

            foreach (ILayoutCellView CellView in CellViewList)
                CellView.Draw();
        }

        /// <summary></summary>
        protected virtual void DrawSelection()
        {
            if (Frame is ILayoutListFrame AsListFrame &&
                StateView.ControllerView.Selection is ILayoutListNodeSelection AsListNodeSelection &&
                AsListNodeSelection.StateView == StateView &&
                AsListFrame.PropertyName == AsListNodeSelection.PropertyName)
            {
                int StartIndex = AsListNodeSelection.StartIndex;
                int EndIndex = AsListNodeSelection.EndIndex;

                if (StartIndex > EndIndex)
                {
                    int n = EndIndex;
                    EndIndex = StartIndex;
                    StartIndex = n;
                }

                Debug.Assert(StartIndex >= 0 && StartIndex < CellViewList.Count);
                Debug.Assert(EndIndex >= 0 && EndIndex < CellViewList.Count);
                Debug.Assert(StartIndex <= EndIndex);

                double Y = CellViewList[StartIndex].CellOrigin.Y;
                double Height = CellViewList[EndIndex].CellOrigin.Y + CellViewList[EndIndex].ActualCellSize.Height - Y;

                Rect SelectionRect = new Rect(CellOrigin.X, Y, ActualCellSize.Width, Height);

                Debug.Assert(StateView != null);
                Debug.Assert(StateView.ControllerView != null);

                ILayoutDrawContext DrawContext = StateView.ControllerView.DrawContext;
                Debug.Assert(DrawContext != null);

                DrawContext.DrawSelectionRectangle(SelectionRect);
            }
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
                drawContext.DrawVerticalSeparator(((ILayoutFrameWithVerticalSeparator)Frame).Separator, cellView.CellOrigin, cellView.CellSize.Width);
        }

        /// <summary>
        /// Returns the measured size of streched cells in the collection.
        /// </summary>
        /// <param name="size">The cell size.</param>
        public virtual Size GetMeasuredSize(Size size)
        {
            Debug.Assert(!double.IsNaN(size.Height));

            double Width = size.Width;
            if (double.IsNaN(Width))
                Width = CellSize.Width;

            if (!double.IsNaN(Width))
            {
                Size MeasuredSize = new Size(Width, size.Height);
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

            if (!comparer.IsSameType(other, out LayoutColumn AsColumn))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsColumn))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
