namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// A selection of part of a comment.
    /// </summary>
    public interface ILayoutCommentSelection : IFocusCommentSelection, ILayoutTextSelection
    {
    }

    /// <summary>
    /// A selection of part of a comment.
    /// </summary>
    public class LayoutCommentSelection : FocusCommentSelection, ILayoutCommentSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutCommentSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        /// <param name="start">Index of the first character in the selected text.</param>
        /// <param name="end">Index following the last character in the selected text.</param>
        public LayoutCommentSelection(ILayoutNodeStateView stateView, int start, int end)
            : base(stateView, start, end)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view that encompasses the selection.
        /// </summary>
        public new ILayoutNodeStateView StateView { get { return (ILayoutNodeStateView)base.StateView; } }
        #endregion
    }
}
