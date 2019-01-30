namespace EaslyController.Focus
{
    using EaslyController.Frame;

    /// <summary>
    /// Frame for cells within a block.
    /// </summary>
    public interface IFocusBlockFrame : IFrameBlockFrame, IFocusFrame
    {
        /// <summary>
        /// Block frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        IFocusBlockFrameVisibility BlockVisibility { get; set; }
    }
}
