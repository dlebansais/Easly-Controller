namespace EaslyController.Focus
{
    public interface IFocusNodeFrameWithSelector : IFocusNodeFrame
    {
        /// <summary>
        /// List of optional selectors.
        /// (Set in Xaml)
        /// </summary>
        IFocusFrameSelectorList Selectors { get; }
    }
}
