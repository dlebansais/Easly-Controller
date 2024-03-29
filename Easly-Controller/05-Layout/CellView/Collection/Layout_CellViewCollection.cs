﻿namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Constants;
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
        new LayoutCellViewList CellViewList { get; }

        /// <summary>
        /// The frame that was used to create this cell. Can be null.
        /// </summary>
        new ILayoutFrame Frame { get; }

        /// <summary>
        /// Returns the measured size of streched cells in the collection.
        /// </summary>
        /// <param name="size">The cell size.</param>
        Size GetMeasuredSize(Size size);

        /// <summary>
        /// Draws a selection rectangle around cells.
        /// </summary>
        /// <param name="startIndex">Index of the first cell in the selection.</param>
        /// <param name="endIndex">Index of the last cell in the selection.</param>
        /// <param name="selectionStyle">The style to use when drawing.</param>
        void DrawSelection(int startIndex, int endIndex, SelectionStyles selectionStyle);

        /// <summary>
        /// Draws container or separator before an element of a collection.
        /// </summary>
        /// <param name="drawContext">The context used to draw the cell.</param>
        /// <param name="cellView">The cell to draw.</param>
        /// <param name="origin">The location where to start drawing.</param>
        /// <param name="size">The drawing size, padding included.</param>
        /// <param name="padding">The padding to use when drawing.</param>
        void DrawBeforeItem(ILayoutDrawContext drawContext, ILayoutCellView cellView, Point origin, Size size, Padding padding);

        /// <summary>
        /// Draws container or separator after an element of a collection.
        /// </summary>
        /// <param name="drawContext">The context used to draw the cell.</param>
        /// <param name="cellView">The cell to draw.</param>
        /// <param name="origin">The location where to start drawing.</param>
        /// <param name="size">The drawing size, padding included.</param>
        /// <param name="padding">The padding to use when drawing.</param>
        void DrawAfterItem(ILayoutDrawContext drawContext, ILayoutCellView cellView, Point origin, Size size, Padding padding);

        /// <summary>
        /// Prints container or separator before an element of a collection.
        /// </summary>
        /// <param name="printContext">The context used to print the cell.</param>
        /// <param name="cellView">The cell to print.</param>
        /// <param name="origin">The location where to start printing.</param>
        /// <param name="size">The printing size, padding included.</param>
        /// <param name="padding">The padding to use when printing.</param>
        void PrintBeforeItem(ILayoutPrintContext printContext, ILayoutCellView cellView, Point origin, Size size, Padding padding);

        /// <summary>
        /// Prints container or separator after an element of a collection.
        /// </summary>
        /// <param name="printContext">The context used to print the cell.</param>
        /// <param name="cellView">The cell to print.</param>
        /// <param name="origin">The location where to start printing.</param>
        /// <param name="size">The printing size, padding included.</param>
        /// <param name="padding">The padding to use when printing.</param>
        void PrintAfterItem(ILayoutPrintContext printContext, ILayoutCellView cellView, Point origin, Size size, Padding padding);
    }

    /// <summary>
    /// Base interface for collection of cell views.
    /// </summary>
    internal abstract class LayoutCellViewCollection : FocusCellViewCollection, ILayoutCellViewCollection
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="LayoutCellViewCollection"/> object.
        /// </summary>
        public static new ILayoutCellViewCollection Empty { get; } = new LayoutEmptyCellViewCollection(LayoutNodeStateView.Empty, null, new LayoutCellViewList(), LayoutFrame.LayoutRoot);

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutCellViewCollection"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        /// <param name="cellViewList">The list of child cell views.</param>
        /// <param name="frame">The frame that was used to create this cell. Can be null.</param>
        public LayoutCellViewCollection(ILayoutNodeStateView stateView, ILayoutCellViewCollection parentCellView, LayoutCellViewList cellViewList, ILayoutFrame frame)
            : base(stateView, parentCellView, cellViewList, frame)
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
        /// The collection of cell views containing this view. Null for the root of the cell tree.
        /// </summary>
        public new ILayoutCellViewCollection ParentCellView { get { return (ILayoutCellViewCollection)base.ParentCellView; } }

        /// <summary>
        /// The collection of child cells.
        /// </summary>
        public new LayoutCellViewList CellViewList { get { return (LayoutCellViewList)base.CellViewList; } }

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
        /// Returns the measured size of streched cells in the collection.
        /// </summary>
        /// <param name="size">The cell size.</param>
        public abstract Size GetMeasuredSize(Size size);

        /// <summary>
        /// Draws the cell.
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// Draws a selection rectangle around cells.
        /// </summary>
        /// <param name="startIndex">Index of the first cell in the selection.</param>
        /// <param name="endIndex">Index of the last cell in the selection.</param>
        /// <param name="selectionStyle">The style to use when drawing.</param>
        public abstract void DrawSelection(int startIndex, int endIndex, SelectionStyles selectionStyle);

        /// <summary>
        /// Draws container or separator before an element of a collection.
        /// </summary>
        /// <param name="drawContext">The context used to draw the cell.</param>
        /// <param name="cellView">The cell to draw.</param>
        /// <param name="origin">The location where to start drawing.</param>
        /// <param name="size">The drawing size, padding included.</param>
        /// <param name="padding">The padding to use when drawing.</param>
        public abstract void DrawBeforeItem(ILayoutDrawContext drawContext, ILayoutCellView cellView, Point origin, Size size, Padding padding);

        /// <summary>
        /// Draws container or separator after an element of a collection.
        /// </summary>
        /// <param name="drawContext">The context used to draw the cell.</param>
        /// <param name="cellView">The cell to draw.</param>
        /// <param name="origin">The location where to start drawing.</param>
        /// <param name="size">The drawing size, padding included.</param>
        /// <param name="padding">The padding to use when drawing.</param>
        public abstract void DrawAfterItem(ILayoutDrawContext drawContext, ILayoutCellView cellView, Point origin, Size size, Padding padding);

        /// <summary>
        /// Prints the cell.
        /// </summary>
        /// <param name="origin">The origin from where to start printing.</param>
        public abstract void Print(Point origin);

        /// <summary>
        /// Prints container or separator before an element of a collection.
        /// </summary>
        /// <param name="printContext">The context used to print the cell.</param>
        /// <param name="cellView">The cell to print.</param>
        /// <param name="origin">The location where to start printing.</param>
        /// <param name="size">The printing size, padding included.</param>
        /// <param name="padding">The padding to use when printing.</param>
        public abstract void PrintBeforeItem(ILayoutPrintContext printContext, ILayoutCellView cellView, Point origin, Size size, Padding padding);

        /// <summary>
        /// Prints container or separator after an element of a collection.
        /// </summary>
        /// <param name="printContext">The context used to print the cell.</param>
        /// <param name="cellView">The cell to print.</param>
        /// <param name="origin">The location where to start printing.</param>
        /// <param name="size">The printing size, padding included.</param>
        /// <param name="padding">The padding to use when printing.</param>
        public abstract void PrintAfterItem(ILayoutPrintContext printContext, ILayoutCellView cellView, Point origin, Size size, Padding padding);
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="LayoutCellViewCollection"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public abstract override bool IsEqual(CompareEqual comparer, IEqualComparable other);
        #endregion
    }
}
