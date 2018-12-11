using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingOptionalNodeIndexList : IList<IReadOnlyBrowsingOptionalNodeIndex>, IReadOnlyList<IReadOnlyBrowsingOptionalNodeIndex>
    {
        new int Count { get; }
        new IReadOnlyBrowsingOptionalNodeIndex this[int index] { get; set; }
    }

    public class ReadOnlyBrowsingOptionalNodeIndexList : Collection<IReadOnlyBrowsingOptionalNodeIndex>, IReadOnlyBrowsingOptionalNodeIndexList
    {
    }
}
