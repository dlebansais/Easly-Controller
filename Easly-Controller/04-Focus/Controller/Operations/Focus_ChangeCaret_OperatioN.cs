namespace EaslyController.Focus
{
    /// <summary>
    /// Operation details for changing the caret in a string property or comment.
    /// </summary>
    public interface IFocusChangeCaretOperation
    {
        /// <summary>
        /// The old caret position.
        /// </summary>
        int OldCaretPosition { get; }

        /// <summary>
        /// The new caret position.
        /// </summary>
        int NewCaretPosition { get; }

        /// <summary>
        /// True if the caret should be changed before the text, to preserve the caret invariant.
        /// </summary>
        bool ChangeCaretBeforeText { get; }
    }
}
