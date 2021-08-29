namespace EaslyController.Controller
{
    using BaseNode;

    /// <summary>
    /// Helper class dedicated to managing comments.
    /// </summary>
    public static class CommentHelper
    {
        /// <summary>
        /// Gets a comment from a <see cref="Document"/>.
        /// </summary>
        /// <param name="documentation">The document with the comment.</param>
        public static string Get(Document documentation)
        {
#if !DEBUG_COMMENT
            if (!string.IsNullOrEmpty(documentation.Comment))
                return documentation.Comment;
            else
                return null;
#else
            return documentation.Uuid.ToString();
#endif
        }

        /// <summary>
        /// Sets the comment of a <see cref="Document"/>.
        /// </summary>
        /// <param name="documentation">The document with the comment.</param>
        /// <param name="text">The comment text.</param>
        public static void Set(Document documentation, string text)
        {
            if (!string.IsNullOrEmpty(text))
                BaseNodeHelper.NodeTreeHelper.SetCommentText(documentation, text);
            else
                BaseNodeHelper.NodeTreeHelper.SetCommentText(documentation, string.Empty);
        }
    }
}
