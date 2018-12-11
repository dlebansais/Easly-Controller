using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingPlaceholderNodeIndexList : IList<IReadOnlyBrowsingPlaceholderNodeIndex>, IReadOnlyList<IReadOnlyBrowsingPlaceholderNodeIndex>
    {
        new int Count { get; }
        new IReadOnlyBrowsingPlaceholderNodeIndex this[int index] { get; set; }
    }

    public class ReadOnlyBrowsingPlaceholderNodeIndexList : Collection<IReadOnlyBrowsingPlaceholderNodeIndex>, IReadOnlyBrowsingPlaceholderNodeIndexList
    {
    }
}
