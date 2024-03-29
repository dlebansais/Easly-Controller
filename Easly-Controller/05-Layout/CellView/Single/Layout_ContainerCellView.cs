﻿namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Controller;
    using EaslyController.Focus;

    /// <summary>
    /// A leaf of the cell view tree for a child state.
    /// </summary>
    public interface ILayoutContainerCellView : IFocusContainerCellView, ILayoutCellView, ILayoutAssignableCellView
    {
        /// <summary>
        /// The collection of cell views containing this view.
        /// </summary>
        new ILayoutCellViewCollection ParentCellView { get; }

        /// <summary>
        /// The state view of the state associated to this cell.
        /// </summary>
        new ILayoutNodeStateView ChildStateView { get; }

        /// <summary>
        /// The frame that was used to create this cell.
        /// </summary>
        new ILayoutFrame Frame { get; }
    }

    /// <summary>
    /// A leaf of the cell view tree for a child state.
    /// </summary>
    internal class LayoutContainerCellView : FocusContainerCellView, ILayoutContainerCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutContainerCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view.</param>
        /// <param name="childStateView">The state view of the state associated to this cell.</param>
        /// <param name="frame">The frame that was used to create this cell.</param>
        public LayoutContainerCellView(ILayoutNodeStateView stateView, ILayoutCellViewCollection parentCellView, ILayoutNodeStateView childStateView, ILayoutFrame frame)
            : base(stateView, parentCellView, childStateView, frame)
        {
            CellOrigin = RegionHelper.InvalidOrigin;
            CellSize = RegionHelper.InvalidSize;
            CellPadding = Padding.Empty;
            ActualCellSize = RegionHelper.InvalidSize;
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
        /// The state view of the state associated to this cell.
        /// </summary>
        public new ILayoutNodeStateView ChildStateView { get { return (ILayoutNodeStateView)base.ChildStateView; } }

        /// <summary>
        /// The frame that was used to create this cell.
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
        public virtual void Measure(ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer, Measure separatorLength)
        {
            CollectionWithSeparator = collectionWithSeparator;
            ReferenceContainer = referenceContainer;
            SeparatorLength = separatorLength;

            Debug.Assert(StateView != null);
            Debug.Assert(StateView.ControllerView != null);

            ILayoutMeasureContext MeasureContext = StateView.ControllerView.MeasureContext;
            Debug.Assert(MeasureContext != null);

            Size MeasuredSize;

            if (Frame is ILayoutMeasurableFrame AsMeasurableFrame)
            {
                AsMeasurableFrame.Measure(MeasureContext, this, collectionWithSeparator, referenceContainer, separatorLength, out Size Size, out Padding Padding);
                MeasuredSize = Size;
                CellPadding = Padding;
            }
            else
            {
                Debug.Assert(ChildStateView != null);
                ChildStateView.MeasureCells(collectionWithSeparator, referenceContainer, separatorLength);

                MeasuredSize = ChildStateView.CellSize;
            }

            CellSize = MeasuredSize;
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

            Point OriginWithPadding = origin.Moved(CellPadding.Left, Controller.Measure.Zero);

            Debug.Assert(ChildStateView != null);
            ChildStateView.ArrangeCells(OriginWithPadding);

            Debug.Assert(RegionHelper.IsValid(CellOrigin));
        }

        /// <summary>
        /// Updates the actual size of the cell.
        /// </summary>
        public virtual void UpdateActualSize()
        {
            Debug.Assert(ChildStateView != null);
            ChildStateView.UpdateActualCellsSize();

            Debug.Assert(RegionHelper.IsValid(ChildStateView.ActualCellSize));
            ActualCellSize = ChildStateView.ActualCellSize;

            Debug.Assert(Size.IsEqual(CellRect.Size, ActualCellSize));
        }

        /// <summary>
        /// Draws the cell.
        /// </summary>
        public virtual void Draw()
        {
            Debug.Assert(RegionHelper.IsValid(ActualCellSize));

            Debug.Assert(ChildStateView != null);
            ChildStateView.DrawCells();
        }

        /// <summary>
        /// Prints the cell.
        /// </summary>
        /// <param name="origin">The origin from where to start printing.</param>
        public virtual void Print(Point origin)
        {
            Debug.Assert(RegionHelper.IsValid(ActualCellSize));

            Debug.Assert(ChildStateView != null);
            ChildStateView.PrintCells(origin);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="LayoutContainerCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out LayoutContainerCellView AsContainerCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsContainerCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
