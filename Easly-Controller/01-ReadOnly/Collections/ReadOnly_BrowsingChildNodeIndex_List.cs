using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingChildNodeIndexList : IList<IReadOnlyBrowsingChildNodeIndex>, IReadOnlyList<IReadOnlyBrowsingChildNodeIndex>
    {
        new int Count { get; }
        new IReadOnlyBrowsingChildNodeIndex this[int index] { get; set; }
    }

    public class ReadOnlyBrowsingChildNodeIndexList : Collection<IReadOnlyBrowsingChildNodeIndex>, IReadOnlyBrowsingChildNodeIndexList
    {
    }
}
