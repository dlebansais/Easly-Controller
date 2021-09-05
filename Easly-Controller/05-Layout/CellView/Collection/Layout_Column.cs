namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Constants;
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
        public LayoutColumn(ILayoutNodeStateView stateView, ILayoutCellViewCollection parentCellView, LayoutCellViewList cellViewList, ILayoutFrame frame)
            : base(stateView, parentCellView, cellViewList, frame)
        {
            Debug.Assert(frame is ILayoutFrameWithVerticalSeparator);

            CellOrigin = RegionHelper.InvalidOrigin;
            CellSize = RegionHelper.InvalidSize;
            CellPadding = Padding.Empty;
            ActualCellSize = RegionHelper.InvalidSize;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The collection of child cells.
        /// </summary>
        public new LayoutCellViewList CellViewList { get { return (LayoutCellViewList)base.CellViewList; } }

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

        /// <summary>
        /// Indicates that block geometry must be drawn around a block.
        /// </summary>
        public bool HasBlockGeometry { get { return (Frame is ILayoutPanelFrame AsPanelFrame && AsPanelFrame.HasBlockGeometry) && StateView.ControllerView.ShowBlockGeometry; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Measures the cell.
        /// </summary>
        /// <param name="collectionWithSeparator">A collection that can draw separators around the cell.</param>
        /// <param name="referenceContainer">The cell view in <paramref name="collectionWithSeparator"/> that contains this cell.</param>
        /// <param name="separatorLength">The length of the separator in <paramref name="collectionWithSeparator"/>.</param>
        public virtual void Measure(ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer, Measure separatorLength)
        {
            CollectionWithSeparator = collectionWithSeparator;
            ReferenceContainer = referenceContainer;
            SeparatorLength = separatorLength;

            Debug.Assert(StateView != null);
            Debug.Assert(StateView.ControllerView != null);

            ILayoutMeasureContext MeasureContext = StateView.ControllerView.MeasureContext;
            Debug.Assert(MeasureContext != null);

            Measure LeftPadding = (Frame is ILayoutVerticalTabulatedFrame AsTabulatedFrame && AsTabulatedFrame.HasTabulationMargin) ? MeasureContext.TabulationWidth : Controller.Measure.Zero;

            ILayoutFrameWithVerticalSeparator AsFrameWithVerticalSeparator = Frame as ILayoutFrameWithVerticalSeparator;
            Debug.Assert(AsFrameWithVerticalSeparator != null);

            bool OverrideReferenceContainer = false;

            // Ensures arguments of Arrange() are valid. This only happens for the root cell view, where there is no separator.
            if (collectionWithSeparator == null && referenceContainer == null)
            {
                collectionWithSeparator = this;
                OverrideReferenceContainer = true;
                separatorLength = Controller.Measure.Zero;
            }

            Measure Width = Controller.Measure.Floating;
            Measure Height = Controller.Measure.Zero;

            for (int i = 0; i < CellViewList.Count; i++)
            {
                ILayoutCellView CellView = (ILayoutCellView)CellViewList[i];

                if (i > 0)
                {
                    // Starting with the second cell, we use the separator of our frame.
                    if (i == 1)
                    {
                        collectionWithSeparator = this;
                        OverrideReferenceContainer = true;
                        separatorLength = MeasureContext.GetVerticalSeparatorHeight(AsFrameWithVerticalSeparator.Separator);
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

                Debug.Assert(!NestedCellSize.Height.IsFloating);
                Height += NestedCellSize.Height;

                if (IsFixed)
                {
                    Debug.Assert(!NestedCellSize.Width.IsFloating);
                    if (Width.IsFloating || Width.Draw < NestedCellSize.Width.Draw)
                        Width = NestedCellSize.Width;
                }
            }

            Size AccumulatedSize = Size.Empty;

            if (!Height.IsZero)
                AccumulatedSize = new Size(Width + LeftPadding + (HasBlockGeometry ? MeasureContext.BlockGeometryWidth : Controller.Measure.Zero), Height);

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

            Measure LeftPadding = (Frame is ILayoutVerticalTabulatedFrame AsTabulatedFrame && AsTabulatedFrame.HasTabulationMargin) ? MeasureContext.TabulationWidth : Controller.Measure.Zero;

            Measure OriginX = origin.X + LeftPadding + (HasBlockGeometry ? MeasureContext.BlockGeometryWidth : Controller.Measure.Zero);
            Measure OriginY = origin.Y;

            for (int i = 0; i < CellViewList.Count; i++)
            {
                ILayoutCellView CellView = (ILayoutCellView)CellViewList[i];

                // The separator length has been calculated in Measure().
                if (i > 0)
                    OriginY += CellView.SeparatorLength;

                Point LineOrigin = new Point(OriginX, OriginY);
                CellView.Arrange(LineOrigin);

                Size NestedCellSize = CellView.CellSize;
                Debug.Assert(!NestedCellSize.Height.IsFloating);
                OriginY += NestedCellSize.Height;
            }

            Point FinalOrigin = new Point(OriginX, OriginY);
            Point ExpectedOrigin = CellOrigin.Moved(LeftPadding + (HasBlockGeometry ? MeasureContext.BlockGeometryWidth : Controller.Measure.Zero), CellSize.Height);
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
            Debug.Assert(!size.Height.IsFloating);

            Measure Width = size.Width;
            if (Width.IsFloating)
                Width = CellSize.Width;

            if (!Width.IsFloating)
            {
                Size MeasuredSize = new Size(Width, size.Height);
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
            DrawBlockGeometry();

            foreach (ILayoutCellView CellView in CellViewList)
                CellView.Draw();
        }

        /// <summary></summary>
        protected virtual void DrawSelection()
        {
            if (Frame is ILayoutNamedFrame AsContentFrame && StateView.ControllerView.Selection is IFocusContentSelection AsContentSelection && AsContentSelection.StateView == StateView && AsContentFrame.PropertyName == AsContentSelection.PropertyName)
            {
                if (AsContentFrame is ILayoutListFrame AsListFrame && AsContentSelection is ILayoutNodeListSelection AsNodeListSelection)
                    DrawSelection(AsNodeListSelection.StartIndex, AsNodeListSelection.EndIndex, SelectionStyles.NodeList);
                else if (AsContentFrame is ILayoutBlockListFrame AsBlockListFrame)
                {
                    if (AsContentSelection is ILayoutBlockNodeListSelection AsBlockNodeListSelection)
                        DrawBlockListNodeSelection(AsBlockNodeListSelection);
                    else if (AsContentSelection is ILayoutBlockListSelection AsBlockListSelection)
                        DrawSelection(AsBlockListSelection.StartIndex, AsBlockListSelection.EndIndex, SelectionStyles.BlockList);
                }
            }
        }

        /// <summary>
        /// Draws a selection rectangle around cells.
        /// </summary>
        /// <param name="startIndex">Index of the first cell in the selection.</param>
        /// <param name="endIndex">Index of the last cell in the selection.</param>
        /// <param name="selectionStyle">The style to use when drawing.</param>
        public virtual void DrawSelection(int startIndex, int endIndex, SelectionStyles selectionStyle)
        {
            Debug.Assert(startIndex >= 0 && startIndex < CellViewList.Count);
            Debug.Assert(endIndex >= 0 && endIndex < CellViewList.Count);
            Debug.Assert(startIndex <= endIndex);

            Rect SelectionRect = GetSelectedRect(startIndex, endIndex);

            Debug.Assert(StateView != null);
            Debug.Assert(StateView.ControllerView != null);

            ILayoutDrawContext DrawContext = StateView.ControllerView.DrawContext;
            Debug.Assert(DrawContext != null);

            DrawContext.DrawSelectionRectangle(SelectionRect, selectionStyle);
        }

        /// <summary></summary>
        protected virtual void DrawBlockListNodeSelection(ILayoutBlockNodeListSelection selection)
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
            double Y = ((ILayoutCellView)CellViewList[startIndex]).CellOrigin.Y.Draw;
            double Height = ((ILayoutCellView)CellViewList[endIndex]).CellOrigin.Y.Draw + ((ILayoutCellView)CellViewList[endIndex]).ActualCellSize.Height.Draw - Y;

            return new Rect(CellOrigin.X.Draw, Y, ActualCellSize.Width.Draw, Height);
        }

        /// <summary></summary>
        protected virtual void DrawBlockGeometry()
        {
            if (HasBlockGeometry)
            {
                ILayoutDrawContext DrawContext = StateView.ControllerView.DrawContext;
                Debug.Assert(DrawContext != null);

                DrawContext.DrawVerticalBlockGeometry(CellOrigin, ActualCellSize.Height);
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
        /// Prints the cell.
        /// </summary>
        /// <param name="origin">The origin from where to start printing.</param>
        public virtual void Print(Point origin)
        {
            Debug.Assert(RegionHelper.IsFixed(ActualCellSize));

            foreach (ILayoutCellView CellView in CellViewList)
                CellView.Print(origin);
        }

        /// <summary>
        /// Prints container or separator before an element of a collection.
        /// </summary>
        /// <param name="printContext">The context used to print the cell.</param>
        /// <param name="cellView">The cell to print.</param>
        /// <param name="origin">The location where to start printing.</param>
        /// <param name="size">The printing size, padding included.</param>
        /// <param name="padding">The padding to use when printing.</param>
        public virtual void PrintBeforeItem(ILayoutPrintContext printContext, ILayoutCellView cellView, Point origin, Size size, Padding padding)
        {
            int Index = CellViewList.IndexOf(cellView);
            Debug.Assert(Index >= 0 && Index < CellViewList.Count);

            if (Index > 0)
                printContext.PrintVerticalSeparator(((ILayoutFrameWithVerticalSeparator)Frame).Separator, cellView.CellOrigin, cellView.CellSize.Width);
        }

        /// <summary>
        /// Prints container or separator after an element of a collection.
        /// </summary>
        /// <param name="printContext">The context used to print the cell.</param>
        /// <param name="cellView">The cell to print.</param>
        /// <param name="origin">The location where to start printing.</param>
        /// <param name="size">The printing size, padding included.</param>
        /// <param name="padding">The padding to use when printing.</param>
        public virtual void PrintAfterItem(ILayoutPrintContext printContext, ILayoutCellView cellView, Point origin, Size size, Padding padding)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="LayoutColumn"/> objects.
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
