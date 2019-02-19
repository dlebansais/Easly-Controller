namespace EaslyController.Constants
{
    /// <summary>
    /// Modes for displaying comments.
    /// </summary>
    public enum CommentDisplayModes
    {
        /// <summary>
        /// Do not display comments.
        /// </summary>
        None,

        /// <summary>
        /// Display as tooltip over the mouse (cannot be edited).
        /// </summary>
        Tooltip,

        /// <summary>
        /// Display as tooltip over each node (cannot be edited).
        /// </summary>
        PermanentTooltip,

        /// <summary>
        /// Display as text that can be edited when it has the focus, hidden otherwise.
        /// </summary>
        OnFocus,

        /// <summary>
        /// Display as text that can be edited everywhere.
        /// </summary>
        All,
    }
}
