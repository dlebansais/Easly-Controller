#pragma warning disable 1591

namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

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
    internal class ReadOnlyNodeStateReadOnlyList : ReadOnlyCollection<IReadOnlyNodeState>, IReadOnlyNodeStateReadOnlyList, IReadOnlyList<IReadOnlyNodeState>
    {
        public ReadOnlyNodeStateReadOnlyList(IReadOnlyNodeStateList list)
            : base(list)
        {
        }
    }
}
