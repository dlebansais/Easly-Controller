using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable 1591

namespace EaslyController.Frame
{
    /// <summary>
    /// List of IxxxKeywordFrame
    /// </summary>
    public interface IFrameKeywordFrameList : IList<IFrameKeywordFrame>, IReadOnlyList<IFrameKeywordFrame>
    {
        new int Count { get; }
        new IFrameKeywordFrame this[int index] { get; set; }
    }

    /// <summary>
    /// List of IxxxKeywordFrame
    /// </summary>
    public class FrameKeywordFrameList : Collection<IFrameKeywordFrame>, IFrameKeywordFrameList
    {
    }
}
