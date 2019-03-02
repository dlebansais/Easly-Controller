namespace EaslyController.Focus
{
    /// <summary>
    /// A selection of text, either in a string property or comment.
    /// </summary>
    public interface IFocusTextSelection : IFocusSelection
    {
        /// <summary>
        /// Index of the first character in the selected text.
        /// </summary>
        int Start { get; }

        /// <summary>
        /// Index following the last character in the selected text.
        /// </summary>
        int End { get; }

        /// <summary>
        /// Updates the text selection with new start and end values.
        /// </summary>
        /// <param name="start">The new start value.</param>
        /// <param name="end">The new end value.</param>
        void Update(int start, int end);
    }
}
