using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Frame for cells within a single node.
    /// </summary>
    public interface IFocusNodeFrame : IFrameNodeFrame
    {
        /// <summary>
        /// Returns the frame associated to a property if can have selectors.
        /// </summary>
        /// <param name="propertyName">Name of the property to look for.</param>
        /// <param name="frame">Frame found upon return. Null if not matching <paramref name="propertyName"/>.</param>
        bool FrameSelectorForProperty(string propertyName, out IFocusNodeFrameWithSelector frame);
    }
}
