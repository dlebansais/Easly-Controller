namespace EaslyController.Layout
{
    using EaslyController.Constants;

    /// <summary>
    /// Frame that can have a vertical separator.
    /// </summary>
    public interface ILayoutFrameWithVerticalSeparator : ILayoutFrame
    {
        /// <summary>
        /// Vertical separator.
        /// (Set in Xaml)
        /// </summary>
        VerticalSeparators Separator { get; }
    }
}
