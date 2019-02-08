namespace EaslyController.Focus
{
    /// <summary>
    /// Frame that can have a property associated to a selector (or children).
    /// </summary>
    public interface IFocusSelectorPropertyFrame : IFocusNodeFrame
    {
        /// <summary>
        /// Returns the frame associated to a property if it can have selectors.
        /// </summary>
        /// <param name="propertyName">Name of the property to look for.</param>
        /// <param name="frame">Frame found upon return. Null if not matching <paramref name="propertyName"/>.</param>
        bool FrameSelectorForProperty(string propertyName, out IFocusNodeFrameWithSelector frame);
    }
}
