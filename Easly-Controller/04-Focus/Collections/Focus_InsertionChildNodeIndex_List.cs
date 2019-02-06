#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxInsertionChildNodeIndex
    /// </summary>
    public interface IFocusInsertionChildNodeIndexList : IList<IFocusInsertionChildNodeIndex>, IReadOnlyList<IFocusInsertionChildNodeIndex>
    {
        new IFocusInsertionChildNodeIndex this[int index] { get; set; }
        new int Count { get; }
    }

    /// <summary>
    /// List of IxxxInsertionChildNodeIndex
    /// </summary>
    internal class FocusInsertionChildNodeIndexList : Collection<IFocusInsertionChildNodeIndex>, IFocusInsertionChildNodeIndexList
    {
    }
}
