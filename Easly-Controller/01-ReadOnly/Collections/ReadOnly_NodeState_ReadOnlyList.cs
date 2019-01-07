using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable 1591

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Read-only list of IxxxNodeState
    /// </summary>
    public interface IReadOnlyNodeStateReadOnlyList : IReadOnlyList<IReadOnlyNodeState>
    {
        bool Contains(IReadOnlyNodeState value);
        int IndexOf(IReadOnlyNodeState value);
    }

    /// <summary>
    /// Read-only list of IxxxNodeState
    /// </summary>
    public class ReadOnlyNodeStateReadOnlyList : ReadOnlyCollection<IReadOnlyNodeState>, IReadOnlyNodeStateReadOnlyList
    {
        public ReadOnlyNodeStateReadOnlyList(IReadOnlyNodeStateList list)
            : base(list)
        {
        }
    }
}
