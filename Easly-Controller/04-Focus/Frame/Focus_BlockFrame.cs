using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Frame for cells within a block.
    /// </summary>
    public interface IFocusBlockFrame : IFrameBlockFrame
    {
        /// <summary>
        /// Block frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        IFocusBlockFrameVisibility BlockVisibility { get; set; }
    }
}
