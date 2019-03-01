namespace EaslyController.Focus
{
    /// <summary>
    /// A selection of part of a comment.
    /// </summary>
    public interface IFocusCommentSelection : IFocusTextSelection
    {
    }

    /// <summary>
    /// A selection of part of a comment.
    /// </summary>
    public class FocusCommentSelection : FocusSelection, IFocusCommentSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusCommentSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        /// <param name="start">Index of the first character in the selected text.</param>
        /// <param name="end">Index following the last character in the selected text.</param>
        public FocusCommentSelection(IFocusNodeStateView stateView, int start, int end)
            : base(stateView)
        {
            Start = start;
            End = end;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Index of the first character in the selected text.
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// Index following the last character in the selected text.
        /// </summary>
        public int End { get; }
        #endregion
    }
}
