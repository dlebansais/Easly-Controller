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
            CellPadding = Padding.Empty;
            ActualCellSize = RegionHelper.InvalidSize;
            CellCorner = RegionHelper.InvalidCorner;
            CellPlane = RegionHelper.InvalidPlane;
            CellSpacePadding = SpacePadding.Empty;
            ActualCellPlane = RegionHelper.InvalidPlane;
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
        /// Padding inside the cell.
        /// </summary>
        public Padding CellPadding { get; private set; }

        /// <summary>
        /// Actual size of the cell.
        /// </summary>
        public Size ActualCellSize { get; private set; }

        /// <summary>
        /// Rectangular region for the cell.
        /// </summary>
        public Rect CellRect { get { return new Rect(CellOrigin, ActualCellSize); } }

        /// <summary>
        /// Location of the cell.
        /// </summary>
        public Corner CellCorner { get; private set; }

        /// <summary>
        /// Floating size of the cell.
        /// </summary>
        public Plane CellPlane { get; private set; }

        /// <summary>
        /// Padding inside the cell.
        /// </summary>
        public SpacePadding CellSpacePadding { get; private set; }

        /// <summary>
        /// Actual size of the cell.
        /// </summary>
        public Plane ActualCellPlane { get; private set; }

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
        public SeparatorLength SeparatorLength { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Measures the cell.
        /// </summary>
        /// <param name="collectionWithSeparator">A collection that can draw separators around the cell.</param>
        /// <param name="referenceContainer">The cell view in <paramref name="collectionWithSeparator"/> that contains this cell.</param>
        /// <param name="separatorLength">The length of the separator in <paramref name="collectionWithSeparator"/>.</param>
        public virtual void Measure(ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer, SeparatorLength separatorLength)
        {
            CollectionWithSeparator = collectionWithSeparator;
            ReferenceContainer = referenceContainer;
            SeparatorLength = separatorLength;

            Debug.Assert(StateView != null);
            Debug.Assert(StateView.ControllerView != null);

            ILayoutMeasureContext MeasureContext = StateView.ControllerView.MeasureContext;
            Debug.Assert(MeasureContext != null);

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
                        separatorLength = MeasureContext.GetHorizontalSeparatorWidth(AsFrameWithHorizontalSeparator.Separator);
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

            ILayoutMeasureContext MeasureContext = StateView.ControllerView.MeasureContext;
            Debug.Assert(MeasureContext != null);

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
            if (Frame is ILayoutNamedFrame AsContentFrame && StateView.ControllerView.Selection is IFocusContentSelection AsContentSelection && AsContentSelection.StateView == StateView && AsContentFrame.PropertyName == AsContentSelection.PropertyName)
            {
                if (AsContentFrame is ILayoutListFrame AsListFrame && AsContentSelection is ILayoutListNodeSelection AsListNodeSelection)
                    DrawSelection(AsListNodeSelection.StartIndex, AsListNodeSelection.EndIndex);

                else if (AsContentFrame is ILayoutBlockListFrame AsBlockListFrame)
                {
                    if (AsContentSelection is ILayoutBlockListNodeSelection AsBlockListNodeSelection)
                        DrawBlockListNodeSelection(AsBlockListNodeSelection);

                    else if (AsContentSelection is ILayoutBlockSelection AsBlockSelection)
                        DrawSelection(AsBlockSelection.StartIndex, AsBlockSelection.EndIndex);
                }
            }
        }

        /// <summary>
        /// Draws a selection rectangle around cells.
        /// </summary>
        /// <param name="startIndex">Index of the first cell in the selection.</param>
        /// <param name="endIndex">Index of the last cell in the selection.</param>
        public virtual void DrawSelection(int startIndex, int endIndex)
        {
            if (startIndex > endIndex)
            {
                int n = endIndex;
                endIndex = startIndex;
                startIndex = n;
            }

            Debug.Assert(startIndex >= 0 && startIndex < CellViewList.Count);
            Debug.Assert(endIndex >= 0 && endIndex < CellViewList.Count);
            Debug.Assert(startIndex <= endIndex);

            Rect SelectionRect = GetSelectedRect(startIndex, endIndex);

            Debug.Assert(StateView != null);
            Debug.Assert(StateView.ControllerView != null);

            ILayoutDrawContext DrawContext = StateView.ControllerView.DrawContext;
            Debug.Assert(DrawContext != null);

            DrawContext.DrawSelectionRectangle(SelectionRect);
        }

        /// <summary></summary>
        protected virtual void DrawBlockListNodeSelection(ILayoutBlockListNodeSelection selection)
        {
            int BlockIndex = selection.BlockIndex;
            Debug.Assert(BlockIndex < CellViewList.Count);

            ILayoutBlockCellView BlockCellView = CellViewList[BlockIndex] as ILayoutBlockCellView;
            Debug.Assert(BlockCellView != null);

            BlockCellView.DrawBlockListNodeSelection(selection);
        }

        /// <summary></summary>
        protected virtual Rect GetSelectedRect(int startIndex, int endIndex)
        {
            double X = CellViewList[startIndex].CellOrigin.X;
            double Width = CellViewList[endIndex].CellOrigin.X + CellViewList[endIndex].ActualCellSize.Width - X;

            return new Rect(X, CellOrigin.Y, Width, ActualCellSize.Height);
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

        /// <summary>
        /// Updates the actual size of the cell.
        /// </summary>
        public virtual void UpdateActualPlane()
        {
            ActualCellPlane = CellPlane;
            if (ParentCellView != null)
                ActualCellPlane = ParentCellView.GetMeasuredPlane(CellPlane);

            foreach (ILayoutCellView CellView in CellViewList)
            {
                CellView.UpdateActualPlane();
                Debug.Assert(RegionHelper.IsFixed(CellView.ActualCellPlane));
            }
        }

        /// <summary>
        /// Returns the measured size of streched cells in the collection.
        /// </summary>
        /// <param name="plane">The cell size.</param>
        public virtual Plane GetMeasuredPlane(Plane plane)
        {
            Debug.Assert(plane.Width >= 0);

            int Height = plane.Height;
            if (Height < 0)
                Height = CellPlane.Height;

            if (Height >= 0)
            {
                Plane MeasuredPlane = new Plane(plane.Width, Height);
                return MeasuredPlane;
            }
            else
                return ParentCellView.GetMeasuredPlane(plane);
        }

        /// <summary>
        /// Prints the cell.
        /// </summary>
        public virtual void Print()
        {
            Debug.Assert(RegionHelper.IsFixed(ActualCellPlane));

            foreach (ILayoutCellView CellView in CellViewList)
                CellView.Print();
        }

        /// <summary>
        /// Prints container or separator before an element of a collection.
        /// </summary>
        /// <param name="printContext">The context used to print the cell.</param>
        /// <param name="cellView">The cell to print.</param>
        /// <param name="corner">The location where to start printing.</param>
        /// <param name="plane">The printing size, padding included.</param>
        /// <param name="padding">The padding to use when printing.</param>
        public virtual void PrintBeforeItem(ILayoutPrintContext printContext, ILayoutCellView cellView, Corner corner, Plane plane, SpacePadding padding)
        {
            int Index = CellViewList.IndexOf(cellView);
            Debug.Assert(Index >= 0 && Index < CellViewList.Count);

            if (Index > 0)
                printContext.PrintHorizontalSeparator(((ILayoutFrameWithHorizontalSeparator)Frame).Separator, cellView.CellCorner, cellView.CellPlane.Height);
        }

        /// <summary>
        /// Prints container or separator after an element of a collection.
        /// </summary>
        /// <param name="printContext">The context used to print the cell.</param>
        /// <param name="cellView">The cell to print.</param>
        /// <param name="corner">The location where to start printing.</param>
        /// <param name="plane">The printing size, padding included.</param>
        /// <param name="padding">The padding to use when printing.</param>
        public virtual void PrintAfterItem(ILayoutPrintContext printContext, ILayoutCellView cellView, Corner corner, Plane plane, SpacePadding padding)
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
