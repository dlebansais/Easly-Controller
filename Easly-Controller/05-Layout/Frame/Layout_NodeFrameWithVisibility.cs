namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Frame that can have a custom visibility.
    /// </summary>
    public interface ILayoutNodeFrameWithVisibility : IFocusNodeFrameWithVisibility, ILayoutNodeFrame
    {
        /// <summary>
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        new ILayoutNodeFrameVisibility Visibility { get; set; }
    }
}
