namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Block Frame that can have a custom visibility.
    /// </summary>
    public interface ILayoutBlockFrameWithVisibility : IFocusBlockFrameWithVisibility, ILayoutBlockFrame
    {
        /// <summary>
        /// Block frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        new ILayoutBlockFrameVisibility BlockVisibility { get; }
    }
}
