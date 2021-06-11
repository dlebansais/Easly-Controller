namespace EaslyController.Frame
{
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// Cell view for text components that can receive the focus and be modified (identifiers).
    /// </summary>
    public interface IFrameCommentCellView : IFrameFocusableCellView, IFrameTextFocusableCellView
    {
        /// <summary>
        /// The comment this cell is displaying.
        /// </summary>
        IDocument Documentation { get; }
    }

    /// <summary>
    /// Cell view for text components that can receive the focus and be modified (identifiers).
    /// </summary>
    internal class FrameCommentCellView : FrameFocusableCellView, IFrameCommentCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameCommentCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        /// <param name="documentation">The comment this cell is displaying.</param>
        public FrameCommentCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameFrame frame, IDocument documentation)
            : base(stateView, parentCellView, frame)
        {
            Documentation = documentation;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The comment this cell is displaying.
        /// </summary>
        public IDocument Documentation { get; }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FrameCommentCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FrameCommentCellView AsCommentCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsCommentCellView))
                return comparer.Failed();

            if (!comparer.IsSameReference(Documentation, AsCommentCellView.Documentation))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
