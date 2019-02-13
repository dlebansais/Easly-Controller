namespace EaslyController.Frame
{
    using System.Diagnostics;

    /// <summary>
    /// Cell view for discrete components that can receive the focus and be modified (enum, bool...)
    /// </summary>
    public interface IFrameDiscreteContentFocusableCellView : IFrameContentFocusableCellView
    {
        /// <summary>
        /// The keyword frame that was used to create this cell.
        /// </summary>
        IFrameKeywordFrame KeywordFrame { get; }
    }

    /// <summary>
    /// Cell view for discrete components that can receive the focus and be modified (enum, bool...)
    /// </summary>
    internal class FrameDiscreteContentFocusableCellView : FrameContentFocusableCellView, IFrameDiscreteContentFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameDiscreteContentFocusableCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        /// <param name="propertyName">Property corresponding to the component of the node.</param>
        /// <param name="keywordFrame">The keyword frame that was used to create this cell.</param>
        public FrameDiscreteContentFocusableCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameFrame frame, string propertyName, IFrameKeywordFrame keywordFrame)
            : base(stateView, parentCellView, frame, propertyName)
        {
            KeywordFrame = keywordFrame;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The keyword frame that was used to create this cell.
        /// </summary>
        public IFrameKeywordFrame KeywordFrame { get; }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FrameDiscreteContentFocusableCellView AsMultiDiscreteFocusableCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsMultiDiscreteFocusableCellView))
                return comparer.Failed();

            if (!comparer.IsSameString(PropertyName, AsMultiDiscreteFocusableCellView.PropertyName))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
