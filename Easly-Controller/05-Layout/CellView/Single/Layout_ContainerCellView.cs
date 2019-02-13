namespace EaslyController.Layout
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
            Debug.Assert(StateView != null);
            Debug.Assert(StateView.ControllerView != null);

            ILayoutDrawContext DrawContext = StateView.ControllerView.DrawContext;
            Debug.Assert(DrawContext != null);

            Size MeasuredSize;

            if (Frame is ILayoutMeasurableFrame AsMeasurableFrame)
            {
                AsMeasurableFrame.Measure(DrawContext, this, out Size Size, out Padding Padding);
                MeasuredSize = Size;
                CellPadding = Padding;
            }
            else
            {
                Debug.Assert(ChildStateView != null);
                ChildStateView.MeasureCells();

                MeasuredSize = ChildStateView.CellSize;
            }

            if (Frame is ILayoutVerticalTabulatedFrame AsTabulatedFrame)
                if (AsTabulatedFrame.HasTabulationMargin)
                    MeasuredSize = new Size(MeasuredSize.Width + DrawContext.TabulationWidth, MeasuredSize.Height);

            CellSize = MeasuredSize;

            Debug.Assert(MeasureHelper.IsValid(CellSize));
        }

        /// <summary>
        /// Arranges the cell.
        /// </summary>
        public virtual void Arrange(Point origin)
        {
            CellOrigin = origin;

            Debug.Assert(StateView != null);
            Debug.Assert(StateView.ControllerView != null);

            ILayoutDrawContext DrawContext = StateView.ControllerView.DrawContext;
            Debug.Assert(DrawContext != null);

            double LeftPadding = CellPadding.Left;
            if (Frame is ILayoutVerticalTabulatedFrame AsTabulatedFrame && AsTabulatedFrame.HasTabulationMargin)
                LeftPadding += DrawContext.TabulationWidth;

            Point OriginWithPadding = new Point(origin.X + LeftPadding, origin.Y);

            Debug.Assert(ChildStateView != null);
            ChildStateView.ArrangeCells(OriginWithPadding);

            Debug.Assert(ArrangeHelper.IsValid(CellOrigin));
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

            if (!comparer.IsSameType(other, out LayoutContainerCellView AsContainerCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsContainerCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
