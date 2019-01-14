using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable 1591

namespace EaslyController.Focus
{
    /// <summary>
    /// List of IxxxFrameSelector
    /// </summary>
    public interface IFocusFrameSelectorList : IList<IFocusFrameSelector>, IReadOnlyList<IFocusFrameSelector>
    {
        new int Count { get; }
        new IFocusFrameSelector this[int index] { get; set; }
    }

    /// <summary>
    /// List of IxxxFrameSelector
    /// </summary>
    public class FocusFrameSelectorList : Collection<IFocusFrameSelector>, IFocusFrameSelectorList
    {
    }
}
