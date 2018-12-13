using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingChildIndexList : IList<IReadOnlyBrowsingChildIndex>, IReadOnlyList<IReadOnlyBrowsingChildIndex>
    {
        new int Count { get; }
        new IReadOnlyBrowsingChildIndex this[int index] { get; set; }
    }

    public class ReadOnlyBrowsingChildIndexList : Collection<IReadOnlyBrowsingChildIndex>, IReadOnlyBrowsingChildIndexList
    {
    }
}
