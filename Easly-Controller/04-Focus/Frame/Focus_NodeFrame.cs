namespace EaslyController.Focus
{
    using EaslyController.Frame;

    /// <summary>
    /// Frame for cells within a single node.
    /// </summary>
    public interface IFocusNodeFrame : IFrameNodeFrame, IFocusFrame
    {
        /// <summary>
        /// Gets preferred frames to receive the focus when the source code is changed.
        /// </summary>
        /// <param name="firstPreferredFrame">The first preferred frame found.</param>
        /// <param name="lastPreferredFrame">The last preferred frame found.</param>
        void GetPreferredFrame(ref IFocusNodeFrame firstPreferredFrame, ref IFocusNodeFrame lastPreferredFrame);
    }
}
