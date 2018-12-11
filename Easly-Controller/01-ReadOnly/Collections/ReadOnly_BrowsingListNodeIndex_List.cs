using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingListNodeIndexList : IList<IReadOnlyBrowsingListNodeIndex>, IReadOnlyList<IReadOnlyBrowsingListNodeIndex>
    {
        new int Count { get; }
        new IReadOnlyBrowsingListNodeIndex this[int index] { get; set; }
    }

    public class ReadOnlyBrowsingListNodeIndexList : Collection<IReadOnlyBrowsingListNodeIndex>, IReadOnlyBrowsingListNodeIndexList
    {
    }
}
