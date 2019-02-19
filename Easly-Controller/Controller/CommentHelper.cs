namespace EaslyController.Controller
{
    using BaseNode;

    /// <summary>
    /// Helper class dedicated to managing comments.
    /// </summary>
    public static class CommentHelper
    {
        /// <summary>
        /// Gets a comment from a <see cref="IDocument"/>.
        /// </summary>
        /// <param name="documentation">The document with the comment.</param>
        public static string Get(IDocument documentation)
        {
#if DEBUG_COMMENT
            if (!string.IsNullOrEmpty(documentation.Comment))
                return documentation.Comment;
            else
                return null;
#else
            return documentation.Uuid.ToString();
#endif
        }

        /// <summary>
        /// Sets the comment of a <see cref="IDocument"/>.
        /// </summary>
        /// <param name="documentation">The document with the comment.</param>
        /// <param name="text">The comment text.</param>
        public static void Set(IDocument documentation, string text)
        {
            if (!string.IsNullOrEmpty(documentation.Comment))
                documentation.Comment = text;
            else
                documentation.Comment = string.Empty;
        }
    }
}
