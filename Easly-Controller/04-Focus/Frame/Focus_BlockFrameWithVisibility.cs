namespace EaslyController.Focus
{
    /// <summary>
    /// Block Frame that can have a custom visibility.
    /// </summary>
    public interface IFocusBlockFrameWithVisibility : IFocusBlockFrame
    {
        /// <summary>
        /// Block frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        IFocusBlockFrameVisibility BlockVisibility { get; }
    }
}
