namespace EaslyController.Focus
{
    /// <summary>
    /// Frame that can have selectors.
    /// </summary>
    public interface IFocusNodeFrameWithSelector
    {
        /// <summary>
        /// List of optional selectors.
        /// (Set in Xaml)
        /// </summary>
        IFocusFrameSelectorList Selectors { get; }
    }
}
