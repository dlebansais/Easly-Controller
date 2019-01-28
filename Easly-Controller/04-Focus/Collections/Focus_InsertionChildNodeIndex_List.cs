using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable 1591

namespace EaslyController.Focus
{
    /// <summary>
    /// List of IxxxInsertionChildNodeIndex
    /// </summary>
    public interface IFocusInsertionChildNodeIndexList : IList<IFocusInsertionChildNodeIndex>, IReadOnlyList<IFocusInsertionChildNodeIndex>
    {
        new int Count { get; }
        new IFocusInsertionChildNodeIndex this[int index] { get; set; }
        new IEnumerator<IFocusInsertionChildNodeIndex> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxInsertionChildNodeIndex
    /// </summary>
    public class FocusInsertionChildNodeIndexList : Collection<IFocusInsertionChildNodeIndex>, IFocusInsertionChildNodeIndexList
    {
    }
}
