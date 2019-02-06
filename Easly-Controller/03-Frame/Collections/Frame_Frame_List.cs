#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxFrame
    /// </summary>
    public interface IFrameFrameList : IList<IFrameFrame>, IReadOnlyList<IFrameFrame>
    {
        new IFrameFrame this[int index] { get; set; }
        new int Count { get; }
    }

    /// <summary>
    /// List of IxxxFrame
    /// </summary>
    internal class FrameFrameList : Collection<IFrameFrame>, IFrameFrameList
    {
    }
}
