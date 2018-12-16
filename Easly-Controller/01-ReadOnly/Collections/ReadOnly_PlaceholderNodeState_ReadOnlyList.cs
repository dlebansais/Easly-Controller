using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Read-only list of IxxxPlaceholderNodeState
    /// </summary>
    public interface IReadOnlyPlaceholderNodeStateReadOnlyList : IReadOnlyList<IReadOnlyPlaceholderNodeState>
    {
        bool Contains(IReadOnlyPlaceholderNodeState value);
        int IndexOf(IReadOnlyPlaceholderNodeState value);
    }

    /// <summary>
    /// Read-only list of IxxxPlaceholderNodeState
    /// </summary>
    public class ReadOnlyPlaceholderNodeStateReadOnlyList : ReadOnlyCollection<IReadOnlyPlaceholderNodeState>, IReadOnlyPlaceholderNodeStateReadOnlyList
    {
        public ReadOnlyPlaceholderNodeStateReadOnlyList(IReadOnlyPlaceholderNodeStateList list)
            : base(list)
        {
        }
    }
}
