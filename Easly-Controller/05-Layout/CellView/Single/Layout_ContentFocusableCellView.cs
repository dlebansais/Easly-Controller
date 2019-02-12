namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Controller;
    using EaslyController.Focus;

    /// <summary>
    /// Cell view for components that can receive the focus and be modified.
    /// </summary>
    public interface ILayoutContentFocusableCellView : IFocusContentFocusableCellView, ILayoutFocusableCellView
    {
    }

    /// <summary>
    /// Cell view for components that can receive the focus and be modified.
    /// </summary>
    internal abstract class LayoutContentFocusableCellView : FocusContentFocusableCellView, ILayoutContentFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutContentFocusableCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        /// <param name="propertyName">Property corresponding to the component of the node.</param>
        public LayoutContentFocusableCellView(ILayoutNodeStateView stateView, ILayoutFrame frame, string propertyName)
            : base(stateView, frame, propertyName)
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
        /// The frame that created this cell view.
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
        public abstract void Measure();

        /// <summary>
        /// Arranges the cell.
        /// </summary>
        public abstract void Arrange(Point origin);

        /// <summary>
        /// Draws the cell.
        /// </summary>
        public abstract void Draw();
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

            if (!comparer.IsSameType(other, out LayoutContentFocusableCellView AsContentFocusableCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsContentFocusableCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
