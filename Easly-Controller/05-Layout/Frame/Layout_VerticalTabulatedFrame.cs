namespace EaslyController.Layout
{
    /// <summary>
    /// Base frame.
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
