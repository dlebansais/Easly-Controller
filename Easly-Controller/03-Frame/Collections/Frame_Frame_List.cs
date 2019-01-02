using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.Frame
{
    /// <summary>
    /// List of IxxxFrame
    /// </summary>
    public interface IFrameFrameList : IList<IFrameFrame>, IReadOnlyList<IFrameFrame>
    {
        new int Count { get; }
        new IFrameFrame this[int index] { get; set; }
    }

    /// <summary>
    /// List of IxxxFrame
    /// </summary>
    public class FrameFrameList : Collection<IFrameFrame>, IFrameFrameList
    {
    }
}
