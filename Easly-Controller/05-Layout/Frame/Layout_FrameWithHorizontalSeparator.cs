namespace EaslyController.Layout
{
    using EaslyController.Constants;

    /// <summary>
    /// Frame that can have an horizontal separator.
    /// </summary>
    public interface ILayoutFrameWithHorizontalSeparator : ILayoutFrame
    {
        /// <summary>
        /// Horizontal separator.
        /// (Set in Xaml)
        /// </summary>
        HorizontalSeparators Separator { get; }
    }
}
