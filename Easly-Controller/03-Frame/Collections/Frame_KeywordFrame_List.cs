#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxKeywordFrame
    /// </summary>
    public interface IFrameKeywordFrameList : IList<IFrameKeywordFrame>, IReadOnlyList<IFrameKeywordFrame>
    {
        new int Count { get; }
        new IFrameKeywordFrame this[int index] { get; set; }
        new IEnumerator<IFrameKeywordFrame> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxKeywordFrame
    /// </summary>
    internal class FrameKeywordFrameList : Collection<IFrameKeywordFrame>, IFrameKeywordFrameList
    {
    }
}
