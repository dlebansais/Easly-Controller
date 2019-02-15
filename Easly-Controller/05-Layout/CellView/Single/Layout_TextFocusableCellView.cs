namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Controller;
    using EaslyController.Focus;

    /// <summary>
    /// Cell view for text components that can receive the focus and be modified (identifiers).
    /// </summary>
    public interface ILayoutTextFocusableCellView : IFocusTextFocusableCellView, ILayoutContentFocusableCellView
    {
    }

    /// <summary>
    /// Cell view for text components that can receive the focus and be modified (identifiers).
    /// </summary>
    internal class LayoutTextFocusableCellView : FocusTextFocusableCellView, ILayoutTextFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutTextFocusableCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        /// <param name="propertyName">Property corresponding to the component of the node.</param>
        public LayoutTextFocusableCellView(ILayoutNodeStateView stateView, ILayoutCellViewCollection parentCellView, ILayoutFrame frame, string propertyName)
            : base(stateView, parentCellView, frame, propertyName)
        {
            Debug.Assert(frame is ILayoutMeasurableFrame);
            Debug.Assert(frame is ILayoutDrawableFrame);

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
        /// The collection of cell views containing this view. Null for the root of the cell tree.
        /// </summary>
        public new ILayoutCellViewCollection ParentCellView { get { return (ILayoutCellViewCollection)base.ParentCellView; } }

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

            ILayoutMeasurableFrame AsMeasurableFrame = Frame as ILayoutMeasurableFrame;
            Debug.Assert(AsMeasurableFrame != null);

            AsMeasurableFrame.Measure(DrawContext, this, collectionWithSeparator, referenceContainer, separatorLength, out Size Size, out Padding Padding);
            CellSize = Size;
            CellPadding = Padding;

            Debug.Assert(MeasureHelper.IsValid(CellSize));
        }

        /// <summary>
        /// Arranges the cell.
        /// </summary>
        /// <param name="origin">The cell location.</param>
        public virtual void Arrange(Point origin)
        {
            CellOrigin = origin;
        }

        /// <summary>
        /// Draws the cell.
        /// </summary>
        public virtual void Draw()
        {
            Debug.Assert(StateView != null);
            Debug.Assert(StateView.ControllerView != null);

            ILayoutDrawContext DrawContext = StateView.ControllerView.DrawContext;
            Debug.Assert(DrawContext != null);

            ILayoutDrawableFrame AsDrawableFrame = Frame as ILayoutDrawableFrame;
            Debug.Assert(AsDrawableFrame != null);

            Size size;
            if (ParentCellView != null)
                size = ParentCellView.GetMeasuredSize(CellSize);
            else
            {
                Debug.Assert(MeasureHelper.IsFixed(CellSize));
                size = CellSize;
            }

            CollectionWithSeparator.DrawBeforeItem(DrawContext, ReferenceContainer, CellOrigin, size, CellPadding);
            AsDrawableFrame.Draw(DrawContext, this, CellOrigin, size, CellPadding);
            CollectionWithSeparator.DrawAfterItem(DrawContext, ReferenceContainer, CellOrigin, size, CellPadding);
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

            if (!comparer.IsSameType(other, out LayoutTextFocusableCellView AsTextFocusableCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsTextFocusableCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
