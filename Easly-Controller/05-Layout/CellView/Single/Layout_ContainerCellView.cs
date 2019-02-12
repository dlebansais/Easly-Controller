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
        /// The frame that was used to create this cell. Can be null.
        /// </summary>
        ILayoutMeasurableFrame Frame { get; }
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
        /// <param name="frame">The frame that was used to create this cell. Can be null.</param>
        public LayoutContainerCellView(ILayoutNodeStateView stateView, ILayoutCellViewCollection parentCellView, ILayoutNodeStateView childStateView, ILayoutMeasurableFrame frame)
            : base(stateView, parentCellView, childStateView)
        {
            Frame = frame;
            CellOrigin = ArrangeHelper.InvalidOrigin;
            CellSize = MeasureHelper.InvalidSize;
            CellPadding = Padding.Empty;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The collection of cell views containing this view.
        /// </summary>
        public new ILayoutCellViewCollection ParentCellView { get { return (ILayoutCellViewCollection)base.ParentCellView; } }

        /// <summary>
        /// The state view of the state associated to this cell.
        /// </summary>
        public new ILayoutNodeStateView ChildStateView { get { return (ILayoutNodeStateView)base.ChildStateView; } }

        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public new ILayoutNodeStateView StateView { get { return (ILayoutNodeStateView)base.StateView; } }

        /// <summary>
        /// The frame that was used to create this cell. Can be null.
        /// </summary>
        public ILayoutMeasurableFrame Frame { get; }

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
            if (Frame != null)
            {
                Debug.Assert(StateView != null);
                Debug.Assert(StateView.ControllerView != null);

                ILayoutDrawContext DrawContext = StateView.ControllerView.DrawContext;
                Debug.Assert(DrawContext != null);

                Frame.Measure(DrawContext, this, out Size Size, out Padding Padding);
                CellSize = Size;
                CellPadding = Padding;
            }
            else
            {
                Debug.Assert(ChildStateView != null);
                ChildStateView.MeasureCells();

                CellSize = ChildStateView.CellSize;
            }

            Debug.Assert(MeasureHelper.IsValid(CellSize));
        }

        /// <summary>
        /// Arranges the cell.
        /// </summary>
        public virtual void Arrange(Point origin)
        {
            CellOrigin = origin;

            Point OriginWithPadding = new Point(origin.X + CellPadding.Left, origin.Y);

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
