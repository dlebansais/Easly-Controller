namespace EaslyController.Focus
{
    /// <summary>
    /// Frame that can have a custom visibility.
    /// </summary>
    public interface IFocusNodeFrameWithVisibility : IFocusNodeFrame
    {
        /// <summary>
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        IFocusNodeFrameVisibility Visibility { get; }

        /// <summary>
        /// Indicates that this is the preferred frame when restoring the focus.
        /// (Set in Xaml)
        /// </summary>
        bool IsPreferred { get; }
    }
}
