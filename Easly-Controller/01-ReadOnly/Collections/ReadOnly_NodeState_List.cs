using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyNodeStateList : IList<IReadOnlyNodeState>, IReadOnlyList<IReadOnlyNodeState>
    {
        new int Count { get; }
        new IReadOnlyNodeState this[int index] { get; set; }
    }

    public class ReadOnlyNodeStateList : Collection<IReadOnlyNodeState>, IReadOnlyNodeStateList
    {
    }
}
