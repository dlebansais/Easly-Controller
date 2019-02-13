namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Frame that can have selectors.
    /// </summary>
    public interface ILayoutFrameWithSelector : IFocusFrameWithSelector
    {
        /// <summary>
        /// List of optional selectors.
        /// (Set in Xaml)
        /// </summary>
        new ILayoutFrameSelectorList Selectors { get; }
    }
}
