using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Frame for cells within a single node.
    /// </summary>
    public interface IFocusNodeFrame : IFrameNodeFrame
    {
        /// <summary>
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        IFocusNodeFrameVisibility Visibility { get; set; }
    }
}
