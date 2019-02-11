namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Focus that can have selectors.
    /// </summary>
    public interface ILayoutNodeFrameWithSelector : IFocusNodeFrameWithSelector
    {
        /// <summary>
        /// List of optional selectors.
        /// (Set in Xaml)
        /// </summary>
        new ILayoutFrameSelectorList Selectors { get; }
    }
}
