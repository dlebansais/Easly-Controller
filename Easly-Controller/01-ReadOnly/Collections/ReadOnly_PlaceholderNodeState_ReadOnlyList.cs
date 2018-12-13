using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyPlaceholderNodeStateReadOnlyList : IReadOnlyList<IReadOnlyPlaceholderNodeState>
    {
        bool Contains(IReadOnlyPlaceholderNodeState value);
        int IndexOf(IReadOnlyPlaceholderNodeState value);
    }

    public class ReadOnlyPlaceholderNodeStateReadOnlyList : ReadOnlyCollection<IReadOnlyPlaceholderNodeState>, IReadOnlyPlaceholderNodeStateReadOnlyList
    {
        public ReadOnlyPlaceholderNodeStateReadOnlyList(IReadOnlyPlaceholderNodeStateList list)
            : base(list)
        {
        }
    }
}
