namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Focus visibility that depends if the current block is replicated.
    /// </summary>
    public interface ILayoutReplicationFrameVisibility : IFocusReplicationFrameVisibility, ILayoutFrameVisibility, ILayoutBlockFrameVisibility
    {
    }

    /// <summary>
    /// Focus visibility that depends if the current block is replicated.
    /// </summary>
    public class LayoutReplicationFrameVisibility : FocusReplicationFrameVisibility, ILayoutReplicationFrameVisibility
    {
    }
}
