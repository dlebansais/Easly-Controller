using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyNodeStateReadOnlyList : IReadOnlyList<IReadOnlyNodeState>
    {
        bool Contains(IReadOnlyNodeState value);
        int IndexOf(IReadOnlyNodeState value);
    }

    public class ReadOnlyNodeStateReadOnlyList : ReadOnlyCollection<IReadOnlyNodeState>, IReadOnlyNodeStateReadOnlyList
    {
        public ReadOnlyNodeStateReadOnlyList(IReadOnlyNodeStateList list)
            : base(list)
        {
        }
    }
}
