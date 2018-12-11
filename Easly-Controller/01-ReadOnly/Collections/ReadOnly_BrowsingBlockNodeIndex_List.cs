using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingBlockNodeIndexList : IList<IReadOnlyBrowsingBlockNodeIndex>, IReadOnlyList<IReadOnlyBrowsingBlockNodeIndex>
    {
        new int Count { get; }
        new IReadOnlyBrowsingBlockNodeIndex this[int index] { get; set; }
    }

    public class ReadOnlyBrowsingBlockNodeIndexList : Collection<IReadOnlyBrowsingBlockNodeIndex>, IReadOnlyBrowsingBlockNodeIndexList
    {
    }
}
