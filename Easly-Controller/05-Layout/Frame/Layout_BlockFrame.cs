namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Frame for cells within a block.
    /// </summary>
    public interface ILayoutBlockFrame : IFocusBlockFrame, ILayoutFrame
    {
        /// <summary>
        /// Block frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        new ILayoutBlockFrameVisibility BlockVisibility { get; set; }
    }
}
