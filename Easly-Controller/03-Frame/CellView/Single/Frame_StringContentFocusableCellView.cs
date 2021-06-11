namespace EaslyController.Frame
{
    using System.Diagnostics;

    /// <summary>
    /// Cell view for text components that can receive the focus and be modified (identifiers).
    /// </summary>
    public interface IFrameStringContentFocusableCellView : IFrameContentFocusableCellView, IFrameTextFocusableCellView
    {
    }

    /// <summary>
    /// Cell view for text components that can receive the focus and be modified (identifiers).
    /// </summary>
    internal class FrameStringContentFocusableCellView : FrameContentFocusableCellView, IFrameStringContentFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameStringContentFocusableCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        /// <param name="propertyName">Property corresponding to the component of the node.</param>
        public FrameStringContentFocusableCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameFrame frame, string propertyName)
            : base(stateView, parentCellView, frame, propertyName)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FrameStringContentFocusableCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FrameStringContentFocusableCellView AsTextFocusableCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsTextFocusableCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
