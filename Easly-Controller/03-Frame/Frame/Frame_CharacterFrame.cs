namespace EaslyController.Frame
{
    /// <summary>
    /// Frame describing a single-character string value property in a node.
    /// </summary>
    public interface IFrameCharacterFrame : IFrameTextValueFrame
    {
    }

    /// <summary>
    /// Frame describing a single-character string value property in a node.
    /// </summary>
    public class FrameCharacterFrame : FrameTextValueFrame, IFrameCharacterFrame
    {
    }
}
