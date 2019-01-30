#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

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
    internal class FocusFrameSelectorList : Collection<IFocusFrameSelector>, IFocusFrameSelectorList
    {
    }
}
