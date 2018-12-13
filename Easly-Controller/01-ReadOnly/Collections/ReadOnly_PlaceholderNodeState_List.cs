using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyPlaceholderNodeStateList : IList<IReadOnlyPlaceholderNodeState>, IReadOnlyList<IReadOnlyPlaceholderNodeState>
    {
        new int Count { get; }
        new IReadOnlyPlaceholderNodeState this[int index] { get; set; }
    }

    public class ReadOnlyPlaceholderNodeStateList : Collection<IReadOnlyPlaceholderNodeState>, IReadOnlyPlaceholderNodeStateList
    {
    }
}
