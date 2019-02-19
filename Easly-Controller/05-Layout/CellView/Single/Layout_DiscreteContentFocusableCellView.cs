namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Controller;
    using EaslyController.Focus;

    /// <summary>
    /// Cell view for discrete components that can receive the focus and be modified (enum, bool...)
    /// </summary>
    public interface ILayoutDiscreteContentFocusableCellView : IFocusDiscreteContentFocusableCellView, ILayoutContentFocusableCellView
    {
        /// <summary>
        /// The keyword frame that was used to create this cell.
        /// </summary>
        new ILayoutKeywordFrame KeywordFrame { get; }
    }

    /// <summary>
    /// Cell view for discrete components that can receive the focus and be modified (enum, bool...)
    /// </summary>
    internal class LayoutDiscreteContentFocusableCellView : FocusDiscreteContentFocusableCellView, ILayoutDiscreteContentFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutDiscreteContentFocusableCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        /// <param name="propertyName">Property corresponding to the component of the node.</param>
        /// <param name="keywordFrame">The keyword frame that was used to create this cell.</param>
        public LayoutDiscreteContentFocusableCellView(ILayoutNodeStateView stateView, ILayoutCellViewCollection parentCellView, ILayoutFrame frame, string propertyName, ILayoutKeywordFrame keywordFrame)
            : base(stateView, parentCellView, frame, propertyName, keywordFrame)
        {
            Debug.Assert(keywordFrame.ParentFrame is ILayoutDiscreteFrame);

            CellOrigin = RegionHelper.InvalidOrigin;
            CellSize = RegionHelper.InvalidSize;
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
        /// The keyword frame that was used to create this cell.
        /// </summary>
        public new ILayoutKeywordFrame KeywordFrame { get { return (ILayoutKeywordFrame)base.KeywordFrame; } }

        /// <summary>
        /// Location of the cell.
        /// </summary>
        public Point CellOrigin { get; private set; }

        /// <summary>
        /// Size of the cell.
        /// </summary>
        public Size CellSize { get; private set; }

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

            ILayoutDiscreteFrame AsDiscreteFrame = KeywordFrame.ParentFrame as ILayoutDiscreteFrame;
            Debug.Assert(AsDiscreteFrame != null);

            AsDiscreteFrame.Measure(DrawContext, this, collectionWithSeparator, referenceContainer, separatorLength, out Size Size, out Padding Padding);
            CellSize = Size;
            CellPadding = Padding;

            Debug.Assert(RegionHelper.IsValid(CellSize));
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
        /// <param name="measuredSize">Size that was used to draw the cell upon return.</param>
        public virtual void Draw(out Size measuredSize)
        {
            Debug.Assert(StateView != null);
            Debug.Assert(StateView.ControllerView != null);

            ILayoutDrawContext DrawContext = StateView.ControllerView.DrawContext;
            Debug.Assert(DrawContext != null);

            ILayoutDiscreteFrame AsDiscreteFrame = KeywordFrame.ParentFrame as ILayoutDiscreteFrame;
            Debug.Assert(AsDiscreteFrame != null);

            measuredSize = CellSize;
            if (ParentCellView != null)
                measuredSize = ParentCellView.GetMeasuredSize(CellSize);

            Debug.Assert(RegionHelper.IsFixed(measuredSize));

            CollectionWithSeparator.DrawBeforeItem(DrawContext, ReferenceContainer, CellOrigin, measuredSize, CellPadding);
            AsDiscreteFrame.Draw(DrawContext, this, CellOrigin, measuredSize, CellPadding);
            CollectionWithSeparator.DrawAfterItem(DrawContext, ReferenceContainer, CellOrigin, measuredSize, CellPadding);
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

            if (!comparer.IsSameType(other, out LayoutDiscreteContentFocusableCellView AsMultiDiscreteFocusableCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsMultiDiscreteFocusableCellView))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxDiscreteContentCellFocus object.
        /// </summary>
        protected override IFocusDiscreteContentCellFocus CreateFocus()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutDiscreteContentFocusableCellView));
            return new LayoutDiscreteContentCellFocus(this);
        }
        #endregion
    }
}
