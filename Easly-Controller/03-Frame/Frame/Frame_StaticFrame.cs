namespace EaslyController.Frame
{
    /// <summary>
    /// Frame for decoration purpose only.
    /// </summary>
    public interface IFrameStaticFrame : IFrameFrame
    {
    }

    /// <summary>
    /// Frame for decoration purpose only.
    /// </summary>
    public class FrameStaticFrame : FrameFrame, IFrameStaticFrame
    {
    }
}
