namespace EaslyController.Frame
{
    using System.Diagnostics;

    /// <summary>
    /// Cell view for components that can receive the focus and be modified.
    /// </summary>
    public interface IFrameContentFocusableCellView : IFrameFocusableCellView
    {
        /// <summary>
        /// Property corresponding to the component of the node.
        /// </summary>
        string PropertyName { get; }
    }

    /// <summary>
    /// Cell view for components that can receive the focus and be modified.
    /// </summary>
    internal abstract class FrameContentFocusableCellView : FrameFocusableCellView, IFrameContentFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameContentFocusableCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        /// <param name="propertyName">Property corresponding to the component of the node.</param>
        public FrameContentFocusableCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameFrame frame, string propertyName)
            : base(stateView, parentCellView, frame)
        {
            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Property corresponding to the component of the node.
        /// </summary>
        public string PropertyName { get; private set; }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FrameContentFocusableCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out FrameContentFocusableCellView AsContentFocusableCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsContentFocusableCellView))
                return comparer.Failed();

            if (!comparer.IsSameString(PropertyName, AsContentFocusableCellView.PropertyName))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
