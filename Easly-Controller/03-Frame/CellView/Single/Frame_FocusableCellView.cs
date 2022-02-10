namespace EaslyController.Frame
{
    using System.Diagnostics;

    /// <summary>
    /// Cell view for discrete elements that can receive the focus but are not always the component of a node (insertion points, keywords and other decorations)
    /// </summary>
    public interface IFrameFocusableCellView : IFrameVisibleCellView
    {
    }

    /// <summary>
    /// Cell view for discrete elements that can receive the focus but are not always the component of a node (insertion points, keywords and other decorations)
    /// </summary>
    internal class FrameFocusableCellView : FrameVisibleCellView, IFrameFocusableCellView
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FrameFocusableCellView"/> object.
        /// </summary>
        public static new FrameFocusableCellView Empty { get; } = new FrameFocusableCellView(FrameNodeStateView.Empty, FrameCellViewCollection.Empty, FrameFrame.FrameRoot);

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameFocusableCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        public FrameFocusableCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameFrame frame)
            : base(stateView, parentCellView, frame)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FrameFocusableCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out FrameFocusableCellView AsFocusableCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsFocusableCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
