using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable 1591

namespace EaslyController.Focus
{
    /// <summary>
    /// List of IxxxInsertionChildIndex
    /// </summary>
    public interface IFocusInsertionChildIndexList : IList<IFocusInsertionChildIndex>, IReadOnlyList<IFocusInsertionChildIndex>
    {
        new int Count { get; }
        new IFocusInsertionChildIndex this[int index] { get; set; }
        new IEnumerator<IFocusInsertionChildIndex> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxInsertionChildIndex
    /// </summary>
    public class FocusInsertionChildIndexList : Collection<IFocusInsertionChildIndex>, IFocusInsertionChildIndexList
    {
    }
}
