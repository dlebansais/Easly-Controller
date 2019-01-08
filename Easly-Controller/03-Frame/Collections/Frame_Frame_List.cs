using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable 1591

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
