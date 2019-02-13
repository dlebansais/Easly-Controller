namespace EaslyController.Layout
{
    /// <summary>
    /// Frames that can have a tabulation margin on their left.
    /// </summary>
    public interface ILayoutVerticalTabulatedFrame : ILayoutFrame
    {
        /// <summary>
        /// Indicates that the frame should have a tabulation margin on the left.
        /// (Set in Xaml)
        /// </summary>
        bool HasTabulationMargin { get; }
    }
}
