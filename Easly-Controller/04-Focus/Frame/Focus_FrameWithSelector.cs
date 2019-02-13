namespace EaslyController.Focus
{
    /// <summary>
    /// Frame that can have selectors.
    /// </summary>
    public interface IFocusFrameWithSelector
    {
        /// <summary>
        /// List of optional selectors.
        /// (Set in Xaml)
        /// </summary>
        IFocusFrameSelectorList Selectors { get; }
    }
}
