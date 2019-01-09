namespace EaslyController.Frame
{
    /// <summary>
    /// Frame describing a number string value property in a node.
    /// </summary>
    public interface IFrameNumberFrame : IFrameTextValueFrame
    {
    }

    /// <summary>
    /// Frame describing a number string value property in a node.
    /// </summary>
    public class FrameNumberFrame : FrameTextValueFrame, IFrameNumberFrame
    {
    }
}
