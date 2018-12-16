using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// List of IxxxBrowsingListNodeIndex
    /// </summary>
    public interface IReadOnlyBrowsingListNodeIndexList : IList<IReadOnlyBrowsingListNodeIndex>, IReadOnlyList<IReadOnlyBrowsingListNodeIndex>
    {
        new int Count { get; }
        new IReadOnlyBrowsingListNodeIndex this[int index] { get; set; }
    }

    /// <summary>
    /// List of IxxxBrowsingListNodeIndex
    /// </summary>
    public class ReadOnlyBrowsingListNodeIndexList : Collection<IReadOnlyBrowsingListNodeIndex>, IReadOnlyBrowsingListNodeIndexList
    {
    }
}
