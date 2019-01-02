namespace EaslyController.Frame
{
    /// <summary>
    /// Frame describing a value property (or string) in a node.
    /// </summary>
    public interface IFrameValueFrame : IFrameNamedFrame
    {
    }

    /// <summary>
    /// Frame describing a value property (or string) in a node.
    /// </summary>
    public class FrameValueFrame : FrameNamedFrame, IFrameValueFrame
    {
    }
}
