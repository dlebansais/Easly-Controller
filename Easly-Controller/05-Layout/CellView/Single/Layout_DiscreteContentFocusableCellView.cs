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
        /// <param name="frame">The frame that created this cell view.</param>
        /// <param name="propertyName">Property corresponding to the component of the node.</param>
        /// <param name="keywordFocus">The keyword frame that was used to create this cell.</param>
        public LayoutDiscreteContentFocusableCellView(ILayoutNodeStateView stateView, ILayoutFrame frame, string propertyName, ILayoutKeywordFrame keywordFocus)
            : base(stateView, frame, propertyName, keywordFocus)
        {
            CellOrigin = ArrangeHelper.InvalidOrigin;
            CellSize = MeasureHelper.InvalidSize;
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

            if (KeywordFrame.ParentFrame is ILayoutDiscreteFrame AsDiscreteFrame)
                CellSize = AsDiscreteFrame.Measure(DrawContext, this);
            else
                CellSize = KeywordFrame.Measure(DrawContext, this);

            Debug.Assert(MeasureHelper.IsValid(CellSize));
        }

        /// <summary>
        /// Arranges the cell.
        /// </summary>
        public virtual void Arrange(Point origin)
        {
            CellOrigin = origin;
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
    }
}
