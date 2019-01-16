namespace EaslyController.Constants
{
    /// <summary>
    /// Modes affecting how text is changed.
    /// </summary>
    public enum CaretModes
    {
        /// <summary>
        /// New characters are inserted at the caret position.
        /// </summary>
        Insertion,

        /// <summary>
        /// New characters override existing characters, if any. Otherwise they are added at the end of the text.
        /// </summary>
        Override
    }
}
